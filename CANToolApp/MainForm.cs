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
