using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace CANToolApp
{
    public class Encode
    {
        /**
        此函数完成的工作：
            接收CANMessage的Name和CANSignal的Name和用户输入的物理值，以及对CANTool向CAN总线发送命令的周期
            返回封装好的字符串
        */
        public static string EncodeCANSignal(string messageName, string signalName, double input)
        {
            return Encode.EncodeCANSignal(messageName, signalName, input, "0000");
        }
        public static string EncodeCANSignal(string messageName, string signalName, double input, string cycle)
        {
            string result = "";
            SqlHelper.connect();
            SqlDataReader msgReader = SqlHelper.query("select * from cantoolapp.canmessage where messagename = '" + messageName + "'");
            if (!msgReader.HasRows)
            {
                MessageBox.Show("数据库中无此Message!");
                SqlHelper.close();
            }
            else
            {
                msgReader.Read();
                int canId = Convert.ToInt32(msgReader[1].ToString());
                char tT = ' ';
                if (canId > 0 && canId < 2047)
                {
                    tT = 't';
                }
                else if (canId > 0 && canId < 536870911)
                {
                    tT = 'T';
                }
                string canIdHex = Convert.ToString(canId, 16).PadLeft(tT == 't' ? 3 : 8, '0');
                int DLC = (int)msgReader[4];

                SqlHelper.close();
                SqlHelper.connect();
                SqlDataReader sigReader = SqlHelper.query("select * from cantoolapp.cansignal where signalname = '" + signalName + "'" + "and canmessageid = " + canId);
                sigReader.Read();
                if (!sigReader.HasRows)
                {
                    MessageBox.Show("Message下无信号!");
                    SqlHelper.close();
                }
                else
                {
                    char[] data = new char[DLC * 8];
                    for (int k = 0; k < DLC * 8; k++)
                    {
                        data[k] = '0';
                    }
                    double A = (double)sigReader[4];
                    double B = (double)sigReader[5];
                    double C = (double)sigReader[6];
                    double D = (double)sigReader[7];
                    if (!(input >= C && input <= D))
                    {
                        MessageBox.Show("输入的数据超出范围，请重新输入！");
                        SqlHelper.close();
                        return null;
                    }
                    int x = (int)((input - B) / A);
                    // 起始位、bit长度、bit格式
                    string[] startAndLengthAndPattern = sigReader[3].ToString().Split(new char[2] { '|', '@' });
                    int start = (int)Convert.ToUInt32(startAndLengthAndPattern[0]);
                    int length = (int)Convert.ToUInt32(startAndLengthAndPattern[1]);
                    string pattern = startAndLengthAndPattern[2];
                    string input_binary = Convert.ToString(x, 2);
                    input_binary = input_binary.PadLeft(length, '0');
                    if (pattern == "0+")
                    {
                        int m = 0, n = start;
                        int line = 0;
                        int leftIndex = 0, rightIndex = 0;
                        for (m = 0; m < length; m++)
                        {
                            data[n] = input_binary[m];
                            line = n / 8;
                            leftIndex = 7 * (line + 1) + line;
                            rightIndex = 8 * line;
                            if (line >= 0 && line < 8)
                            {
                                if (n <= leftIndex && n > rightIndex)
                                {
                                    n--;
                                }
                                else if (n == rightIndex && line < 7)
                                {
                                    n = n + 15;
                                }
                            }
                        }
                    }
                    else if (pattern == "1+")
                    {
                        int t = 0, j = start;
                        for (t = length - 1; t >= 0; t--)
                        {
                            data[j] = input_binary[t];
                            j++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("暂不支持此数据格式!");
                        SqlHelper.close();
                        return "";
                    }
                    SqlHelper.close();
                    string s = new string(data);
                    StringBuilder shex = new StringBuilder();
                    int i = 0;
                    int len = data.Length;
                    for (i = 0; i < len; i += 8)
                    {
                        int m, n;
                        int line = i / 8;
                        StringBuilder tempdata = new StringBuilder();
                        for (m = 0, n = 7 * (line + 1) + line; m < 4; m++, n--)
                        {
                            tempdata.Append(data[n]);
                        }
                        shex.Append(string.Format("{0:X}", Convert.ToInt32(tempdata.ToString(), 2)));
                        tempdata = new StringBuilder();
                        for (m = 0, n = 7 * (line + 1) + line - 4; m < 4; m++, n--)
                        {
                            tempdata.Append(data[n]);
                        }
                        shex.Append(string.Format("{0:X}", Convert.ToInt32(tempdata.ToString(), 2)));
                    }
                    result = tT + canIdHex.ToUpper() + DLC + shex.ToString().ToUpper() + cycle + "\r";
                }
            }
            return result;
        }
    }
}