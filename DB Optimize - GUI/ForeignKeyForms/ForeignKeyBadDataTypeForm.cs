using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeyBadDataTypeForm : Form
    {
        private readonly IList<DatabaseObject> _tables;
        private readonly DatabaseOperations _databaseOperations;

        public ForeignKeyBadDataTypeForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tables = tables;
            _databaseOperations = new DatabaseOperations(Database.Instance);
        }

        private void ForeignKeyBadDataTypeForm_Load(object sender, EventArgs e)
        {
            try
            {
                ISet<DatabaseObject> tablesWithoutForeignKeys = new HashSet<DatabaseObject>(_tables);
                IList<ForeignKey> foreignKeys = new List<ForeignKey>();
                foreach (DatabaseObject table in _tables)
                {
                    foreach (DependencyEdge edge in Optimizer.Instance.DependencyGraph[table].ParentEdges)
                    {
                        foreignKeys.Add(edge.ForeignKey);
                    }
                }
                IDictionary<ForeignKey, int> foreignKeySizes = _databaseOperations.GetForeignKeySize(foreignKeys);
                foreach (ForeignKey foreignKey in foreignKeys)
                {
                    tablesWithoutForeignKeys.Remove(foreignKey.Table);
                    ForeignKeyBadDataTypeTabPage foreignKeyTabPage = new ForeignKeyBadDataTypeTabPage(foreignKey, foreignKeySizes[foreignKey]);
                    TabPage tabPage = new TabPage($"{foreignKey.Table.NameWithSchema} - {foreignKey.Name}");
                    tabControlForeignKeys.TabPages.Add(tabPage);
                    tabPage.Controls.Add(foreignKeyTabPage);
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
                MessageBox.Show(this, "Foreign key sizes cannot be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
    }
}
