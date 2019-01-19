using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;
using ADL.Configs;
using ADL.Streams;
namespace ADL.CustomCMD
{
    /// <summary>
    /// Utils to Create and Use CustomCMDs
    /// </summary>
    public static class CMDUtils
    {
        /// <summary>
        /// Flag to Call InitWinForms once
        /// </summary>
        private static bool _WinFormsFlagsInitialized = false;



        /// <summary>
        /// Runs a WindowsForm on a different thread.
        /// </summary>
        /// <param name="ps">the Form to start on a different Thread.</param>
        private static void CreateCustomConsole(Form ps)
        {
            new System.Threading.Thread(() =>
                Run(ps)).Start();

        }

        #region Creating Custom Console

        /// <summary>
        /// Runs the form.
        /// </summary>
        /// <param name="ps"></param>
        static void Run(Form ps)
        {
            
            Application.Run(ps);

        }

        /// <summary>
        /// Creates a Custom Console on the Supplied PipeStream.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="Background">Background Color</param>
        /// <param name="FontColor">Font Color</param>
        /// <param name="FontSize">Font Size</param>
        /// <param name="colorCoding">Color Coding for the Tags</param>
        /// <returns>Reference to the Created Console.(Not Thread Save)</returns>
        public static Form CreateCustomConsole(PipeStream ps, Color Background, Color FontColor, float FontSize = 8.25f, int FrameTime = 250, Dictionary<int, SerializableColor> colorCoding = null)
        {
            if (Debug.SendUpdateMessageOnFirstLog)
            {
                string msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                Debug.Log(Debug.UpdateMask, msg);
            }


            if (!_WinFormsFlagsInitialized)
                InitWinForms();

            Form cmd;
            cmd = new CustomCMDForm(ps, Background, FontColor, FontSize, FrameTime, colorCoding);
            CreateCustomConsole(cmd);
            return cmd;
        }

        /// <summary>
        /// Create a Console Window with a supplied config
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, ADLCustomConsoleConfig config)
        {
            return CreateCustomConsole(ps, config.BackgroundColor, config.FontColor, config.FontSize, config.FrameTime, config.ColorCoding.ToDictionary());
        }

        /// <summary>
        /// Creates a CustomCmd based on the config at the supplied filepath
        /// </summary>
        /// <param name="ps">The pipestream that is used.</param>
        /// <param name="configPath">The path to the config</param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, string configPath = "adl_customcmd_config.xml")
        {
            return CreateCustomConsole(ps, ConfigManager.ReadFromFile<ADLCustomConsoleConfig>(configPath));
        }

        /// <summary>
        /// Creates a Custom Console but without returning it.
        /// This function can be called without having to reference System.Windows.Forms.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="Background">Background Color</param>
        /// <param name="FontColor">Font Color</param>
        /// <param name="FontSize">Font Size</param>
        /// <param name="colorCoding">Color Coding for the Tags</param>
        public static void CreateCustomConsoleNoReturn(PipeStream ps, ADLCustomConsoleConfig config)
        {
            //Fix readability later

            CreateCustomConsole(CreateCustomConsole(ps as PipeStream, config.BackgroundColor , config.FontColor, config.FontSize, config.FrameTime, config.ColorCoding.ToDictionary()));
        }

        /// <summary>
        /// Creates a Custom Console but without returning it.
        /// This function can be called without having to reference System.Windows.Forms.
        /// This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="FontSize">Font Size</param>
        /// <param name="colorCoding">Color Coding for the Tags</param>
        public static void CreateCustomConsoleNoReturn(PipeStream ps, string configPath = "adl_customcmd_config.xml")
        {
            CreateCustomConsoleNoReturn(ps, ConfigManager.ReadFromFile<ADLCustomConsoleConfig>(configPath));
        }

        #endregion

        /// <summary>
        /// Saves the supplied config to the supplied file path
        /// </summary>
        /// <param name="config"></param>
        /// <param name="path"></param>
        public static void SaveConfig(ADLCustomConsoleConfig config, string path = "adl_customcmd_config.xml")
        {
            ConfigManager.SaveToFile(path, config);
        }

        /// <summary>
        /// Saves the Supplied Arguments into a config file, that can be loaded when creating the form.
        /// </summary>
        /// <param name="Background"></param>
        /// <param name="FontColor"></param>
        /// <param name="FontSize"></param>
        /// <param name="colorCoding"></param>
        /// <param name="path"></param>
        public static void SaveConfig(Color Background, Color FontColor, float FontSize, Dictionary<int, SerializableColor> colorCoding, string path = "adl_customcmd_config.xml")
        {
            ADLCustomConsoleConfig config = new ADLCustomConsoleConfig
            {
                BackgroundColor = Background,
                FontColor = FontColor,
                FontSize = FontSize,
                ColorCoding = new SerializableDictionary<int, SerializableColor>(colorCoding)
            };
            SaveConfig(config, path);
        }

        

        /// <summary>
        /// Initializes the System.Windows.Forms System.
        /// </summary>
        private static void InitWinForms()
        {
            _WinFormsFlagsInitialized = true;
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (System.Exception)
            {
                
            }
        }
    }
}
