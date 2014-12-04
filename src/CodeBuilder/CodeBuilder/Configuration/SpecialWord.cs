using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    /// <summary>特殊文字</summary>
    public class SpecialWord
    {
        private string m_Name;

        /// <summary>名称</summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Value;

        /// <summary>值</summary>
        [XmlAttribute("value")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public SpecialWord() { }

        public SpecialWord(string name, string value)
        {
            this.m_Name = name;
            this.m_Value = value;
        }
    }
}
