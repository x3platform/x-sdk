namespace X3Platform.CodeBuilder.Templates.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Reflection;

    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.CodeBuilder.Templates.CSharp;
    using X3Platform.Velocity;
    using X3Platform.Util;

    /// <summary>数据字典</summary>
    public class DataDictionary : SqlScriptGenerator
    {
        /// <summary>表信息</summary>
        protected List<DataTableSchema> tables;

        /// <summary>列信息</summary>
        protected IList<CSharpField> fields;

        public override void Init(string taskName, CodeBuilderConfiguration configuration)
        {
            provider = (X3Platform.CodeBuilder.Data.IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            database = new X3Platform.CodeBuilder.Data.DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            string dataTables = configuration.Tasks[taskName].Properties["DataTables"].Value;

            string[] tableNames = dataTables.Split(',');

            this.tables = new List<DataTableSchema>();

            // List<DataTableSchema>
            foreach (string tableName in tableNames)
            {
                DataTableSchema table = provider.GetTable(database.Name, database.OwnerName, tableName);

                this.tables.Add(table);
            }

            // 补全
            for (int i = 0; i < tables.Count; i++)
            {
                DataTableSchema table = this.tables[i];

                Dictionary<string, int> dict = GetDisplayLength(table);

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    DataColumnSchema column = table.Columns[j];

                    column.Name = Padding(column.Name, dict["Name"]);
                    column.NativeType = Padding(column.NativeType, dict["NativeType"]);
                    column.Description = Padding(column.Description, dict["Description"]);
                }
            }

            //设置 输出文件
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new TaskProperty("File", taskName + ".md"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = taskName + ".md";
            }
        }

        #region 函数:GenerateScript()
        public override void GenerateScript()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintScript());

            Notify(buffer.ToString());
        }
        #endregion

        #region 函数:PrintScript()
        public override string PrintScript()
        {
            VelocityContext context = new VelocityContext();

            context.Put("tables", this.tables);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                        StringHelper.NullOrEmptyTo(TemplateFile, "templates/Sql/DataDictionary.vm"));
        }
        #endregion
        /// <summary>  
        /// 计算文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="Text">需计算长度的字符串</param>
        /// <returns>int</returns>
        public int GetTextLength(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }

        private Dictionary<string, int> GetDisplayLength(DataTableSchema table)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>() { { "Name", 0 }, { "NativeType", 0 }, { "Description", 0 } };

            int maxlength = 0;

            for (int j = 0; j < table.Columns.Count; j++)
            {
                DataColumnSchema column = table.Columns[j];

                maxlength = GetTextLength(column.Name);

                if (dict["Name"] < maxlength)
                {
                    dict["Name"] = maxlength;
                }

                maxlength = GetTextLength(column.NativeType);

                if (dict["NativeType"] < maxlength)
                {
                    dict["NativeType"] = maxlength;
                }

                maxlength = GetTextLength(column.Description);

                if (dict["Description"] < maxlength)
                {
                    dict["Description"] = maxlength;
                }
            }

            return dict;
        }

        private string Padding(string text, int length)
        {
            int paddingLength = length - GetTextLength(text);

            for (int i = 0; i < paddingLength; i++)
            {
                text = text + " ";
            }

            return text;
        }
    }
}