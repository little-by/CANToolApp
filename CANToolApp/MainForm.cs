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
        //ComPortForm comform = new ComPortForm();
        CurveShow csForm = new CurveShow();

        public event DelegateUpdateUI delegateUpdateUI;

        public MainForm()
        {
            m_SyncContext = SynchronizationContext.Current;
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory.ToString());

            InitTreeList();
            //AddItems();
            addone("t3588A5SD566D9F8SD565");
            string str = XMLProcess.Read("data.xml", "/root/data0/message");
            Console.WriteLine("-----------------------------------------dadadasda"+str);

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
            ComPortForm comform = new ComPortForm();
            comform.delegateUpdateUI += new DelegateUpdateUI(UpdateUI);
            comform.delegateUpdateUI += new DelegateUpdateUI(csForm.UpdateData);
            comform.Show();
        }

        private void CurveShowBt_Click(object sender, EventArgs e)
        {
            csForm.Show();
        }

        private void DashboardShowBt_Click(object sender, EventArgs e)
        {
            GaugeboardShow gsForm = new GaugeboardShow();
            gsForm.Show();
            //DashboardShow dsForm = new DashboardShow();
            //dsForm.Show();
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

        public static void addone(Dictionary<string, string> returnedData)
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

        private enum MessageCol { Name, Value, Data }

        private enum FILETYPE { xml, csv, json }

        private void xml文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            //加载登录名单的xml文档

            doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "data.xml");

            //CANToolApp.XMLProcess.Insert("data.xml", "users", "test", "", "lll");

            //查找namelist节点,并把它赋给root

            XmlNode root = doc.SelectSingleNode("root");
            int j = 0;
            int i = 0;
            foreach(TreeListViewItem item in treeListView1.Items)
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
                        Console.WriteLine(sigsubitem.Text);
                    }
                    data.AppendChild(signal);

                    j++;
                }
                
                j = 0;
                i++;
                root.AppendChild(data);
            }
            doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + "data.xml");
            /*XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "data.xml");
            XmlNode xn = doc.SelectSingleNode("root/row/ko");
            Console.WriteLine("-------------------------------creating" + (xn==null));
            */

        }

        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c://";
            openFileDialog.Filter = "xml文件|*.xml|csv文件|*.csv|json文件|*.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            string fName="";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = openFileDialog.FileName;
                Console.WriteLine(fName+" is opened");
            }
            string[] tmp = fName.Split('.');
            string filetype = tmp[tmp.Length-1];

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
                    XmlNode signal = doc.SelectSingleNode("/root/data" + n );
                    
                    string msg = XMLProcess.Read("data.xml", "/root/data"+n+"/message");
                        if (msg == null||msg.Equals("")) break;
                        Console.WriteLine("--------------------------------" + msg+"//"+( signal.ChildNodes.Count - 1));
                        returnedData.Add("messageName", msg);

                        for (int m = 1; m < (signal.ChildNodes.Count); m++)
                        {
                            string name = XMLProcess.Read("data.xml", "/root/data" + n + "/signal[" + m + "]","name");
                            string value = XMLProcess.Read("data.xml", "/root/data" + n + "/signal["+m+"]");
                            if (name == null||name.Equals("")) break;

                            Console.WriteLine("--------------------------------"+name+"   "+value);

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
                    while (line != null &&!line.StartsWith("messageName"))
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
            //Console.WriteLine(json);
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
    }

}
