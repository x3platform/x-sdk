using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.DataDump.Configuration
{
    public class TaskStatement
    {
        private string m_Description;

        /// <summary>名称</summary>
        [XmlElement("description")]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        private string m_Sql;

        /// <summary>查询语句</summary>
        [XmlElement("sql")]
        public string Sql
        {
            get { return m_Sql; }
            set { m_Sql = value; }
        }

        private string m_DestTable;

        /// <summary>目标表的名称</summary>
        [XmlElement("destTable")]
        public string DestTable
        {
            get { return m_DestTable; }
            set { m_DestTable = value; }
        }

        public TaskStatement() { }
    }
}
