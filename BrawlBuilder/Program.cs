using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace BrawlBuilder
{
	static class Program
	{
		private static string DllDir = Path.GetDirectoryName(Application.ExecutablePath) + @"\Resources\";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Load DLLs from a custom directory
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CustomAssemblyPathResolver);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new BrawlBuilder());
		}

		// This allows us to load our DLLs from a folder other than the folder our exe is in. Code based off https://support.microsoft.com/en-gb/kb/837908.
		// I was using the application config method for BrawlBuilder up to v1.2, but I hated that I had to include the BrawlBuilder.exe.config file, since
		// the reason I want to do this in the first place was to keep everything other than the exe itself outside of the base directory.
		private static Assembly CustomAssemblyPathResolver(object sender, ResolveEventArgs args)
		{
			// Get an array of all the assemblies that BrawlBuilder references
			AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

			string assemblyPath = null;
			foreach (AssemblyName assemblyName in referencedAssemblies)
			{
				//Check for the assembly names that have raised the "AssemblyResolve" event.
				if (assemblyName.FullName.Substring(0, assemblyName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
				{
					//Build the path of the assembly from where it has to be loaded.                
					assemblyPath = DllDir + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
					break;
				}

			}

			if (assemblyPath == null)
				return null;

			// Return the loaded assembly.
			return Assembly.LoadFrom(assemblyPath);
		}
	}
}
