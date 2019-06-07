using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    class ListItemComparer : IComparer<ExtendedGUIListItem>
    {
        int modifier = 1;
        string propertyStr = null;
        public ListItemComparer(ListItemProperty itemProperty, bool descending = false)
        {
            if (descending)
                modifier = -1;

            switch (itemProperty)
            {
                case ListItemProperty.COMPANY:
                    propertyStr = "Company";
                    break;
                case ListItemProperty.GRADE:
                    propertyStr = "Grade";
                    break;
                case ListItemProperty.LASTPLAYED:
                    propertyStr = "LastPlayed";
                    break;
                case ListItemProperty.PLAYCOUNT:
                    propertyStr = "PlayCount";
                    break;
                case ListItemProperty.TITLE:
                    propertyStr = "Label";
                    break;
                case ListItemProperty.YEAR:
                    propertyStr = "ReleaseYear";
                    break;
            }
        }

        #region IComparer<ExtendedGUIListItem> Members

        public int Compare(ExtendedGUIListItem x, ExtendedGUIListItem y)
        {
            if (x == y)
                return 0;
            if (y == null)
                return -1;
            if(x == null)
                return 1;

            if (string.IsNullOrEmpty(propertyStr))
                return x.ListPosition.CompareTo(y.ListPosition);

            if (!y.Sortable)
                return -1;
            if (!x.Sortable)
                return 1;

            object yVal = y.GetType().GetProperty(propertyStr).GetValue(y, null);
            object xVal = x.GetType().GetProperty(propertyStr).GetValue(x, null);

            if (yVal == null)
                return -1;

            if (xVal == null)
                return 1;

            if (yVal.GetType() == typeof(string))
            {
                yVal = ((string)yVal).ToLower();
                xVal = ((string)xVal).ToLower();
            }

            int compare = modifier * (int)xVal.GetType().GetMethod("CompareTo", new[] { typeof(object) }).Invoke(xVal, new[] { yVal });
            if (compare == 0 && propertyStr != "Label")
                compare = x.Label.ToLower().CompareTo(y.Label.ToLower());
            return compare;
        }

        #endregion
    }

    public enum ListItemProperty
    {
        TITLE,
        YEAR,
        COMPANY,
        PLAYCOUNT,
        LASTPLAYED,
        GRADE,
        NONE,
        DEFAULT
    }
}
