using System;
using System.Data.SqlClient;
using System.Windows.Forms;

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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void test_Click(object sender, EventArgs e)
        {
            //string str = "t03d82153547865423425";
            //Decode.DecodeCANSignal(str);
            //string str = "t35882153547865423425";
            //Decode.DecodeCANSignal(str);
            string messageName = "CDU_4";
            string signalName = "CDU_HVACAirCirCfg";
            Console.WriteLine(Encode.EncodeCANSignal(messageName, signalName));
        }

        private void cOM口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ComPortForm().Show();
        }

        private void CurveShowBt_Click(object sender, EventArgs e)
        {
            CurveShow csForm = new CurveShow();
            csForm.Show();
        }

        private void DashboardShowBt_Click(object sender, EventArgs e)
        {
            DashboardShow dsForm = new DashboardShow();
            dsForm.Show();
        }
    }
}
