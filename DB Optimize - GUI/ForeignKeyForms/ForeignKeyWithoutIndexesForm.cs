using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Optimization;
using DB_Optimize.GUI.HelperControls;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeyWithoutIndexesForm : Form
    {
        private readonly DatabaseOperations _databaseOperations;
        private readonly IList<DatabaseObject> _tablesLimit;
        private int _selectedRows;

        public ForeignKeyWithoutIndexesForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _databaseOperations = new DatabaseOperations(Database.Instance);
            _tablesLimit = tables;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            checkBoxColumn.CellTemplate = new DataGridViewCheckBoxCell();
            checkBoxColumn.ValueType = typeof(bool);
            dataGridViewForeignKeys.Columns.Add(checkBoxColumn);
        }

        private void ForeignKeyWithoutIndexes_Load(object sender, EventArgs e)
        {
            _selectedRows = 0;
            try
            {
                IList<ForeignKeyMissingIndexes> keys = _databaseOperations.GetForeignKeysWithoutIndexes(_tablesLimit);
                if (keys.Count > 0)
                {
                    keys = keys.OrderBy(k => k.Table.NameWithSchema).ToList();
                    dataGridViewForeignKeys.DataSource = keys;
                    dataGridViewForeignKeys.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    foreach (DataGridViewColumn column in dataGridViewForeignKeys.Columns)
                    {
                        if (column.DataPropertyName == nameof(ForeignKeyMissingIndexes.MissingIndexColumnsText))
                        {
                            column.HeaderText = "Missing columns in index";
                            column.DisplayIndex = dataGridViewForeignKeys.ColumnCount - 1;
                        }
                        else if (column.DataPropertyName == nameof(ForeignKey.ColumnsText))
                        {
                            column.HeaderText = "Columns";
                        }
                    }
                }
                else
                {
                    labelEmpty.Visible = true;
                    dataGridViewForeignKeys.Visible = false;
                    buttonSelectAll.Enabled = false;
                    buttonDeselectAll.Enabled = false;
                }
                UpdateComponentsState();
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Foreign keys without indexes cannot be loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SetCheckStateForAll(true);
            _selectedRows = dataGridViewForeignKeys.RowCount;
            UpdateComponentsState();
            Cursor.Current = Cursors.Default;
        }

        private void buttonDeselectAll_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SetCheckStateForAll(false);
            _selectedRows = 0;
            UpdateComponentsState();
            Cursor.Current = Cursors.Default;
        }

        private void buttonCreateIndexFull_Click(object sender, EventArgs e)
        {
            CreateIndexes(true);
        }

        private void buttonCreateIndexMissing_Click(object sender, EventArgs e)
        {
            CreateIndexes(false);
        }

        private IList<ForeignKeyMissingIndexes> GetSelectedForeignKeys()
        {
            IList<ForeignKeyMissingIndexes> result = new List<ForeignKeyMissingIndexes>();
            foreach (DataGridViewRow row in dataGridViewForeignKeys.Rows)
            {
                if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                {
                    result.Add((ForeignKeyMissingIndexes)row.DataBoundItem);
                }
            }
            return result;
        }

        private void CreateIndexes(bool full)
        {
            IList<ForeignKeyMissingIndexes> missingIndexes = GetSelectedForeignKeys();
            string createIndexes = Optimizer.CreateIndexesForeignKey(missingIndexes, full);
            MessageBoxWithTextBox form = new MessageBoxWithTextBox(this, createIndexes, "Creating indexes", "Create indexes");
            if (form.ShowDialog(this) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    Database.Instance.ExecuteNonResultQuery(form.richTextBoxText.Text);
                    MessageBox.Show(this, "Creating indexes was successful.", "Creating indexes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                    MessageBox.Show(this, "Creating indexes failed with this error: " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
                ForeignKeyWithoutIndexes_Load(this, null);
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetCheckStateForAll(bool value)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow row in dataGridViewForeignKeys.Rows)
            {
                row.Cells[0].Value = value;
            }
            Cursor.Current = Cursors.Default;
        }

        private void UpdateComponentsState()
        {
            buttonCreateIndexFull.Enabled = _selectedRows > 0;
            buttonCreateIndexMissing.Enabled = buttonCreateIndexFull.Enabled;
            toolStripStatusLabelInfo.Text = $"Selected {_selectedRows} rows of total {dataGridViewForeignKeys.RowCount} rows";
        }

        private void dataGridViewForeignKeys_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool value;
                if (dataGridViewForeignKeys[e.ColumnIndex, e.RowIndex].Value == null)
                {
                    value = true;
                }
                else
                {
                    value = !(bool) dataGridViewForeignKeys[e.ColumnIndex, e.RowIndex].Value;
                }
                dataGridViewForeignKeys[e.ColumnIndex, e.RowIndex].Value = value;
                if (value)
                {
                    _selectedRows++;
                }
                else
                {
                    _selectedRows--;
                }
                UpdateComponentsState();
            }
        }
    }
}
