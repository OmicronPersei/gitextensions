using GitCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{

	/// <summary>
	/// <para>Class used to obtain the path to the requested shell type,
	/// including necessary arguments.</para>
	/// </summary>
	internal class ConsolePathGetter
	{
		public ConsolePathGetter()
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

		/// <summary>
		/// <para>Query the static lookup function to find the requested 
		/// executable name.  Serves as a wrapper around global/static code.</para>
		/// </summary>
		/// <param name="shell"></param>
		/// <param name="foundPath"></param>
		/// <returns></returns>
		protected virtual bool AttemptFindShellPath(string shell, out string foundPath)
		{
			return PathUtil.TryFindShellPath(shell, out foundPath);
		}

		#region Arguments for shell constants
		/// <summary>
		/// Default arguments for bash.exe
		/// </summary>
		private const string BashShellParams = "--login -i";

		/// <summary>
		/// Default arguments for powershell.exe
		/// </summary>
		private const string PowerShellParams = "";

		/// <summary>
		/// Default arguments for cmd.exe
		/// </summary>
		private const string WindowsCmdParams = "";
		#endregion

		/// <summary>
		/// <para>Adds static arguments to the requested shell.</para>
		/// </summary>
		/// <param name="ShellType"></param>
		/// <param name="ShellPath"></param>
		/// <returns></returns>
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

		/// <summary>
		/// <para>Gets filenames of the requested <see cref="ConEmuShell"/> type, in descending order
		/// of preference.</para>
		/// </summary>
		/// <param name="ShellType"></param>
		/// <returns></returns>
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
					return null;
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

			if (binaryNamesForShellType == null)
			{
				return null;
			}
			else
			{
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
}
