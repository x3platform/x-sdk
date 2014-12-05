namespace X3Platform.DataDump
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.DataDump.Configuration;

    public interface IDataDumpProvider
    {
        /// <summary>��ʼ������</summary>
        /// <param name="task">������Ϣ</param>
        void Init(DataDumpTask task);

        /// <summary>���ɽű�</summary>
        string Generate();
    }
}
