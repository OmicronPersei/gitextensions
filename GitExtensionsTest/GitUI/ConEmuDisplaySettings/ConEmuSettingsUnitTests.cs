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

            public string MockGetDataAttributeFromName(string Attribute)
            {
                return GetStringDataAttributeFromName(Attribute);
            }
        }

        private MockConEmuSettings mObj;

        public ConEmuSettingsUnitTests()
        {
            ConEmuStartInfo cesi = new ConEmuStartInfo();
            //mBaseSettings = cesi.BaseConfiguration;

            mObj = new MockConEmuSettings();
            mObj.LoadConEmuStartInfo(cesi);
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

        [Test]
        public void TestGettingStringDataAttributeFromLoadedXmlSettingsDoc()
        {
            string actual = mObj.MockGetDataAttributeFromName("FontName");

            Assert.AreEqual("Consolas", actual);
        }

    }

    /// <summary>
    /// <para>Class for unit testing methods within the settings without</para>
    /// <para>without loading a <see cref="ConEmuStartInfo"/> object.</para>
    /// </summary>
    public class ConEmuSettingsUnitTestsNoSettingsLoaded
    {
        private class MockConEmuStartInfoDisplaySettings : ConEmuStartInfoDisplaySettings
        {
            //private XmlElement mSettingsNode;

            public MockConEmuStartInfoDisplaySettings()
                : base()
            {
                //ConEmuConstants.XmlElementKey
                //var softwareNode = StartInfo.BaseConfiguration.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueSoftware}']") as XmlElement;
                //var conEmuNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueConEmu}']") as XmlElement;
                //var vanillaNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueDotVanilla}']") as XmlElement;

                //if (vanillaNode != null)
                //{
                //    mSettingsNode = vanillaNode;
                //}
            }

            public string MockGetFormattedValue(long val, IntFormatType format)
            {
                return GetFormattedValue(val, format);
            }

            public string MockGetDataAttributeFromName(string name)
            {
                return GetStringDataAttributeFromName(name);
            }

            public long TestParseLongFromHexadecimalString(string LongVal)
            {
                return ParseLongFromHexadecimalString(LongVal);
            }

            public bool TestGetBooleanFromString(string BoolVal)
            {
                return GetBooleanFromString(BoolVal);
            }

        }

        private MockConEmuStartInfoDisplaySettings mObj;

        public ConEmuSettingsUnitTestsNoSettingsLoaded()
        {
            mObj = new MockConEmuStartInfoDisplaySettings();
        }

        [Test]
        public void TestStoringValueAsHex()
        {
            int val = 0xFE;

            string actual = mObj.MockGetFormattedValue(val, ConEmuStartInfoDisplaySettings.IntFormatType.hex);

            Assert.AreEqual("FE", actual);
        }

        [Test]
        public void TestStoringValueLargerThanHexThrowsException()
        {
            int val = 0x100;

            Assert.Throws<ArgumentOutOfRangeException>(
                delegate ()
                {
                    mObj.MockGetFormattedValue(val, ConEmuStartInfoDisplaySettings.IntFormatType.hex);
                });
        }

        [Test]
        public void TestStoringValueLargerThanDwordThrowsException()
        {
            long val = 0x0000000F00000000;

            Assert.Throws<ArgumentOutOfRangeException>(
                delegate ()
                {
                    mObj.MockGetFormattedValue(val, ConEmuStartInfoDisplaySettings.IntFormatType.dword);
                });
        }

        [Test]
        public void TestStoringValueAsDword()
        {
            long val = 0xFFFFFFFF;

            string actual = mObj.MockGetFormattedValue(val, ConEmuStartInfoDisplaySettings.IntFormatType.dword);

            Assert.AreEqual("FFFFFFFF", actual);
        }

        [Test]
        public void TestGetLongValueFromHexString()
        {
            string val = "0c";

            Assert.AreEqual(0xc, mObj.TestParseLongFromHexadecimalString(val));

            val = "0xF0000000";

            Assert.AreEqual(0xF0000000, mObj.TestParseLongFromHexadecimalString(val));
        }

        [Test]
        public void TestGetBooelanFromHexString()
        {
            string boolFalse = "00";

            Assert.AreEqual(false, mObj.TestGetBooleanFromString(boolFalse));

            string boolTrue = "01";

            Assert.AreEqual(true, mObj.TestGetBooleanFromString(boolTrue));
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
