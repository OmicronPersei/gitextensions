using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    /// <summary>
    /// <para>Serves as a generic interface for packing display based</para>
    /// <para>settings into a setting object.</para>
    /// </summary>
    public interface IDisplaySettings
    {


        IFontSettings FontSettings { get; set; }


    }
}
