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
        private class MockConEmuSettings : ConEmuStartInfoDisplaySettings
        {
            public MockConEmuSettings()
                : base ()
            { }

            //public string MockGetDataAttributeFromName(string Attribute)
            //{
            //    return GetStringDataAttributeFromName(Attribute);
            //}
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
            IConEmuStartInfoLoadSave c = mObj;

            f.FontName = "newName";
            f.Bold = true;
            f.Italic = true;
            f.FontSize = 5;

            //Now let's look at what it actually stored.

            ConEmuLookAtSettings peek = new ConEmuLookAtSettings(c.GetStoredValues());

            Assert.AreEqual("newName", peek.GetStringDataAttributeFromName("FontName"));
            Assert.AreEqual("01", peek.GetStringDataAttributeFromName("FontBold"));
            Assert.AreEqual("01", peek.GetStringDataAttributeFromName("FontItalic"));
            Assert.AreEqual("00000005", peek.GetStringDataAttributeFromName("FontSize"));
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
            IConEmuStartInfoLoadSave m = new ConEmuStartInfoDisplaySettings();

            Assert.Throws<ArgumentException>(
                delegate ()
                {
                    m.LoadConEmuStartInfo(cesiBroken);
                });
        }
    }
}
