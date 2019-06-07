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
    public partial class Conf_IgnoredFiles : Form
    {
        public Conf_IgnoredFiles()
        {
            InitializeComponent();
        }

        private void Conf_IgnoredFiles_Load(object sender, EventArgs e)
        {
            foreach (string path in Options.Instance.IgnoredFiles())
            {
                dataGridView1.Rows.Add(path);
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string path = row.Cells[0].Value as string;
                Options.Instance.RemoveIgnoreFile(path);
                dataGridView1.Rows.Remove(row);
            }
        }
    }
}
