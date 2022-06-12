using Metroit.Data.Extensions;
using Metroit.Extensions;
using Metroit.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : MetForm, IControlRollback
    {
        public bool IsRollbacked
        {
            get
            {
                if (this.ActiveControl is DataGridViewTextBoxEditingControl)
                {
                    return false;
                }

                return true;
            }
        }

        public void Rollback()
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
            row["Column1"] = "ＣＣＣ column1";
            row["Column2"] = "ＣＣＣ column2";
            row["Column3"] = "ＣＣＣ column3";
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

            var ds = new DataSet();
            ds.Tables.Add(dt);

            //metTextBox1.CustomAutoCompleteBox.DataSource = dt;
            metTextBox1.CustomAutoCompleteBox.DataSource = ds;
            autoCompleteBox1.DataSource = ds;


            metTextBox1.CustomAutoCompleteBox.CandidateSelected += CustomAutoCompleteBox_CandidateSelected;



            var items = new List<ListItem>()
            {
                new ListItem() { Value1 = "aaa1", Value2 = "aaa2" },
                new ListItem() { Value1 = "bbb1", Value2 = "bbb2" },
                new ListItem() { Value1 = "ccc1", Value2 = "ccc2" }
            };
            metTextBox10.CustomAutoCompleteBox.DataSource = items;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            metTextBox1.CustomAutoCompleteBox.DataSource = "test";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            metTextBox1.Text = "bbb column2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            metTextBox1.Text = "bbb column2";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.metTextBox1.CustomAutoCompleteBox.DisplayMember = "Column1";
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

        private void metTextBox1_CompleteBoxOpening(object sender, EventArgs e)
        {
            //var dt = new System.Data.DataTable();
            //dt.Columns.Add("Column1");
            //dt.Columns.Add("Column2");
            //dt.Columns.Add("Column3");

            //var row = dt.NewRow();
            //row["Column1"] = "aaa column1";
            //row["Column2"] = "aaa column2";
            //row["Column3"] = "aaa column3";
            //dt.Rows.Add(row);

            //metTextBox1.CustomAutoCompleteBox.DataSource = dt;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(metTextBox1.CustomAutoCompleteBox.SelectedValue?.ToString());
            Console.WriteLine("SelectedItem：" + metTextBox1.CustomAutoCompleteBox.SelectedItem);
            Console.WriteLine("SelectedValue：" + metTextBox1.CustomAutoCompleteBox.SelectedValue);
            Console.WriteLine("SelectedIndex：" + metTextBox1.CustomAutoCompleteBox.SelectedIndex);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            metTextBox5.Enabled = !metTextBox5.Enabled;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            metTextBox5.ReadOnly = !metTextBox5.ReadOnly;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            metTextBox5.ReadOnlyLabel = !metTextBox5.ReadOnlyLabel;
        }

        private void metTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("TextChanged");

            ////Console.WriteLine("index:" + metTextBox1.CustomAutoCompleteBox.SelectedIndex);

            //var value = metTextBox1.CustomAutoCompleteBox.SelectedValue;
            //if (value != null)
            //{
            //    Console.WriteLine("value:" + value.ToString());
            //}
            //else
            //{
            //    Console.WriteLine("value:null");
            //}

            var item = metTextBox1.CustomAutoCompleteBox.SelectedItem;
            if (item != null)
            {
                //Console.WriteLine("item:" + item.ToString());
                Console.WriteLine("TextChanged index:" + metTextBox1.CustomAutoCompleteBox.SelectedIndex + ", item details:" + ((DataRow)item)["Column3"].ToString());
            }
            else
            {
                Console.WriteLine("TextChanged index:" + metTextBox1.CustomAutoCompleteBox.SelectedIndex + ", item:null");
            }

            //var row = metTextBox1.CustomAutoCompleteBox.SelectedItem as DataRowView;
            //if (row != null)
            //{
            //    Console.WriteLine(row["Column3"].ToString());
            //}
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //metTextBox1.CustomAutoCompleteBox.Open(metTextBox1.Text);
            metTextBox1.CustomAutoCompleteBox.Open("bbb");
        }

        private void metTextBox1_Validating(object sender, CancelEventArgs e)
        {
            Console.WriteLine("TextBox Validating");
        }

        private void metTextBox1_Validated(object sender, EventArgs e)
        {
            Console.WriteLine("TextBox Validated");
        }

        private void CustomAutoCompleteBox_CandidateSelected(object sender, CandidateSelectedEventArgs item)
        {
            Console.WriteLine("CustomAutoCompleteBox_CandidateSelected");
        }

        private void metTextBox1_TextChangeValidation(object sender, Metroit.Windows.Forms.TextChangeValidationEventArgs e)
        {
            //Console.WriteLine("TextBox TextChangeValidation");
        }

        private void button14_Click(object sender, EventArgs e)
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

            metTextBox1.CustomAutoCompleteBox.DataSource = dt;

            Console.WriteLine(metTextBox1.CustomAutoCompleteBox.SelectedIndex);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //metTextBox1.CustomAutoCompleteBox.SelectedIndex = 5;
        }

        private void metTextBox9_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("TextChanged");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Console.WriteLine("SelectedItem：" + metTextBox10.CustomAutoCompleteBox.SelectedItem);
            Console.WriteLine("SelectedValue：" + metTextBox10.CustomAutoCompleteBox.SelectedValue);
            Console.WriteLine("SelectedIndex：" + metTextBox10.CustomAutoCompleteBox.SelectedIndex);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            metTextBox10.Text = "ccc1";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            comboBox1.DataSource = "test";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            autoCompleteBox1.Open(textBox3.Text);
        }

        private bool candidateSelectedValueChanging = false;
        private void autoCompleteBox1_CandidateSelected(object sender, CandidateSelectedEventArgs e)
        {
            // 選択された候補の値をテキストに表示する
            candidateSelectedValueChanging = true;
            textBox3.Text = e.SelectedText;
            candidateSelectedValueChanging = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (autoCompleteBox1.IsOpened && !candidateSelectedValueChanging)
            {
                autoCompleteBox1.Extract(textBox3.Text);

                // 候補が0件の時はドロップダウンを閉じる
                // 候補が1件のみで、その候補が選択状態になった時はドロップダウンを閉じる
                if ((this.autoCompleteBox1.GetCandidateCount() == 0) ||
                    (this.autoCompleteBox1.GetCandidateCount() == 1 && this.autoCompleteBox1.SelectedItem != null))
                {
                    this.autoCompleteBox1.Close();
                }
            }


        }

        private void metTextBox1_CandidateBoxOpening(object sender, EventArgs e)
        {
            Console.WriteLine("metTextBox1_CandidateBoxOpening");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var result = "(aaa(bbb(ccc)ddd)eee)";
            result = result.GetEnclosedText();
            Console.WriteLine(result);
            result = result.GetEnclosedText();
            Console.WriteLine(result);
            result = result.GetEnclosedText();
            Console.WriteLine(result);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            metDateTimePicker3.Value = new DateTime(2020, 12, 5);
        }

        private void metDateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ValueChanged");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            metDateTimePicker3.MinCalendarType = CalendarType.Day;
            metDateTimePicker3.ShowToday = true;
            metDateTimePicker3.ShowTodayCircle = true;
            //metDateTimePicker3.ShowNearMonthDays = true;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            metDateTimePicker3.MinCalendarType = CalendarType.Month;
        }

        private void dateTimePickerEx1_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Ex ValueChanged");
        }

        private void metTextBox10_Validating(object sender, CancelEventArgs e)
        {
            Console.WriteLine("metTextBox10 Validating");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            metTextBox10.CustomAutoCompleteBox.Open(metTextBox10.Text);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            var simpleLabel = new Label();
            Console.WriteLine(simpleLabel.AutoSize.ToString());
            simpleLabel.Location = new Point(50, 50);
            this.Controls.Add(simpleLabel);
            Console.WriteLine(simpleLabel.AutoSize.ToString());

            var metLabel = new MetLabel();
            metLabel.Location = new Point(10, 10);
            metLabel.Text = "CodeBase";
            Console.WriteLine(metLabel.AutoSize.ToString());
            this.Controls.Add(metLabel);
            Console.WriteLine(metLabel.AutoSize.ToString());

            var c = new SqlConnection(@"XXXX");
            c.Open();

            var com = c.CreateCommand();
            com.CommandText = @"SELECT * FROM XXXX";

            var r = com.ExecuteReader();
            r.Read();
            var item = r.ToEntity<RowItem>();

        }

        private void metNumericTextBox1_Validating(object sender, CancelEventArgs e)
        {
            Console.WriteLine($"Validating Text:{metNumericTextBox1.Text}");
        }

        private void metNumericTextBox1_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"ValueChanged Value:{metNumericTextBox1.Value}");
        }

        private void metNumericTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine($"TextChanged Value:{metNumericTextBox1.Text}");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"MenuStrip:{metNumericTextBox1.Value}");
            Console.WriteLine($"MenuStrip numericUpDownEx1:{numericUpDownEx1.Value}");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            metNumericTextBox1.Value = -20;
        }

        private void metNumericTextBox1_TextChangeValidation(object sender, TextChangeValidationEventArgs e)
        {
            Console.WriteLine($"TextChangeValiation:Input={e.Input}, Before={e.Before}, After={e.After}");
        }

        private void metTextBox11_TextChangeValidation(object sender, TextChangeValidationEventArgs e)
        {
            Console.WriteLine("kita");
        }

        private void metTextBox12_TextChangeValidation(object sender, TextChangeValidationEventArgs e)
        {
            Console.WriteLine("kita2");
        }
    }

    class RowItem
    {
        [Column("ent_date")]
        public DateTime EntDate { get; set; }

        [Column("ent_user")]
        public string EntUser { get; set; }

        [Column("upd_date")]
        public DateTime? UpdDate { get; set; }

        [Column("upd_user")]
        public string UpdUser { get; set; }

        [Column("table_name")]
        public DateTime TableName { get; set; }
    }
}
