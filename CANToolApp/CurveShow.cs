using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CANToolApp
{
    public partial class CurveShow : Form
    {
        public CurveShow()
        {
            InitializeComponent();
        }


        private void CurveShow_Load(object sender, EventArgs e)
        {
            InitTreeView(treeView1,0);
        }

        private void InitTreeView(TreeView treeview,int parentId)
        {
            Dictionary<string, string> returnedData = Decode.DecodeCANSignal("t3588A5SD566D9F8SD565");
            foreach (string key in returnedData.Keys)
            {
                if (key == "messageName")
                {   
                        TreeNode messagenode = new TreeNode();
                        messagenode.Text = returnedData[key];
                        treeView1.Nodes.Add(messagenode);
                    
                    
                    
                }
            }
            foreach (string key in returnedData.Keys)
            {
                if (key != "messageName")
                {
        
                        TreeNode signalenode = new TreeNode();
                        signalenode.Text = key;
                        treeView1.Nodes.Add(signalenode);
                    
                    

                }
            }
   
        }

    }
}
