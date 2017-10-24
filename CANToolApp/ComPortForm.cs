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

namespace CANToolApp
{
    public partial class ComPortForm : Form
    {
        private List<CANSignalObject> canSignal = new List<CANSignalObject>();
        private List<CANMessageObject> canMessage = new List<CANMessageObject>();
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
                //System.Diagnostics.Debug.WriteLine(s);
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
            //cbBaudRate.SelectedIndex = 5;
            // cbDataBits.SelectedIndex = 3;
            // cbStop.SelectedIndex = 0;
            // cbParity.SelectedIndex = 0;
            //sp1.BaudRate = 115200;

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

        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp1.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                //输出当前时间
                // DateTime dt = DateTime.Now;
                //txtReceive.Text += dt.GetDateTimeFormats('f')[0].ToString() + "\r\n";
                txtReceive.SelectAll();
                txtReceive.SelectionColor = Color.Blue;         //改变字体的颜色

                byte[] byteRead = new byte[sp1.BytesToRead];    //BytesToRead:sp1接收的字符个数
                string msgrec = sp1.ReadLine();
                txtReceive.Text += msgrec + "\r\n"; //注意：回车换行必须这样写，单独使用"\r"和"\n"都不会有效果
                delegateUpdateUI(msgrec);
                sp1.DiscardInBuffer();                      //清空SerialPort控件的Buffer 
                //try
                //{
                //    Byte[] receivedData = new Byte[sp1.BytesToRead];        //创建接收字节数组
                //    sp1.Read(receivedData, 0, receivedData.Length);         //读取数据
                //    //string text = sp1.Read();   //Encoding.ASCII.GetString(receivedData);
                //    sp1.DiscardInBuffer();                                  //清空SerialPort控件的Buffer
                //    //这是用以显示字符串
                //    //    string strRcv = null;
                //    //    for (int i = 0; i < receivedData.Length; i++ )
                //    //    {
                //    //        strRcv += ((char)Convert.ToInt32(receivedData[i])) ;
                //    //    }
                //    //    txtReceive.Text += strRcv + "\r\n";             //显示信息
                //    //}
                //}
                //catch (System.Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "出错提示");
                //   // txtSend.Text = "";
                //}
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }

