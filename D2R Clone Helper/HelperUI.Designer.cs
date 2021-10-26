
namespace D2R_Clone_Helper
{
    partial class HelperUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelperUI));
            this.connectionListView = new System.Windows.Forms.ListView();
            this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Notes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IsActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IsBlocked = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.filterLabel = new System.Windows.Forms.Label();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.blockButton = new System.Windows.Forms.Button();
            this.notesLabel = new System.Windows.Forms.Label();
            this.unblockButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.legalTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // connectionListView
            // 
            this.connectionListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IP,
            this.Notes,
            this.IsActive,
            this.IsBlocked});
            this.connectionListView.FullRowSelect = true;
            this.connectionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.connectionListView.HideSelection = false;
            this.connectionListView.Location = new System.Drawing.Point(12, 38);
            this.connectionListView.Name = "connectionListView";
            this.connectionListView.Size = new System.Drawing.Size(592, 433);
            this.connectionListView.TabIndex = 0;
            this.connectionListView.UseCompatibleStateImageBehavior = false;
            this.connectionListView.View = System.Windows.Forms.View.Details;
            this.connectionListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // IP
            // 
            this.IP.Text = "IP";
            this.IP.Width = 119;
            // 
            // Notes
            // 
            this.Notes.Text = "Notes";
            this.Notes.Width = 281;
            // 
            // IsActive
            // 
            this.IsActive.Text = "Is active";
            this.IsActive.Width = 75;
            // 
            // IsBlocked
            // 
            this.IsBlocked.Text = "Is blocked";
            this.IsBlocked.Width = 73;
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(12, 15);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(170, 13);
            this.filterLabel.TabIndex = 1;
            this.filterLabel.Text = "Filter by IP/Notes (supports regex):";
            // 
            // filterTextBox
            // 
            this.filterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTextBox.Location = new System.Drawing.Point(188, 12);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(416, 20);
            this.filterTextBox.TabIndex = 2;
            // 
            // descTextBox
            // 
            this.descTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descTextBox.Enabled = false;
            this.descTextBox.Location = new System.Drawing.Point(610, 112);
            this.descTextBox.Multiline = true;
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(277, 291);
            this.descTextBox.TabIndex = 3;
            this.descTextBox.TextChanged += new System.EventHandler(this.descTextBox_TextChanged);
            // 
            // blockButton
            // 
            this.blockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blockButton.Enabled = false;
            this.blockButton.Location = new System.Drawing.Point(610, 38);
            this.blockButton.Name = "blockButton";
            this.blockButton.Size = new System.Drawing.Size(277, 23);
            this.blockButton.TabIndex = 5;
            this.blockButton.Text = "Block selected";
            this.blockButton.UseVisualStyleBackColor = true;
            this.blockButton.Click += new System.EventHandler(this.blockButton_Click);
            // 
            // notesLabel
            // 
            this.notesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(610, 96);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(107, 13);
            this.notesLabel.TabIndex = 6;
            this.notesLabel.Text = "Notes (edit selected):";
            // 
            // unblockButton
            // 
            this.unblockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unblockButton.Enabled = false;
            this.unblockButton.Location = new System.Drawing.Point(610, 38);
            this.unblockButton.Name = "unblockButton";
            this.unblockButton.Size = new System.Drawing.Size(277, 23);
            this.unblockButton.TabIndex = 7;
            this.unblockButton.Text = "Unblock selected";
            this.unblockButton.UseVisualStyleBackColor = true;
            this.unblockButton.Click += new System.EventHandler(this.unblockButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(610, 67);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(277, 23);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete selected";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(610, 448);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(277, 23);
            this.addButton.TabIndex = 9;
            this.addButton.Text = "Add IP";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // ipLabel
            // 
            this.ipLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(610, 406);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(20, 13);
            this.ipLabel.TabIndex = 10;
            this.ipLabel.Text = "IP:";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ipTextBox.Location = new System.Drawing.Point(610, 422);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(277, 20);
            this.ipTextBox.TabIndex = 11;
            this.ipTextBox.Click += new System.EventHandler(this.ipTextBox_Click);
            // 
            // legalTextBox
            // 
            this.legalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.legalTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.legalTextBox.Location = new System.Drawing.Point(12, 477);
            this.legalTextBox.Multiline = true;
            this.legalTextBox.Name = "legalTextBox";
            this.legalTextBox.ReadOnly = true;
            this.legalTextBox.Size = new System.Drawing.Size(875, 61);
            this.legalTextBox.TabIndex = 13;
            this.legalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HelperUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 550);
            this.Controls.Add(this.legalTextBox);
            this.Controls.Add(this.blockButton);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.unblockButton);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.descTextBox);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.connectionListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(915, 550);
            this.Name = "HelperUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "D2R Clone Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HelperUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView connectionListView;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.ColumnHeader Notes;
        private System.Windows.Forms.ColumnHeader IsActive;
        private System.Windows.Forms.ColumnHeader IsBlocked;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.TextBox descTextBox;
        private System.Windows.Forms.Button blockButton;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.Button unblockButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox legalTextBox;
    }
}