namespace X3Platform.CodeBuilder.Templates.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;

    public class CustomPluginContext : CSharpGenerator
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

        #region 属性:ApplicationName
        private string m_ApplicationName;

        /// <summary>应用名称</summary>
        public string ApplicationName
        {
            get { return this.m_ApplicationName; }
            set { this.m_ApplicationName = value; }
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

            // 应用名称
            this.ApplicationName = configuration.Tasks[taskName].Properties["ApplicationName"].Value;

            //设置 输出文件
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

        #region 函数:PrintCode()
        /// <summary>输出代码</summary>
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
