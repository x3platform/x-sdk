using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Data
{
    /// <summary>
    /// 数据库摘要
    /// </summary>
    public class DatabaseSchema
    {
        private string m_Name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_OwnerName;

        /// <summary>
        /// 拥有者
        /// </summary>
        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }

        private List<DataTableSchema> tables = null;

        public void Add(DataTableSchema table)
        {
            tables.Add(table);
        }

        public void Remove(DataTableSchema table)
        {
            tables.Remove(table);
        }

        public List<DataTableSchema> Tables
        {
            get { return tables; }
        }

    }
}
