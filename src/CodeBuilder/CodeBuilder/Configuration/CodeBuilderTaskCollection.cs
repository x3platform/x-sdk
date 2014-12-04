using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace X3Platform.CodeBuilder.Configuration
{
    public class CodeBuilderTaskCollection : Collection<CodeBuilderTask>
    {
        public CodeBuilderTask this[string index]
        {
            get
            {
                foreach (CodeBuilderTask task in this)
                {
                    if (task.Name == index)
                    {
                        return task;
                    }
                }
                return null;
            }
        }

        private string m_Name;

        /// <summary>Ãû³Æ</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
    }
}
