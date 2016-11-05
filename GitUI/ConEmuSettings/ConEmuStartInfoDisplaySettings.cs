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
        #region Field Name Constants

        public const string FontName = "FontName";
        public const string FontSize = "FontSize";
        public const string FontBold = "FontBold";
        public const string FontItalic = "FontItalic";

        #endregion

        private ConEmuStartInfoSettingsInterface mSettings;

        public ConEmuStartInfoDisplaySettings()
        {
            
        }

        #region Private methods

        private void LoadSettings()
        {
            GetFontSettings();
        }

        #endregion  

        #region DisplaySettings

        public IFontSettings FontSettings { get; set; }

        private class ConEmuFontSettings : IFontSettings
        {
            public bool Bold { get; set; }
            public string FontName { get; set; }
            public int FontSize { get; set; }
            public bool Italic { get; set; }
        }

        private void GetFontSettings()
        {
            IFontSettings fs = new ConEmuFontSettings();

            fs.FontName = mSettings.GetString(FontName);
            fs.FontSize = Convert.ToInt32(mSettings.GetLongValue(FontSize));
            fs.Bold = mSettings.GetBooleanValue(FontBold);
            fs.Italic = mSettings.GetBooleanValue(FontItalic);

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
            ConEmuStartInfoXmlInterface xmlInterface = new ConEmuStartInfoXmlInterface(StartInfo);
            mSettings = new ConEmuStartInfoSettingsInterface(xmlInterface);

            LoadSettings();
        }

        public ConEmuStartInfo GetConEmuStartInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
