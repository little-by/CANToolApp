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
            接收CANMessage的Name和CANSignal的Name
            返回封装好的字符串
        */
        public static string EncodeCANSignal(string messageName, string signalName, int input)
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
                    for (int i = 0; i < DLC * 8; i++)
                    {
                        data[i] = '0';
                    }
                    double A = (double)sigReader[4];
                    double B = (double)sigReader[5];
                    double C = (double)sigReader[6];
                    double D = (double)sigReader[7];
                    double temp = (input - B) / A;
                    if (!(temp >= C && temp <= D))
                    {
                        MessageBox.Show("输入的数据超出范围，请重新输入！");
                        SqlHelper.close();
                        return null;
                    }
                    string x = ((int)((input - B) / A)).ToString("x8");
                    // 起始位、bit长度、bit格式
                    string[] startAndLengthAndPattern = sigReader[3].ToString().Split(new char[2] { '|', '@' });
                    int start = (int)Convert.ToUInt32(startAndLengthAndPattern[0]);
                    int length = (int)Convert.ToUInt32(startAndLengthAndPattern[1]);
                    string pattern = startAndLengthAndPattern[2];
                    string input_binary = Convert.ToString(Convert.ToInt32(x, 16), 2);
                    input_binary =  input_binary.PadLeft(length, '0');
                    if (pattern == "0+")
                    {
                        int i = 0, j = start;
                        int line = 0;
                        int leftIndex = 0, rightIndex = 0;
                        for (i = 0; i < length; i++)
                        {
                            data[j] = input_binary[i];
                            line = j / 8;
                            leftIndex = 7 * (line + 1) + line;
                            rightIndex = 8 * line;
                            if (line >= 0 && line < 8)
                            {
                                if (j < leftIndex && j >= rightIndex)
                                {
                                    j++;
                                }
                                else if (j == leftIndex && line > 0)
                                {
                                    j = j - 15;
                                }
                            }
                        }
                    }
                    else if (pattern == "1+")
                    {
                        int i = 0, j = start;
                        for (i = 0; i < length; i++)
                        {
                            data[j] = input_binary[i];
                            if (j < 64)
                            {
                                j++;
                            }
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
                    for (int n = 0; n < s.Length; n = n + 4)
                    {
                        shex.Append(string.Format("{0:X}", Convert.ToInt32(s.Substring(n, 4), 2)));
                    }
                    result = tT + canIdHex + DLC + shex + "\\r";
                }
            }
            return result;
        }
    }
}