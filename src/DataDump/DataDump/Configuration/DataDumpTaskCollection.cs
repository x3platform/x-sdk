namespace X3Platform.DataDump.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public class DataDumpTaskCollection : Collection<DataDumpTask>
    {
        public DataDumpTask this[string index]
        {
            get
            {
                foreach (DataDumpTask task in this)
                {
                    if (task.Name == index)
                    {
                        return task;
                    }
                }
                return null;
            }
        }
    }
}
