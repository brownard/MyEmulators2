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
    public partial class Conf_ManualSearch : Form
    {
        public string SearchTerm { get; private set; }
        public Conf_ManualSearch(string title)
        {
            InitializeComponent();
            SearchTerm = title;
            searchTextBox.SelectedText = title;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchTerm = searchTextBox.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
