namespace X3Platform.CodeBuilder
{
    #region Using Libraries
    using System;
    using System.Reflection;

    using X3Platform.CommandLine;
    using X3Platform.CommandLine.Text;
    using X3Platform.Configuration;
    using X3Platform.Util;

    using X3Platform.CodeBuilder.Configuration;
    using X3Platform.CodeBuilder.Template;
    #endregion

    static class Program
    {
        private static readonly AssemblyName program = Assembly.GetExecutingAssembly().GetName();

        /// <summary>头部信息</summary>
        private static readonly HeadingInfo headingInfo = new HeadingInfo(program.Name,
            string.Format("{0}.{1}", program.Version.Major, program.Version.Minor));

        private sealed class Options
        {
            #region Standard Option Attribute

            [Option("task", HelpText = "任务名称。")]
            public string TaskName { get; set; }

            [Option('s', "silent", HelpText = "安静模式，程序执行完毕自动关闭。")]
            public bool Silent { get; set; }

            /// <summary>用法</summary>
            /// <returns></returns>
            [HelpOption(HelpText = "显示帮助信息。")]
            public string GetUsage()
            {
                var help = new HelpText(Program.headingInfo);

                help.AdditionalNewLineAfterOption = true;

                help.Copyright = new CopyrightInfo(KernelConfigurationView.Instance.WebmasterEmail, 2010, DateTime.Now.Year);

                help.AddPreOptionsLine("代码生成命令行工具");
                help.AddPreOptionsLine("用于数据读取基础操作的管理.\r\n");

                help.AddPreOptionsLine("Usage:");
                help.AddPreOptionsLine(string.Format("  {0} --task News", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --task News --silent", program.Name));

                help.AddPreOptionsLine(string.Format("  {0} --help", program.Name));
                help.AddOptions(this);

                return help;
            }
            #endregion
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();

                if (args.Length == 0)
                {
                    Console.WriteLine(options.GetUsage());
                    Environment.Exit(0);
                }

                var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);

                if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(1)))
                {
                    Command(options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "}");
                Console.WriteLine(ex);
            }

            Environment.Exit(0);
        }

        private static void Command(Options options)
        {
            // 开始时间
            TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            if (!string.IsNullOrEmpty(options.TaskName))
            {
                // 生成基础应用SQL脚本
                GenerateCode(options);
            }

            // 结束时间
            TimeSpan endTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
            Console.WriteLine("\r\n执行结束，共耗时{0}秒。", beginTimeSpan.Subtract(endTimeSpan).Duration().TotalSeconds);
            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);

            if (!options.Silent)
            {
                Console.WriteLine("Pass any key to continue");
                Console.Read();
            }
        }

        #region 静态函数:GenerateCode(Options options)
        /// <summary>生成SQL脚本</summary>
        /// <param name="options"></param>
        static void GenerateCode(Options options)
        {
            try
            {
                Console.WriteLine("loading configuration ...");
                
                string[] taskNames = options.TaskName.Split(',');

                foreach (string taskName in taskNames)
                {
                    var config = CodeBuilderEngine.GetTaskConfiguration(taskName);

                    if (config == null)
                    {
                        Console.WriteLine("未找到相关任务配置信息. [{\"task\":{\"name\":\"" + taskName + "\"}}]");
                        return;
                    }

                    Console.WriteLine();

                    Console.WriteLine("task name : {0}", config.Name);

                    foreach (TaskProperty property in config.Properties)
                    {
                        Console.WriteLine("\t{0} => {1}", property.Name, property.Value);
                    }

                    Console.WriteLine("\r\ngenerating code ...");

                    CodeBuilderEngine.Run(taskName);
                }

                Console.WriteLine("\r\nfinished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                FileObserver file = new FileObserver("\\error_" + DateTime.Now.ToString("yyyy_MM_dd") + ".log");
                file.Generate(ex.ToString());
            }
        }
        #endregion
    }
}