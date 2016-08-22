using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using X3Platform.Configuration;
using X3Platform.Services;
using X3Platform.Services.Configuration;

namespace X3Platform.HttpServer.Terminal
{
    /// <summary>
    /// 服务管理者
    /// </summary>
    public class ServerManagement
    {
        string ServiceName = null;

        HttpServer server = null;

        int port = 0;

        string virtualPath = null;

        string physicalPath = null;

        private IList<Process> backgroundProcesses = new List<Process>();

        public ServerManagement()
        {
            this.ServiceName = ServicesConfigurationView.Instance.ServiceName;

            this.port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

            this.virtualPath = ConfigurationManager.AppSettings["virtualPath"];

            if (string.IsNullOrEmpty(this.virtualPath))
            {
                this.virtualPath = "/";
            }

            this.physicalPath = ConfigurationManager.AppSettings["physicalPath"];

            if (string.IsNullOrEmpty(this.physicalPath))
            {
                this.physicalPath = KernelConfigurationView.Instance.ApplicationPathRoot;
            }
            else
            {
                this.physicalPath = System.IO.Path.GetFullPath(this.physicalPath);
            }
        }

        public void Start()
        {
            server = new HttpServer(port, virtualPath, physicalPath,
               ConfigurationManager.AppSettings["staticFileFilters"],
               ConfigurationManager.AppSettings["directoryBrowse"]
               );

            string processes = ConfigurationManager.AppSettings["processes"];

            if (!string.IsNullOrEmpty(processes))
            {
                string[] fileNames = processes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string fileName in fileNames)
                {
                    Console.WriteLine("正在启动后台程序:" + fileName);

                    Process backgroundProcess = Process.Start(new ProcessStartInfo() { FileName = fileName, WindowStyle = ProcessWindowStyle.Hidden });

                    backgroundProcesses.Add(backgroundProcess);
                }
            }

            server.Start();

            // 输出配置信息
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.WriteLine("Http server  starting success.");
            Console.WriteLine("Url \t\t: http://localhost:{0}{1}", port, virtualPath);
            Console.WriteLine("Physical path \t: {0}", physicalPath);
        }

        public void Stop()
        {
            EventLogHelper.Write(string.Format("{0}服务被停止。", this.ServiceName));

            foreach (Process backgroundProcess in backgroundProcesses)
            {
                if (!backgroundProcess.HasExited)
                {
                    backgroundProcess.Close();
                }
            }

            backgroundProcesses.Clear();

            if (server != null)
            {
                server.Stop();
            }
        }

        public void Pause()
        {
            // EventLogHelper.Write(string.Format("{0} 服务被暂停。", this.ServiceName));           
        }
    }
}