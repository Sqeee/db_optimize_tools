using System.Windows.Forms;

namespace DB_Optimize.GUI.DependencyForms
{
    partial class DependeciesForm
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
            Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation planeTransformation1 = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DependeciesForm));
            this.graphViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.buttonExportDot = new System.Windows.Forms.Button();
            this.saveDotFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveGraphMLFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.checkBoxInEdges = new System.Windows.Forms.CheckBox();
            this.checkBoxOutEdges = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // graphViewer
            // 
            this.graphViewer.ArrowheadLength = 10D;
            this.graphViewer.AsyncLayout = false;
            this.graphViewer.AutoScroll = true;
            this.graphViewer.BackwardEnabled = false;
            this.graphViewer.BuildHitTree = true;
            this.graphViewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            this.graphViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphViewer.EdgeInsertButtonVisible = false;
            this.graphViewer.FileName = "";
            this.graphViewer.ForwardEnabled = false;
            this.graphViewer.Graph = null;
            this.graphViewer.InsertingEdge = false;
            this.graphViewer.LayoutAlgorithmSettingsButtonVisible = false;
            this.graphViewer.LayoutEditingEnabled = true;
            this.graphViewer.Location = new System.Drawing.Point(0, 0);
            this.graphViewer.LooseOffsetForRouting = 0.25D;
            this.graphViewer.MinimumSize = new System.Drawing.Size(915, 0);
            this.graphViewer.MouseHitDistance = 0.05D;
            this.graphViewer.Name = "graphViewer";
            this.graphViewer.NavigationVisible = true;
            this.graphViewer.NeedToCalculateLayout = true;
            this.graphViewer.OffsetForRelaxingInRouting = 0.6D;
            this.graphViewer.PaddingForEdgeRouting = 8D;
            this.graphViewer.PanButtonPressed = false;
            this.graphViewer.SaveAsImageEnabled = true;
            this.graphViewer.SaveAsMsaglEnabled = false;
            this.graphViewer.SaveButtonVisible = true;
            this.graphViewer.SaveGraphButtonVisible = true;
            this.graphViewer.SaveInVectorFormatEnabled = true;
            this.graphViewer.Size = new System.Drawing.Size(915, 429);
            this.graphViewer.TabIndex = 0;
            this.graphViewer.TightOffsetForRouting = 0.125D;
            this.graphViewer.ToolBarIsVisible = true;
            this.graphViewer.Transform = planeTransformation1;
            this.graphViewer.UndoRedoButtonsVisible = true;
            this.graphViewer.WindowZoomButtonPressed = false;
            this.graphViewer.ZoomF = 1D;
            this.graphViewer.ZoomWindowThreshold = 0.05D;
            this.graphViewer.Click += new System.EventHandler(this.graphViewer_Click);
            // 
            // buttonExportDot
            // 
            this.buttonExportDot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportDot.Location = new System.Drawing.Point(700, 6);
            this.buttonExportDot.Name = "buttonExportDot";
            this.buttonExportDot.Size = new System.Drawing.Size(203, 23);
            this.buttonExportDot.TabIndex = 1;
            this.buttonExportDot.Text = "Export to Dot file format";
            this.buttonExportDot.UseVisualStyleBackColor = true;
            this.buttonExportDot.Click += new System.EventHandler(this.buttonExportDot_Click);
            // 
            // saveDotFileDialog
            // 
            this.saveDotFileDialog.DefaultExt = "gv";
            this.saveDotFileDialog.Filter = "All files|*.gv";
            this.saveDotFileDialog.Title = "Save exported dot file";
            // 
            // saveGraphMLFileDialog
            // 
            this.saveGraphMLFileDialog.DefaultExt = "graphml";
            this.saveGraphMLFileDialog.Filter = "All files|*.graphml";
            this.saveGraphMLFileDialog.Title = "Save exported GraphML file";
            // 
            // checkBoxInEdges
            // 
            this.checkBoxInEdges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxInEdges.AutoSize = true;
            this.checkBoxInEdges.Checked = true;
            this.checkBoxInEdges.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInEdges.Location = new System.Drawing.Point(267, 8);
            this.checkBoxInEdges.Name = "checkBoxInEdges";
            this.checkBoxInEdges.Size = new System.Drawing.Size(211, 21);
            this.checkBoxInEdges.TabIndex = 3;
            this.checkBoxInEdges.Text = "Heatmap for incoming edges";
            this.checkBoxInEdges.UseVisualStyleBackColor = true;
            this.checkBoxInEdges.CheckedChanged += new System.EventHandler(this.checkBoxEdges_CheckedChanged);
            // 
            // checkBoxOutEdges
            // 
            this.checkBoxOutEdges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOutEdges.AutoSize = true;
            this.checkBoxOutEdges.Location = new System.Drawing.Point(484, 8);
            this.checkBoxOutEdges.Name = "checkBoxOutEdges";
            this.checkBoxOutEdges.Size = new System.Drawing.Size(210, 21);
            this.checkBoxOutEdges.TabIndex = 4;
            this.checkBoxOutEdges.Text = "Heatmap for outgoing edges";
            this.checkBoxOutEdges.UseVisualStyleBackColor = true;
            this.checkBoxOutEdges.CheckedChanged += new System.EventHandler(this.checkBoxEdges_CheckedChanged);
            // 
            // DependeciesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 429);
            this.Controls.Add(this.checkBoxOutEdges);
            this.Controls.Add(this.checkBoxInEdges);
            this.Controls.Add(this.buttonExportDot);
            this.Controls.Add(this.graphViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DependeciesForm";
            this.Text = "Dependecies";
            this.Load += new System.EventHandler(this.DependeciesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Microsoft.Msagl.GraphViewerGdi.GViewer graphViewer;
        private Button buttonExportDot;
        private SaveFileDialog saveDotFileDialog;
        private SaveFileDialog saveGraphMLFileDialog;
        private CheckBox checkBoxInEdges;
        private CheckBox checkBoxOutEdges;
    }
}