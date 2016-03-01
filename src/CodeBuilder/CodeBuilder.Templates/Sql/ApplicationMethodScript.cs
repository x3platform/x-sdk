using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Reflection;

using X3Platform.CodeBuilder.Data;
using X3Platform.CodeBuilder.Configuration;

namespace X3Platform.CodeBuilder.Template.Sql
{
    public class ApplicationMethodScript : TemplateGenerator
    {
        // private IDatabaseProvider provider;

        private DatabaseSchema database;

        private DataTableSchema table;

        private StringBuilder buffer = new StringBuilder();

        private string ownerName = "dbo";

        public override void Init(string generatorName, CodeBuilderConfiguration configuration)
        {
            provider = (X3Platform.CodeBuilder.Data.IDatabaseProvider)Assembly.Load(configuration.DatabaseProvider.Assembly).CreateInstance(configuration.DatabaseProvider.ClassName,
                    false, BindingFlags.Default,
                    null, new object[] { configuration.DatabaseProvider.ConnectionString },
                    null, null);

            database = new X3Platform.CodeBuilder.Data.DatabaseSchema();

            database.Name = provider.GetDatabaseName();
            database.OwnerName = configuration.DatabaseProvider.OwnerName;

            table = provider.GetTable(database.Name, database.OwnerName, configuration.Tasks[generatorName].Properties["DataTable"].Value);

            //���� ����ļ�
            if (configuration.Tasks[generatorName].Properties["File"] == null)
            {
                configuration.Tasks[generatorName].Properties.Add(new X3Platform.CodeBuilder.Configuration.TaskProperty("File", configuration.Tasks[generatorName].Properties["DataTable"].Value + ".StorageProcedure.sql"));
            }
            else if (string.IsNullOrEmpty(configuration.Tasks[generatorName].Properties["File"].Value))
            {
                configuration.Tasks[generatorName].Properties["File"].Value = configuration.Tasks[generatorName].Properties["DataTable"].Value + ".StorageProcedure.sql";
            }
        }

        #region ����:StorageProcedureGenerate()
        /// <summary>
        /// ���ɴ洢����
        /// </summary>
        public void StorageProcedureGenerate()
        {
            buffer.Length = 0;

            buffer.AppendLine(GetItemByPrimaryKey()); //��ѯ

            if (table.Columns[0].Name.ToUpper() == "ID")
                buffer.AppendLine(GetItemById()); //Id ��ѯ

            buffer.AppendLine(GetItemsByWhereClause()); //��ѯ

            buffer.AppendLine(InsertItem()); //���

            buffer.AppendLine(UpdateItem()); //�޸�

            buffer.AppendLine(DeleteItem()); //ɾ��

            if (table.Columns[0].Name.ToUpper() == "ID")
                buffer.AppendLine(DeleteItemById()); //ɾ��

            buffer.AppendLine(DeleteItemsByArray()); // ����ɾ��

            buffer.AppendLine(IsExist()); //�Ƿ����

            buffer.AppendLine(GetPages()); //��ҳ

            if (table.Name == "CodeSmithTable" || table.Name == "Account" || table.Name == "Data.Account" || table.Name == "Data_Account")
                buffer.AppendLine(LoginCheck());

            Notify(buffer.ToString());
        }
        #endregion

        //===========================================================
        // ��ѯ
        //===========================================================

        #region ����:GetItemById()
        public string GetItemById()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_GetItemById]");
            outString.AppendLine("(");
            outString.AppendLine("\t@Id int ");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �в�ѯĳ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");

            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");

            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tSELECT ");

            //��ѯ������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine();

