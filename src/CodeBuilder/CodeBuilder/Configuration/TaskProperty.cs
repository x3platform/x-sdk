using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    public class TaskProperty
    {
        private string m_Name;

        /// <summary>
        /// Ãû³Æ
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Value;

        /// <summary>
        /// Öµ
        /// </summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public TaskProperty() { }

        public TaskProperty(string name, string value)
        {
            this.m_Name = name;
            this.m_Value = value;
        }
    }
}
