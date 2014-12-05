namespace X3Platform.CodeBuilder.Template
{
    using System;
    using System.Reflection;
    using X3Platform.CodeBuilder.Configuration;

    public class TemplateFactory
    {
        private TemplateFactory() { }

        #region 函数:CreateObject(string className)
        /// <summary>创建模板生成器对象 <summary>
        public static TemplateGenerator CreateObject(string taskName, string className, CodeBuilderConfiguration configuration)
        {
            TemplateGenerator template = (TemplateGenerator)KernelContext.CreateObject(className);

            if (template == null) throw new Exception("创建[" + className + "]类型对象失败,请确认此类型是否存在.");

            template.Init(taskName, configuration);

            return template;
        }
        #endregion
    }
}
