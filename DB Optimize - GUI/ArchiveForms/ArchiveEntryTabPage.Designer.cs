using DB_Optimize.GUI.HelperControls;

namespace DB_Optimize.GUI.ArchiveForms
{
    partial class ArchiveEntryTabPage
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
            this.labelTablenames = new System.Windows.Forms.Label();
            this.tableLayoutPanelTablesnames = new System.Windows.Forms.TableLayoutPanel();
            this.filterConditionsPanel = new FilterConditionsPanel();
            this.tableLayoutPanelPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPage
            // 
            this.tableLayoutPanelPage.AutoScroll = true;
            this.tableLayoutPanelPage.ColumnCount = 1;
            this.tableLayoutPanelPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPage.Controls.Add(this.labelFilter, 0, 0);
            this.tableLayoutPanelPage.Controls.Add(this.filterConditionsPanel, 0, 1);
            this.tableLayoutPanelPage.Controls.Add(this.labelTablenames, 0, 2);
            this.tableLayoutPanelPage.Controls.Add(this.tableLayoutPanelTablesnames, 0, 3);
            this.tableLayoutPanelPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPage.Name = "tableLayoutPanelPage";
            this.tableLayoutPanelPage.Padding = new System.Windows.Forms.Padding(0, 0, 7, 0);
            this.tableLayoutPanelPage.RowCount = 5;
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPage.Size = new System.Drawing.Size(398, 281);
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
            // labelTablenames
            // 
            this.labelTablenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTablenames.AutoSize = true;
            this.labelTablenames.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTablenames.Location = new System.Drawing.Point(3, 149);
            this.labelTablenames.Name = "labelTablenames";
            this.labelTablenames.Size = new System.Drawing.Size(131, 17);
            this.labelTablenames.TabIndex = 6;
            this.labelTablenames.Text = "New tablenames:";
            // 
            // tableLayoutPanelTablesnames
            // 
            this.tableLayoutPanelTablesnames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelTablesnames.AutoSize = true;
            this.tableLayoutPanelTablesnames.ColumnCount = 4;
            this.tableLayoutPanelTablesnames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTablesnames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTablesnames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTablesnames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTablesnames.Location = new System.Drawing.Point(0, 167);
            this.tableLayoutPanelTablesnames.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTablesnames.Name = "tableLayoutPanelTablesnames";
            this.tableLayoutPanelTablesnames.RowCount = 1;
            this.tableLayoutPanelTablesnames.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTablesnames.Size = new System.Drawing.Size(391, 0);
            this.tableLayoutPanelTablesnames.TabIndex = 7;
            // 
            // filterConditionsPanel
            // 
            this.filterConditionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterConditionsPanel.AutoSize = true;
            this.filterConditionsPanel.Location = new System.Drawing.Point(0, 20);
            this.filterConditionsPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.filterConditionsPanel.Name = "filterConditionsPanel";
            this.filterConditionsPanel.Size = new System.Drawing.Size(391, 126);
            this.filterConditionsPanel.TabIndex = 8;
            // 
            // ArchiveEntryTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanelPage);
            this.Name = "ArchiveEntryTabPage";
            this.Size = new System.Drawing.Size(398, 281);
            this.tableLayoutPanelPage.ResumeLayout(false);
            this.tableLayoutPanelPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPage;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label labelTablenames;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelTablesnames;
        private FilterConditionsPanel filterConditionsPanel;
    }
}
