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
        public const string PowerGistFolder = @"C:\Users\lagrippe\Documents\PowerGist\";

        public static ObjectModelRoot ObjectModelRoot { get; set; }
    }
}
