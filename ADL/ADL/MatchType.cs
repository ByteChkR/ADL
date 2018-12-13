﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ADL
{
    /// <summary>
    /// Specifies how the LogStream Masks react to flags.
    /// </summary>
    public enum MatchType : int
    {
        /// <summary>
        /// If one flag is not in the logstream mask, return false
        /// </summary>
        MATCH_ALL = 0,
        /// <summary>
        /// If there is at least one flag in the mask
        /// </summary>
        MATCH_ONE = 1
    }
}
