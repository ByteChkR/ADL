namespace ADL.Configs
{
    /// <summary>
    ///     A interface that all Config files in this project have in common. This makes me able to always return "something"
    ///     even if i can not read the config.
    /// </summary>
    public interface IADLConfig
    {
        /// <summary>
        ///     Used by the Config Manager to read the standard config when reading the actual config file failed.
        /// </summary>
        /// <returns></returns>
        IADLConfig GetStandard();
    }
}