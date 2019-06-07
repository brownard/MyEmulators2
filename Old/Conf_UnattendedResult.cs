using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    public partial class Conf_UnattendedResult : Form
    {
        public Conf_UnattendedResult(string status, List<string> results)
        {
            InitializeComponent();

            statusLabel.Text = status;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                sb.AppendLine(results[i]);
            }
            resultsTextBox.Text = sb.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
