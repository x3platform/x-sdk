namespace X3Platform.CodeBuilder.Templates.Java
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;

    public class BusinessLogicService : JavaGenerator
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

        #region ����:InterfaceName
        private string m_InterfaceName;

        /// <summary>�ӿڵ�����</summary>
        public string InterfaceName
        {
            get { return m_InterfaceName; }
            set { m_InterfaceName = value; }
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

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ����
            this.Author = configuration.Author;

            // ���ƿռ�
            this.Package = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Package"].Value;

            // ������
            this.InterfaceName = configuration.Tasks[taskName].Properties["InterfaceName"].Value;

            // ʵ��������
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            //���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["InterfaceName"].Value + ".java"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["InterfaceName"].Value + ".java";
            }
        }
        #endregion

        #region ����:PrintCode()
        /// <summary>�������</summary>
        /// <returns></returns>
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("author", this.Author);
            context.Put("package", this.Package);
            context.Put("interfaceName", this.InterfaceName);
            context.Put("entityClassPackage", this.EntityClassPackage);
            context.Put("entityClass", this.EntityClass);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/BusinessLogicService.vm"));
        }
        #endregion
    }
}
