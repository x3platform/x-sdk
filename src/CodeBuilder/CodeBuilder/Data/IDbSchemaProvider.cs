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

        /// <summary>
        /// ��ѯ[���ݿ�].[������].[��] �е��ֶ���Ϣ
        /// </summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataTableSchema GetTable(string databaseName, string ownerName, string tableName);

        /// <summary>
        /// ��ѯ[���ݿ�].[������].[��] �е��ֶ���Ϣ
        /// </summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetColumns(string databaseName, string ownerName, string tableName);

        /// <summary>��ѯ[���ݿ�].[������].[��] �е������ֶ���Ϣ</summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetPrimaryKeyColumns(string databaseName, string ownerName, string tableName);

        /// <summary>
        /// ��ѯ[���ݿ�].[������].[��] �е�����������ֶ���Ϣ
        /// </summary>
        /// <param name="databaseName">���ݿ�</param>
        /// <param name="ownerName">������</param>
        /// <param name="tableName">��</param>
        /// <returns></returns>
        DataColumnSchemaCollection GetNoPrimaryKeyColumns(DataTableSchema table);

        DbType SetDataType(string type);

        string GetDataType(DbType type);
    }
}
