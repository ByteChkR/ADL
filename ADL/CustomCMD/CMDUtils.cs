using System.Collections.Generic;
using System.Windows.Forms;

namespace ADL.CustomCMD
{
    public static class CMDUtils
    {
        private static bool _WinFormsFlagsInitialized = false;

        private static void CreateCustomConsole(Form ps)
        {
            new System.Threading.Thread(() => Application.Run(ps)).Start();
        }

        /// <summary>
        /// Creates a Custom Console on the Supplied PipeStream.
        /// The console will have the default Background and BaseFontColor
        /// This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="colorCoding">Default null</param>
        /// <param name="FontSize">Default 8.25</param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, Dictionary<string, System.Drawing.Color> colorCoding = null, float FontSize = 8.25f)
        {
            return CreateCustomConsole(ps, System.Drawing.Color.Black, System.Drawing.Color.White, FontSize, colorCoding);
        }

        /// <summary>
        /// Creates a Custom Console on the Supplied PipeStream.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="Background">Background Color</param>
        /// <param name="FontColor">Font Color</param>
        /// <param name="FontSize">Font Size</param>
        /// <param name="colorCoding"></param>
        /// <returns></returns>
        public static Form CreateCustomConsole(PipeStream ps, System.Drawing.Color Background, System.Drawing.Color FontColor, float FontSize = 8.25f, Dictionary<string, System.Drawing.Color> colorCoding = null)
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
        /// <param name="ps"></param>
        /// <param name="Background"></param>
        /// <param name="FontColor"></param>
        /// <param name="FontSize"></param>
        /// <param name="colorCoding"></param>
        public static void CreateCustomConsoleNoReturn(System.IO.Stream ps, System.Drawing.Color Background, System.Drawing.Color FontColor, float FontSize = 8.25f, Dictionary<string, System.Drawing.Color> colorCoding = null)
        {
            if (ps != null && !(ps is PipeStream)) return;
            CreateCustomConsole(new CustomCMDForm(ps as PipeStream, Background, FontColor, FontSize, colorCoding));
        }

        /// <summary>
        /// Creates a Custom Console but without returning it.
        /// This function can be called without having to reference System.Windows.Forms.
        /// This Function is a Wrapper. See Other Overloads for more options.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="FontSize"></param>
        /// <param name="colorCoding"></param>
        public static void CreateCustomConsoleNoReturn(System.IO.Stream ps, float FontSize = 8.25f, Dictionary<string, System.Drawing.Color> colorCoding = null)
        {
            CreateCustomConsoleNoReturn(ps, System.Drawing.Color.Black, System.Drawing.Color.White, FontSize, colorCoding);
        }
        private static void InitWinForms()
        {
            _WinFormsFlagsInitialized = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
