using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using WpfControlLibrary;

namespace CANToolApp
{
    public partial class DashboardShow : Form
    {
        ElementHost host = new ElementHost();
        WpfControlLibrary.UserCl uc = new WpfControlLibrary.UserCl();
        public DashboardShow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create the ElementHost control for hosting the
            // WPF UserControl.
            host.Dock = DockStyle.Fill;
            // Create the WPF UserControl.
            // Assign the WPF UserControl to the ElementHost control's
            // Child property.
            host.Child = uc;
            // Add the ElementHost control to the form's
            // collection of child controls.
            this.panel1.Controls.Add(host);
            uc.setMaxNum(100);
            uc.setMinNum(-100);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            uc.setValue(trackBar1.Value);
        }
    }

}
