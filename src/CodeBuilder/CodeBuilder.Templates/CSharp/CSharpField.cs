using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    /// <summary>字段信息</summary>
    public class CSharpField
    {
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Type;
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }


        private string m_DefaultValue;

        /// <summary>默认值</summary>
        public string DefaultValue
        {
            get { return this.m_DefaultValue; }
            set { this.m_DefaultValue = value; }
        }


        private bool m_IsNull;
        /// <summary>
        /// 允许空值
        /// </summary>
        public bool IsNull
        {
            get { return m_IsNull; }
            set { m_IsNull = value; }
        }

        private int m_Length;

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        private byte m_Precision;
        /// <summary>
        /// 精度
        /// </summary>
        public byte Precision
        {
            get { return m_Precision; }
            set { m_Precision = value; }
        }

        private int m_Scale;
        /// <summary>
        /// 比例
        /// </summary>
        public int Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        private string m_Description;
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
    }
}
