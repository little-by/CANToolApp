using System;
using System.IO;
using System.Text;

public class Decode
{
    public static void DecodeCANSignal(string canMessage)
    {
        string strFilePath = "E:\\CANToolApp\\CANToolApp\\canmsg-sample.dbc";
        FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs);

        char standardOrExtend = canMessage[0];
        int flag = 0;
        uint canId = 0;
        uint DLC = 0;
        int dataLength = 0;
        string data = "";
        try
        {
            if (standardOrExtend == 't')
            {
                //标准帧处理
                flag = 0;
                string canIdStr = canMessage.Substring(1, 3);
                canId = Convert.ToUInt32(canIdStr, 16);
                DLC = Convert.ToUInt32(canMessage[4].ToString(), 16);
                dataLength = (int)DLC * 2;
                Console.WriteLine(canMessage);
                Console.WriteLine(canId);
                Console.WriteLine(DLC);
                Console.WriteLine(dataLength);
                data = canMessage.Substring(5, dataLength);
            }
            else if (standardOrExtend == 'T')
            {
                //扩展帧处理
                flag = 1;
                string canIdStr = canMessage.Substring(1, 8);
                canId = Convert.ToUInt32(canIdStr, 16);
                DLC = Convert.ToUInt32(canMessage[9].ToString(), 16); ;
                dataLength = (int)DLC * 2;
                data = canMessage.Substring(10, dataLength);
            }
            
            string strReadLine;
            for (strReadLine = read.ReadLine(); ; strReadLine = read.ReadLine())
            {
                if (strReadLine != null)
                {
                    if (strReadLine.StartsWith("BO"))
                    {
                        string s = strReadLine.Substring(4, flag == 0 ? 3 : 8);
                        if (Convert.ToUInt32(s, 16) == canId)
                        {
                            //对属于该CAN信息的信号进行解析
                            while ((strReadLine = read.ReadLine()).StartsWith(" SG"))
                            {
                                Console.WriteLine(strReadLine);
                                string[] strArray = strReadLine.Replace("  ", " ").Split(' ');
                                // 起始位、bit长度、bit格式
                                string[] startAndLength = strArray[4].Split(new char[2] { '|', '@' });
                                int start = (int)Convert.ToUInt32(startAndLength[0]);
                                Console.WriteLine(start);
                                int length = (int)Convert.ToUInt32(startAndLength[1]);
                                Console.WriteLine(length);
                                string pattern = startAndLength[2];
                                Console.WriteLine(pattern);
                                //把data转化为二进制
                                string binaryData = bianma(data);
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
                                            else if (j == rightIndex)
                                            {
                                                j = j + 15;
                                            }
                                        }
                                    }
                                    Console.WriteLine("信号的二进制数值：" + sb);
                                    int decimalSign = Convert.ToInt32(sb.ToString(), 2);
                                    Console.WriteLine("信号的十进制数值：" + decimalSign);
                                    string hexSign = string.Format("{0:X}", decimalSign);
                                    Console.WriteLine("信号的十六进制数值：" + hexSign);
                                    string[] AB = strArray[5].Split(new char[3] { '(', ',', ')' });
                                    double A = Convert.ToDouble(AB[1]);
                                    Console.WriteLine(A);
                                    double B = Convert.ToDouble(AB[2]);
                                    Console.WriteLine(B);
                                    //物理值
                                    double phy = A * decimalSign + B;
                                    Console.WriteLine(phy);
                                }
                            }
                        }
                        break;
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
            read.Close();
            fs.Close();
        }

        string bianma(string s)
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
}
