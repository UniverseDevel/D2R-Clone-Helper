using System;
using System.Windows.Forms;

namespace D2R_Clone_Helper
{
    internal partial class Init
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Detector detector = new Detector();
            detector.Start();
        }
    }
}
