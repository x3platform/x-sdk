namespace X3Platform.HttpServer.Terminal
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Reflection;

    using X3Platform.Util;
    using X3Platform.Data;
    using X3Platform.Configuration;
    using X3Platform.CommandLine;
    using X3Platform.CommandLine.Text;
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

            [Option("port", HelpText = "端口。")]
            public int Port { get; set; }

            [Option("physicalPath", HelpText = "物理路径。")]
            public string PhysicalPath { get; set; }

            [Option("virtualPath", HelpText = "虚拟路径。")]
            public string VirtualPath { get; set; }

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
                help.AddPreOptionsLine(string.Format("  {0} --port 8800", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --port 8800 --physicalPath \"D:\\vhosts\\web\\com.x3platform.www.80\" ", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --port 8800 --virtualPath api --physicalPath \"D:\\vhosts\\web\\com.x3platform.www.80\" ", program.Name));

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

                if (options.Port == 0)
                {
                    options.Port = ConfigurationManager.AppSettings["port"] == null ?
                        8888 : Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                }

                if (string.IsNullOrEmpty(options.VirtualPath))
                {
                    options.VirtualPath = ConfigurationManager.AppSettings["virtualPath"] == null ?
                        "/" : ConfigurationManager.AppSettings["virtualPath"];
                }

                if (string.IsNullOrEmpty(options.PhysicalPath))
                {
                    options.PhysicalPath = ConfigurationManager.AppSettings["physicalPath"] == null ?
                        AppDomain.CurrentDomain.SetupInformation.ApplicationBase : ConfigurationManager.AppSettings["physicalPath"];
                }

                var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);

                if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(1)))
                {
                    Command(options);
                }
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex);

                Console.WriteLine("{" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "}");
                Console.WriteLine(ex);
            }

            Environment.Exit(0);
        }

        private static void Command(Options options)
        {
            HttpServer server = new HttpServer(options.Port, options.VirtualPath, options.PhysicalPath, 
                ConfigurationManager.AppSettings["staticFileFilters"],
                ConfigurationManager.AppSettings["directoryBrowse"]
                );

            server.Start();

            // 输出配置信息
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.WriteLine("Http server  starting success.");
            Console.WriteLine("Url \t\t: http://localhost:{0}{1}", options.Port, options.VirtualPath);
            Console.WriteLine("Physical path \t: {0}", options.PhysicalPath);

            Console.WriteLine("Pass any key to continue");
            Console.Read();

            server.Stop();
        }
    }
}