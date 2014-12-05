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
        #region 属性:NamespacePrefix
        private string m_NamespacePrefix;

        /// <summary>名称空间前缀</summary>
        public string NamespacePrefix
        {
            get { return this.m_NamespacePrefix; }
            set { this.m_NamespacePrefix = value; }
        }
        #endregion

        #region 属性:InterfaceName
        private string m_InterfaceName;
        /// <summary>
        /// 接口的名称
        /// </summary>
        public string InterfaceName
        {
            get { return m_InterfaceName; }
            set { m_InterfaceName = value; }
        }
        #endregion

        #region 属性:EntityClass
        private string m_EntityClass;
        /// <summary>
        /// 实体类
        /// </summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region 函数:Init(string taskName, CodeBuilderConfiguration configuration)
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // 名称空间前缀
            this.NamespacePrefix = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["NamespacePrefix"].Value;

            // 名称空间
            this.Namespace = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Namespace"].Value;

            // 接口名称
            this.InterfaceName = configuration.Tasks[taskName].Properties["InterfaceName"].Value;

            // 实体类名称
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // 设置 输出文件
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
