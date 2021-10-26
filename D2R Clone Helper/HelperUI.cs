using ConnectionManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFirewallHelper;

namespace D2R_Clone_Helper
{
    internal partial class HelperUI : Form
    {
        internal string appAlias = "D2RCloneHelper";

        internal bool guiActive = false;
        internal bool exitProgram = false;

        private ListViewColumnSorter lvwColumnSorter;

        internal HelperUI()
        {
            InitializeComponent();

            legalTextBox.Text =
                "©2001 Blizzard Entertainment, Inc. All rights reserved. Diablo, " +
                "Lord of Destruction and Blizzard Entertainment are trademarks or " +
                "registered trademarks of Blizzard Entertainment, Inc. in the U.S. " +
                "and/or other countries." + Environment.NewLine +
                "This application and its developer are in no way affiliated with Blizzard Entertainment, Inc.." + Environment.NewLine +
                "Libraries used: WindowsFirewallHelper (https://github.com/falahati/WindowsFirewallHelper)";

            lvwColumnSorter = new ListViewColumnSorter();
            connectionListView.ListViewItemSorter = lvwColumnSorter;

            guiActive = true;
        }

        internal void updateDataSource(Dictionary<string, Detector.ConnectionData> connectionList)
        {
            bool sort = false;
            string filter = filterTextBox.Text;
            Regex rgx = new Regex("");

            try
            {
                rgx = new Regex(filterTextBox.Text);
            }
            catch {
                filter = String.Empty;
            }

            // Remove items that does not match regex
            foreach (ListViewItem item in connectionListView.Items)
            {
                if (rgx.IsMatch(item.SubItems[0].Text) || rgx.IsMatch(item.SubItems[1].Text) || filter == String.Empty)
                {
                    // Filter matched, this item should stay
                }
                else
                {
                    connectionListView.Items.RemoveByKey(item.Text);
                }
            }

            // Add or update existing items
            foreach (KeyValuePair<string, Detector.ConnectionData> kvp in connectionList)
            {
                Detector.ConnectionData data = kvp.Value;

                if (rgx.IsMatch(data.IP) || rgx.IsMatch(data.Notes) || filter == String.Empty)
                {
                    if (connectionListView.Items.ContainsKey(data.IP))
                    {
                        // Check for update
                        Color color;
                        int itemIndex = connectionListView.Items.IndexOfKey(data.IP);

                        if (connectionListView.Items[itemIndex].SubItems[0].Text != data.IP)
                        {
                            connectionListView.Items[itemIndex].SubItems[0].Text = data.IP;
                        }
                        if (connectionListView.Items[itemIndex].SubItems[1].Text != data.Notes)
                        {
                            connectionListView.Items[itemIndex].SubItems[1].Text = data.Notes;
                        }
                        if (connectionListView.Items[itemIndex].SubItems[2].Text != data.IsActive.ToString())
                        {
                            connectionListView.Items[itemIndex].SubItems[2].Text = data.IsActive.ToString();

                            if (data.IsActive)
                            {
                                color = Color.LightGreen;
                            }
                            else
                            {
                                color = Color.LightPink;
                            }
                            connectionListView.Items[itemIndex].UseItemStyleForSubItems = false;
                            if (connectionListView.Items[itemIndex].SubItems[2].BackColor != color)
                            {
                                connectionListView.Items[itemIndex].SubItems[2].BackColor = color;
                            }
                        }
                        if (connectionListView.Items[itemIndex].SubItems[3].Text != data.IsBlocked.ToString())
                        {
                            connectionListView.Items[itemIndex].SubItems[3].Text = data.IsBlocked.ToString();

                            if (data.IsBlocked)
                            {
                                color = Color.LightPink;
                            }
                            else
                            {
                                color = Color.LightGreen;
                            }
                            connectionListView.Items[itemIndex].UseItemStyleForSubItems = false;
                            if (connectionListView.Items[itemIndex].SubItems[3].BackColor != color)
                            {
                                connectionListView.Items[itemIndex].SubItems[3].BackColor = color;
                            }
                        }

                    }
                    else
                    {
                        // Add entry
                        ListViewItem listViewItem = new ListViewItem(new string[] { data.IP, data.Notes, String.Empty, String.Empty } );
                        listViewItem.Name = data.IP;
                        connectionListView.Items.Add(listViewItem);

                        sort = true;
                    }
                }
            }

            // Remove deleted items
            foreach (ListViewItem item in connectionListView.Items)
            {
                if (!connectionList.ContainsKey(item.Name))
                {
                    connectionListView.Items.RemoveByKey(item.Name);

                    sort = true;
                }
            }

            lvwColumnSorter.SortColumn = 0;
            lvwColumnSorter.Order = SortOrder.Ascending;

            if (sort)
            {
                connectionListView.Sort();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            int index = connectionListView.SelectedItems[0].Index;
        }

        private Detector.ConnectionData getItemAsConnData(int index)
        {
            Detector.ConnectionData connection = new Detector.ConnectionData()
            {
                IP = connectionListView.Items[index].SubItems[0].Text,
                Notes = connectionListView.Items[index].SubItems[1].Text,
                IsActive = Convert.ToBoolean(connectionListView.Items[index].SubItems[2].Text),
                IsBlocked = Convert.ToBoolean(connectionListView.Items[index].SubItems[3].Text)
            };

            return connection;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = -1;

            try
            {
                index = connectionListView.SelectedItems[0].Index;
            }
            catch { }

            if (index == -1)
            {
                unblockButton.Enabled = false;
                blockButton.Enabled = false;
                deleteButton.Enabled = false;
                descTextBox.Enabled = false;
                blockButton.Visible = true;
                unblockButton.Visible = false;

                descTextBox.Text = String.Empty;
            }
            else
            {
                unblockButton.Enabled = true;
                blockButton.Enabled = true;
                descTextBox.Enabled = true;

                Detector.ConnectionData connection = getItemAsConnData(index);

                if (connection.IsBlocked)
                {
                    blockButton.Visible = false;
                    unblockButton.Visible = true;
                }
                else
                {
                    blockButton.Visible = true;
                    unblockButton.Visible = false;
                }

                if (connection.IsBlocked || connection.IsActive)
                {
                    deleteButton.Enabled = false;
                }
                else
                {
                    deleteButton.Enabled = true;
                }

                descTextBox.Text = connection.Notes;
            }
        }

        private void descTextBox_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < connectionListView.SelectedItems.Count; i++)
            {
                int index = connectionListView.SelectedItems[i].Index;

                Detector.ConnectionData connection = getItemAsConnData(index);

                if (Detector.connectionList.ContainsKey(connection.IP))
                {
                    Detector.connectionList[connection.IP].Notes = descTextBox.Text.Trim();
                }

                break;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            bool notify = false;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            string message = String.Empty;
            IPAddress ip;
            string ipText = ipTextBox.Text.Trim();

            if (ipTextBox.Text != String.Empty && IPAddress.TryParse(ipText, out ip))
            {
                if (
                       !ip.Equals(IPAddress.Parse("0.0.0.0"))
                    && !ip.Equals(IPAddress.Parse("[::]"))
                    && !ip.Equals(IPAddress.Parse("127.0.0.1"))
                )
                {
                    if (!Detector.connectionList.ContainsKey(ipText))
                    {
                        Detector.connectionList.Add(ipTextBox.Text, new Detector.ConnectionData() { IP = ipText });
                        ipTextBox.Text = String.Empty;
                    }
                    else
                    {
                        notify = true;
                        icon = MessageBoxIcon.Information;
                        message = "IP already exists in the list.";
                    }
                }
                else
                {
                    notify = true;
                    icon = MessageBoxIcon.Error;
                    message = "Provided IP address not allowed.";
                }
            }
            else
            {
                notify = true;
                icon = MessageBoxIcon.Error;
                message = "Invalid IP address.";
            }

            if (notify)
            {
                MessageBox.Show(message, "Add action", MessageBoxButtons.OK, icon);

            }
        }

