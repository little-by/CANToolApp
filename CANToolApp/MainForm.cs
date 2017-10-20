using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using System.IO;

namespace CANToolApp
{
    public delegate void DelegateUpdateUI(string msgobj);
    public partial class MainForm : Form
    {
        private static SynchronizationContext m_SyncContext = null;
        private static TreeListView treeListView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ColumnHeader columnHeader3;
        
        public event DelegateUpdateUI delegateUpdateUI;

        public MainForm()
        {
            m_SyncContext = SynchronizationContext.Current;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            InitTreeList();
            //AddItems();
            //addone();



            DataTable table = new DataTable();
            //DataTable table = (DataTable)dataTable;
            
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            for (int i = 0; i < 10; i++)
                table.Columns.Add("");

            //this.dataGridView.DataSource = table;

            
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
            ComPortForm comform=new ComPortForm();
            comform.delegateUpdateUI += new DelegateUpdateUI(UpdateUI);
            comform.Show();
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

        
        public void UpdateUI(string msgobj)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUpdateUI(UpdateUI),msgobj);
            }
            else
            {
                addone(msgobj);
            }
        }
        public static void addone(string msgobj)
        {
            string msg = (string)msgobj;
            Dictionary<string, string> returnedData = Decode.DecodeCANSignal(msg);
            TreeListViewItem itemA = new TreeListViewItem("messageName ", 0);
            foreach (string key in returnedData.Keys)
            {
                if (key == "messageName")
                {
                    
                    itemA.Expand();
                    itemA.SubItems.Add(returnedData[key]);
                    treeListView1.Items.Add(itemA);
                }
            }
            foreach (string key in returnedData.Keys)
            {
                if (key != "messageName")
                {
                    TreeListViewItem item = new TreeListViewItem(key, 1);
                    item.SubItems.Add(returnedData[key]);
                    itemA.Items.Add(item);
                }
            }
            itemA.Collapse();
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
            treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            treeListView1.AllowColumnReorder = true;
            treeListView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
            treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                            this.columnHeader1,
                                                                                            this.columnHeader2,
                                                                                            this.columnHeader3});
            treeListView1.HideSelection = false;
            treeListView1.LabelEdit = false;
            treeListView1.Location = new System.Drawing.Point(4, 4);
            treeListView1.Name = "treeListView1";
            treeListView1.Size = new System.Drawing.Size(580, 420);
            treeListView1.SmallImageList = this.imageList1;
            treeListView1.TabIndex = 0;
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
            // CurveShowBt
            // 
            
            this.CurveShowBt.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.CurveShowBt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CurveShowBt.Location = new System.Drawing.Point(8, 425);
            //this.CurveShowBt.Name = "CurveShowBt";
            this.CurveShowBt.Size = new System.Drawing.Size(112, 20);
            this.CurveShowBt.TabIndex = 1;
            //this.CurveShowBt.Text = "Add / Remove All";
            //this.CurveShowBt.Click += new System.EventHandler(this.CurveShowBt_Click);
            // 
            // DashboardShowBt
            // 
            
            this.DashboardShowBt.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.DashboardShowBt.FlatStyle = System.Windows.Forms.FlatStyle.System;

            this.DashboardShowBt.Location = new System.Drawing.Point(450, 425);
            //this.DashboardShowBt.Name = "DashboardShowBt";
            this.DashboardShowBt.Size = new System.Drawing.Size(128, 20);
            this.DashboardShowBt.TabIndex = 2;
            //this.DashboardShowBt.Text = "Expand / Collapse All";
            
            //this.DashboardShowBt.Click += new System.EventHandler(this.DashboardShowBt_Click);
            // 
            // TryTreeListView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.DashboardShowBt,
                                                                          this.CurveShowBt,
                                                                          treeListView1});
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }
        private enum DrivesDescr { First, Second, Third, Fourth }
        private enum Drives { C, D, E, Z }

        private void 保存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFile = new OpenFileDialog();
            saveFile.Filter = "cvs|*.cvs|json|*.json|xml|*.xml";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                if (saveFile.FileName != "")
                {
                    string fileName = saveFile.FileName;
                    FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                    if (fileName.EndsWith(".xml"))
                    {

                    }
                    else if (fileName.EndsWith(".cvs"))
                    {

                    }
                    else if (fileName.EndsWith(".json"))
                    {

                    }
                }
            }
        }
    }
}
