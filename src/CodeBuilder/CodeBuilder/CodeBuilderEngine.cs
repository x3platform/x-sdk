using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;

using X3Platform.CodeBuilder.Configuration;
using X3Platform.CodeBuilder.Template;

namespace X3Platform.CodeBuilder
{
    public class CodeBuilderEngine
    {
        public readonly string ConnectionString = CodeBuilderConfiguration.Instance.DatabaseProvider.ConnectionString;

        public CodeBuilderEngine() { }

        public static void Run()
        {
            CodeBuilderConfiguration configuration = CodeBuilderConfiguration.Instance;

            for (int i = 0; i < configuration.Tasks.Count; i++)
            {
                TemplateGenerator generator = TemplateFactory.CreateObject(
                    configuration.Tasks[i].Name,
                    configuration.Tasks[i].Generator,
                    configuration);

                ITemplateObserver observer;

                foreach (TaskObserver item in configuration.Tasks[i].Observeres)
                {
                    switch (item.Type)
                    {
                        case "Console":
                            observer = new ConsoleObserver();
                            generator.AddObserver(observer);
                            break;
                        case "File":
                            observer = new FileObserver(configuration.Tasks[i]);
                            generator.AddObserver(observer);
                            break;
                        default:
                            break;
                    }
                }

                generator.Generate();
            }
        }

        public static void Run(string taskName)
        {
            CodeBuilderConfiguration configuration = CodeBuilderConfiguration.Instance;

            for (int i = 0; i < configuration.Tasks.Count; i++)
            {
                if (configuration.Tasks[i].Name.ToLower() == taskName.ToLower())
                {
                    TemplateGenerator generator = TemplateFactory.CreateObject(
                        configuration.Tasks[i].Name,
                        configuration.Tasks[i].Generator,
                        configuration);

                    ITemplateObserver observer;

                    foreach (TaskObserver item in configuration.Tasks[i].Observeres)
                    {
                        switch (item.Type)
                        {
                            case "Console":
                                observer = new ConsoleObserver();
                                generator.AddObserver(observer);
                                break;
                            case "File":
                                observer = new FileObserver(configuration.Tasks[i]);
                                generator.AddObserver(observer);
                                break;
                            default:
                                break;
                        }
                    }

                    generator.Generate();
                }
            }
        }

        public static CodeBuilderTask GetTaskConfiguration(string taskName)
        {
            CodeBuilderConfiguration configuration = CodeBuilderConfiguration.Instance;

            for (int i = 0; i < configuration.Tasks.Count; i++)
            {
                if (configuration.Tasks[i].Name.ToLower() == taskName.ToLower())
                {
                    return configuration.Tasks[i];
                }
            }

            return null;
        }
    }
}
