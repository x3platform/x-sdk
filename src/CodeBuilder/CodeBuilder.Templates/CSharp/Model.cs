using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using System.Reflection;
using X3Platform.Velocity;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    public class Model : CSharpGenerator
    {
        #region ����:ClassName
        private string m_ClassName;
        /// <summary>
        /// ʵ����
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        /// <summary>����Ϣ</summary>
        protected IList<CSharpField> fields;

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        /// <summary></summary>
        /// <param name="generatorName"></param>
        /// <param name="configuration"></param>
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ģ���ļ�λ��
            this.TemplateFile = configuration.Tasks[taskName].Properties["TemplateFile"] == null ? null : configuration.Tasks[taskName].Properties["TemplateFile"].Value;

            // ���ƿռ�
            this.Namespace = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Namespace"].Value;

            // ʵ��������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            //���� table ��Ϣ
            IDbSchemaProvider provider =
                (IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            X3Platform.CodeBuilder.Data.DatabaseSchema database = new X3Platform.CodeBuilder.Data.DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            var table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[taskName].Properties["DataTable"].Value);

            this.fields = GetFields(table);

            // ���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".cs"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".cs";
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

        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("fields", this.fields);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/Model.vm"));
        }
    }
}
