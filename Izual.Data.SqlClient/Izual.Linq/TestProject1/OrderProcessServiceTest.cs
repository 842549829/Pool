using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Order.Domain;
using Xie;

namespace TestProject1
{
    
    
    /// <summary>
    ///这是 OrderProcessServiceTest 的测试类，旨在
    ///包含所有 OrderProcessServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class OrderProcessServiceTest
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

        static OrderProcessServiceTest() {
            MockOption.SetReturnsDefault(100);
            MockOption.SetReturnsDefault("测试数据");
            MockOption.SetReturnsDefault(DateTime.Now);
            MockOption.SetReturnsDefault<DateTime?>(DateTime.Now);
            MockOption.SetReturnsDefault(true);
            MockOption.SetReturnsDefault(Guid.Empty);
            MockOption.SetReturnsDefault(CompanyType.Purchaser);
            MockOption.SetReturnsDefault(RefundType.SpecialReason);
            MockOption.SetReturnsDefault(RefundApplyformStatus.DeniedByProviderBusiness);
            MockOption.SetReturnsDefault(ApplyformProcessStatus.Processing);
            MockOption.SetReturnsDefault<NormalRefundBill>(null);
            MockOption.SetReturnsDefault<decimal?>(99);
            MockOption.SetReturnsDefault(98m);
            MockOption.SetReturnsDefault(ProductType.Promotion);
            MockOption.SetReturnsDefault(new PNRPair("aaaa","Bbbb"));
            MockOption.SetReturnsDefault(PassengerType.Adult);
            MockOption.SetReturnsDefault(CredentialsType.身份证);
            MockOption.SetReturnsDefault(OrderSource.PlatformOrder);
            MockOption.SetReturnsDefault(Gender.Female);
            MockOption.SetReturnsDefault<IEnumerable<Guid>>(new Guid[]{Guid.Empty});
        }

        /// <summary>
        ///ProduceOrder 的测试
        ///</summary>
        [TestMethod()]
        public void ProduceOrderTest()
        {
            var flight = new MockX<FlightView>();
            flight.SetupAllProperties();
            MockOption.SetReturnsDefault(flight.Object);
            MockOption.SetReturnsDefault<IEnumerable<FlightView>>(new List<FlightView>(){flight.Object,flight.Object,flight.Object});
            var passenger = new MockX<PassengerView>();
            passenger.SetupAllProperties();
            MockOption.SetReturnsDefault(passenger.Object);
            MockOption.SetReturnsDefault<IEnumerable<PassengerView>>(new List<PassengerView>()
                {
                    passenger.Object,passenger.Object,passenger.Object
                });
            var contract = new MockX<Contact>();
            contract.SetupAllProperties();
            MockOption.SetReturnsDefault(contract.Object);

            var role = new MockX<PermissionRoleInfo>();
            role.SetupAllProperties();
            MockOption.SetReturnsDefault(role);
            MockOption.SetReturnsDefault <IEnumerable<PermissionRoleInfo>>(new List<PermissionRoleInfo>()
                {
                    role.Object,role.Object
                });


            var ovP = new MockX<OrderView>();
            ovP.SetupAllProperties();
            var emp = new MockX<EmployeeDetailInfo>();
            emp.SetupAllProperties();

            OrderView orderView = ovP.Object;
            Guid publisher = new Guid("74ae6725-654e-444f-abcc-29b1568db845");
            Guid policy = new Guid(); // TODO: 初始化为适当的值
            EmployeeDetailInfo employee = emp.Object;
            Order expected = null; // TODO: 初始化为适当的值
            Order actual;
            actual = OrderProcessService.ProduceOrder(orderView, publisher, policy, employee);
            Assert.AreNotEqual(expected, actual);
            
        }
    }
}
