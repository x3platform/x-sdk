using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Template.Web
{
    public abstract class WebGenerator : TemplateGenerator
    {
        #region 属性:FileName
        private string m_FileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        #endregion

        #region 属性:Directory
        private string m_Directory;
        /// <summary>
        /// 目录
        /// </summary>
        public string Directory
        {
            get { return m_Directory; }
            set { m_Directory = value; }
        }
        #endregion

        protected StringBuilder buffer = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public WebGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateHTML);
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        public abstract void GenerateHTML();

        #region 函数:PrintCopyright()
        /// <summary>
        /// 生成描述信息
        /// </summary>
        /// <returns>描述信息</returns>
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
