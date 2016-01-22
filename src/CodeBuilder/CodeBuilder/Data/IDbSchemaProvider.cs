namespace X3Platform.CodeBuilder.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>���ݿ�ṹ�ṩ��</summary>
    public interface IDbSchemaProvider
    {
        /// <summary>���ݿ������ַ���</summary>
        string ConnectionString { get; set; }

        /// <summary>��ѯ���ݿ�����</summary>
        /// <returns>���ݿ�����</returns>
        string GetDatabaseName();

        /// <summary>��ѯ���ݿ��ж�������Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableNames">����, ����԰�Ƕ��Ÿ���</param>
        /// <returns>������ݱ���Ϣ���б�</returns>
        IList<DataTableSchema> GetTables(string databaseName, string ownerName, string tableNames);

        /// <summary>��ѯ���ݿ��б����Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">����</param>
        /// <returns>���ݱ���Ϣ</returns>
        DataTableSchema GetTable(string databaseName, string ownerName, string tableName);

        /// <summary>��ѯ���ݿ��б���ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName);

        /// <summary>��ѯ���ݿ��б�������ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>��ѯ���ݿ��б������ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns>����ֶ���Ϣ����</returns>
        DataColumnSchemaCollection GetForeignKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>��ѯ���ݿ��б������������ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetNoPrimaryKeyColumns(DataTableSchema table);

        /// <summary>������������</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DbType SetDataType(string type);

        /// <summary>��ȡ��������</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetDataType(DbType type);
    }
}
