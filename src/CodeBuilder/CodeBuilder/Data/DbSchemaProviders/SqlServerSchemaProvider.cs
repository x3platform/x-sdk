
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace X3Platform.CodeBuilder.Data.DbSchemaProviders
{
    public class SqlServerSchemaProvider : IDbSchemaProvider
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public SqlServerSchemaProvider() { }

        public SqlServerSchemaProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region 函数:GetDatabaseName()
        /// <summary>取得数据库名称</summary>
        /// <returns>数据库名称</returns>
        public string GetDatabaseName()
        {
            string databaseName = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = "SELECT db_name();";

                conn.Open();

                databaseName = (string)cmd.ExecuteScalar();

                conn.Close();
            }

            return databaseName;
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
                    for(int i=0; i<table.Columns.Count ; i++)
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = @"
SELECT  
	cols.COLUMN_NAME AS Name,  
    CASE  
        WHEN cols.DOMAIN_NAME IS NOT NULL THEN cols.DOMAIN_NAME  
        ELSE cols.DATA_TYPE  
    END AS Type,
    cols.DATA_TYPE AS DataType,
    CAST(cols.CHARACTER_MAXIMUM_LENGTH AS int) AS Length,  
    cols.NUMERIC_PRECISION AS Precision,  
    cols.NUMERIC_SCALE AS Scale,  
    cols.IS_NULLABLE AS IsNull,  
    cols.COLUMN_DEFAULT,  
    COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'),cols.COLUMN_NAME,'IsIdentity') AS IS_IDENTITY, 
    COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'),cols.COLUMN_NAME,'IsRowGuidCol') AS IS_ROW_GUID_COL,  
    COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'),cols.COLUMN_NAME,'IsComputed') AS IS_COMPUTED,  
    COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'),cols.COLUMN_NAME,'IsDeterministic') AS IS_DETERMINISTIC,  
    CASE 
        WHEN (COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'), cols.COLUMN_NAME, N'IsIdentity') <> 0) 
        then CONVERT(nvarchar(40), ident_seed(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']')) 
        else null end AS IDENTITY_SEED,  
    CASE 
        WHEN (COLUMNPROPERTY(OBJECT_ID(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']'), cols.COLUMN_NAME, N'IsIdentity') <> 0) 
        then CONVERT(nvarchar(40), ident_incr(N'[' + @DatabaseName + N'].[' + @OwnerName + N'].[' + @TableName + N']')) else null 
    end AS IDENTITY_INCREMENT,
    NULL AS COMPUTED_DEFINITION,
    NULL AS [collation]  
FROM  
    INFORMATION_SCHEMA.COLUMNS cols  
WHERE  
	cols.TABLE_CATALOG = @DatabaseName  
	AND cols.TABLE_SCHEMA = @OwnerName  
	AND cols.TABLE_NAME = @TableName  
ORDER BY  
	cols.ORDINAL_POSITION;
";
                SqlParameter param;

                param = new SqlParameter("@DatabaseName", SqlDbType.NVarChar);
                param.Value = databaseName;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@OwnerName", SqlDbType.NVarChar);
                param.Value = ownerName;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@TableName", SqlDbType.NVarChar);
                param.Value = tableName;
                cmd.Parameters.Add(param);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        DataColumnSchema item = new DataColumnSchema();

                        item.Name = (dr["Name"] == DBNull.Value) ? string.Empty : (string)dr["Name"];

                        item.Type = SetDataType((string)dr["Type"]);

                        item.Nullable = ((string)dr["IsNull"] == "NO") ? false : true;

                        switch (item.Type)
                        {
                            case DbType.String:
                                item.Length = (dr["Length"] == DBNull.Value) ? 0 : (int)dr["Length"];
                                break;
                            case DbType.Decimal:
                                item.Precision = (dr["Precision"] == DBNull.Value) ? (byte)0 : (byte)dr["Precision"];
                                item.Scale = (dr["Scale"] == DBNull.Value) ? 0 : (int)dr["Scale"];
                                break;
                            default:
                                break;
                        }
                        
                        list.Add(item);
                    }
                }

                conn.Close();
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = @"
SELECT  
	cols.COLUMN_NAME AS Name
FROM  
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE cols  
WHERE  
	cols.TABLE_CATALOG = @DatabaseName  
	AND cols.TABLE_SCHEMA = @OwnerName  
	AND cols.TABLE_NAME = @TableName  
ORDER BY  
	cols.ORDINAL_POSITION;
";
                SqlParameter param;

                param = new SqlParameter("@DatabaseName", SqlDbType.NVarChar);
                param.Value = databaseName;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@OwnerName", SqlDbType.NVarChar);
                param.Value = ownerName;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@TableName", SqlDbType.NVarChar);
                param.Value = tableName;
                cmd.Parameters.Add(param);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataColumnSchema item = new DataColumnSchema();

                        item.Name = (dr["Name"] == DBNull.Value) ? string.Empty : (string)dr["Name"];

                        list.Add(item);
                    }
                }
                conn.Close();
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

                case "image": return DbType.Binary;

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
                default: return "UnknownDbType";
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

                default: return "UnknownDbType";
            }
        }
    }
}
