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

        public CurveShow()
        {
            InitializeComponent();
        }


        private void CurveShow_Load(object sender, EventArgs e)
        {
            InitTreeView(treeView1,0);
            this.timer1.Start();
        }

        private void InitTreeView(TreeView treeview,int parentId)
        {
            Dictionary<string, string> returnedData = Decode.DecodeCANSignal("t3588A5SD566D9F8SD565");
            foreach (string key in returnedData.Keys)
            {
                if (key == "messageName")
                {   
                        TreeNode messagenode = new TreeNode();
                        messagenode.Text = returnedData[key];
                        treeView1.Nodes.Add(messagenode);               
                }
            }
            foreach (string key in returnedData.Keys)
            {
                if (key != "messageName")
                {   
                        TreeNode signalenode = new TreeNode();
                        signalenode.Text = key;
                        treeView1.Nodes.Add(signalenode);
                }
            }
   
        }
        //定时器事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateQueueValue();
            this.SignalChangedChart.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                this.SignalChangedChart.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
            }
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
            this.SignalChangedChart.ChartAreas[0].AxisX.Interval = 5;
            this.SignalChangedChart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.SignalChangedChart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.SignalChangedChart.Titles.Clear();
            this.SignalChangedChart.Titles.Add("S01");
            this.SignalChangedChart.Titles[0].Text = "XXX显示";
            this.SignalChangedChart.Titles[0].ForeColor = Color.RoyalBlue;
            this.SignalChangedChart.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);  
        }
        //更新队列中的值
        private void UpdateQueueValue()
        {
            if (dataQueue.Count > 100)
            {
                //先出列
                for (int i = 0; i < num; i++)
                {
                    dataQueue.Dequeue();
                }
            }
           
                Random r = new Random();//先用随机数测试
                for (int i = 0; i < num; i++)
                {
                    dataQueue.Enqueue(r.Next(0, 100));
                }
            
            
            }
        }

      
    }

