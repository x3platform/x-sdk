namespace X3Platform.CodeBuilder.Templates.Java.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Configuration;

    /// <summary>数据层</summary>
    public abstract class DataAccessGenerator : JavaGenerator
    {
        #region 属性:NamespacePrefix
        private string m_NamespacePrefix;

        /// <summary>名称空间前缀</summary>
        public string NamespacePrefix
        {
            get { return this.m_NamespacePrefix; }
            set { this.m_NamespacePrefix = value; }
        }
        #endregion

        #region 属性:ClassName
        private string m_ClassName;

        /// <summary>
        /// 类的名称
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region 属性:EntityClassPackage
        private string m_EntityClassPackage;
        /// <summary>实体类所在的包名称</summary>
        public string EntityClassPackage
        {
            get { return m_EntityClassPackage; }
            set { m_EntityClassPackage = value; }
        }
        #endregion

        #region 属性:EntityClass
        private string m_EntityClass;
        /// <summary>
        /// 实体类
        /// </summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region 属性:ApplicationName
        private string m_ApplicationName;

        /// <summary>应用名称</summary>
        public string ApplicationName
        {
            get { return this.m_ApplicationName; }
            set { this.m_ApplicationName = value; }
        }
        #endregion

        #region 属性:DataAccessInterface
        private string m_DataAccessInterface;
        /// <summary>
        /// 数据层接口
        /// </summary>
        public string DataAccessInterface
        {
            get { return m_DataAccessInterface; }
            set { m_DataAccessInterface = value; }
        }
        #endregion

        /// <summary>表名称</summary>
        protected string DataTableName;

        /// <summary>表结构</summary>
        protected DataTableSchema table;

        /// <summary>字段信息</summary>
        protected IList<JavaField> fields;

        /// <summary>支持授权</summary>
        protected string SupportAuthorization;

        #region 函数:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // 名称空间前缀
            this.DataTableName = configuration.Tasks[taskName].Properties["DataTable"].Value;

            // 名称空间
            this.Package = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Package"].Value;

            // 类名称
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"] == null ? "" : configuration.Tasks[taskName].Properties["ClassName"].Value;

            // 实体类所在的包名称
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"] == null ? "" : configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;

            // 实体类名称
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // 应用名称
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"] == null ? "" : configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            // 数据层接口
            this.DataAccessInterface = configuration.Tasks[taskName].Properties["DataAccessInterface"] == null ? "" : configuration.Tasks[taskName].Properties["DataAccessInterface"].Value;

            // 数据层接口
            this.SupportAuthorization = configuration.Tasks[taskName].Properties["SupportAuthorization"] == null ? "" : configuration.Tasks[taskName].Properties["SupportAuthorization"].Value;

            //设置 table 信息
            IDbSchemaProvider provider = (IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            DatabaseSchema database = new DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            table = provider.GetTable(database.Name, database.OwnerName, this.DataTableName);

            var noPrimaryKeyColumns = provider.GetNoPrimaryKeyColumns(table);

            this.fields = GetFields(table);

            // 设置 输出文件
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", this.ClassName + ".cs"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = this.ClassName + ".cs";
            }
        }
        #endregion

        public string GetColumnResult(DataColumnSchema column)
        {
            string param;

            param = "(dr[\"" + column.Name + "\"] == DBNull.Value) ? " + this.GetDefaultValue(column.Type) + ":" +
                "(" + this.ConvertType(column.Type) + ")dr[\"" + column.Name + "\"]";

            return param;
        }
    }
}
