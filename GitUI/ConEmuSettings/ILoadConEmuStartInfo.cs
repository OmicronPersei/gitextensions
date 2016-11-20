using ConEmu.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Provides a wrapper for loading a <see cref="ConEmuStartInfo"/> </para>
    /// <para>for settings modification</para>
    /// </summary>
    internal interface ILoadConEmuStartInfo
    {
		/// <summary>
		/// <para>Instructs the implementing class to load the <see cref="ConEmuStartInfo"/> object and parse relevant settings from it.</para> 
		/// </summary>
		/// <param name="StartInfo">The object to load.</param>
		void LoadConEmuStartInfo(ConEmuStartInfo StartInfo);

		/// <summary>
		/// <para>Instructs the implementing class to store settings in the provided <see cref="ConEmuStartInfo"/> object,
		/// which was provided using the call <see cref="ILoadConEmuStartInfo.LoadConEmuStartInfo(ConEmuStartInfo)"/></para>
		/// </summary>
		void SaveSettings();	//to do: is this redundant?
    }
}
