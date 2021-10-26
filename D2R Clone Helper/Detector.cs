using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WindowsFirewallHelper;

namespace D2R_Clone_Helper
{
    class Detector
    {
        HelperUI helperUI = new HelperUI();

        internal List<int> processList = new List<int>();
        internal static Dictionary<string, ConnectionInfo> activeConnectionList = new Dictionary<string, ConnectionInfo>();
        internal static Dictionary<string, IFirewallRule> blockedConnectionList = new Dictionary<string, IFirewallRule>();
        internal static Dictionary<string, ConnectionData> connectionList = new Dictionary<string, ConnectionData>();
        internal static Dictionary<string, List<ConnectionInfo>> allConnectionList = new Dictionary<string, List<ConnectionInfo>>();

        internal string keyPath = @"SOFTWARE";

        internal class ConnectionData
        {
            public string IP;
            public string Notes = String.Empty;
            public bool IsActive = false;
            public bool IsBlocked = false;

        }

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

        internal void Start()
        {
            loadData();

            // Start connection detector
            Thread connectionDetector = new Thread(detectConnections);
            connectionDetector.Start();

            // Start data saver
            Thread dataSaver = new Thread(saveData);
            dataSaver.Start();

            // Load GUI
            Application.Run(helperUI);
        }

        internal void createRegSubKey(RegistryKey location, string keyPath, string keyName)
        {
            using (RegistryKey regKey = location.OpenSubKey(keyPath, true))
            {
                regKey.CreateSubKey(keyName);
            }
        }

