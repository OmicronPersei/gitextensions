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
    internal class ConEmuSettingValueFormatting : ILoadConEmuStartInfo
    {
        protected ConEmuStartInfoXmlInterface mXmlInterface;

        public ConEmuSettingValueFormatting()
        { }

        #region ILoadConEmuStartInfo Interface

        public void LoadConEmuStartInfo(ConEmuStartInfo StartInfo)
        {
            mXmlInterface = InstantiateXmlInterface(StartInfo);

            mXmlInterface.LoadConEmuStartInfo(StartInfo);
        }

		public void SaveSettings()
		{
			//Nothing to be done.  All settings to be saved are already accomplished
			//using XmlDocument node writes.
		}

        #endregion

        protected virtual ConEmuStartInfoXmlInterface InstantiateXmlInterface(ConEmuStartInfo StartInfo)
        {
            return new ConEmuStartInfoXmlInterface();
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

                    return (val.ToString("X8")).ToUpper();

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

		/// <summary>
		/// Format a boolean as a single byte string.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Parse boolean from single byte string.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Parse long integer from hexadecimal string.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		protected long ParseLongInt(string Value)
        {
            return Convert.ToInt64(Value, 16);
        }

		#endregion
		#region Data setters/getters

		/// <summary>
		/// Get the boolean value of the respective name.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public bool GetBooleanValue(string Name)
        {
            return ParseBoolean(mXmlInterface.GetStringDataAttributeFromName(Name));
        }

		/// <summary>
		/// Set the boolean value of the respective name.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Value"></param>
		public void SetBooleanValue(string Name, bool Value)
        {
            mXmlInterface.SetDataValueForAttribute(Name, GetFormattedValue(Value));
        }

		/// <summary>
		/// Get the long integer value of the respective name.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public long GetLongValue(string Name)
        {
            return ParseLongInt(mXmlInterface.GetStringDataAttributeFromName(Name));
        }

		/// <summary>
		/// Set the integer / long integer value of the specified name and the specified storage type.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="value"></param>
		/// <param name="FormatType"></param>
		public void SetLongValue(string Name, long value, IntFormatType FormatType)
        {
            mXmlInterface.SetDataValueForAttribute(Name, GetFormattedValue(value, FormatType));
        }

		/// <summary>
		/// Get the string value of the respective name.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public string GetString(string Name)
        {
            return mXmlInterface.GetStringDataAttributeFromName(Name);
        }

		/// <summary>
		/// Set the string value of the respective name.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Value"></param>
		public void SetString(string Name, string Value)
        {
            mXmlInterface.SetDataValueForAttribute(Name, Value);
        }

        #endregion
    }
}
