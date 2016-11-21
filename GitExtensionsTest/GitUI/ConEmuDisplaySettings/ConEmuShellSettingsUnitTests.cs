using ConEmu.WinForms;
using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
	public class ConEmuShellSettingsLoadingUnitTests
	{
		private class MockConEmuShellSettings : ConEmuShellSettings
		{
			public ConEmuShell MockParseShellFromStartInfo(ConEmuStartInfo StartInfo)
			{
				return ParseShellFromStartInfo(StartInfo);
			}
		}

		#region Test parsing ConEmuStartInfo object

		[Test]
		public void TestConEmuShellSettingsParsesBashDotExe()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();
			startInfo.ConsoleProcessCommandLine = "C:\\bash.exE";

			MockConEmuShellSettings mock = new MockConEmuShellSettings();
			mock.LoadConEmuStartInfo(startInfo);
			Assert.AreEqual(ConEmuShell.Bash, mock.MockParseShellFromStartInfo(startInfo));
		}

		[Test]
		public void TestConEmuShellSettingsParsesShDotExe()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();
			startInfo.ConsoleProcessCommandLine = "C:\\sh.exE";

			MockConEmuShellSettings mock = new MockConEmuShellSettings();
			mock.LoadConEmuStartInfo(startInfo);
			Assert.AreEqual(ConEmuShell.Bash, mock.MockParseShellFromStartInfo(startInfo));
		}

		[Test]
		public void TestConEmuShellSettingsParsesPowershell()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();
			startInfo.ConsoleProcessCommandLine = "C:\\powershelL.exe";

			MockConEmuShellSettings mock = new MockConEmuShellSettings();
			mock.LoadConEmuStartInfo(startInfo);
			Assert.AreEqual(ConEmuShell.PowerShell, mock.MockParseShellFromStartInfo(startInfo));
		}

		[Test]
		public void TestConEmuShellSettingsParsesCmdDotExe()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();
			startInfo.ConsoleProcessCommandLine = "C:\\cmd.exE";

			MockConEmuShellSettings mock = new MockConEmuShellSettings();
			mock.LoadConEmuStartInfo(startInfo);
			Assert.AreEqual(ConEmuShell.Cmd, mock.MockParseShellFromStartInfo(startInfo));
		}

		#endregion

		
	}

	public class ConEmuShellSettingsSavingUnitTests
	{
		private class MockPathFindingConEmuShellSettings : ConEmuShellSettings
		{
			public bool FindPathSuccess;
			public string PathFound;

			protected override ConsolePathGetter InstantiateConsolePathGetter()
			{
				MockConsolePathGetter mock = new MockConsolePathGetter();
				mock.FindPathSuccess = FindPathSuccess;
				mock.PathFound = PathFound;

				return mock;
			}
		}

		private class MockConsolePathGetter : ConsolePathGetter
		{
			public bool FindPathSuccess = true;
			public string PathFound = string.Empty;

			protected override bool AttemptFindShellPath(string shell, out string foundPath)
			{
				if (FindPathSuccess)
				{
					foundPath = PathFound;
				}
				else
				{
					foundPath = null;
				}

				return FindPathSuccess;
			}

		}
		
		[Test]
		public void TestConEmuShellSettingsSetsBash()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			MockPathFindingConEmuShellSettings shellSettings = new MockPathFindingConEmuShellSettings();
			shellSettings.FindPathSuccess = true;
			shellSettings.PathFound = "C:\\bash.exe";

			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Bash;

			shellSettings.SaveSettings();

			string expectedPathWithParams = "C:\\bash.exe --login -i";

			Assert.AreEqual(expectedPathWithParams, startInfo.ConsoleProcessCommandLine);
		}

		[Test]
		public void TestConEmuShellSettingsSetsPowershell()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			MockPathFindingConEmuShellSettings shellSettings = new MockPathFindingConEmuShellSettings();
			shellSettings.FindPathSuccess = true;
			shellSettings.PathFound = "C:\\pw.exe";

			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.PowerShell;

			shellSettings.SaveSettings();

			string expectedPathWithParams = "C:\\pw.exe ";

			Assert.AreEqual(expectedPathWithParams, startInfo.ConsoleProcessCommandLine);
		}

		[Test]
		public void TestConEmuShellSettingsSetsWindowsCmd()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			MockPathFindingConEmuShellSettings shellSettings = new MockPathFindingConEmuShellSettings();
			shellSettings.FindPathSuccess = true;
			shellSettings.PathFound = "C:\\cmd.exe";

			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Cmd;

			shellSettings.SaveSettings();

			string expectedPathWithParams = "C:\\cmd.exe ";

			Assert.AreEqual(expectedPathWithParams, startInfo.ConsoleProcessCommandLine);
		}

		[Test]
		public void TestConEmuShellSettingsSetsDefaultWhenUnkownValueProvided()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			MockPathFindingConEmuShellSettings shellSettings = new MockPathFindingConEmuShellSettings();
			shellSettings.FindPathSuccess = true;

			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Unknown;

			shellSettings.SaveSettings();

			Assert.AreEqual(ConEmuConstants.DefaultConsoleCommandLine, startInfo.ConsoleProcessCommandLine);
		}

		[Test]
		public void TestConEmuShellSettingsSetsDefaultWhenCantFindPath()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			MockPathFindingConEmuShellSettings shellSettings = new MockPathFindingConEmuShellSettings();
			shellSettings.FindPathSuccess = false;

			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Bash;

			shellSettings.SaveSettings();

			Assert.AreEqual(ConEmuConstants.DefaultConsoleCommandLine, startInfo.ConsoleProcessCommandLine);
		}
	}
}
