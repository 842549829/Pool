using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Order.Repository.SqlServer;
using Izual;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.DataAccess;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace TestExternalOrder
{
    
    
    /// <summary>
    ///这是 OrderRepositoryTest 的测试类，旨在
    ///包含所有 OrderRepositoryTest 单元测试
    ///</summary>
    [TestClass()]
    public class OrderRepositoryTest
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
        ///SaveExternalOrderInfo 的测试

        /// <summary>
        ///SaveExternalOrderInfo 的测试
        ///</summary>
        [TestMethod()]
        public void SaveExternalOrderInfoTest1()
        {
            OrderRepository target = new OrderRepository(dbOperator);
            ExternalOrder externalOrder = new ExternalOrder(20131465 + RandomNum)
            {
                ExternalOrderId = "201314",
                ECommission = 0.2m,
                PayStatus = PayStatus.NoPay,
                PayTime = DateTime.Now,
                PayTradNO = "11111111",
                Platform = PlatformType.Yeexing,
                IsAutoPay = true,
                Amount = 10000
            };
            var acturl = target.SaveExternalOrderInfo(externalOrder);
            Assert.AreEqual(acturl, true);
        }

        string provider ="System.Data.SqlClient";
        string connstring = "server=192.168.1.253;database=B3B;uid=sa;password=123456";
        public int RandomNum
        {
            get {
                return (new Random()).Next(1, 100000);
            }
        }

        private DbOperator dbOperator { get {return new DbOperator(provider, connstring);}
        }

        /// <summary>
        ///SaveExternalPolicyCopy 的测试
        ///</summary>
        [TestMethod()]
        public void SaveExternalPolicyCopyTest()
        {
            ExternalPolicyView testPolicy = new ExternalPolicyView()
            {
                Platform = PlatformType.Yeexing,
                Provider = new Guid("4BA6539C-9072-4DFB-AC6E-67CAD72FEBC4"),
                Id = "201314"+(new Random()).Next(1,10000).ToString(),
                TicketType = TicketType.B2B,
                RequireChangePNR = false,
                Condition = "11111",
                ETDZSpeed = 5,
                WorkStart = new Time(0,0,0),
                WorkEnd = new Time(23, 59, 0),
                ScrapStart = new Time(0, 0, 0),
                ScrapEnd = new Time(23, 59, 0),
                OriginalRebate = 0.12m,
                Rebate = 0.1m,
                ParValue = 2000m,
                OfficeNo = "KMG216",
                RequireAuth = false
            };



            OrderRepository target = new OrderRepository(dbOperator); 
            bool expected = true;
            bool actual;
            actual = target.SaveExternalPolicyCopy(testPolicy,0);
            Assert.AreEqual(expected, actual);
        }
    }
}
