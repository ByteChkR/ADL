using System;

namespace ADL.Configs
{
    /// <summary>
    ///     A interface that all Config files in this project have in common. This makes me able to always return "something"
    ///     even if i can not read the config.
    /// </summary>
    [Serializable]
    public abstract class AbstractAdlConfig
    {
        ///// <summary>
        /////     Used by the Config Manager to read the standard config when reading the actual config file failed.
        ///// </summary>
        ///// <returns></returns>
        //AbstractAdlConfig GetStandard();
        public bool CheckForUpdates;
        public abstract AbstractAdlConfig GetStandard();
    }
}