using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.CodeBuilder.Configuration
{
    [XmlRoot("codeBuilder")]
    public class CodeBuilderConfiguration
    {
        #region 属性:Instance
        public static CodeBuilderConfiguration Instance
        {
            get
            {
                return (CodeBuilderConfiguration)ConfigurationManager.GetSection("codeBuilder");
            }
        }
        #endregion

        #region 函数:Save()
        public void Save()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode node;

            string path = System.IO.Directory.GetCurrentDirectory() + "\\X3Platform.CodeBuilder.exe.config";

            doc.Load(path);

            node = doc.GetElementsByTagName("X3Platform.CodeBuilderConfiguration")[0];

            node.InnerXml = SerializeConfiguration(this);

            doc.Save(path);
        }
        #endregion

        #region 函数:SerializeConfiguration(CodeBuilderConfiguration value)
        public string SerializeConfiguration(CodeBuilderConfiguration value)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode node;

            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                // Create an instance of XmlSerializer based on the RewriterConfiguration type...
                XmlSerializer ser = new XmlSerializer(typeof(CodeBuilderConfiguration));

                // Return the Deserialized object from the Web.config XML
                ser.Serialize(stringWriter, value);

                doc.LoadXml(stringWriter.GetStringBuilder().ToString());
            }

            node = doc.GetElementsByTagName("X3Platform.CodeBuilderConfiguration")[0];

            return node.InnerXml;
        }
        #endregion

        /// <summary>默认名称空间</summary>
        private string m_NamespaceRoot;

        [XmlElement("namespaceRoot")]
        public string NamespaceRoot
        {
            get { return m_NamespaceRoot; }
            set { m_NamespaceRoot = value; }
        }

        private DatabaseProvider m_DatabaseProvider = new DatabaseProvider();

        /// <summary>数据库提供器</summary>
        [XmlElement("databaseProvider")]
        public DatabaseProvider DatabaseProvider
        {
            get { return m_DatabaseProvider; }
            set { m_DatabaseProvider = value; }
        }

        #region 属性 SpecialWords
        private SpecialWords m_SpecialWords = new SpecialWords();

        /// <summary>特殊文字</summary>
        [XmlElement("specialWords")]
        public SpecialWords SpecialWords
        {
            get { return m_SpecialWords; }
            set { m_SpecialWords = value; }
        }
        #endregion

        #region 属性 CodeBuilderTaskCollection;
        private CodeBuilderTaskCollection m_Tasks = new CodeBuilderTaskCollection();

        /// <summary>
        /// 任务配置
        /// </summary>
        [XmlElement("task")]
        public CodeBuilderTaskCollection Tasks
        {
            get { return m_Tasks; }
            set { m_Tasks = value; }
        }
        #endregion
    }
}
