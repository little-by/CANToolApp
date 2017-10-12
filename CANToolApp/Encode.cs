using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CANToolApp
{
    public class Encode
    {
        public static void EncodeCANSignal(string messageName, string signalName)
        {
            string result;
            SqlHelper.connect();
            Console.WriteLine("select * from cantoolapp.canmessage where messagename =" + messageName);
            SqlDataReader msgReader = SqlHelper.query("select * from cantoolapp.canmessage where messagename = '" + messageName + "'");
            if (!msgReader.HasRows)
            {
                MessageBox.Show("数据库中无此Message!");
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
                string canIdHex = Convert.ToString(canId, 16);
                int DLC = (int)msgReader[4];

                SqlHelper.close();
                SqlHelper.connect();
                SqlDataReader sigReader = SqlHelper.query("select * from cantoolapp.cansignal where signalname = '" + signalName + "'");
                sigReader.Read();
                if (!sigReader.HasRows)
                {
                    MessageBox.Show("Message下无信号!");
                }
                else
                {
                    char[] data = new char[DLC * 8];
                    for (int i = 0; i < DLC * 8; i++)
                    {
                        data[i] = '0';
                    }
                    //获取输入的值
                    int input = 15;
                    double A = (double)sigReader[4];
                    double B = (double)sigReader[5]; ;
                    string x = ((int)((input - B) / A)).ToString("x8");
                    // 起始位、bit长度、bit格式
                    string[] startAndLengthAndPattern = sigReader[3].ToString().Split(new char[2] { '|', '@' });
                    int start = (int)Convert.ToUInt32(startAndLengthAndPattern[0]);
                    int length = (int)Convert.ToUInt32(startAndLengthAndPattern[1]);
                    string pattern = startAndLengthAndPattern[2];
                    string input_binary = Convert.ToString(Convert.ToInt32(x, 16), 2);
                    while (input_binary.Length < length)
                    {
                        input_binary.Insert(0, "0");
                    }
                    if (pattern == "0+")
                    {
                        int i = 0, j = start;
                        int line = 0;
                        int leftIndex = 0, rightIndex = 0;
                        for (i = 0; i < length; i++)
                        {
                            data[start] = input_binary[j];
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
                        string s = new string(data);
                        StringBuilder shex=new StringBuilder();
                        for(int n = 0; n < s.Length; n = n + 4)
                        {
                            shex.Append(string.Format("{0:X}", Convert.ToInt32(s.Substring(n, 4), 2)));
                        }
                        result = tT + canIdHex + DLC + shex + "\\r";
                        MessageBox.Show(result);
                        SqlHelper.close();
                    }
                }
            }     
        }
    }
}