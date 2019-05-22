namespace DB_Optimize.GUI.HelperControls
{
    partial class MessageBoxWithTextBox
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
            this.buttonPrimary = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.richTextBoxText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonPrimary
            // 
            this.buttonPrimary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrimary.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonPrimary.Location = new System.Drawing.Point(12, 297);
            this.buttonPrimary.Name = "buttonPrimary";
            this.buttonPrimary.Size = new System.Drawing.Size(493, 23);
            this.buttonPrimary.TabIndex = 0;
            this.buttonPrimary.Text = "buttonPrimary";
            this.buttonPrimary.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 326);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(493, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // richTextBoxText
            // 
            this.richTextBoxText.AcceptsTab = true;
            this.richTextBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxText.DetectUrls = false;
            this.richTextBoxText.Location = new System.Drawing.Point(13, 13);
            this.richTextBoxText.Name = "richTextBoxText";
            this.richTextBoxText.Size = new System.Drawing.Size(492, 278);
            this.richTextBoxText.TabIndex = 3;
            this.richTextBoxText.Text = "";
            // 
            // MessageBoxWithTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 361);
            this.Controls.Add(this.richTextBoxText);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonPrimary);
            this.Name = "MessageBoxWithTextBox";
            this.Text = "MessageBoxWithTextBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPrimary;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.RichTextBox richTextBoxText;
    }
}