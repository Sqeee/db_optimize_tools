namespace DB_Optimize.GUI.OtherForms
{
    partial class ChangeCollationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeCollationForm));
            this.comboBoxCollation = new System.Windows.Forms.ComboBox();
            this.labelCollation = new System.Windows.Forms.Label();
            this.buttonCollation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxCollation
            // 
            this.comboBoxCollation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCollation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCollation.FormattingEnabled = true;
            this.comboBoxCollation.Location = new System.Drawing.Point(152, 12);
            this.comboBoxCollation.Name = "comboBoxCollation";
            this.comboBoxCollation.Size = new System.Drawing.Size(319, 24);
            this.comboBoxCollation.TabIndex = 0;
            // 
            // labelCollation
            // 
            this.labelCollation.AutoSize = true;
            this.labelCollation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCollation.Location = new System.Drawing.Point(12, 15);
            this.labelCollation.Name = "labelCollation";
            this.labelCollation.Size = new System.Drawing.Size(133, 17);
            this.labelCollation.TabIndex = 1;
            this.labelCollation.Text = "Choose collation:";
            // 
            // buttonCollation
            // 
            this.buttonCollation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCollation.Location = new System.Drawing.Point(15, 42);
            this.buttonCollation.Name = "buttonCollation";
            this.buttonCollation.Size = new System.Drawing.Size(458, 23);
            this.buttonCollation.TabIndex = 2;
            this.buttonCollation.Text = "Change collation of whole database and all objects";
            this.buttonCollation.UseVisualStyleBackColor = true;
            this.buttonCollation.Click += new System.EventHandler(this.buttonCollation_Click);
            // 
            // ChangeCollationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 78);
            this.Controls.Add(this.buttonCollation);
            this.Controls.Add(this.labelCollation);
            this.Controls.Add(this.comboBoxCollation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(501, 125);
            this.Name = "ChangeCollationForm";
            this.Text = "Change database collation";
            this.Load += new System.EventHandler(this.ChangeCollationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCollation;
        private System.Windows.Forms.Label labelCollation;
        private System.Windows.Forms.Button buttonCollation;
    }
}