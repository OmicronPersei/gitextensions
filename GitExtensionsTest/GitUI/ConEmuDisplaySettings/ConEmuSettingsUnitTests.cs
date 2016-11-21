using ConEmu.WinForms;
using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
    public class ConEmuSettingsUnitTests
    {
        private class MockConEmuSettings : ConEmuSettings
        {
            public MockConEmuSettings()
                : base ()
            { }
			
        }

        private MockConEmuSettings mObj;
        private ConEmuStartInfo mStartInfo;

        public ConEmuSettingsUnitTests()
        {
            mStartInfo = new ConEmuStartInfo();

            mObj = new MockConEmuSettings();
            mObj.LoadConEmuStartInfo(mStartInfo);
        }

        [Test]
        public void TestFontSettingsAreNullWhenConEmuStartInfoNotLoaded()
        {
            MockConEmuSettings m = new MockConEmuSettings();

            Assert.IsNull(m.FontSettings);
        }

        [Test]
        public void TestFontSettingsLoadsFromConEmuStartInfoObj()
        {
            IFontSettings f = mObj.FontSettings;

            Assert.AreNotEqual(null, f);

            Assert.AreEqual("Consolas", f.FontName);
            Assert.AreEqual(12, f.FontSize);
            Assert.AreEqual(false, f.Bold);
            Assert.AreEqual(false, f.Italic);
        }

        private class ConEmuLookAtSettings
        {
            protected XmlDocument mSettingsXml;
            protected XmlElement mSettingsElem;

            public ConEmuLookAtSettings(ConEmuStartInfo StartInfo)
            {
                mSettingsXml = StartInfo.BaseConfiguration;

                try
                {
                    var softwareNode = mSettingsXml.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueSoftware}']") as XmlElement;
                    var conEmuNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueConEmu}']") as XmlElement;
                    var vanillaNode = conEmuNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueDotVanilla}']") as XmlElement;

                    mSettingsElem = vanillaNode as XmlElement;
                }
                catch
                {
                    throw new ArgumentException("Could not navigate to the setttings node.");
                }
            }

            /// <summary>
            /// <para>Get the string value of the specified name from the currently loaded</para>
            /// <para>settings file.</para>
            /// </summary>
            /// <param name="AttributeName">Attribute Name value to get data value for.</param>
            /// <returns>Data value exactly as it is stored.</returns>
            public virtual string GetStringDataAttributeFromName(string AttributeName)
            {
                XmlElement targetElem = GetElement(AttributeName);
                if (targetElem != null)
                {
                    return targetElem.GetAttribute("data");
                }
                else
                {
                    throw new ArgumentException($"Could not nagivate to the attribute name {AttributeName}");
                }
            }

            private XmlElement GetElement(string AttributeName)
            {
                return mSettingsElem.SelectSingleNode($"value[@name='{AttributeName}']") as XmlElement;
            }
        }

		[Test]
        public void TestStoringFontValues()
        {
            IFontSettings f = mObj.FontSettings;
            ILoadConEmuStartInfo c = mObj;

			f.FontName = "newName";
            f.Bold = true;
            f.Italic = true;
            f.FontSize = 5;

			//Now let's look at what it actually stored.

			c.SaveSettings();

            ConEmuLookAtSettings peek = new ConEmuLookAtSettings(mStartInfo);

            Assert.AreEqual("newName", peek.GetStringDataAttributeFromName("FontName"));
            Assert.AreEqual("01", peek.GetStringDataAttributeFromName("FontBold"));
            Assert.AreEqual("01", peek.GetStringDataAttributeFromName("FontItalic"));
            Assert.AreEqual("00000005", peek.GetStringDataAttributeFromName("FontSize"));
        }

		private class MockConEmuSettingsFakeBashFound : ConEmuSettings
		{
			private string mFakePath;

			public MockConEmuSettingsFakeBashFound(string FakePath)
				: base()
			{
				mFakePath = FakePath;
			}

			protected override ConEmuShellSettings InstantiateShellSettingsObj()
			{
				return new MockConEmuShellSettings(mFakePath);
			}

			private class MockConEmuShellSettings : ConEmuShellSettings
			{
				private string mFakePath;

				public MockConEmuShellSettings(string FakePath)
					: base()
				{
					mFakePath = FakePath;
				}

				protected override ConsolePathGetter InstantiateConsolePathGetter()
				{
					return new MockConsolePathGetter(mFakePath);
				}

				private class MockConsolePathGetter : ConsolePathGetter
				{
					private string mFakePath;

					public MockConsolePathGetter(string FakePath)
						: base()
					{
						mFakePath = FakePath;
					}

					protected override string AttemptFindShellPathSelectFirst(string[] ShellBinaryNames)
					{
						return mFakePath;
					}
				}
			}
		}

		[Test]
		public void TestSettingShellTypePowershell()
		{
			string fakePath = "C:\\pw.exe";
			ConEmuStartInfo sInfo = new ConEmuStartInfo();

			MockConEmuSettingsFakeBashFound mock = new MockConEmuSettingsFakeBashFound(fakePath);
			mock.LoadConEmuStartInfo(sInfo);
			mock.ShellToLaunch = ConEmuShell.PowerShell;

			mock.SaveSettings();

			Assert.AreEqual("C:\\pw.exe ", sInfo.ConsoleProcessCommandLine);
		}

		[Test]
		public void TestSettingShellTypeBash()
		{
			string fakePath = "C:\\bash.exe";
			ConEmuStartInfo sInfo = new ConEmuStartInfo();

			MockConEmuSettingsFakeBashFound mock = new MockConEmuSettingsFakeBashFound(fakePath);
			mock.LoadConEmuStartInfo(sInfo);
			mock.ShellToLaunch = ConEmuShell.Bash;

			mock.SaveSettings();

			Assert.AreEqual("C:\\bash.exe --login -i", sInfo.ConsoleProcessCommandLine);
		}
	}

    /// <summary>
    /// <para>Class for unit testing the behaviour of the settings object</para>
    /// <para> when a broken <see cref="ConEmuStartInfo"/> object is provided.</para>
    /// </summary>
    public class ConEmuSettingsUnitTestBrokenSettingsObj
    {
        [Test]
        public void TestConstructionThrowsExceptionWhenInvalidXmlDocumentProvided()
        {
            ConEmuStartInfo cesiBroken = new ConEmuStartInfo();
            cesiBroken.BaseConfiguration = new XmlDocument();   //blank XmlDocument, broke as shit!;

            ILoadConEmuStartInfo m = new ConEmuSettings();
            Assert.Throws(typeof(NullReferenceException),
                delegate ()
                {
                    m.LoadConEmuStartInfo(cesiBroken);
                });
        }
    }
}
