using GitCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{

	internal class ConEmuSettingsBashCmdCreator
	{

		public ConEmuSettingsBashCmdCreator()
		{ }

		/// <summary>
		/// <para>Find the path to the first available shell binary, where the first</para>
		/// <para>has the higher priority</para>
		/// </summary>
		/// <param name="ShellBinaryNames"></param>
		/// <returns></returns>
		protected virtual string AttemptFindShellPathSelectFirst(string[] ShellBinaryNames)
		{
			string pathToUse = ShellBinaryNames.Select(
				shell =>
				{
					string foundPath = string.Empty;
					if (AttemptFindShellPath(shell, out foundPath))
					{
						return foundPath;
					}
					else
					{
						return null;
					}
				}).FirstOrDefault(shellPath => shellPath != null);


			return pathToUse;
		}

		protected virtual bool AttemptFindShellPath(string shell, out string foundPath)
		{
			return PathUtil.TryFindShellPath(shell, out foundPath);
		}

		private const string BashShellParams = "--login -i";
		private const string PowerShellParams = "";
		private const string WindowsCmdParams = "";

		protected string AddParamsToShellPath(ConEmuShell ShellType, string ShellPath)
		{
			switch (ShellType)
			{
				case ConEmuShell.Bash:
					return ShellPath + " " + BashShellParams;

				case ConEmuShell.Cmd:
					return ShellPath + " " + WindowsCmdParams;

				case ConEmuShell.PowerShell:
					return ShellPath + " " + PowerShellParams;

				default:
					throw new NotImplementedException("The ConEmu shell type is unknown.  Not able to determine the correct starting parameters for it");
			}
		}

		protected string[] GetShellFileNames(ConEmuShell ShellType)
		{
			switch (ShellType)
			{
				case ConEmuShell.Bash:
					return new string[] { "bash.exe", "sh.exe" };

				case ConEmuShell.Cmd:
					return new string[] { "cmd.exe" };

				case ConEmuShell.PowerShell:
					return new string[] { "powershell.exe" };

				default:
					throw new NotImplementedException("The ConEmu shell type is unknown.  Not able to determine the correct starting parameters for it");
			}
		}

		/// <summary>
		/// <para>Gets the path to the requested shell type with typical startup parameters.</para>
		/// </summary>
		/// <param name="ShellType"></param>
		/// <returns><para>The full path to the shell with default parameters.  Returns null if the requested shell could not be found.</para></returns>
		public string GetShellPathWithParams(ConEmuShell ShellType)
		{
			string[] binaryNamesForShellType = GetShellFileNames(ShellType);

			string pathToShell = AttemptFindShellPathSelectFirst(binaryNamesForShellType);

			if (!pathToShell.IsNullOrEmpty())
			{
				string pathWithParams = AddParamsToShellPath(ShellType, pathToShell);

				return pathWithParams;
			}
			else
			{
				return null;
			}
		}
	}
}
