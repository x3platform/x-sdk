namespace X3Platform.CodeBuilder.Tests.Data
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    
    using NUnit.Framework;

    using X3Platform.CodeBuilder.Data;
    using X3Platform.CodeBuilder.Data.DbSchemaProviders;

    /// <summary></summary>
    [TestFixture]
    public class OracleProviderTests
    {
        /// <summary>数据库连接字符串</summary>
        private string connectionString = "Data Source=localhost;User Id=test;Password=test;";

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [Test]
        [Category("ManualTesting")]
        public void TestGetTable()
        {
            OracleSchemaProvider provider = new OracleSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetTable(string.Empty, string.Empty, "TB_ARTICLE");

            Assert.AreEqual("Id", table.Columns[0].Name);
            Assert.AreEqual(DbType.String, table.Columns[0].Type);
        }

        [Test]
        [Category("ManualTesting")]
        public void TestGetPrimaryKeyColumns()
        {
            OracleSchemaProvider provider = new OracleSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetPrimaryKeyColumns(string.Empty, string.Empty, "tb_News");
        }
    }
}
