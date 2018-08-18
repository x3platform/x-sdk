namespace X3Platform.CodeBuilder.Templates.Java.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.Velocity;

    public class BusinessLogicServiceTests: JavaGenerator
    {
        #region 属性:ClassName
        private string m_ClassName;

        /// <summary>类的名称</summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion
        
        #region 属性:EntityClassPackage
        private string m_EntityClassPackage;
        /// <summary>实体类</summary>
        public string EntityClassPackage
        {
            get { return m_EntityClassPackage; }
            set { m_EntityClassPackage = value; }
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

        #region 属性:BusinessLogicInterfacePackage
        private string m_BusinessLogicInterfacePackage;

        /// <summary>业务层接口</summary>
        public string BusinessLogicInterfacePackage
        {
            get { return this.m_BusinessLogicInterfacePackage; }
            set { this.m_BusinessLogicInterfacePackage = value; }
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
            // 名称空间
            this.Package = ((string.IsNullOrEmpty(configuration.NamespaceRoot)) ? string.Empty : (configuration.NamespaceRoot + ".")) +
                configuration.Tasks[taskName].Properties["Package"].Value;

            // 类名称
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;
            
            // 实体类包名称
            this.EntityClassPackage = configuration.Tasks[taskName].Properties["EntityClassPackage"].Value;

            // 实体类名称
            this.EntityClass = configuration.Tasks[taskName].Properties["EntityClass"].Value;
            
            // 业务层接口包名称
            this.BusinessLogicInterfacePackage = configuration.Tasks[taskName].Properties["BusinessLogicInterfacePackage"].Value;

            // 业务层接口
            this.BusinessLogicInterface = configuration.Tasks[taskName].Properties["BusinessLogicInterface"].Value;

            // 业务层服务实例
            this.BusinessLogicServiceInstance = configuration.Tasks[taskName].Properties["BusinessLogicServiceInstance"].Value;

            //设置 输出文件
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

        #region 函数:PrintCode()
        /// <summary>输出代码</summary>
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("self", this);
            context.Put("package", this.Package);
            context.Put("className", this.ClassName);
            context.Put("entityClassPackage", this.EntityClassPackage);
            context.Put("entityClass", this.EntityClass);
            context.Put("businessLogicInterfacePackage", this.BusinessLogicInterfacePackage);
            context.Put("businessLogicInterface", this.BusinessLogicInterface);
            context.Put("businessLogicServiceInstance", this.BusinessLogicServiceInstance);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
               X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Java/Tests/BusinessLogicServiceTests.vm"));
        }
        #endregion
    }
}
