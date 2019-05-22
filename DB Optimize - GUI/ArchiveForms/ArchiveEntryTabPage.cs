using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;

namespace DB_Optimize.GUI.ArchiveForms
{
    public partial class ArchiveEntryTabPage: UserControl
    {
        public string Conditions => filterConditionsPanel.textBoxConditions.Text;

        public ArchiveEntryTabPage(DatabaseObject table, TableDependencyGraph dependencies)
        {
            InitializeComponent();

            filterConditionsPanel.LoadColumns(table);

            ISet<DependencyNode> dependentTables = dependencies.GetAncestors(table);
            dependentTables.Add(dependencies[table]);
            tableLayoutPanelTablesnames.SuspendLayout();
            int columnOffset = 0;
            foreach (DependencyNode dependentTable in dependentTables)
            {
                int row = tableLayoutPanelTablesnames.RowCount;
                Label labelTablename = NewLabelTablename();
                labelTablename.Text = dependentTable.DbObject.NameWithSchema;
                tableLayoutPanelTablesnames.Controls.Add(labelTablename, 0 + columnOffset, row);
                TextBox textBoxTableName = NewTextBoxTablename();
                textBoxTableName.Text = dependentTable.DbObject.NameWithSchema;
                tableLayoutPanelTablesnames.Controls.Add(textBoxTableName, 1 + columnOffset, row);
                if (columnOffset > 0)
                {
                    tableLayoutPanelTablesnames.RowStyles.Add(new RowStyle());
                    tableLayoutPanelTablesnames.RowCount = row + 1;
                    columnOffset = 0;
                }
                else
                {
                    columnOffset += 2;
                }
            }
            tableLayoutPanelTablesnames.ResumeLayout(false);
            tableLayoutPanelTablesnames.PerformLayout();

            Dock = DockStyle.Fill;
        }

        private Label NewLabelTablename()
        {
            return new Label()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = "labelTablename",
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private TextBox NewTextBoxTablename()
        {
            return new TextBox()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Name = "textBoxTablename"
            };
        }
    }
}
