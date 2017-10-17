using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using System.Data;

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
            
            DataTable table = new DataTable();
            //DataTable table = (DataTable)dataTable;
            
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            for (int i = 0; i < 10; i++)
                table.Columns.Add("");

            this.dataGridView.DataSource = table;

            Thread thread = new Thread(new ParameterizedThreadStart(UpdateTableThread.updateUi));
            thread.Start(table);
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
