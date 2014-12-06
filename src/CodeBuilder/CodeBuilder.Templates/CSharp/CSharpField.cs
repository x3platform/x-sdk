using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using X3Platform.CodeBuilder.Data;

namespace X3Platform.CodeBuilder.Templates.CSharp
{
    /// <summary>�ֶ���Ϣ</summary>
    public class CSharpField
    {
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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


        private string m_DefaultValue;

        /// <summary>Ĭ��ֵ</summary>
        public string DefaultValue
        {
            get { return this.m_DefaultValue; }
            set { this.m_DefaultValue = value; }
        }


        private bool m_IsNull;
        /// <summary>
        /// �����ֵ
        /// </summary>
        public bool IsNull
        {
            get { return m_IsNull; }
            set { m_IsNull = value; }
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
