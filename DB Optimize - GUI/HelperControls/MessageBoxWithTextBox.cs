using System.Windows.Forms;

namespace DB_Optimize.GUI.HelperControls
{
    public partial class MessageBoxWithTextBox : Form
    {
        public MessageBoxWithTextBox(IWin32Window owner, string text, string caption, string buttonText)
        {
            InitializeComponent();
            Owner = FromHandle(owner.Handle).FindForm();
            richTextBoxText.Text = text;
            Text = caption;
            buttonPrimary.Text = buttonText;
        }
    }
}
