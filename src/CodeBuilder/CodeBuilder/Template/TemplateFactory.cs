namespace X3Platform.CodeBuilder.Template
{
    using System;
    using System.Reflection;
    using X3Platform.CodeBuilder.Configuration;

    public class TemplateFactory
    {
        private TemplateFactory() { }

        #region ����:CreateObject(string className)
        /// <summary>����ģ������������ <summary>
        public static TemplateGenerator CreateObject(string taskName, string className, CodeBuilderConfiguration configuration)
        {
            TemplateGenerator template = (TemplateGenerator)KernelContext.CreateObject(className);

            if (template == null) throw new Exception("����[" + className + "]���Ͷ���ʧ��,��ȷ�ϴ������Ƿ����.");

            template.Init(taskName, configuration);

            return template;
        }
        #endregion
    }
}
