using Metroit.Windows.Forms;
using System.Diagnostics;

namespace Metroit.Windows.FormsTest
{
    public partial class Form1 : MetForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            metLimitedTextBox1.AutoFocus = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            metLimitedTextBox1.FullWidthCharTwo = checkBox2.Checked;
        }

        private void metNumericTextBox1_TextChanged(object sender, EventArgs e)
        {
            metLimitedTextBox1.MaxLength = (int)metNumericTextBox1.Value;
        }

        private void metLimitedTextBox1_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(metLimitedTextBox1.Text);
        }
    }
}
