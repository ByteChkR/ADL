using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    /// <summary>
    ///     Config Object for the Network Extensions of ADL
    /// </summary>
    public class NetworkServerConfig : IAdlConfig
    {


        /// <summary>
        ///     The map that maps the ids the clients send to names that make sense.
        /// </summary>
        public SerializableDictionary<string, string> Id2NameMap = new SerializableDictionary<string, string>(
            new Dictionary<string, string>()
            {
                {"", ""}
            });


        /// <summary>
        ///     Port for server and client.
        /// </summary>
        public int Port = 1337;




        /// <summary>
        ///     Loads the Network Config from the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static NetworkServerConfig Load(string path = "")
        {
            var ret = Standard;
            if (!File.Exists(path)) return ret;
            var cs = new XmlSerializer(typeof(NetworkServerConfig));
            var fs = new FileStream(path, FileMode.Open);
            ret = (NetworkServerConfig)cs.Deserialize(fs);
            fs.Close();
            
            return ret;
        }

        /// <summary>
        ///     Saves the Network Config to the specififed path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="conf"></param>
        public static void Save(string path, NetworkServerConfig conf)
        {
            if (File.Exists(path))
                File.Delete(path);
            var cs = new XmlSerializer(typeof(NetworkServerConfig));
            var fs = new FileStream(path, FileMode.Create);
            
            cs.Serialize(fs, conf);
            fs.Close();
        }


        public static NetworkServerConfig Standard
        {
            get
            {
                NetworkServerConfig nws = new NetworkServerConfig();
                nws.Port = 1337;
                nws.Id2NameMap = new SerializableDictionary<string, string>(new Dictionary<string, string>()
                {
                    {"YEET", "0.0.0.0" },
                    {"YEETus", "0.0.0.0" },
                    {"YEETing", "0.0.0.0" },
                });
                return nws;
            }
        }

        public IAdlConfig GetStandard()
        {
            return Standard;
        }
    }
}