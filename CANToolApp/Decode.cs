﻿using System;
using System.Text;
using CANToolApp;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;

public class Decode
{
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
                        //把data转化为二进制
                        string binaryData = bianma(data);
                        int decimalSign = 0;
                        //根据上面三个标准求解信号的值
                        if (pattern == "0+")
                        {
                            StringBuilder sb = new StringBuilder("");
                            int i = 0, j = start;
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
                            StringBuilder sb = new StringBuilder("");
                            int i = 0, j = start;
                            for (i = 0; i < length; i++)
                            {
                                sb.Append(binaryData[j]);
                                j++;
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
    //将16进制字符串转化为2进制字符串
    public static string bianma(string s)
    {
        byte[] dataByte = Encoding.Unicode.GetBytes(s);
        StringBuilder result = new StringBuilder(dataByte.Length * 8);
        foreach (byte b in dataByte)
        {
            result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        return result.ToString();
    }
}
