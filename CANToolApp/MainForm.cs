using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CANToolApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void sendBt_Click(object sender, EventArgs e)
        {
            CANToolApp.SqlHelper.connect();
            SqlDataReader dr=CANToolApp.SqlHelper.query("select * from cantoolapp.canmessage");
            Console.WriteLine(dr.FieldCount);
            while (dr.Read())
            {
                for(int i=0;i< dr.FieldCount; i++)
                {
                    Console.WriteLine(dr.GetName(i)+":"+ dr[i].ToString());
                }
            }
            CANToolApp.SqlHelper.close();
        }

        private void showBt_Click(object sender, EventArgs e)
        {
            GraphVisual gvForm = new GraphVisual();
            gvForm.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void test_Click(object sender, EventArgs e)
        {
	   //Form1 comForm = new Form1();
	   //comForm.Show();
            string str = "t35882153547865423425";
            Decode.DecodeCANSignal(str);
            //string str = "t35882153547865423425";
            //Decode.DecodeCANSignal(str);
            string messageName = "CDU_1";
            string signalName = "CDU_HV";
            Encode.EncodeCANSignal(messageName, signalName);
        }

        private void cOM口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ComPortForm().Show();
        }
    }
}
