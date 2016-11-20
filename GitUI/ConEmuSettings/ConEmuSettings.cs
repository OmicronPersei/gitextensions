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
    internal class ConEmuSettings : IDisplaySettings, ILoadConEmuStartInfo
    {
        #region Field Name Constants

        public const string FontName = "FontName";
        public const string FontSize = "FontSize";
        public const string FontBold = "FontBold";
        public const string FontItalic = "FontItalic";

        #endregion

        private ConEmuStartInfoSettingsInterface mSettings;

        public ConEmuSettings()
        {
            mSettings = new ConEmuStartInfoSettingsInterface();
        }

        #region ILoadConEmuStartInfo interface

        public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
        {
            mSettings.LoadConEmuStartInfo(StartInfo);

            LoadAllSettings();
        }

        public void SaveSettings()
		{
			PackAllSettings();
		}

        #endregion

        #region Private methods

        private void LoadAllSettings()
        {
            GetFontSettings();
        }

        private void PackAllSettings()
        {
            StoreFontSettings();
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

        private void StoreFontSettings()
        {
            mSettings.SetString(FontName, FontSettings.FontName);
            mSettings.SetLongValue(FontSize, Convert.ToInt64(FontSettings.FontSize), ConEmuStartInfoSettingsInterface.IntFormatType.dword);
            mSettings.SetBooleanValue(FontBold, FontSettings.Bold);
            mSettings.SetBooleanValue(FontItalic, FontSettings.Italic);
        }

		#endregion



		#region Shell Settings
		

		#endregion
	}
}
