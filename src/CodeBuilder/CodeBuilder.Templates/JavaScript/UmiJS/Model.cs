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
        #region ����:ClassName
        private string m_ClassName;
        /// <summary>
        /// ʵ����
        /// </summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region ����:Namespace
        private string m_Namespace;
        /// <summary>
        /// ���ƿռ�
        /// </summary>
        public string Namespace
        {
            get { return m_Namespace; }
            set { m_Namespace = value; }
        }
        #endregion

        #region ����:ServiceName
        private string m_ServiceName;
        /// <summary>
        /// ��������
        /// </summary>
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; }
        }
        #endregion

        #region ����:LocationPath
        private string m_LocationPath;
        /// <summary>
        /// ·��·��
        /// </summary>
        public string LocationPath
        {
            get { return m_LocationPath; }
            set { m_LocationPath = value; }
        }
        #endregion

        #region ����:Init(string taskName, CodeBuilderConfiguration configuration)
        /// <summary>����������Ϣ��ʼ������</summary>
        /// <param name="generatorName"></param>
        /// <param name="configuration"></param>
        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            // ģ���ļ�λ��
            this.TemplateFile = configuration.Tasks[taskName].Properties["TemplateFile"] == null ? null : configuration.Tasks[taskName].Properties["TemplateFile"].Value;

            // ʵ��������
            this.ClassName = configuration.Tasks[taskName].Properties["ClassName"].Value;

            // ʵ��������
            this.Namespace = configuration.Tasks[taskName].Properties["Namespace"].Value;

            // ʵ��������
            this.ServiceName = configuration.Tasks[taskName].Properties["ServiceName"].Value;

            // ����·��
            this.LocationPath = configuration.Tasks[taskName].Properties["LocationPath"].Value;

            // ���� ����ļ�
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

        #region ����:GenerateCode()
        public override void GenerateCode()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintCode());

            Notify(buffer.ToString());
        }
        #endregion

        #region ����:PrintCode()
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
