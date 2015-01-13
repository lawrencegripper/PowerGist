using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            newFile.SaveAs(filePath);
        }
    }
}
