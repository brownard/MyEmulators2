using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public class GroupItemInfo
    {
        public GroupItemInfo(string sql)
        {
            this.sql = sql;
        }

        public GroupItemInfo(int id, bool emulator = false)
        {
            this.id = id;
            this.emulator = emulator;
        }

        string sql = null;
        public string SQL
        {
            get { return sql; }
            set { sql = value; }
        }

        string column = null;
        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        string order = null;
        public string Order
        {
            get { return order; }
            set { order = value; }
        }

        int id = -2;
        public int Id
        {
            get { return id; }
        }

        bool emulator = false;
        public bool Emulator
        {
            get { return emulator; }
        }
    }
}
