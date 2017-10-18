using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace CANToolApp
{
    public partial class MainForm : Form
    {

		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            InitTreeList();
            AddItems();




            DataTable table = new DataTable();
            //DataTable table = (DataTable)dataTable;
            
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            for (int i = 0; i < 10; i++)
                table.Columns.Add("");

            //this.dataGridView.DataSource = table;

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

        private void AddItems()
        {
            for (int k = 0; k < 4; k++)
            {
                TreeListViewItem itemA = new TreeListViewItem("Drive " + Enum.GetName(typeof(Drives), k), 0);
                itemA.Expand();
                itemA.SubItems.Add(Enum.GetName(typeof(DrivesDescr), k) + " drive");
                treeListView1.Items.Add(itemA);
                for (int i = 1; i < 11; i++)
                {
                    TreeListViewItem item = new TreeListViewItem("Folder " + i.ToString(), 1);
                    itemA.Items.Add(item);
                    ListViewItem.ListViewSubItem subitem = item.SubItems.Add((Math.Abs(2 - i)).ToString());
                    //					subitem.ForeColor = Color.FromArgb(173,194,231);
                    //					subitem.BackColor = Color.Yellow;
                    //					subitem.Font = new Font(subitem.Font, FontStyle.Bold | FontStyle.Underline);
                    for (int j = 1; j < 5; j++)
                        item.Items.Add("Sub " + j, 3);
                }
            }
        }
        private void InitTreeList()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TreeListView));
            //this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            //this.button1 = new System.Windows.Forms.Button();
            //this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            this.treeListView1.AllowColumnReorder = true;
            this.treeListView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
            this.treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                            this.columnHeader1,
                                                                                            this.columnHeader2,
                                                                                            this.columnHeader3});
            this.treeListView1.HideSelection = false;
            this.treeListView1.LabelEdit = true;
            this.treeListView1.Location = new System.Drawing.Point(4, 4);
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.Size = new System.Drawing.Size(392, 180);
            this.treeListView1.SmallImageList = this.imageList1;
            this.treeListView1.TabIndex = 0;
            //this.treeListView1.BeforeLabelEdit += new System.Windows.Forms.TreeListViewBeforeLabelEditEventHandler(this.treeListView1_BeforeLabelEdit);
            //this.treeListView1.BeforeCollapse += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeCollapse);
            //this.treeListView1.BeforeExpand += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeExpand);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Attribute";
            this.columnHeader2.Width = 100;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // button1
            // 
            /*
            this.button1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(8, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add / Remove All";*/
            //this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            /*
            this.button2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(264, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Expand / Collapse All";
            */
            //this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TryTreeListView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 219);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.button2,
                                                                          this.button1,
                                                                          this.treeListView1});
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }
        private enum DrivesDescr { First, Second, Third, Fourth }
        private enum Drives { C, D, E, Z }


    }
}
