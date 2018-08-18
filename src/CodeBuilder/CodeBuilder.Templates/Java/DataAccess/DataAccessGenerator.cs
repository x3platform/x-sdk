namespace X3Platform.CodeBuilder.Templates.Java.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Configuration;

    /// <summary>���ݲ�</summary>
    public abstract class DataAccessGenerator : JavaGenerator
    {
        #region ����:NamespacePrefix
        private string m_NamespacePrefix;

        /// <summary>���ƿռ�ǰ׺</summary>
        public string NamespacePrefix
        {
            get { return this.m_NamespacePrefix; }
            set { this.m_NamespacePrefix = value; }
        }
        #endregion

        #region ����:ClassName
        private string m_ClassName;

        /// <summary>
        /// �������
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region ����:EntityClassPackage
        private string m_EntityClassPackage;
        /// <summary>ʵ�������ڵİ�����</summary>
        public string EntityClassPackage
        {
            get { return m_EntityClassPackage; }
            set { m_EntityClassPackage = value; }
        }
        #endregion

        #region ����:EntityClass
        private string m_EntityClass;
        /// <summary>
        /// ʵ����
        /// </summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region ����:ApplicationName
        private string m_ApplicationName;

        /// <summary>Ӧ������</summary>
        public string ApplicationName
        {
            get { return this.m_ApplicationName; }
            set { this.m_ApplicationName = value; }
        }
        #endregion

        #region ����:DataAccessInterface
        private string m_DataAccessInterface;
        /// <summary>
        /// ���ݲ�ӿ�
        /// </summary>
        public string DataAccessInterface
        {
            get { return m_DataAccessInterface; }
            set { m_DataAccessInterface = value; }
        }
        #endregion

        /// <summary>������</summary>
        protected string DataTableName;

        /// <summary>��ṹ</summary>
        protected DataTableSchema table;

        /// <summary>�ֶ���Ϣ</summary>
        protected IList<JavaField> fields;

        /// <summary>֧����Ȩ</summary>
        protected string SupportAuthorization;

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ���ƿռ�ǰ׺
            this.DataTableName = configuration.Tasks[taskName].Properties["DataTable"].Value;

            // ���ƿռ�
            this.Package = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Package"].Value;

            // ������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"] == null ? "" : configuration.Tasks[taskName].Properties["ClassName"].Value;

            // ʵ�������ڵİ�����
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"] == null ? "" : configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // Ӧ������
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"] == null ? "" : configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            // ���ݲ�ӿ�
            this.DataAccessInterface = configuration.Tasks[taskName].Properties["DataAccessInterface"] == null ? "" : configuration.Tasks[taskName].Properties["DataAccessInterface"].Value;

            // ���ݲ�ӿ�
            this.SupportAuthorization = configuration.Tasks[taskName].Properties["SupportAuthorization"] == null ? "" : configuration.Tasks[taskName].Properties["SupportAuthorization"].Value;

            //���� table ��Ϣ
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

            // ���� ����ļ�
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
