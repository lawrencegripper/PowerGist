using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
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

        public void Invoke(string name, string content)
        {
            var newFile = host.CurrentPowerShellTab.Files.Add();
            host.CurrentPowerShellTab.Files.SelectedFile = newFile;
            newFile.Editor.InsertText(content);
            var filePath = string.Format("{0}\\{1}", Config.PowerGistFolder, name);

            if (System.IO.File.Exists(filePath))
            {
                var result = MessageBox.Show("File already exists, do you want to overwrite? (if not I'll leave it unsaved) Path: " + filePath, "Overwrite?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    newFile.SaveAs(filePath);
                }
            }
        }
    }
}
