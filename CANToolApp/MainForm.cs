using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using System.IO;
using System.Collections;
using static System.Windows.Forms.ListViewItem;
using System.Xml;
using System.Text;
using JsonSerializerAndDeSerializer;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows;
using LedDigitalDemo;
namespace CANToolApp
{
    public delegate void DelegateSendSig(List<string> sigs);
    public delegate void DelegateSendData(Dictionary<string, string> returnedData);
    public delegate void DelegateUpdateUI(string msgobj);
    public partial class MainForm : Form
    {
        private static SynchronizationContext m_SyncContext = null;
        //private static TreeListView treeListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        //ComPortForm comform = new ComPortForm();
        CurveShow csForm = null;
        GaugeboardShow gsForm = null;
        MainWindow ledwindow = null;
        //Table数据源
        DataTable dataTable = new DataTable();
        //public event DelegateUpdateUI delegateUpdateUI;
        //绘制signal需要
        TableMsg tm = null;
        Color[][] tablecolor = new Color[9][];

        private event DelegateSendData delegateSendData;
        private event DelegateSendSig delegateSendSig;
        private event DelegateSendSig delegateSendSigForGs;

        public MainForm()
        {
            m_SyncContext = SynchronizationContext.Current;
            InitializeComponent();
            Random rd = new Random();
            for (int i = 0; i < 30; i++)
            {
                colors[i] = Color.FromArgb(rd.Next(0, 255), rd.Next(0, 255), rd.Next(0, 255));
            }
            clearColor();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            InitTreeList();
            //下面是显示每个message的bit用到的程序
            //DataGridView的数据源
            dataTable = new DataTable();
            //绑定数据源
            dataGridView1.DataSource = dataTable;
            //dataGridView1.Width = dataGridView1.Parent.Width;
            //初始化列
            for (int i = 0; i < 8; i++)
            {
                dataTable.Columns.Add("" + (7 - i));
                //8.53对于每列最合适
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width / (8.6));
            }
            //初始化显示表格
            InitTable();
        }

        private void sendBt_Click(object sender, EventArgs e)
        {
            CANToolApp.SqlHelper.connect();
            SqlDataReader dr = CANToolApp.SqlHelper.query("select * from cantoolapp.canmessage");
            CANToolApp.SqlHelper.close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void cOM口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComPortForm comform = new ComPortForm();
            comform.delegateUpdateUI += new DelegateUpdateUI(UpdateUI);
            //Decode.delegateUpdateLog += new DelegateUpdateLog(comform.output);
            //comform.delegateUpdateUI += new DelegateUpdateUI(csForm.UpdateData);
            comform.Show();
        }

        private void CurveShowBt_Click(object sender, EventArgs e)
        {
            List<String> signallist = new List<string>();

            foreach (TreeListViewItem item in treeListView1.CheckedItems)
            {

                if (!item.Text.StartsWith("messageName"))
                    signallist.Add(item.Text);
            }
            csForm = new CurveShow();
            delegateSendData += new DelegateSendData(csForm.UpdateData);
            delegateSendSig += new DelegateSendSig(csForm.UpdateSignal);
            csForm.Show();
            delegateSendSig(signallist);
        }

