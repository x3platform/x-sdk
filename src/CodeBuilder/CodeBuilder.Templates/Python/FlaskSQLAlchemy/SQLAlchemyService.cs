using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using X3Platform.CodeBuilder.Configuration;
using X3Platform.CodeBuilder.Data;
using X3Platform.Velocity;

namespace X3Platform.CodeBuilder.Templates.Python.FlaskSQLAlchemy
{
    public class SQLAlchemyService : PythonGenerator
    {
        #region ����:ClassName
        private string m_ClassName;
        /// <summary>
        /// ������
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region ����:EntityModuleName
        private string m_EntityModuleName;
        /// <summary>
        /// ʵ����������ģ������
        /// </summary>
        public string EntityModuleName
        {
            get { return m_EntityModuleName; }
            set { m_EntityModuleName = value; }
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

        /// <summary>����Ϣ</summary>
        protected IList<PythonField> fields;

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        /// <summary>����������Ϣ��ʼ������</summary>
        /// <param name="generatorName"></param>
        /// <param name="configuration"></param>
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ģ���ļ�λ��
            this.TemplateFile = configuration.Tasks[taskName].Properties["TemplateFile"] == null ? null : configuration.Tasks[taskName].Properties["TemplateFile"].Value;

            // ʵ��������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // ʵ��������ģ������
            this.EntityModuleName = configuration.Tasks[taskName].Properties["EntityModuleName"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // ���� table ��Ϣ
            IDbSchemaProvider provider = (IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            DatabaseSchema database = new DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            var table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[taskName].Properties["DataTable"].Value);

            this.fields = GetFields(table);

            // ���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".py"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".py";
            }
        }
        #endregion

        #region ����:GenerateCode()
        public override void GenerateCode()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintCode());

            Notify(buffer.ToString());
        }
        #endregion

        #region ����:PrintCode()
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("className", this.ClassName);
            context.Put("entityModuleName", this.EntityModuleName);
            context.Put("entityClass", this.EntityClass);
            
            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Python/FlaskSQLAlchemy/SQLAlchemyService.vm"));
        }
        #endregion
    }
}
