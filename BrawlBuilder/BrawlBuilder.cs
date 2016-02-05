using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace BrawlBuilder
{
	public partial class BrawlBuilder : Form
	{
		private bool _exiting = false;
		private bool _dontTouch = false;
		private int _progress = 0;
		private int _progressMax = 0;
		private string _curStatus;
		private string _saveFileName = "";

		public BrawlBuilder()
		{
			InitializeComponent();

			// Save initial button value
			build.Tag = _curStatus = build.Text;
		
		}

		// For dragging the form around
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void exit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			exit.Invalidate();
			Update();

			for (double i = Opacity; i < 1; i += 0.01)
			{
				Opacity = i;
				Thread.Sleep(8);
			}
		}

		private void modFolder_lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("Select the folder where all the brawl files to be replaced are located.\nUsually this is named \"pf\".\n\nFor Project M, this folder can be found inside the \"\\projectm\\\" folder.\n\nFor other mods it will most likely be in the \"\\private\\wii\\app\\RSBE\\\" folder", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void modFolderBrowse_Click(object sender, EventArgs e)
		{
			Ookii.Dialogs.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();

			DialogResult result = folderDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				modFolder.Text = folderDialog.SelectedPath;
			}
		}

		private void brawlIso_lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("Select your unmodded Brawl disk image. This can be an ISO, CISO, WBFS, WBI, WIA, or WDF file\n\nNote: If there is an ssbb.d folder containing all of Brawl's files in the same folder as this application, then this field is unnecessary.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void brawlIsoBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = false;
			fileDialog.CheckFileExists = true;
			fileDialog.Filter = "Wii Disk Images (*.iso; *.ciso; *.wbfs; *.wbi; *.wia; *.wdf)|*.iso; *.ciso; *.wbfs; *.wbi; *.wia; *.wdf|All files (*.*)|*.*";

			DialogResult result = fileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				brawlIso.Text = fileDialog.FileName;
			}
		}

		private void gctFile_lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("Select the GCT file containing the codes for the selected mod. This can usually be found in the \"\\codes\\\" folder and will probably be named  \"RSBE01.gct\"", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void gctFileBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = false;
			fileDialog.CheckFileExists = true;
			fileDialog.Filter = "GCT File (*.gct)|*.gct|All files (*.*)|*.*";

			DialogResult result = fileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				gctFile.Text = fileDialog.FileName;
			}
		}

		private void customID_CheckedChanged(object sender, EventArgs e)
		{
			gameID.Enabled = ((CheckBox) sender).Checked;
		}

		private void customTitle_CheckedChanged(object sender, EventArgs e)
		{
			gameTitle.Enabled = ((CheckBox)sender).Checked;
		}

		private void customBanner_CheckedChanged(object sender, EventArgs e)
		{
			banner.Enabled = ((CheckBox)sender).Checked;
			bannerBrowse.Enabled = ((CheckBox)sender).Checked;
		}

		private void bannerBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = false;
			fileDialog.CheckFileExists = true;
			fileDialog.Filter = "Wii Banner Files (*.bnr)|*.bnr|All files (*.*)|*.*";

			DialogResult result = fileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				banner.Text = fileDialog.FileName;
			}
		}

		private void build_Click(object sender, EventArgs e)
		{
			if (buildWorker.IsBusy)
			{
				// Cancel
				DialogResult cancel = MessageBox.Show("Are you sure you want to cancel the build?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (cancel == DialogResult.Yes)
				{
					_dontTouch = true;
					SetStatus("Cancelling...", true);
					buildWorker.CancelAsync();
				}

				return;
			}
				

			// Verify stuff
			if (!File.Exists(brawlIso.Text) && !Directory.Exists("ssbb.d"))
			{
				MessageBox.Show("Brawl ISO location is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (modFolder.Text == "" && gctFile.Text == "" && !removeSubspace.Checked && !customID.Checked && !cutomTitle.Checked && !customBanner.Checked)
			{
				MessageBox.Show("You haven't specified any changes, building not required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (modFolder.Text != "" && !Directory.Exists(modFolder.Text))
			{
				MessageBox.Show("Mod folder location is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string[] brawlFolders = {"effect", "fighter", "game", "info", "info2", "item", "item_gen", "menu", "menu2", "minigame", "module", "movie", "net", "sound", "stage", "system", "toy"};
			if (!Directory.GetDirectories(modFolder.Text).Any(d => brawlFolders.Contains(new DirectoryInfo(d).Name)))
			{
				DialogResult badfolder = MessageBox.Show("It doesn't look like the mod folder you selected contains any replacement Brawl files, are you sure you selected the right foler? Usually it will be called 'pf'.\n\nDo you still want to attempt the build using the selected folder?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (badfolder == DialogResult.No)
					return;
			}

			if (gctFile.Text != "" && !File.Exists(gctFile.Text))
			{
				MessageBox.Show("GCT file location is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (customID.Checked && !Regex.IsMatch(gameID.Text, "^[a-zA-Z0-9_]{6}$"))
			{
				MessageBox.Show("Game ID must be 6 characters and be made up of some combination of A-Z, 0-9, and underscores. No other characters are allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (customBanner.Checked && !File.Exists(banner.Text))
			{
				MessageBox.Show("Banner file location is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Ask where to save output
			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.OverwritePrompt = true;
			fileDialog.Title = "Select where you wish to save modded Brawl image...";
			fileDialog.Filter = "ISO Image|*.iso|CISO Image|*.ciso|WBFS Image|*.wbfs|WBI Image|*.wbi|WIA Image|*.wia|WDF Image|*.wdf";

			string id = customID.Checked ? gameID.Text : "RSBE01";
			string title = cutomTitle.Checked ? gameTitle.Text : "Super Smash Bros. Brawl";

			fileDialog.FileName = title + " [" + id + "]";

			DialogResult result = fileDialog.ShowDialog();

			if (result != DialogResult.OK)
				return;

			_saveFileName = fileDialog.FileName;

			// Disable form controls
			foreach (Control c in Controls)
			{
				if (c != exit && c != build)
				{
					c.Tag = c.Enabled;
					c.Enabled = false;
				}
			}

			// Start background worker
			buildWorker.RunWorkerAsync();
		}

		private void BrawlBuilder_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (buildWorker.IsBusy)
			{
				// Cancel quitting, if user chooses to quit, then the background worker will do it once it cancels.
				e.Cancel = true;

				DialogResult result = MessageBox.Show("A build is in progress, quitting will stop the current build. Are you sure you want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)
					return;

				_exiting = true;
				_dontTouch = true;
				SetStatus("Quitting...", true);
				buildWorker.CancelAsync();
			}
		}

		private void build_MouseEnter(object sender, EventArgs e)
		{
			if (buildWorker.IsBusy && !buildWorker.CancellationPending)
			{
				_dontTouch = true;
				build.Text = "Cancel?";
			}
		}

		private void build_MouseLeave(object sender, EventArgs e)
		{
			if (!buildWorker.CancellationPending)
			{
				_dontTouch = false;
				build.Text = _curStatus;
			}
		}
	}
}
