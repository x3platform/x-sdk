using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using X3Platform.Velocity;
using X3Platform.Util;

namespace X3Platform.CodeBuilder.Templates.Java
{
    public class BusinessLogicServiceImpl : JavaGenerator
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

        #region ����:ConfigurationClass
        private string m_ConfigurationClass;
        
        /// <summary>������</summary>
        public string ConfigurationClass
        {
            get { return m_ConfigurationClass; }
            set { m_ConfigurationClass = value; }
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

        /// <summary>ʵ����</summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region ����:BusinessLogicInterfacePackage
        private string m_BusinessLogicInterfacePackage;

        /// <summary>ҵ���ӿ����ڵİ�����</summary>
        public string BusinessLogicInterfacePackage
        {
            get { return this.m_BusinessLogicInterfacePackage; }
            set { this.m_BusinessLogicInterfacePackage = value; }
        }
        #endregion

        #region ����:BusinessLogicInterface
        private string m_BusinessLogicInterface;
        /// <summary>
        /// ҵ���ӿ�
        /// </summary>
        public string BusinessLogicInterface
        {
            get { return m_BusinessLogicInterface; }
            set { m_BusinessLogicInterface = value; }
        }
        #endregion

        #region ����:DataAccessInterfacePackage
        private string m_DataAccessInterfacePackage;

        /// <summary>���ݲ�ӿ����ڵİ�����</summary>
        public string DataAccessInterfacePackage
        {
            get { return this.m_DataAccessInterfacePackage; }
            set { this.m_DataAccessInterfacePackage = value; }
        }
        #endregion
        
        #region ����:DataAccessInterface
        private string m_DataAccessInterface;
        /// <summary>���ݲ�ӿ� </summary>
        public string DataAccessInterface
        {
            get { return m_DataAccessInterface; }
            set { m_DataAccessInterface = value; }
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

            // ʵ�������ڵİ�����
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;
            
            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // ҵ���ӿ����ڵİ�����
            this.BusinessLogicInterfacePackage = configuration.Tasks[taskName].Properties["BusinessLogicInterfacePackage"].Value;

            // ҵ���ӿ�
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // ���ݲ�ӿ����ڵİ�����
            this.DataAccessInterfacePackage = configuration.Tasks[taskName].Properties["DataAccessInterfacePackage"].Value;

            // ���ݲ�ӿ�
            this.DataAccessInterface = configuration.Tasks[taskName].Properties["DataAccessInterface"].Value;

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
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();
            
            context.Put("package", this.Package);
            context.Put("className", this.ClassName);
            context.Put("entityClassPackage", this.EntityClassPackage);
            context.Put("entityClass", this.EntityClass);
            context.Put("businessLogicInterfacePackage", this.BusinessLogicInterfacePackage);
            context.Put("businessLogicInterface", this.BusinessLogicInterface);
            context.Put("dataAccessInterfacePackage", this.DataAccessInterfacePackage);
            context.Put("dataAccessInterface", this.DataAccessInterface);
          
            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/BusinessLogicServiceImpl.vm"));
        }
        #endregion
    }
}
