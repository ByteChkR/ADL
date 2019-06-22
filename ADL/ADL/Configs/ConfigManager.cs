﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    /// <summary>
    ///     Contains the code for saving and loading Config files in this project
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        ///     The field of the serializer that gets used.
        /// </summary>
        private static XmlSerializer _serializer;

        /// <summary>
        ///     Reads a config of type T from file.
        /// </summary>
        /// <typeparam name="T">Type of Config</typeparam>
        /// <param name="path">Path to config</param>
        /// <returns>Deserialized Config File.</returns>
        public static T ReadFromFile<T>(string path) where T : IADLConfig
        {
            T ret;
            _serializer = new XmlSerializer(typeof(T));
            if (!File.Exists(path))
            {
                Debug.Log(BitMask.WildCard, "Config Manager: File" + path + "does not exist");
                return (T) Activator.CreateInstance<T>().GetStandard();
            }

            try
            {
                var fs = File.Open(path, FileMode.Open, FileAccess.Read);
                ret = (T) _serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception)
            {
                ret = (T) Activator.CreateInstance<T>().GetStandard();
                Debug.Log(BitMask.WildCard,
                    "Config Manager: Failed to deserialize XML file. Either XML file is corrupted or file access is denied.");
            }

            return ret;
        }

        /// <summary>
        ///     Saves the specified Config File of type T at the supplied filepath
        /// </summary>
        /// <typeparam name="T">Type of config</typeparam>
        /// <param name="path">path to config file.</param>
        /// <param name="data">config object></param>
        public static void SaveToFile<T>(string path, T data) where T : IADLConfig
        {
            try
            {
                _serializer = new XmlSerializer(typeof(T));
                var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                _serializer.Serialize(fs, data);
                fs.Close();
            }
            catch (Exception)
            {
                Debug.Log(BitMask.WildCard,
                    "Config Manager: Failed to save xml file. Directory exists? Access to Write to directory?");
            }
        }
    }
}