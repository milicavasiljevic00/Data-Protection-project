using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoHost
{
    public partial class Form1 : Form
    {
        private ServiceHost host;
        public Form1()
        {
            InitializeComponent();
            host = new ServiceHost(typeof(CryptoService.CryptoService));
            host.Open();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            label1.Text = "Servis started";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(CryptoService.CryptoService));
            host.Open();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            label1.Text = "Servis started";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            host.Close();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            label1.Text = "Servis stopped";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            host.Close();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}
