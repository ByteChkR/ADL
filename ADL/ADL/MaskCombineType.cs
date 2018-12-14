namespace ADL
{
    /// <summary>
    /// Specifies the way the program should combine masks
    /// </summary>
    public enum MaskCombineType
    {
        /// <summary>
        /// Add everything that both "tables" have
        /// </summary>
        BIT_OR = 0,
        /// <summary>
        /// Add only flags that is represented in both tables
        /// </summary>
        BIT_AND = 1
    }
}
