namespace X3Platform.CodeBuilder.Templates.ExtJS
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CodeBuilder.Template;

    public abstract class ExtJSGenerator : TemplateGenerator
    {  
        #region ����:FileName
        private string m_FileName;
        /// <summary>
        /// �ļ�����
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        #endregion

        #region ����:Directory
        private string m_Directory;
        /// <summary>
        /// ���ƿռ�
        /// </summary>
        public string Directory
        {
            get { return m_Directory; }
            set { m_Directory = value; }
        }
        #endregion

        protected StringBuilder buffer = new StringBuilder();

        /// <summary>
        /// ���캯��
        /// </summary>
        public ExtJSGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateJavascript);
        }

        /// <summary>
        /// ���ɴ���
        /// </summary>
        public abstract void GenerateJavascript();

        #region ����:PrintCopyright()
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <returns>������Ϣ</returns>
        protected virtual string PrintCopyright()
        {
            StringBuilder outString = new StringBuilder();
            
            string path = ((string)(Directory + FileName)).ToLower();

            outString.AppendLine("//========================================================");
            outString.AppendLine("//");
            outString.AppendLine("// Default Description.");
            outString.AppendLine("//");
            outString.AppendLine("//========================================================");

            return outString.ToString();
        }
        #endregion
    }
}
