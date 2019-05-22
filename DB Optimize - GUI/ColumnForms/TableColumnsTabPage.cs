using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ColumnForms
{
    public partial class TableColumnsTabPage : UserControl
    {
        private readonly int _width;

        public TableColumnsTabPage(DatabaseObject table)
        {
            InitializeComponent();

            DatabaseOperations operations = new DatabaseOperations(Database.Instance);

            IList<TableColumn> columns = operations.GetTableColumns(table);

            foreach (TableColumn column in columns)
            {
                TabPage tabPage = new TabPage(column.Name);
                FlowLayoutPanel mainFlowLayoutPanel = NewFlowLayoutPanelTopDown();
                mainFlowLayoutPanel.Dock = DockStyle.Fill;
                tabPage.Controls.Add(mainFlowLayoutPanel);
                _width = mainFlowLayoutPanel.Width;
                foreach (IList<string> infoPair in Optimizer.Instance.EvaluateColumnDataType(column, table))
                {
                    Label newLabel;
                    if (infoPair.Count > 1)
                    {
                        FlowLayoutPanel newFlowLayoutPanel = NewFlowLayoutPanelLeftToRight();
                        for (int i = 0; i < infoPair.Count; i++)
                        {
                            newLabel = NewLabel();
                            newLabel.Text = infoPair[i];
                            if (i == 0)
                            {
                                newLabel.Font = new Font(newLabel.Font, FontStyle.Bold);
                            }
                            newFlowLayoutPanel.Controls.Add(newLabel);
                        }
                        mainFlowLayoutPanel.Controls.Add(newFlowLayoutPanel);
                    }
                    else
                    {
                        newLabel = NewLabel();
                        newLabel.Text = infoPair[0];
                        newLabel.Font = new Font(newLabel.Font, FontStyle.Italic);
                        mainFlowLayoutPanel.Controls.Add(newLabel);
                    }
                }
                tabControlColumns.TabPages.Add(tabPage);
            }

            Dock = DockStyle.Fill;
        }

        private Label NewLabel()
        {
            Label label = new Label();
            label.AutoSize = true;
            return label;
        }

        private FlowLayoutPanel NewFlowLayoutPanelTopDown()
        {
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            return flowLayoutPanel;
        }

        private FlowLayoutPanel NewFlowLayoutPanelLeftToRight()
        {
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            flowLayoutPanel.AutoSize = true;
            flowLayoutPanel.Width = _width - flowLayoutPanel.Margin.All * 2;
            return flowLayoutPanel;
        }
    }
}
