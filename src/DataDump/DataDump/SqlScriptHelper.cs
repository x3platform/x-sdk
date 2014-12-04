namespace X3Platform.DataDump
{
    using System;
    using System.Data;
    using System.Text;
    using X3Platform.Util;

    public static class SqlScriptHelper
    {
        private static string FormatName(string dbType, string name)
        {
            switch (dbType.ToUpper())
            {
                case "SQLSERVER":
                    return string.Concat("[", name, "]");
                case "ORACLE":
                    return string.Concat("\"", name, "\"");
                case "MYSQL":
                    return string.Concat("`", name, "`");

                default: return name;
            }
        }

        public static string GenerateDateTableScript(string dbType, string tableName, DataTable table)
        {
            if (table.Rows.Count == 0) { return string.Empty; };

            table.TableName = tableName;

            StringBuilder outString = new StringBuilder();

            StringBuilder prefixInsertStatement = new StringBuilder();

            prefixInsertStatement.Append("INSERT INTO " + FormatName(dbType, table.TableName) + " ");
            prefixInsertStatement.Append("(");

            foreach (DataColumn column in table.Columns)
            {
                prefixInsertStatement.Append(FormatName(dbType, column.ColumnName) + ", ");
            }

            prefixInsertStatement = StringHelper.TrimEnd(prefixInsertStatement, ", ");

            prefixInsertStatement.Append(") VALUES (");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                outString.Append(prefixInsertStatement.ToString());

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Columns[j].DataType.FullName == "System.String")
                    {
                        outString.Append("'" + table.Rows[i][j].ToString() + "', ");
                    }
                    else if (table.Columns[j].DataType.FullName == "System.Int32")
                    {
                        outString.Append(table.Rows[i][j].ToString() + ", ");
                    }
                    else if (table.Columns[j].DataType.FullName == "System.Boolean")
                    {
                        outString.Append(table.Rows[i][j].ToString() + ", ");
                    }
                    else
                    {
                        outString.Append("'" + table.Rows[i][j].ToString() + "', ");
                    }
                }

                outString = StringHelper.TrimEnd(outString, ", ");

                outString.AppendLine(");");
            }

            return outString.ToString();
        }
    }
}
