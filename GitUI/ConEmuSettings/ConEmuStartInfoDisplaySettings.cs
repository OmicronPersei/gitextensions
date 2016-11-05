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
