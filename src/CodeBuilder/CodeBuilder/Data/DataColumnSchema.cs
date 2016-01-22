using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace X3Platform.CodeBuilder.Data
{
    /// <summary>
    /// 数据列摘要
    /// </summary>
    public class DataColumnSchema
    {
        private string m_Name = string.Empty;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private bool m_PrimaryKey;

        /// <summary>主键</summary>
        public bool PrimaryKey
        {
            get { return m_PrimaryKey; }
            set { m_PrimaryKey = value; }
        }

        private bool m_ForeignKey;

        /// <summary>外键</summary>
        public bool ForeignKey
        {
            get { return this.m_ForeignKey; }
            set { this.m_ForeignKey = value; }
        }

        private DbType m_Type;

        /// <summary>数据类型</summary>
        public DbType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_NativeType;
        
        /// <summary>原生数据类型</summary>
        public string NativeType
        {
            get { return this.m_NativeType; }
            set { this.m_NativeType = value; }
        }

        private bool m_Nullable;

        /// <summary>允许空值</summary>
        public bool Nullable
        {
            get { return m_Nullable; }
            set { m_Nullable = value; }
        }

        private int m_Length;

        /// <summary>长度</summary>
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        private byte m_Precision;

        /// <summary>精度</summary>
        public byte Precision
        {
            get { return m_Precision; }
            set { m_Precision = value; }
        }

        private int m_Scale;

        /// <summary>比例</summary>
        public int Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }
        
        private string m_DefaultValue = string.Empty;

        /// <summary>默认值信息</summary>
        public string DefaultValue
        {
            get { return this.m_DefaultValue; }
            set { this.m_DefaultValue = value; }
        }

        private string m_Description = string.Empty;

        /// <summary>描述信息</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
    }
}
