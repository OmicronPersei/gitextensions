using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
	[TestFixture]
	public class ConEmuSettingsBashCmdCreatorUnitTests
	{
		private class MockConEmuSettingsBashCmdCreator : ConEmuSettingsBashCmdCreator
		{
			public bool GiveBackShellPath = true;

			protected override string AttemptFindShellPathSelectFirst(string[] ShellBinaryNames)
			{
				if (GiveBackShellPath)
				{
					return "C:\\path\\binary.exe";
				}
				else
				{
					return null;
				}
			}

			public string MockAddParamsToShellpath(ConEmuShell ShellType, string Path)
			{
				return AddParamsToShellPath(ShellType, Path);
			}

			public string[] MockGetShellFileNames(ConEmuShell ShellType)
			{
				return GetShellFileNames(ShellType);
			}
		}

		private MockConEmuSettingsBashCmdCreator mCreator;

		public void TestInit()
		{
			mCreator = new MockConEmuSettingsBashCmdCreator();
		}

		[Test]
		public void TestConEmuSettingsAddParamsToShellPath()
		{
			TestInit();

			Assert.AreEqual("C:\\path\\binary.exe --login -i", mCreator.MockAddParamsToShellpath(ConEmuShell.Bash, "C:\\path\\binary.exe"));
			Assert.AreEqual("C:\\path\\binary.exe ", mCreator.MockAddParamsToShellpath(ConEmuShell.Cmd, "C:\\path\\binary.exe"));
			Assert.AreEqual("C:\\path\\binary.exe ", mCreator.MockAddParamsToShellpath(ConEmuShell.PowerShell, "C:\\path\\binary.exe"));
		}

		[Test]
		public void TestConEmuSettingsGetCorrespondingShellFileNames()
		{
			TestInit();

			CollectionAssert.AreEqual(new string[] { "bash.exe", "sh.exe" }, mCreator.MockGetShellFileNames(ConEmuShell.Bash));
			CollectionAssert.AreEqual(new string[] { "cmd.exe" }, mCreator.MockGetShellFileNames(ConEmuShell.Cmd));
			CollectionAssert.AreEqual(new string[] { "powershell.exe" }, mCreator.MockGetShellFileNames(ConEmuShell.PowerShell));
		}

		[Test]
		public void TestGettingFullShellPathSuccess()
		{
			TestInit();

			string pathWithParams = mCreator.GetShellPathWithParams(ConEmuShell.Bash);
			Assert.AreEqual("C:\\path\\binary.exe --login -i", pathWithParams);

			pathWithParams = mCreator.GetShellPathWithParams(ConEmuShell.Cmd);
			Assert.AreEqual("C:\\path\\binary.exe ", pathWithParams);

			pathWithParams = mCreator.GetShellPathWithParams(ConEmuShell.PowerShell);
			Assert.AreEqual("C:\\path\\binary.exe ", pathWithParams);
		}

		[Test]
		public void TestGetFullShellPathFailure()
		{
			TestInit();
			mCreator.GiveBackShellPath = false;

			string pathWithParams = mCreator.GetShellPathWithParams(ConEmuShell.Bash);

			Assert.AreEqual(null, pathWithParams);
		}
	}

	public class ConEmuSettingsPathFindingUnitTests
	{
		private class TestShellSelection : ConEmuSettingsBashCmdCreator
		{

			protected override bool AttemptFindShellPath(string shell, out string foundPath)
			{
				return ShellFindPathDelegate(shell, out foundPath);
			}

			public delegate bool OverrideShellFindPath(string Shell, out string FoundPath);
			public OverrideShellFindPath ShellFindPathDelegate;

			public string MockAttemptFindShellPathSelectFirst(string[] ShellBinaryNames)
			{
				return AttemptFindShellPathSelectFirst(ShellBinaryNames);
			}
		}

		private TestShellSelection mMock;
		
		public ConEmuSettingsPathFindingUnitTests()
		{
			mMock = new TestShellSelection();
		}


		[Test]
		public void TestConEmuSettingsSelectsFirstAvailableBashPathFirstIsAvail()
		{
			TestShellSelection.OverrideShellFindPath bothAvail = new TestShellSelection.OverrideShellFindPath(
				delegate (string Shell, out string FoundPath)
				{
					if (Shell.Equals("one.exe"))
					{
						FoundPath = "one.exe";
						return true;
					}
					else if (Shell.Equals("two.exe"))
					{
						FoundPath = "two.exe";
						return true;
					}
					else
					{
						FoundPath = null;
						return false;
					}
				});

			mMock.ShellFindPathDelegate = bothAvail;

			Assert.AreEqual("one.exe", mMock.MockAttemptFindShellPathSelectFirst(new string[] { "one.exe", "two.exe" }));
		}

		[Test]
		public void TestConEmuSettingsSelectFirstAvailableBasePathFirstNotAvail()
		{
			TestShellSelection.OverrideShellFindPath secondAvail = new TestShellSelection.OverrideShellFindPath(
				delegate (string Shell, out string FoundPath)
				{
					if (Shell.Equals("one.exe"))
					{
						FoundPath = "";
						return false;
					}
					else if (Shell.Equals("two.exe"))
					{
						FoundPath = "two.exe";
						return true;
					}
					else
					{
						FoundPath = null;
						return false;
					}
				});

			mMock.ShellFindPathDelegate = secondAvail;

			Assert.AreEqual("two.exe", mMock.MockAttemptFindShellPathSelectFirst(new string[] { "one.exe", "two.exe" }));
		}

		[Test]
		public void TestConEmuSettingsSelectFirstAvailableBasePathNoneAvail()
		{
			TestShellSelection.OverrideShellFindPath secondAvail = new TestShellSelection.OverrideShellFindPath(
				delegate (string Shell, out string FoundPath)
				{
					if (Shell.Equals("one.exe"))
					{
						FoundPath = "";
						return false;
					}
					else if (Shell.Equals("two.exe"))
					{
						FoundPath = "";
						return false;
					}
					else
					{
						FoundPath = null;
						return false;
					}
				});

			mMock.ShellFindPathDelegate = secondAvail;

			Assert.AreEqual(null, mMock.MockAttemptFindShellPathSelectFirst(new string[] { "one.exe", "two.exe" }));
		}

		[Test]
		public void TestConEmuSettingsSelectFirstAvailableBaseOnlyOneToChoseFrom()
		{
			TestShellSelection.OverrideShellFindPath secondAvail = new TestShellSelection.OverrideShellFindPath(
				delegate (string Shell, out string FoundPath)
				{
					if (Shell.Equals("one.exe"))
					{
						FoundPath = "one.exe";
						return true;
					}
					else
					{
						FoundPath = null;
						return false;
					}
				});

			mMock.ShellFindPathDelegate = secondAvail;

			Assert.AreEqual("one.exe", mMock.MockAttemptFindShellPathSelectFirst(new string[] { "one.exe" }));
		}

		[Test]
		public void TestConEmuSettingsSelectsFirstAvailableBashPathUnrecognizedBinaryName()
		{
			TestShellSelection.OverrideShellFindPath bothAvail = new TestShellSelection.OverrideShellFindPath(
				delegate (string Shell, out string FoundPath)
				{
					if (Shell.Equals("one.exe"))
					{
						FoundPath = "one.exe";
						return true;
					}
					else if (Shell.Equals("two.exe"))
					{
						FoundPath = "two.exe";
						return true;
					}
					else
					{
						FoundPath = null;
						return false;
					}
				});

			mMock.ShellFindPathDelegate = bothAvail;

			Assert.AreEqual(null, mMock.MockAttemptFindShellPathSelectFirst(new string[] { "unrecognized.exe" }));
		}
	}
}
