using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeyBadDataTypeTabPage : UserControl
    {
        private readonly int _width;

        public ForeignKeyBadDataTypeTabPage(ForeignKey key, int size)
        {
            InitializeComponent();

            _width = flowLayoutPanelMain.Width;
            foreach (IList<string> infoPair in Optimizer.Instance.EvaluateForeignKeyDataType(key, size))
            {
                Label newLabel;
                if (infoPair.Count > 1)
                {
                    FlowLayoutPanel newFlowLayoutPanel = NewFlowLayoutPanel();
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
                    flowLayoutPanelMain.Controls.Add(newFlowLayoutPanel);
                }
                else
                {
                    newLabel = NewLabel();
                    newLabel.Text = infoPair[0];
                    newLabel.Font = new Font(newLabel.Font, FontStyle.Italic);
                    flowLayoutPanelMain.Controls.Add(newLabel);
                }
            }

            Dock = DockStyle.Fill;
        }

        private Label NewLabel()
        {
            Label label = new Label();
            label.AutoSize = true;
            return label;
        }

        private FlowLayoutPanel NewFlowLayoutPanel()
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
