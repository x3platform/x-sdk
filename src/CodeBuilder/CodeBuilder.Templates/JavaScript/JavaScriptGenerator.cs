namespace X3Platform.CodeBuilder.Templates.JavaScript
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Template;
    using X3Platform.CodeBuilder.Util;
    using X3Platform.Velocity;

    public abstract class JavaScriptGenerator : TemplateGenerator
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
        public JavaScriptGenerator()
        {
            this.Generate += new GenerateHandler(this.GenerateCode);
        }

        #region ����:GenerateCode()
        /// <summary>���ɴ���</summary>
        public virtual void GenerateCode()
        {
            buffer.Append(this.PrintCopyright());

            buffer.Append(this.PrintCode());

            Notify(buffer.ToString());
        }
        #endregion

        #region ����:PrintCode()
        /// <summary>�������</summary>
        public abstract string PrintCode();
        #endregion

        #region ����:PrintCopyright()
        /// <summary>�����Ȩ��Ϣ</summary>
        /// <returns>��Ȩ��Ϣ</returns>
        protected virtual string PrintCopyright()
        {
            VelocityContext context = new VelocityContext();

            context.Put("year", DateTime.Now.Year);
            context.Put("author", this.Author);
            context.Put("fileName", this.FileName);
            context.Put("description", this.Description);
            context.Put("date", this.Date);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context, "templates/JavaScript/Copyright.vm");
        }
        #endregion
        
        #region ����:GetFields(DataTableSchema table)
        public IList<JavaScriptField> GetFields(DataTableSchema table)
        {
            IList<JavaScriptField> list = new List<JavaScriptField>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                list.Add(new JavaScriptField
                {
                    Name = X3Platform.Util.StringHelper.ToFirstLower(FieldHelper.FormatName(table.Columns[i].Name)),
                    DataColumnName = table.Columns[i].Name,
                    Type = ConvertType(table.Columns[i].Type),
                    JdbcType = ConvertJdbcType(table.Columns[i].Type),
                    Length = table.Columns[i].Length,
                    DefaultValue = GetDefaultValue(table.Columns[i].Type),
                    Nullable = table.Columns[i].Nullable,
                    Description = table.Columns[i].Description
                });
            }

            return list;
        }
        #endregion

        #region ����:ConvertType(DbType type)
        /// <summary>
        /// �����ݿ�� DbType ��������ת���� .Net Framework ����������
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns>System.Data.DbType ����</returns>
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

        #region ����:ConvertJdbcType(DbType type)
        /// <summary>
        /// �����ݿ�� DbType ��������ת���� .Net Framework ����������
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns>System.Data.DbType ����</returns>
        protected virtual string ConvertJdbcType(DbType type)
        {
            switch (type)
            {
                case DbType.AnsiString: return "VARCHAR";
                case DbType.AnsiStringFixedLength: return "VARCHAR";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool";
                case DbType.Byte: return "int";
                case DbType.Currency: return "decimal";
                case DbType.Date: return "TIMESTAMP";
                case DbType.DateTime: return "TIMESTAMP";
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "short";
                case DbType.Int32: return "INTEGER";
                case DbType.Int64: return "long";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte";
                case DbType.Single: return "float";
                case DbType.String: return "VARCHAR";
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

        #region ����:GetDefaultValue(DbType type)
        /// <summary>��ȡ���͵�Ĭ��ֵ</summary>
        /// <param name="type">DbType ����</param>
        /// <returns>Ĭ��ֵ</returns>
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