namespace X3Platform.CodeBuilder.Template
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.CodeBuilder.Configuration;

    public delegate void GenerateHandler();

    /// <summary>ģ��������</summary>
    public abstract class TemplateGenerator 
    {
        /// <summary>���ɴ���</summary>
        public GenerateHandler Generate;

        private string m_TemplateFile = string.Empty;

        /// <summary>ģ���ļ�</summary>
        public string TemplateFile
        {
            get { return this.m_TemplateFile; }
            set { this.m_TemplateFile = value; }
        }

        private string description = string.Empty;

        /// <summary>��������</summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string author = "ruanyu";

        /// <summary>����</summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Date
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd"); }
        }

        /// <summary>��ʼ������</summary>
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
        /// ͨ��
        /// </summary>
        /// <param name="value">ͨ����Ϣ</param>
        public virtual void Notify(string value)
        {
            foreach (ITemplateObserver observer in observers)
            {
                observer.Generate(value);
            }
        }

        /// <summary>��� �۲���</summary>
        /// <param name="item"></param>
        public void AddObserver(ITemplateObserver item)
        {
            observers.Add(item);
        }

        /// <summary>ɾ�� �۲���</summary>
        /// <param name="item"></param>
        public void RemoveObserver(ITemplateObserver item)
        {
            observers.Remove(item);
        }
    }
}
