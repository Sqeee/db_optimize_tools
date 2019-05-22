using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;

namespace DB_Optimize.GUI.ForeignKeyForms
{
    public partial class ForeignKeyNotUsedTabPage : UserControl
    {
        private const string TOTAL_VALUE = "Total values {0}";
        private const string NO_VALUES_SHOW = "No value is showed, change counter how many values should be loaded";
        private const string NO_VALUES_ALL = "Every value of foreign key is used";
        private const string VALUE_LOAD_ERROR = "Error: Cannot load values";
        private const string LOAD_VALUE_INFO = "To view values, you must click on button Load values";

        private readonly DependencyEdge _edge;
        private readonly DatabaseOperations _databaseOperations;

        public ForeignKeyNotUsedTabPage(DependencyEdge edge, DatabaseOperations databaseOperations)
        {
            InitializeComponent();

            _edge = edge;
            _databaseOperations = databaseOperations;
            int valuesCount = 0;
            try
            {
                valuesCount = _databaseOperations.CountValuesNotUsedInForeignKeys(edge);
                if (valuesCount == 0)
                {
                    buttonLoad.Enabled = false;
                    numericUpDownLoadValues.Value = 0;
                    numericUpDownLoadValues.Maximum = 0;
                    numericUpDownLoadValues.Enabled = false;
                    labelEmpty.Text = NO_VALUES_ALL;
                }
                else
                {
                    buttonLoad.Enabled = true;
                    numericUpDownLoadValues.Maximum = valuesCount;
                    numericUpDownLoadValues.Value = valuesCount;
                    numericUpDownLoadValues.Enabled = true;
                    labelEmpty.Text = LOAD_VALUE_INFO;
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                buttonLoad.Enabled = false;
                numericUpDownLoadValues.Value = 0;
                numericUpDownLoadValues.Maximum = 0;
                numericUpDownLoadValues.Enabled = false;
                labelEmpty.Text = VALUE_LOAD_ERROR;
            }
            labelValues.Text = string.Format(TOTAL_VALUE, valuesCount);

            Dock = DockStyle.Fill;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable values;
            try
            {
                values = _databaseOperations.ValuesNotUsedInForeignKeys(_edge, (int) numericUpDownLoadValues.Value);
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Values cannot be loaded", "Error while loading values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OutOfMemoryException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "Values cannot be loaded", "Error while loading values - out of memory. Try to lower count of values which you want to show", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (values.Rows.Count > 0)
            {
                labelEmpty.Visible = false;
                dataGridViewForeignKeys.DataSource = values;
            }
            else
            {
                labelEmpty.Text = NO_VALUES_SHOW;
                labelEmpty.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
