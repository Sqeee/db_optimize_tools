using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;
using DB_Optimize.GUI.HelperControls;

namespace DB_Optimize.GUI.DeleteForms
{
    public partial class DeleteFromTablesForm : Form
    {
        private TableDependencyGraph _dependencies;
        private readonly IList<DatabaseObject> _tablesLimit;

        public DeleteFromTablesForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tablesLimit = tables;
        }

        private void DeleteFromTablesForm_Load(object sender, EventArgs e)
        {
            try
            {
                _dependencies = Optimizer.Instance.DependencyGraph;
            }
            catch (TableDependencyException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Dependencies cannot be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IList<string> errorTableList = new List<string>();
            foreach (DatabaseObject table in _tablesLimit)
            {
                try
                {
                    DeleteEntryTabPage archiveEntryTabPage = new DeleteEntryTabPage(table, _dependencies);
                    TabPage page = new TabPage(table.NameWithSchema);
                    tabControlTables.TabPages.Add(page);
                    page.Controls.Add(archiveEntryTabPage);
                }
                catch (DatabaseException exc)
                {
                    Debug.WriteLine(exc);
                    errorTableList.Add(table.NameWithSchema);
                }
            }
            Cursor.Current = Cursors.Default;
            if (errorTableList.Count > 0)
            {
                MessageBox.Show(this, $"Columns of tables {string.Join(", ", errorTableList)} cannot be loaded", "Loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (errorTableList.Count == _tablesLimit.Count)
                {
                    Close();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DeleteEntryTabPage tabPage = (DeleteEntryTabPage)tabControlTables.SelectedTab.Controls[0];
            IDictionary<ForeignKey, ForeignKey.DeleteActions> foreignKeyActions = new Dictionary<ForeignKey, ForeignKey.DeleteActions>();
            for (int i = 0; i < tabPage.tableLayoutPanelForeignKeys.RowCount; i++)
            {
                ComboBox comboBox = (ComboBox)tabPage.tableLayoutPanelForeignKeys.GetControlFromPosition(1, i);
                if (comboBox == null)
                {
                    break;
                }
                ForeignKey.DeleteActions deleteAction = ((DeleteActionObject) comboBox.SelectedItem).DeleteAction.Value;
                foreignKeyActions.Add((ForeignKey)tabPage.tableLayoutPanelForeignKeys.GetControlFromPosition(0, i).Tag, deleteAction);
            }
            DatabaseObject tableWhereDelete = _tablesLimit.First(t => t.NameWithSchema == tabControlTables.SelectedTab.Text);
            MessageBoxWithTextBox messageBox = new MessageBoxWithTextBox(this, "Delete query:", "Delete procedure is ready", "Execute");
            try
            {
                messageBox.richTextBoxText.Text = Optimizer.Instance.DeleteEntriesFromTable(tableWhereDelete, tabPage.Conditions, foreignKeyActions);
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, $"Error while getting data - {exc.InnerException.Message}", "Cannot get data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (DeleteDependencyException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, exc.Message, "Cannot get data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor.Current = Cursors.Default;
            //To option repair query in case of error and execute it again
            while (true)
            {
                if (messageBox.ShowDialog(this) == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Database.Instance.ExecuteNonResultQuery(messageBox.richTextBoxText.Text);
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(this, "Delete procedure was executed.", "Delete procedure completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                    catch (SqlException exc)
                    {
                        Debug.WriteLine(exc);
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(this, "Error occured during executing query: " + exc.Message, "Query failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    break;
                }
            }
            messageBox.Dispose();
        }
    }
}
