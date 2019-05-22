using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using DB_Optimize.DB;
using DB_Optimize.Dependency;

namespace DB_Optimize.GUI.DependencyForms
{
    public partial class DependeciesForm : Form
    {
        private readonly Color COLOR_SELECTED_NODE = Color.DodgerBlue;
        private readonly Color COLOR_PARENT_NODE = Color.Cyan;
        private readonly Color COLOR_CHILD_NODE = Color.Violet;
        private readonly Color COLOR_ORPHAN_NODE = Color.Gray;
        private readonly Color COLOR_EDGE = Color.Black;
        private const int WIDTH_EDGE = 1;
        private const int WIDTH_SELECTED_EDGE = 4;

        private TableDependencyGraph _dependencies;
        private readonly IList<DatabaseObject> _tablesLimit;
        private readonly bool _onlySelectedTables;
        private bool _firstSelection;
        private readonly IList<Node> _firstSelectedNodes = new List<Node>();
        private Node _selectedNode;
        private int _maxEdges = 1;
        private int _maxInOutEdges = 1;
        private int _maxInEdges = 1;
        private int _maxOutEdges = 1;
        private Func<Node, int> _nodeEdgesCountFunc;
        private Graph _graph;

        public DependeciesForm(IList<DatabaseObject> tables, bool onlySelected)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tablesLimit = tables;
            _onlySelectedTables = onlySelected;
            if (tables != null && !onlySelected)
            {
                _firstSelection = true;
            }
        }

        private void DependeciesForm_Load(object sender, EventArgs e)
        {
            try
            {
                _dependencies = new TableDependencyGraph(_tablesLimit, _onlySelectedTables);
            }
            catch (TableDependencyException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Dependencies cannot be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            _graph = new Graph("graph");
            graphViewer.CurrentLayoutMethod = LayoutMethod.MDS;
            graphViewer.LayoutAlgorithmSettingsButtonVisible = true;
            foreach (DependencyEdge edge in _dependencies.DependencyEdges)
            {
                _graph.AddEdge(edge.Parent.DbObject.NameWithSchema, edge.Name, edge.Child.DbObject.NameWithSchema);
            }
            foreach (DependencyNode node in _dependencies.NodesWithoutEdges)
            {
                _graph.AddNode(node.DbObject.NameWithSchema);
            }
            foreach (Node node in _graph.Nodes)
            {
                _maxInEdges = Math.Max(node.InEdges.Count(), _maxInEdges);
                _maxOutEdges = Math.Max(node.OutEdges.Count(), _maxOutEdges);
                _maxInOutEdges = Math.Max(node.Edges.Count(), _maxInOutEdges);
            }
            SetEdgesCounts();
            CalculateNodesColor();
            graphViewer.Graph = _graph;
            Cursor.Current = Cursors.Default;
        }

        private void CalculateNodeColor(Node node)
        {
            node.Attr.FillColor = !node.Edges.Any() ? COLOR_ORPHAN_NODE : GetHeatMapColor(_nodeEdgesCountFunc(node), 0, _maxEdges);
        }

        private void CalculateNodesColor()
        {
            _selectedNode = null;
            ISet<string> tablesLimitNames = null;
            if (_firstSelection)
            {
                tablesLimitNames = new HashSet<string>(_tablesLimit.Select(t => t.NameWithSchema));
            }
            foreach (Node node in _graph.Nodes)
            {
                if (!_firstSelection || !tablesLimitNames.Contains(node.LabelText))
                {
                    CalculateNodeColor(node);
                }
                else
                {
                    _firstSelectedNodes.Add(node);
                    node.Attr.FillColor = COLOR_SELECTED_NODE;
                }
            }
            foreach (Edge edge in _graph.Edges)
            {
                edge.Attr.Color = COLOR_EDGE;
                edge.Attr.LineWidth = WIDTH_EDGE;
            }
            graphViewer.Invalidate();
        }

        private Color GetHeatMapColor(double value, double min, double max)
        {
            double ratio = (value - min) / (max - min);
            return new Color(255, Convert.ToByte(255 * ratio), Convert.ToByte(255 * (1 - ratio)), 0);
        }

        private void graphViewer_Click(object sender, EventArgs e)
        {
            if (_firstSelection)
            {
                foreach (Node node in _firstSelectedNodes)
                {
                    CalculateNodeColor(node);
                }
                _firstSelection = false;
                _firstSelectedNodes.Clear();
            }
            if (_selectedNode != null)
            {
                CalculateNodeColor(_selectedNode);
                foreach (Edge edge in _selectedNode.InEdges)
                {
                    CalculateNodeColor(edge.SourceNode);
                    edge.Attr.Color = COLOR_EDGE;
                    edge.Attr.LineWidth = WIDTH_EDGE;
                }
                foreach (Edge edge in _selectedNode.OutEdges)
                {
                    CalculateNodeColor(edge.TargetNode);
                    edge.Attr.Color = COLOR_EDGE;
                    edge.Attr.LineWidth = WIDTH_EDGE;
                }
            }
            Node clickedNode = ((GViewer)sender).SelectedObject as Node;
            if (clickedNode != null)
            {
                _selectedNode = clickedNode;
                _selectedNode.Attr.FillColor = COLOR_SELECTED_NODE;
                foreach (Edge edge in _selectedNode.InEdges)
                {
                    edge.SourceNode.Attr.FillColor = COLOR_PARENT_NODE;
                    edge.Attr.Color = COLOR_PARENT_NODE;
                    edge.Attr.LineWidth = WIDTH_SELECTED_EDGE;
                }
                foreach (Edge edge in _selectedNode.OutEdges)
                {
                    edge.TargetNode.Attr.FillColor = COLOR_CHILD_NODE;
                    edge.Attr.Color = COLOR_CHILD_NODE;
                    edge.Attr.LineWidth = WIDTH_SELECTED_EDGE;
                }
            }
            else
            {
                _selectedNode = null;
            }
            graphViewer.Invalidate();
        }

        private void buttonExportDot_Click(object sender, EventArgs e)
        {
            if (saveDotFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream fileStream = saveDotFileDialog.OpenFile())
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(_dependencies.ToDotFormat());
                        MessageBox.Show(this, "Dependencies have been successfully exported.", "Export completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void SetEdgesCounts()
        {
            if (checkBoxInEdges.Checked && !checkBoxOutEdges.Checked)
            {
                _maxEdges = _maxInEdges;
                _nodeEdgesCountFunc = n => n.InEdges.Count();
            }
            else if (!checkBoxInEdges.Checked && checkBoxOutEdges.Checked)
            {
                _maxEdges = _maxOutEdges;
                _nodeEdgesCountFunc = n => n.OutEdges.Count();
            }
            else
            {
                _maxEdges = _maxInOutEdges;
                _nodeEdgesCountFunc = n => n.Edges.Count();
            }
        }

        private void checkBoxEdges_CheckedChanged(object sender, EventArgs e)
        {
            SetEdgesCounts();
            CalculateNodesColor();
        }
    }
}
