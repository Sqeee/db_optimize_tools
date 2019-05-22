namespace DB_Optimize.GUI.OtherForms
{
    partial class MissingIndexesStatisticsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingIndexesStatisticsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewMissingIndexes = new System.Windows.Forms.DataGridView();
            this.labelEmpty = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMissingIndexes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(474, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Statistics are automatically deleted by database server after server restart";
            // 
            // dataGridViewMissingIndexes
            // 
            this.dataGridViewMissingIndexes.AllowUserToAddRows = false;
            this.dataGridViewMissingIndexes.AllowUserToDeleteRows = false;
            this.dataGridViewMissingIndexes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMissingIndexes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewMissingIndexes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMissingIndexes.Location = new System.Drawing.Point(13, 34);
            this.dataGridViewMissingIndexes.Name = "dataGridViewMissingIndexes";
            this.dataGridViewMissingIndexes.ReadOnly = true;
            this.dataGridViewMissingIndexes.RowHeadersVisible = false;
            this.dataGridViewMissingIndexes.RowTemplate.Height = 24;
            this.dataGridViewMissingIndexes.Size = new System.Drawing.Size(950, 387);
            this.dataGridViewMissingIndexes.TabIndex = 1;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmpty.Location = new System.Drawing.Point(10, 34);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(953, 387);
            this.labelEmpty.TabIndex = 7;
            this.labelEmpty.Text = "For selected table is not any index recommendation";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MissingIndexesStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 433);
            this.Controls.Add(this.labelEmpty);
            this.Controls.Add(this.dataGridViewMissingIndexes);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(601, 392);
            this.Name = "MissingIndexesStatisticsForm";
            this.Text = "Recommedation of indexes based on server statistics";
            this.Load += new System.EventHandler(this.MissingIndexesStatisticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMissingIndexes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewMissingIndexes;
        private System.Windows.Forms.Label labelEmpty;
    }
}