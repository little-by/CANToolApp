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
        public static void updateUi(object tablemsg)
        {
            Console.WriteLine("this is a thread!!");
            TableMsg tm = (TableMsg)tablemsg;
            for (int j = 0; j < 8; j++)
            {
                Console.WriteLine("this is a thread!!" + tm.Binarydata[j]);
            }
            
            DataTable table = (DataTable)tm.DataTable;
            
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    table.Rows[i].ItemArray[j] = tm.Binarydata[i*8+j];
                }
                
            }
        }




    }
}
