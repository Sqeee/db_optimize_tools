using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DB_Optimize.DB;

namespace DB_Optimize.GUI.MainForms
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Database.StoredConnectionString == "")
            {
                return;
            }
            textBoxConnectionString.Text = Database.StoredConnectionString;
            Regex connectionStringRegex = new Regex(@"(?=(^|;)(?<param>data source|database|user id|password)=(?<value>[\w \\\(\)/_]+)*;)", RegexOptions.IgnoreCase);
            foreach (Match match in connectionStringRegex.Matches(textBoxConnectionString.Text))
            {
                switch (match.Groups["param"].ToString().ToLower())
                {
                    case "data source":
                        textBoxDataSource.Text = match.Groups["value"].Value;
                        break;
                    case "database":
                        textBoxDatabase.Text = match.Groups["value"].Value;
                        break;
                    case "user id":
                        textBoxUser.Text = match.Groups["value"].Value;
                        break;
                    case "password":
                        textBoxPassword.Text = match.Groups["value"].Value;
                        break;
                }
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            bool connectionSuccessful = tabControlConnection.SelectedTab == tabPageString ? Database.Instance.Connect(textBoxConnectionString.Text) : Database.Instance.Connect(textBoxDataSource.Text, textBoxDatabase.Text, textBoxUser.Text, textBoxPassword.Text);
            if (!connectionSuccessful)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, Database.Instance.ErrorMessage, "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonConnect.Enabled = true;
                return;
            }
            if (checkBoxSave.Checked)
            {
                Database.StoredConnectionString = tabControlConnection.SelectedTab == tabPageString ? textBoxConnectionString.Text : Database.Instance.GenerateConnectionString(textBoxDataSource.Text, textBoxDatabase.Text, textBoxUser.Text, textBoxPassword.Text);
            }
            buttonConnect.DialogResult = DialogResult.OK;
            Close();
        }

        private void ConnectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
    }
}
