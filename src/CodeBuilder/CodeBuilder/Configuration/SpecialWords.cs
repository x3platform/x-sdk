using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace X3Platform.CodeBuilder.Configuration
{
    public class SpecialWords
    {
        private Collection<SpecialWord> m_List = new Collection<SpecialWord>();

        /// <summary>
        /// ÎÄ¼þÂ·¾¶
        /// </summary>
        [XmlElement("add")]
        public Collection<SpecialWord> List
        {
            get { return m_List; }
            set { m_List = value; }
        }
    }
}
