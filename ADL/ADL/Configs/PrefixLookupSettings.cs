using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL.Configs
{
    [Flags]
    public enum PrefixLookupSettings
    {
        ADDPREFIXIFAVAILABLE = 1,
        DECONSTRUCTMASKTOFIND = 2,
        ONLYONEPREFIX = 4
    }
}
