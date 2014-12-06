using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using X3Platform.Velocity;
using X3Platform.Util;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    public class BusinessLogicService : CSharpGenerator
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

        #region ����:EntityClass
        private string m_EntityClass;

        /// <summary>ʵ����</summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
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

        #region ����:DataAccessInterface
        private string m_DataAccessInterface;
        /// <summary>
        /// ���ݲ�ӿ�
        /// </summary>
        public string DataAccessInterface
        {
            get { return m_DataAccessInterface; }
            set { m_DataAccessInterface = value; }
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

            // ������
            this.ConfigurationClass = configuration.Tasks[taskName].Properties["ConfigurationClass"].Value;
            
            // ���ݲ�ӿ�
            this.DataAccessInterface = configuration.Tasks[taskName].Properties["DataAccessInterface"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // ҵ���ӿ�
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // ���ݲ�ӿ�
            this.DataAccessInterface = configuration.Tasks[taskName].Properties["DataAccessInterface"].Value;

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
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("namespace", this.Namespace);
            context.Put("className", this.ClassName);
            context.Put("configurationClass", this.ConfigurationClass);
            context.Put("entityClass", this.EntityClass);
            context.Put("businessLogicInterface", this.BusinessLogicInterface);
            context.Put("dataAccessInterface", this.DataAccessInterface);
          
            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/BusinessLogicService.vm"));
        }
        #endregion
    }
}
