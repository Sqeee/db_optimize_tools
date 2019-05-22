using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class TableForeignKeysCascadeForm : Form
    {
        private readonly IList<DatabaseObject> _tables;
        private TableDependencyGraph _dependencies;

        public TableForeignKeysCascadeForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tables = tables;
        }

        private void TableForeignKeysCascadeForm_Load(object sender, EventArgs e)
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
            foreach (DatabaseObject table in _tables)
            {
                ForeignKeysOverviewTabPage overview = new ForeignKeysOverviewTabPage(table, _dependencies);
                TabPage tabPage = new TabPage(table.NameWithSchema);
                tabControlTables.TabPages.Add(tabPage);
                tabPage.Controls.Add(overview);
            }
        }
    }
}
