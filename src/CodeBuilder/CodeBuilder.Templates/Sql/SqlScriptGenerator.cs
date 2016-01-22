namespace X3Platform.CodeBuilder.Templates.Sql
{
    using System;
    using System.Data;
    using System.Text;
    using System.Reflection;

    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Template;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;
    using X3Platform.CodeBuilder.Templates.CSharp;
    using System.Collections.Generic;
    using X3Platform.CodeBuilder.Util;

    /// <summary>SQL �ű�������</summary>
    public abstract class SqlScriptGenerator : TemplateGenerator
    {
        #region ����:FileName
        private string m_FileName;
        /// <summary>�ļ�����</summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        #endregion

        protected IDbSchemaProvider provider;

        protected DatabaseSchema database;

        private string ownerName = string.Empty;

        protected StringBuilder buffer = new StringBuilder();

        public SqlScriptGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateScript);
        }

        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            provider = (X3Platform.CodeBuilder.Data.IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            database = new X3Platform.CodeBuilder.Data.DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            // table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[taskName].Properties["DataTable"].Value);

            // ���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["DataTable"].Value + ".StorageProcedure.sql"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["DataTable"].Value + ".StorageProcedure.sql";
            }
        }

        #region ����:GenerateScript()
        /// <summary>���ɽű�</summary>
        public virtual void GenerateScript()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintScript());

            Notify(buffer.ToString());
        }
        #endregion

        #region ����:PrintCopyright()
        /// <summary>�����Ȩ��Ϣ</summary>
        /// <returns>��Ȩ��Ϣ</returns>
        protected virtual string PrintCopyright()
        {
            VelocityContext context = new VelocityContext();

            context.Put("year", DateTime.Now.Year);
            context.Put("author", this.Author);
            context.Put("fileName", this.FileName);
            context.Put("description", this.Description);
            context.Put("date", this.Date);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context, "templates/CSharp/Copyright.vm");
        }
        #endregion

        #region ����:PrintScript()
        /// <summary>����ű�</summary>
        public abstract string PrintScript();
        #endregion
        
        #region ����:GetFields(DataTableSchema table)
        public IList<CSharpField> GetFields(DataTableSchema table)
        {
            IList<CSharpField> list = new List<CSharpField>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                list.Add(new CSharpField
                {
                    Name = FieldHelper.FormatName(table.Columns[i].Name),
                    // Type = ConvertType(table.Columns[i].Type),
                    // DefaultValue = GetDefaultValue(table.Columns[i].Type),
                });
            }

            return list;
        }
        #endregion
    }
}
