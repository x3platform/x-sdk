using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using System.Reflection;
using X3Platform.Velocity;
using X3Platform.CodeBuilder.Data;
         
namespace X3Platform.CodeBuilder.Templates.JavaScript.UmiJS
{
    public class Service : JavaScriptGenerator
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

        #region ����:MicroserviceName
        private string m_MicroserviceName;
        /// <summary>
        /// ΢��������
        /// </summary>
        public string MicroserviceName
        {
            get { return m_MicroserviceName; }
            set { m_MicroserviceName = value; }
        }
        #endregion

        #region ����:RequestPath
        private string m_RequestPath;
        /// <summary>
        /// ����·��
        /// </summary>
        public string RequestPath
        {
            get { return m_RequestPath; }
            set { m_RequestPath = value; }
        }
        #endregion

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

            // ��������
            this.MicroserviceName = configuration.Tasks[taskName].Properties["MicroserviceName"].Value;

            // ����·��
            this.RequestPath = configuration.Tasks[taskName].Properties["RequestPath"].Value;

            // ���� table ��Ϣ
            // IDbSchemaProvider provider = (IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
            //        false, BindingFlags.Default,
            //        null, new object[] { configuration.DatabaseProvider.ConnectionString },
            //        null, null);

            // DatabaseSchema database = new DatabaseSchema();

            // database.Name = provider.GetDatabaseName();
            // database.OwnerName = configuration.DatabaseProvider.OwnerName;

            // var table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[taskName].Properties["DataTable"].Value);

            // this.fields = GetFields(table);

            // ���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".js"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".js";
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
            context.Put("microserviceName", this.MicroserviceName);
            context.Put("requestPath", this.RequestPath);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/JavaScript/UmiJS/Service.vm"));
        }
        #endregion
    }
}
