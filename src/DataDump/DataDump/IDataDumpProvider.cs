namespace X3Platform.DataDump
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.DataDump.Configuration;

    public interface IDataDumpProvider
    {
        /// <summary>初始化配置</summary>
        /// <param name="task">任务信息</param>
        void Init(DataDumpTask task);

        /// <summary>生成脚本</summary>
        string Generate();
    }
}
