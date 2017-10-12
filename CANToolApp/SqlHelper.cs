using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CANToolApp
{
    public static class SqlHelper
    {
        static private SqlConnection con = new SqlConnection();
        static private SqlCommand com = new SqlCommand();
        static SqlDataReader dr;

        public static void connect()
        {
            con.ConnectionString = "server=bds266769316.my3w.com;database=bds266769316_db;user=bds266769316;pwd=959511586";
            con.Open();
        }

        public static SqlDataReader query(string sqlstr)
        {
            com.Connection = con;
            com.CommandType = CommandType.Text;
            com.CommandText = sqlstr;
            try
            {
                dr = com.ExecuteReader();
            }
            catch(InvalidOperationException e1)
            {
                Console.WriteLine(e1.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return dr;
        }
        public static void close()
        {
            dr.Close();
            con.Close();
        }
    }
}