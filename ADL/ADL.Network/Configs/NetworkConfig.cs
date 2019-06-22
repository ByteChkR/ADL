using System.IO;
using System.Xml.Serialization;

namespace ADL.Configs
{
    public class NetworkConfig
    {
        public string[] ID2NameMap =
        {
            "Test"
        };

        public string IP = "localhost";
        public int Port = 1337;

        public string TimeFormatString = "MM-dd-yyyy-H-mm-ss";

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