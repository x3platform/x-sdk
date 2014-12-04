
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using X3Platform.Data;
using X3Platform.Util;

namespace X3Platform.CodeBuilder.Data.DbSchemaProviders
{
    public class OracleSchemaProvider : IDbSchemaProvider
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public OracleSchemaProvider() { }

        public OracleSchemaProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region 函数:GetDatabaseName()
        /// <summary>取得数据库名称</summary>
        /// <returns>数据库名称</returns>
        public string GetDatabaseName()
        {
            return string.Empty;
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

                foreach (DataColumnSchema item in list)
                {
                    table.Columns.Add(item);
                }
            }

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

            return table;
        }
        #endregion

        #region 函数:GetColumns(string databaseName, string ownerName, string tableName)
        /// <summary>
        /// 取得数据库名称
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns>数据库名称</returns>
        public DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName)
        {
            DataColumnSchemaCollection list = new DataColumnSchemaCollection();

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "OracleClient");

            string commandText = string.Format(@"
select column_id, 
       column_name, 
       data_type, 
       data_length, 
       data_precision, 
       data_scale, 
       nullable,
       data_default 
  from user_tab_columns 
 where table_name = '{0}' 
 order by column_id", tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                item.Name = row["column_name"].ToString().ToLower();

                item.Type = SetDataType(row["data_type"].ToString());

                item.Nullable = (row["nullable"].ToString() == "N") ? false : true;

                switch (item.Type)
                {
                    case DbType.String:
                        item.Length = (row["data_length"] == DBNull.Value) ? 0 : Convert.ToInt32(row["data_length"].ToString());
                        break;
                    case DbType.Decimal:
                        // item.Precision = (dr["Precision"] == DBNull.Value) ? (byte)0 : (byte)dr["Precision"];
                        // item.Scale = (dr["Scale"] == DBNull.Value) ? 0 : (int)dr["Scale"];
                        break;
                    default:
                        break;
                }

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

            GenericSqlCommand command = new GenericSqlCommand(connectionString, "OracleClient");

            string commandText = string.Format(@"
select cu.* 
  from user_cons_columns cu, user_constraints au 
 where cu.constraint_name = au.constraint_name 
   and au.constraint_type = 'P' 
   and au.table_name = '{0}' ", tableName);

            var table = command.ExecuteQueryForDataTable(commandText);

            foreach (DataRow row in table.Rows)
            {
                DataColumnSchema item = new DataColumnSchema();

                item.Name = row["column_name"].ToString();

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
            switch (type.ToLower())
            {
                case "nchar":
                case "char":
                case "nvarchar":
                case "nvarchar2":
                case "varchar":
                case "varchar2":
                case "clob": return DbType.String;

                case "date":
                case "datetime": return DbType.DateTime;

                case "bit": return DbType.Boolean;

                case "smallint": return DbType.Int16;

                case "number":
                case "int": return DbType.Int32;
                case "float": return DbType.Double;

                case "money":
                case "decimal": return DbType.Decimal;

                case "uniqueidentifier": return DbType.Guid;

                case "image":
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
