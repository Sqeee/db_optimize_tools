using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;

namespace DB_Optimize.GUI.OtherForms
{
    public partial class MissingIndexesStatisticsForm : Form
    {
        private readonly IList<DatabaseObject> _tables;

        public MissingIndexesStatisticsForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tables = tables;
        }

        private void MissingIndexesStatisticsForm_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseOperations databaseOperations = new DatabaseOperations(Database.Instance);
                DataTable values = databaseOperations.MissingIndexesFromStatistics(_tables);
                if (values.Rows.Count > 0)
                {
                    dataGridViewMissingIndexes.DataSource = values;
                    labelEmpty.Visible = false;
                }
                else
                {
                    labelEmpty.Visible = true;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Values cannot be loaded", "Error while loading values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
    }
}
