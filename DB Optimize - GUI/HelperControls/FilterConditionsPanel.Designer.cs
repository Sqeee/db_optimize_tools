namespace DB_Optimize.GUI.HelperControls
{
    partial class FilterConditionsPanel
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
            this.tableLayoutPanelFilter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelColumns = new System.Windows.Forms.TableLayoutPanel();
            this.labelAndOr = new System.Windows.Forms.Label();
            this.labelColumn = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.buttonGenerateConditions = new System.Windows.Forms.Button();
            this.textBoxConditions = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelFilter.SuspendLayout();
            this.tableLayoutPanelColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelFilter
            // 
            this.tableLayoutPanelFilter.AutoSize = true;
            this.tableLayoutPanelFilter.ColumnCount = 1;
            this.tableLayoutPanelFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFilter.Controls.Add(this.buttonGenerateConditions, 0, 1);
            this.tableLayoutPanelFilter.Controls.Add(this.tableLayoutPanelColumns, 0, 0);
            this.tableLayoutPanelFilter.Controls.Add(this.textBoxConditions, 0, 2);
            this.tableLayoutPanelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFilter.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelFilter.Name = "tableLayoutPanelFilter";
            this.tableLayoutPanelFilter.RowCount = 4;
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFilter.Size = new System.Drawing.Size(364, 131);
            this.tableLayoutPanelFilter.TabIndex = 0;
            // 
            // tableLayoutPanelColumns
            // 
            this.tableLayoutPanelColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelColumns.AutoSize = true;
            this.tableLayoutPanelColumns.ColumnCount = 4;
            this.tableLayoutPanelColumns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColumns.Controls.Add(this.labelAndOr, 0, 0);
            this.tableLayoutPanelColumns.Controls.Add(this.labelColumn, 1, 0);
            this.tableLayoutPanelColumns.Controls.Add(this.labelOperator, 2, 0);
            this.tableLayoutPanelColumns.Controls.Add(this.labelValue, 3, 0);
            this.tableLayoutPanelColumns.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelColumns.Name = "tableLayoutPanelColumns";
            this.tableLayoutPanelColumns.RowCount = 1;
            this.tableLayoutPanelColumns.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelColumns.Size = new System.Drawing.Size(358, 17);
            this.tableLayoutPanelColumns.TabIndex = 3;
            // 
            // labelAndOr
            // 
            this.labelAndOr.AutoSize = true;
            this.labelAndOr.Location = new System.Drawing.Point(0, 0);
            this.labelAndOr.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelAndOr.Name = "labelAndOr";
            this.labelAndOr.Size = new System.Drawing.Size(113, 17);
            this.labelAndOr.TabIndex = 0;
            this.labelAndOr.Text = "And/Or grouping";
            // 
            // labelColumn
            // 
            this.labelColumn.AutoSize = true;
            this.labelColumn.Location = new System.Drawing.Point(119, 0);
            this.labelColumn.Name = "labelColumn";
            this.labelColumn.Size = new System.Drawing.Size(55, 17);
            this.labelColumn.TabIndex = 1;
            this.labelColumn.Text = "Column";
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Location = new System.Drawing.Point(180, 0);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(65, 17);
            this.labelOperator.TabIndex = 2;
            this.labelOperator.Text = "Operator";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(251, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(44, 17);
            this.labelValue.TabIndex = 3;
            this.labelValue.Text = "Value";
            // 
            // buttonGenerateConditions
            // 
            this.buttonGenerateConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerateConditions.Location = new System.Drawing.Point(6, 26);
            this.buttonGenerateConditions.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.buttonGenerateConditions.Name = "buttonGenerateConditions";
            this.buttonGenerateConditions.Size = new System.Drawing.Size(352, 27);
            this.buttonGenerateConditions.TabIndex = 5;
            this.buttonGenerateConditions.Text = "Generate conditions";
            this.buttonGenerateConditions.UseVisualStyleBackColor = true;
            this.buttonGenerateConditions.Click += new System.EventHandler(this.buttonGenerateConditions_Click);
            // 
            // textBoxConditions
            // 
            this.textBoxConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConditions.Location = new System.Drawing.Point(7, 59);
            this.textBoxConditions.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.textBoxConditions.Multiline = true;
            this.textBoxConditions.Name = "textBoxConditions";
            this.textBoxConditions.Size = new System.Drawing.Size(350, 64);
            this.textBoxConditions.TabIndex = 6;
            // 
            // FilterConditionsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanelFilter);
            this.Name = "FilterConditionsPanel";
            this.Size = new System.Drawing.Size(364, 131);
            this.tableLayoutPanelFilter.ResumeLayout(false);
            this.tableLayoutPanelFilter.PerformLayout();
            this.tableLayoutPanelColumns.ResumeLayout(false);
            this.tableLayoutPanelColumns.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelColumns;
        private System.Windows.Forms.Label labelAndOr;
        private System.Windows.Forms.Label labelColumn;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Button buttonGenerateConditions;
        public System.Windows.Forms.TextBox textBoxConditions;
    }
}
