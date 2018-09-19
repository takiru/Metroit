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
    }
}
