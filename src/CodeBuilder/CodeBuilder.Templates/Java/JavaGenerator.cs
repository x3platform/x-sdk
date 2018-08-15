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
    /// <summary>Java ����������</summary>
    public abstract class JavaGenerator : TemplateGenerator
    {
        #region ����:FileName
        private string m_FileName;
        /// <summary>�ļ�����</summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        #endregion

        #region ����:Package
        private string m_Package;
        /// <summary>
        /// ��
        /// </summary>
        public string Package
        {
            get { return m_Package; }
            set { m_Package = value; }
        }
        #endregion

        protected StringBuilder buffer = new StringBuilder();

        /// <summary>���캯��</summary>
        public JavaGenerator()
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

            return VelocityManager.Instance.ParseTemplateVirtualPath(context, "templates/Java/Copyright.vm");
        }
        #endregion

        #region ����:GetFields(DataTableSchema table)
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