            outString.AppendLine("\tFROM");
            outString.AppendLine("\t\t[" + table.Name + "]");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine(GetWhereClause(table));

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("SET @Err = @@Error");
            outString.AppendLine("RETURN @Err");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:GetItemByPrimaryKey()
        public string GetItemByPrimaryKey()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_GetItemByPrimaryKey]");
            outString.AppendLine("(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].PrimaryKey)
                    outString.AppendLine("\t" + GetSqlParameterStatement(table.Columns[i]) + ",");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }

            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �в�ѯĳ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");

            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");

            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tSELECT ");

            //��ѯ������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine();

            outString.AppendLine("\tFROM");
            outString.AppendLine("\t\t[" + table.Name + "]");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine(GetWhereClause(table));

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("SET @Err = @@Error");
            outString.AppendLine("RETURN @Err");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:GetItemsByWhereClause()
        public string GetItemsByWhereClause()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_GetItemsByWhereClause]");
            outString.AppendLine("(");
            outString.AppendLine("\t@WhereClause nvarchar(800),");
            outString.AppendLine("\t@Length int");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �в�ѯ��ؼ�¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");

            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");

            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tDECLARE @SQL as nvarchar(4000)");
            outString.AppendLine();
            outString.AppendLine("\tSET @SQL = 'SELECT '");
            outString.AppendLine();
            outString.AppendLine("\tIF @Length > 0 ");
            outString.AppendLine("\tBEGIN");
            outString.AppendLine("\t\tSET @SQL = @SQL + ' TOP ' + CONVERT(nvarchar(15),@Length) + ' '");
            outString.AppendLine("\tEND");
            outString.AppendLine();
            outString.AppendLine("\tSET @SQL = @SQL + '");

            //������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine();

            outString.AppendLine("\tFROM " + table.Name + " '");
            outString.AppendLine();
            outString.AppendLine("\tIF Len(@WhereClause) > 0 ");
            outString.AppendLine("\tBEGIN");
            outString.AppendLine("\t\tSET @SQL = @SQL + ' WHERE ' + @WhereClause ");
            outString.AppendLine("\tEND");

            outString.AppendLine("\tEXEC sp_executesql @SQL");

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("SET @Err = @@Error");
            outString.AppendLine("RETURN @Err");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        //===========================================================
        // ��� �޸� ɾ��
        //===========================================================

        #region ����:InsertItem()
        public string InsertItem()
        {
            StringBuilder outString = new StringBuilder();

            bool isExistId = false;

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_InsertItem]");
            outString.AppendLine("(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.Append("\t" + GetSqlParameterStatement(table.Columns[i]));

                if (table.Columns[i].Name.ToUpper() == "ID")
                {
                    isExistId = true;
                    outString.Append(" output");
                }
                outString.AppendLine(",");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }

            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �����һ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");

            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");

            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tINSERT INTO [" + table.Name + "] ");
            outString.AppendLine("\t(");

            //���������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine("\t)");

            outString.AppendLine("\tVALUES");
            outString.AppendLine("\t(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t@" + table.Columns[i].Name + ",");
            }
            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine("\t)");

            //���� @Id ֵ.
            if (isExistId)
            {
                outString.AppendLine("\tSET @Id = @@IDENTITY");
            }

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("SET @Err = @@Error");
            outString.AppendLine("RETURN @Err");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:UpdateItem()
        public string UpdateItem()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_UpdateItem]");
            outString.AppendLine("(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.Append("\t" + GetSqlParameterStatement(table.Columns[i]));

                if (i < table.Columns.Count - 1)
                    outString.Append(",");

                outString.AppendLine();
            }

            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �и���һ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tUPDATE [" + table.Name + "] SET");

            //����������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (!table.Columns[i].PrimaryKey)
                    outString.AppendLine("\t\t[" + table.Columns[i].Name + "] = @" + table.Columns[i].Name + ",");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }

            outString.AppendLine("\tWHERE");
            outString.AppendLine(GetWhereClause(table));

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("SET @Err = @@Error");
            outString.AppendLine("RETURN @Err");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:DeleteItem()
        public string DeleteItem()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_DeleteItem]");
            outString.AppendLine("(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].PrimaryKey)
                    outString.AppendLine("\t" + GetSqlParameterStatement(table.Columns[i]) + ",");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }

            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �и�������ɾ��һ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tDELETE FROM [" + table.Name + "] ");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine(GetWhereClause(table));

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:DeleteItemById()
        public string DeleteItemById()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_DeleteItemById]");
            outString.AppendLine("(");
            outString.AppendLine("\t@Id int");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �и�������ɾ��һ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tDELETE FROM [" + table.Name + "] ");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine("\t\tId = @Id");

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:DeleteItemsByArray()
        public string DeleteItemsByArray()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_DeleteItemsByArray]");
            outString.AppendLine("(");
            outString.AppendLine("\t@Array nvarchar(200)");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] �и�������ɾ����¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tDECLARE @SQL AS nvarchar(3500)");

            outString.AppendLine();
            outString.AppendLine("\tSET @SQL = 'DELETE FROM [" + table.Name + "] WHERE [Id] IN (' + @Array + ') '");

            outString.AppendLine();
            outString.AppendLine("\tEXEC sp_executesql @SQL");


            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        //===========================================================
        // �û��Զ��幦��
        //===========================================================

        #region ����:GetPages()
        public string GetPages()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_GetPages]");
            outString.AppendLine("(");
            outString.AppendLine("\t@WhereClause nvarchar(200),");
            outString.AppendLine("\t@OrderBy nvarchar(200),");
            outString.AppendLine("\t@StartIndex int,");
            outString.AppendLine("\t@PageSize int,");
            outString.AppendLine("\t@RowsNum int output");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary:[" + table.Name + "] select of pages.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tDECLARE @PageLowerBound int");
            outString.AppendLine("\tDECLARE @PageUpperBound int");
            outString.AppendLine("\tDECLARE @RowsToReturn int");
            outString.AppendLine();
            outString.AppendLine("\t-- First set the rowcount");
            outString.AppendLine();
            outString.AppendLine("\t-- Set ROWCOUNT @RowsToReturn");
            outString.AppendLine();
            outString.AppendLine("\t-- Set the page bounds");
            outString.AppendLine();
            outString.AppendLine("\tSET @PageLowerBound = @StartIndex");
            outString.AppendLine("\tSET @PageUpperBound = @StartIndex + @PageSize");
            outString.AppendLine();

            outString.AppendLine("\t-- Create a temp table to store the select results");

            outString.AppendLine("\tCREATE TABLE #PageIndex");
            outString.AppendLine("\t(");
            outString.AppendLine("\t\t[IndexId] int IDENTITY (1, 1) NOT NULL,");
            outString.AppendLine("\t\t[ID] int");
            outString.AppendLine("\t)");
            outString.AppendLine();
            outString.AppendLine("\t-- Insert into the temp table");
            outString.AppendLine("\tDECLARE @SQL AS nvarchar(4000)");
            outString.AppendLine("\tSET @SQL = 'INSERT INTO #PageIndex ([ID])'");
            outString.AppendLine("\tSET @SQL = @SQL + ' SELECT [ID]'");
            outString.AppendLine("\tSET @SQL = @SQL + ' FROM [" + table.Name + "] '");
            outString.AppendLine("\tIF LEN(@WhereClause) > 0");
            outString.AppendLine("\tBEGIN");
            outString.AppendLine("\t\tSET @SQL = @SQL + ' WHERE ' + @WhereClause");
            outString.AppendLine("\tEND");
            outString.AppendLine();
            outString.AppendLine("\tIF LEN(@OrderBy) > 0");
            outString.AppendLine("\tBEGIN");
            outString.AppendLine("\t\tSET @SQL = @SQL + ' ORDER BY ' + @OrderBy");
            outString.AppendLine("\tEND");
            outString.AppendLine();
            outString.AppendLine("\t-- Order by the first column.");
            outString.AppendLine("\tELSE");
            outString.AppendLine("\tBEGIN");
            outString.AppendLine("\t\tSET @SQL = @SQL + ' ORDER BY [" + table.Columns[0].Name + "] DESC'");
            outString.AppendLine("\tEND");
            outString.AppendLine();

            outString.AppendLine("\t-- Populate the temp table");
            outString.AppendLine("\tEXEC sp_executesql @SQL");
            outString.AppendLine();

            outString.AppendLine("\t-- Return total count");
            outString.AppendLine("\tSET @RowsNum = @@ROWCOUNT");
            outString.AppendLine();

            outString.AppendLine("\t-- Set RowCount After Total Rows is determined");
            outString.AppendLine("\t-- Return paged results");
            outString.AppendLine();
            outString.AppendLine("\tSELECT ");
            //��ѯ������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\tT.[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine();

            outString.AppendLine("\tFROM ");
            outString.AppendLine("\t\t" + table.Name + " T,");
            outString.AppendLine("\t\t#PageIndex PageIndex ");
            outString.AppendLine();

            outString.AppendLine("\tWHERE ");
            outString.AppendLine("\t\tT.Id = PageIndex.Id ");
            outString.AppendLine("\t\tAND PageIndex.IndexId > @PageLowerBound ");
            outString.AppendLine("\t\tAND PageIndex.IndexID <= @PageUpperBound ");
            outString.AppendLine();

            outString.AppendLine("\tORDER BY ");
            outString.AppendLine("\t\tPageIndex.IndexId ");
            outString.AppendLine();
            //    SELECT 
            //    <% for (int i = 0; i < Table.Columns.Count; i++) { %>
            //        T.[<%= Table.Columns[i].Name %>]<% if (i < Table.Columns.Count - 1) { %>,<% } %>
            //    <% } %>

            //    FROM
            //        <%= Table.Name %> T,
            //        #PageIndex PageIndex
            //    WHERE
            //        T.Id = PageIndex.Id AND 				    
            //        PageIndex.IndexID > @PageLowerBound AND
            //        PageIndex.IndexID <= @PageUpperBound
            //    ORDER BY
            //        PageIndex.IndexId


            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:IsExist()
        public string IsExist()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_IsExist]");
            outString.AppendLine("(");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].PrimaryKey)
                    outString.AppendLine("\t" + GetSqlParameterStatement(table.Columns[i]) + ",");
            }

            outString.AppendLine("\t@Count int output");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �� [" + table.Name + "] ���Ƿ���ڴ˼�¼.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tSELECT @Count = count(*) FROM [" + table.Name + "] ");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine(GetWhereClause(table));

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        #region ����:LoginCheck()
        public string LoginCheck()
        {
            StringBuilder outString = new StringBuilder();

            outString.AppendLine("CREATE PROCEDURE [" + ownerName + "].[" + table.Name + "_LoginCheck]");
            outString.AppendLine("(");
            outString.AppendLine("\t@UserName nvarchar(50),");
            outString.AppendLine("\t@Password nvarchar(50)");
            outString.AppendLine(")");
            outString.AppendLine();
            outString.AppendLine("AS");
            outString.AppendLine();
            outString.AppendLine("--===========================================================");
            outString.AppendLine("-- Author: " + Author);
            outString.AppendLine("-- Summary: �ʺŵ�½���.");
            outString.AppendLine("-- Date: " + Date);
            outString.AppendLine("--===========================================================");
            outString.AppendLine();
            outString.AppendLine("SET NOCOUNT ON");
            outString.AppendLine("DECLARE @Err int");
            outString.AppendLine();
            outString.AppendLine("BEGIN");
            outString.AppendLine();
            outString.AppendLine("\tSELECT ");

            //��ѯ������Ϣ
            for (int i = 0; i < table.Columns.Count; i++)
            {
                outString.AppendLine("\t\t[" + table.Columns[i].Name + "],");
            }

            if (outString.ToString().Substring(outString.Length - 3, 3) == ",\r\n")
            {
                outString.Remove(outString.Length - 3, 3);
                outString.AppendLine();
            }
            outString.AppendLine();

            outString.AppendLine("\tFROM");
            outString.AppendLine("\t\t[" + table.Name + "]");
            outString.AppendLine();

            outString.AppendLine("\tWHERE");
            outString.AppendLine("\t\tUserName = @UserName AND Password = @Password");
            outString.AppendLine();

            outString.AppendLine("\tUpdate [" + table.Name + "] SET [LoginTime] = getdate() WHERE UserName = @UserName And Password = @Password");

            outString.AppendLine("END");

            outString.AppendLine();
            outString.AppendLine("GO");

            return outString.ToString();
        }
        #endregion

        //===========================================================
        // ����
        //===========================================================

        #region ����:GetSqlParameterStatement(DataColumn column)
        /// <summary>
        /// Sql�������ʽ
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private string GetSqlParameterStatement(X3Platform.CodeBuilder.Data.DataColumnSchema column)
        {
            string param = "@" + column.Name + " " + provider.GetDataType(column.Type);

            switch (column.Type)
            {
                case DbType.String:
                    {
                        if (column.Length > 0)
                        {
                            param += "(" + column.Length + ")";
                        }
                        else if (column.Length == -1)
                        {
                            param += "(max)";
                        }
                        break;
                    }
                case DbType.Decimal:
                    {

                        param += "(" + column.Precision + "," + column.Scale + ")";
                        break;
                    }
                case DbType.DateTime:
                case DbType.Boolean:
                case DbType.Int16:
                case DbType.Int32: break;

                default:
                    break;
            }

            return param;
        }
        #endregion

        #region ����:GetWhereClause(DataTableSchema table)
        /// <summary>
        /// ȡ�ó��� Where ��Ϣ
        /// </summary>
        /// <returns>Where Clause</returns>
        private string GetWhereClause(DataTableSchema table)
        {
            StringBuilder outString = new StringBuilder();

            bool firstClause = true;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].PrimaryKey)
                {
                    outString.Append("\t\t");
                    if (!firstClause)
                        outString.Append("AND ");

                    outString.AppendLine(table.Columns[i].Name + " = @" + table.Columns[i].Name);
                    firstClause = false;
                }
            }

            return outString.ToString();
        }
        #endregion
    }
}
