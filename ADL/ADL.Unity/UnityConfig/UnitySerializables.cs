using System;
using System.Collections.Generic;
using ADL.Configs;

namespace ADL.Unity.UnityConfig
{
    [Serializable]
    public class SerializableDictionaryIntString : SerializableDictionary<int, string>
    {
        public SerializableDictionaryIntString(Dictionary<int, string> dict) : base(dict)
        {
        }

        public SerializableDictionaryIntString()
        {
        }
    }

    [Serializable]
    public class SerializableDictionaryIntColor : SerializableDictionary<int, SerializableColor>
    {
        public SerializableDictionaryIntColor(Dictionary<int, SerializableColor> dict) : base(dict)
        {
        }

        public SerializableDictionaryIntColor()
        {
        }
    }

    [Serializable]
    public class UnityAAdlConfig : AdlConfig
    {
        public SerializableDictionaryIntString MaskPrefix;

        public void Prepare()
        {
            Prefixes = MaskPrefix;
        }
    }

    [Serializable]
    public class UnityAdlCustomConsoleConfig : AdlCustomConsoleConfig
    {
        public SerializableDictionaryIntColor LogColorCoding;

        public void Prepare()
        {
            ColorCoding = LogColorCoding;
        }
    }
}