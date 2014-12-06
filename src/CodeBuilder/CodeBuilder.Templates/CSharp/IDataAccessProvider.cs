using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using X3Platform.Velocity;
using X3Platform.Util;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    public class IDataAccessProvider : CSharpGenerator
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
        /// <summary>
        /// �ӿڵ�����
        /// </summary>
        public string InterfaceName
        {
            get { return m_InterfaceName; }
            set { m_InterfaceName = value; }
        }
        #endregion

        #region ����:EntityClass
        private string m_EntityClass;
        /// <summary>
        /// ʵ����
        /// </summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
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

            // �ӿ�����
            this.InterfaceName = configuration.Tasks[taskName].Properties["InterfaceName"].Value;

            // ʵ��������
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // ���� ����ļ�
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new TaskProperty("File", configuration.Tasks[taskName].Properties["InterfaceName"].Value + ".cs"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["InterfaceName"].Value + ".cs";
            }
        }
        #endregion

        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("namespacePrefix", this.NamespacePrefix);
            context.Put("namespace", this.Namespace);
            context.Put("interfaceName", this.InterfaceName);
            context.Put("entityClass", this.EntityClass);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/CSharp/IDataAccessProvider.vm"));
        }
    }
}
