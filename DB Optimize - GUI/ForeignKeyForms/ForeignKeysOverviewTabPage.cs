using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeysOverviewTabPage : UserControl
    {
        public ForeignKeysOverviewTabPage(DatabaseObject table, TableDependencyGraph dependencies)
        {
            InitializeComponent();

            if (dependencies[table].Children.Count == 0)
            {
                Label labelEmpty = NewLabel();
                labelEmpty.Text = "No foreign keys";
                labelEmpty.Font = new Font(labelEmpty.Font, FontStyle.Italic);
                labelEmpty.Margin = new Padding(0);
                flowLayoutPanelForeignKeys.Controls.Add(labelEmpty);
            }
            else
            {
                bool OK;
                string explanation;
                IList<Tuple<DependencyEdge, string>> keys = Optimizer.Instance.CheckDeleteCascade(dependencies[table], out OK, out explanation);
                foreach (Tuple<DependencyEdge, string> key in keys)
                {
                    Label labelForeignKey = NewLabel();
                    labelForeignKey.Text = $"{key.Item2}: {key.Item1.Name} - {ForeignKey.DeleteActionToString(key.Item1.ForeignKey.DeleteAction)}";
                    labelForeignKey.Margin = new Padding(key.Item2.Count(ch => ch == '.') * 20, 0, 0, 0);
                    flowLayoutPanelForeignKeys.Controls.Add(labelForeignKey);
                }
                if (OK)
                {
                    labelStatus.Text = "OK";
                }
                else
                {
                    labelStatus.Text = "NOK";
                    labelStatus.ForeColor = Color.Red;
                }
                if (explanation.Length > 0)
                {
                    labelStatus.Text += " - " + explanation;
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
    }
}
