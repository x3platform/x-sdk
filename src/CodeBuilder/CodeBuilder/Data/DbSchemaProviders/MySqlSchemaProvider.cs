namespace X3Platform.CodeBuilder.Data.DbSchemaProviders
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using X3Platform.Data;

    public class MySqlSchemaProvider : IDbSchemaProvider
    {
        /// <summary>�����ṩ������</summary>
        private const string PROVIDER_NAME = "MySql";

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public MySqlSchemaProvider() { }

        public MySqlSchemaProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region ����:GetDatabaseName()
        /// <summary>
        /// ȡ�����ݿ�����
        /// </summary>
        /// <returns>���ݿ�����</returns>
        public string GetDatabaseName()
        {
            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = "select database()";

            var result = command.ExecuteScalar(commandText);

            return result == null ? string.Empty : result.ToString();
        }
        #endregion

        #region ����:GetTables(string databaseName, string ownerName, string tableNames)
        /// <summary>��ѯ���ݿ��ж�������Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableNames">����, ����԰�Ƕ��Ÿ���</param>
        /// <returns>������ݱ���Ϣ���б�</returns>
        public IList<DataTableSchema> GetTables(string databaseName, string ownerName, string tableNames)
        {
            IList<DataTableSchema> tables = new List<DataTableSchema>();

            string[] list = tableNames.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string tableName in list)
            {
                DataTableSchema table = this.GetTable(databaseName, ownerName, tableName);

                if (table == null) { continue; }

                tables.Add(table);
            }

            return tables;
        }
        #endregion

        #region ����:GetTable(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// ȡ�����ݿ�����
        /// </summary>
        /// <param name="tableName">���ݱ�</param>
        /// <returns>���ݿ�����</returns>
        public DataTableSchema GetTable(string databaseName, string ownerName, string tableName)
        {
            DataTableSchema table = null;

            List<DataColumnSchema> list = null;

            list = GetColumns(databaseName, ownerName, tableName);

            if (list.Count > 0)
            {
                table = new DataTableSchema(tableName);

                table.Name = tableName;
                table.Description = GetTableDescription(databaseName, ownerName, tableName);

                foreach (DataColumnSchema item in list)
                {
                    table.Columns.Add(item);
                }
            }

            // ��������
            list = GetPrimaryKeyColumns(databaseName, ownerName, tableName);

            if (list.Count > 0)
            {
                foreach (DataColumnSchema item in list)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (table.Columns[i].Name == item.Name)
                        {
                            table.Columns[i].PrimaryKey = true;
                            break;
                        }
                    }
                }
            }

            // �������
            list = GetForeignKeyColumns(databaseName, ownerName, tableName);

            if (list.Count > 0)
            {
                foreach (DataColumnSchema item in list)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (table.Columns[i].Name == item.Name)
                        {
                            table.Columns[i].ForeignKey = true;
                            break;
                        }
                    }
                }
            }

            return table;
        }
        #endregion

        #region ����:GetTableDescription(string databaseName, string ownerName, string tableName)
        /// <summary>ȡ�����ݿ�����</summary>
        /// <param name="tableName">���ݱ�</param>
        /// <returns>���ݿ�����</returns>
        private string GetTableDescription(string databaseName, string ownerName, string tableName)
        {
            GenericSqlCommand command = new GenericSqlCommand(connectionString, PROVIDER_NAME);

            string commandText = string.Format("SHOW TABLE STATUS FROM {0}", databaseName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                if (row["Name"].ToString() == tableName)
                {
                    // ���ע����Ϣ
                    string comment = row["Comment"].ToString();

                    if (comment.IndexOf(';') == -1) return string.Empty;

                    return comment.Substring(0, comment.IndexOf(';'));
                }
            }

            return string.Empty;
        }
        #endregion

        #region ����:GetColumns(string databaseName, string ownerName, string tableName)
        /// <summary>ȡ�����ݿ�����</summary>
        /// <param name="tableName">���ݱ�</param>
        /// <returns>���ݿ�����</returns>
        public DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = string.Format("SHOW FULL FIELDS FROM {1} FROM {0}", databaseName, tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                // ����
                item.Name = row["Field"].ToString();
                // ��������
                item.Type = SetDataType(row["Type"].ToString());
                // ԭ����������
                item.NativeType = row["Type"].ToString();
                // �Ƿ�����Ϊ��
                item.Nullable = (row["Null"].ToString() == "NO") ? false : true;
                // Ĭ��ֵ
                item.DefaultValue = row["Default"].ToString();
                // ע��
                item.Description = row["Comment"].ToString();

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region ����:GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// ȡ�����ݿ�����
        /// </summary>
        /// <param name="tableName">���ݱ�</param>
        /// <returns>���ݿ�����</returns>
        public DataColumnSchemaCollection GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = string.Format("SHOW FULL FIELDS FROM {1} FROM {0} WHERE `Key`='PRI'", databaseName, tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                item.Name = row["Field"].ToString();

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region ����:GetForeignKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>��ѯ���ݿ��б������ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">����</param>
        /// <returns>����ֶ���Ϣ����</returns>
        public DataColumnSchemaCollection GetForeignKeyColumns(string databaseName, string ownerName, string tableName)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = string.Format("SHOW FULL FIELDS FROM {1} FROM {0} WHERE `Key`='MUL'", databaseName, tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                item.Name = row["Field"].ToString();

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region ����:GetNoPrimaryKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// ȡ�����ݿ�����
        /// </summary>
        /// <param name="tableName">���ݱ�</param>
        /// <returns>���ݿ�����</returns>
        public DataColumnSchemaCollection GetNoPrimaryKeyColumns(DataTableSchema table)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (!table.Columns[i].PrimaryKey)
                {
                    list.Add(table.Columns[i]);
                }
            }

            return list;
        }
        #endregion

        public DbType SetDataType(string type)
        {
            string actualType = type.ToLower();

            if (actualType.IndexOf('(') > -1)
            {
                actualType = actualType.Substring(0, actualType.IndexOf('('));
            }

            switch (actualType)
            {
                case "nchar":
                case "char":
                case "nvarchar":
                case "varchar":
                case "ntext":
                case "text": return DbType.String;

                case "datetime": return DbType.DateTime;
                case "bit": return DbType.Boolean;

                case "smallint": return DbType.Int16;

                case "int": return DbType.Int32;
                case "float": return DbType.Double;

                case "money":
                case "decimal": return DbType.Decimal;

                case "uniqueidentifier": return DbType.Guid;

                case "blob": return DbType.Binary;

                default:
                    throw new Exception("������������ʧ��,������δ֪������������" + type + "!");
            }
        }

        public string GetDataType(Type type)
        {
            switch (type.ToString())
            {
                case "System.Int32": return GetDataType(DbType.Int32);
                case "System.Boolean": return GetDataType(DbType.Boolean);
                case "System.DateTime": return GetDataType(DbType.DateTime);
                case "System.Single": return GetDataType(DbType.Single);
                case "System.Decimal": return GetDataType(DbType.Decimal);

                case "System.String": return GetDataType(DbType.String);
                //case DbType.AnsiString: return "string";
                //case DbType.AnsiStringFixedLength: return "string";
                //case DbType.Binary: return "byte[]";
                //case DbType.Boolean: return "bool";
                //case DbType.Byte: return "int";
                //case DbType.Currency: return "decimal";
                //case DbType.Date: return "DateTime";
                //case DbType.DateTime: return "DateTime";
                //case DbType.Decimal: return "decimal";
                //case DbType.Double: return "double";
                //case DbType.Guid: return "Guid";
                //case DbType.Int16: return "short";
                //case DbType.Int32: return "int";
                //case DbType.Int64: return "long";
                //case DbType.Object: return "object";
                //case DbType.SByte: return "sbyte";
                //case DbType.Single: return "float";
                //case DbType.String: return "string";
                //case DbType.StringFixedLength: return "string";
                //case DbType.Time: return "TimeSpan";
                //case DbType.UInt16: return "ushort";
                //case DbType.UInt32: return "uint";
                //case DbType.UInt64: return "ulong";
                //case DbType.VarNumeric: return "decimal";
                default: return "UnknownType";
            }
        }

        public string GetDataType(DbType type)
        {
            switch (type)
            {
                case DbType.String: return "nvarchar";

                case DbType.Guid: return "uniqueidentifier";

                case DbType.Binary: return "image";
                case DbType.Boolean: return "bit";
                case DbType.Byte: return "int";

                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal: return "decimal";

                case DbType.Date: return "datetime";
                case DbType.DateTime: return "datetime";
                case DbType.Double: return "float";
                case DbType.Single: return "float";

                case DbType.Int16: return "short";
                case DbType.Int32: return "int";
                case DbType.Int64: return "long";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan";
                case DbType.UInt16: return "ushort";
                case DbType.UInt32: return "uint";
                case DbType.UInt64: return "ulong";

                default: return "UnknownType";
            }
        }
    }
}
