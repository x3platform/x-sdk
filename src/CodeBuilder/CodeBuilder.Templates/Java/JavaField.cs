using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.Java
{
    /// <summary>�ֶ���Ϣ</summary>
    public class JavaField
    {
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_NameFirstUpperCase;

        /// <summary>��������ĸ��д</summary>
        public string NameFirstUpperCase
        {
            get { return m_NameFirstUpperCase; }
            set { m_NameFirstUpperCase = value; }
        }

        private string m_DataColumnName;

        /// <summary>����������</summary>
        public string DataColumnName
        {
            get { return this.m_DataColumnName; }
            set { this.m_DataColumnName = value; }
        }

        private string m_Type;
        /// <summary>
        /// ��������
        /// </summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }

        private string m_JdbcType;
        /// <summary>
        /// JDBC ��������
        /// </summary>
        public string JdbcType
        {
            get { return this.m_JdbcType; }
            set { this.m_JdbcType = value; }
        }
        
        private string m_DefaultValue;

        /// <summary>Ĭ��ֵ</summary>
        public string DefaultValue
        {
            get { return this.m_DefaultValue; }
            set { this.m_DefaultValue = value; }
        }


        private bool m_Nullable;
        /// <summary>
        /// �����ֵ
        /// </summary>
        public bool Nullable
        {
            get { return m_Nullable; }
            set { m_Nullable = value; }
        }

        private int m_Length;

        /// <summary>
        /// ����
        /// </summary>
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        private byte m_Precision;
        /// <summary>
        /// ����
        /// </summary>
        public byte Precision
        {
            get { return m_Precision; }
            set { m_Precision = value; }
        }

        private int m_Scale;
        /// <summary>
        /// ����
        /// </summary>
        public int Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        private string m_Description;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
    }
}
