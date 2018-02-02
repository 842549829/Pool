using ChinaPay.B3B.Service.Organization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChinaPay.B3B.Data.DataMapping;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///这是 CompanyServiceTest 的测试类，旨在
    ///包含所有 CompanyServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class CompanyServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///CreateCompanyGroup 的测试
        ///</summary>
        [TestMethod()]
        public void CreateCompanyGroupTest()
        {
            Guid ownerId = new Guid("67007e18-a8ee-4ce0-b860-ae81ddd2a4f3"); // TODO: 初始化为适当的值
            CompanyGroup group = new CompanyGroup()
                {
                    Id = Guid.NewGuid(),
                    Name = "复仇者联盟",
                    Description = "都是英雄",
                    AllowExternalPurchase = true,
                    Company = ownerId,
                    CreateTime = DateTime.Now,
                    LastModifyTime = DateTime.Now,
                    Creator = "lanlan"
                };
            IEnumerable<Guid> members = new List<Guid>(); 
            IEnumerable<CompanyGroupLimitation> limitations = new List<CompanyGroupLimitation>();
            string creator = "lanlan";
            bool expected = true; 
            bool actual;
            actual = CompanyService.CreateCompanyGroup(ownerId, group, members, limitations, creator);
            Assert.AreEqual(expected, actual);
        }
    }
}
