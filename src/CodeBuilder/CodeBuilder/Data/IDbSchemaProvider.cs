namespace X3Platform.CodeBuilder.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>数据库结构提供器</summary>
    public interface IDbSchemaProvider
    {
        /// <summary>数据库连接字符串</summary>
        string ConnectionString { get; set; }

        /// <summary>查询数据库名称</summary>
        /// <returns>数据库名称</returns>
        string GetDatabaseName();

        /// <summary>
        /// 查询[数据库].[所有者].[表] 中的字段信息
        /// </summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataTableSchema GetTable(string databaseName, string ownerName, string tableName);

        /// <summary>
        /// 查询[数据库].[所有者].[表] 中的字段信息
        /// </summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName);

        /// <summary>查询[数据库].[所有者].[表] 中的主键字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>
        /// 查询[数据库].[所有者].[表] 中的主键意外的字段信息
        /// </summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetNoPrimaryKeyColumns(DataTableSchema table);

        DbType SetDataType(string type);

        string GetDataType(DbType type);
    }
}
