using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Columns.Add("Column3");

            var row = dt.NewRow();
            row["Column1"] = "aaa column1";
            row["Column2"] = "aaa column2";
            row["Column3"] = "aaa column3";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Column1"] = "bbb column1";
            row["Column2"] = "bbb column2";
            row["Column3"] = "bbb column3";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Column1"] = "aaA column1";
            row["Column2"] = "aaA column2";
            row["Column3"] = "aaA column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "文字カナa column1";
            row["Column2"] = "文字カナa column2";
            row["Column3"] = "文字カナa column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "文字ｶﾅA column1";
            row["Column2"] = "文字ｶﾅA column2";
            row["Column3"] = "文字ｶﾅA column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "文字かなA column1";
            row["Column2"] = "文字かなA column2";
            row["Column3"] = "文字かなA column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "文字かなAB column1";
            row["Column2"] = "文字かなAB column2";
            row["Column3"] = "文字かなAB column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "文字かなAB column1B";
            row["Column2"] = "文字かなAB column2B";
            row["Column3"] = "文字かなAB column3B";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Column1"] = "ccc column1";
            row["Column2"] = "ccc column2";
            row["Column3"] = "ccc column3";
            dt.Rows.Add(row);

            metTextBox1.CustomAutoCompleteBox.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine(metTextBox1.CustomAutoCompleteBox.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            metTextBox1.Text = "bbb column2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            metTextBox1.Width -= 10;
            //metTextBox2.Width -= 10;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.metTextBox1.ReadOnlyLabel = !this.metTextBox1.ReadOnlyLabel;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            metTextBox1.Error = !metTextBox1.Error;
            metDateTimePicker1.Error = !metDateTimePicker1.Error;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            metTextBox1.ReadOnlyLabel = !metTextBox1.ReadOnlyLabel;
            metDateTimePicker1.ReadOnlyLabel = !metDateTimePicker1.ReadOnlyLabel;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            metTextBox1.ReadOnly = !metTextBox1.ReadOnly;
            metDateTimePicker1.ReadOnly = !metDateTimePicker1.ReadOnly;
        }
    }
}
