using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    public partial class Conf_ProgressDialog : Form
    {
        ITaskProgress handler;

        public Conf_ProgressDialog(ITaskProgress handler)
        {
            InitializeComponent();
            this.handler = handler;
            statusLabel.Text = "";
        }

        void Conf_ProgressDialog_Load(object sender, EventArgs e)
        {
            if (handler == null)
                Close();
            handler.OnTaskProgress += new BackgroundTaskProgress(updateStatusInfo);
            if (!handler.Start())
                Close();
        }

        void updateStatusInfo(int perc, string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() 
                    {
                        updateStatusInfo(perc, status); 
                    }
                    ));
                return;
            }

            if (perc < 0)
                perc = 0;
            else if (perc > 100)
                perc = 100;
            
            statusLabel.Text = status;
            progressBar.Value = perc;
            
            if (handler.IsComplete)
                Close();
        }

        void Conf_ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (handler != null)
                handler.Stop();
        }
    }
}
