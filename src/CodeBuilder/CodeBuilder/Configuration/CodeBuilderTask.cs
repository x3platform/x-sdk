using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    public class CodeBuilderTask
    {
        private string m_Name;
        
        /// <summary>
        /// 任务名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Assembly = "X3Platform.CodeBuilder";
        /// <summary>
        /// 程序集
        /// </summary>
        [XmlAttribute("assembly")]
        public string Assembly
        {
            get { return m_Assembly; }
            set { m_Assembly = value; }
        }

        private string m_Generator;
        /// <summary>
        /// 生成器
        /// </summary>
        [XmlAttribute("generator")]
        public string Generator
        {
            get { return m_Generator; }
            set { m_Generator = value; }
        }

        private string m_Action;

        /// <summary>
        /// 操作
        /// </summary>
        [XmlAttribute("action")]
        public string Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        #region 属性 Observeres
        private TaskObserverCollection m_Obverseres = new TaskObserverCollection();

        /// <summary>
        /// Obverser
        /// </summary>
        [XmlElement("observer")]
        public TaskObserverCollection Observeres
        {
            get { return m_Obverseres; }
            set { m_Obverseres = value; }
        }
        #endregion

        #region 属性 Properties
        private TaskPropertyCollection m_Properties = new TaskPropertyCollection();

        /// <summary>
        /// 属性
        /// </summary>
        [XmlElement("property")]
        public TaskPropertyCollection Properties
        {
            get { return m_Properties; }
            set { m_Properties = value; }
        }
                #endregion
    }
}
