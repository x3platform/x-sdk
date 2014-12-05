using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using X3Platform.CodeBuilder.Configuration;
using X3Platform.CodeBuilder.Util;

namespace X3Platform.CodeBuilder.Template
{
    public class FileObserver : ITemplateObserver
    {
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public FileObserver() { }

        public FileObserver(CodeBuilderTask task)
        {
            this.filePath = Directory.GetCurrentDirectory() + "\\" + task.Properties["Directory"].Value + "\\" + task.Properties["File"].Value;

            this.filePath = X3Platform.Util.DirectoryHelper.FormatLocalPath(this.filePath);
        }

        public FileObserver(string path)
        {
            this.filePath = Directory.GetCurrentDirectory() + "\\" + path;

            this.filePath = X3Platform.Util.DirectoryHelper.FormatLocalPath(this.filePath);
        }

        public void Generate(string value, string path)
        {
            if (string.IsNullOrEmpty(value))
                return;

            // 保证保存路径的文件夹一定存在

            string directoryPath = Path.GetDirectoryName(this.filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(path, value);
        }

        public void Generate(string value)
        {
            this.Generate(value, this.filePath);
        }
    }
}
