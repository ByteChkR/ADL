namespace ADL
{
    /// <summary>
    ///     Specifies how the LogStream Masks react to flags.
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        ///     If one flag is not in the logstream mask, return false
        /// </summary>
        MatchAll = 0,

        /// <summary>
        ///     If there is at least one flag in the mask
        /// </summary>
        MatchOne = 1
    }
}