using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    public class TaskObserver
    {
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

        private string m_Path;
        
        /// <summary>
        /// �ļ�·��
        /// </summary>
        [XmlAttribute("path")]
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }
    }
}
