using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    public enum ConEmuShell
    {
        PowerShell,
        Bash,
        Cmd
    };

    internal interface IShellSettings
    {
        ConEmuShell ShellToLaunch { get; set; }
    }
}
