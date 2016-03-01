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

namespace X3Platform.Apps.Tests
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
    public class OracleProviderTests
    {
        /// <summary>数据库连接字符串</summary>
        private string connectionString = "Data Source=NXTWAPDEV;User Id=nxtwap;Password=nxtwap123;";

        //-------------------------------------------------------
        // 测试内容
        //-------------------------------------------------------

        [TestMethod]
        public void TestGetTable()
        {
            OracleSchemaProvider provider = new OracleSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetTable(string.Empty, string.Empty, "TWAP_D_ARTICLE");

            Assert.AreEqual("Id", table.Columns[0].Name);
            Assert.AreEqual(DbType.String, table.Columns[0].Type);
        }

        [TestMethod]
        public void TestGetPrimaryKeyColumns()
        {
            OracleSchemaProvider provider = new OracleSchemaProvider();

            provider.ConnectionString = connectionString;

            var table = provider.GetPrimaryKeyColumns(string.Empty, string.Empty, "tb_News");
        }
    }
}