        internal string[] getRegValueNames(RegistryKey location, string keyPath, string keyName)
        {
            using (RegistryKey regKey = location.OpenSubKey(keyPath + @"\" + keyName, true))
            {
                return regKey.GetValueNames();
            }
        }

        internal void delRegValueName(RegistryKey location, string keyPath, string keyName, string valueName)
        {
            using (RegistryKey regKey = location.OpenSubKey(keyPath + @"\" + keyName, true))
            {
                regKey.DeleteValue(valueName);
            }
        }

        internal string getRegValue(RegistryKey location, string keyPath, string keyName, string valueName)
        {
            using (RegistryKey regKey = location.OpenSubKey(keyPath + @"\" + keyName, true))
            {
                string value = regKey.GetValue(valueName).ToString();

                return value;
            }
        }

        internal void setRegValue(RegistryKey location, string keyPath, string keyName, string valueName, string value)
        {
            using (RegistryKey regKey = location.OpenSubKey(keyPath + @"\" + keyName, true))
            {
                regKey.SetValue(valueName, value);
            }
        }

        internal void loadData()
        {
            createRegSubKey(Registry.CurrentUser, keyPath, helperUI.appAlias);

            Dictionary<string, ConnectionData> connectionListNew = new Dictionary<string, ConnectionData>();
            string[] ipList = getRegValueNames(Registry.CurrentUser, keyPath, helperUI.appAlias);

            // Save existing IPs
            foreach (string ipAddr in ipList)
            {
                bool isValid = false;
                IPAddress ip;
                if (!connectionListNew.ContainsKey(ipAddr) && IPAddress.TryParse(ipAddr, out ip))
                {
                    if (
                           !ip.Equals(IPAddress.Parse("0.0.0.0"))
                        && !ip.Equals(IPAddress.Parse("[::]"))
                        && !ip.Equals(IPAddress.Parse("127.0.0.1"))
                    )
                    {
                        string desc = getRegValue(Registry.CurrentUser, keyPath, helperUI.appAlias, ipAddr);
                        Console.WriteLine("Loading connection {0} from registry.", ipAddr);
                        connectionListNew.Add(ipAddr, new ConnectionData() { IP = ipAddr, Notes = desc });
                        isValid = true;
                    }
                }

                if (!isValid)
                {
                    delRegValueName(Registry.CurrentUser, keyPath, helperUI.appAlias, ipAddr);
                }
            }

            connectionList = connectionListNew;
        }

        internal void saveData()
        {
            while (!helperUI.exitProgram)
            {
                string[] ipList = getRegValueNames(Registry.CurrentUser, keyPath, helperUI.appAlias);
                Dictionary<string, ConnectionData> connList = new Dictionary<string, ConnectionData>(connectionList);

                foreach (KeyValuePair<string, ConnectionData> kvp in connList)
                {
                    createRegSubKey(Registry.CurrentUser, keyPath, helperUI.appAlias);

                    setRegValue(Registry.CurrentUser, keyPath, helperUI.appAlias, kvp.Key, kvp.Value.Notes);
                }

                // Remove non existing IPs
                foreach (string ip in ipList)
                {
                    if (!connList.ContainsKey(ip))
                    {
                        delRegValueName(Registry.CurrentUser, keyPath, helperUI.appAlias, ip);
                    }
                }

                // Wait a while so we won't burn CPU
                Thread.Sleep(100);
            }
        }

        internal void detectConnections()
        {
            while (!helperUI.exitProgram)
            {
                Process[] processList = Process.GetProcesses();
                List<int> foundList = new List<int>();
                List<int> D2RprocessesListNew = new List<int>();

                // Check if we need to hook new process and if yes, hook the process
                foreach (Process process in processList)
                {
                    if (process.ProcessName == "D2R")
                    {
                        if (!this.processList.Contains(process.Id))
                        {
                            Console.WriteLine("D2R process detected with ID {0}.", process.Id);
                            this.processList.Add(process.Id);
                        }
                        foundList.Add(process.Id);
                    }
                }

                // Check if there are process IDs that were hooked and they no longer are
                foreach (int processId in this.processList)
                {
                    if (foundList.Contains(processId))
                    {
                        D2RprocessesListNew.Add(processId);
                    }
                }

                // Add updated list of currently hooked processes
                this.processList = D2RprocessesListNew;

                // Check netstat for active connections and pair them to processes
                findConnections();

                // Check activity and block state
                updateConnectionStates();

                if (helperUI.guiActive)
                {
                    handleViews();
                }

                // Wait a while so we won't burn CPU
                Thread.Sleep(100);
            }
        }

        internal void handleViews()
        {
            try
            {
                helperUI.Invoke(new Action(() => { helperUI.updateDataSource(connectionList); }));
            }
            catch (ObjectDisposedException ex) { } // Prevent error when closing application
            catch (System.ComponentModel.InvalidAsynchronousStateException ex) { } // Prevent error when closing application
            catch (InvalidOperationException ex) { } // Prevent error when closing application
        }

        internal void updateConnectionStates()
        {
            // Update active status
            foreach (KeyValuePair<string, ConnectionData> kvp in connectionList)
            {
                ConnectionData data = kvp.Value;

                if (activeConnectionList.ContainsKey(data.IP))
                {
                    connectionList[data.IP].IsActive = true;
                }
                else
                {
                    connectionList[data.IP].IsActive = false;
                }
            }

            // Update blocked status
            if (FirewallManager.IsServiceRunning)
            {
                // Update current firewall rules related to this app
                IFirewallRule[] firewallRules = FirewallManager.Instance.Rules.ToArray();
                blockedConnectionList.Clear();
                foreach (IFirewallRule rule in firewallRules)
                {
                    if (rule.Name.StartsWith(helperUI.appAlias))
                    {
                        string ip = rule.Name.Replace(helperUI.appAlias + " - ", String.Empty);
                        blockedConnectionList.Add(ip, rule);
                    }
                }

                foreach (KeyValuePair<string, ConnectionData> kvp in connectionList)
                {
                    ConnectionData data = kvp.Value;

                    if (blockedConnectionList.ContainsKey(data.IP))
                    {
                        connectionList[data.IP].IsBlocked = true;
                    }
                    else
                    {
                        connectionList[data.IP].IsBlocked = false;
                    }
                }
            }
        }

        internal void findConnections()
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

            Dictionary<string, ConnectionInfo> activeConnectionListNew = new Dictionary<string, ConnectionInfo>();
            Dictionary<string, List<ConnectionInfo>> allConnectionListNew = new Dictionary<string, List<ConnectionInfo>>();
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

                    if (processList.Contains(ci.PID))
                    {
                        if (
                               !ci.RemoteIP.Equals(IPAddress.Parse("0.0.0.0"))
                            && !ci.RemoteIP.Equals(IPAddress.Parse("[::]"))
                            && !ci.RemoteIP.Equals(IPAddress.Parse("127.0.0.1"))
                        )
                        {
                            if (!allConnectionListNew.ContainsKey(ci.RemoteIP.ToString()))
                            {
                                allConnectionListNew.Add(ci.RemoteIP.ToString(), new List<ConnectionInfo>() { ci });
                            }
                            else
                            {
                                allConnectionListNew[ci.RemoteIP.ToString()].Add(ci);
                            }
                            if (!activeConnectionListNew.ContainsKey(ci.RemoteIP.ToString()))
                            {
                                activeConnectionListNew.Add(ci.RemoteIP.ToString(), ci);
                            }
                            if (!connectionList.ContainsKey(ci.RemoteIP.ToString()))
                            {
                                connectionList.Add(ci.RemoteIP.ToString(), new ConnectionData() { IP = ci.RemoteIP.ToString(), IsActive = true });
                                Console.WriteLine("Found new valid connection for process {0} connecting to {1}.", ci.PID, ci.RemoteIP.ToString());
                            }
                        }
                    }
                }
            }

            process.Close();

            activeConnectionList = activeConnectionListNew;
            allConnectionList = allConnectionListNew;
        }

        internal static IPAddress parseIP(string value)
        {
            int portStart = value.LastIndexOf(':');
            string parsedVal = value.Substring(0, portStart);
            IPAddress ip;
            if (!IPAddress.TryParse(parsedVal, out ip))
            {
                ip = IPAddress.Any;
            }
            return ip;
        }

        internal static int parsePort(string value)
        {
            int portStart = value.LastIndexOf(':');
            string parsedVal = value.Substring(portStart + 1);
            int port = (parsedVal == "*") ? -1 : Convert.ToInt32(parsedVal);

            return port;
        }
    }
}
