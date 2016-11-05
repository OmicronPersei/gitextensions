using ConEmu.WinForms;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Provides a wrapper and getter around the <see cref="ConEmuStartInfo"/> class to allow
    /// for easy display settings modification.  Display settings modification should use the 
    /// <see cref="IDisplaySettings"/> interface.</para>
    /// </summary>
    internal class ConEmuStartInfoDisplaySettings : IDisplaySettings, IConEmuStartInfoLoadSave
    {
        #region XML Settings Constants

        public const string XmlValueNodeName = "value";
        public const string XmlDataAttributeName = "data";

        public const string XmlFontName = "FontName";
        public const string XmlFontSize = "FontSize";
        public const string XmlFontBold = "FontBold";
        public const string XmlFontItalic = "FontItalic";

        public enum IntFormatType
        {
            hex,
            dword
        };

        #endregion

        protected XmlDocument mSettingsXml;
        protected XmlElement mSettingsElem;

        public ConEmuStartInfoDisplaySettings()
        {
            
        }

        #region Private methods

        private void LoadSettings()
        {
            GetFontSettingsFromXml();
        }

        private void NavigateToSettingsNode()
        {
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

        #endregion  

        #region DisplaySettings

        public IFontSettings FontSettings { get; set; }

        private class ConEmuFontSettings : IFontSettings
        {
            public ConEmuFontSettings()
            { }

            public bool Bold { get; set; }

            public string FontName { get; set; }

            public int FontSize { get; set; }

            public bool Italic { get; set; }
        }

        private void GetFontSettingsFromXml()
        {
            IFontSettings fs = new ConEmuFontSettings();

            fs.FontName = GetStringDataAttributeFromName(XmlFontName);
            fs.FontSize = Convert.ToInt32(ParseLongInt(
                GetStringDataAttributeFromName(XmlFontSize)));
            fs.Bold = ParseBoolean(GetStringDataAttributeFromName(XmlFontBold));
            fs.Italic = ParseBoolean(GetStringDataAttributeFromName(XmlFontItalic));

            FontSettings = fs;
        }

        private void SendFontSettingsToXmlDocument()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Data formatting

        /// <summary>
        /// <para>Provides a wrapper for storing integer values formatted the same</para>
        /// <para>as the base settings template.</para>
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected string GetFormattedValue(long val, IntFormatType format)
        {
            switch (format)
            {
                case IntFormatType.dword:
                    if (val > 0xFFFFFFFF)
                    {
                        throw new ArgumentOutOfRangeException("The number storage was dword but the field does not support values larger than 0xFFFFFFFF");
                    }

                    return string.Format(Convert.ToString(val, 16), "00000000").ToUpper();

                case IntFormatType.hex:
                    if (val > 0xFF)
                    {
                        throw new ArgumentOutOfRangeException("The number storage type was hex but the field does not support values larger than 0xFF");
                    }

                    return string.Format(Convert.ToString(val, 16), "00").ToUpper();

                default:
                    throw new NotImplementedException("Unknown integer storage format for the ConEmu settings XML.");
            }
        }

        protected string GetFormattedValue(bool Value)
        {
            if (!Value)
            {
                return "00";
            }
            else
            {
                return "01";
            }
        }

        protected bool ParseBoolean(string val)
        {
            if (val == "00")
            {
                return false;
            }
            else if (val == "01")
            {
                return true;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Value cannot be correctly parsed as boolean.  Only valid values are 00 and 01");
            }
        }

        protected long ParseLongInt(string Value)
        {
            return Convert.ToInt64(Value, 16);
        }

        /// <summary>
        /// <para>Get the string value of the specified name from the currently loaded</para>
        /// <para>settings file.</para>
        /// </summary>
        /// <param name="Name">Attribute Name value to get data value for.</param>
        /// <returns>Data value exactly as it is stored.</returns>
        protected string GetStringDataAttributeFromName(string Name)
        {
            var targetElem = mSettingsElem.SelectSingleNode($"{XmlValueNodeName}[@{ConEmuConstants.XmlAttrName}='{Name}']") as XmlElement;
            if (targetElem != null)
            {
                return targetElem.GetAttribute(XmlDataAttributeName);
            }
            else
            {
                throw new ArgumentException($"Could not nagivate to the attribute name {Name}");
            }
        }

        #endregion  

        #region IConEmuStartInfoLoadSave interface

        public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
        {
            mSettingsXml = StartInfo.BaseConfiguration;
            NavigateToSettingsNode();

            LoadSettings();
        }

        public ConEmuStartInfo GetConEmuStartInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
