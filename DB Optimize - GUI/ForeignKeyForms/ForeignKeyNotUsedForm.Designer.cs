namespace DB_Optimize.GUI.ForeignKeyForms
{
    partial class ForeignKeyNotUsedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForeignKeyNotUsedForm));
            this.tabControlForeignKeys = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlForeignKeys
            // 
            this.tabControlForeignKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlForeignKeys.Location = new System.Drawing.Point(13, 13);
            this.tabControlForeignKeys.Name = "tabControlForeignKeys";
            this.tabControlForeignKeys.SelectedIndex = 0;
            this.tabControlForeignKeys.Size = new System.Drawing.Size(743, 379);
            this.tabControlForeignKeys.TabIndex = 0;
            // 
            // ForeignKeyNotUsedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 404);
            this.Controls.Add(this.tabControlForeignKeys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ForeignKeyNotUsedForm";
            this.Text = "Check values if they are used by parents of foreign keys";
            this.Load += new System.EventHandler(this.ForeignKeyNotUsedForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlForeignKeys;
    }
}