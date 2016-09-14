using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using JR.Utils.GUI.Forms;
using System.Collections.Specialized;

namespace BrawlBuilder
{
	partial class BrawlBuilder
	{
		private bool _remove_en;
		private bool _showWit;
		private State _state;

		private static string _basePath = Path.GetDirectoryName(Application.ExecutablePath);

		private enum State
		{
			Analyze,
			Extract,
			Verify,
			DeleteSSE,
			CopyModFiles,
			CopyBanner,
			Patch,
			Build,
			Finish
		}

		// Yes most of these are the same. I realized after I wrote them out it would have probably been faster to just make a few special cases, but w/e
		private static readonly NameValueCollection pacRelMappings = new NameValueCollection()
		{
			{"stgdonkey", "st_donkey"},         /* 75m               */     {"stgkart", "st_kart"},             /* Mario Circuit          */
			{"stgbattlefield", "st_battle"},    /* Battlefield       */     {"stgmariopast", "st_mariopast"},   /* Mushroomy Kingdom      */
			{"stgdxbigblue", "st_dxbigblue"},   /* Big Blue          */     {"stgnewpork", "st_newpork"},       /* New Pork City          */
			{"stgoldin", "st_oldin"},           /* Bridge of Eldin   */     {"stgnorfair", "st_norfair"},       /* Norfair                */
			{"stgdxzebes", "st_dxzebes"},       /* Brinstar          */     {"stgdxonett", "st_dxonett"},       /* Onett                  */
			{"stgemblem", "st_emblem"},         /* Castle Siege      */     {"stgonlinetraining", "st_otrain"}, /* Online Training        */
			{"stgconfigtest", "st_config"},     /* ConfigTest        */     {"stgpictchat", "st_pictchat"},     /* PictoChat              */
			{"stgdxcorneria", "st_dxcorneria"}, /* Corneria          */     {"stgpirates", "st_pirates"},       /* Pirate Ship            */
			{"stgdolpic", "st_dolpic"},         /* Delfino Plaza     */     {"stgdxpstadium", "st_dxpstadium"}, /* Pokémon Stadium        */
			{"stgearth", "st_earth"},           /* Distant Planet    */     {"stgstadium", "st_stadium"},       /* Pokémon Stadium 2      */
			{"stgedit", "st_stageedit"},        /* Edit              */     {"stgfzero", "st_fzero"},           /* Port Town Aero Dive    */
			{"stgfinal", "st_final"},           /* Final Destination */     {"stgdxrcruise", "st_dxrcruise"},   /* Rainbow Cruise         */
			{"stggw", "st_gw"},                 /* Flat Zone 2       */     {"stgjungle", "st_jungle"},         /* Rumble Falls           */
			{"stgorpheon", "st_orpheon"},       /* Frigate Orpheon   */     {"stgmetalgear", "st_metalgear"},   /* Shadow Moses Island    */
			{"stgdxgreens", "st_dxgreens"},     /* Green Greens      */     {"stgpalutena", "st_palutena"},     /* Skyworld               */
			{"stggreenhill", "st_greenhill"},   /* Green Hill Zone   */     {"stgvillage", "st_village"},       /* Smashville             */
			{"stghalberd", "st_halberd"},       /* Halberd           */     {"stgtengan", "st_tengan"},         /* Spear Pillar           */
			{"stgplankton", "st_plankton"},     /* Hanenbow          */     {"stgice", "st_ice"},               /* Summit                 */
			{"stgheal", "st_heal"},             /* Heal              */     {"stgtarget", "st_tbreak"},         /* Target Break           */
			{"stghomerun", "st_homerun"},       /* Homerun           */     {"stgdxshrine", "st_dxshrine"},     /* Temple                 */
			{"stgdxgarden", "st_dxgarden"},     /* Jungle Japes      */     {"stgmadein", "st_madein"},         /* WarioWare Inc.         */
			{"stgmansion", "st_mansion"},       /* Luigi's Mansion   */     {"stgcrayon", "st_crayon"},         /* Yoshi's Island (Brawl) */
			{"stgstarfox", "st_starfox"},       /* Lylat Cruise      */     {"stgdxyorster", "st_dxyorster"},   /* Yoshi's Island (Melee) */
			{"stgfamicom", "st_famicom"},       /* Mario Bros.       */
		};

		private void buildWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Don't remove the _en suffix by default
			_remove_en = false;

