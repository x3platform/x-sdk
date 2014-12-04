namespace X3Platform.DataDump.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
    using System.Collections.ObjectModel;

    public class DataDumpTask
    {
        private string m_Name;

        /// <summary>任务名称</summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        private string m_DataDumpProvider = "X3Platform.DataDump.Providers.GenericDataDumpProvider,X3Platform.DataDump";

        /// <summary>数据转储提供器</summary>
        [XmlAttribute("dataDumpProvider")]
        public string DataDumpProvider
        {
            get { return this.m_DataDumpProvider; }
            set { this.m_DataDumpProvider = value; }
        }

        private string m_DataSourceName = "ConnectionString";

        /// <summary>数据源名称</summary>
        [XmlAttribute("datasourceName")]
        public string DataSourceName
        {
            get { return this.m_DataSourceName; }
            set { this.m_DataSourceName = value; }
        }

        private string m_Depends = string.Empty;

        /// <summary>依赖项</summary>
        [XmlAttribute("depends")]
        public string Depends
        {
            get { return this.m_Depends; }
            set { this.m_Depends = value; }
        }

        private string m_Options = string.Empty;

        /// <summary>选项信息</summary>
        [XmlAttribute("options")]
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }

        private string m_OutputFile;

        /// <summary>数据文件路径</summary>
        [XmlAttribute("output")]
        public string OutputFile
        {
            get { return this.m_OutputFile; }
            set { this.m_OutputFile = value; }
        }

        private string m_OutputDbType = string.Empty;

        /// <summary>输出数据库类型</summary>
        [XmlAttribute("outputDbType")]
        public string OutputDbType
        {
            get { return this.m_OutputDbType; }
            set { this.m_OutputDbType = value; }
        }

        #region 属性 Statements
        private Collection<TaskStatement> m_Statements = new Collection<TaskStatement>();

        /// <summary>语句声明</summary>
        [XmlElement("statement")]
        public Collection<TaskStatement> Statements
        {
            get { return this.m_Statements; }
            set { this.m_Statements = value; }
        }
        #endregion
    }
}
