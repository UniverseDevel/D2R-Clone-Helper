using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace D2R_Clone_Helper
{
    public partial class interface_form : Form
    {
        private static bool exitProgram = false;
        private static List<int> D2RprocessesList = new List<int>();
        private static Dictionary<string, string> connectionList = new Dictionary<string, string>();

        internal class ConnectionInfo
        {
            public string Protocol;
            public IPAddress LocalIP;
            public int LocalPort;
            public IPAddress RemoteIP;
            public int RemotePort;
            public string State;
            public int PID;

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread worker = new Thread(detectProcesses);
            worker.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new @interface());
        }

        private static void detectProcesses()
        {
            while (!exitProgram)
            {
                Process[] processList = Process.GetProcesses();
                List<int> foundList = new List<int>();
                List<int> D2RprocessesListNew = new List<int>();

                // Check if we need to hook new process and if yes, hook the process
                foreach (Process process in processList)
                {
                    if (process.ProcessName == "D2R")
                    {
                        if (!D2RprocessesList.Contains(process.Id))
                        {
                            Console.WriteLine("D2R process detected with ID {0}.", process.Id);
                            D2RprocessesList.Add(process.Id);
                        }
                        foundList.Add(process.Id);
                    }
                }

                // Check if there are process IDs that were hooked and they no longer are
                foreach (int processId in D2RprocessesList)
                {
                    if (foundList.Contains(processId))
                    {
                        D2RprocessesListNew.Add(processId);
                    }
                }

                // Add updated list of currently hooked processes
                D2RprocessesList = D2RprocessesListNew;

                findConnections();

                // Wait a while so we won't burn CPU
                Thread.Sleep(10);
            }
        }

        private static void findConnections()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine("netstat -ano");
            process.StandardInput.WriteLine("exit");

            Regex reg = new Regex("\\s+", RegexOptions.Compiled);

            string line;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                line = line.Trim();
                line = reg.Replace(line, ",");
                string[] arr = line.Split(',');
                if (arr[0].ToLower() == "tcp" || arr[0].ToLower() == "udp")
                {
                    ConnectionInfo ci = new ConnectionInfo();
                    ci.Protocol = arr[0].ToLower();
                    ci.LocalIP = parseIP(arr[1]);
                    ci.LocalPort = parsePort(arr[1]);
                    ci.RemoteIP = parseIP(arr[2]);
                    ci.RemotePort = parsePort(arr[2]);
                    ci.State = (ci.Protocol == "tcp") ? arr[3].ToLower() : "none";
                    ci.PID = Convert.ToInt32((ci.Protocol == "tcp") ? arr[4] : arr[3]);

                    if (D2RprocessesList.Contains(ci.PID))
                    {
                        if (
                               !ci.RemoteIP.Equals(IPAddress.Parse("0.0.0.0"))
                            && !ci.RemoteIP.Equals(IPAddress.Parse("[::]"))
                            && !ci.RemoteIP.Equals(IPAddress.Parse("127.0.0.1"))
                        )
                        {
                            if (!connectionList.Contains(ci.RemoteIP.ToString()))
                            {
                                connectionList.Add(ci.RemoteIP.ToString());
                                Console.WriteLine("Found new valid connection for process {0} connecting to {1}.", ci.PID, ci.RemoteIP.ToString());
                            }
                        }
                    }
                }
            }

            process.Close();
        }

        private static IPAddress parseIP(string value)
        {
            int portStart = value.LastIndexOf(':');
            string parsedVal = value.Substring(0, portStart);
            IPAddress ip = (parsedVal == "*") ? IPAddress.Any : IPAddress.Parse(parsedVal);

            return ip;
        }

        private static int parsePort(string value)
        {
            int portStart = value.LastIndexOf(':');
            string parsedVal = value.Substring(portStart + 1);
            int port = (parsedVal == "*") ? -1 : Convert.ToInt32(parsedVal);

            return port;
        }
    }
}
