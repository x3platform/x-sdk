using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace X3Platform.DataDump.Configuration
{
    [XmlRoot("dataDump")]
    public class DataDumpConfiguration
    {
        #region 属性:Instance
        public static DataDumpConfiguration Instance
        {
            get
            {
                return (DataDumpConfiguration)ConfigurationManager.GetSection("dataDump");
            }
        }
        #endregion

        #region 函数:Save()
        public void Save()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode node;

            string path = System.IO.Directory.GetCurrentDirectory() + "\\X3Platform.DataDump.exe.config";

            doc.Load(path);

            node = doc.GetElementsByTagName("X3Platform.DataDumpConfiguration")[0];

            node.InnerXml = SerializeConfiguration(this);

            doc.Save(path);
        }
        #endregion

        #region 函数:SerializeConfiguration(DataDumpConfiguration value)
        public string SerializeConfiguration(DataDumpConfiguration value)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode node;

            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                // Create an instance of XmlSerializer based on the RewriterConfiguration type...
                XmlSerializer ser = new XmlSerializer(typeof(DataDumpConfiguration));

                // Return the Deserialized object from the Web.config XML
                ser.Serialize(stringWriter, value);

                doc.LoadXml(stringWriter.GetStringBuilder().ToString());
            }

            node = doc.GetElementsByTagName("dataDump")[0];

            return node.InnerXml;
        }
        #endregion

        #region 属性 DataDumpTaskCollection;
        private DataDumpTaskCollection m_Tasks = new DataDumpTaskCollection();

        /// <summary>
        /// 任务配置
        /// </summary>
        [XmlElement("task")]
        public DataDumpTaskCollection Tasks
        {
            get { return m_Tasks; }
            set { m_Tasks = value; }
        }
        #endregion
    }
}
