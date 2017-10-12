using System;
using System.IO;
using System.Text;

namespace CANToolApp
{
    public class Encode
    {
        public static void EncodeCANSignal(CANMessageObject cmo)
        {
            string strFilePath = "E:\\CANToolApp\\CANToolApp\\canmsg-sample.dbc";
            FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs);

            char tT = ' ';
            if (cmo.Id > 0 && cmo.Id < 2047)
            {
                tT = 't';
            }
            else if (cmo.Id > 0 && cmo.Id < 536870911)
            {
                tT = 'T';
            }
            string iii = cmo.Id.ToString("x8");
            byte[] DLC = cmo.DLC1;

            string strReadLine;
            for (strReadLine = read.ReadLine(); ; strReadLine = read.ReadLine())
            {
                if (strReadLine != null)
                {
                    if (strReadLine.StartsWith("BO"))
                    {
                        string s = strReadLine.Substring(4, tT == 't' ? 3 : 8);
                        if (Convert.ToUInt32(s, 16) == cmo.Id)
                        {
                            //对属于该CAN信息的信号进行设置
                            while ((strReadLine = read.ReadLine()).StartsWith(" SG"))
                            {
                                char[] data = new char[Convert.ToUInt32(DLC) * 2 * 8];
                                string[] strArray = strReadLine.Replace("  ", " ").Split(' ');
                                //获取输入的值
                                int input = 15;
                                string[] AB = strArray[5].Split(new char[3] { '(', ',', ')' });
                                double A = Convert.ToDouble(AB[1]);
                                Console.WriteLine(A);
                                double B = Convert.ToDouble(AB[2]);
                                Console.WriteLine(B);
                                string x = ((input - B) / A).ToString("x8");

                                // 起始位、bit长度、bit格式
                                string[] startAndLength = strArray[4].Split(new char[2] { '|', '@' });
                                int start = (int)Convert.ToUInt32(startAndLength[0]);
                                Console.WriteLine(start);
                                int length = (int)Convert.ToUInt32(startAndLength[1]);
                                Console.WriteLine(length);
                                string pattern = startAndLength[2];
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
                                        data[start] = input_binary[i]; 
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
                                    string result = tT + iii + DLC + data.ToString() + "\r";
                                    Console.WriteLine(result);
                                }

                            }
                        }
                        break;
                    }
                }

            }
        }
    }
}

