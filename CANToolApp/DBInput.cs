using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANToolApp
{
    public class DBInput
    {
        /*
        由于数据格式不规范，需要手工进行输入文件的部分处理。     
        */
        public static void InputData(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs);
            string str = "server=bds266769316.my3w.com;database=bds266769316_db;user=bds266769316;pwd=959511586";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            string strReadLine;
            string id = "";
            for (strReadLine = read.ReadLine(); ; strReadLine = read.ReadLine())
            {
                if (strReadLine != null)
                {
                    string first = strReadLine.Replace("   ", " ");
                    string[] dbstr = first.Replace("  ", " ").Split(new char[2] { ' ', '"'});
                    if (dbstr[0] == "BO_")
                    {
                        id = dbstr[1];
                        string sql = "insert into cantoolapp.canmessage(id, messagename, dlc, nodename)values('" + dbstr[1] + "','" + dbstr[2] + "','" + dbstr[4] + "','" + dbstr[5] + "')";
                        SqlCommand cmd = new SqlCommand(sql, con); //定义一个sql操作命令对象             
                        cmd.ExecuteNonQuery(); //执行语句
                    }
                    if (dbstr[1] == "SG_")
                    {
                        string[] AB = dbstr[5].Split(new char[3] { '(', ',', ')' });
                        string[] CD = dbstr[6].Split(new char[3] { '[', '|', ']' });
                        string sql = "insert into cantoolapp.cansignal(signalname, start_length_pattern, A, B, C, D, measure, nodename, canmessageid)values('" + dbstr[2] + "','" + dbstr[4] + "','" + AB[1] + "','" + AB[2] + "','" + CD[1] + "','" + CD[2] + "','" + dbstr[8] + "','" + dbstr[10] + "','" + id +"')";
                        SqlCommand cmd = new SqlCommand(sql, con); //定义一个sql操作命令对象             
                        cmd.ExecuteNonQuery(); //执行语句
                    }
                }
            }
            con.Close(); //关闭连接
            con.Dispose(); //释放对象  
        }
    }
}