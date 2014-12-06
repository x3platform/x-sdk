namespace X3Platform.CodeBuilder.Templates.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;
    using X3Platform.Util;

    public class AjaxWrapper : CSharpGenerator
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

        #region ����:EntityClass
        private string m_EntityClass;
        /// <summary>ʵ����</summary>
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

        #region ����:BusinessLogicInterface
        private string m_BusinessLogicInterface;

        /// <summary>ҵ���ӿ�</summary>
        public string BusinessLogicInterface
        {
            get { return this.m_BusinessLogicInterface; }
            set { this.m_BusinessLogicInterface = value; }
        }
        #endregion

        #region ����:BusinessLogicServiceInstance
        private string m_BusinessLogicServiceInstance;

        /// <summary>ҵ������ʵ��</summary>
        public string BusinessLogicServiceInstance
        {
            get { return this.m_BusinessLogicServiceInstance; }
            set { this.m_BusinessLogicServiceInstance = value; }
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

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // Ӧ������
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            // ҵ���ӿ�
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // ҵ������ʵ��
            this.BusinessLogicServiceInstance = configuration.Tasks[taskName].Properties["BusinessLogicServiceInstance"].Value;

            //���� ����ļ�
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

        #region ����:PrintCode()
        /// <summary>�������</summary>
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("entityClass", this.EntityClass);
            context.Put("applicationName", this.BusinessLogicInterface);
            context.Put("businessLogicInterface", this.BusinessLogicInterface);
            context.Put("businessLogicServiceInstance", this.BusinessLogicServiceInstance);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/AjaxWrapper.vm"));
        }
        #endregion
    }
}
