namespace X3Platform.DataDump
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Reflection;

    using X3Platform.Util;
    using X3Platform.Data;
    using X3Platform.Configuration;
    using X3Platform.CommandLine;
    using X3Platform.CommandLine.Text;
    using X3Platform.DataDump.Configuration;
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

            [Option('t', "task", HelpText = "任务名称。")]
            public string TaskName { get; set; }

            [Option("application", HelpText = "应用名称。")]
            public string ApplicationName { get; set; }

            [Option('o', "output", HelpText = "输出文件路径。")]
            public string OutputFile { get; set; }

            [Option('s', "silent", HelpText = "安静模式，程序执行完毕自动关闭。")]
            public bool Silent { get; set; }

            /// <summary>用法</summary>
            /// <returns></returns>
            [HelpOption(HelpText = "显示帮助信息。")]
            public string GetUsage()
            {
                var help = new HelpText(Program.headingInfo);

                help.AdditionalNewLineAfterOption = true;

                help.Copyright = new CopyrightInfo(KernelConfigurationView.Instance.WebmasterEmail, 2008, DateTime.Now.Year);

                //help.AddPreOptionsLine("人员及权限管理命令行工具");
                //help.AddPreOptionsLine("用于系统的人员及权限的日常维护管理.\r\n");

                help.AddPreOptionsLine("Usage:");
                help.AddPreOptionsLine(string.Format("  {0} --application News", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --application News --silent", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --application News --output \"06.新闻管理_应用默认配置初始化脚本(yyyy-MM-dd).sql\" ", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --application News --output \"C:\\06.新闻管理_应用默认配置初始化脚本(yyyy-MM-dd).sql\" --silent", program.Name));

                help.AddPreOptionsLine(string.Format("  {0} --help", program.Name));
                help.AddOptions(this);

                return help;
            }
            #endregion
        }

        /// <summary>应用程序的主入口点。</summary>
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
                RunTask(options);
            }
            else if (string.IsNullOrEmpty(options.ApplicationName) || options.ApplicationName.ToLower() == "kernel")
            {
                // 生成基础应用SQL脚本
                GenerateKernelSqlScript(options);
            }
            else
            {
                // 生成应用SQL脚本
                GenerateSqlScript(options);
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

        #region 静态函数:RunTask(Options options)
        /// <summary>执行任务</summary>
        /// <param name="options"></param>
        static void RunTask(Options options)
        {
            DataDumpTask task = DataDumpConfiguration.Instance.Tasks[options.TaskName];

            if (task == null)
            {
                Console.WriteLine("task : " + options.TaskName + " is null");
            }
            else
            {
                Console.WriteLine("task : " + options.TaskName + " running...");

                IDataDumpProvider provider = (IDataDumpProvider)KernelContext.CreateObject(task.DataDumpProvider);

                provider.Init(task);

                var result = provider.Generate();

                Console.WriteLine("result => " + result);

                // 输出位置
                string path = StringHelper.NullTo(options.OutputFile, task.OutputFile);

                if (!string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, path);

                    File.WriteAllText(path, result, Encoding.UTF8);
                }
            }
        }
        #endregion

        #region 静态函数:GenerateKernelSqlScript(Options options)
        /// <summary>生成SQL脚本</summary>
        /// <param name="options"></param>
        static void GenerateKernelSqlScript(Options options)
        {

        }
        #endregion

        #region 静态函数:GenerateSqlScript(Options options)
        /// <summary>生成SQL脚本</summary>
        /// <param name="options"></param>
        static void GenerateSqlScript(Options options)
        {
            StringBuilder outString = new StringBuilder();

            string applicationDisplayName = FindApplicationDisplayName(options.ApplicationName);

            Console.WriteLine("{0}正在根据应用【{1}】的配置信息生成SQL脚本。", DateTime.Now.ToString("{yyyy-MM-dd HH:mm:ss.fff}"), options.ApplicationName);

            if (string.IsNullOrEmpty(applicationDisplayName))
            {
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Red);
                Console.WriteLine("{0}未找到相关的应用【{1}】的配置信息，请确认相关数据库中是否存在相关应用配置。", DateTime.Now.ToString("{yyyy-MM-dd HH:mm:ss.fff}"), options.ApplicationName);
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
            }
            else
            {
                outString.AppendLine("-- ================================================");
                outString.AppendLine("-- 初始化应用默认配置");
                outString.AppendLine("-- ================================================");
                outString.AppendLine();
                outString.AppendLine("-- 应用信息");
                outString.AppendLine(GenerateApplicationScript(options));
                outString.AppendLine(GenerateApplicationScopeScript(options));
                outString.AppendLine("-- 应用选项信息");
                outString.AppendLine(GenerateApplicationOptionScript(options));
                outString.AppendLine("-- 应用方法信息");
                outString.AppendLine(GenerateApplicationMethodScript(options));
                outString.AppendLine("-- 应用参数分组信息");
                outString.AppendLine(GenerateApplicationSettingGroupScript(options));
                outString.AppendLine("-- 应用参数信息");
                outString.AppendLine(GenerateApplicationSettingScript(options));
                outString.AppendLine("-- 应用菜单信息");
                outString.AppendLine(GenerateApplicationMenuScript(options));
                outString.AppendLine(GenerateApplicationMenuScopeScript(options));
                outString.AppendLine("-- 应用错误信息");
                // outString.AppendLine(GenerateApplicationMenuScript(options));
                // outString.AppendLine(GenerateApplicationMenuScopeScript(options));

                string path = string.Empty;

                if (string.IsNullOrEmpty(options.OutputFile))
                {
                    path = "SQLGenerator_Result(" + DateTime.Now.ToString("yyyy-MM-dd") + ").sql";
                }
                else
                {
                    path = options.OutputFile.Replace("yyyy-MM-dd", DateTime.Now.ToString("yyyy-MM-dd"));
                }

                path = Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, path);

                File.WriteAllText(path, outString.ToString(), Encoding.UTF8);
            }
        }
        #endregion

        static string FindApplicationDisplayName(string applicationName)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            object result = command.ExecuteScalar(" SELECT ApplicationDisplayName FROM tb_Application WHERE ApplicationName = '" + applicationName + "' ");

            return result == null ? string.Empty : result.ToString();
        }

        static string GenerateApplicationScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application", table);
        }

        static string GenerateApplicationScopeScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Scope WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY AuthorizationObjectType ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Scope", table);
        }

        static string GenerateApplicationOptionScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Option WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY OrderId, Name ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Option", table);
        }

        static string GenerateApplicationMethodScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Method WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY Code, OrderId ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Method", table);
        }

        static string GenerateApplicationSettingGroupScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_SettingGroup WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY OrderId ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_SettingGroup", table);
        }

        static string GenerateApplicationSettingScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Setting WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' )  ORDER BY ApplicationSettingGroupId, OrderId ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Setting", table);
        }

        static string GenerateApplicationMenuScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Menu WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY ParentId, OrderId ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Menu", table);
        }

        static string GenerateApplicationMenuScopeScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Menu_Scope WHERE EntityId IN ( SELECT Id FROM tb_Application_Menu WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ) ORDER BY EntityId, AuthorizationObjectType ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Menu_Scope", table);
        }

        static string GenerateApplicationErrorScript(Options options)
        {
            GenericSqlCommand command = new GenericSqlCommand("ConnectionString");

            DataTable table = command.ExecuteQueryForDataTable(" SELECT * FROM tb_Application_Error WHERE ApplicationId IN ( SELECT Id FROM tb_Application WHERE ApplicationName = '" + options.ApplicationName + "' ) ORDER BY ParentId, OrderId ");

            return SqlScriptHelper.GenerateDateTableScript("", "tb_Application_Error", table);
        }
    }
}