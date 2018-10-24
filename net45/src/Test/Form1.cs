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

namespace Test
{
    public partial class Form1 : MetForm, IControlRollback
    {
        public bool IsRollbacked(object sender, Control control)
        {
            if (this.ActiveControl is DataGridViewTextBoxEditingControl)
            {
                return false;
            }

            return true;
        }

        public void Rollback(object sender, Control control)
        {
            var c = this.ActiveControl as DataGridViewTextBoxEditingControl;
            c.Text = "ddd";
        }

        private void Form1_ControlRollbacking(object sender, CancelEventArgs e)
        {
            if (this.ActiveControl is DataGridViewTextBoxEditingControl)
            {
                e.Cancel = true;
            }
        }

        protected override void OnControlLeaving(CancelEventArgs e)
        {
            base.OnControlLeaving(e);

            // グリッドのテキストエリアはLeave対象外
            if (this.ActiveControl is DataGridViewTextBoxEditingControl)
            {
                Console.WriteLine("Cancel!");
                e.Cancel = true;
            }
        }

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
            row["Column1"] = "文字かなAC column1B";
            row["Column2"] = "文字かなAC column2B";
            row["Column3"] = "文字かなAC column3B";
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
            metComboBox1.Error = !metComboBox1.Error;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            metTextBox1.ReadOnlyLabel = !metTextBox1.ReadOnlyLabel;
            metDateTimePicker1.ReadOnlyLabel = !metDateTimePicker1.ReadOnlyLabel;
            metComboBox1.ReadOnlyLabel = !metComboBox1.ReadOnlyLabel;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            metTextBox1.ReadOnly = !metTextBox1.ReadOnly;
            metDateTimePicker1.ReadOnly = !metDateTimePicker1.ReadOnly;
            metComboBox1.ReadOnly = !metComboBox1.ReadOnly;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            metTextBox1.Visible = !metTextBox1.Visible;
            metDateTimePicker1.Visible = !metDateTimePicker1.Visible;
            metComboBox1.Visible = !metComboBox1.Visible;
        }

        private void metTextBox3_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("changed:" + metTextBox3.Text);
        }

        private void metTextBox3_TextChangeValidation(object sender, Metroit.Windows.Forms.TextChangeValidationEventArgs e)
        {
            Console.WriteLine("validtion:" + e.After);
            if (e.After == "test")
            {
                e.Cancel = true;
            }
            if (e.After == "あ")
            {
                e.Cancel = true;
            }
        }

        private void metTextBox1_TextChangeValidation(object sender, Metroit.Windows.Forms.TextChangeValidationEventArgs e)
        {
            //Console.WriteLine("kita");
            //if (e.After == "かなac")
            if (e.After == "かなa")
            {
                e.Cancel = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("kita:" + textBox1.Text);
        }

        private void metTextBox4_TextChangeValidation(object sender, Metroit.Windows.Forms.TextChangeValidationEventArgs e)
        {
            Console.WriteLine("Validtin:" + e.After);
        }

        private void metTextBox4_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Changed:" + metTextBox4.Text);
        }

        private void metTextBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("Enter");
        }
    }
}
