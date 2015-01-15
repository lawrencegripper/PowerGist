using GistsApi;
using GripDev.PowerGist.Addin.ISEInterop;
using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    class GetCurrentFileContent
    {
        private ObjectModelRoot host;
        public GetCurrentFileContent()
        {
            host = Config.ObjectModelRoot;

        }

        public string Invoke(string filename)
        {
            var fileFromEditor = host.CurrentPowerShellTab.Files
                .Where(x => x.DisplayName == filename)
                .FirstOrDefault();

            if (fileFromEditor == null)
            {
                return null;
            }

            return fileFromEditor.Editor.Text;
        }
    }

    
}
