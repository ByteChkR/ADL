using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    /// <summary>
    ///     Config Object for the Network Extensions of ADL
    /// </summary>
    public class NetworkConfig
    {

        /// <summary>
        ///     IP where the Client Connects to
        /// </summary>
        public string Ip = "localhost";

        /// <summary>
        ///     Port for server and client.
        /// </summary>
        public int Port = 1337;


        /// <summary>
        ///     Flag that can be used to skip checking for updates.
        /// </summary>
        public bool UseNetwork = true;

        /// <summary>
        ///     Loads the Network Config from the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static NetworkConfig Load(string path = "")
        {
            var ret = new NetworkConfig();
            if (!File.Exists(path)) return ret;
            var cs = new XmlSerializer(typeof(NetworkConfig));
            var fs = new FileStream(path, FileMode.Open);
            ret = (NetworkConfig) cs.Deserialize(fs);

            return ret;
        }

        /// <summary>
        ///     Saves the Network Config to the specififed path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="conf"></param>
        public static void Save(string path, NetworkConfig conf)
        {
            if (File.Exists(path))
                File.Delete(path);
            var cs = new XmlSerializer(typeof(NetworkConfig));
            var fs = new FileStream(path, FileMode.Create);
            cs.Serialize(fs, conf);
        }
    }
}