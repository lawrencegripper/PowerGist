using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class Config
    {
        public static string PowerGistFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PowerGist");

        public static ObjectModelRoot ObjectModelRoot { get; set; }
    }
}
