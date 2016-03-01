#region Using Testing Libraries
#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestContext = System.Object;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

#endregion

namespace X3Platform.CodeBuilder.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using X3Platform.CodeBuilder.Data;
    using System.Data;
    using X3Platform.CodeBuilder.Data.DbSchemaProviders;

    /// <summary></summary>
    [TestClass]
    public class MySqlProviderTests
    {
        /// <summary>数据库连接字符串</summary>
        private string connectionString = "server=localhost;database=examples;uid=test;pwd=test";

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestGetTable()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetTable("examples", string.Empty, "tb_News");

            Assert.AreEqual("Id", table.Columns[0].Name);
            Assert.AreEqual(DbType.String, table.Columns[0].Type);
        }

        [TestMethod]
        public void TestGetPrimaryKeyColumns()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetPrimaryKeyColumns("examples", string.Empty, "tb_News");
        }

        [TestMethod]
        public void TestGetForeignKeyColumns()
        {
            MySqlSchemaProvider provider = new MySqlSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetForeignKeyColumns("examples", string.Empty, "tb_News");
        }
    }
}
