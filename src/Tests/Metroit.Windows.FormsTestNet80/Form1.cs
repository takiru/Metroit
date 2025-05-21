using Metroit.Windows.Forms;

namespace Metroit.Windows.FormsTestNet80
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
            metLimitedTextBox1.MaxLength = (int)(metNumericTextBox1.Value ?? 0m);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            metMultilineLimitedTextBox1.AutoFocus = checkBox4.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            metMultilineLimitedTextBox1.FullWidthCharTwo = checkBox3.Checked;
        }

        private void metNumericTextBox3_TextChanged(object sender, EventArgs e)
        {
            metMultilineLimitedTextBox1.MaxLineCount = (int)(metNumericTextBox3.Value ?? 0m);
        }

        private void metNumericTextBox2_TextChanged(object sender, EventArgs e)
        {
            metMultilineLimitedTextBox1.MaxLineLength = (int)(metNumericTextBox2.Value ?? 0m);
        }

        private void OverlayShowButton_Click(object sender, EventArgs e)
        {
            var overlay = new MetOverlay();
            overlay.Show(groupBox2, (token) =>
            {
                for (int i = 0; i <= 3000; i++)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        metMultilineLimitedTextBox1.Text = i.ToString();
                    }));
                }
            });
        }
    }
}
