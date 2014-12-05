using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Data
{
    /// <summary>���ݱ���Ϣ</summary>
    public class DataTableSchema
    {
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Description;

        /// <summary>����</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        private DataColumnSchemaCollection columns = new DataColumnSchemaCollection();

        public DataTableSchema(string tableName)
            : this()
        {
            this.m_Name = tableName;
        }

        public DataTableSchema()
        {
        }

        /// <summary>�����м���</summary>
        public DataColumnSchemaCollection Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }
}
