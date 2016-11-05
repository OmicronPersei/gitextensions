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
    [TestFixture]
    public class ConEmuSettingsUnitTests
    {
        private class MockConEmuStartInfoDisplaySettings : ConEmuStartInfoDisplaySettings
        {
            private XmlElement mSettingsNode;

            public MockConEmuStartInfoDisplaySettings(ConEmuStartInfo StartInfo)
                : base(StartInfo)
            {
                //ConEmuConstants.XmlElementKey
                var softwareNode = StartInfo.BaseConfiguration.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueSoftware}']") as XmlElement;
                var conEmuNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueConEmu}']") as XmlElement;
                var vanillaNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueDotVanilla}']") as XmlElement;

                if (vanillaNode != null)
                {
                    mSettingsNode = vanillaNode;
                }
            }
        }

        private XmlDocument mBaseSettings;

        private MockConEmuStartInfoDisplaySettings mMock;

        [TestFixtureSetUp]
        public void TestInit()
        {
            ConEmuStartInfo cesi = new ConEmuStartInfo();
            mBaseSettings = cesi.BaseConfiguration;

            mMock = new MockConEmuStartInfoDisplaySettings(cesi);
        }

        [Test]
        public void TestFontSettingsLoadsFromObjectProvidedInConstructor()
        {
            Font fMain = mMock.FontSettings;
            Font fBackup = mMock.

            Assert.AreNotEqual(null, fMain);

            Assert.AreEqual("Consolas", fMain.OriginalFontName);
            Assert.AreEqual(12, fMain.Size);
            Assert.AreEqual(false, fMain.Bold);
            Assert.AreEqual(false, fMain.Italic);

        }
    }
}
