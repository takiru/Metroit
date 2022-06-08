using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public class NumericUpDownEx : NumericUpDown
    {
        protected override void OnLostFocus(EventArgs e)
        {
            Console.WriteLine("LostFocus");
            base.OnLostFocus(e);
        }
    }
}
