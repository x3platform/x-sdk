using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using X3Platform.CodeBuilder.Template;
using X3Platform.CodeBuilder.Data;
using X3Platform.CodeBuilder.Util;
using X3Platform.Velocity;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    /// <summary>
    /// C# ����������
    /// </summary>
    public abstract class CSharpGenerator : TemplateGenerator
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

        #region ����:Namespace
        private string m_Namespace;
        /// <summary>
        /// ���ƿռ�
        /// </summary>
        public string Namespace
        {
            get { return m_Namespace; }
            set { m_Namespace = value; }
        }
        #endregion

        protected StringBuilder buffer = new StringBuilder();

        /// <summary>���캯��</summary>
        public CSharpGenerator()
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
        /// <summary>����������Ϣ</summary>
        /// <returns>������Ϣ</returns>
        protected virtual string PrintCopyright()
        {
            VelocityContext context = new VelocityContext();

            context.Put("year", DateTime.Now.Year);
            context.Put("author", this.Author);
            context.Put("fileName", this.FileName);
            context.Put("description", this.Description);
            context.Put("date", this.Date);

            return VelocityManager.Instance.ParseTemplateVirtualPath(context, "templates/CSharp/Copyright.vm");
        }
        #endregion

        public IList<CSharpField> GetFields(DataTableSchema table)
        {
            IList<CSharpField> list = new List<CSharpField>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                list.Add(new CSharpField
                {
                    Name = FieldHelper.FormatName(table.Columns[i].Name),
                    Type = ConvertType(table.Columns[i].Type),
                    DefaultValue = GetDefaultValue(table.Columns[i].Type),
                });
            }

            return list;
        }

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
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool";
                case DbType.Byte: return "int";
                case DbType.Currency: return "decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "short";
                case DbType.Int32: return "int";
                case DbType.Int64: return "long";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte";
                case DbType.Single: return "float";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
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
        /// <summary>
        /// ���͵�Ĭ��ֵ��
        /// </summary>
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
                case DbType.StringFixedLength: return "string.Empty";

                case DbType.Date:
                case DbType.DateTime:
                case DbType.Time: return "new DateTime(1970, 1, 1)";

                case DbType.Guid: return "Guid.Empty";

                case DbType.Object:
                case DbType.Binary: return "null";

                case DbType.Boolean: return "false";

                default: return "UnknownType";
            }
        }
        #endregion 
}
}
