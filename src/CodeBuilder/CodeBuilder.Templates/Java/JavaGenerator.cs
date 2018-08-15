using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using X3Platform.CodeBuilder.Template;
using X3Platform.CodeBuilder.Data;
using X3Platform.CodeBuilder.Util;
using X3Platform.Velocity;

namespace X3Platform.CodeBuilder.Templates.Java
{
    /// <summary>Java 代码生成器</summary>
    public abstract class JavaGenerator : TemplateGenerator
    {
        #region 属性:FileName
        private string m_FileName;
        /// <summary>文件名称</summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        #endregion

        #region 属性:Package
        private string m_Package;
        /// <summary>
        /// 包
        /// </summary>
        public string Package
        {
            get { return m_Package; }
            set { m_Package = value; }
        }
        #endregion

        protected StringBuilder buffer = new StringBuilder();

        /// <summary>构造函数</summary>
        public JavaGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateCode);
        }

        #region 函数:GenerateCode()
        /// <summary>生成代码</summary>
        public virtual void GenerateCode()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintCode());

            Notify(buffer.ToString());
        }
        #endregion

        #region 函数:PrintCode()
        /// <summary>输出代码</summary>
        public abstract string PrintCode();
        #endregion

        #region 函数:PrintCopyright()
        /// <summary>输出版权信息</summary>
        /// <returns>版权信息</returns>
        protected virtual string PrintCopyright()
        {
            VelocityContext context = new VelocityContext();

            context.Put("year", DateTime.Now.Year);
            context.Put("author", this.Author);
            context.Put("fileName", this.FileName);
            context.Put("description", this.Description);
            context.Put("date", this.Date);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context, "templates/Java/Copyright.vm");
        }
        #endregion

        #region 函数:GetFields(DataTableSchema table)
        public IList<JavaField> GetFields(DataTableSchema table)
        {
            IList<JavaField> list = new List<JavaField>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                list.Add(new JavaField
                {
                    Name = X3Platform.Util.StringHelper.ToFirstLower(FieldHelper.FormatName(table.Columns[i].Name)),
                    NameFirstUpperCase = FieldHelper.FormatName(table.Columns[i].Name),
                    DataColumnName = table.Columns[i].Name,
                    Type = ConvertType(table.Columns[i].Type),
                    Length = table.Columns[i].Length,
                    DefaultValue = GetDefaultValue(table.Columns[i].Type),
                    Nullable = table.Columns[i].Nullable,
                    Description = table.Columns[i].Description
                });
            }

            return list;
        }
        #endregion

        #region 函数:ConvertType(DbType type)
        /// <summary>
        /// 将数据库的 DbType 数据类型转换到 .Net Framework 的数据类型
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>System.Data.DbType 类型</returns>
        protected virtual string ConvertType(DbType type)
        {
            switch (type)
            {
                case DbType.AnsiString: return "String";
                case DbType.AnsiStringFixedLength: return "String";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool";
                case DbType.Byte: return "int";
                case DbType.Currency: return "decimal";
                case DbType.Date: return "Date";
                case DbType.DateTime: return "Date";
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "short";
                case DbType.Int32: return "int";
                case DbType.Int64: return "long";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte";
                case DbType.Single: return "float";
                case DbType.String: return "String";
                case DbType.StringFixedLength: return "String";
                case DbType.Time: return "TimeSpan";
                case DbType.UInt16: return "ushort";
                case DbType.UInt32: return "uint";
                case DbType.UInt64: return "ulong";
                case DbType.VarNumeric: return "decimal";
                default: return "UnknownType";
            }
        }
        #endregion

        #region 函数:GetDefaultValue(DbType type)
        /// <summary>获取类型的默认值</summary>
        /// <param name="type">DbType 类型</param>
        /// <returns>默认值</returns>
        protected virtual string GetDefaultValue(DbType type)
        {
            switch (type)
            {
                case DbType.Byte:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric: return "0";

                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength: return "\"\"";

                case DbType.Date:
                case DbType.DateTime:
                case DbType.Time: return "DateUtil.getDefaultDate()";

                case DbType.Guid: return "UUIDUtil.empty()";

                case DbType.Object:
                case DbType.Binary: return "null";

                case DbType.Boolean: return "false";

                default: return "UnknownType";
            }
        }
        #endregion
    }
}
