using ConEmu.WinForms;
using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
    public class ConEmuStartInfoXmlInterfaceUnitTests
    {
        private class MockConEmuStartInfoXmlInterface : ConEmuStartInfoXmlInterface
        {
            public MockConEmuStartInfoXmlInterface(ConEmuStartInfo StartInfo)
                : base(StartInfo)
            { }
        }

        private MockConEmuStartInfoXmlInterface mMock;

        public ConEmuStartInfoXmlInterfaceUnitTests()
        {
            ConEmuStartInfo cesi = new ConEmuStartInfo();
            mMock = new MockConEmuStartInfoXmlInterface(cesi);
        }

        [Test]
        public void TestGettingStringFromConEmuStartInfo()
        {
            Assert.AreEqual("Consolas", mMock.GetStringDataAttributeFromName("FontName"));
        }

        [Test]
        public void TestSettingStringToConEmuStartInfo()
        {
            mMock.SetDataValueForAttribute("FontName", "UglyFont");
            Assert.AreEqual("UglyFont", mMock.GetStringDataAttributeFromName("FontFace"));
        }
    }
}
