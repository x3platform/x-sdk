using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using System.Reflection;
using X3Platform.Velocity;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.Java
{
    public class RestController : JavaGenerator
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

        #region ����:RequestUri
        private string m_RequestUri;

        /// <summary>�����ַ</summary>
        public string RequestUri
        {
            get { return this.m_RequestUri; }
            set { this.m_RequestUri = value; }
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

        #region ����:EntityClassPackage
        private string m_EntityClassPackage;
        /// <summary>ʵ����</summary>
        public string EntityClassPackage
        {
            get { return m_EntityClassPackage; }
            set { m_EntityClassPackage = value; }
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

        #region ����:BusinessLogicInterfacePackage
        private string m_BusinessLogicInterfacePackage;

        /// <summary>ҵ���ӿ�</summary>
        public string BusinessLogicInterfacePackage
        {
            get { return this.m_BusinessLogicInterfacePackage; }
            set { this.m_BusinessLogicInterfacePackage = value; }
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
            // ���ƿռ�
            this.Package = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Package"].Value;

            // ������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // Ӧ������
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            // �����ַ
            this.RequestUri = configuration.Tasks[taskName].Properties["RequestUri"].Value;

            // ʵ���������
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            
            // ҵ���ӿڰ�����
            this.BusinessLogicInterfacePackage = configuration.Tasks[taskName].Properties["BusinessLogicInterfacePackage"].Value;

            // ҵ���ӿ�
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // ҵ������ʵ��
            this.BusinessLogicServiceInstance = configuration.Tasks[taskName].Properties["BusinessLogicServiceInstance"].Value;

            //���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".java"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".java";
            }
        }
        #endregion

        #region ����:PrintCode()
        /// <summary>�������</summary>
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            context.Put("author", this.Author);
            context.Put("package", this.Package);
            context.Put("className", this.ClassName);
            context.Put("applicationName", this.ApplicationName);
            context.Put("requestUri", this.RequestUri);
            context.Put("entityClassPackage", this.EntityClassPackage);
            context.Put("entityClass", this.EntityClass);
            context.Put("businessLogicInterfacePackage", this.BusinessLogicInterfacePackage);
            context.Put("businessLogicInterface", this.BusinessLogicInterface);
            context.Put("businessLogicServiceInstance", this.BusinessLogicServiceInstance);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/RestController.vm"));
        }
        #endregion
    }
}