        private void DashboardShowBt_Click(object sender, EventArgs e)
        {
            List<String> signallist = new List<string>();

            foreach (TreeListViewItem item in treeListView1.CheckedItems)
            {

                if (!item.Text.StartsWith("messageName"))
                {
                    string sig = "";
                    foreach (ListViewSubItem sub in item.SubItems)
                    {

                        sig += (" " + sub.Text);
                    }

                    signallist.Add(sig);
                }

            }
            gsForm = new GaugeboardShow();
            delegateSendData += new DelegateSendData(gsForm.UpdateData);
            delegateSendSigForGs += new DelegateSendSig(gsForm.UpdateSignal);
            gsForm.Show();
            delegateSendSigForGs(signallist);
            //DashboardShow dsForm = new DashboardShow();
            //dsForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LedDigitalDemo.MainWindow mw=LedDigitalDemo.MainWindow;
            //Uri uri=new Uri("MainWindow.xaml", UriKind.Relative);
            List<String> signallist = new List<string>();

            foreach (TreeListViewItem item in treeListView1.CheckedItems)
            {

                if (!item.Text.StartsWith("messageName"))
                {
                    string sig = "";
                    foreach (ListViewSubItem sub in item.SubItems)
                    {

                        sig += (" " + sub.Text);
                    }

                    signallist.Add(sig);
                }

            }
            
            ledwindow = new CANToolApp.MainWindow();
            delegateSendData += new DelegateSendData(ledwindow.UpdateData);
            delegateSendSigForGs += new DelegateSendSig(ledwindow.UpdateSignal);
            ledwindow.Show();
            delegateSendSigForGs(signallist);
            
        }


