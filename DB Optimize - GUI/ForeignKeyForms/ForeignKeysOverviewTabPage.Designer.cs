namespace DB_Optimize.GUI.ForeignKeyForms
{
    partial class ForeignKeysOverviewTabPage
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
            this.labelStatusText = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelForeignKeys = new System.Windows.Forms.Label();
            this.flowLayoutPanelForeignKeys = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // labelStatusText
            // 
            this.labelStatusText.AutoSize = true;
            this.labelStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelStatusText.Location = new System.Drawing.Point(4, 4);
            this.labelStatusText.Name = "labelStatusText";
            this.labelStatusText.Size = new System.Drawing.Size(64, 17);
            this.labelStatusText.TabIndex = 0;
            this.labelStatusText.Text = "Status: ";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.labelStatus.Location = new System.Drawing.Point(74, 4);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(30, 17);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "OK";
            // 
            // labelForeignKeys
            // 
            this.labelForeignKeys.AutoSize = true;
            this.labelForeignKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelForeignKeys.Location = new System.Drawing.Point(4, 30);
            this.labelForeignKeys.Name = "labelForeignKeys";
            this.labelForeignKeys.Size = new System.Drawing.Size(179, 17);
            this.labelForeignKeys.TabIndex = 2;
            this.labelForeignKeys.Text = "Foreign keys hierarchy:";
            // 
            // flowLayoutPanelForeignKeys
            // 
            this.flowLayoutPanelForeignKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelForeignKeys.AutoSize = true;
            this.flowLayoutPanelForeignKeys.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelForeignKeys.Location = new System.Drawing.Point(7, 51);
            this.flowLayoutPanelForeignKeys.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelForeignKeys.Name = "flowLayoutPanelForeignKeys";
            this.flowLayoutPanelForeignKeys.Size = new System.Drawing.Size(504, 0);
            this.flowLayoutPanelForeignKeys.TabIndex = 3;
            // 
            // ForeignKeysOverviewTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.flowLayoutPanelForeignKeys);
            this.Controls.Add(this.labelForeignKeys);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelStatusText);
            this.Name = "ForeignKeysOverviewTabPage";
            this.Size = new System.Drawing.Size(514, 334);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatusText;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelForeignKeys;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelForeignKeys;
    }
}
