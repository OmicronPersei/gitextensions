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
    /// <see cref="IDisplaySettingsPacker"/> interface.</para>
    /// </summary>
    internal class ConEmuStartInfoDisplaySettings : IDisplaySettingsPacker
    {
        protected XmlDocument mSettingsXml;

        public ConEmuStartInfoDisplaySettings(ConEmuStartInfo StartInfo)
        {
            mSettingsXml = StartInfo.BaseConfiguration;

            var softwareNode = StartInfo.BaseConfiguration.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueSoftware}']") as XmlElement;
            var conEmuNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueConEmu}']") as XmlElement;
            var vanillaNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueDotVanilla}']") as XmlElement;

        }

        public Font FontBackupSettings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }


        private Font mMainFontSettings;
        public Font FontSettings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        protected void WriteFontSettingsToXml(Font Settings)
        {

        }

    }
}
