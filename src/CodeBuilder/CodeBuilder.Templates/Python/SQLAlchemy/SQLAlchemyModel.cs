using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using X3Platform.CodeBuilder.Configuration;
using X3Platform.CodeBuilder.Data;
using X3Platform.Velocity;

namespace X3Platform.CodeBuilder.Templates.Python.SQLAlchemy
{
    public class SQLAlchemyModel : PythonGenerator
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

        #region 属性:DataTableName
        private string m_DataTableName;
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string DataTableName
        {
            get { return m_DataTableName; }
            set { m_DataTableName = value; }
        }
        #endregion
        
        /// <summary>列信息</summary>
        protected IList<PythonField> fields;

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
            // 数据表名称
            this.DataTableName = configuration.Tasks[taskName].Properties["DataTable"].Value;

            // 设置 table 信息
            IDbSchemaProvider provider = (IDbSchemaProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            DatabaseSchema database = new DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            var table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[taskName].Properties["DataTable"].Value);

            this.fields = GetFields(table);

            // 设置 输出文件
            if (configuration.Tasks[taskName].Properties["File"] == null)
            {
                configuration.Tasks[taskName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[taskName].Properties["ClassName"].Value + ".py"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[taskName].Properties["File"].Value))
            {
                configuration.Tasks[taskName].Properties["File"].Value = configuration.Tasks[taskName].Properties["ClassName"].Value + ".py";
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
            context.Put("dataTableName", this.DataTableName);
            context.Put("fields", this.fields);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context,
                X3Platform.Util.StringHelper.NullOrEmptyTo(TemplateFile, "templates/Python/SQLAlchemy/SQLAlchemyModel.vm"));
        }
        #endregion
    }
}
