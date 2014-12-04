using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace X3Platform.CodeBuilder.Configuration
{
    public class TaskPropertyCollection : Collection<TaskProperty>
    {
        public TaskProperty this[string index]
        {
            get
            {
                foreach (TaskProperty property in this)
                {
                    if (property.Name == index)
                    {
                        return property;
                    }
                }
                return null;
            }
        }
    }
}
