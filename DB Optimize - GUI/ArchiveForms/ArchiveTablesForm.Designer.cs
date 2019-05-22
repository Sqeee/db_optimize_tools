namespace DB_Optimize.GUI.ArchiveForms
{
    partial class ArchiveTablesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveTablesForm));
            this.tabControlTables = new System.Windows.Forms.TabControl();
            this.buttonArchive = new System.Windows.Forms.Button();
            this.checkBoxOnlySave = new System.Windows.Forms.CheckBox();
            this.checkBoxInsertWithSelect = new System.Windows.Forms.CheckBox();
            this.saveSQLFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.flowLayoutPanelOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxSaveAllEntries = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateTable = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlTables
            // 
            this.tabControlTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlTables.Location = new System.Drawing.Point(13, 13);
            this.tabControlTables.Name = "tabControlTables";
            this.tabControlTables.SelectedIndex = 0;
            this.tabControlTables.Size = new System.Drawing.Size(842, 392);
            this.tabControlTables.TabIndex = 0;
            // 
            // buttonArchive
            // 
            this.buttonArchive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonArchive.Location = new System.Drawing.Point(12, 439);
            this.buttonArchive.Name = "buttonArchive";
            this.buttonArchive.Size = new System.Drawing.Size(843, 23);
            this.buttonArchive.TabIndex = 1;
            this.buttonArchive.Text = "Archive";
            this.buttonArchive.UseVisualStyleBackColor = true;
            this.buttonArchive.Click += new System.EventHandler(this.buttonArchive_Click);
            // 
            // checkBoxOnlySave
            // 
            this.checkBoxOnlySave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxOnlySave.AutoSize = true;
            this.checkBoxOnlySave.Location = new System.Drawing.Point(3, 3);
            this.checkBoxOnlySave.Name = "checkBoxOnlySave";
            this.checkBoxOnlySave.Size = new System.Drawing.Size(131, 21);
            this.checkBoxOnlySave.TabIndex = 2;
            this.checkBoxOnlySave.Text = "Only save to file";
            this.checkBoxOnlySave.UseVisualStyleBackColor = true;
            this.checkBoxOnlySave.CheckedChanged += new System.EventHandler(this.checkBoxOnlySave_CheckedChanged);
            // 
            // checkBoxInsertWithSelect
            // 
            this.checkBoxInsertWithSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxInsertWithSelect.AutoSize = true;
            this.checkBoxInsertWithSelect.Location = new System.Drawing.Point(140, 3);
            this.checkBoxInsertWithSelect.Name = "checkBoxInsertWithSelect";
            this.checkBoxInsertWithSelect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxInsertWithSelect.Size = new System.Drawing.Size(163, 21);
            this.checkBoxInsertWithSelect.TabIndex = 3;
            this.checkBoxInsertWithSelect.Text = "Use insert with select";
            this.checkBoxInsertWithSelect.UseVisualStyleBackColor = true;
            // 
            // saveSQLFileDialog
            // 
            this.saveSQLFileDialog.DefaultExt = "sql";
            this.saveSQLFileDialog.Filter = "All files|*.sql";
            this.saveSQLFileDialog.Title = "Save SQL file";
            // 
            // flowLayoutPanelOptions
            // 
            this.flowLayoutPanelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelOptions.Controls.Add(this.checkBoxOnlySave);
            this.flowLayoutPanelOptions.Controls.Add(this.checkBoxInsertWithSelect);
            this.flowLayoutPanelOptions.Controls.Add(this.checkBoxSaveAllEntries);
            this.flowLayoutPanelOptions.Controls.Add(this.checkBoxCreateTable);
            this.flowLayoutPanelOptions.Location = new System.Drawing.Point(12, 411);
            this.flowLayoutPanelOptions.Name = "flowLayoutPanelOptions";
            this.flowLayoutPanelOptions.Size = new System.Drawing.Size(843, 30);
            this.flowLayoutPanelOptions.TabIndex = 4;
            // 
            // checkBoxSaveAllEntries
            // 
            this.checkBoxSaveAllEntries.AutoSize = true;
            this.checkBoxSaveAllEntries.Location = new System.Drawing.Point(309, 3);
            this.checkBoxSaveAllEntries.Name = "checkBoxSaveAllEntries";
            this.checkBoxSaveAllEntries.Size = new System.Drawing.Size(256, 21);
            this.checkBoxSaveAllEntries.TabIndex = 4;
            this.checkBoxSaveAllEntries.Text = "Save all entries in dependent tables";
            this.checkBoxSaveAllEntries.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateTable
            // 
            this.checkBoxCreateTable.AutoSize = true;
            this.checkBoxCreateTable.Location = new System.Drawing.Point(571, 3);
            this.checkBoxCreateTable.Name = "checkBoxCreateTable";
            this.checkBoxCreateTable.Size = new System.Drawing.Size(219, 21);
            this.checkBoxCreateTable.TabIndex = 5;
            this.checkBoxCreateTable.Text = "Include create table command";
            this.checkBoxCreateTable.UseVisualStyleBackColor = true;
            // 
            // ArchiveTablesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 474);
            this.Controls.Add(this.flowLayoutPanelOptions);
            this.Controls.Add(this.buttonArchive);
            this.Controls.Add(this.tabControlTables);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(670, 200);
            this.Name = "ArchiveTablesForm";
            this.Text = "Archive database entries";
            this.Load += new System.EventHandler(this.ArchiveTablesForm_Load);
            this.flowLayoutPanelOptions.ResumeLayout(false);
            this.flowLayoutPanelOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlTables;
        private System.Windows.Forms.Button buttonArchive;
        private System.Windows.Forms.CheckBox checkBoxOnlySave;
        private System.Windows.Forms.CheckBox checkBoxInsertWithSelect;
        private System.Windows.Forms.SaveFileDialog saveSQLFileDialog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOptions;
        private System.Windows.Forms.CheckBox checkBoxSaveAllEntries;
        private System.Windows.Forms.CheckBox checkBoxCreateTable;
    }
}