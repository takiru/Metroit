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

        private void ControlSampleForm_Load(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");

            var row = dt.NewRow();
            row["Column1"] = "Sample1 Display";
            row["Column2"] = "Sample1 Value";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Column1"] = "Sample2 Display";
            row["Column2"] = "Sample2 Value";
            dt.Rows.Add(row);

            listBox1.DataSource = dt;
            listBox1.DisplayMember = "Column1";
            listBox1.ValueMember = "Column2";

            metTextBox1.CustomAutoCompleteMode = CustomAutoCompleteMode.KeysSuggest;
            metTextBox1.CustomAutoCompleteBox.DataSource = dt;
            metTextBox1.CustomAutoCompleteBox.DisplayMember = "Column1";
            metTextBox1.CustomAutoCompleteBox.ValueMember = "Column2";
        }

        private void metTextBox1_CandidateSelected(object sender, CandidateSelectedEventArgs e)
        {
            var row = (DataRow)e.SelectedItem;
            MessageBox.Show(row["Column2"].ToString());
        }
    }
}
