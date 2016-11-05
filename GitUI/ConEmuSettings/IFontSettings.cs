using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitUI.ConEmuSettings
{
    public interface IFontSettings
    {
        string FontName { get; set; }
        int FontSize { get; set; }
        bool Bold { get; set; }
        bool Italic { get; set; }
    }
}
