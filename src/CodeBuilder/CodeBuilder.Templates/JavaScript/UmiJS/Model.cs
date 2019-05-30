using System;
using System.Collections.Generic;
using System.Text;
using X3Platform.CodeBuilder.Configuration;
using System.Reflection;
using X3Platform.Velocity;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.JavaScript.UmiJS
{
    public class Model : JavaScriptGenerator
    {
        #region 属性:ClassName
        private string m_ClassName;
        /// <summary>
        /// 实体类
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region 属性:Namespace
        private string m_Namespace;
        /// <summary>
        /// 名称空间
        /// </summary>
        public string Namespace
        {
            get { return m_Namespace; }
            set { m_Namespace = value; }
        }
        #endregion

        #region 属性:ServiceName
        private string m_ServiceName;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; }
        }
        #endregion

        #region 属性:LocationPath
        private string m_LocationPath;
        /// <summary>
        /// 路由路径
        /// </summary>
        public string LocationPath
        {
            get { return m_LocationPath; }
            set { m_LocationPath = value; }
        }
        #endregion

        #region 函数:Init(string taskName, CodeBuilderConfiguration configuration)
        /// <summary>根据配置信息初始化对象</summary>
        /// <param name="generatorName"></param>
        /// <param name="configuration"></param>
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // 模板文件位置
            this.TemplateFile = configuration.Tasks[taskName].Properties["TemplateFile"] == null ? null : configuration.Tasks[taskName].Properties["TemplateFile"].Value;

            // 实体类名称
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // 实体类名称
            this.Namespace = configuration.Tasks[taskName].Properties["Namespace"].Value;

            // 实体类名称
            this.ServiceName = configuration.Tasks[taskName].Properties["ServiceName"].Value;

            // 请求路径
            this.LocationPath = configuration.Tasks[taskName].Properties["LocationPath"].Value;

            // 设置 输出文件
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".js"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".js";
            }
        }
        #endregion

        #region 函数:GenerateCode()
        public override void GenerateCode()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintCode());

            Notify(buffer.ToString());
        }
        #endregion

        #region 函数:PrintCode()
        public override string PrintCode()
        {
            VelocityContext context = new VelocityContext();

            context.Put("className", this.ClassName);
            context.Put("namespace", this.Namespace);
            context.Put("serviceName", this.ServiceName);
            context.Put("locationPath", this.LocationPath);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/JavaScript/UmiJS/Model.vm"));
        }
        #endregion
    }
}
