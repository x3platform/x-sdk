namespace X3Platform.DataDump.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    
    using X3Platform.Data;
    using X3Platform.Json;
    
    using X3Platform.DataDump.Configuration;
    
    public class GenericDataDumpProvider : IDataDumpProvider
    {
        protected DataDumpTask task = null;

        protected IDictionary<string, string> options = null;

        /// <summary>��ʼ������</summary>
        /// <param name="task">������Ϣ</param>
        public void Init(DataDumpTask task)
        {
            this.task = task;
            this.options = new Dictionary<string, string>();

            JsonData data = ((JsonData)Json.JsonMapper.ToObject(task.Options));

            foreach (string key in data.Keys)
            {
                if (options.ContainsKey(key))
                {
                    options[key] = data[key].ToString();
                }
                else
                {
                    options.Add(key, data[key].ToString());
                }
            }
        }

        public string Generate()
        {
            StringBuilder outString = new StringBuilder();

            // ִ����������
            if (!string.IsNullOrEmpty(task.Depends))
            {
                string[] depends = task.Depends.Split(',');

                foreach (string depend in depends)
                {
                    // �������������� ���ⷢ��ѭ������
                    if (depend == task.Name) continue;

                    DataDumpTask dependTask = DataDumpConfiguration.Instance.Tasks[depend];

                    // ��д��������Ĳ���ѡ���������ݿ�����
                    dependTask.Options = task.Options;
                    dependTask.OutputDbType = task.OutputDbType;

                    IDataDumpProvider dependProvider = (IDataDumpProvider)KernelContext.CreateObject(dependTask.DataDumpProvider);

                    dependProvider.Init(dependTask);

                    outString.AppendLine(dependProvider.Generate());
                }
            }

            // ִ������
            string comment, result;

            foreach (TaskStatement statement in task.Statements)
            {
                comment = null;
                result = null;

                if (!string.IsNullOrEmpty(statement.Description))
                {
                    string[] descriptionLines = statement.Description.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string descriptionLine in descriptionLines)
                    {
                        if (!string.IsNullOrEmpty(descriptionLine.Trim()))
                        {
                            comment += "-- " + descriptionLine.Trim() + Environment.NewLine;
                        }
                    }
                }

                GenericSqlCommand command = new GenericSqlCommand(task.DataSourceName);

                string sql = statement.Sql;

                foreach (KeyValuePair<string, string> option in options)
                {
                    sql = sql.Replace("$" + option.Key + "$", option.Value);
                }

                DataTable table = command.ExecuteQueryForDataTable(sql);

                if (table.Rows.Count == 0) continue;

                result = SqlScriptHelper.GenerateDateTableScript(task.OutputDbType, statement.DestTable, table);

                outString.Append(comment);
                outString.AppendLine(result);
            }

            return outString.ToString();
        }
    }
}
