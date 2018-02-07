using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;

using X3Platform;
using X3Platform.Configuration;
using X3Platform.Services;
using X3Platform.Services.Configuration;
using X3Platform.Util;

namespace X3Platform.HttpServer.ServiceObservers
{
    /// <summary>守护服务监听器</summary>
    public class HttpServerServiceObserver : IServiceObserver
    {
        #region 属性:Name
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
        }
        #endregion

        private bool running = false;

        /// <summary>是否正在运行</summary>
        public bool IsRunning
        {
            get { return running; }
        }

        #region 属性:Sleeping
        public bool Sleeping
        {
            get { return (nextRunTime > DateTime.Now) ? true : false; }
        }
        #endregion

        public HttpServerServiceObserver()
            : this("HttpServerService", string.Empty)
        {
        }

        public HttpServerServiceObserver(string name, string args)
        {
            this.m_Name = name;

            Reload();
        }

        private ServicesConfiguration configuration = null;

        /// <summary>上一次执行的结束时间</summary>
        private DateTime nextRunTime = DateHelper.DefaultTime;

        // 单位(小时)
        private int runTimeInterval = 1;

        // 跟踪运行时间
        private bool trackRunTime = false;

        string ServiceName = null;

        HttpServer server = null;

        int port = 0;

        string virtualPath = null;

        string physicalPath = null;

        private IList<Process> backgroundProcesses = new List<Process>();

        public void Reload()
        {
            this.configuration = ServicesConfigurationView.Instance.Configuration;

            this.trackRunTime = ServicesConfigurationView.Instance.TrackRunTime;

            // this.nextRunTime = this.configuration.Observers[this.Name].NextRunTime;
            
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

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            if (running)
                return;

            try
            {
                if (nextRunTime < DateTime.Now)
                {
                    nextRunTime = DateTime.Now.AddHours(runTimeInterval);

                    this.configuration.Observers[this.Name].NextRunTime = nextRunTime;

                    ServicesConfigurationView.Instance.Save();

                    running = true;
                }
            }
            catch (Exception ex)
            {
                // 发生异常是, 记录异常信息  并把运行标识为false.

                EventLogHelper.Write(string.Format("{0}服务发生异常信息\r\n{1}", this.Name, ex.ToString()));

                running = false;
            }
        }
        
        public void Start()
        {
            EventLogHelper.Write(string.Format("{0}服务正在启动。", this.Name));

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
            KernelContext.Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.WriteLine("Http server  starting success.");
            Console.WriteLine("Url \t\t: http://localhost:{0}{1}", port, virtualPath);
            Console.WriteLine("Physical path \t: {0}", physicalPath);
        }

        public void Close()
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
    }
}