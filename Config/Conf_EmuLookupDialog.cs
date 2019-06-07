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
    public partial class Conf_EmuLookupDialog : Form
    {
        Dictionary<string, string> platforms = null;

        public string SelectedKey
        {
            get;
            protected set;
        }

        public Conf_EmuLookupDialog(Dictionary<string, string> platforms)
        {
            InitializeComponent();
            this.platforms = platforms;
        }

        private void Conf_EmuLookupDialog_Load(object sender, EventArgs e)
        {
            if (platforms == null)
                return;

            foreach (KeyValuePair<string, string> platform in platforms)
            {
                comboBox1.Items.Add(platform.Key);
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void Conf_EmuLookupDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedKey = comboBox1.SelectedItem as string;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