        private void blockButton_Click(object sender, EventArgs e)
        {
            IPAddress ip;

            for (int i = 0; i < connectionListView.SelectedItems.Count; i++)
            {
                int index = connectionListView.SelectedItems[i].Index;

                Detector.ConnectionData connection = getItemAsConnData(index);

                if (IPAddress.TryParse(connection.IP, out ip))
                {
                    blockButton.Visible = false;
                    unblockButton.Visible = true;
                    deleteButton.Enabled = false;

                    WindowsFirewallHelper.Addresses.SingleIP singleIP = new WindowsFirewallHelper.Addresses.SingleIP(ip.GetAddressBytes());
                    IAddress[] addressList = new IAddress[] { singleIP };
                    WindowsFirewallHelper.FirewallRules.FirewallWASRuleWin8 rule = new WindowsFirewallHelper.FirewallRules.FirewallWASRuleWin8(
                        appAlias + " - " + connection.IP,
                        FirewallAction.Block,
                        FirewallDirection.Outbound | FirewallDirection.Inbound,
                        FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public
                    )
                    {
                        Description = appAlias + " IP block",
                        RemoteAddresses = addressList
                    };
                    FirewallManager.Instance.Rules.Add(rule);
                    Disconnecter.CloseRemoteIP(connection.IP);
                }
            }
        }

        private void unblockButton_Click(object sender, EventArgs e)
        {
            IPAddress ip;

            for (int i = 0; i < connectionListView.SelectedItems.Count; i++)
            {
                int index = connectionListView.SelectedItems[i].Index;

                Detector.ConnectionData connection = getItemAsConnData(index);

                if (IPAddress.TryParse(connection.IP, out ip))
                {
                    if (Detector.blockedConnectionList.ContainsKey(connection.IP))
                    {
                        blockButton.Visible = true;
                        unblockButton.Visible = false;
                        deleteButton.Enabled = true;

                        FirewallManager.Instance.Rules.Remove(Detector.blockedConnectionList[connection.IP]);
                    }
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < connectionListView.SelectedItems.Count; i++)
            {
                int index = connectionListView.SelectedItems[i].Index;

                Detector.ConnectionData connection = getItemAsConnData(index);

                if (Detector.connectionList.ContainsKey(connection.IP) && !connection.IsBlocked && !connection.IsActive)
                {
                    Detector.connectionList.Remove(connection.IP);
                }
            }
        }

        private void HelperUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitProgram = true;
        }

        private void ipTextBox_Click(object sender, EventArgs e)
        {
            connectionListView.SelectedItems.Clear();
        }
    }
}