using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;

namespace DB_Optimize.GUI.ColumnForms
{
    public partial class ColumnDataTypeForm : Form
    {
        private readonly IList<DatabaseObject> _tablesLimit;

        public ColumnDataTypeForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tablesLimit = tables;
        }

        private void ColumnDataTypeForm_Load(object sender, EventArgs e)
        {
            IList<string> errorTableList = new List<string>();
            foreach (DatabaseObject table in _tablesLimit)
            {
                try
                {
                    TableColumnsTabPage tableColumnsTabPage = new TableColumnsTabPage(table);
                    TabPage page = new TabPage(table.NameWithSchema);
                    tabControlTables.TabPages.Add(page);
                    page.Controls.Add(tableColumnsTabPage);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine($"{table.NameWithSchema} - {exc}");
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
    }
}
