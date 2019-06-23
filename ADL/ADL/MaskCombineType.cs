namespace ADL
{
    /// <summary>
    ///     Specifies the way the program should combine masks
    /// </summary>
    public enum MaskCombineType
    {
        /// <summary>
        ///     Add everything that both "tables" have
        /// </summary>
        BitOr = 0,

        /// <summary>
        ///     Add only flags that is represented in both tables
        /// </summary>
        BitAnd = 1
    }
}