			// Set up wit
			_showWit = Environment.GetCommandLineArgs().Contains("--show-wit") || Environment.GetCommandLineArgs().Contains("--show-wit-debug");

			if (!File.Exists(_basePath + @"\Resources\wit\wit.exe"))
				StopWorker("Unable to find wit executable, stopping build...");
			ProcessStartInfo pStartInfo = new ProcessStartInfo(_basePath + @"\Resources\wit\wit.exe");
			pStartInfo.CreateNoWindow = !_showWit;
			pStartInfo.UseShellExecute = _showWit;
			pStartInfo.RedirectStandardOutput = !_showWit;
			pStartInfo.RedirectStandardError = !_showWit;

			if (buildWorker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}

			// Check if wit is already running
			Process[] wits = Process.GetProcessesByName("wit");

			if (wits.Length != 0)
			{
				DialogResult result = MessageBox.Show("One or more instances of wit are already running. This could potentially cause issues with the build. Would you like the program to try and kill all instances of wit before continuing?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					foreach (Process wit in wits)
					{
						wit.Kill();
						wit.WaitForExit(5000); // Wait for up to 5 seconds for wit to be killed
					}

					// Make sure it worked
					wits = Process.GetProcessesByName("wit");

					if (wits.Length != 0)
						StopWorker("Unable to kill all instances of wit, stopping build...");
				}
			}

			if (buildWorker.CancellationPending)
			{
				e.Cancel = true;
				return;
			}

			File.Delete(_basePath + @"\Resources\temp.gct"); // Make sure any leftover gct gets removed

			// Set state to the starting state
			_state = State.Analyze;

			while (_state != State.Finish)
			{
				switch (_state)
				{
					case State.Analyze:
						if (gctFile.Text != "" && !Environment.GetCommandLineArgs().Contains("--no-gct-patch"))
						{
							if (!Analyze())
							{
								e.Cancel = true;
								return;
							}
						}
						_state = State.Extract;
						break;

					case State.Extract:
						if (!Extract(pStartInfo))
						{
							e.Cancel = true;
							return;
						}
						_state = State.Verify;
						break;

					case State.Verify:
						if (!Verify())
						{
							e.Cancel = true;
							return;
						}
						_state = State.DeleteSSE;
						break;

					case State.DeleteSSE:
						if (removeSubspace.Checked)
						{
							if (!DeleteSSE())
							{
								e.Cancel = true;
								return;
							}
						}
						_state = State.CopyModFiles;
						break;

					case State.CopyModFiles:
						if (modFolder.Text != "")
						{
							if (!CopyModFiles())
							{
								e.Cancel = true;
								return;
							}
						}
						_state = State.CopyBanner;
						break;

					case State.CopyBanner:
						if (customBanner.Checked)
						{
							if (!CopyBanner())
							{
								e.Cancel = true;
								return;
							}
						}
						_state = State.Patch;
						break;

					case State.Patch:
						if (gctFile.Text != "" || customID.Checked)
						{
							if (!Patch(pStartInfo))
							{
								e.Cancel = true;
								return;
							}
						}
						_state = State.Build;
						break;

					case State.Build:
						if (!Build(pStartInfo))
						{
							e.Cancel = true;
							return;
						}
						_state = State.Finish;
						break;
				}
			}

			// Clean up
			DeleteBrawlFolder();
		}

