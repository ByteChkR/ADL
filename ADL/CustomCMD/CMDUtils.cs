using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
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
        /// Creates the console on a different thread.
        /// </summary>
        /// <param name="ps">the Form to start on a different Thread.</param>
        private static void CreateCustomConsole(Form ps)
        {
            new System.Threading.Thread(() => 
            
            Application.Run(ps)).Start();
        }

        /// <summary>
        /// Creates a Custom Console on the Supplied PipeStream.
        /// The console will have the default Background and BaseFontColor
        /// This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="colorCoding">Default null</param>
        /// <param name="FontSize">Default 8.25</param>
        /// <returns>The Created Console</returns>
        public static Form CreateCustomConsole(PipeStream ps, Dictionary<string, Color> colorCoding = null, float FontSize = 8.25f)
        {
            return CreateCustomConsole(ps, Color.Black, Color.White, FontSize, colorCoding);
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
        public static Form CreateCustomConsole(PipeStream ps, Color Background, Color FontColor, float FontSize = 8.25f, Dictionary<string, Color> colorCoding = null)
        {
            if (!_WinFormsFlagsInitialized)
                InitWinForms();

            Form cmd;
            cmd = new CustomCMDForm(ps, Background, FontColor, FontSize, colorCoding);
            CreateCustomConsole(cmd);
            return cmd;
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
        public static void CreateCustomConsoleNoReturn(Stream ps, Color Background, Color FontColor, float FontSize = 8.25f, Dictionary<string, Color> colorCoding = null)
        {
            if (ps != null && !(ps is PipeStream)) return;
            CreateCustomConsole(new CustomCMDForm(ps as PipeStream, Background, FontColor, FontSize, colorCoding));
        }

        /// <summary>
        /// Creates a Custom Console but without returning it.
        /// This function can be called without having to reference System.Windows.Forms.
        /// This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps">Pipe Stream</param>
        /// <param name="FontSize">Font Size</param>
        /// <param name="colorCoding">Color Coding for the Tags</param>
        public static void CreateCustomConsoleNoReturn(Stream ps, float FontSize = 8.25f, Dictionary<string, Color> colorCoding = null)
        {
            CreateCustomConsoleNoReturn(ps, Color.Black, Color.White, FontSize, colorCoding);
        }

        /// <summary>
        /// Initializes the System.Windows.Forms System.
        /// </summary>
        private static void InitWinForms()
        {
            _WinFormsFlagsInitialized = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
