using System;
using System.Collections.Generic;

namespace ADL.Configs
{
    /// <summary>
    ///     A Serializable Dictionary that splits up the dictionary in two lists(while giving up the dictionaries
    ///     functionality) to make it serializable
    /// </summary>
    /// <typeparam name="T1">Key</typeparam>
    /// <typeparam name="T2">Value</typeparam>
    public class SerializableDictionary<T1, T2>
    {
        /// <summary>
        ///     The Stored Keys
        /// </summary>
        public List<T1> Keys;

        /// <summary>
        ///     The Stored Values
        /// </summary>
        public List<T2> Values;

        /// <summary>
        ///     Converts dict into a Serializable Dictionary
        /// </summary>
        /// <param name="dict">The Dictionary you want to serialize</param>
        public SerializableDictionary(Dictionary<T1, T2> dict)
        {
            Keys = new List<T1>();
            Values = new List<T2>();
            foreach (var kvp in dict)
            {
                Keys.Add(kvp.Key);
                Values.Add(kvp.Value);
            }
        }

        public SerializableDictionary()
        {
            Keys = new List<T1>();
            Values = new List<T2>();
        }

        public T2 GetValue(T1 key)
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (Keys[i].Equals(key)) return Values[i];
            }
            throw new Exception("Thats fucked up");
        }

        /// <summary>
        ///     Creates a Dictionary and puts the content of this into it.
        /// </summary>
        /// <returns>The dictionary with the content of the key and value lists.</returns>
        public Dictionary<T1, T2> ToDictionary()
        {
            var ret = new Dictionary<T1, T2>();
            for (var i = 0; i < Keys.Count; i++) ret.Add(Keys[i], Values[i]);
            return ret;
        }
    }
}