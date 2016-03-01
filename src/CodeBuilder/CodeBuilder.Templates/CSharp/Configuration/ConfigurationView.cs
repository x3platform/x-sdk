namespace X3Platform.CodeBuilder.Templates.CSharp.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Util;
    using X3Platform.Velocity;
    
    using X3Platform.CodeBuilder.Configuration;

    public class ConfigurationView : CSharpGenerator
    {
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

        #region ����:SectionName
        private string m_SectionName;

        /// <summary>�������</summary>
        public string SectionName
        {
            get { return m_SectionName; }
            set { m_SectionName = value; }
        }
        #endregion

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ���ƿռ�
            this.Namespace = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Namespace"].Value;

            // ������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // ������
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;
            
            // ������
            this.SectionName = configuration.Tasks[taskName].Properties["SectionName"].Value;

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

            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("applicationName", this.ApplicationName);
            context.Put("sectionName", this.SectionName);
          
            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/Configuration/ConfigurationView.vm"));
        }
        #endregion
    }
}
