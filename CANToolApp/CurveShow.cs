using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CANToolApp
{

    public partial class CurveShow : Form
    {
        private Queue<double> dataQueue = new Queue<double>(100);
        private int num = 1;//每次删除增加几个点
        Dictionary<string, string> returnedData = new Dictionary<string, string>();

        public event DelegateUpdateUI delegateUpdateUI;
        private event DelegateSendData delegateSendData;
        string messgaeName = "";
        string signaleName = "";
        List<string> signals = new List<string>();
        string lastname = "";
        double currentValue = 0;


        public CurveShow()
        {
            InitializeComponent();
            cleardata();
        }


        private void CurveShow_Load(object sender, EventArgs e)
        {
            //InitTreeView(treeView1,0);
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.timer1.Start();
            this.SignalChangedChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.ff";
        }

        //初始化图表
        private void InitChart()
        {
            //定义图表区域
            this.SignalChangedChart.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.SignalChangedChart.ChartAreas.Add(chartArea1);
            //定义存储和显示数据点的容器
            this.SignalChangedChart.Series.Clear();
            Series series1 = new Series("S1");
            this.SignalChangedChart.Series.Add(series1);

            //设置图表显示样式
            this.SignalChangedChart.ChartAreas[0].AxisY.Minimum = 0;
            this.SignalChangedChart.ChartAreas[0].AxisY.Maximum = 0;
            this.SignalChangedChart.ChartAreas[0].AxisX.Interval = 5;//设置横坐标间隔为1，使得每个刻度间均匀分开
            this.SignalChangedChart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.SignalChangedChart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;


            this.SignalChangedChart.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
     
            //设置标题
            this.SignalChangedChart.Titles.Clear();
            this.SignalChangedChart.Titles.Add("S01");
            this.SignalChangedChart.Titles[0].Text = "CAN信号显示";
            this.SignalChangedChart.Titles[0].ForeColor = Color.RoyalBlue;
            this.SignalChangedChart.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            DateTime time = DateTime.Now;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            SignalChangedChart.DoubleClick += SignalChangedChart_DoubleClick;

            SignalChangedChart.ChartAreas[0].AxisX.ScaleView.Size = 5;//视野范围内共有多少个数据点
            SignalChangedChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true; //指滚动条位于图表区内还是图表区外
            SignalChangedChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

        }


        //定时器事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue();
            this.SignalChangedChart.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                // this.SignalChangedChart.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
                this.SignalChangedChart.Series[0].Points.AddXY(DateTime.Now, dataQueue.ElementAt(i));
                SignalChangedChart.ChartAreas[0].AxisX.ScaleView.Position = SignalChangedChart.Series[0].Points.Count - 5;
            }
        }

        //chart的双击事件，用来恢复隐藏的滑动条
        private void SignalChangedChart_DoubleClick(object sender, EventArgs e)
        {
            SignalChangedChart.ChartAreas[0].AxisX.ScaleView.Size = 5;//视野范围内共有多少个数据点
            SignalChangedChart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true; //指滚动条位于图表区内还是图表区外
            SignalChangedChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
        }

        //更新队列中的值
        private void UpdateQueueValue()
        {
            if (!lastname.Equals(signaleName))
            {
      
                lastname = signaleName;
                //Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++---" + signaleName);
            }
            //Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++"+dataQueue.Count);

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
        public void UpdateSignal(List<string> signals)
        {
            if (signals != null && signals.Count > 0)
            {
                this.signaleName = signals[0];
                label1.Text = "当前显示的signal：" + signaleName;
            }
            else
            {
                label1.Text = "未选择信号";
            }
            
        }

      
        /*  private void InitTreeView(TreeView treeview,int parentId)
          {
              returnedData = Decode.DecodeCANSignal("t3588A5SD566D9F8SD565");
              foreach (string key in returnedData.Keys)
              {
                  if (key == "messageName")
                  {   
                          TreeNode messagenode = new TreeNode();
                          messagenode.Text = returnedData[key];
                          //treeView1.Nodes.Add(messagenode);               
                  }
              }
              foreach (string key in returnedData.Keys)
              {
                  if (key != "messageName")
                  {   
                          TreeNode signalnode = new TreeNode();

                          signalnode.Text = key;
                          //treeView1.Nodes.Add(signalnode);
                  }
              }
              treeview.MouseClick += Treeview_MouseClick;

          }


          private void Treeview_MouseClick(object sender, MouseEventArgs e)
          {
              TreeView tv = (TreeView)sender;
              TreeNode tn = tv.GetNodeAt(new Point(e.X,e.Y));
              signaleName = tn.Text;
          }

          private void Treeview_Click(object sender, EventArgs e)
          {
              /*TreeView tv = (TreeView)sender;
              TreeNode tn=tv.GetNodeAt();
              signaleName = tn.Text;*/

        //  }


    }




        

      
}

