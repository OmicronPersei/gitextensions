using ConEmu.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Provides a wrapper for loading and getting</para>
    /// <para>a <see cref="ConEmuStartInfo"/> settings object.</para>
    /// </summary>
    internal interface IConEmuStartInfoLoadSave
    {
        void LoadConEmuStartInfo(ConEmuStartInfo StartInfo);
        ConEmuStartInfo GetStartInfo();
    }
}
