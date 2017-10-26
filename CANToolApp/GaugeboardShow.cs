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
        private Queue<double> dataQueue = new Queue<double>(100);
        private int num = 1;//每次删除增加几个点
        Dictionary<string, string> returnedData = new Dictionary<string, string>();

        string messgaeName = "";
        string signaleName = "";
        string lastname = "";
        double currentValue = 0;

        private event DelegateSendData delegateSendData;

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

        private void GaugeboardShow_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue();
            //this.SignalChangedChart.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                //this.SignalChangedChart.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
                this.gauge1.Value = (float)currentValue;
            }
        }
        private void UpdateQueueValue()
        {
            if (!lastname.Equals(signaleName))
            {
                dataQueue.Clear();
                lastname = signaleName;
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++---" + signaleName);
            }
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++" + dataQueue.Count);

            if (dataQueue.Count > 100)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue.Dequeue();
                }
            }
            dataQueue.Enqueue(currentValue);

        }


        public void ReceiveData(Dictionary<string, string> returnedData)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateSendData(ReceiveData), returnedData);
            }
            else
            {
                UpdateData(returnedData);
            }
        }

        public void UpdateData(Dictionary<string, string> returnedData)
        {
            this.returnedData = returnedData;
            if (returnedData[signaleName] != null || !returnedData[signaleName].Equals(""))
            {

                string value = returnedData[signaleName];
                value = value.Split(' ')[0];

                currentValue = Double.Parse(value);
            }
        }
        private void cleardata()
        {
            timer1.Stop();
            dataQueue.Clear();
            for (int i = 0; i < 100; i++)
            {
                dataQueue.Enqueue(0);
            }
            timer1.Start();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