            //"t3588A5SD566D9F8SD565"

        }

        //发送按钮
        private void btnSend_Click(object sender, EventArgs e)
        {

            if (!sp1.IsOpen) //如果没打开
            {
                MessageBox.Show("请先打开串口！", "Error");
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
            //double strSend = -1;
            int  strSend = -1;
            if (txtSend.Text == "")
            {
                MessageBox.Show("请填写信息！", "Error");
                return;
            }
            //else if (double.TryParse(txtSend.Text, out strSend) == false)
            else if (int.TryParse(txtSend.Text, out strSend) == false)
            {
                MessageBox.Show("输入内容有误，请重新输入", "Error");
                return;
            }            
            //MessageBox.Show(strSend);
            //丢弃来自串行驱动程序的接受缓冲区的数据
            sp1.DiscardInBuffer();
            //丢弃来自串行驱动程序的传输缓冲区的数据
            sp1.DiscardOutBuffer();

            string str3 = "";
            if (sendcycle.Text == "")
            {
                str3 = Encode.EncodeCANSignal(strMessage, strSignal, strSend);
            }
            else
            {
                str3 = Encode.EncodeCANSignal(strMessage, strSignal, strSend, sendcycle.Text);
            }
            //使用缓冲区的数据将指定数量的字节写入串行端口

            //MessageBox.Show(str3, "发送的数据为");
            sp1.WriteLine(str3);
            //sp1.Write(list, 0, list.Length);
            // sp1.WriteLine(strSend);    //写入数据
            //关闭端口连接
            //         sp1.Close();
            //当前线程挂起500毫秒
            //           System.Threading.Thread.Sleep(500);


        }

        //处理数字转换
        /*
                       string sendBuf = strSend;
                       string sendnoNull = sendBuf.Trim();
                       //string sendNOComma = sendnoNull.Replace(',', ' ');    //去掉英文逗号
                       //string sendNOComma1 = sendNOComma.Replace('，', ' '); //去掉中文逗号
                       //string strSendNoComma2 = sendNOComma1.Replace("0x", "");   //去掉0x
                       //strSendNoComma2.Replace("0X", "");   //去掉0X
                       string[] strArray = sendnoNull.Split(' ');

                       int byteBufferLength = strArray.Length;
                       for (int i = 0; i < strArray.Length; i++)
                       {
                           if (strArray[i] == "")
                           {
                               byteBufferLength--;
                           }
                       }
                       // int temp = 0;
                       byte[] byteBuffer = new byte[byteBufferLength];
                       int ii = 0;
                       for (int i = 0; i < strArray.Length; i++)        //对获取的字符做相加运算
                       {

                           Byte[] bytesOfStr = Encoding.Default.GetBytes(strArray[i]);

                           int decNum = 0;
                           if (strArray[i] == "")
                           {
                               //ii--;     //加上此句是错误的，下面的continue以延缓了一个ii，不与i同步
                               continue;
                           }
                           else
                           {
                               decNum = Convert.ToInt32(strArray[i], 16); //atrArray[i] == 12时，temp == 18 
                           }

                           try    //防止输错，使其只能输入一个字节的字符
                           {
                               byteBuffer[ii] = Convert.ToByte(decNum);
                           }
                           catch (System.Exception ex)
                           {
                               MessageBox.Show("字节越界，请逐个字节输入！", "Error");
                               tmSend.Enabled = false;
                               return;
                           }

                           ii++;
                       }
                       sp1.Write(byteBuffer, 0, byteBuffer.Length);
            
                       //以字符串形式发送时 
            
                      sp1.WriteLine(txtSend.Text);    //写入数据
            
               }
       */
        //开关按钮
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            //serialPort1.IsOpen
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

                    //设置必要控件不可用
                    cbSerial.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;

                    sp1.Open();     //打开串口
                    btnSwitch.Text = "Close";
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    tmSend.Enabled = false;
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
                // tmSend.Enabled = false;         //关闭计时器
            }
        }

        //清空按钮
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtReceive.Text = "";       //清空文本
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
            //正则匹配
            string patten = "[0-9|.]|\b"; //“\b”：退格键
            //string patten = "[0-9]|\b";
            Regex r = new Regex(patten);
            Match m = r.Match(e.KeyChar.ToString());

            if (m.Success)//&&(txtSend.Text.LastIndexOf(" ") != txtSend.Text.Length-1))
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
            txtSend.Text = "";
        }
        private void txtSend_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //设置各“串口设置”
            string strBaudRate = cbBaudRate.Text;
            string strDateBits = cbDataBits.Text;
            string strStopBits = cbStop.Text;
            string strRate = canRate.Text;
            Int32 iBaudRate = Convert.ToInt32(strBaudRate);
            Int32 iDateBits = Convert.ToInt32(strDateBits);
            Int32 iRate = Convert.ToInt32(strRate);

            Profile.G_BAUDRATE = iBaudRate + "";       //波特率
            Profile.G_DATABITS = iDateBits + "";       //数据位
            Profile.G_RATE = iRate + "";
            switch (cbStop.Text)            //停止位转成字符的格式
            {
                case "1":
                    Profile.G_STOP = "1";
                    break;
                case "1.5":
                    Profile.G_STOP = "1.5";
                    break;
                case "2":
                    Profile.G_STOP = "2";
                    break;
                default:
                    MessageBox.Show("Error：参数不正确!", "Error");
                    break;
            }
            switch (cbParity.Text)             //校验位
            {
                case "None":
                    Profile.G_PARITY = "None";
                    break;
                case "Odd":
                    Profile.G_PARITY = "Odd";
                    break;
                case "Even":
                    Profile.G_PARITY = "Even";
                    break;
                default:
                    MessageBox.Show("Error：参数不正确!", "Error");
                    break;
            }


            Profile.SaveProfile();
            new CheckForm().Show();

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

        private void txtReceive_TextChanged(object sender, EventArgs e)
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
            double signal_C = 0, signal_D = 0;
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
            sp1.WriteLine("O1\\r");
            string recData = "";
            System.Threading.Thread.Sleep(3000);
            recData = sp1.ReadLine();
            if (recData == "")
            {
                MessageBox.Show("连接失败，请重新发送", "Error");
                return;
            }
            if (recData.Equals("\\r"))
            {
                MessageBox.Show("已进入CAN工作状态", "Success");
                canOpen.Enabled = false;
            }
            else if (recData.Equals("\\BEL"))
            {
                MessageBox.Show("无法进入CAN工作状态", "Error");
                return;
            }
            else
            {
                MessageBox.Show("请检查", "Error");
                return;
            }
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
            sp1.WriteLine("C\r");
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
                        sp1.WriteLine("S0");
                        break;
                    case "20":
                        sp1.WriteLine("S1");
                        break;
                    case "50":
                        sp1.WriteLine("S2");
                        break;
                    case "100":
                        sp1.WriteLine("S3");
                        break;
                    case "125":
                        sp1.WriteLine("S4");
                        break;
                    case "250":
                        sp1.WriteLine("S5");
                        break;
                    case "500":
                        sp1.WriteLine("S6");
                        break;
                    case "800":
                        sp1.WriteLine("S7");
                        break;
                    case "1024":
                        sp1.WriteLine("S8");
                        break;
                    default:
                        {
                            MessageBox.Show("CAN速率预置参数错误,请重新选择！","Error");
                            return;
                        }
                }

            }
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
            sp1.WriteLine("V\r");
        }
    }
}
            
          

