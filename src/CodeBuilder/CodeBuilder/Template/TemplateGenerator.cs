namespace X3Platform.CodeBuilder.Template
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CodeBuilder.Configuration;

    public delegate void GenerateHandler();

    /// <summary>模板生成器</summary>
    public abstract class TemplateGenerator 
    {
        /// <summary>生成代码</summary>
        public GenerateHandler Generate;

        private string m_TemplateFile = string.Empty;

        /// <summary>模板文件</summary>
        public string TemplateFile
        {
            get { return this.m_TemplateFile; }
            set { this.m_TemplateFile = value; }
        }

        private string description = string.Empty;

        /// <summary>内容描述</summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string author = "ruanyu";

        /// <summary>作者</summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd"); }
        }

        /// <summary>初始化配置</summary>
        /// <param name="taskName"></param>
        /// <param name="configuration"></param>
        public virtual void Init(string taskName, CodeBuilderConfiguration configuration) {
        }

        List<ITemplateObserver> observers = new List<ITemplateObserver>();

        public List<ITemplateObserver> TemplateObserverList
        {
            get { return observers; }
            set { observers = value; }
        }

        /// <summary>
        /// 通报
        /// </summary>
        /// <param name="value">通报信息</param>
        public virtual void Notify(string value)
        {
            foreach (ITemplateObserver observer in observers)
            {
                observer.Generate(value);
            }
        }

        /// <summary>添加 观察者</summary>
        /// <param name="item"></param>
        public void AddObserver(ITemplateObserver item)
        {
            observers.Add(item);
        }

        /// <summary>删除 观察者</summary>
        /// <param name="item"></param>
        public void RemoveObserver(ITemplateObserver item)
        {
            observers.Remove(item);
        }
    }
}