        public void UpdateUI(string msgobj)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUpdateUI(UpdateUI), msgobj);
            }
            else
            {
                addone(msgobj);
            }
        }
        public void addone(string msgobj)
        {
            string msg = (string)msgobj;
            Dictionary<string, string> returnedData = Decode.DecodeCANSignal(msg);
            if (csForm != null)
            {
                delegateSendData(returnedData);
            }
            if (gsForm != null)
            {
                delegateSendData(returnedData);
            }
            if (ledwindow != null)
            {
                delegateSendData(returnedData);

            }


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

        public void addone(Dictionary<string, string> returnedData)
        {
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
            //treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeListView1
            // 
            treeListView1.AllowColumnReorder = true;
            //treeListView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            //   | System.Windows.Forms.AnchorStyles.Left)
            //   | System.Windows.Forms.AnchorStyles.Right);
            treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                            this.columnHeader1,
                                                                                            this.columnHeader2,
                                                                                            this.columnHeader3});
            treeListView1.HideSelection = false;
            treeListView1.LabelEdit = false;
            //treeListView1.Location = new System.Drawing.Point(4, 4);
            //treeListView1.Name = "treeListView1";
            //treeListView1.Size = new System.Drawing.Size(580, 420);
            treeListView1.SmallImageList = this.imageList1;
            treeListView1.TabIndex = 0;
            //this.treeListView1.BeforeLabelEdit += new System.Windows.Forms.TreeListViewBeforeLabelEditEventHandler(this.treeListView1_BeforeLabelEdit);
            //this.treeListView1.BeforeCollapse += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeCollapse);
            //this.treeListView1.BeforeExpand += new System.Windows.Forms.TreeListViewCancelEventHandler(this.treeListView1_BeforeExpand);
            // 
            // columnHeader1
            // 
            //添加鼠标点击事件
            treeListView1.MouseClick += TreeListView1_MouseClick;
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 230;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Attribute";
            this.columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Remark";
            this.columnHeader3.Width = 100;
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

            //this.CurveShowBt.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.CurveShowBt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            //this.CurveShowBt.Location = new System.Drawing.Point(8, 425);
            //this.CurveShowBt.Name = "CurveShowBt";
            //this.CurveShowBt.Size = new System.Drawing.Size(112, 20);
            this.CurveShowBt.TabIndex = 1;
            //this.CurveShowBt.Text = "Add / Remove All";
            //this.CurveShowBt.Click += new System.EventHandler(this.CurveShowBt_Click);
            // 
            // DashboardShowBt
            // 

            //this.DashboardShowBt.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.DashboardShowBt.FlatStyle = System.Windows.Forms.FlatStyle.System;

            //this.DashboardShowBt.Location = new System.Drawing.Point(450, 425);
            //this.DashboardShowBt.Name = "DashboardShowBt";
            //this.DashboardShowBt.Size = new System.Drawing.Size(128, 20);
            this.DashboardShowBt.TabIndex = 2;
            //this.DashboardShowBt.Text = "Expand / Collapse All";

            //this.DashboardShowBt.Click += new System.EventHandler(this.DashboardShowBt_Click);
            // 
            // TryTreeListView
            // 
            //this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            //this.ClientSize = new System.Drawing.Size(600, 450);
            //this.Controls.AddRange(new System.Windows.Forms.Control[] {
            //                                                             this.DashboardShowBt,
            //                                                            this.CurveShowBt,
            //                                                             treeListView1});
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);


        }

        private void TreeListView1_MouseClick(object sender, MouseEventArgs e)
        {
            string canData = "";
            string msgName = "";
            string tmp = "";
            tm = new TableMsg(dataTable);
            TreeListViewItem msgitem = null;
            TreeListViewItem item = treeListView1.GetItemAt(new System.Drawing.Point(e.X, e.Y));
            if (item.Text.Equals("messageName "))
            {
                msgitem = item;
                string datastr = "";
                foreach (ListViewSubItem msgdata in msgitem.SubItems)
                {
                    tmp = msgdata.Text;
                }
                canData = tmp.Split(' ')[1];
                msgName = tmp.Split(' ')[0];
                Decode.DecodeCANSignal(canData, msgName, ref dataTable, out tm);
                updateUi(tm);

            }
            else
            {
                msgitem = item.Parent;
                string datastr = "";
                foreach (ListViewSubItem msgdata in msgitem.SubItems)
                {
                    tmp = msgdata.Text;
                }

                canData = tmp.Split(' ')[1];
                msgName = tmp.Split(' ')[0];
                Decode.DecodeCANSignal(canData, msgName, ref dataTable, out tm);
                Dictionary<string, string> returnedDatatmp = new Dictionary<string, string>();
                foreach (string key in tm.ReturnedData.Keys)
                {
                    if (key.Equals(item.Text))
                    {
                        returnedDatatmp.Add(key, tm.ReturnedData[key]);
                    }
                }
                tm.ReturnedData = returnedDatatmp;

                updateUi(tm);

            }
            //Thread th = new Thread(new ParameterizedThreadStart(UpdateTableThread.updateUi));
            //th.Start(tm);


        }

        private void InitTable()
        {
            dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;
            for (int i = 0; i < 8; i++)
            {
                DataRow dr = dataTable.NewRow();
                for (int j = 0; j < 8; j++)
                {
                    //两种存取方式
                    //dataTable.Rows[0].ItemArray[0] = 0;
                    dr[j] = 0;
                }
                dataTable.Rows.Add(dr);

            }
            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width / (8.53));
            }

        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < tablecolor.Length)
            {
                for (int i = 0; i < 8; i++)
                {
                    this.dataGridView1.Rows[e.RowIndex].Cells["" + (7 - i)].Style.BackColor = tablecolor[e.RowIndex][i];
                }

            }
        }

        public void updateUi(object tablemsg)
        {
            clearColor();
            tm = (TableMsg)tablemsg;
            dataTable.Clear();
            for (int i = 0; i < 8; i++)
            {
                DataRow dr = dataTable.NewRow();
                for (int j = 0; j < 8; j++)
                {
                    //两种存取方式
                    //dataTable.Rows[0].ItemArray[0] = 0;
                    dr[7-j] = tm.Binarydata[8 * i + j];
                }
                dataTable.Rows.Add(dr);

            }
            for (int i = 0; i < 8; i++)
            {
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width / (8.53));
            }
            //dataGridView1.DataSource = dataTable;
            int num = 0;
            foreach (string str in tm.ReturnedData.Values)
            {
                if (str.Split('@')[1].Equals("0+"))
                {
                    string[] startandlen = str.Split('@')[0].Split('|');
                    int startpos = int.Parse(startandlen[0]);
                    int len = int.Parse(startandlen[1]);
                    int row = startpos / 8;
                    int col = startpos % 8;
                    for (int i = 0; i < len; i++)
                    {
                        tablecolor[row][7 - col] = colors[num];
                        col--;
                        if (col < 0)
                        {
                            col = 7;
                            row++;
                        }
                    }

                }
                else
                {
                    string[] startandlen = str.Split('@')[0].Split('|');
                    int startpos = int.Parse(startandlen[0]);
                    int len = int.Parse(startandlen[1]);
                    int row = startpos / 8;
                    int col = startpos % 8;
                    for (int i = 0; i < len; i++)
                    {
                        tablecolor[row][7 - col] = colors[num];
                        col++;
                        if (col > 7)
                        {
                            col = 0;
                            row++;
                        }
                    }
                }
                num++;

            }
            dataGridView1.Refresh();


        }
        private void clearColor()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablecolor[i] = new Color[8];
                    tablecolor[i][j] = Color.White;
                }
            }
        }

        private enum DrivesDescr { First, Second, Third, Fourth }
        private enum Drives { C, D, E, Z }

        private enum MessageCol { Name, Value, Data }

        private enum FILETYPE { xml, csv, json }

        private Color[] colors = new Color[30];

        private void xml文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml file|*.xml";
            saveFileDialog.Title = "Save an Xml File";
            saveFileDialog.ShowDialog();

            XmlDocument doc = new XmlDocument();
            //加载登录名单的xml文档
            doc.LoadXml("<root></root>");
            //doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "data.xml");

            //CANToolApp.XMLProcess.Insert("data.xml", "users", "test", "", "lll");

            //查找namelist节点,并把它赋给root

            XmlNode root = doc.SelectSingleNode("root");
            int j = 0;
            int i = 0;
            foreach (TreeListViewItem item in treeListView1.Items)
            {
                //配置realname节点,赋给cname（childname）

                XmlElement data = doc.CreateElement("data" + i);
                //配置name节点,赋给ccname

                XmlElement message = doc.CreateElement("message");


                //向ccname节点中加入内容
                message.SetAttribute("name", item.Text);
                foreach (ListViewSubItem msgsubitem in item.SubItems)
                {
                    message.InnerText = msgsubitem.Text;
                }

                data.AppendChild(message);
                foreach (TreeListViewItem sigitem in item.Items)
                {
                    XmlElement signal = doc.CreateElement("signal");

                    signal.SetAttribute("name", sigitem.Text);
                    foreach (ListViewSubItem sigsubitem in sigitem.SubItems)
                    {
                        signal.InnerText = sigsubitem.Text;
                    }
                    data.AppendChild(signal);

                    j++;
                }

                j = 0;
                i++;
                root.AppendChild(data);
            }
            if (saveFileDialog.FileName != "")
            {
                doc.Save(saveFileDialog.FileName);
            }
        }

        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c://";
            openFileDialog.Filter = "xml文件|*.xml|csv文件|*.csv|json文件|*.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            string fName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = openFileDialog.FileName;
                Console.WriteLine(fName + " is opened");
            }
            string[] tmp = fName.Split('.');
            string filetype = tmp[tmp.Length - 1];

            if (filetype.Equals("xml"))
            {
                XmlDocument doc = new XmlDocument();
                //加载登录名单的xml文档
                doc.Load(fName);
                XmlNode root = doc.SelectSingleNode("root");
                //string str = XMLProcess.Read("data.xml", "/root/data0/message");
                for (int n = 0; n < root.ChildNodes.Count; n++)
                {
                    Dictionary<string, string> returnedData = new Dictionary<string, string>();
                    XmlNode signal = doc.SelectSingleNode("/root/data" + n);

                    string msg = XMLProcess.Read(fName, "/root/data" + n + "/message");
                    if (msg == null || msg.Equals("")) break;
                    returnedData.Add("messageName", msg);

                    XmlNodeList xns = doc.SelectNodes("/root/data" + n + "/signal");
                    for (int m = 0; m < xns.Count; m++)
                    {
                        string name = xns[m].Attributes["name"].Value;
                        string value=xns[m].InnerText;
                        if (name == null || name.Equals("")) break;
                        if (returnedData.ContainsKey(name)) break;  
                        returnedData.Add(name, value);
                       
                        
                    }
                    addone(returnedData);
                }
            }
            else if (filetype.Equals("csv"))
            {
                StreamReader sr = new StreamReader(fName, Encoding.UTF8);
                Dictionary<string, string> data = null;
                string[] temp;
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("messageName"))
                    {
                        temp = Regex.Split(line, "\\s*,");
                        data = null;
                        data = new Dictionary<string, string>();
                        data.Add(temp[0], temp[1]);
                    }
                    line = sr.ReadLine();
                    while (line != null && !line.StartsWith("messageName"))
                    {
                        temp = line.Split(',');
                        data.Add(temp[0], temp[1]);
                        line = sr.ReadLine();
                    }
                    addone(data);
                }
                sr.Close();
            }
            else if (filetype.Equals("json"))
            {
                StreamReader sr = new StreamReader(fName, Encoding.UTF8);
                StringBuilder sb = new StringBuilder();
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                string data = sb.ToString();
                string[] msgAndSig = Regex.Split(data, "{\"message\":");
                for (int i = 0; i < msgAndSig.Length - 1; i++)
                {
                    string readjson = "{\"message\":" + msgAndSig[i + 1];
                    using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(readjson)))
                    {
                        DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(MsgJson));
                        MsgJson model = (MsgJson)deseralizer.ReadObject(ms);// //反序列化ReadObject
                        Dictionary<string, string> jsondata = new Dictionary<string, string>();
                        jsondata.Add("messageName", model.message);
                        foreach (SigJson sigjson in model.signal)
                        {
                            jsondata.Add(sigjson.sigName, sigjson.pyh);
                        }
                        addone(jsondata);
                    }
                }
            }

        }

        private void json文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream msObj = new MemoryStream();
            foreach (TreeListViewItem item in treeListView1.Items)
            {
                MsgJson msgjson = new MsgJson();
                foreach (ListViewSubItem msgitem in item.SubItems)
                {
                    msgjson.message = msgitem.Text;
                }
                msgjson.signal = new SigJson[item.Items.Count];
                int i = 0;
                foreach (TreeListViewItem sigitem in item.Items)
                {
                    SigJson temp = new SigJson();
                    temp.sigName = sigitem.Text;
                    foreach (ListViewSubItem sigsubitem in sigitem.SubItems)
                    {
                        temp.pyh = sigsubitem.Text;
                    }
                    msgjson.signal[i] = temp;
                    i++;
                }
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(MsgJson));
                js.WriteObject(msObj, msgjson);
            }
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
            //json字符串
            string json = sr.ReadToEnd();

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "json files(*.json)|*.json";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        sw.Write(json);
                    }
                    myStream.Close();
                }
            }
            sr.Close();
            msObj.Close();
        }

        private void csv文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv files(*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            FileStream fs = null;
            StreamWriter sw = null;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                sw = new StreamWriter(fs);
            }
            foreach (TreeListViewItem item in treeListView1.Items)
            {
                string temp = "";
                foreach (ListViewSubItem msgitem in item.SubItems)
                {
                    temp = item.Text + "," + msgitem.Text;
                }
                sw.WriteLine(temp);
                foreach (TreeListViewItem sigitem in item.Items)
                {
                    foreach (ListViewSubItem sigsubitem in sigitem.SubItems)
                    {
                        temp = sigitem.Text + "," + sigsubitem.Text;
                    }
                    sw.WriteLine(temp);
                }
            }
            sw.Close();
            fs.Close();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComPortForm comform = new ComPortForm();
            comform.delegateUpdateUI += new DelegateUpdateUI(UpdateUI);
            //comform.delegateUpdateUI += new DelegateUpdateUI(csForm.UpdateData);
            comform.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
