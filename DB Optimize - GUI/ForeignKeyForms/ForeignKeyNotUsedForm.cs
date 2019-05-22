using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeyNotUsedForm : Form
    {
        private readonly IList<DatabaseObject> _tables;
        private readonly DatabaseOperations _databaseOperations;

        public ForeignKeyNotUsedForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tables = tables;
            _databaseOperations = new DatabaseOperations(Database.Instance);
        }

        private void ForeignKeyNotUsedForm_Load(object sender, EventArgs e)
        {
            try
            {
                ISet<DatabaseObject> tablesWithoutForeignKeys = new HashSet<DatabaseObject>(_tables);
                foreach (DatabaseObject table in _tables)
                {
                    foreach (DependencyEdge edge in Optimizer.Instance.DependencyGraph[table].ChildEdges)
                    {
                        tablesWithoutForeignKeys.Remove(edge.Parent.DbObject);
                        ForeignKeyNotUsedTabPage foreignKeyTabPage = new ForeignKeyNotUsedTabPage(edge, _databaseOperations);
                        TabPage tabPage = new TabPage($"{edge.Parent.DbObject} - {edge.ForeignKey.Name}");
                        tabControlForeignKeys.TabPages.Add(tabPage);
                        tabPage.Controls.Add(foreignKeyTabPage);
                    }
                }
                Cursor.Current = Cursors.Default;
                if (tabControlForeignKeys.TabCount == 0)
                {
                    MessageBox.Show(this, "All tables are without foreign keys", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }
                else if (tablesWithoutForeignKeys.Count > 0)
                {
                    MessageBox.Show(this, $"These tables are without foreign keys: {string.Join(", ", tablesWithoutForeignKeys)}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Foreign keys without indexes cannot be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
    }
}
