using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace ADL
{
    /// <summary>
    /// Contains the code for saving and loading Config files in this project
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// The field of the serializer that gets used.
        /// </summary>
        private static XmlSerializer ser;

        /// <summary>
        /// Reads a config of type T from file.
        /// </summary>
        /// <typeparam name="T">Type of Config</typeparam>
        /// <param name="path">Path to config</param>
        /// <returns>Deserialized Config File.</returns>
        public static T ReadFromFile<T>(string path) where T : IADLConfig
        {
            T ret;
            ser = new XmlSerializer(typeof(T));
            if (!File.Exists(path))
            {
                return (T)Activator.CreateInstance<T>().GetStandard();
            }
            try
            {
                FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
                ret = (T)ser.Deserialize(fs);
                fs.Close();
            }
            catch (Exception)
            {
                ret = (T)Activator.CreateInstance<T>().GetStandard();
            }

            return ret;
        }

        /// <summary>
        /// Saves the specified Config File of type T at the supplied filepath
        /// </summary>
        /// <typeparam name="T">Type of config</typeparam>
        /// <param name="path">path to config file.</param>
        /// <param name="data">config object></param>
        public static void SaveToFile<T>(string path, T data) where T : IADLConfig
        {
            ser = new XmlSerializer(typeof(T));
            FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            ser.Serialize(fs, data);
            fs.Close();
        }
    }
}
