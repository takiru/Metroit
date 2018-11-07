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

namespace Tutorial
{
    public partial class Form1 : MetForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_ControlLeaving(object sender, CancelEventArgs e)
        {
            // DataGridView のオブジェクトは、標準動作でフォーカスアウトさせる
            var control = this.ActiveControl;
            if (control is DataGridViewTextBoxEditingControl || control is DataGridViewComboBoxEditingControl)
            {
                e.Cancel = true;
            }
        }
    }
}
