using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CANToolApp
{
    public partial class GaugeboardShow : Form
    {
        public GaugeboardShow()
        {
            InitializeComponent();
        }

        private void gauge1_ValueInRangeChanged(object sender, Gauge.ValueInRangeChangedEventArgs e)
        {
            if (e.valueInRange == 0)
            {
                picturebox1.BackColor = Color.Green;
            }
            else
            {
                picturebox1.BackColor = Color.Red;
            }
        }

        private void trackbar1_ValueChanged(object sender, EventArgs e)
        {
            gauge1.Value = trackBar1.Value;


            textBox1.Text = gauge1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gauge1.NeedleType == 0)
            {
                gauge1.NeedleType = 1;
            }
            else
            {
                gauge1.NeedleType = 0;
            }

        }
    }
}
