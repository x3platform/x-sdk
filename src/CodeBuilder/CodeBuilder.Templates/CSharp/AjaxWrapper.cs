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
        #region 属性:NamespacePrefix
        private string m_NamespacePrefix;

        /// <summary>名称空间前缀</summary>
        public string NamespacePrefix
        {
            get { return this.m_NamespacePrefix; }
            set { this.m_NamespacePrefix = value; }
        }
        #endregion

        #region 属性:ClassName
        private string m_ClassName;

        /// <summary>类的名称</summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region 属性:EntityClass
        private string m_EntityClass;
        /// <summary>实体类</summary>
        public string EntityClass
        {
            get { return m_EntityClass; }
            set { m_EntityClass = value; }
        }
        #endregion

        #region 属性:ApplicationName
        private string m_ApplicationName;

        /// <summary>应用名称</summary>
        public string ApplicationName
        {
            get { return this.m_ApplicationName; }
            set { this.m_ApplicationName = value; }
        }
        #endregion

        #region 属性:BusinessLogicInterface
        private string m_BusinessLogicInterface;

        /// <summary>业务层接口</summary>
        public string BusinessLogicInterface
        {
            get { return this.m_BusinessLogicInterface; }
            set { this.m_BusinessLogicInterface = value; }
        }
        #endregion

        #region 属性:BusinessLogicServiceInstance
        private string m_BusinessLogicServiceInstance;

        /// <summary>业务层服务实例</summary>
        public string BusinessLogicServiceInstance
        {
            get { return this.m_BusinessLogicServiceInstance; }
            set { this.m_BusinessLogicServiceInstance = value; }
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

            // 类名称
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // 实体类名称
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;

            // 应用名称
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            // 业务层接口
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // 业务层服务实例
            this.BusinessLogicServiceInstance = configuration.Tasks[taskName].Properties["BusinessLogicServiceInstance"].Value;

            //设置 输出文件
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

        #region 函数:PrintCode()
        /// <summary>输出代码</summary>
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
