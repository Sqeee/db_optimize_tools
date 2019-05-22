using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;

namespace DB_Optimize.GUI.DeleteForms
{
    public partial class DeleteEntryTabPage: UserControl
    {
        private readonly DeleteActionObject CASCADE_DELETE_ACTION_OBJECT = new DeleteActionObject(ForeignKey.DeleteActions.Cascade, false);
        private readonly DeleteActionObject SET_NULL_DELETE_ACTION_OBJECT = new DeleteActionObject(ForeignKey.DeleteActions.SetNull, false);
        private readonly DeleteActionObject SET_DEFAULT_DELETE_ACTION_OBJECT = new DeleteActionObject(ForeignKey.DeleteActions.SetDefault, false);

        public string Conditions => filterConditionsPanel.textBoxConditions.Text;

        public DeleteEntryTabPage(DatabaseObject table, TableDependencyGraph dependencies)
        {
            InitializeComponent();

            IList<DeleteActionObject> comboBoxDefaultActionItems = new List<DeleteActionObject>(4);
            comboBoxDefaultActionItems.Add(new DeleteActionObject(null, true));
            comboBoxDefaultActionItems.Add(CASCADE_DELETE_ACTION_OBJECT);
            comboBoxDefaultActionItems.Add(SET_NULL_DELETE_ACTION_OBJECT);
            comboBoxDefaultActionItems.Add(SET_DEFAULT_DELETE_ACTION_OBJECT);
            comboBoxDefaultAction.DataSource = comboBoxDefaultActionItems;

            filterConditionsPanel.LoadColumns(table);

            ISet<DependencyEdge> edges = dependencies.GetDescendantEdges(table);
            tableLayoutPanelForeignKeys.SuspendLayout();
            int row = 0;
            foreach (DependencyEdge edge in edges)
            {
                if (row > 0)
                {
                    tableLayoutPanelForeignKeys.RowStyles.Add(new RowStyle());
                    tableLayoutPanelForeignKeys.RowCount = row + 1;
                }
                Label labelTablename = NewLabelForeignKey();
                labelTablename.Tag = edge.ForeignKey;
                labelTablename.Text = edge.Name;
                tableLayoutPanelForeignKeys.Controls.Add(labelTablename, 0, row);
                ComboBox comboBoxForeignKey = NewComboBoxForeignKey();
                IList<DeleteActionObject> actions = new List<DeleteActionObject>();
                actions.Add(new DeleteActionObject(edge.ForeignKey.DeleteAction, true));
                actions.Add(CASCADE_DELETE_ACTION_OBJECT);
                if (edge.ForeignKey.CanBeNull)
                {
                    actions.Add(SET_NULL_DELETE_ACTION_OBJECT);
                }
                if (edge.ForeignKey.HasDefaultValue)
                {
                    actions.Add(SET_DEFAULT_DELETE_ACTION_OBJECT);
                }
                comboBoxForeignKey.DataSource = actions;
                tableLayoutPanelForeignKeys.Controls.Add(comboBoxForeignKey, 1, row);
                row++;
            }
            if (edges.Count == 0)
            {
                comboBoxDefaultAction.Enabled = false;
                buttonCopyAction.Enabled = false;
                labelForeignKeys.Text = "No foreign keys";
                labelForeignKeys.Font = new Font(labelForeignKeys.Font, FontStyle.Italic);
            }
            tableLayoutPanelForeignKeys.ResumeLayout(false);
            tableLayoutPanelForeignKeys.PerformLayout();

            Dock = DockStyle.Fill;
        }

        private Label NewLabelForeignKey()
        {
            Label label = new Label()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = "labelForeignKeyName",
                TextAlign = ContentAlignment.MiddleLeft
            };
            return label;
        }

        private ComboBox NewComboBoxForeignKey()
        {
            ComboBox combobox = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "comboBoxForeignKeyAction",
                Size = new Size(200, 24)
            };
            return combobox;
        }

        private void buttonCopyAction_Click(object sender, EventArgs e)
        {
            DeleteActionObject actionToSet = (DeleteActionObject)comboBoxDefaultAction.SelectedItem;
            bool originalAction = actionToSet.IsOriginalAction;
            for (int i = 0; i < tableLayoutPanelForeignKeys.RowCount; i++)
            {
                ComboBox comboBox = (ComboBox)tableLayoutPanelForeignKeys.GetControlFromPosition(1, i);
                if (originalAction)
                {
                    comboBox.SelectedIndex = 0;
                }
                else if(comboBox.Items.Contains(actionToSet))
                {
                    comboBox.SelectedItem = actionToSet;
                }
            }
        }
    }
}
