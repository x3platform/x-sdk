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
        /// <summary>数据提供器名称</summary>
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

        #region 函数:GetDatabaseName()
        /// <summary>
        /// 取得数据库名称
        /// </summary>
        /// <returns>数据库名称</returns>
        public string GetDatabaseName()
        {
            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = "select database()";

            var result = command.ExecuteScalar(commandText);

            return result == null ? string.Empty : result.ToString();
        }
        #endregion

        #region 函数:GetTables(string databaseName, string ownerName, string tableNames)
        /// <summary>查询数据库中多个表的信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableNames">表名, 多个以半角逗号隔开</param>
        /// <returns>多个数据表信息的列表</returns>
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

        #region 函数:GetTable(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// 取得数据库名称
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
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

            // 设置主键
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

            // 设置外键
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

        #region 函数:GetTableDescription(string databaseName, string ownerName, string tableName)
        /// <summary>取得数据库名称</summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
        private string GetTableDescription(string databaseName, string ownerName, string tableName)
        {
            GenericSqlCommand command = new GenericSqlCommand(connectionString, PROVIDER_NAME);

            string commandText = string.Format("SHOW TABLE STATUS FROM {0}", databaseName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                if (row["Name"].ToString() == tableName)
                {
                    // 输出注释信息
                    string comment = row["Comment"].ToString();

                    if (comment.IndexOf(';') == -1) return string.Empty;

                    return comment.Substring(0, comment.IndexOf(';'));
                }
            }

            return string.Empty;
        }
        #endregion

        #region 函数:GetColumns(string databaseName, string ownerName, string tableName)
        /// <summary>取得数据库名称</summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
        public DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "MySql");

            string commandText = string.Format("SHOW FULL FIELDS FROM {1} FROM {0}", databaseName, tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                // 名称
                item.Name = row["Field"].ToString();
                // 数据类型
                item.Type = SetDataType(row["Type"].ToString());
                // 原生数据类型
                item.NativeType = row["Type"].ToString();
                // 是否允许为空
                item.Nullable = (row["Null"].ToString() == "NO") ? false : true;
                // 默认值
                item.DefaultValue = row["Default"].ToString();
                // 注释
                item.Description = row["Comment"].ToString();

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region 函数:GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// 取得数据库名称
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
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

        #region 函数:GetForeignKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>查询数据库中表的外键字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表名</param>
        /// <returns>外键字段信息集合</returns>
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

        #region 函数:GetNoPrimaryKeyColumns(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// 取得数据库名称
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
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
                    throw new Exception("设置数据类型失败,可能是未知的新数据类型" + type + "!");
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
