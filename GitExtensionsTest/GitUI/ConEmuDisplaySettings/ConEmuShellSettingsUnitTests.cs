using ConEmu.WinForms;
using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
	public class ConEmuShellSettingsUnitTests
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

		#region Test saving ConEmuStartInfo

		[Test]
		public void TestConEmuShellSettingsSetsBash()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			ConEmuShellSettings shellSettings = new ConEmuShellSettings();
			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Bash;

			shellSettings.SaveSettings();

			Assert.Fail("need to continue implementing this test");

		}

		[Test]
		public void TestConEmuShellSettingsSetsDefaultWhenUnkownValueProvided()
		{
			ConEmuStartInfo startInfo = new ConEmuStartInfo();

			ConEmuShellSettings shellSettings = new ConEmuShellSettings();
			shellSettings.LoadConEmuStartInfo(startInfo);

			shellSettings.ShellToLaunch = ConEmuShell.Unknown;

			shellSettings.SaveSettings();

			Assert.Fail("need to continue implementing this test");
		}

		#endregion
	}
}
