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
    public class SQLiteProviderTests
    {
        /// <summary>数据库连接字符串</summary>
        private string connectionString = "server=localhost;database=examples;uid=test;pwd=test";

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------
        [Test]
        [Category("ManualTesting")]
        public void TestGetTable()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetTable("examples", string.Empty, "tb_News");

            Assert.AreEqual("Id", table.Columns[0].Name);
            Assert.AreEqual(DbType.String, table.Columns[0].Type);
        }

        [Test]
        [Category("ManualTesting")]
        public void TestGetPrimaryKeyColumns()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetPrimaryKeyColumns("examples", string.Empty, "tb_News");
        }

        [Test]
        [Category("ManualTesting")]
        public void TestGetForeignKeyColumns()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetForeignKeyColumns("examples", string.Empty, "tb_News");
        }
    }
}
