﻿using ConEmu.WinForms;
using GitUI.ConEmuSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitExtensionsTest.GitUI.ConEmuDisplaySettings
{
    public class ConEmuStartInfoSettingsInterfaceUnitTests
    {
        #region Test Setup

        private class MockSettingsInterface : ConEmuStartInfoSettingsInterface
        {
            public MockSettingsInterface(ConEmuStartInfoXmlInterface XmlInterface)
                : base(XmlInterface)
            { }

            public string MockGetFormattedValue(long val, IntFormatType format)
            {
                return GetFormattedValue(val, format);
            }

            public string MockGetFormattedValue(bool Value)
            {
                return GetFormattedValue(Value);
            }

            public long TestParseLongInt(string LongVal)
            {
                return ParseLongInt(LongVal);
            }

            public bool TestParseBoolean(string BoolVal)
            {
                return ParseBoolean(BoolVal);
            }

            public string TestGetFormattedBoolean(bool Value)
            {
                return GetFormattedValue(Value);
            }
        }

        private MockSettingsInterface mMock;
        private MockXmlInterface mMockXml;

        private class MockXmlInterface : ConEmuStartInfoXmlInterface
        {
            public MockXmlInterface()
                : base(null)
            { }

            protected override void NavigateToSettingsNode()
            {
                //Do nothing.  We are exercizing the data formatting calls, so writing
                //to an actual XmlDocument object is irrelevant here.
            }

            public string AttributeValueToGet = null;
            public string AttributeValueSet = null;

            public override string GetStringDataAttributeFromName(string AttributeName)
            {
                return AttributeValueToGet;
            }

            public override void SetDataValueForAttribute(string AttributeName, string Value)
            {
                AttributeValueSet = Value;
            }
        }

        public ConEmuStartInfoSettingsInterfaceUnitTests()
        {
            mMockXml = new MockXmlInterface();
            mMock = new MockSettingsInterface(mMockXml);
        }

        #endregion
        #region Data formatting tests

        [Test]
        public void TestFormattingValueAsHex()
        {
            int val = 0xFE;

            string actual = mMock.MockGetFormattedValue(val, ConEmuStartInfoSettingsInterface.IntFormatType.hex);

            Assert.AreEqual("FE", actual);
        }

        [Test]
        public void TestFormattingValueLargerThanHexThrowsException()
        {
            int val = 0x100;

            Assert.Throws<ArgumentOutOfRangeException>(
                delegate ()
                {
                    mMock.MockGetFormattedValue(val, ConEmuStartInfoSettingsInterface.IntFormatType.hex);
                });
        }

        [Test]
        public void TestFormattingValueLargerThanDwordThrowsException()
        {
            long val = 0x0000000F00000000;

            Assert.Throws<ArgumentOutOfRangeException>(
                delegate ()
                {
                    mMock.MockGetFormattedValue(val, ConEmuStartInfoSettingsInterface.IntFormatType.dword);
                });
        }

        [Test]
        public void TestStoringValueAsDword()
        {
            long val = 0xFFFFFFFF;

            string actual = mMock.MockGetFormattedValue(val, ConEmuStartInfoSettingsInterface.IntFormatType.dword);

            Assert.AreEqual("FFFFFFFF", actual);
        }

        [Test]
        public void TestGetLongValueFromHexString()
        {
            string val = "0c";

            Assert.AreEqual(0xc, mMock.TestParseLongInt(val));

            val = "0xF0000000";

            Assert.AreEqual(0xF0000000, mMock.TestParseLongInt(val));
        }

        [Test]
        public void TestGetBooelanFromHexString()
        {
            string boolFalse = "00";

            Assert.AreEqual(false, mMock.TestParseBoolean(boolFalse));

            string boolTrue = "01";

            Assert.AreEqual(true, mMock.TestParseBoolean(boolTrue));
        }

        [Test]
        public void TestGetHexStringOfBoolean()
        {
            Assert.AreEqual("00", mMock.MockGetFormattedValue(false));
            Assert.AreEqual("01", mMock.MockGetFormattedValue(true));
        }

        #endregion
        #region Data setting/getting tests

        [Test]
        public void TestGetBooleanValue()
        {
            mMockXml.AttributeValueToGet = "00";

            Assert.AreEqual(false, mMock.GetBooleanValue(""));

            mMockXml.AttributeValueToGet = "01";

            Assert.AreEqual(true, mMock.GetBooleanValue(""));
        }

        [Test]
        public void TestSettingBooleanValue()
        {
            mMock.SetBooleanValue("", false);

            Assert.AreEqual("00", mMockXml.AttributeValueSet);

            mMock.SetBooleanValue("", true);

            Assert.AreEqual("01", mMockXml.AttributeValueSet);
        }

        [Test]
        public void TestGetLongValue()
        {
            mMockXml.AttributeValueToGet = "0c";
            Assert.AreEqual(0xc, mMock.GetLongValue(""));

            mMockXml.AttributeValueToGet = "FFFFFFFF";
            Assert.AreEqual(0xFFFFFFFF, mMock.GetLongValue(""));
        }

        [Test]
        public void TestSetLongValueFormattedHex()
        {

        }

        #endregion
    }
}
