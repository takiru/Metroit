using Metroit.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class DateTimePickerEx : MetDateTimePicker
    {
        protected override void OnValueChanged(EventArgs eventargs)
        {
            //if (this.InnerValueChanged)
            //{
            //    return;
            //}
            Console.WriteLine(this.Value.HasValue ? this.Value.Value.ToLongDateString() : "null");
            base.OnValueChanged(eventargs);
        }
    }
}
