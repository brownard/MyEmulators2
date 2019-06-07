using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    /// <summary>
    /// Class used to store column info
    /// </summary>
    class ColumnInfo
    {
        public ColumnInfo(string index, string name, string type)
        {
            Index = int.Parse(index);
            Name = name;

            if (type == "int")
                Type = ColumnType.Int;
            else if (type == "char(5)")
                Type = ColumnType.Bool;
            else
                Type = ColumnType.String;
        }

        internal string columnValue = "";

        /// <summary>
        /// Returns the column value formatted for use in an SQL statement
        /// </summary>
        public string SQLValue
        {
            get
            {
                string format;
                if (Type != ColumnType.Int && columnValue != "NULL")
                    format = "'{0}'";
                else
                    format = "{0}";

                return string.Format(System.Globalization.CultureInfo.InvariantCulture, format, columnValue);
            }
            set
            {
                columnValue = getColumnValue(value);
            }
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public ColumnType Type { get; set; }

        //sanitises or creates default column value
        string getColumnValue(string colVal)
        {
            if (colVal == "NULL")
                return colVal;

            //int column
            if (Type == ColumnType.Int)
            {
                int y;
                //if data not present or not a valid int, create default
                if (string.IsNullOrEmpty(colVal) || !int.TryParse(colVal, out y))
                    colVal = "0";
            }

            //bool column
            else if (Type == ColumnType.Bool)
            {
                bool y;
                //if data not present or not a valid bool, create default
                if (string.IsNullOrEmpty(colVal) || !bool.TryParse(colVal, out y))
                    colVal = "False";
            }
            //else just ensure it's not null
            else if (string.IsNullOrEmpty(colVal))
                colVal = "";

            return colVal;
        }
    }

    enum ColumnType
    {
        Int,
        Bool,
        String
    }
}
