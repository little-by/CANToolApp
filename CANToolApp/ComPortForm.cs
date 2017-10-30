using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using INIFILE;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

namespace CANToolApp
{
    public partial class ComPortForm : Form
    {
        private List<CANSignalObject> canSignal = new List<CANSignalObject>();
        private List<CANMessageObject> canMessage = new List<CANMessageObject>();
        private double signal_C = 0, signal_D = 0;
        //SerialPort类用于控制串行端口文件资源
        SerialPort sp1 = new SerialPort();
        public event DelegateUpdateUI delegateUpdateUI;
        //sp1.ReceivedBytesThreshold = 1;//只要有1个字符送达端口时便触发DataReceived事件 

        public ComPortForm()
        {

            InitializeComponent();
        }

        //加载
        private void Form1_Load(object sender, EventArgs e)
        {
            INIFILE.Profile.LoadProfile();//加载所有

            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。");
                        return;
                    }
            }
            //预置数据位
            switch (Profile.G_DATABITS)
            {
                case "5":
                    cbDataBits.SelectedIndex = 0;
                    break;
                case "6":
                    cbDataBits.SelectedIndex = 1;
                    break;
                case "7":
                    cbDataBits.SelectedIndex = 2;
                    break;
                case "8":
                    cbDataBits.SelectedIndex = 3;
                    break;
                default:
                    {
                        MessageBox.Show("数据位预置参数错误。");
                        return;
                    }
            }
            //预置停止位
            switch (Profile.G_STOP)
            {
                case "1":
                    cbStop.SelectedIndex = 0;
                    break;
                case "1.5":
                    cbStop.SelectedIndex = 1;
                    break;
                case "2":
                    cbStop.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("停止位预置参数错误。");
                        return;
                    }
            }

            //预置校验位
            switch (Profile.G_PARITY)
            {
                case "None":
                    cbParity.SelectedIndex = 0;
                    break;
                case "Odd":
                    cbParity.SelectedIndex = 1;
                    break;
                case "Even":
                    cbParity.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("校验位预置参数错误。");
                        return;
                    }
            }
            //预置CAN速率
            switch (Profile.G_RATE)
            {
                case "10":
                    canRate.SelectedIndex = 0;
                    break;
                case "20":
                    canRate.SelectedIndex = 1;
                    break;
                case "50":
                    canRate.SelectedIndex = 2;
                    break;
                case "100":
                    canRate.SelectedIndex = 3;
                    break;
                case "125":
                    canRate.SelectedIndex = 4;
                    break;
                case "250":
                    canRate.SelectedIndex = 5;
                    break;
                case "500":
                    canRate.SelectedIndex = 6;
                    break;
                case "800":
                    canRate.SelectedIndex = 7;
                    break;
                case "1024":
                    canRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("CAN速率预置参数错误。");
                        return;
                    }
            }

            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.Write(s);
                cbSerial.Items.Add(s);
            }

            //message
            string sql_m = "select * from cantoolapp.canmessage";
            SqlHelper.connect();
            SqlDataReader sqldr_m = SqlHelper.query(sql_m);
            while (sqldr_m.Read())
            {
                string messageName = (string)sqldr_m["messagename"];
                CANMessageObject temp = new CANMessageObject();
                temp.MessageName = messageName.ToCharArray();
                temp.Id = Convert.ToUInt32(sqldr_m["id"]);
                canMessage.Add(temp);
                message.Items.Add(messageName);
            }
            SqlHelper.close();

            //串口设置默认选择项
            //cbSerial.SelectedIndex = 1;         //note：获得COM9口，但别忘修改

            Control.CheckForIllegalCrossThreadCalls = false;
            //这个类中我们不检查跨线程的调用是否合法(因为.net 2.0以后加强了安全机制,，不允许在winform中直接跨线程访问控件的属性)
            sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);
            //sp1.ReceivedBytesThreshold = 1;

            //准备就绪              
            sp1.DtrEnable = true;
            sp1.RtsEnable = true;
            //设置数据读取超时为1秒
            sp1.ReadTimeout = 1000;
            sp1.Close();
        }

        string error = "" + (char)7;

        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp1.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                byte[] byteRead = new byte[sp1.BytesToRead];    //BytesToRead:sp1接收的字符个数
                int msglen = sp1.BytesToRead;
                sp1.Read(byteRead, 0, msglen);
                string msgrec = System.Text.Encoding.Default.GetString(byteRead);
                //MessageBox.Show(msgrec);
                if(msgrec != "" && msgrec != null)
                {
                   
                    string sPattern = "^(t|T)(\\d|[ABCDEF]){3,}";
                    if (System.Text.RegularExpressions.Regex.IsMatch(msgrec, sPattern))
                    {
                        System.Console.WriteLine(" - valid");
                        delegateUpdateUI(msgrec);
                    }
                    else if (msgrec.Equals("\r"))
                    {
                        MessageBox.Show("Success!", "Success");
                    }
                    else if (msgrec.Equals(error))
                    {
                        MessageBox.Show("Failure!", "Error");
                        canClose.Enabled = true;
                        canOpen.Enabled = true;
                    }
                    else if (msgrec.StartsWith("S"))
                    {
                        MessageBox.Show(msgrec, "Version");
                    }
                    else
                    {
                        System.Console.WriteLine(" - invalid");
                    }
                    
                }
                sp1.DiscardInBuffer();                      //清空SerialPort控件的Buffer 
               
               
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        //发送按钮
        private void btnSend_Click(object sender, EventArgs e)
        {

            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            if (canOpen.Enabled)
            {
                MessageBox.Show("请先打开CANTool装置！", "Error");
                return;
            }
            string strMessage = "";
            string strSignal = "";
            if (message.SelectedItem == null)
            {
                MessageBox.Show("请选择Message！", "Error");
                return;
            }
            else
            {
                strMessage = (string)message.SelectedItem.ToString();
            }
            if (signal.SelectedItem == null)
            {
                MessageBox.Show("请选择Signal！", "Error");
                return;
            }
            else
            {
                strSignal = (string)signal.SelectedItem.ToString();
            }

            double strSend = -1;
            if (txtSend.Text == "")
            {
                MessageBox.Show("请填写信息！", "Error");
                return;
            }
            else if (double.TryParse(txtSend.Text, out strSend) == false)

            {
                MessageBox.Show("输入内容有误，请重新输入", "Error");
                return;
            }

            //检查sendcycle非法输入

            string str3 = "";
            int strcheck = -1;
            if (sendcycle.Text == "Range：0 - 65535")
            {
                str3 = Encode.EncodeCANSignal(strMessage, strSignal, strSend);
            }
            else if (int.TryParse(sendcycle.Text, out strcheck) == false)
            {
                MessageBox.Show("输入内容有误，请重新输入", "Error");
                return;
            }
            else
            {
                int a = int.Parse(sendcycle.Text);
                if (a < 0 || a > 65535)
                {
                    MessageBox.Show("输入数据超出范围，请重新输入", "Error");
                    return;
                }
                else
                {
                    str3 = Encode.EncodeCANSignal(strMessage, strSignal, strSend, Convert.ToUInt32(sendcycle.Text).ToString("x4").ToUpper());
                }
            }
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据 
            sp1.DiscardOutBuffer();

            //使用缓冲区的数据将指定数量的字节写入串行端口

            //MessageBox.Show(str3, "发送的数据为");
            //sp1.Write(str3);
            if (str3!=null&&!str3.Equals(""))
            {
                sp1.Write(str3);
            }
            
        }


        //开关按钮
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen)
            {
                try
                {
                    if (cbSerial.SelectedItem == null)
                    {
                        MessageBox.Show("Please Select ComPort", "Error");
                        return;
                    }
                    //设置串口号
                    string serialName = cbSerial.SelectedItem.ToString();
                    sp1.PortName = serialName;

                    //设置各“串口设置”
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    sp1.BaudRate = iBaudRate;       //波特率
                    sp1.DataBits = iDateBits;       //数据位
                    switch (cbStop.Text)            //停止位
                    {
                        case "1":
                            sp1.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            sp1.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            sp1.StopBits = StopBits.Two;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }
                    switch (cbParity.Text)             //校验位
                    {
                        case "None":
                            sp1.Parity = Parity.None;
                            break;
                        case "Odd":
                            sp1.Parity = Parity.Odd;
                            break;
                        case "Even":
                            sp1.Parity = Parity.Even;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }

                    if (sp1.IsOpen == true)//如果打开状态，则先关闭一下
                    {
                        sp1.Close();
                    }
                    //状态栏设置
                    tsSpNum.Text = "Comm：" + sp1.PortName + "|";
                    tsBaudRate.Text = "Baud Rate：" + sp1.BaudRate + "|";
                    tsDataBits.Text = "Data Bits：" + sp1.DataBits + "|";
                    tsStopBits.Text = "Stop Bits：" + sp1.StopBits + "|";
                    tsParity.Text = "Parity：" + sp1.Parity + "|";                    
                    sp1.Open();     //打开串口
                    //设置必要控件不可用
                    cbSerial.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;
                    btnSwitch.Text = "Close";
                    canClose.Enabled = false;
                    sendRate.Enabled = true;


                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error: " + "该串口已被占用", "Error");
                    return;
                }
            }
            else
            {
                //状态栏设置
                tsSpNum.Text = "Comm：None|";
                tsBaudRate.Text = "Baud Rate：None|";
                tsDataBits.Text = "Data Bits：None|";
                tsStopBits.Text = "Stop Bits：None|";
                tsParity.Text = "Parity：None|";
                //恢复控件功能
                //设置必要控件不可用
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;

                sp1.Close();                    //关闭串口
                btnSwitch.Text = "Open";               
            }
        }

        

        //退出按钮
        private void btnExit_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Hide();
        }

        //关闭时事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            INIFILE.Profile.SaveProfile();
            sp1.Close();
        }

        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            //正则匹配txtSend
            string patten = "[0-9|.]|\b"; //“\b”：退格键
            //string patten = "[0-9]|\b";
            Regex r = new Regex(patten);
            Match m = r.Match(e.KeyChar.ToString());

            if (m.Success)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void sendcycle_KeyPress(object sender, KeyPressEventArgs e)
        {
            //正则匹配sendcycyle
            string patten1 = "[0-9]|\b"; //“\b”：退格键
            //string patten = "[0-9]|\b";
            Regex r1 = new Regex(patten1);
            Match m1 = r1.Match(e.KeyChar.ToString());

            if (m1.Success)//&&(txtSend.Text.LastIndexOf(" ") != txtSend.Text.Length-1))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtSend_Click(object sender, EventArgs e)
        {
            if(txtSend.Text.Contains("Range:"))
            txtSend.Text = "";
        }

        private void txtSend_MouseLeave(object sender, EventArgs e)
        {
            if (txtSend.Text == "")
            {
                txtSend.Text = "Range: " + signal_C + " - " + signal_D;
            }
        }

        private void txtSend_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //设置各“串口设置”
            Profile.G_BAUDRATE = cbBaudRate.Text;
            Profile.G_DATABITS = cbDataBits.Text;
            Profile.G_RATE = canRate.Text;
            Profile.G_STOP = cbStop.Text;
            Profile.G_PARITY = cbParity.Text;
            Profile.SaveProfile();
            MessageBox.Show("用户设定文件保存成功", "Success");
            return;
            //new CheckForm().Show();

        }

        private void txtSecond_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

       

        private void cbParity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbStop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void message_SelectedIndexChanged(object sender, EventArgs e)
        {
            signal.Items.Clear();
            txtSend.Text = "";
            string select_m = (string)message.SelectedItem.ToString();

            if (select_m != "")
            {
                int m_id = 0;
                foreach (CANMessageObject cm in canMessage)
                {
                    string ss = new string(cm.MessageName);
                    if (ss.Equals(select_m))
                    {
                        m_id = Convert.ToInt32(cm.Id);
                        break;
                    }
                }
                //SqlHelper.connect();
                //string sql_select = "select id from cantoolapp.canmessage where messagename = '" + select_m + "'";
                //SqlDataReader sql_m_id = SqlHelper.query(sql_select);
                //sql_m_id.Read();
                //int m_id = Convert.ToInt32(sql_m_id[0]);
                //SqlHelper.close();

                SqlHelper.connect();
                //   string sql_s = "select signalname from cantoolapp.cansignal where canmessageid = " + m_id;
                string sql_s = "select * from cantoolapp.cansignal where canmessageid = " + m_id;
                SqlDataReader sqlReader_s = SqlHelper.query(sql_s);

                while (sqlReader_s.Read())
                {
                    CANSignalObject temp = new CANSignalObject();
                    string signalName = (string)sqlReader_s["signalname"];
                    temp.SignalName = signalName.ToCharArray();
                    temp.C1 = (double)sqlReader_s["C"];
                    temp.D1 = (double)sqlReader_s["D"];
                    canSignal.Add(temp);
                    signal.Items.Add(signalName);
                }
                SqlHelper.close();

            }
        }

        private void signal_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSignal = "";

            if (signal.SelectedItem == null)
            {
                MessageBox.Show("请选择Signal！", "Error");
                return;
            }
            else
            {
                strSignal = (string)signal.SelectedItem.ToString();
            }
            foreach (CANSignalObject can_s in canSignal)
            {
                string ss = new string(can_s.SignalName);
                if (ss.Equals(strSignal))
                {
                    signal_C = can_s.C1;
                    signal_D = can_s.D1;
                }
            }
            txtSend.Text = "Range: " + signal_C + " - " + signal_D;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void canOpen_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据
            sp1.DiscardOutBuffer();
            sp1.Write("O1\r");
            canOpen.Enabled = false;
            canClose.Enabled = true;
            sendRate.Enabled = false;
        }

        private void canClose_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据
            sp1.DiscardOutBuffer();
            sp1.Write("C\r");
            canClose.Enabled = false;
            canOpen.Enabled = true;
            sendRate.Enabled = true;
        }

        private void canRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void sendRate_Click(object sender, EventArgs e)
        {

            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据
            sp1.DiscardOutBuffer();
            //string sRate = "";
            if (canRate.SelectedItem == null)
            {
                MessageBox.Show("请选择CAN速率！", "Error");
                return;
            }
            else
            {
                switch (canRate.Text)
                {
                    case "10":
                        sp1.Write("S0\r");
                        break;
                    case "20":
                        sp1.Write("S1\r");
                        break;
                    case "50":
                        sp1.Write("S2\r");
                        break;
                    case "100":
                        sp1.Write("S3\r");
                        break;
                    case "125":
                        sp1.Write("S4\r");
                        break;
                    case "250":
                        sp1.Write("S5\r");
                        break;
                    case "500":
                        sp1.Write("S6\r");
                        break;
                    case "800":
                        sp1.Write("S7\r");
                        break;
                    case "1024":
                        sp1.Write("S8\r");
                        break;
                    default:
                        {
                            MessageBox.Show("CAN速率预置参数错误,请重新选择！", "Error");
                            return;
                        }
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void sendcycle_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void sendcycle_MouseLeave(object sender, EventArgs e)
        {
            if (sendcycle.Text.Equals(""))
            {
                sendcycle.Text = "Range：0 - 65535";
            }
        }
        private void sendcycle_Click(object sender, EventArgs e)
        {
            if (sendcycle.Text.Equals("Range：0 - 65535"))
                sendcycle.Text = "";
                   
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void canVersion_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
                return;
            }
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据
            sp1.DiscardOutBuffer();

            sp1.Write("V\r");
        }
        /*public void output(string log)
        {
            txtReceive.AppendText(DateTime.Now.ToString("HH:mm:ss ") + log + "\r\n");
            write(log);
        }

        public void write(string msg)
        {
            string logPath = Path.GetDirectoryName(Application.ExecutablePath);
            System.IO.StreamWriter sw = System.IO.File.AppendText(logPath + "/log.txt");
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss " + msg));
            sw.Close();
            sw.Dispose();
        }*/
    }
}



