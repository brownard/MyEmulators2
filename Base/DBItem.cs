using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public abstract class DBItem
    {
        public abstract ExtendedGUIListItem CreateGUIListItem();

        internal void DeleteThumbs()
        {
            using (ThumbGroup thumbs = new ThumbGroup(this))
            {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(thumbs.ThumbPath);
                if (dir.Exists)
                {
                    Logger.LogDebug("Deleting thumb folder {0}", dir.FullName);
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogDebug("Failed to delete pre-existing thumb folder {0} - {1}", dir.FullName, ex.Message);
                    }
                }
            }
        }
    }
}
