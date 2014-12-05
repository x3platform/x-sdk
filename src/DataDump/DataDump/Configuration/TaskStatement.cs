using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.DataDump.Configuration
{
    public class TaskStatement
    {
        private string m_Description;

        /// <summary>����</summary>
        [XmlElement("description")]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        private string m_Sql;

        /// <summary>��ѯ���</summary>
        [XmlElement("sql")]
        public string Sql
        {
            get { return m_Sql; }
            set { m_Sql = value; }
        }

        private string m_DestTable;

        /// <summary>Ŀ��������</summary>
        [XmlElement("destTable")]
        public string DestTable
        {
            get { return m_DestTable; }
            set { m_DestTable = value; }
        }

        public TaskStatement() { }
    }
}
