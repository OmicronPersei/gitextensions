using ConEmu.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// Class that is a wrapper around <see cref="ConEmuStartInfoXmlInterface"/> to provide
    /// the ability read/write formatted values to the settings object.
    /// </summary>
    internal class ConEmuStartInfoSettingsInterface
    {
        protected ConEmuStartInfoXmlInterface mXmlInterface;

        public ConEmuStartInfoSettingsInterface()
        { }

        public void LoadStartInfo(ConEmuStartInfo StartInfo)
        {
            mXmlInterface = InstantiateXmlInterface(StartInfo);
        }

        protected virtual ConEmuStartInfoXmlInterface InstantiateXmlInterface(ConEmuStartInfo StartInfo)
        {
            ConEmuStartInfoXmlInterface xmlInterface = new ConEmuStartInfoXmlInterface();
            xmlInterface.LoadStartInfo(StartInfo);

            return xmlInterface;
        }

        #region Data formatting

        public enum IntFormatType
        {
            hex,
            dword
        };

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

        #endregion
        #region Data setters/getters

        public bool GetBooleanValue(string Name)
        {
            return ParseBoolean(mXmlInterface.GetStringDataAttributeFromName(Name));
        }

        public void SetBooleanValue(string Name, bool Value)
        {
            mXmlInterface.SetDataValueForAttribute(Name, GetFormattedValue(Value));
        }

        public long GetLongValue(string Name)
        {
            return ParseLongInt(mXmlInterface.GetStringDataAttributeFromName(Name));
        }

        public void SetLongValue(string Name, long value, IntFormatType FormatType)
        {
            mXmlInterface.SetDataValueForAttribute(Name, GetFormattedValue(value, FormatType));
        }

        public string GetString(string Name)
        {
            return mXmlInterface.GetStringDataAttributeFromName(Name);
        }

        public void SetString(string Name, string Value)
        {
            mXmlInterface.SetDataValueForAttribute(Name, Value);
        }

        #endregion
    }
}
