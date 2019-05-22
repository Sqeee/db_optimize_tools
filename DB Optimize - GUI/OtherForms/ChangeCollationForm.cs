using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Optimization;

namespace DB_Optimize.GUI.OtherForms
{
    public partial class ChangeCollationForm : Form
    {
        public ChangeCollationForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();
        }

        private void ChangeCollationForm_Load(object sender, System.EventArgs e)
        {
            try
            {
                DatabaseOperations databaseOperations = new DatabaseOperations(Database.Instance);
                IList<string> collations = databaseOperations.GetCollationList();
                foreach (string collation in collations)
                {
                    comboBoxCollation.Items.Add(collation);
                }
                string defaultCollation = Database.Instance.Collation;
                comboBoxCollation.SelectedItem = defaultCollation;
                Cursor.Current = Cursors.Default;
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, "List of collations cannot be loaded", "Error while loading values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }

        private void buttonCollation_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show(this, "It is recommended to create backup of your database before the start of change database collation. Change of collatin is very time demanding action (it can take minutes) and for example in case of unexpected turnoff computer you may lose your database. Are you sure to continue with change of collation?", "Confirmation of change collation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    string collation = comboBoxCollation.SelectedItem.ToString();
                    Optimizer.Instance.ChangeCollation(collation);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(this, "Change of collation was succesfully executed.", "Change of collation was completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(this, $"Change of collation failed with error - {exc.Message}", "Change of collation failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
