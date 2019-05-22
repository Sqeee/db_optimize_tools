namespace DB_Optimize.GUI.MainForms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.checkedListBoxTables = new System.Windows.Forms.CheckedListBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.tableLayoutPanelSelection = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelTools = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDependenciesSelected = new System.Windows.Forms.Button();
            this.buttonArchiveEntries = new System.Windows.Forms.Button();
            this.labelAvailableTools = new System.Windows.Forms.Label();
            this.buttonDependencies = new System.Windows.Forms.Button();
            this.buttonForeignMissingIndexes = new System.Windows.Forms.Button();
            this.buttonForeignBadType = new System.Windows.Forms.Button();
            this.buttonColumnsOptimalType = new System.Windows.Forms.Button();
            this.buttonForeignIsUsed = new System.Windows.Forms.Button();
            this.buttonForeignCascadeDelete = new System.Windows.Forms.Button();
            this.buttonRecommendationIndexes = new System.Windows.Forms.Button();
            this.buttonChangeCollation = new System.Windows.Forms.Button();
            this.buttonDeleteEntries = new System.Windows.Forms.Button();
            this.tableLayoutPanelSelection.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanelTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxTables
            // 
            this.checkedListBoxTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxTables.CheckOnClick = true;
            this.checkedListBoxTables.FormattingEnabled = true;
            this.checkedListBoxTables.Location = new System.Drawing.Point(13, 83);
            this.checkedListBoxTables.Name = "checkedListBoxTables";
            this.checkedListBoxTables.ScrollAlwaysVisible = true;
            this.checkedListBoxTables.Size = new System.Drawing.Size(448, 514);
            this.checkedListBoxTables.Sorted = true;
            this.checkedListBoxTables.TabIndex = 0;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelectAll.Location = new System.Drawing.Point(3, 3);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(218, 40);
            this.buttonSelectAll.TabIndex = 1;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeselectAll.Location = new System.Drawing.Point(227, 3);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(218, 40);
            this.buttonDeselectAll.TabIndex = 2;
            this.buttonDeselectAll.Text = "Deselect all";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
            // 
            // tableLayoutPanelSelection
            // 
            this.tableLayoutPanelSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSelection.ColumnCount = 2;
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelection.Controls.Add(this.buttonSelectAll, 0, 0);
            this.tableLayoutPanelSelection.Controls.Add(this.buttonDeselectAll, 1, 0);
            this.tableLayoutPanelSelection.Location = new System.Drawing.Point(13, 31);
            this.tableLayoutPanelSelection.Name = "tableLayoutPanelSelection";
            this.tableLayoutPanelSelection.RowCount = 1;
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanelSelection.Size = new System.Drawing.Size(448, 46);
            this.tableLayoutPanelSelection.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(744, 28);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStripMenu";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemReconnect,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(44, 24);
            this.toolStripMenuItemFile.Text = "File";
            // 
            // toolStripMenuItemReconnect
            // 
            this.toolStripMenuItemReconnect.Name = "toolStripMenuItemReconnect";
            this.toolStripMenuItemReconnect.Size = new System.Drawing.Size(153, 26);
            this.toolStripMenuItemReconnect.Text = "Reconnect";
            this.toolStripMenuItemReconnect.Click += new System.EventHandler(this.toolStripMenuItemReconnect_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(153, 26);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout});
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(53, 24);
            this.toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(125, 26);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // tableLayoutPanelTools
            // 
            this.tableLayoutPanelTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelTools.ColumnCount = 1;
            this.tableLayoutPanelTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTools.Controls.Add(this.buttonDependenciesSelected, 0, 4);
            this.tableLayoutPanelTools.Controls.Add(this.buttonArchiveEntries, 0, 1);
            this.tableLayoutPanelTools.Controls.Add(this.labelAvailableTools, 0, 0);
            this.tableLayoutPanelTools.Controls.Add(this.buttonDependencies, 0, 3);
            this.tableLayoutPanelTools.Controls.Add(this.buttonForeignMissingIndexes, 0, 5);
            this.tableLayoutPanelTools.Controls.Add(this.buttonForeignBadType, 0, 6);
            this.tableLayoutPanelTools.Controls.Add(this.buttonColumnsOptimalType, 0, 7);
            this.tableLayoutPanelTools.Controls.Add(this.buttonForeignIsUsed, 0, 8);
            this.tableLayoutPanelTools.Controls.Add(this.buttonForeignCascadeDelete, 0, 9);
            this.tableLayoutPanelTools.Controls.Add(this.buttonRecommendationIndexes, 0, 10);
            this.tableLayoutPanelTools.Controls.Add(this.buttonChangeCollation, 0, 11);
            this.tableLayoutPanelTools.Controls.Add(this.buttonDeleteEntries, 0, 2);
            this.tableLayoutPanelTools.Location = new System.Drawing.Point(468, 31);
            this.tableLayoutPanelTools.Name = "tableLayoutPanelTools";
            this.tableLayoutPanelTools.RowCount = 13;
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelTools.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTools.Size = new System.Drawing.Size(264, 568);
            this.tableLayoutPanelTools.TabIndex = 5;
            // 
            // buttonDependenciesSelected
            // 
            this.buttonDependenciesSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDependenciesSelected.Location = new System.Drawing.Point(3, 167);
            this.buttonDependenciesSelected.Name = "buttonDependenciesSelected";
            this.buttonDependenciesSelected.Size = new System.Drawing.Size(258, 43);
            this.buttonDependenciesSelected.TabIndex = 12;
            this.buttonDependenciesSelected.Text = "Show dependencies (only between selected tables)";
            this.buttonDependenciesSelected.UseVisualStyleBackColor = true;
            this.buttonDependenciesSelected.Click += new System.EventHandler(this.buttonDependenciesSelected_Click);
            // 
            // buttonArchiveEntries
            // 
            this.buttonArchiveEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonArchiveEntries.Location = new System.Drawing.Point(3, 20);
            this.buttonArchiveEntries.Name = "buttonArchiveEntries";
            this.buttonArchiveEntries.Size = new System.Drawing.Size(258, 43);
            this.buttonArchiveEntries.TabIndex = 10;
            this.buttonArchiveEntries.Text = "Archive entries";
            this.buttonArchiveEntries.UseVisualStyleBackColor = true;
            this.buttonArchiveEntries.Click += new System.EventHandler(this.buttonArchiveEntries_Click);
            // 
            // labelAvailableTools
            // 
            this.labelAvailableTools.AutoSize = true;
            this.labelAvailableTools.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAvailableTools.Location = new System.Drawing.Point(3, 0);
            this.labelAvailableTools.Name = "labelAvailableTools";
            this.labelAvailableTools.Size = new System.Drawing.Size(119, 17);
            this.labelAvailableTools.TabIndex = 0;
            this.labelAvailableTools.Text = "Available tools:";
            // 
            // buttonDependencies
            // 
            this.buttonDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDependencies.Location = new System.Drawing.Point(3, 118);
            this.buttonDependencies.Name = "buttonDependencies";
            this.buttonDependencies.Size = new System.Drawing.Size(258, 43);
            this.buttonDependencies.TabIndex = 1;
            this.buttonDependencies.Text = "Show dependencies (all relevant to selected tables)";
            this.buttonDependencies.UseVisualStyleBackColor = true;
            this.buttonDependencies.Click += new System.EventHandler(this.buttonDependencies_Click);
            // 
            // buttonForeignMissingIndexes
            // 
            this.buttonForeignMissingIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonForeignMissingIndexes.Location = new System.Drawing.Point(3, 216);
            this.buttonForeignMissingIndexes.Name = "buttonForeignMissingIndexes";
            this.buttonForeignMissingIndexes.Size = new System.Drawing.Size(258, 43);
            this.buttonForeignMissingIndexes.TabIndex = 2;
            this.buttonForeignMissingIndexes.Text = "Check foreign keys for missing indexes";
            this.buttonForeignMissingIndexes.UseVisualStyleBackColor = true;
            this.buttonForeignMissingIndexes.Click += new System.EventHandler(this.buttonForeignMissingIndexes_Click);
            // 
            // buttonForeignBadType
            // 
            this.buttonForeignBadType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonForeignBadType.Location = new System.Drawing.Point(3, 265);
            this.buttonForeignBadType.Name = "buttonForeignBadType";
            this.buttonForeignBadType.Size = new System.Drawing.Size(258, 43);
            this.buttonForeignBadType.TabIndex = 3;
            this.buttonForeignBadType.Text = "Check foreign keys for bad data type";
            this.buttonForeignBadType.UseVisualStyleBackColor = true;
            this.buttonForeignBadType.Click += new System.EventHandler(this.buttonForeignBadType_Click);
            // 
            // buttonColumnsOptimalType
            // 
            this.buttonColumnsOptimalType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonColumnsOptimalType.Location = new System.Drawing.Point(3, 314);
            this.buttonColumnsOptimalType.Name = "buttonColumnsOptimalType";
            this.buttonColumnsOptimalType.Size = new System.Drawing.Size(258, 43);
            this.buttonColumnsOptimalType.TabIndex = 4;
            this.buttonColumnsOptimalType.Text = "Check columns for optimal data type";
            this.buttonColumnsOptimalType.UseVisualStyleBackColor = true;
            this.buttonColumnsOptimalType.Click += new System.EventHandler(this.buttonColumnsOptimalType_Click);
            // 
            // buttonForeignIsUsed
            // 
            this.buttonForeignIsUsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonForeignIsUsed.Location = new System.Drawing.Point(3, 363);
            this.buttonForeignIsUsed.Name = "buttonForeignIsUsed";
            this.buttonForeignIsUsed.Size = new System.Drawing.Size(258, 43);
            this.buttonForeignIsUsed.TabIndex = 5;
            this.buttonForeignIsUsed.Text = "Check values if they are used by parents of foreign keys";
            this.buttonForeignIsUsed.UseVisualStyleBackColor = true;
            this.buttonForeignIsUsed.Click += new System.EventHandler(this.buttonForeignIsUsed_Click);
            // 
            // buttonForeignCascadeDelete
            // 
            this.buttonForeignCascadeDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonForeignCascadeDelete.Location = new System.Drawing.Point(3, 412);
            this.buttonForeignCascadeDelete.Name = "buttonForeignCascadeDelete";
            this.buttonForeignCascadeDelete.Size = new System.Drawing.Size(258, 43);
            this.buttonForeignCascadeDelete.TabIndex = 6;
            this.buttonForeignCascadeDelete.Text = "Check table for foreign keys and delete actions";
            this.buttonForeignCascadeDelete.UseVisualStyleBackColor = true;
            this.buttonForeignCascadeDelete.Click += new System.EventHandler(this.buttonForeignCascadeDelete_Click);
            // 
            // buttonRecommendationIndexes
            // 
            this.buttonRecommendationIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRecommendationIndexes.Location = new System.Drawing.Point(3, 461);
            this.buttonRecommendationIndexes.Name = "buttonRecommendationIndexes";
            this.buttonRecommendationIndexes.Size = new System.Drawing.Size(258, 43);
            this.buttonRecommendationIndexes.TabIndex = 7;
            this.buttonRecommendationIndexes.Text = "Recommendation of indices based on server statistics";
            this.buttonRecommendationIndexes.UseVisualStyleBackColor = true;
            this.buttonRecommendationIndexes.Click += new System.EventHandler(this.buttonRecommendationIndexes_Click);
            // 
            // buttonChangeCollation
            // 
            this.buttonChangeCollation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonChangeCollation.Location = new System.Drawing.Point(3, 510);
            this.buttonChangeCollation.Name = "buttonChangeCollation";
            this.buttonChangeCollation.Size = new System.Drawing.Size(258, 43);
            this.buttonChangeCollation.TabIndex = 8;
            this.buttonChangeCollation.Text = "Change database collation";
            this.buttonChangeCollation.UseVisualStyleBackColor = true;
            this.buttonChangeCollation.Click += new System.EventHandler(this.buttonChangeCollation_Click);
            // 
            // buttonDeleteEntries
            // 
            this.buttonDeleteEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteEntries.Location = new System.Drawing.Point(3, 69);
            this.buttonDeleteEntries.Name = "buttonDeleteEntries";
            this.buttonDeleteEntries.Size = new System.Drawing.Size(258, 43);
            this.buttonDeleteEntries.TabIndex = 11;
            this.buttonDeleteEntries.Text = "Delete entries";
            this.buttonDeleteEntries.UseVisualStyleBackColor = true;
            this.buttonDeleteEntries.Click += new System.EventHandler(this.buttonDeleteEntries_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 615);
            this.Controls.Add(this.tableLayoutPanelTools);
            this.Controls.Add(this.tableLayoutPanelSelection);
            this.Controls.Add(this.checkedListBoxTables);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(18, 662);
            this.Name = "MainForm";
            this.Text = "DB Optimize Tools";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanelSelection.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanelTools.ResumeLayout(false);
            this.tableLayoutPanelTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxTables;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelection;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReconnect;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTools;
        private System.Windows.Forms.Label labelAvailableTools;
        private System.Windows.Forms.Button buttonDependencies;
        private System.Windows.Forms.Button buttonForeignMissingIndexes;
        private System.Windows.Forms.Button buttonForeignBadType;
        private System.Windows.Forms.Button buttonColumnsOptimalType;
        private System.Windows.Forms.Button buttonForeignIsUsed;
        private System.Windows.Forms.Button buttonArchiveEntries;
        private System.Windows.Forms.Button buttonForeignCascadeDelete;
        private System.Windows.Forms.Button buttonRecommendationIndexes;
        private System.Windows.Forms.Button buttonChangeCollation;
        private System.Windows.Forms.Button buttonDeleteEntries;
        private System.Windows.Forms.Button buttonDependenciesSelected;
    }
}