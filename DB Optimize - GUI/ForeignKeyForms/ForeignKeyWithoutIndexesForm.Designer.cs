namespace DB_Optimize.GUI.ForeignKeyForms
{
    partial class ForeignKeyWithoutIndexesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForeignKeyWithoutIndexesForm));
            this.dataGridViewForeignKeys = new System.Windows.Forms.DataGridView();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.buttonCreateIndexFull = new System.Windows.Forms.Button();
            this.statusStripInfo = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelEmpty = new System.Windows.Forms.Label();
            this.buttonCreateIndexMissing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForeignKeys)).BeginInit();
            this.statusStripInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewForeignKeys
            // 
            this.dataGridViewForeignKeys.AllowUserToAddRows = false;
            this.dataGridViewForeignKeys.AllowUserToDeleteRows = false;
            this.dataGridViewForeignKeys.AllowUserToResizeRows = false;
            this.dataGridViewForeignKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewForeignKeys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewForeignKeys.Location = new System.Drawing.Point(12, 47);
            this.dataGridViewForeignKeys.Name = "dataGridViewForeignKeys";
            this.dataGridViewForeignKeys.ReadOnly = true;
            this.dataGridViewForeignKeys.RowHeadersVisible = false;
            this.dataGridViewForeignKeys.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewForeignKeys.RowTemplate.Height = 24;
            this.dataGridViewForeignKeys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewForeignKeys.Size = new System.Drawing.Size(1122, 392);
            this.dataGridViewForeignKeys.TabIndex = 0;
            this.dataGridViewForeignKeys.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewForeignKeys_CellClick);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(12, 12);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(145, 29);
            this.buttonSelectAll.TabIndex = 1;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(163, 12);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(145, 29);
            this.buttonDeselectAll.TabIndex = 2;
            this.buttonDeselectAll.Text = "Deselect all";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
            // 
            // buttonCreateIndexFull
            // 
            this.buttonCreateIndexFull.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateIndexFull.Enabled = false;
            this.buttonCreateIndexFull.Location = new System.Drawing.Point(12, 480);
            this.buttonCreateIndexFull.Name = "buttonCreateIndexFull";
            this.buttonCreateIndexFull.Size = new System.Drawing.Size(1123, 29);
            this.buttonCreateIndexFull.TabIndex = 3;
            this.buttonCreateIndexFull.Text = "Create indexes for selected foreign keys (all columns)";
            this.buttonCreateIndexFull.UseVisualStyleBackColor = true;
            this.buttonCreateIndexFull.Click += new System.EventHandler(this.buttonCreateIndexFull_Click);
            // 
            // statusStripInfo
            // 
            this.statusStripInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelInfo});
            this.statusStripInfo.Location = new System.Drawing.Point(0, 512);
            this.statusStripInfo.Name = "statusStripInfo";
            this.statusStripInfo.Size = new System.Drawing.Size(1147, 25);
            this.statusStripInfo.TabIndex = 4;
            this.statusStripInfo.Text = "statusStripInfo";
            // 
            // toolStripStatusLabelInfo
            // 
            this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
            this.toolStripStatusLabelInfo.Size = new System.Drawing.Size(169, 20);
            this.toolStripStatusLabelInfo.Text = "toolStripStatusLabelInfo";
            // 
            // labelEmpty
            // 
            this.labelEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmpty.Location = new System.Drawing.Point(12, 44);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(1123, 395);
            this.labelEmpty.TabIndex = 5;
            this.labelEmpty.Text = "Every foreign key has index.";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelEmpty.Visible = false;
            // 
            // buttonCreateIndexMissing
            // 
            this.buttonCreateIndexMissing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateIndexMissing.Enabled = false;
            this.buttonCreateIndexMissing.Location = new System.Drawing.Point(12, 445);
            this.buttonCreateIndexMissing.Name = "buttonCreateIndexMissing";
            this.buttonCreateIndexMissing.Size = new System.Drawing.Size(1123, 29);
            this.buttonCreateIndexMissing.TabIndex = 6;
            this.buttonCreateIndexMissing.Text = "Create indexes for selected foreign keys (only for columns missing in indexes)";
            this.buttonCreateIndexMissing.UseVisualStyleBackColor = true;
            this.buttonCreateIndexMissing.Click += new System.EventHandler(this.buttonCreateIndexMissing_Click);
            // 
            // ForeignKeyWithoutIndexesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 537);
            this.Controls.Add(this.buttonCreateIndexMissing);
            this.Controls.Add(this.labelEmpty);
            this.Controls.Add(this.statusStripInfo);
            this.Controls.Add(this.buttonCreateIndexFull);
            this.Controls.Add(this.buttonDeselectAll);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.dataGridViewForeignKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ForeignKeyWithoutIndexesForm";
            this.Text = "Foreign keys without indexes";
            this.Load += new System.EventHandler(this.ForeignKeyWithoutIndexes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForeignKeys)).EndInit();
            this.statusStripInfo.ResumeLayout(false);
            this.statusStripInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewForeignKeys;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Button buttonCreateIndexFull;
        private System.Windows.Forms.StatusStrip statusStripInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInfo;
        private System.Windows.Forms.Label labelEmpty;
        private System.Windows.Forms.Button buttonCreateIndexMissing;
    }
}