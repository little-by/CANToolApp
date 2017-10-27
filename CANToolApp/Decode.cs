using System;
using System.Text;
using CANToolApp;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

public delegate void DelegateUpdateLog(string logobj);
public class Decode
{

    public static DelegateUpdateLog delegateUpdateLog;
    public static Dictionary<string, string> DecodeCANSignal(string canMessage)
    {
        SqlHelper.connect();
        
        char standardOrExtend = canMessage[0];
        uint canId = 0;
        uint DLC = 0;
        int dataLength = 0;
        string data = "";
        Dictionary<string, string> returnedData = new Dictionary<string, string>();
        
        try
        {
            if (standardOrExtend == 't')
            {
                //标准帧处理
                string canIdStr = canMessage.Substring(1, 3);
                canId = Convert.ToUInt32(canIdStr, 16);
                DLC = Convert.ToUInt32(canMessage[4].ToString(), 16);
                dataLength = (int)DLC * 2;
                data = canMessage.Substring(5, dataLength);
            }
            else if (standardOrExtend == 'T')
            {
                //扩展帧处理
                string canIdStr = canMessage.Substring(1, 8);
                canId = Convert.ToUInt32(canIdStr, 16);
                DLC = Convert.ToUInt32(canMessage[9].ToString(), 16); ;
                dataLength = (int)DLC * 2;
                data = canMessage.Substring(10, dataLength);
            }
            /*else
            {
                delegateUpdateLog(canMessage + "需要是标准帧或扩展帧的一种！");
            }*/
            //把data转化为二进制
            char[] binaryData = bianma(data).ToCharArray();
            int decimalSign = 0;

            int len = binaryData.Length;
            char[] temp = new char[len];
            int k = 0;
            for (k = 0; k < len; k++)
            {
                temp[k] = binaryData[k];
            }
            for (k = 0; k < len; k++)
            {
                int line = k / 8;
                int row = 7 - k % 8;
                binaryData[line * 8 + row] = temp[k];
            }

            string message = "select * from cantoolapp.canmessage where id = " + canId;
            string signal = "select * from cantoolapp.cansignal where canmessageid = " + canId;
            SqlDataReader messageExist = SqlHelper.query(message);

            if (!messageExist.HasRows)
            {
                //MessageBox.Show("系统中不存在此message!");
                return null;
            }
            else
            {
                messageExist.Read();
                //MessageBox.Show("系统中存在此message!");
                returnedData.Add("messageName", messageExist[2].ToString() + " " + data);
                SqlHelper.close();
                SqlHelper.connect();
                SqlDataReader reader = SqlHelper.query(signal);
                if (!reader.HasRows)
                {
                    //MessageBox.Show("此Message下不包含信号!");
                    return returnedData;
                }
                else
                {
                    while (reader.Read())
                    {
                        // 起始位、bit长度、bit格式
                        string[] startAndLengthAndPattern = reader[3].ToString().Split(new char[2] { '|', '@' });
                        int start = (int)Convert.ToUInt32(startAndLengthAndPattern[0]);
                        int length = (int)Convert.ToUInt32(startAndLengthAndPattern[1]);
                        string pattern = startAndLengthAndPattern[2];

                        //根据上面三个标准求解信号的值
                        StringBuilder sb = new StringBuilder("");
                        if (pattern == "0+")
                        {
                            int i = 0, j = start, end = start + length - 1;
                            int line = 0;
                            int leftIndex = 0, rightIndex = 0;
                            for (i = 0; i < length; i++)
                            {
                                sb.Append(binaryData[j]);
                                line = j / 8;
                                leftIndex = 7 * (line + 1) + line;
                                rightIndex = 8 * line;
                                if (line >= 0 && line < 8)
                                {
                                    if (j <= leftIndex && j > rightIndex)
                                    {
                                        j--;
                                    }
                                    else if (j == rightIndex && line < 7)
                                    {
                                        j = j + 15;
                                    }
                                }
                            }
                            decimalSign = Convert.ToInt32(sb.ToString(), 2);
                        }
                        else if (pattern == "1+")
                        {
                            int i, j = start + length - 1;
                            for (i = 0; i < length; i++)
                            {
                                sb.Append(binaryData[j]);
                                j--;
                            }
                            decimalSign = Convert.ToInt32(sb.ToString(), 2);
                        }
                        else
                        {
                            MessageBox.Show("信号中含有暂不支持的数据格式!");
                            return null;
                        }
                        double A = (double)reader[4];
                        double B = (double)reader[5];
                        double C = (double)reader[6];
                        double D = (double)reader[7];
                        //物理值
                        double phy = A * decimalSign + B;
                        //MessageBox.Show("" + phy);
                        returnedData.Add((string)reader[1], phy + " [" + C + "|" + D + "]");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.StackTrace);
        }
        finally
        {
            SqlHelper.close();
        }
        return returnedData;
    }
    //重载decode函数，为了显示bit
    public static TableMsg DecodeCANSignal(string canData, string messageName,ref DataTable dt,out TableMsg tableMsg)
    {
        tableMsg =new TableMsg(dt);
        
        string canId = "";
        int dataLength = 0;
        int[] Binarydata = new int[64];
        try
        {
            SqlHelper.connect();
            string sqlselectid = "select id from cantoolapp.canmessage where messagename='" + messageName+"'";
            SqlDataReader sdr = SqlHelper.query(sqlselectid);
            sdr.Read();
            string canIdStr = sdr[0].ToString();
            SqlHelper.close();
            dataLength = 16;
            //把data转化为二进制
            char[] binaryData = bianma(canData).ToCharArray();
            int decimalSign = 0;

            int len = binaryData.Length;
            char[] temp = new char[len];
            int k = 0;
            for (k = 0; k < len; k++)
            {
                temp[k] = binaryData[k];
            }
            for (k = 0; k < len; k++)
            {
                int line = k / 8;
                int row = 7 - k % 8;
                binaryData[line * 8 + row] = temp[k];
            }
            for(k = 0; k < len; k++)
            {
                Binarydata[k] =int.Parse(""+binaryData[k]);
                Console.WriteLine(Binarydata[k]);
            }
            tableMsg.Binarydata = Binarydata;
            SqlHelper.connect();
            string message = "select signalname,start_length_pattern from cantoolapp.cansignal where canmessageid = '" + canIdStr + "'";
            SqlDataReader dr = SqlHelper.query(message);
            while (dr.Read())
            {
                string signame = dr[0].ToString();
                string sigpos = dr[1].ToString();
                tableMsg.ReturnedData.Add(signame, sigpos);
            }
            SqlHelper.close();
            for (int j = 0; j < 8; j++)
            {
                Console.WriteLine("this is a fun!!" + tableMsg.Binarydata[j]);
            }
            return tableMsg;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return null;
    }


    //将16进制字符串转化为2进制字符串
    public static string bianma(string s)
    {
        int len = s.Length;
        string ret = "";
        for (int i = 0; i < len; i++)
        {
            ret += foo(s[i]);
        }
        return ret;
    }
    public static string foo(char c)
    {
        if (c == '0')
            return "0000";
        else if (c == '1')
            return "0001";
        else if (c == '2')
            return "0010";
        else if (c == '3')
            return "0011";
        else if (c == '4')
            return "0100";
        else if (c == '5')
            return "0101";
        else if (c == '6')
            return "0110";
        else if (c == '7')
            return "0111";
        else if (c == '8')
            return "1000";
        else if (c == '9')
            return "1001";
        else if (c == 'A')
            return "1010";
        else if (c == 'B')
            return "1011";
        else if (c == 'C')
            return "1100";
        else if (c == 'D')
            return "1101";
        else if (c == 'E')
            return "1110";
        else if (c == 'F')
            return "1111";
        return "";
    }
}
