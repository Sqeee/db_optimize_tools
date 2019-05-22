using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.GUI.ArchiveForms;
using DB_Optimize.GUI.ColumnForms;
using DB_Optimize.GUI.DeleteForms;
using DB_Optimize.GUI.DependencyForms;
using DB_Optimize.GUI.ForeignKeyForms;
using DB_Optimize.GUI.OtherForms;

namespace DB_Optimize.GUI.MainForms
{
    public partial class MainForm : Form
    {
        private DatabaseOperations _databaseOperations;
        private IList<DatabaseObject> _tablesList;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _databaseOperations = new DatabaseOperations(Database.Instance);
            
            if (new ConnectionForm().ShowDialog() == DialogResult.Abort)
            {
                Close();
                return;
            }
            try
            {
                _tablesList = _databaseOperations.GetTables();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                DialogResult result = MessageBox.Show(this, "List of tables cannot be loaded", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    MainForm_Load(sender, e);
                    return;
                }
                Database.Instance.Disconnect();
                Close();
                return;
            }
            checkedListBoxTables.DataSource = _tablesList;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            SetCheckedForAll(true);
        }

        private void buttonDeselectAll_Click(object sender, EventArgs e)
        {
            SetCheckedForAll(false);
        }

        private void SetCheckedForAll(bool checkedValue)
        {
            for (int i = 0; i < checkedListBoxTables.Items.Count; i++)
            {
                checkedListBoxTables.SetItemChecked(i, checkedValue);
            }
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Database.Instance.Disconnect();
            Application.Exit();
        }

        private void toolStripMenuItemReconnect_Click(object sender, EventArgs e)
        {
            buttonDeselectAll.PerformClick();
            Database.Instance.Disconnect();
            MainForm_Load(sender, e);
        }

        private void buttonDependencies_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                DependeciesForm form = checkedListBoxTables.Items.Count == checkedListBoxTables.CheckedItems.Count ? new DependeciesForm(null, false) : new DependeciesForm(selectedTables, false);
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
        }

        private void buttonForeignMissingIndexes_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                ForeignKeyWithoutIndexesForm form = new ForeignKeyWithoutIndexesForm(selectedTables);
                form.Show();
            }
        }

        private IList<DatabaseObject> GetSelectedTables()
        {
            return checkedListBoxTables.CheckedItems.Cast<DatabaseObject>().ToList();
        }

        private void buttonArchiveEntries_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                ArchiveTablesForm form = new ArchiveTablesForm(selectedTables);
                form.Show();
            }
        }

        private void buttonDependenciesSelected_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                DependeciesForm form = checkedListBoxTables.Items.Count == checkedListBoxTables.CheckedItems.Count ? new DependeciesForm(null, false) : new DependeciesForm(selectedTables, true);
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
        }

        private void buttonDeleteEntries_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                DeleteFromTablesForm form = new DeleteFromTablesForm(selectedTables);
                form.Show();
            }
        }

        private bool CheckSelectedAtLeastOne(IList<DatabaseObject> selection)
        {
            if (selection.Count < 1)
            {
                MessageBox.Show(this, "You must select at least one table.", "No table has been selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void buttonForeignIsUsed_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                ForeignKeyNotUsedForm form = new ForeignKeyNotUsedForm(selectedTables);
                form.Show();
            }
        }

        private void buttonForeignCascadeDelete_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                TableForeignKeysCascadeForm form = new TableForeignKeysCascadeForm(selectedTables);
                form.Show();
            }
        }

        private void buttonColumnsOptimalType_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                ColumnDataTypeForm form = new ColumnDataTypeForm(selectedTables);
                form.Show();
            }
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            AboutAppForm form = new AboutAppForm();
            form.Show();
        }

        private void buttonRecommendationIndexes_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                MissingIndexesStatisticsForm form = new MissingIndexesStatisticsForm(selectedTables);
                form.Show();
            }
        }

        private void buttonForeignBadType_Click(object sender, EventArgs e)
        {
            IList<DatabaseObject> selectedTables = GetSelectedTables();
            if (CheckSelectedAtLeastOne(selectedTables))
            {
                ForeignKeyBadDataTypeForm form = new ForeignKeyBadDataTypeForm(selectedTables);
                form.Show();
            }
        }

        private void buttonChangeCollation_Click(object sender, EventArgs e)
        {
            ChangeCollationForm form = new ChangeCollationForm();
            form.Show();
        }
    }
}
