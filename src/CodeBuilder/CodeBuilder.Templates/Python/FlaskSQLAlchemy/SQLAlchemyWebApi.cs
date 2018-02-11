using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using X3Platform.CodeBuilder.Configuration;
using X3Platform.CodeBuilder.Data;
using X3Platform.Velocity;

namespace X3Platform.CodeBuilder.Templates.Python.FlaskSQLAlchemy
{
    public class SQLAlchemyWebApi : PythonGenerator
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

        #region ����:ServiceModuleName
        private string m_ServiceModuleName;
        /// <summary>
        /// ʵ����������ģ������
        /// </summary>
        public string ServiceModuleName
        {
            get { return m_ServiceModuleName; }
            set { m_ServiceModuleName = value; }
        }
        #endregion

        #region ����:ServiceClass
        private string m_ServiceClass;
        /// <summary>
        /// ʵ����
        /// </summary>
        public string ServiceClass
        {
            get { return m_ServiceClass; }
            set { m_ServiceClass = value; }
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
            this.ServiceModuleName = configuration.Tasks[taskName].Properties["ServiceModuleName"].Value;

            // ʵ��������
            this.ServiceClass = configuration.Tasks[taskName].Properties["ServiceClass"].Value;

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
            context.Put("serviceModuleName", this.ServiceModuleName);
            context.Put("serviceClass", this.ServiceClass);
            context.Put("fields", this.fields);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Python/FlaskSQLAlchemy/SQLAlchemyWebApi.vm"));
        }
        #endregion
    }
}
