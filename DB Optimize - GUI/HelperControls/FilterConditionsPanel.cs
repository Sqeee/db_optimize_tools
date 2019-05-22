using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB_Optimize.DB;

namespace DB_Optimize.GUI.HelperControls
{
    public partial class FilterConditionsPanel : UserControl
    {
        private const string AND_CONDITION = "And";
        private const string OR_CONDITION = "Or";
        private const string EMPTY_CONDITION = "";
        private const string EQUAL_CONDITION = "=";
        private const string NOT_EQUAL_CONDITION = "!=";
        private const string GREATER_EQUAL_CONDITION = ">=";
        private const string GREATER_CONDITION = ">";
        private const string LESS_CONDITION = "<";
        private const string LESS_GREATER_CONDITION = "<=";
        private const string LIKE_CONDITION = "LIKE";
        private const string NOT_LIKE_CONDITION = "NOT LIKE";
        private const string IS_NULL_CONDITION = "IS NULL";
        private const string IS_NOT_NULL_CONDITION = "IS NOT NULL";

        private string _lastGeneratedConditions = "";
        private DatabaseObject _table;

        public FilterConditionsPanel()
        {
            InitializeComponent();
        }

        public void LoadColumns(DatabaseObject table)
        {
            _table = table;
            DatabaseOperations databaseOperations = new DatabaseOperations(Database.Instance);
            IList<string> columns = databaseOperations.GetColumnNames(table);
            tableLayoutPanelColumns.SuspendLayout();
            foreach (string column in columns)
            {
                int row = tableLayoutPanelColumns.RowCount;
                tableLayoutPanelColumns.Controls.Add(NewComboBoxAndOr(), 0, row);
                Label labelColumnName = NewLabelColumnName();
                labelColumnName.Text = column;
                tableLayoutPanelColumns.Controls.Add(labelColumnName, 1, row);
                tableLayoutPanelColumns.Controls.Add(NewComboBoxOperator(), 2, row);
                tableLayoutPanelColumns.Controls.Add(NewTextBoxValue(), 3, row);
                tableLayoutPanelColumns.RowStyles.Add(new RowStyle());
                tableLayoutPanelColumns.RowCount = row + 1;
            }
            tableLayoutPanelColumns.ResumeLayout(false);
            tableLayoutPanelColumns.PerformLayout();
        }

        private ComboBox NewComboBoxAndOr()
        {
            ComboBox result = new ComboBox()
            {
                Name = "comboBoxAndOr",
                Size = new Size(80, 24)
            };
            result.Items.AddRange(new object[] {
            AND_CONDITION,
            OR_CONDITION});
            result.SelectedIndex = 0;
            return result;
        }

        private ComboBox NewComboBoxOperator()
        {
            ComboBox result = new ComboBox()
            {
                Name = "comboBoxOperator",
                Size = new Size(90, 24)
            };
            result.Items.AddRange(new object[]
            {
                EMPTY_CONDITION,
                EQUAL_CONDITION,
                NOT_EQUAL_CONDITION,
                GREATER_EQUAL_CONDITION,
                GREATER_CONDITION,
                LESS_CONDITION,
                LESS_GREATER_CONDITION,
                LIKE_CONDITION,
                NOT_LIKE_CONDITION,
                IS_NULL_CONDITION,
                IS_NOT_NULL_CONDITION
            });
            result.SelectedIndex = 0;
            return result;
        }

        private Label NewLabelColumnName()
        {
            return new Label()
            {
                AutoSize = true,
                Name = "labeColumnName",
                Padding = new Padding(0, 7, 0, 0)
            };
        }

        private TextBox NewTextBoxValue()
        {
            return new TextBox()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Name = "textBoxValue"
            };
        }

        private void buttonGenerateConditions_Click(object sender, EventArgs e)
        {
            if (_lastGeneratedConditions != textBoxConditions.Text)
            {
                if (MessageBox.Show("Generating conditions will overwrite written text. Are you sure, you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }
            IList<string> andList = new List<string>();
            IList<string> orList = new List<string>();
            for (int i = 1; i < tableLayoutPanelColumns.RowCount; i++)
            {
                Control comboBoxOperator = tableLayoutPanelColumns.GetControlFromPosition(2, i);
                if (comboBoxOperator.Text == EMPTY_CONDITION)
                {
                    continue;
                }
                Control comboBoxAndOr = tableLayoutPanelColumns.GetControlFromPosition(0, i);
                Control labelColumnname = tableLayoutPanelColumns.GetControlFromPosition(1, i);
                Control textBoxValue = tableLayoutPanelColumns.GetControlFromPosition(3, i);
                string condition = $"{_table.NameWithSchemaBrackets}.[{labelColumnname.Text}] {comboBoxOperator.Text}";
                if (comboBoxOperator.Text != IS_NULL_CONDITION && comboBoxOperator.Text != IS_NOT_NULL_CONDITION)
                {
                    condition += $"'{textBoxValue.Text.Replace("'", "''")}'";
                }
                if (comboBoxAndOr.Text == AND_CONDITION)
                {
                    andList.Add(condition);
                }
                else
                {
                    orList.Add(condition);
                }
            }
            string andCondition = string.Join(" AND ", andList);
            string orCondition = string.Join(" OR ", orList);
            if (andCondition.Length > 0)
            {
                _lastGeneratedConditions = andCondition;
                if (orCondition.Length > 0)
                {
                    _lastGeneratedConditions += " AND (" + orCondition + ")";
                }
            }
            else
            {
                _lastGeneratedConditions = orCondition;
            }
            textBoxConditions.Text = _lastGeneratedConditions;
        }
    }
}
