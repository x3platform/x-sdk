using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    public class DatabaseProvider
    {
        private string m_Assembly = "X3Platform.CodeBuilder";

        /// <summary>
        /// ����
        /// </summary>
        [XmlAttribute("assembly")]
        public string Assembly
        {
            get { return m_Assembly; }
            set { m_Assembly = value; }
        }

        private string m_ClassName = "X3Platform.CodeBuilder";
        /// <summary>
        /// ����
        /// </summary>
        [XmlAttribute("className")]
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        private string m_Type;

        /// <summary>
        /// ����
        /// </summary>
        [XmlAttribute("type")]
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Name;

        /// <summary>
        /// �ļ�·��
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_OwnerName;

        /// <summary>
        /// �ļ�·��
        /// </summary>
        [XmlAttribute("ownerName")]
        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }

        private string m_ConnectionString;

        /// <summary>
        /// �ļ�·��
        /// </summary>
        [XmlElement("connectionString")]
        public string ConnectionString
        {
            get { return m_ConnectionString; }
            set { m_ConnectionString = value; }
        }
    }
}
