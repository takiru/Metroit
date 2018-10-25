using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tutorial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            metTextBox1.Error = !metTextBox1.Error;
            metLimitedTextBox1.Error = !metLimitedTextBox1.Error;
            metNumericTextBox1.Error = !metNumericTextBox1.Error;
            metComboBox1.Error = !metComboBox1.Error;
            metDateTimePicker1.Error = !metDateTimePicker1.Error;
        }
    }
}
