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

        public static Form CreateCustomConsole(PipeStream ps, Dictionary<string, System.Drawing.Color> colorCoding = null, float FontSize = 8.25f)
        {
            return CreateCustomConsole(ps, System.Drawing.Color.Black, System.Drawing.Color.White, FontSize, colorCoding);
        }

        public static Form CreateCustomConsole(PipeStream ps, System.Drawing.Color Background, System.Drawing.Color FontColor, float FontSize = 8.25f, Dictionary<string, System.Drawing.Color> colorCoding = null)
        {
            if (!_WinFormsFlagsInitialized)
                InitWinForms();

            Form cmd;
            cmd = new CustomCMDForm(ps, Background, FontColor, FontSize, colorCoding);
            CreateCustomConsole(cmd);
            return cmd;
        }
        public static void CreateCustomConsoleNoReturn(System.IO.Stream ps, System.Drawing.Color Background, System.Drawing.Color FontColor, float FontSize = 8.25f, Dictionary<string, System.Drawing.Color> colorCoding = null)
        {
            if (ps != null && !(ps is PipeStream)) return;
            if (!_WinFormsFlagsInitialized)
                InitWinForms();
            CreateCustomConsole(new CustomCMDForm(ps as PipeStream, Background, FontColor, FontSize, colorCoding));
        }

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
