using Metroit.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample
{
    public partial class ControlSampleForm : Form
    {
        public ControlSampleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            metDateTimePicker1.ReadOnlyLabel = !metDateTimePicker1.ReadOnlyLabel;
            dynaDtp.ReadOnlyLabel = !dynaDtp.ReadOnlyLabel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            metDateTimePicker1.Value = new DateTime(2010, 1, 1);
            dynaDtp.Value = new DateTime(2010, 1, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            metDateTimePicker1.Value = null;
            dynaDtp.Value = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            metDateTimePicker1.Format = DateTimePickerFormat.Long;
            dynaDtp.Format = DateTimePickerFormat.Long;
        }

        private MetDateTimePicker dynaDtp = null;
        private void button5_Click(object sender, EventArgs e)
        {
            dynaDtp = new MetDateTimePicker();
            dynaDtp.ReadOnlyLabel = true;
            dynaDtp.AcceptNull = true;
            dynaDtp.Location = button5.Location;
            dynaDtp.Top = button5.Height + button5.Top;
            dynaDtp.ValueChanged += (s, ex) =>
            {
                if (dynaDtp.Value.HasValue)
                {
                    Console.WriteLine("dynaDtp ValueChanged:" + dynaDtp.Value.Value.ToString("yyyy/MM/dd HH:mm:ss.fffff"));
                }
                else
                {
                    Console.WriteLine("dynaDtp ValueChanged:null");
                }
            };
            tabPage1.Controls.Add(dynaDtp);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Value:" + metDateTimePicker1.Value.ToString());
        }

        private void metDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (metDateTimePicker1.Value.HasValue)
            {
                Console.WriteLine("ValueChanged:" + metDateTimePicker1.Value.Value.ToString("yyyy/MM/dd HH:mm:ss.fffff"));
            } else
            {
                Console.WriteLine("ValueChanged:null");
            }
        }
    }
}
