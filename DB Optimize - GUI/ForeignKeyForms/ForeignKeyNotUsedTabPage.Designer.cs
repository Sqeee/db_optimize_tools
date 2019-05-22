namespace DB_Optimize.GUI.ForeignKeyForms
{
    partial class ForeignKeyNotUsedTabPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewForeignKeys = new System.Windows.Forms.DataGridView();
            this.labelEmpty = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelValues = new System.Windows.Forms.Label();
            this.numericUpDownLoadValues = new System.Windows.Forms.NumericUpDown();
            this.buttonLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForeignKeys)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLoadValues)).BeginInit();
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
            this.dataGridViewForeignKeys.Location = new System.Drawing.Point(6, 59);
            this.dataGridViewForeignKeys.Name = "dataGridViewForeignKeys";
            this.dataGridViewForeignKeys.ReadOnly = true;
            this.dataGridViewForeignKeys.RowHeadersVisible = false;
            this.dataGridViewForeignKeys.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewForeignKeys.RowTemplate.Height = 24;
            this.dataGridViewForeignKeys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewForeignKeys.Size = new System.Drawing.Size(363, 123);
            this.dataGridViewForeignKeys.TabIndex = 2;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmpty.Location = new System.Drawing.Point(4, 59);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(364, 123);
            this.labelEmpty.TabIndex = 6;
            this.labelEmpty.Text = "Every value of foreign key is used";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonLoad);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDownLoadValues);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 22);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(365, 31);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // labelValues
            // 
            this.labelValues.AutoSize = true;
            this.labelValues.Location = new System.Drawing.Point(4, 2);
            this.labelValues.Name = "labelValues";
            this.labelValues.Size = new System.Drawing.Size(107, 17);
            this.labelValues.TabIndex = 0;
            this.labelValues.Text = "Total values {0}";
            // 
            // numericUpDownLoadValues
            // 
            this.numericUpDownLoadValues.Enabled = false;
            this.numericUpDownLoadValues.Location = new System.Drawing.Point(115, 4);
            this.numericUpDownLoadValues.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLoadValues.Name = "numericUpDownLoadValues";
            this.numericUpDownLoadValues.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownLoadValues.TabIndex = 1;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Enabled = false;
            this.buttonLoad.Location = new System.Drawing.Point(3, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(105, 23);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load values:";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // ForeignKeyNotUsedTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelValues);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.labelEmpty);
            this.Controls.Add(this.dataGridViewForeignKeys);
            this.Name = "ForeignKeyNotUsedTabPage";
            this.Size = new System.Drawing.Size(372, 185);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForeignKeys)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLoadValues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewForeignKeys;
        private System.Windows.Forms.Label labelEmpty;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.NumericUpDown numericUpDownLoadValues;
        private System.Windows.Forms.Label labelValues;
    }
}
