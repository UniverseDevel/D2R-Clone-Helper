using D2R_Clone_Helper.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace D2R_Clone_Helper
{
    internal partial class Init
    {
        internal static readonly Process product_process = Process.GetCurrentProcess();
        internal static readonly int product_process_id = product_process.Id;
        internal static readonly string product_name = Path.GetFileName(Assembly.GetEntryAssembly().Location);
        internal static readonly string product_location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        internal static readonly string lib_WindowsFirewallHelper = product_location + @"\WindowsFirewallHelper.dll";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            resourceHandler(lib_WindowsFirewallHelper, Resources.WindowsFirewallHelper);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Detector detector = new Detector();
            detector.Start();
        }

        internal static void resourceHandler(string location, byte[] resource)
        {
            if (File.Exists(location))
            {
                string file_md5 = getFileMD5(location);
                string resource_md5 = getResourceMD5(resource);
                if (file_md5 != resource_md5)
                {
                    File.Delete(location);
                    File.WriteAllBytes(location, resource);
                }
            }
            else
            {
                File.WriteAllBytes(location, resource);
            }
        }

        internal static string getFileMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return String.Join(String.Empty, md5.ComputeHash(stream));
                }
            }
        }

        internal static string getResourceMD5(byte[] resouce)
        {
            using (MemoryStream stream = new MemoryStream(resouce))
            {
                using (MD5 md5 = MD5.Create())
                {
                    return String.Join(String.Empty, md5.ComputeHash(stream));
                }
            }
        }
    }
}
