namespace DB_Optimize.GUI.ColumnForms
{
    partial class TableColumnsTabPage
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
            this.labelColumns = new System.Windows.Forms.Label();
            this.tabControlColumns = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // labelColumns
            // 
            this.labelColumns.AutoSize = true;
            this.labelColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelColumns.Location = new System.Drawing.Point(4, 4);
            this.labelColumns.Name = "labelColumns";
            this.labelColumns.Size = new System.Drawing.Size(74, 17);
            this.labelColumns.TabIndex = 0;
            this.labelColumns.Text = "Columns:";
            // 
            // tabControlColumns
            // 
            this.tabControlColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlColumns.Location = new System.Drawing.Point(7, 25);
            this.tabControlColumns.Name = "tabControlColumns";
            this.tabControlColumns.SelectedIndex = 0;
            this.tabControlColumns.Size = new System.Drawing.Size(348, 190);
            this.tabControlColumns.TabIndex = 1;
            // 
            // TableColumnsTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControlColumns);
            this.Controls.Add(this.labelColumns);
            this.Name = "TableColumnsTabPage";
            this.Size = new System.Drawing.Size(358, 218);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelColumns;
        private System.Windows.Forms.TabControl tabControlColumns;
    }
}
