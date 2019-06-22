using System;

namespace ADL.Configs
{
    [Flags]
    public enum PrefixLookupSettings
    {
        NOPREFIX = 0,
        ADDPREFIXIFAVAILABLE = 1,
        DECONSTRUCTMASKTOFIND = 2,
        ONLYONEPREFIX = 4,
        BAKEPREFIXES = 8
    }
}