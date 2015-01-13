using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin.IseIntrop
{
    public class CreateNewFile
    {

        private string content;
        public CreateNewFile(HostObject host, string content)
        {
            this.content = content;
        }

        public void Invoke()
        {
            var newFile = HostObject.CurrentPowerShellTab.Files.Add();
            HostObject.CurrentPowerShellTab.Files.SelectedFile = newFile;

        }
    }
}
