using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class SaveAllItems
    {
        private ObjectModelRoot host;
        public SaveAllItems()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }

        public void Invoke()
        {
            foreach (var f in host.CurrentPowerShellTab.Files)
            {
                f.Save();
            }
        }
    }
}
