using LedDigitalDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CANToolApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Queue<double> dataQueue = new Queue<double>(100);
        private int num = 1;//每次删除增加几个点
        Dictionary<string, string> returnedData = new Dictionary<string, string>();
        string messgaeName = "";
        string signaleName = "";
        string lastname = "";
        double currentValue = 0;
        float min = 0;
        float max = 100;

        private event CANToolApp.DelegateSendData delegateSendData;


        MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = MainWindowViewModel.Instance;
            DataContext = _viewModel;

            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _viewModel.LedDigitalValue1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UpdateQueueValue();
                _viewModel.LedDigitalValue= currentValue.ToString();
                _viewModel.DigitalCount = 12;

                this.Title = "LED:"+signaleName;


            });
        }


        public void UpdateSignal(List<string> signals)
        {
            if (signals != null && signals.Count > 0)
            {
                string[] tmparr = signals[0].Split(' ');
                foreach (string str in tmparr)
                {
                    Console.WriteLine(str);
                }
                this.signaleName = tmparr[1];
                string range = tmparr[3].Substring(1, tmparr[3].Length - 2);
                min = float.Parse(range.Split('|')[0]);
                max = float.Parse(range.Split('|')[1]);
                //label1.Text = signaleName + ":";
                Console.WriteLine(max);
                Console.WriteLine(min);
                this.currentValue = float.Parse(tmparr[2]);

            }
            else
            {
                //label1.Text = "未选择信号";
            }

        }
        private void UpdateQueueValue()
        {
            if (!lastname.Equals(signaleName))
            {
                lastname = signaleName;


            }

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


        

        public void UpdateData(Dictionary<string, string> returnedData)
        {
            this.returnedData = returnedData;
            try {
                if (returnedData[signaleName] != null || !returnedData[signaleName].Equals(""))
                {
                    string value = returnedData[signaleName];
                    value = value.Split(' ')[0];

                    currentValue = Double.Parse(value);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("未接收到当前选择的信号值");
            }
}
        private void cleardata()
        {
            //timer1.Stop();
            dataQueue.Clear();
            for (int i = 0; i < 100; i++)
            {
                dataQueue.Enqueue(0);
            }
            //timer1.Start();
        }

    }
}
