using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace X3Platform.CodeBuilder.Configuration
{
    public class TaskObserverCollection : Collection<TaskObserver>
    {
        public TaskObserver this[string index]
        {
            get 
            {
                foreach (TaskObserver observer in this)
                {
                    if(observer.Type == index)
                    {
                        return observer;
                    }
                }
                return null;
            }
        }
    }
}
