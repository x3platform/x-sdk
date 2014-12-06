namespace X3Platform.CodeBuilder.Templates.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;

    public class CustomPluginContext : CSharpGenerator
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

        /// <summary>�������</summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
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

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ���ƿռ�ǰ׺
            this.NamespacePrefix = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["NamespacePrefix"].Value;

            // ���ƿռ�
            this.Namespace = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Namespace"].Value;

            // ������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // Ӧ������
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            //���� ����ļ�
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

        #region ����:PrintCode()
        /// <summary>�������</summary>
        /// <returns></returns>
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("applicationName", this.ApplicationName);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/CustomPluginContext.vm"));
        }
        #endregion
    }
}
