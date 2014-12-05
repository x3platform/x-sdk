using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Template
{
    public class ConsoleObserver : ITemplateObserver
    {
        public void Generate(string value)
        {
            Console.WriteLine(value);
        }
    }
}
