using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CANToolApp
{
    public class UpdateTableThread
    {
        public static void updateUi(object dataTable)
        {
            Console.WriteLine("this is a thread!!");
            DataTable table = (DataTable)dataTable;
            DataRow dr = table.NewRow();
            dr[0] = "125";
            dr[1] = "358";
            table.Rows.Add(dr);
            //Console.WriteLine(str);
            //table.Columns.Add("he");
        } 



    }
}
