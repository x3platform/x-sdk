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
    using System.Diagnostics;
    using Topshelf;
    using Services.Configuration;
    #endregion

    static class Program
    {
        /// <summary>Ӧ�ó��������ڵ㡣</summary>
        [STAThread]
        static void Main(string[] args)
        {
            // -------------------------------------------------------
            // ����������������
            // -------------------------------------------------------

            HostFactory.Run(configure =>
            {
                configure.Service<ServerManagement>(callback =>
                {
                    callback.ConstructUsing(instance => new ServerManagement());
                    callback.WhenStarted(instance => instance.Start());
                    callback.WhenStopped(instance => instance.Stop());
                });

                // Support Linux
                configure.UseLinuxIfAvailable();

                configure.RunAsLocalSystem();

                configure.SetServiceName(ServicesConfigurationView.Instance.ServiceName);
                configure.SetDisplayName(ServicesConfigurationView.Instance.ServiceDisplayName);
                configure.SetDescription(ServicesConfigurationView.Instance.ServiceDescription);
            });
        }
    }
}