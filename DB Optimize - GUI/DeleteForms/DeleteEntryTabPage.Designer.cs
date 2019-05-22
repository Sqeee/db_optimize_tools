using DB_Optimize.GUI.HelperControls;

namespace DB_Optimize.GUI.DeleteForms
{
    partial class DeleteEntryTabPage
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
            this.tableLayoutPanelPage = new System.Windows.Forms.TableLayoutPanel();
            this.labelFilter = new System.Windows.Forms.Label();
            this.filterConditionsPanel = new FilterConditionsPanel();
            this.flowLayoutPanelCopySetter = new System.Windows.Forms.FlowLayoutPanel();
            this.labelSetter = new System.Windows.Forms.Label();
            this.comboBoxDefaultAction = new System.Windows.Forms.ComboBox();
            this.buttonCopyAction = new System.Windows.Forms.Button();
            this.labelForeignKeys = new System.Windows.Forms.Label();
            this.tableLayoutPanelForeignKeys = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelPage.SuspendLayout();
            this.flowLayoutPanelCopySetter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPage
            // 
            this.tableLayoutPanelPage.AutoScroll = true;
            this.tableLayoutPanelPage.ColumnCount = 1;
            this.tableLayoutPanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPage.Controls.Add(this.labelFilter, 0, 0);
            this.tableLayoutPanelPage.Controls.Add(this.filterConditionsPanel, 0, 1);
            this.tableLayoutPanelPage.Controls.Add(this.flowLayoutPanelCopySetter, 0, 2);
            this.tableLayoutPanelPage.Controls.Add(this.labelForeignKeys, 0, 3);
            this.tableLayoutPanelPage.Controls.Add(this.tableLayoutPanelForeignKeys, 0, 4);
            this.tableLayoutPanelPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPage.Name = "tableLayoutPanelPage";
            this.tableLayoutPanelPage.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.tableLayoutPanelPage.RowCount = 6;
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.Size = new System.Drawing.Size(639, 281);
            this.tableLayoutPanelPage.TabIndex = 0;
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFilter.Location = new System.Drawing.Point(3, 0);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(151, 17);
            this.labelFilter.TabIndex = 1;
            this.labelFilter.Text = "Filtering conditions:";
            // 
            // filterConditionsPanel
            // 
            this.filterConditionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterConditionsPanel.AutoSize = true;
            this.filterConditionsPanel.Location = new System.Drawing.Point(0, 20);
            this.filterConditionsPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.filterConditionsPanel.Name = "filterConditionsPanel";
            this.filterConditionsPanel.Size = new System.Drawing.Size(632, 126);
            this.filterConditionsPanel.TabIndex = 8;
            // 
            // flowLayoutPanelCopySetter
            // 
            this.flowLayoutPanelCopySetter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelCopySetter.AutoSize = true;
            this.flowLayoutPanelCopySetter.Controls.Add(this.labelSetter);
            this.flowLayoutPanelCopySetter.Controls.Add(this.comboBoxDefaultAction);
            this.flowLayoutPanelCopySetter.Controls.Add(this.buttonCopyAction);
            this.flowLayoutPanelCopySetter.Location = new System.Drawing.Point(0, 149);
            this.flowLayoutPanelCopySetter.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelCopySetter.Name = "flowLayoutPanelCopySetter";
            this.flowLayoutPanelCopySetter.Size = new System.Drawing.Size(632, 30);
            this.flowLayoutPanelCopySetter.TabIndex = 9;
            // 
            // labelSetter
            // 
            this.labelSetter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSetter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSetter.Location = new System.Drawing.Point(3, 0);
            this.labelSetter.Name = "labelSetter";
            this.labelSetter.Size = new System.Drawing.Size(183, 30);
            this.labelSetter.TabIndex = 0;
            this.labelSetter.Text = "Default delete action:";
            this.labelSetter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxDefaultAction
            // 
            this.comboBoxDefaultAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDefaultAction.FormattingEnabled = true;
            this.comboBoxDefaultAction.Location = new System.Drawing.Point(192, 3);
            this.comboBoxDefaultAction.Name = "comboBoxDefaultAction";
            this.comboBoxDefaultAction.Size = new System.Drawing.Size(144, 24);
            this.comboBoxDefaultAction.TabIndex = 1;
            // 
            // buttonCopyAction
            // 
            this.buttonCopyAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyAction.Location = new System.Drawing.Point(342, 3);
            this.buttonCopyAction.Name = "buttonCopyAction";
            this.buttonCopyAction.Size = new System.Drawing.Size(287, 24);
            this.buttonCopyAction.TabIndex = 2;
            this.buttonCopyAction.Text = "Copy action to keys where it is possible";
            this.buttonCopyAction.UseVisualStyleBackColor = true;
            this.buttonCopyAction.Click += new System.EventHandler(this.buttonCopyAction_Click);
            // 
            // labelForeignKeys
            // 
            this.labelForeignKeys.AutoSize = true;
            this.labelForeignKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelForeignKeys.Location = new System.Drawing.Point(3, 179);
            this.labelForeignKeys.Name = "labelForeignKeys";
            this.labelForeignKeys.Size = new System.Drawing.Size(213, 17);
            this.labelForeignKeys.TabIndex = 10;
            this.labelForeignKeys.Text = "Foreign keys delete actions:";
            // 
            // tableLayoutPanelForeignKeys
            // 
            this.tableLayoutPanelForeignKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelForeignKeys.AutoSize = true;
            this.tableLayoutPanelForeignKeys.ColumnCount = 2;
            this.tableLayoutPanelForeignKeys.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelForeignKeys.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelForeignKeys.Location = new System.Drawing.Point(0, 197);
            this.tableLayoutPanelForeignKeys.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelForeignKeys.Name = "tableLayoutPanelForeignKeys";
            this.tableLayoutPanelForeignKeys.RowCount = 1;
            this.tableLayoutPanelForeignKeys.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelForeignKeys.Size = new System.Drawing.Size(632, 0);
            this.tableLayoutPanelForeignKeys.TabIndex = 11;
            // 
            // DeleteEntryTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanelPage);
            this.Name = "DeleteEntryTabPage";
            this.Size = new System.Drawing.Size(639, 281);
            this.tableLayoutPanelPage.ResumeLayout(false);
            this.tableLayoutPanelPage.PerformLayout();
            this.flowLayoutPanelCopySetter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPage;
        private System.Windows.Forms.Label labelFilter;
        private FilterConditionsPanel filterConditionsPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCopySetter;
        private System.Windows.Forms.Label labelSetter;
        private System.Windows.Forms.ComboBox comboBoxDefaultAction;
        private System.Windows.Forms.Button buttonCopyAction;
        private System.Windows.Forms.Label labelForeignKeys;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelForeignKeys;
    }
}
