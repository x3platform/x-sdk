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

        /// <summary>查询数据库中多个表的信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableNames">表名, 多个以半角逗号隔开</param>
        /// <returns>多个数据表信息的列表</returns>
        IList<DataTableSchema> GetTables(string databaseName, string ownerName, string tableNames);

        /// <summary>查询数据库中表的信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表名</param>
        /// <returns>数据表信息</returns>
        DataTableSchema GetTable(string databaseName, string ownerName, string tableName);

        /// <summary>查询数据库中表的字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName);

        /// <summary>查询数据库中表的主键字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>查询数据库中表的外键字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns>外键字段信息集合</returns>
        DataColumnSchemaCollection GetForeignKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>查询数据库中表的主键意外的字段信息</summary>
        /// <param name="databaseName">数据库</param>
        /// <param name="ownerName">所有者</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetNoPrimaryKeyColumns(DataTableSchema table);

        /// <summary>设置数据类型</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DbType SetDataType(string type);

        /// <summary>获取数据类型</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetDataType(DbType type);
    }
}
