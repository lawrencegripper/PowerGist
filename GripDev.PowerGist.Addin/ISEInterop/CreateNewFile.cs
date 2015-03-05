using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class CreateNewFile
    {
        private ObjectModelRoot host;
        public CreateNewFile()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }

        public string FilePath { get; private set; }
        public string DirPath { get; private set; }


        public ISEFile Invoke(string name, string id, string content)
        {
			DirPath = GetDirPath(id);
			var expectedFilePath = Path.Combine(DirPath, name);

			ISEFile iseFile = null;
			if (File.Exists(expectedFilePath))
			{
				FilePath = expectedFilePath;
				iseFile = OpenExistingFileInISE();
			}
			else
			{
				iseFile = CreateNewFileInISE();
			}

			//if (FileSaveHelper.NoLocalchanges(content, expectedFilePath) || FileSaveHelper.OverwriteLocalChanges(expectedFilePath))
			//{
				
			//}


            FilePath = FileSaveHelper.SaveFile(DirPath, name, id, content, iseFile);

            return iseFile;
        }

        public static string GetDirPath(string id)
        {
            return System.IO.Path.Combine(Config.PowerGistFolder, id);
        }

        private string GetFilePath(string name, string directoryPath)
        {
            return System.IO.Path.Combine(directoryPath, name);
        }

        private ISEFile CreateNewFileInISE()
        {
			var newFile = host.CurrentPowerShellTab.Files.Add();
            host.CurrentPowerShellTab.Files.SelectedFile = newFile;
            return newFile;
        }

		private ISEFile OpenExistingFileInISE()
		{
			var newFile = host.CurrentPowerShellTab.Files.Add(FilePath);
			host.CurrentPowerShellTab.Files.SelectedFile = newFile;
			return newFile;
		}
	}
}
