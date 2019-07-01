using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    /// <summary>
    ///     Config Object for the Network Extensions of ADL
    /// </summary>
    public class NetworkServerConfig
    {
        /// <summary>
        ///     The map that maps the ids the clients send to names that make sense.
        /// </summary>
        public string[] Id2NameMap =
        {
            "Test"
        };


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
            var ret = new NetworkServerConfig();
            if (!File.Exists(path)) return ret;
            var cs = new XmlSerializer(typeof(NetworkServerConfig));
            var fs = new FileStream(path, FileMode.Open);
            ret = (NetworkServerConfig) cs.Deserialize(fs);

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
        }
    }
}