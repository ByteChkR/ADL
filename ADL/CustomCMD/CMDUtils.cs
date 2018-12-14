using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ADL;

namespace ADL.CustomCMD
{
    public static class CMDUtils
    {
        private static void CreateCustomConsole(Form ps)
        {
            new System.Threading.Thread(() => Application.Run(ps)).Start();
        }

        public static Form CreateCustomConsole(PipeStream ps)
        {
            
            Form cmd;
            cmd = new CustomCMDForm(new System.IO.StreamReader(ps));
            CreateCustomConsole(cmd);
            return cmd;
        }

        public static void CreateCustomConsoleNoReturn(System.IO.Stream ps)
        {
            if (ps != null && !(ps is PipeStream)) return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CreateCustomConsole(new CustomCMDForm(new System.IO.StreamReader(ps)));
        }
    }
}
