using ConEmu.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Class for getting and setting values to the <see cref="ConEmuStartInfo.BaseConfiguration"/> </para>
    /// </summary>
    internal class ConEmuStartInfoXmlInterface : ILoadConEmuStartInfo
    {
        #region XML Settings Constants

        public const string XmlValueNodeName = "value";
        public const string XmlDataAttributeName = "data";

        public const string XmlFontName = "FontName";
        public const string XmlFontSize = "FontSize";
        public const string XmlFontBold = "FontBold";
        public const string XmlFontItalic = "FontItalic";

        #endregion
        #region Private members

        protected XmlDocument mSettingsXml;
        protected XmlElement mSettingsElem;
        private ConEmuStartInfo mSettingsObj;

        #endregion
        #region Constructor

        public ConEmuStartInfoXmlInterface()
        { }

        #endregion
        #region ILoadConEmuStartInfo Interface

        public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
        {
            mSettingsObj = StartInfo;

            LoadSettingsNode(StartInfo);
        }

		public void SaveSettings()
		{
			//Nothing to do, all values are saved by virtue of having using write calls
			//to set them.
		}

        #endregion

        protected virtual void LoadSettingsNode(ConEmuStartInfo StartInfo)
        {
            try
            {
                var softwareNode = StartInfo.BaseConfiguration.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueSoftware}']") as XmlElement;
                var conEmuNode = softwareNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueConEmu}']") as XmlElement;
                var vanillaNode = conEmuNode.SelectSingleNode($"{ConEmuConstants.XmlElementKey}[@{ConEmuConstants.XmlAttrName}='{ConEmuConstants.XmlValueDotVanilla}']") as XmlElement;

                mSettingsElem = vanillaNode as XmlElement;
            }
            catch
            {
                throw new ArgumentException("Could not navigate to the setttings node.");
            }
        }

        #region XmlDocument getters/setters

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
                return targetElem.GetAttribute(XmlDataAttributeName);
            }
            else
            {
                throw new ArgumentException($"Could not nagivate to the attribute name {AttributeName}");
            }
        }

        private XmlElement GetElement(string AttributeName)
        {
            return mSettingsElem.SelectSingleNode($"{XmlValueNodeName}[@{ConEmuConstants.XmlAttrName}='{AttributeName}']") as XmlElement;
        }

        /// <summary>
        /// Set the data value to the string provided.
        /// </summary>
        /// <param name="AttributeName"></param>
        public virtual void SetDataValueForAttribute(string AttributeName, string Value)
        {
            GetElement(AttributeName).SetAttribute(XmlDataAttributeName, Value);
        }

        #endregion  

    }
}
