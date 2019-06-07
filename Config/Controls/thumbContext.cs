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
    public partial class ThumbContext : ContextMenuStrip
    {
        public ThumbContext()
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size(171, 70);
            this.Opening += new CancelEventHandler(ThumbContext_Opening);

            // view Thumb
            ToolStripMenuItem view = new ToolStripMenuItem();
            view.Size = new System.Drawing.Size(170, 22);
            view.Text = "View Thumb";
            view.Click += new EventHandler(view_Click);
            
            // delete Thumb
            ToolStripMenuItem delete = new ToolStripMenuItem();
            delete.Size = new System.Drawing.Size(170, 22);
            delete.Text = "Delete Thumb";
            delete.Click += new EventHandler(delete_Click);
            
            // browse In Explorer
            ToolStripMenuItem browse = new ToolStripMenuItem();
            browse.Size = new System.Drawing.Size(170, 22);
            browse.Text = "Browse in Explorer";
            browse.Click += new EventHandler(browse_Click);

            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] 
            {
            view,
            delete,
            browse
            });
            
        }

        void browse_Click(object sender, EventArgs e)
        {
            if (panel == null)
                return;

            panel.ThumbGroup.BrowseThumbs(panel.ThumbType);
        }

        void delete_Click(object sender, EventArgs e)
        {
            if (panel == null)
                return;

            panel.ThumbGroup.UpdateThumb(panel.ThumbType, "");
            panel.BackgroundImage = null;
        }

        void view_Click(object sender, EventArgs e)
        {
            if (panel == null)
                return;

            panel.ThumbGroup.LaunchThumb(panel.ThumbType);
        }

        ThumbPanel panel = null;
        void ThumbContext_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            if (menu != null)
            {
                panel = menu.SourceControl as ThumbPanel;
            }
            else
                panel = null;
        }
    }
}
