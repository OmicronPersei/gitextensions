using ConEmu.WinForms;
using System;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Provides a wrapper and getter around the <see cref="ConEmuStartInfo"/> class to allow
    /// for easy, encapsulated settings modification.</para>
	/// <para>Display settings modification should use the <see cref="IDisplaySettings"/> interface.</para>
    /// </summary>
    internal class ConEmuSettings : IDisplaySettings, IShellSettings, ILoadConEmuStartInfo
    {
        #region Field Name Constants

        public const string FontName = "FontName";
        public const string FontSize = "FontSize";
        public const string FontBold = "FontBold";
        public const string FontItalic = "FontItalic";

		#endregion
		#region Private members

		/// <summary>
		/// Allows formattable value get/set of settings values.
		/// </summary>
		private ConEmuSettingValueFormatting mSettings;

		/// <summary>
		/// <para>Provides an interface to changing the shell to be launched.  Encapsulates 
		/// getting the path to the specified shell.</para>
		/// </summary>
		private ConEmuShellSettings mShellSettings;

		#endregion
		#region Constructor

		public ConEmuSettings()
        { }

		#endregion
		#region ILoadConEmuStartInfo interface

		public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
        {
			mSettings = new ConEmuSettingValueFormatting();
			mSettings.LoadConEmuStartInfo(StartInfo);

			mShellSettings = InstantiateShellSettingsObj();
			mShellSettings.LoadConEmuStartInfo(StartInfo);

            LoadAllSettings();
        }

        public void SaveSettings()
		{
			StoreFontSettings();
			StoreShellToLaunch();
		}

		#endregion
		#region Private methods

		/// <summary>
		/// Master method for reading the current settings from the provided <see cref="ConEmuStartInfo"/> object.
		/// </summary>
		private void LoadAllSettings()
        {
            GetFontSettings();
			GetShellToLaunch();
        }

		protected virtual ConEmuShellSettings InstantiateShellSettingsObj()
		{
			return new ConEmuShellSettings();
		}

        #endregion  
        #region IDisplaySettings Interface

        public IFontSettings FontSettings { get; set; }

		private class ConEmuFontSettings : IFontSettings
        {
            public bool Bold { get; set; }
            public string FontName { get; set; }
            public int FontSize { get; set; }
            public bool Italic { get; set; }
        }

		/// <summary>
		/// Parse font settings from the <see cref="mSettings"/> object.
		/// </summary>
		private void GetFontSettings()
        {
            IFontSettings fs = new ConEmuFontSettings();

            fs.FontName = mSettings.GetString(FontName);
            fs.FontSize = Convert.ToInt32(mSettings.GetLongValue(FontSize));
            fs.Bold = mSettings.GetBooleanValue(FontBold);
            fs.Italic = mSettings.GetBooleanValue(FontItalic);

            FontSettings = fs;
        }

		/// <summary>
		/// Store settings to the <see cref="mSettings"/> object.
		/// </summary>
		private void StoreFontSettings()
        {
            mSettings.SetString(FontName, FontSettings.FontName);
            mSettings.SetLongValue(FontSize, Convert.ToInt64(FontSettings.FontSize), ConEmuSettingValueFormatting.IntFormatType.dword);
            mSettings.SetBooleanValue(FontBold, FontSettings.Bold);
            mSettings.SetBooleanValue(FontItalic, FontSettings.Italic);
        }

		#endregion
		#region IShellSettings Interface

		public ConEmuShell ShellToLaunch { get; set; }

		/// <summary>
		/// Load the parsed shell from the <see cref="mShellSettings"/> object.
		/// </summary>
		private void GetShellToLaunch()
		{
			ShellToLaunch = mShellSettings.ShellToLaunch;
		}

		/// <summary>
		/// Store the shell to be launched into the <see cref="mShellSettings"/> object.
		/// </summary>
		private void StoreShellToLaunch()
		{
			mShellSettings.ShellToLaunch = ShellToLaunch;
			mShellSettings.SaveSettings();
		}

		#endregion
	}
}
