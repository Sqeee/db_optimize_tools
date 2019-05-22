using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;
using DB_Optimize.GUI.HelperControls;

namespace DB_Optimize.GUI.ArchiveForms
{
    public partial class ArchiveTablesForm : Form
    {
        private TableDependencyGraph _dependencies;
        private readonly IList<DatabaseObject> _tablesLimit;

        public ArchiveTablesForm(IList<DatabaseObject> tables)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();

            _tablesLimit = tables;
        }

        private void ArchiveTablesForm_Load(object sender, EventArgs e)
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
            IList<Tuple<DatabaseObject, DatabaseObject>> removedTables = new List<Tuple<DatabaseObject, DatabaseObject>>();
            for (int i = 0; i < _tablesLimit.Count; i++)
            {
                ISet<DatabaseObject> nodes = new HashSet<DatabaseObject>(_dependencies.GetAncestors(_tablesLimit[i]).Select(t => t.DbObject));
                for (int j = 0; j < _tablesLimit.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (nodes.Contains(_tablesLimit[j]))
                    {
                        removedTables.Add(new Tuple<DatabaseObject, DatabaseObject>(_tablesLimit[j], _tablesLimit[i]));
                        _tablesLimit.Remove(_tablesLimit[j]);
                        if (j < i)
                        {
                            i--;
                        }
                        j--;
                    }
                }
            }
            IList<string> errorTableList = new List<string>();
            foreach (DatabaseObject table in _tablesLimit)
            {
                try
                {
                    ArchiveEntryTabPage archiveEntryTabPage = new ArchiveEntryTabPage(table, _dependencies);
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
            }
            if (removedTables.Count > 0)
            {
                MessageBox.Show(this, $"These tables are dependent on others and there is no need to archive them multiple times.{Environment.NewLine}{string.Join(Environment.NewLine, removedTables.Select(t => t.Item1 + " -> " + t.Item2))}", "Table dependency check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBoxOnlySave_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxInsertWithSelect.Enabled = !checkBoxOnlySave.Checked;
        }

        private void buttonArchive_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ArchiveEntryTabPage tabPage = (ArchiveEntryTabPage) tabControlTables.SelectedTab.Controls[0];
            IDictionary<string, string> convertorTableNames = new Dictionary<string, string>(tabPage.tableLayoutPanelTablesnames.RowCount * 2);
            bool tableNamesChanged = false;
            for (int i = 0; i <= tabPage.tableLayoutPanelTablesnames.RowCount; i++)
            {
                for (int j = 0; j < tabPage.tableLayoutPanelTablesnames.ColumnCount; j += 2)
                {
                    if (tabPage.tableLayoutPanelTablesnames.GetControlFromPosition(j, i) != null)
                    {
                        string text = tabPage.tableLayoutPanelTablesnames.GetControlFromPosition(j + 1, i).Text;
                        if (text.Trim().Length == 0)
                        {
                            MessageBox.Show(this, $"New tablename of tablename {tabPage.tableLayoutPanelTablesnames.GetControlFromPosition(j, i).Text} cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!tableNamesChanged && tabPage.tableLayoutPanelTablesnames.GetControlFromPosition(j, i).Text != text)
                        {
                            tableNamesChanged = true;
                        }
                        convertorTableNames.Add(tabPage.tableLayoutPanelTablesnames.GetControlFromPosition(j, i).Text.Replace("[", "").Replace("]", "").ToLower(), text);
                    }
                }
            }
            if (!tableNamesChanged && checkBoxInsertWithSelect.Checked)
            {
                MessageBox.Show(this, $"You have checked \"{checkBoxInsertWithSelect.Text}\", but you do not change any tablename. You cannot Select for Insert from same table. Please rename table or uncheck option {checkBoxInsertWithSelect.Checked}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DatabaseObject tableToArchive = _tablesLimit.First(t => t.NameWithSchema == tabControlTables.SelectedTab.Text);
            Cursor.Current = Cursors.Default;
            Stream fileStream = null;
            StreamWriter streamWriter = null;
            string filename = "";
            bool deleteFile = false;
            try
            {
                if (checkBoxOnlySave.Checked && saveSQLFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    fileStream = saveSQLFileDialog.OpenFile();
                    filename = saveSQLFileDialog.FileName;
                }
                else
                {
                    fileStream = Optimizer.GetTempFileStreamWriter(out filename);
                    deleteFile = true;
                }
                streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                Cursor.Current = Cursors.WaitCursor;
                if (!Optimizer.Instance.ArchiveTable(tableToArchive, convertorTableNames, checkBoxCreateTable.Checked, tabPage.Conditions, checkBoxInsertWithSelect.Checked, checkBoxSaveAllEntries.Checked, streamWriter))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(this, "No data to archiving", "Archive status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (checkBoxOnlySave.Checked)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(this, "Archived entries were saved.", "Archiving completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBoxWithTextBox messageBox = new MessageBoxWithTextBox(this, "Archive:", "Archiving is ready", "Execute");
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        messageBox.richTextBoxText.Text = streamReader.ReadToEnd();
                        streamWriter = null;
                        fileStream = null;
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
                                MessageBox.Show(this, "Archiving was executed.", "Archiving completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                MessageBox.Show(this, $"Error while getting data - {exc.InnerException.Message}", "Cannot get data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException exc)
            {
                Debug.WriteLine(exc);
                MessageBox.Show(this, $"Error with data file - {exc.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OutOfMemoryException exc)
            {
                Debug.WriteLine(exc);
                MessageBox.Show(this, $"Program is out of memory - processed data are saved at file {filename}", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                deleteFile = false;
                Process.Start("explorer.exe", $"/select, {filename}");
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                if (streamWriter != null)
                {
                    streamWriter.Dispose();
                    fileStream = null;
                }
                fileStream?.Dispose();
                if (deleteFile)
                {
                    File.Delete(filename);
                }
            }
        }
    }
}
