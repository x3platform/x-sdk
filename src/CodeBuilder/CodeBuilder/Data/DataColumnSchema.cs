using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace X3Platform.CodeBuilder.Data
{
    /// <summary>
    /// ������ժҪ
    /// </summary>
    public class DataColumnSchema
    {
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private bool m_PrimaryKey;

        /// <summary>����</summary>
        public bool PrimaryKey
        {
            get { return m_PrimaryKey; }
            set { m_PrimaryKey = value; }
        }

        private DbType m_Type;

        /// <summary>��������</summary>
        public DbType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private bool m_Nullable;

        /// <summary>�����ֵ</summary>
        public bool Nullable
        {
            get { return m_Nullable; }
            set { m_Nullable = value; }
        }

        private int m_Length;

        /// <summary>����</summary>
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        private byte m_Precision;

        /// <summary>����</summary>
        public byte Precision
        {
            get { return m_Precision; }
            set { m_Precision = value; }
        }

        private int m_Scale;

        /// <summary>����</summary>
        public int Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        private string m_Description;

        /// <summary>������Ϣ</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
    }
}
