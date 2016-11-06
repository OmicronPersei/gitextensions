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
    internal interface IConEmuStartInfoLoadSave
    {
        void LoadConEmuStartInfo(ConEmuStartInfo StartInfo);
        ConEmuStartInfo GetStoredValues();
    }
}