		private bool Analyze()
		{
			SetStatus("Analyzing...");

			if (File.Exists(_basePath + @"\Resources\CodePatches.txt"))
			{
				// First we'll read the patches we are going to make
				string[] actions = { "REMOVE", "PATCH", "TO", "IF", "ENDIF", "REMOVE_EN" };
				string action = "";
				bool doActions = true;

				string bytesString = "";
				string patchBytesString = "";

				int removes = 0;
				int successfulRemoves = 0;

				int patches = 0;
				int successfulPatches = 0;

				// Load GCT into memory
				byte[] gctBytes = File.ReadAllBytes(gctFile.Text);

				foreach (string s in File.ReadLines(_basePath + @"\Resources\CodePatches.txt"))
				{
					string line = s.Trim().Substring(0, s.IndexOf('#') < 0 ? s.Length : s.IndexOf('#')); // Trim whitespace, ignore comments

					if (actions.Contains(line))
					{
						// Handle one-liner statements. These don't affect action
						if (line == "ENDIF")
						{
							doActions = true; // 'IF' statement is over, start doing actions again (if they were ever stopped)
							continue;
						}
						else if (line == "REMOVE_EN" && doActions)
						{
							_remove_en = true;
							continue;
						}

						action = line;

						continue;
					}

					if (!doActions)
						continue; // If we aren't doing actions just keep looping until the 'ENDIF'

					// Handle multi-line statements
					switch (action)
					{
						case "IF":
							if (line == "") // Blank line means end of codes block, so we can handle the 'IF' statement now.
							{
								action = "";

								if (bytesString.Length > 0)
								{
									byte[] bytes = Enumerable.Range(0, bytesString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(bytesString.Substring(x, 2), 16)).ToArray();
									if (SearchBytes(gctBytes, bytes) < 1)
										doActions = false; // If code wasn't found, stop doing actions until the 'ENDIF' statement
								}

								bytesString = "";
							}
							else
								bytesString += line.Replace(" ", "");
							break;
						case "REMOVE":
							if (line == "")
							{
								action = "";

								if (bytesString.Length > 0)
								{
									removes++;
									byte[] bytes = Enumerable.Range(0, bytesString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(bytesString.Substring(x, 2), 16)).ToArray();
									int index = SearchBytes(gctBytes, bytes);
									if (index >= 0)
									{
										IEnumerable<byte> before = gctBytes.Take(index);
										IEnumerable<byte> after = gctBytes.Skip(index + bytes.Length);

										gctBytes = before.Concat(after).ToArray();
										successfulRemoves++;
									}

									bytesString = "";
								}
							}
							else
								bytesString += line.Replace(" ", "");
							break;
						case "PATCH":
							if (line == "")
								action = ""; // Patch is done after next 'TO'
							else
								patchBytesString += line.Replace(" ", "");
							break;
						case "TO":
							if (line == "")
							{
								action = "";

								if (bytesString.Length > 0 && patchBytesString.Length > 0)
								{
									patches++;
									byte[] patchBytes = Enumerable.Range(0, patchBytesString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(patchBytesString.Substring(x, 2), 16)).ToArray();
									byte[] bytes = Enumerable.Range(0, bytesString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(bytesString.Substring(x, 2), 16)).ToArray();
									int index = SearchBytes(gctBytes, patchBytes);
									if (index >= 0)
									{
										IEnumerable<byte> before = gctBytes.Take(index);
										IEnumerable<byte> after = gctBytes.Skip(index + patchBytes.Length);

										gctBytes = before.Concat(bytes.Concat(after)).ToArray();

										successfulPatches++;
									}

									bytesString = "";
									patchBytesString = "";
								}
							}
							else
								bytesString += line.Replace(" ", "");
							break;
					}
				}

				if (Environment.GetCommandLineArgs().Contains("--notify-gct-patch"))
					MessageBox.Show("Removed codes: " + successfulRemoves + "/" + removes + "\nPatched Codes: " + successfulPatches + "/" + patches + "\nRemove '_en': " + _remove_en, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				File.WriteAllBytes(_basePath + @"\Resources\temp.gct", gctBytes);
			}
			else
			{
				DialogResult result = MessageBox.Show("CodePatches.txt not found. Do you want to continue without GCT patching? The output ISO may not work without this.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)
					buildWorker.CancelAsync();
			}

			if (buildWorker.CancellationPending)
				return false;

			return true;
		}

		// http://stackoverflow.com/a/26880541/1687909
		static int SearchBytes(byte[] haystack, byte[] needle)
		{
			int len = needle.Length;
			int limit = haystack.Length - len;
			for (int i = 0; i <= limit; i++)
			{
				int k = 0;
				for (; k < len; k++)
					if (needle[k] != haystack[i + k])
						break;
				if (k == len) return i;
			}
			return -1;
		}

		private bool Extract(ProcessStartInfo pStartInfo)
		{
			SetStatus("Extracting...");

			bool skipExtraction = false;

			// Check if there is already an extracted Brawl folder
			if (Directory.Exists("ssbb.d") && brawlIso.Text == "")
			{
				skipExtraction = true;
			}
			else if (Directory.Exists("ssbb.d"))
			{
				DialogResult result = MessageBox.Show("There is already an ssbb.d directory. Would you like to skip the extraction and use these files instead?\n\nNote: If you choose 'No', the current ssbb.d directory will be overwritten.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
					skipExtraction = true;
				else
					DeleteBrawlFolder();

				if (buildWorker.CancellationPending)
					return false;
			}

			if (!skipExtraction)
			{
				// Extract brawl to ssbb.d folder
				pStartInfo.Arguments = "extract \"" + brawlIso.Text + "\" ssbb.d --psel=DATA -1ovv";

				if (!DoWit(pStartInfo))
					return false;
			}

			if (!Directory.Exists("ssbb.d"))
				StopWorker("Extraction failed, stopping build...");

			if (buildWorker.CancellationPending)
			{
				// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
				DeleteBrawlFolder();
				return false;
			}

			return true;
		}

		private bool Verify()
		{
			SetStatus("Verifying...");

			if (File.Exists(_basePath + @"\Resources\BrawlFileList.txt"))
			{
				List<Tuple<string, long>> fileList = new List<Tuple<string, long>>();

				// Parse BrawlFileList.txt
				bool success = true;
				foreach (string s in File.ReadLines(_basePath + @"\Resources\BrawlFileList.txt"))
				{
					// Each line should have 1 space separating the file path from the file size
					string[] parts = s.Split(' ');

					// Try to convert file size string to long 
					long size;
					if (!long.TryParse(parts[1], out size))
					{
						DialogResult result = MessageBox.Show("Error parsing BrawlFileList.txt, do you want to continue without verifying extracted files?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

						if (result == DialogResult.No)
							buildWorker.CancelAsync();

						success = false;
						break;
					}

					// Add to list
					fileList.Add(Tuple.Create(parts[0], size));
				}

				// Verify brawl files
				if (success)
				{
					foreach (Tuple<string, long> file in fileList)
					{
						if (!File.Exists(@"ssbb.d\" + file.Item1) || new FileInfo(@"ssbb.d\" + file.Item1).Length != file.Item2)
						{
							DialogResult result = MessageBox.Show("One or more files are either missing or the wrong size in the extracted Brawl image. Do you still wish to continue?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

							if (result == DialogResult.No)
								buildWorker.CancelAsync();

							break;
						}
					}
				}
			}
			else
			{
				DialogResult result = MessageBox.Show("BrawlFileList.txt not found, do you want to continue without verifying extracted files?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)
					buildWorker.CancelAsync();
			}

			if (buildWorker.CancellationPending)
				return false;

			return true;
		}

		private bool DeleteSSE()
		{
			if (File.Exists(_basePath + @"\Resources\SubspaceEmissaryFiles.txt"))
			{
				SetStatus("Deleting SSE...");

				foreach (string file in File.ReadLines(_basePath + @"\Resources\SubspaceEmissaryFiles.txt"))
				{
					File.Delete(@"ssbb.d\files\" + file);
				}
			}
			else
			{
				SetStatus("Deleting SSE...");
				DialogResult result = MessageBox.Show("SubspaceEmissaryFiles.txt not found. Do you want to continue the build without removing Subspace Emissary?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)
					buildWorker.CancelAsync();
			}

			if (buildWorker.CancellationPending)
				return false;

			return true;
		}

		private bool CopyModFiles()
		{
			if (Directory.Exists(modFolder.Text))
			{
				// If the mod contains stages (actually just alt stages) then we have to do some preparation
				if (Directory.Exists(modFolder.Text + @"\stage\melee"))
				{
					SetStatus("Preparing...");

					// The first thing we want to do is get a list of all alt stages included in the mod. This is to resolve the problem described here: https://github.com/mogzol/BrawlBuilder/issues/4#issuecomment-245806404
					// We will use that list to create duplicates of the original .rel files for each alternate stage prior to copying over any files.
					IEnumerable<string> altStages = Directory.EnumerateFiles(modFolder.Text + @"\stage\melee")
						.Where(f => Regex.IsMatch(f, @"_[A-Z]\.pac$", RegexOptions.IgnoreCase));

					foreach (string stage in altStages)
					{
						Regex altStageMatcher = new Regex(@"melee\" + Path.DirectorySeparatorChar + "(STG.+?)(?:_.+|LV[0-9])?(_[A-Z]).pac$", RegexOptions.IgnoreCase);
						Match match = altStageMatcher.Match(stage);

						string baseName = match.Groups[1].Value;
						string altCode = match.Groups[2].Value;
						string relPath = @"ssbb.d\files\module\" + pacRelMappings[baseName];

						if (File.Exists(relPath + ".rel") && !File.Exists(relPath + altCode + ".rel"))
							File.Copy(relPath + ".rel", relPath + altCode + ".rel");
					}
				}

				SetStatus("Copying...");

				// Get mod files in alphabetical order (makes alt stage checking easy)
				string[] modFilesAbsolute = Directory.GetFiles(modFolder.Text, "*", SearchOption.AllDirectories);
				Array.Sort(modFilesAbsolute);

				_progress = 0;
				_progressMax = modFilesAbsolute.Length;

				blinker.RunWorkerAsync();

				Uri relative = new Uri(modFolder.Text);
				foreach (string absoluteFile in modFilesAbsolute)
				{
					// Convert to relative path for easy copying
					Uri fileUri = new Uri(absoluteFile);
					string relativeFile = relative.MakeRelativeUri(fileUri).ToString();

					// relativeFile will still have base folder at this point, so lets remove that
					// First remove leading slash. I think at this point all slashes will be forward slashes, and it wont start with a slash, but w/e
					if (relativeFile.StartsWith("/") || relativeFile.StartsWith("\\"))
						relativeFile = relativeFile.Substring(1);

					// Now remove base folder
					relativeFile = relativeFile.Substring(relativeFile.IndexOfAny(new char[] { '/', '\\' }));

					// Some files need _en added to the end, check in ssbb.d for that
					bool needs_en = false;
					string relativeFile_en;
					if (relativeFile.Contains('.'))
						relativeFile_en = relativeFile.Substring(0, relativeFile.LastIndexOf('.')) + "_en" + relativeFile.Substring(relativeFile.LastIndexOf('.'), relativeFile.Length - relativeFile.LastIndexOf('.'));
					else
						relativeFile_en = relativeFile + "_en";

					// Check brawl files for match with _en, if found then set needs_en to true
					if (File.Exists(@"ssbb.d\files" + relativeFile_en))
						needs_en = true;

					// Perform copy
					Directory.CreateDirectory(@"ssbb.d\files" + Path.GetDirectoryName(relativeFile)); // Just in case it doesn't already exist

					// If remove_en was set in the CodePatches.txt file, then remove _en suffixes
					if (_remove_en)
					{
						File.Copy(absoluteFile, @"ssbb.d\files" + relativeFile, true);
						File.Delete(@"ssbb.d\files" + relativeFile_en); // Get rid of those pesky space-wasting _en files, we don't need em here
					}
					else
					{
						File.Copy(absoluteFile, needs_en ? @"ssbb.d\files" + relativeFile_en : @"ssbb.d\files" + relativeFile, true);
					}

					_progress++;

					if (buildWorker.CancellationPending)
					{
						blinker.CancelAsync();

						// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
						DeleteBrawlFolder();

						return false;
					}
				}

				// Stop blinker before continuing
				blinker.CancelAsync();
				while (blinker.IsBusy)
					Thread.Sleep(100);

				// Base stage files need to be padded to be the same size as their largest alt stage
				// If there aren't alt stages this will do nothing
				if (!Environment.GetCommandLineArgs().Contains("--no-alt-pad"))
				{
					string[] stages = Directory.GetFiles(@"ssbb.d\files\stage\melee");

					SetStatus("Padding...");
					_progress = 0;
					_progressMax = stages.Length;
					blinker.RunWorkerAsync();

					foreach (string stage in stages)
					{
						if (!Regex.IsMatch(stage, @"_[A-Z]\.pac$", RegexOptions.IgnoreCase)) // If not an alt stage
						{
							string filename = Path.GetFileNameWithoutExtension(stage);

							// Select all the alt stages for this stage
							IEnumerable<string> altStages = stages.Where(s => Regex.IsMatch(s, filename + @"_[A-Z]\.pac$", RegexOptions.IgnoreCase));

							// Determine the largest one (resharper converted my foreach loop. LINQ is cool.)
							long largest = altStages.Select(altStage => new FileInfo(altStage).Length).DefaultIfEmpty(0).Max();

							// If base stage is smaller, add padding to match largest alt stage
							long baseStageSize = new FileInfo(stage).Length;
							if (baseStageSize < largest)
							{
								long padding = largest - baseStageSize;

								using (FileStream stream = new FileStream(stage, FileMode.Append))
								{
									stream.Write(new byte[padding], 0, (int)padding); // Just going to cast to an int since I doubt padding will be > 2GB
								}
							}
						}

						_progress++;

						if (buildWorker.CancellationPending)
						{
							blinker.CancelAsync();

							// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
							DeleteBrawlFolder();

							return false;
						}
					}

					// Stop blinker before continuing
					blinker.CancelAsync();
					while (blinker.IsBusy)
						Thread.Sleep(100);
				}

				// Finally we need to clean up any extra uneeded alternate stage .rel files (since we made copies of the original for every alt stage)
				// The way we will do this is delete any alternate .rel files that are identical to the base one (since the mod will fall back to the base one anyway)
				IEnumerable<string> altRels = Directory.EnumerateFiles(@"ssbb.d\files\module\").Where(f => Regex.IsMatch(f, @"_[A-Z]\.rel$", RegexOptions.IgnoreCase));

				if (altRels.Any())
				{
					SetStatus("Cleaning Up...");
					_progress = 0;
					_progressMax = altRels.Count();
					blinker.RunWorkerAsync();

					foreach (string altRel in altRels)
					{
						string filename = Path.GetFileNameWithoutExtension(altRel);
						string baseRel = @"ssbb.d\files\module\" + filename.Remove(filename.Length - 2) + ".rel"; // Remove the _[A-Z] part to get the base rel name

						FileInfo baseInfo = new FileInfo(baseRel);

						if (baseInfo.Exists)
						{
							FileInfo altInfo = new FileInfo(altRel);

							// First check if files are the same size. If yes, then check if they are identical.
							if (baseInfo.Length == altInfo.Length && File.ReadAllBytes(baseRel).SequenceEqual(File.ReadAllBytes(altRel)))
								File.Delete(altRel);
						}

						_progress++;

						if (buildWorker.CancellationPending)
						{
							blinker.CancelAsync();

							// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
							DeleteBrawlFolder();

							return false;
						}
					}

					// Stop blinker before continuing
					blinker.CancelAsync();
					while (blinker.IsBusy)
						Thread.Sleep(100);
				}
			}
			else
			{
				// Mod folder was selected, but it has been deleted or moved since the initial check
				StopWorker("Mod folder not found, stopping build...");
				return false;
			}

			return true;
		}

		private bool CopyBanner()
		{
			if (File.Exists(banner.Text))
			{
				File.Copy(banner.Text, @"ssbb.d\files\opening.bnr", true);
			}
			else
			{
				DialogResult result = MessageBox.Show("Custom banner file not found, do you want to continue the build without a custom banner?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)
					buildWorker.CancelAsync();
			}

			if (buildWorker.CancellationPending)
			{
				// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
				DeleteBrawlFolder();
				return false;
			}

			return true;
		}

		private bool Patch(ProcessStartInfo pStartInfo)
		{
			string patchArgs = "";
			string gct = gctFile.Text;

			if (File.Exists(_basePath + @"\Resources\temp.gct"))
				gct = _basePath + @"\Resources\temp.gct";

			if (File.Exists(gct))
			{
				// Determine GCT file size
				long gctSize = new FileInfo(gct).Length;

				// Set up offset
				string offset = "80570000";
				foreach (string arg in Environment.GetCommandLineArgs())
					if (arg.StartsWith("--offset="))
						offset = arg.Substring(9);

				patchArgs += " NEW=TEXT,80001800,10C0 LOAD=80001800,\"" + _basePath + "/Resources/patch/codehandler.bin\" XML=\"" + _basePath + "/Resources/patch/PatchCommon.xml\" NEW=DATA," + offset + "," + gctSize.ToString("X") + " LOAD=" + offset + ",\"" + gct + "\"";
			}
			else if (gctFile.Text != "")
			{
				// GCT file was selected, but it has been deleted or moved since the initial check
				StopWorker("GCT file not found, stopping build...");
			}

			if (customID.Checked)
			{
				// Need to patch dol to avoid the Please insert disc screen
				string first = gameID.Text.Substring(0, 4);
				string last = gameID.Text.Substring(4);

				if (first != "RSBE")
				{
					// Convert to hex
					first = BitConverter.ToString(Encoding.ASCII.GetBytes(first)).Replace("-", "");
					patchArgs += " 805A14B0=" + first;
				}

				if (last != "01")
				{
					// Convert to hex
					last = BitConverter.ToString(Encoding.ASCII.GetBytes(last)).Replace("-", "");
					patchArgs += " 805A14B8=" + last;
				}
			}

			if (patchArgs != "")
			{
				// Apply patch
				SetStatus("Patching...");

				pStartInfo.Arguments = "dolpatch ssbb.d/sys/main.dol" + patchArgs;

				if (!DoWit(pStartInfo, true))
					return false;
			}

			if (buildWorker.CancellationPending)
			{
				// Since we have been copying files in, ssbb.d is no longer a clean Brawl. Delete it.
				DeleteBrawlFolder();
				return false;
			}

			return true;
		}

		private bool Build(ProcessStartInfo pStartInfo)
		{
			SetStatus("Building...");

			bool splitOutput = false;
			if (_saveFileName.EndsWith(".wbfs"))
			{
				DialogResult result = MessageBox.Show("You selected a wbfs output. Do you want to split the output into 4GB chunks for use on FAT32 filesystems? (Assuming the output is greater than 4GB)", "Split output?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
					splitOutput = true;
			}

			pStartInfo.Arguments = "copy ssbb.d \"" + _saveFileName + "\" -ovv" + (splitOutput ? "z" : "") + (customID.Checked ? " --id=" + gameID.Text : "") + (cutomTitle.Checked ? " --name \"" + gameTitle.Text + "\"" : "");

			if (!DoWit(pStartInfo, false, true))
				return false;

			return true;
		}

		private bool DoWit(ProcessStartInfo pStartInfo, bool forceNoProgress = false, bool finalize = false)
		{
			// Determine if we want wit to be hidden or not
			if (!_showWit && !forceNoProgress)
			{
				// Set up blinker
				_progress = 0;
				_progressMax = 100;
				if (blinker.IsBusy)
				{
					blinker.CancelAsync();
					while (blinker.IsBusy)
					{
						Thread.Sleep(100);
					}
				}
				blinker.RunWorkerAsync();

				// Create outside of loop for better performance
				Regex r = new Regex(@"(\d+)%");

				using (Process p = Process.Start(pStartInfo))
				{
					using (StreamReader reader = p.StandardOutput)
					{
						while (!p.HasExited)
						{
							reader.DiscardBufferedData();

							string curStatus = reader.ReadLine();

							if (curStatus == null)
							{
								// Wit has probably exited, but we'll try to loop through again after a short sleep.
								Thread.Sleep(100);
								continue;
							}

							Match m = r.Match(curStatus);

							if (m.Groups.Count > 1)
								_progress = int.Parse(m.Groups[1].Value);

							// During build 99% can stay up for a loooong time depending on drive speed, so switch status to Finalizing...
							if (_progress >= 99 && finalize)
							{
								blinker.CancelAsync();
								while (blinker.IsBusy)
									Thread.Sleep(100);

								SetStatus("Finalizing...");
								finalize = false; // No need to keep setting it, once is enough
							}

							if (buildWorker.CancellationPending)
							{
								// Stop blinker
								blinker.CancelAsync();

								// Kill process
								if (!p.HasExited)
								{
									p.Kill();
									p.WaitForExit();
								}

								// Didn't finish working, delete ssbb.d
								DeleteBrawlFolder();

								return false;
							}

							Thread.Sleep(100);
						}
					}

					string error = "";
					using (StreamReader s = p.StandardError)
					{
						error = s.ReadToEnd();
					}

					// Check wit exit code
					if (p.ExitCode != 0)
					{
						StopWorker("Wit closed unexpectedly with exit code " + p.ExitCode + ", stopping build..." + (error.Length > 0 ? "\n\nWit error messages:\n\n" + error : ""), error.Length > 0);
					}
					// If there were errors, but exit code was fine, notify the user, but let them continue
					else if (error.Length > 0)
					{
						DialogResult result = FlexibleMessageBox.Show("Wit didn't exit with an error code, however it did write to the error output with the following:\n\n" + error + "\n\nDo you still want to continue the build?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

						if (result == DialogResult.No)
							buildWorker.CancelAsync();
					}
				}
			}
			else
			{
				// Check if we want to keep the window open after, and handle that if we do
				if (Environment.GetCommandLineArgs().Contains("--show-wit-debug"))
				{
					ProcessStartInfo newStartInfo = new ProcessStartInfo();
					newStartInfo.FileName = "CMD.exe";
					newStartInfo.Arguments = "/C \"\"" + pStartInfo.FileName + "\" " + pStartInfo.Arguments + " & pause & if errorlevel 1 exit -1\"";
					pStartInfo = newStartInfo;
				}

				using (Process p = Process.Start(pStartInfo))
				{
					p.WaitForExit();

					if (p.ExitCode != 0)
					{
						if (Environment.GetCommandLineArgs().Contains("--show-wit-debug"))
						{
							// The exit code won't be accurate, so just say Wit closed
							StopWorker("Wit closed unexpectedly, stopping build...");
						}
						else
						{
							StopWorker("Wit closed unexpectedly with exit code " + p.ExitCode + ", stopping build...");
						}
					}
				}
			}

			// Stop blinker before continuing
			blinker.CancelAsync();
			while (blinker.IsBusy)
				Thread.Sleep(100);

			if (buildWorker.CancellationPending)
			{
				// Didn't finish working, delete ssbb.d
				DeleteBrawlFolder();

				return false;
			}

			return true;
		}

		private void DeleteBrawlFolder()
		{
			if (Directory.Exists("ssbb.d"))
			{
				try
				{
					Directory.Delete("ssbb.d", true);
				}
				catch (Exception e)
				{
					MessageBox.Show("Unable to delete ssbb.d directory. Deletion failed with error:\n\n" + e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void StopWorker(string message, bool flex = false)
		{
			if (flex)
				FlexibleMessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
				MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			buildWorker.CancelAsync();
		}

		private void SetStatus(string status, bool force = false)
		{
			_curStatus = status;

			if (!_dontTouch || force)
				build.Invoke(new Action(() => build.Text = _curStatus));
		}

		private void buildWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// Clean up files
			File.Delete(_basePath + @"\Resources\temp.gct");

			if (_exiting)
				Environment.Exit(1);

			// Stop the blinker if it is running
			blinker.CancelAsync();
			while (blinker.IsBusy)
				Thread.Sleep(100);

			// Re-enable controls
			foreach (Control c in Controls)
			{
				if (c != exit && c != build)
				{
					c.Enabled = (bool)c.Tag;
				}
			}

			// Reset status
			SetStatus((string)build.Tag, true);

			Activate();

			// Show success if builder actually finished
			if (_state == State.Finish)
			{
				if (File.Exists(_saveFileName))
				{
					MessageBox.Show("Build Completed!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("BrawlBuilder did not encounter any errors during the build process, but the output image doesn't seem to exist, which indicates that the build did not complete successfully. Sorry about that.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else if (!e.Cancelled)
			{
				string message = "";
				switch (_state)
				{
					case State.Analyze:
						message = "Build failed due to an unknown error while patching the GCT file.";
						break;
					case State.Verify:
						message = "Build failed due to an unknown error while verifiying the Brawl image.";
						break;
					case State.Extract:
						message = "Build failed due to an unknown error while extracting the Brawl image.";
						break;
					case State.DeleteSSE:
						message = "Build failed due to an unknown error while deleting Subspace Emissary files.";
						break;
					case State.CopyModFiles:
						message = "Build failed due to an unknown error while copying mod files.";
						break;
					case State.CopyBanner:
						message = "Build failed due to an unknown error while copying the custom banner.";
						break;
					case State.Patch:
						message = "Build failed due to an unknown error while applying the GCT file to the Brawl image.";
						break;
					case State.Build:
						message = "Build failed due to an unkonwn error while building the modded Brawl image.";
						break;
					default:
						message = "Build failed due to an unknown error.";
						break;
				}

				MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void blinker_DoWork(object sender, DoWorkEventArgs e)
		{
			string statusBack = _curStatus;

			while (true)
			{
				// 'Sleep' for ~2000ms, but cancel faster
				for (int i = 0; i < 20; i++)
				{
					if (blinker.CancellationPending)
						break;
					Thread.Sleep(100);
				}

				if (blinker.CancellationPending)
					break;

				// 'Sleep' for ~4000ms, but also update & cancel faster
				for (int i = 0; i < 40; i++)
				{
					float percent = (int)((float)_progress / _progressMax * 100);
					SetStatus(percent + "%");
					Thread.Sleep(100);

					if (blinker.CancellationPending)
						break;
				}

				if (blinker.CancellationPending)
					break;

				SetStatus(statusBack);
			}

			SetStatus(statusBack);

			e.Cancel = true;
		}
	}
}
