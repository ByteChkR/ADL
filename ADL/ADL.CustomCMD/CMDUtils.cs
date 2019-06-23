using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ADL.Configs;
using ADL.Streams;

namespace ADL.CustomCMD
{
    /// <summary>
    ///     Utils to Create and Use CustomCMDs
    /// </summary>
    public static class CmdUtils
    {
        /// <summary>
        ///     Flag to Call InitWinForms once
        /// </summary>
        private static bool _winFormsFlagsInitialized;


        /// <summary>
        ///     Runs a WindowsForm on a different thread.
        /// </summary>
        /// <param name="ps">the Form to start on a different Thread.</param>
        private static void CreateCustomConsole(Form ps)
        {
            new Thread(() =>
                Run(ps)).Start();
        }

        /// <summary>
        ///     Saves the supplied config to the supplied file path
        /// </summary>
        /// <param name="config"></param>
        /// <param name="path"></param>
        public static void SaveConfig(AdlCustomConsoleConfig config, string path = "adl_customcmd_config.xml")
        {
            ConfigManager.SaveToFile(path, config);
        }

        /// <summary>
        ///     Saves the Supplied Arguments into a config file, that can be loaded when creating the form.
        /// </summary>
        /// <param name="background"></param>
        /// <param name="fontColor"></param>
        /// <param name="fontSize"></param>
        /// <param name="colorCoding"></param>
        /// <param name="path"></param>
        public static void SaveConfig(Color background, Color fontColor, float fontSize,
            Dictionary<int, SerializableColor> colorCoding, string path = "adl_customcmd_config.xml")
        {
            var config = new AdlCustomConsoleConfig
            {
                BackgroundColor = background,
                FontColor = fontColor,
                FontSize = fontSize,
                ColorCoding = new SerializableDictionary<int, SerializableColor>(colorCoding)
            };
            SaveConfig(config, path);
        }


        /// <summary>
        ///     Initializes the System.Windows.Forms System.
        /// </summary>
        private static void InitWinForms()
        {
            _winFormsFlagsInitialized = true;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (Exception)
            {
            }
        }

        #region Creating Custom Console

        /// <summary>
        ///     Runs the form.
        /// </summary>
        /// <param name="ps"></param>
        private static void Run(Form ps)
        {
            Application.Run(ps);
        }

        /// <summary>
        ///     Creates a Custom Console on the Supplied PipeStream.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="background">Background Color</param>
        /// <param name="fontColor">Font Color</param>
        /// <param name="fontSize">Font Size</param>
        /// <param name="frameTime">Frame time of the main loop</param>
        /// <param name="colorCoding">Color Coding for the Tags</param>
        /// <returns>Reference to the Created Console.(Not Thread Save)</returns>
        public static Form CreateCustomConsole(PipeStream ps, Color background, Color fontColor, float fontSize = 8.25f,
            int frameTime = 250, Dictionary<int, SerializableColor> colorCoding = null)
        {
            if (Debug.SendUpdateMessageOnFirstLog)
            {
                var msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version);
                Debug.Log(Debug.UpdateMask, msg);
            }


            if (!_winFormsFlagsInitialized)
                InitWinForms();

            Form cmd = new CustomCmdForm(ps, background, fontColor, fontSize, frameTime, colorCoding);
            CreateCustomConsole(cmd);
            return cmd;
        }

        /// <summary>
        ///     Create a Console Window with a supplied config
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, AdlCustomConsoleConfig config)
        {
            return CreateCustomConsole(ps, config.BackgroundColor, config.FontColor, config.FontSize, config.FrameTime,
                config.ColorCoding.ToDictionary());
        }

        /// <summary>
        ///     Creates a CustomCmd based on the config at the supplied filepath
        /// </summary>
        /// <param name="ps">The pipestream that is used.</param>
        /// <param name="configPath">The path to the config</param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, string configPath = "adl_customcmd_config.xml")
        {
            return CreateCustomConsole(ps, ConfigManager.ReadFromFile<AdlCustomConsoleConfig>(configPath));
        }

        /// <summary>
        ///     Creates a Custom Console but without returning it.
        ///     This function can be called without having to reference System.Windows.Forms.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="config">Config for the CustomCMD</param>
        public static void CreateCustomConsoleNoReturn(PipeStream ps, AdlCustomConsoleConfig config)
        {
            //Fix readability later

            CreateCustomConsole(CreateCustomConsole(ps, config.BackgroundColor, config.FontColor, config.FontSize,
                config.FrameTime, config.ColorCoding.ToDictionary()));
        }

        /// <summary>
        ///     Creates a Custom Console but without returning it.
        ///     This function can be called without having to reference System.Windows.Forms.
        ///     This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="configPath">Path of the Config File</param>
        public static void CreateCustomConsoleNoReturn(PipeStream ps, string configPath = "adl_customcmd_config.xml")
        {
            CreateCustomConsoleNoReturn(ps, ConfigManager.ReadFromFile<AdlCustomConsoleConfig>(configPath));
        }

        #endregion
    }
}