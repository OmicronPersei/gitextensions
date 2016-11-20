using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConEmu.WinForms;

namespace GitUI.ConEmuSettings
{
	internal class ConEmuShellSettings : IShellSettings, ILoadConEmuStartInfo
	{
		#region Private members

		private ConEmuStartInfo mStartInfo;

		#endregion
		#region Constructor

		public ConEmuShellSettings()
		{ }

		#endregion
		#region IShellSettings interface

		public ConEmuShell ShellToLaunch { get; set; }

		#endregion	
		#region ILoadConEmuStartInfo interface

		public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
		{
			mStartInfo = StartInfo;

			ShellToLaunch = ParseShellFromStartInfo(StartInfo);
		}

		public void SaveSettings()
		{
			ConsolePathGetter pathGetter = InstantiateConsolePathGetter();

			string pathWithArgs = pathGetter.GetShellPathWithParams(ShellToLaunch);

			mStartInfo.ConsoleProcessCommandLine = pathWithArgs;
		}

		#endregion
		#region Private methods

		private const string BashFilename = "bash.exe";
		private const string PowershellFilename = "powershell.exe";
		private const string ShFilename = "sh.exe";
		private const string WindowsConsoleFilename = "cmd.exe";

		protected ConEmuShell ParseShellFromStartInfo(ConEmuStartInfo StartInfo)
		{
			string startCmdLine = StartInfo.ConsoleProcessCommandLine.ToLower();

			if (startCmdLine.Contains(BashFilename) || startCmdLine.Contains(ShFilename))
			{
				return ConEmuShell.Bash;
			}
			else if (startCmdLine.Contains(PowershellFilename))
			{
				return ConEmuShell.PowerShell;
			}
			else if (startCmdLine.Contains(WindowsConsoleFilename))
			{
				return ConEmuShell.Cmd;
			}
			else
			{
				return ConEmuShell.Unknown;
			}
		}

		protected virtual ConsolePathGetter InstantiateConsolePathGetter()
		{
			return new ConsolePathGetter();
		}

		#endregion
	}
}
