using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    public class NetworkConfig
    {


        public bool UseNetwork = true;
        

        /// <summary>
        /// The map that maps the ids the clients send to names that make sense.
        /// </summary>
        public string[] ID2NameMap =
        {
            "Test"
        };

        /// <summary>
        /// IP where the Client Connects to
        /// </summary>
        public string IP = "localhost";
        /// <summary>
        /// Port for server and client.
        /// </summary>
        public int Port = 1337;

        /// <summary>
        /// Determines how the time stamp in the log files looks like.
        /// </summary>
        public string TimeFormatString = "MM-dd-yyyy-H-mm-ss";

        /// <summary>
        /// Loads the Network Config from the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static NetworkConfig Load(string path = "")
        {
            var ret = new NetworkConfig();
            if (File.Exists(path))
            {
                var cs = new XmlSerializer(typeof(NetworkConfig));
                var fs = new FileStream(path, FileMode.Open);
                ret = (NetworkConfig) cs.Deserialize(fs);
            }

            return ret;
        }

        /// <summary>
        /// Saves the Network Config to the specififed path
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