using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.CodeBuilder.Template
{
    public interface ITemplateObserver
    {
        void Generate(string value);
    }
}
