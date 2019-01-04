using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL.Configs
{
    /// <summary>
    /// A Serializable Dictionary that splits up the dictionary in two lists(while giving up the dictionaries functionality) to make it serializable
    /// </summary>
    /// <typeparam name="T1">Key</typeparam>
    /// <typeparam name="T2">Value</typeparam>
    public struct SerializableDictionary<T1, T2>
    {
        /// <summary>
        /// The Stored Keys
        /// </summary>
        public List<T1> keys;
        /// <summary>
        /// The Stored Values
        /// </summary>
        public List<T2> values;

        /// <summary>
        /// Converts dict into a Serializable Dictionary
        /// </summary>
        /// <param name="dict">The Dictionary you want to serialize</param>
        public SerializableDictionary(Dictionary<T1,T2> dict)
        {
            keys = new List<T1>();
            values = new List<T2>();
            foreach (KeyValuePair<T1,T2> kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        /// <summary>
        /// Creates a Dictionary and puts the content of this into it.
        /// </summary>
        /// <returns>The dictionary with the content of the key and value lists.</returns>
        public Dictionary<T1, T2> ToDictionary()
        {
            Dictionary<T1, T2> ret = new Dictionary<T1, T2>();
            for (int i = 0; i < keys.Count; i++)
            {
                ret.Add(keys[i], values[i]);
            }
            return ret;
        }
    }
}
