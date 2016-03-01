using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Template.Web
{
    public abstract class WebGenerator : TemplateGenerator
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
        /// Ŀ¼
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
        public WebGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateHTML);
        }

        /// <summary>
        /// ���ɴ���
        /// </summary>
        public abstract void GenerateHTML();

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
