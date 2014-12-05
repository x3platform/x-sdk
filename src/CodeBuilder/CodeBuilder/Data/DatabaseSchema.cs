using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Data
{
    /// <summary>
    /// ���ݿ�ժҪ
    /// </summary>
    public class DatabaseSchema
    {
        private string m_Name;

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_OwnerName;

        /// <summary>
        /// ӵ����
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
