using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using System.Data;
using ChinaPay.B3B.DataTransferObject.Order.External;
using Time = Izual.Time;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer
{
    class OrderRepository : SqlServerTransaction, IOrderRepository
    {
        public OrderRepository(DbOperator dbOperator)
            : base(dbOperator)
        {
        }
        public void InsertOrder(Domain.Order order)
        {
            saveOrderMainInfo(order);
            savePNRContents(order.Id, order.PNRInfos);
            if (order is ExternalOrder)
            {
                SaveExternalOrderInfo(order as ExternalOrder);
            }
        }

        public bool SaveExternalOrderInfo(ExternalOrder externalOrder)
        {
            ClearParameters();
            string sql = "INSERT INTO [B3B].[dbo].[T_ExternalOrder] ([OrderId],[ExternalOrderId],[ECommission],[PayStatus] ,[IsAutoPay],[PlatformType],[Amount],[PayTime]) VALUES(@OrderId,@ExternalOrderId,@ECommission,@PayStatus,@IsAutoPay,@PlatformType,@Amount,@PayTime)";
            AddParameter("ORDERID", externalOrder.Id);
            AddParameter("ExternalOrderId", externalOrder.ExternalOrderId);
            AddParameter("ECommission", externalOrder.ECommission);
            AddParameter("PayStatus", (byte)externalOrder.PayStatus);
            AddParameter("IsAutoPay", externalOrder.IsAutoPay);
            AddParameter("PlatformType", externalOrder.Platform);
            if (externalOrder.PayTime.HasValue)
            {
                AddParameter("PayTime", externalOrder.PayTime.Value);
            }
            else
            {
                AddParameter("PayTime", DBNull.Value);
            }
            if (externalOrder.Amount.HasValue)
            {
                AddParameter("Amount", externalOrder.Amount.Value);
            }
            else
            {
                AddParameter("Amount", DBNull.Value);
            }
            return ExecuteNonQuery(sql) > 0;
        }

        public void UpdateStatus(decimal orderId, OrderStatus status, string remark)
        {
            ClearParameters();
            string sql = null;
            if (OrderStatus.DeniedWithETDZ == status || OrderStatus.DeniedWithSupply == status || OrderStatus.ConfirmFailed == status)
            {
                sql = "UPDATE [dbo].[T_Order] SET [Status]=@STATUS,[Remark]=@REMARK,RefuseETDZTime=@RefuseETDZTime,IsNeedReminded=@IsNeedReminded WHERE [Id]=@ORDERID";
                AddParameter("ORDERID", orderId);
                AddParameter("IsNeedReminded",false);
                AddParameter("STATUS", (int)status);
                AddParameter("REMARK", remark ?? string.Empty);
                AddParameter("RefuseETDZTime", DateTime.Now);
                ExecuteNonQuery(sql);
            }
            else
            {
                sql = "UPDATE [dbo].[T_Order] SET [Status]=@STATUS,[Remark]=@REMARK WHERE [Id]=@ORDERID";
                AddParameter("ORDERID", orderId);
                AddParameter("STATUS", (int)status);
                AddParameter("REMARK", remark ?? string.Empty);
                ExecuteNonQuery(sql);
            }
        }
        public void UpdateOrderForSupplyResource(Order.Domain.Order order)
        {
            saveSupplyTime(order);
            updatePNRForSupplyResource(order);
            updateTicketPrice(order);
            updateFlights(order);
            if (order.IsThirdRelation)
            {
                deleteProviderInfo(order.Id);
                saveProviderInfo(order.Id, order.Provider);
                updateOrderMainInfoForTradeRoles(order);
            }
        }
        public void UpdateOrderForProviderChanged(Order.Domain.Order order)
        {
            updateOrderMainInfoForTradeRoles(order);
            deleteProviderInfo(order.Id);
            saveProviderInfo(order.Id, order.Provider);
        }
        public void UpdateOrderForPaySuccess(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Order] SET [Status]=@STATUS,[PayTime]=@PAYTIME,[Remark]=@REMARK,VisibleRole=@VisibleRole WHERE [Id]=@ORDERID";
            AddParameter("ORDERID", order.Id);
            AddParameter("STATUS", (int)order.Status);
            AddParameter("PAYTIME", order.Purchaser.PayTime.Value);
            AddParameter("REMARK", order.Remark ?? string.Empty);
            AddParameter("VisibleRole", (byte)order.VisibleRole);
            ExecuteNonQuery(sql);
        }
        public void UpdateOrderForETDZ(Order.Domain.Order order)
        {
            saveETDZInfo(order);
            updatePNRForETDZ(order);
            updateTicketNos(order);
        }
        public void UpdateOrderForReviseReleasedFare(Order.Domain.Order order)
        {
            updateOrderMainInfoForReviseReleasedFare(order);
            updateFlights(order);
            updateTicketPrice(order);
        }
        public void UpdateOrderForReviseFare(Order.Domain.Order order)
        {
            updateOrderMainInfoForReviseFare(order);
            updateFlights(order);
            updateTicketPrice(order);
        }
        public void UpdateOrderForApplyform(Order.Domain.Order order)
        {
            cancelPassengers(order.Id);
            cancelFlights(order.Id);
            resetPassengersRelation(order);
            resetFlights(order);
            saveNewPNRMainInfos(order);
        }
        public void UpdateCredentitals(Guid passengerId, string credentials)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Passenger] SET [Credentials]=@CREDENTIALS WHERE [Id]=@PASSENGERID";
            AddParameter("PASSENGERID", passengerId);
            AddParameter("CREDENTIALS", credentials);
            ExecuteNonQuery(sql);
        }
        public void UpdateTicketNo(Guid passengerId, int serial, string ticketNo, string settleCode)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Ticket] SET [TicketNo]=@TICKETNO,[SettleCode]=@SettleCode WHERE [Passenger]=@PASSENGERID AND [Serial]=@SERIAL";
            AddParameter("PASSENGERID", passengerId);
            AddParameter("SERIAL", serial);
            AddParameter("TICKETNO", ticketNo);
            AddParameter("SettleCode", settleCode);
            ExecuteNonQuery(sql);
        }
        public Guid QueryReservationFlightId(Guid flightId)
        {
            ClearParameters();
            var sql = "WITH rec AS(SELECT Id,AssociateFlight FROM dbo.T_Flight WHERE Id=@FlightId " +
                    "UNION ALL SELECT a.Id,a.AssociateFlight FROM dbo.T_Flight a INNER JOIN rec b ON b.AssociateFlight=a.Id)" +
                    "SELECT Id FROM rec WHERE AssociateFlight IS NULL";
            AddParameter("FlightId", flightId);
            var obj = ExecuteScalar(sql);
            if (obj == null || obj == DBNull.Value) return flightId;
            return (Guid)obj;
        }

        Guid saveOrderContactInfo(Contact contact)
        {
            ClearParameters();
            var sql = "INSERT INTO [dbo].[T_OrderContact] ([Id],[Name],[Mobile],[Email]) VALUES (@CONTACTID,@NAME,@MOBILE,@EMAIL);";
            var contactId = Guid.NewGuid();
            AddParameter("CONTACTID", contactId);
            AddParameter("NAME", contact.Name ?? string.Empty);
            AddParameter("MOBILE", contact.Mobile ?? string.Empty);
            AddParameter("EMAIL", contact.Email ?? string.Empty);
            ExecuteNonQuery(sql);
            return contactId;
        }
        void saveOrderMainInfo(Domain.Order order)
        {
            var contactId = saveOrderContactInfo(order.Contact);
            saveOrderMainInfo(order, contactId);
            if (order.Supplier != null)
            {
                saveSupplierInfo(order.Id, order.Supplier);
            }
            if (order.Provider != null)
            {
                saveProviderInfo(order.Id, order.Provider);
            }
        }
        void saveOrderMainInfo(Order.Domain.Order order, Guid contact)
        {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.T_Order");
            sql.Append("(Id,[Status],[Source],ProductType,TripType,Contact,Purchaser,PurchaserName,PurchaserRebate,PurchaserCommission,PurchaserAmount,");
            sql.Append("Provider,ProviderName,ProviderRebate,ProviderCommission,ProviderAmount,OfficeNo,ETDZMode,Supplier,SupplierName,");
            sql.Append("SupplierRebate,SupplierCommission,SupplierAmount,ProducedTime,ProducedAccount,ProducedAccountName,BPNR,PNR,AssociateOrder,AssociatePNR,AssociateBPNR,");
            sql.Append("Remark,RevisedFare,IsReduce,Choise,CustomNo,IsCustomerResource,IsTeam,IsStandby,VisibleRole,IsB3BOrder,ForbidChangPNR,OEMID,NeedAUTH)");
            sql.Append(" VALUES ");
            sql.Append("(@ORDERID,@STATUS,@SOURCE,@PRODUCTTYPE,@TRIPTYPE,@CONTACT,@PURCHASER,@PURCHASERNAME,@PURCHASERREBATE,@PURCHASERCOMMISSION,@PURCHASERAMOUNT,");
            sql.Append("@PROVIDER,@PROVIDERNAME,@PROVIDERREBATE,@PROVIDERCOMMISSION,@PROVIDERAMOUNT,@OFFICENO,@ETDZMODE,@SUPPLIER,@SUPPLIERNAME,");
            sql.Append("@SUPPLIERREBATE,@SUPPLIERCOMMISSION,@SUPPLIERAMOUNT,@PRODUCEDTIME,@PRODUCEDACCOUNT,@ProducedAccountName,@BPNR,@PNR,@ASSOCIATEORDER,@AssociatePNR,@AssociateBPNR,");
            sql.Append("@REMARK,@REVISEDFARE,@ISREDUCE,@Choise,@CustomNo,@IsCustomerResource,@IsTeam,@IsStandby,@VisibleRole,@IsB3BOrder,@ForbidChangPNR,@OEMID,@NeedAUTH)");

            AddParameter("ORDERID", order.Id);
            AddParameter("STATUS", (int)order.Status);
            AddParameter("SOURCE", (byte)order.Source);
            AddParameter("PRODUCTTYPE", (byte)order.Product.ProductType);
            AddParameter("TRIPTYPE", (byte)order.TripType);
            AddParameter("CONTACT", contact);
            AddParameter("PURCHASER", order.Purchaser.CompanyId);
            AddParameter("PURCHASERNAME", order.Purchaser.Name);
            AddParameter("PURCHASERREBATE", order.Purchaser.Rebate);
            AddParameter("PURCHASERCOMMISSION", order.Purchaser.Commission);
            AddParameter("PURCHASERAMOUNT", order.Purchaser.Amount);
            AddParameter("IsB3BOrder", order.IsB3BOrder);
            AddParameter("NeedAUTH",(bool)order.NeedAUTH);
            if (order.OEMID.HasValue)
            {
                AddParameter("OEMID", order.OEMID.Value);
            }
            else
            {
                AddParameter("OEMID", DBNull.Value);
            }
            if (order.Provider == null)
            {
                AddParameter("PROVIDER", DBNull.Value);
                AddParameter("PROVIDERNAME", DBNull.Value);
                AddParameter("PROVIDERREBATE", DBNull.Value);
                AddParameter("PROVIDERCOMMISSION", DBNull.Value);
                AddParameter("PROVIDERAMOUNT", DBNull.Value);
                AddParameter("ETDZMODE", DBNull.Value);
            }
            else
            {
                AddParameter("PROVIDER", order.Provider.CompanyId);
                AddParameter("PROVIDERNAME", order.Provider.Name);
                AddParameter("PROVIDERREBATE", order.Provider.Rebate);
                AddParameter("PROVIDERCOMMISSION", order.Provider.Commission);
                AddParameter("PROVIDERAMOUNT", order.Provider.Amount);
                if (order.Provider.Product is Order.Domain.CommonProductInfo)
                {
                    var commonProductInfo = order.Provider.Product as Order.Domain.CommonProductInfo;
                    AddParameter("ETDZMODE", (byte)commonProductInfo.ETDZMode);
                }
                else
                {
                    AddParameter("ETDZMODE", DBNull.Value);
                }
            }
            if (string.IsNullOrWhiteSpace(order.Product.OfficeNo))
            {
                AddParameter("OFFICENO", DBNull.Value);
            }
            else
            {
                AddParameter("OFFICENO", order.Product.OfficeNo.Trim());
            }
            if (order.Supplier == null)
            {
                AddParameter("SUPPLIER", DBNull.Value);
                AddParameter("SUPPLIERNAME", DBNull.Value);
                AddParameter("SUPPLIERREBATE", DBNull.Value);
                AddParameter("SUPPLIERCOMMISSION", DBNull.Value);
                AddParameter("SUPPLIERAMOUNT", DBNull.Value);
            }
            else
            {
                AddParameter("SUPPLIER", order.Supplier.CompanyId);
                AddParameter("SUPPLIERNAME", order.Supplier.Name);
                AddParameter("SUPPLIERREBATE", order.Supplier.Rebate);
                AddParameter("SUPPLIERCOMMISSION", order.Supplier.Commission);
                AddParameter("SUPPLIERAMOUNT", order.Supplier.Amount);
            }
            AddParameter("PRODUCEDTIME", order.Purchaser.ProducedTime);
            AddParameter("PRODUCEDACCOUNT", order.Purchaser.OperatorAccount);
            AddParameter("ProducedAccountName", order.Purchaser.OperatorName);
            if (order.ReservationPNR == null)
            {
                AddParameter("BPNR", DBNull.Value);
                AddParameter("PNR", DBNull.Value);
            }
            else
            {
                AddParameter("BPNR", order.ReservationPNR.BPNR ?? string.Empty);
                AddParameter("PNR", order.ReservationPNR.PNR ?? string.Empty);
            }
            if (order.AssociateOrderId.HasValue)
            {
                AddParameter("ASSOCIATEORDER", order.AssociateOrderId.Value);
            }
            else
            {
                AddParameter("ASSOCIATEORDER", DBNull.Value);
            }
            if (order.AssociatePNR == null)
            {
                AddParameter("AssociatePNR", DBNull.Value);
                AddParameter("AssociateBPNR", DBNull.Value);
            }
            else
            {
                AddParameter("AssociatePNR", order.AssociatePNR.PNR ?? string.Empty);
                AddParameter("AssociateBPNR", order.AssociatePNR.BPNR ?? string.Empty);
            }
            AddParameter("REMARK", order.Remark ?? string.Empty);
            AddParameter("REVISEDFARE", order.RevisedPrice);
            AddParameter("ISREDUCE", order.IsReduce);
            AddParameter("Choise", order.Choise);
            AddParameter("CustomNo", order.CustomNo ?? string.Empty);
            AddParameter("IsCustomerResource", order.IsCustomerResource);
            AddParameter("IsTeam", order.IsTeam);
            AddParameter("IsStandby", order.IsStandby);
            AddParameter("VisibleRole", (byte)order.VisibleRole);
            AddParameter("ForbidChangPNR", order.ForbidChangPNR);
            ExecuteNonQuery(sql);
        }
        void updateOrderMainInfoForReviseFare(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE dbo.T_Order SET RevisedFare=@REVISEDFARE WHERE Id=@ORDERID";
            AddParameter("ORDERID", order.Id);
            AddParameter("REVISEDFARE", order.RevisedPrice);
            ExecuteNonQuery(sql);
        }
        void updateOrderMainInfoForReviseReleasedFare(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE dbo.T_Order SET PurchaserAmount=@PURCHASERAMOUNT,PurchaserCommission=@PURCHASERCOMMISSION,RevisedFare=@REVISEDFARE WHERE Id=@ORDERID";
            AddParameter("ORDERID", order.Id);
            AddParameter("PURCHASERAMOUNT", order.Purchaser.Amount);
            AddParameter("PURCHASERCOMMISSION", order.Purchaser.Commission);
            AddParameter("REVISEDFARE", order.RevisedPrice);
            ExecuteNonQuery(sql);
        }
        void updateOrderMainInfoForTradeRoles(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Order] SET [Status]=@STATUS,[Remark]=@REMARK,[SupplierRebate]=@SUPPLIEREBATE,[SupplierCommission]=@SUPPLIERCOMMISSION,[SupplierAmount]=@SUPPLIERAMOUNT," +
                "[Provider]=@PROVIDER,[ProviderName]=@PROVIDERNAME,[ProviderRebate]=@PROVIDERREBATE,[ProviderCommission]=@PROVIDERCOMMISSION,[ProviderAmount]=@PROVIDERAMOUNT,OfficeNo=@OFFICENO,ETDZMode=@ETDZMODE,CustomNO=@CustomNO,ForbidChangPNR=@ForbidChangPNR,IsB3BOrder=@IsB3BOrder, NeedAUTH =@NeedAUTH" +
                " WHERE [Id]=@ORDERID";
            AddParameter("ORDERID", order.Id);
            AddParameter("STATUS", (int)order.Status);
            AddParameter("REMARK", order.Remark ?? string.Empty);
            AddParameter("NeedAUTH",(bool)order.NeedAUTH);
            if (order.Supplier == null)
            {
                AddParameter("SUPPLIEREBATE", DBNull.Value);
                AddParameter("SUPPLIERCOMMISSION", DBNull.Value);
                AddParameter("SUPPLIERAMOUNT", DBNull.Value);
            }
            else
            {
                AddParameter("SUPPLIEREBATE", order.Supplier.Rebate);
                AddParameter("SUPPLIERCOMMISSION", order.Supplier.Commission);
                AddParameter("SUPPLIERAMOUNT", order.Supplier.Amount);
            }
            AddParameter("PROVIDER", order.Provider.CompanyId);
            AddParameter("PROVIDERNAME", order.Provider.Name);
            AddParameter("PROVIDERREBATE", order.Provider.Rebate);
            AddParameter("PROVIDERCOMMISSION", order.Provider.Commission);
            AddParameter("PROVIDERAMOUNT", order.Provider.Amount);
            AddParameter("CustomNO", order.CustomNo ?? string.Empty);
            AddParameter("ForbidChangPNR", order.ForbidChangPNR);
            AddParameter("IsB3BOrder", order.IsB3BOrder);
            if (order.Provider == null || string.IsNullOrWhiteSpace(order.Provider.Product.OfficeNo))
            {
                AddParameter("OFFICENO", DBNull.Value);
            }
            else
            {
                AddParameter("OFFICENO", order.Provider.Product.OfficeNo.Trim());
            }
            if (order.Provider.Product is Order.Domain.SpeicalProductInfo)
            {
                AddParameter("ETDZMODE", DBNull.Value);
            }
            else
            {
                var commonProductInfo = order.Provider.Product as Order.Domain.CommonProductInfo;
                AddParameter("ETDZMODE", (byte)commonProductInfo.ETDZMode);
            }
            ExecuteNonQuery(sql);
        }
        void updateOrderForPNR(decimal orderId, PNRPair pnr, bool isReservationPNR)
        {
            ClearParameters();
            string sql = null;
            if (isReservationPNR)
            {
                sql = "UPDATE [dbo].[T_Order] SET [BPNR]=@BPNR,[PNR]=@PNR WHERE [Id]=@ORDERID;";
            }
            else
            {
                sql = "UPDATE [dbo].[T_Order] SET [EBPNR]=@BPNR,[EPNR]=@PNR WHERE [Id]=@ORDERID;";
            }
            AddParameter("ORDERID", orderId);
            if (string.IsNullOrWhiteSpace(pnr.BPNR))
            {
                AddParameter("BPNR", DBNull.Value);
            }
            else
            {
                AddParameter("BPNR", pnr.BPNR);
            }
            if (string.IsNullOrWhiteSpace(pnr.PNR))
            {
                AddParameter("PNR", DBNull.Value);
            }
            else
            {
                AddParameter("PNR", pnr.PNR);
            }
            ExecuteNonQuery(sql);
        }
        void saveSupplyTime(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Order] SET [Status]=@STATUS,[SupplyTime]=@SUPPLYTIME,[Remark]=@REMARK,VisibleRole=@VisibleRole,IsEmergentOrder=@IsEmergentOrder WHERE [Id]=@ORDERID";
            AddParameter("ORDERID", order.Id);
            AddParameter("STATUS", (int)order.Status);
            AddParameter("SUPPLYTIME", order.SupplyTime.Value);
            AddParameter("REMARK", order.Remark ?? string.Empty);
            AddParameter("VisibleRole", (byte)order.VisibleRole);
            AddParameter("IsEmergentOrder", order.IsEmergentOrder);
            ExecuteNonQuery(sql);
        }
        void saveETDZInfo(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = "UPDATE dbo.T_Order SET [Status]=@Status,OfficeNo=@OfficeNo,[ETDZTime]=@ETDZTime,[Remark]=@Remark,[ETDZOperatorName]=@ETDZOperator," +
                "[ETDZOperatorAccount]=@ETDZOperatorAccount,IsEmergentOrder=@IsEmergentOrder WHERE [Id]=@OrderId;" +
                "UPDATE dbo.T_Provider SET OfficeNo=@OfficeNo,PolicyType=@TicketType WHERE OrderId=@OrderId;";
            AddParameter("OrderId", order.Id);
            AddParameter("Status", (int)order.Status);
            AddParameter("OfficeNo", order.Provider.Product.OfficeNo);
            AddParameter("TicketType", (byte)order.Provider.Product.TicketType);
            AddParameter("ETDZTime", order.ETDZTime.Value);
            AddParameter("Remark", order.Remark ?? string.Empty);
            AddParameter("ETDZOperator", order.Provider.OperatorName);
            AddParameter("ETDZOperatorAccount", order.Provider.OperatorAccount);
            AddParameter("IsEmergentOrder", order.IsEmergentOrder);
            ExecuteNonQuery(sql);
        }
        void deleteProviderInfo(decimal orderId)
        {
            ClearParameters();
            var sql = "DELETE FROM dbo.T_Provision WHERE EXISTS(SELECT NULL FROM dbo.T_Provider WHERE OrderId=@ORDERID AND Provision=[Id]);" +
                "DELETE FROM [dbo].[T_Provider] WHERE OrderId=@ORDERID;";
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql);
        }
        void deleteSupplierInfo(decimal orderId)
        {
            ClearParameters();
            var sql = "DELETE FROM [dbo].[T_Provision] WHERE EXISTS(SELECT NULL FROM [dbo].[T_Supplier] WHERE [OrderId]=@ORDERID AND [Provision]=[Id]);" +
                "DELETE FROM [dbo].[T_Supplier] WHERE [OrderId]=@ORDERID;";
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql);
        }
        void saveProviderInfo(decimal orderId, ProviderInfo provider)
        {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[T_Provider]");
            sql.Append("([OrderId],ProductType,[Policy],[PolicyType],[OfficeNo],[ETDZMode],[RequireChangePNR],[SpecialProductType],[RequireConfirm],[Remark],Condition,[Provision],[PurchaserRelationType],[IsDefaultPolicy])");
            sql.Append(" VALUES ");
            sql.Append("(@ORDERID,@ProductType,@POLICY,@POLICYTYPE,@OFFICENO,@ETDZMODE,@REQUIRECHANGEPNR,@SPECIALPRODUCTTYPE,@REQUIRECONFIRM,@POLICYREMARK,@Condition,@PROVIDERPROVISIONID,@PURCHASERRELATIONTYPE,@IsDefaultPolicy);");
            AddParameter("ORDERID", orderId);
            AddParameter("ProductType", (byte) provider.Product.ProductType);
            AddParameter("POLICY", provider.Product.Id);
            if (string.IsNullOrWhiteSpace(provider.Product.OfficeNo))
            {
                AddParameter("OFFICENO", DBNull.Value);
            }
            else
            {
                AddParameter("OFFICENO", provider.Product.OfficeNo.Trim());
            }
            if (provider.Product is Order.Domain.SpeicalProductInfo)
            {
                var specialProductInfo = provider.Product as Order.Domain.SpeicalProductInfo;
                AddParameter("ETDZMODE", DBNull.Value);
                AddParameter("REQUIRECHANGEPNR", DBNull.Value);
                AddParameter("SPECIALPRODUCTTYPE", (byte)specialProductInfo.SpeicalProductType);
                AddParameter("REQUIRECONFIRM", specialProductInfo.RequireConfirm);
            }
            else
            {
                var commonProductInfo = provider.Product as Order.Domain.CommonProductInfo;
                AddParameter("ETDZMODE", (byte)commonProductInfo.ETDZMode);
                AddParameter("REQUIRECHANGEPNR", commonProductInfo.RequireChangePNR);
                AddParameter("SPECIALPRODUCTTYPE", DBNull.Value);
                AddParameter("REQUIRECONFIRM", DBNull.Value);
            }

            AddParameter("IsDefaultPolicy", provider.Product.IsDefaultPolicy);
            AddParameter("POLICYTYPE", (byte)provider.Product.TicketType);
            AddParameter("POLICYREMARK", provider.Product.Remark ?? string.Empty);
            AddParameter("Condition", provider.Product.Condition ?? string.Empty);
            AddParameter("PURCHASERRELATIONTYPE", (byte)provider.PurchaserRelationType);
            if (provider.Product.RefundAndReschedulingProvision == null)
            {
                AddParameter("PROVIDERPROVISIONID", DBNull.Value);
            }
            else
            {
                sql.Append("INSERT INTO [dbo].[T_Provision]");
                sql.Append("([Id],[Refund],[Scrap],[Alteration],[Transfer])");
                sql.Append(" VALUES ");
                sql.Append("(@PROVIDERPROVISIONID,@PROVIDERREFUND,@PROVIDERSCRAP,@PROVIDERALTERATION,@PROVIDERTRANSFER);");
                AddParameter("PROVIDERPROVISIONID", Guid.NewGuid());
                AddParameter("PROVIDERREFUND", provider.Product.RefundAndReschedulingProvision.Refund ?? string.Empty);
                AddParameter("PROVIDERSCRAP", provider.Product.RefundAndReschedulingProvision.Scrap ?? string.Empty);
                AddParameter("PROVIDERALTERATION", provider.Product.RefundAndReschedulingProvision.Alteration ?? string.Empty);
                AddParameter("PROVIDERTRANSFER", provider.Product.RefundAndReschedulingProvision.Transfer ?? string.Empty);
            }
            ExecuteNonQuery(sql.ToString());
        }
        void saveSupplierInfo(decimal orderId, SupplierInfo supplier)
        {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[T_Supplier]");
            sql.Append("([OrderId],[Product],[ProductType],[RequireConfirm],[Remark],Condition,[Provision])");
            sql.Append(" VALUES ");
            sql.Append("(@ORDERID,@PRODUCT,@PRODUCTTYPE,@REQUIRECONFIRM,@PRODUCTREMARK,@Condition,@SUPPLIERPROVISIONID);");
            AddParameter("ORDERID", orderId);
            AddParameter("PRODUCT", supplier.Product.Id);
            AddParameter("PRODUCTTYPE", (byte)supplier.Product.SpeicalProductType);
            AddParameter("REQUIRECONFIRM", supplier.Product.RequireConfirm);
            AddParameter("PRODUCTREMARK", supplier.Product.Remark ?? string.Empty);
            AddParameter("Condition", supplier.Product.Condition ?? string.Empty);
            if (supplier.Product.RefundAndReschedulingProvision == null)
            {
                AddParameter("SUPPLIERPROVISIONID", DBNull.Value);
            }
            else
            {
                sql.Append("INSERT INTO [dbo].[T_Provision]");
                sql.Append("([Id],[Refund],[Scrap],[Alteration],[Transfer])");
                sql.Append(" VALUES ");
                sql.Append("(@SUPPLIERPROVISIONID,@SUPPLIERREFUND,@SUPPLIERSCRAP,@SUPPLIERALTERATION,@SUPPLIERTRANSFER);");
                AddParameter("SUPPLIERPROVISIONID", Guid.NewGuid());
                AddParameter("SUPPLIERREFUND", supplier.Product.RefundAndReschedulingProvision.Refund ?? string.Empty);
                AddParameter("SUPPLIERSCRAP", supplier.Product.RefundAndReschedulingProvision.Scrap ?? string.Empty);
                AddParameter("SUPPLIERALTERATION", supplier.Product.RefundAndReschedulingProvision.Alteration ?? string.Empty);
                AddParameter("SUPPLIERTRANSFER", supplier.Product.RefundAndReschedulingProvision.Transfer ?? string.Empty);
            }
            ExecuteNonQuery(sql.ToString());
        }
        void saveFlights(decimal orderId, IEnumerable<PNRInfo> pnrInfos)
        {
            ClearParameters();
            var sql = new StringBuilder();
            int index = 0;
            foreach (var pnr in pnrInfos)
            {
                foreach (var flight in pnr.Flights)
                {
                    sql.Append(prepareInsertFlight(flight, index, pnr.Id, orderId));
                    index++;
                }
            }
            if (sql.Length > 0)
            {
                ExecuteNonQuery(sql);
            }
        }
        void updateFlights(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = string.Empty;
            var index = 0;
            foreach (var pnr in order.PNRInfos)
            {
                foreach (var flight in pnr.Flights)
                {
                    sql += string.Format("UPDATE dbo.T_Flight" +
                        " SET ReleasedFare=@ReleasedFare{0},Fare=@Fare{0},Discount=@Discount{0},AirportFee=@AirportFee{0},BAF=@BAF{0},Bunk=@Bunk{0},EI=@EI{0},IsShare=@IsShare{0}" +
                        " WHERE [Id]=@Flight{0};", index);
                    if (flight.Bunk is Order.Domain.Bunk.SpecialBunk)
                    {
                        AddParameter("ReleasedFare" + index, (flight.Bunk as Order.Domain.Bunk.SpecialBunk).ReleasedFare);
                    }
                    else
                    {
                        AddParameter("ReleasedFare" + index, DBNull.Value);
                    }
                    AddParameter("Fare" + index, flight.Bunk.Fare);
                    AddParameter("Discount" + index, flight.Bunk.Discount);
                    AddParameter("AirportFee" + index, flight.AirportFee);
                    AddParameter("BAF" + index, flight.BAF);
                    AddParameter("Bunk" + index, flight.Bunk.Code ?? string.Empty);
                    AddParameter("EI" + index, flight.Bunk.EI ?? string.Empty);
                    AddParameter("IsShare" + index, flight.IsShare);
                    AddParameter("Flight" + index, flight.Id);
                    index++;
                }
            }
            ExecuteNonQuery(sql);
        }
        void updateTicketPrice(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = string.Empty;
            var index = 0;
            foreach (var pnr in order.PNRInfos)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    foreach (var ticket in passenger.Tickets)
                    {
                        sql += string.Format("UPDATE dbo.T_Ticket SET Fare=@Fare{0},AirportFee=@AirportFee{0},BAF=@BAF{0} WHERE Passenger=@Passenger{0} AND Serial=@Serial{0};", index);
                        AddParameter("Fare" + index, ticket.Price.Fare);
                        AddParameter("AirportFee" + index, ticket.Price.AirportFee);
                        AddParameter("BAF" + index, ticket.Price.BAF);
                        AddParameter("Passenger" + index, passenger.Id);
                        AddParameter("Serial" + index, ticket.Serial);
                        index++;
                    }
                }
            }
            ExecuteNonQuery(sql);
        }
        void cancelFlights(decimal orderId)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Flight] SET [PNR]=@PNR WHERE [OrderId]=@ORDERID";
            AddParameter("PNR", DBNull.Value);
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql);
        }
        void resetFlights(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = new StringBuilder();
            var index = 0;
            foreach (var pnr in order.PNRInfos)
            {
                foreach (var flight in pnr.Flights)
                {
                    sql.AppendFormat("IF EXISTS(SELECT NULL FROM dbo.T_Flight WHERE [Id]=@ID{0}) UPDATE [dbo].[T_Flight] SET [PNR]=@PNR{0} WHERE [Id]=@ID{0} ELSE ", index);
                    sql.Append(prepareInsertFlight(flight, index, pnr.Id, order.Id));
                    index++;
                }
            }
            if (sql.Length > 0)
            {
                ExecuteNonQuery(sql);
            }
        }
        string prepareInsertFlight(Domain.Flight flight, int index, Guid pnr, decimal orderId)
        {
            var result = new StringBuilder();
            result.Append("INSERT INTO [dbo].[T_Flight]" +
                          "([OrderId],[PNR],[Id],[Serial],[Departure],[DepartureCityName],[DepartureAirportName],[Arrival],[ArrivalCityName],[ArrivalAirportName]," +
                          "[Carrier],[CarrierName],[SettleCode],[FlightNo],[TakeoffTime],[LandingTime],[AirCraft],[Bunk],[Discount],[BunkType]," +
                          "[BunkDescription],[EI],[YBPrice],[ReleasedFare],[Fare],[AirportFee],[BAF],AssociateFlight,ReservateFlight,Ticket,IsShare,ArrivalTerminal,DepartureTerminal,Increasing) VALUES ");
            result.AppendFormat("(@ORDERID{0},@PNR{0},@ID{0},@SERIAL{0},@DEPARTURE{0},@DEPARTURECITYNAME{0},@DEPARTUREAIRPORTNAME{0},@ARRIVAL{0},@ARRIVALCITYNAME{0},", index);
            result.AppendFormat("@ARRIVALAIRPORTNAME{0},@CARRIER{0},@CARRIERNAME{0},@SETTLECODE{0},@FLIGHTNO{0},@TAKEOFFTIME{0},@LANDINGTIME{0},@AIRCRAFT{0},", index);
            result.AppendFormat("@BUNK{0},@DISCOUNT{0},@BUNKTYPE{0},@BUNKDESCRIPTION{0},@EI{0},@YBPRICE{0},@RELEASEDFARE{0},@FARE{0},@AIRPORTFEE{0},@BAF{0},", index);
            result.AppendFormat("@AssociateFlight{0},@ReservateFlight{0},@Ticket{0},@IsShare{0},@ArrivalTerminal{0},@DepartureTerminal{0},@Increasing{0});", index);
            AddParameter("ORDERID" + index, orderId);
            AddParameter("PNR" + index, pnr);
            AddParameter("ID" + index, flight.Id);
            AddParameter("SERIAL" + index, flight.Serial);
            AddParameter("DEPARTURE" + index, flight.Departure.Code);
            AddParameter("DEPARTURECITYNAME" + index, flight.Departure.City);
            AddParameter("DEPARTUREAIRPORTNAME" + index, flight.Departure.Name);
            AddParameter("ARRIVAL" + index, flight.Arrival.Code);
            AddParameter("ARRIVALCITYNAME" + index, flight.Arrival.City);
            AddParameter("ARRIVALAIRPORTNAME" + index, flight.Arrival.Name);
            AddParameter("CARRIER" + index, flight.Carrier.Code);
            AddParameter("CARRIERNAME" + index, flight.Carrier.Name);
            AddParameter("SETTLECODE" + index, flight.Carrier.SettleCode);
            AddParameter("FLIGHTNO" + index, flight.FlightNo);
            AddParameter("TAKEOFFTIME" + index, flight.TakeoffTime);
            AddParameter("LANDINGTIME" + index, flight.LandingTime);
            AddParameter("AIRCRAFT" + index, flight.AirCraft ?? string.Empty);
            AddParameter("YBPRICE" + index, flight.YBPrice);
            AddParameter("AIRPORTFEE" + index, flight.AirportFee);
            AddParameter("BAF" + index, flight.BAF);
            AddParameter("BUNK" + index, flight.Bunk.Code ?? string.Empty);
            AddParameter("FARE" + index, flight.Bunk.Fare);
            AddParameter("DISCOUNT" + index, flight.Bunk.Discount);
            AddParameter("EI" + index, flight.Bunk.EI ?? string.Empty);
            AddParameter("Increasing" + index, flight.Increasing);
            if (flight.Bunk is Domain.Bunk.FirstOrBusinessBunk)
            {
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.FirstOrBusiness);
                AddParameter("BUNKDESCRIPTION" + index, (flight.Bunk as Domain.Bunk.FirstOrBusinessBunk).Description ?? string.Empty);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else if (flight.Bunk is Domain.Bunk.EconomicBunk)
            {
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Economic);
                AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else if (flight.Bunk is Domain.Bunk.PromotionBunk)
            {
                var promotionBunk = flight.Bunk as Domain.Bunk.PromotionBunk;
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Promotion);
                AddParameter("BUNKDESCRIPTION" + index, promotionBunk.Description ?? string.Empty);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else if (flight.Bunk is Domain.Bunk.ProductionBunk)
            {
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Production);
                AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else if (flight.Bunk is Domain.Bunk.SpecialBunk)
            {
                var specialBunk = flight.Bunk as Domain.Bunk.SpecialBunk;
                if (flight.Bunk is Domain.Bunk.FreeBunk)
                {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Free);
                    AddParameter("BUNKDESCRIPTION" + index, (flight.Bunk as Domain.Bunk.FreeBunk).Description ?? string.Empty);
                }
                else
                {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Speical);
                    AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                }
                AddParameter("RELEASEDFARE" + index, specialBunk.ReleasedFare);
            }
            else if (flight.Bunk is Domain.Bunk.TeamBunk)
            {
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Team);
                AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else if (flight.Bunk is Domain.Bunk.TransferBunk)
            {
                AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Transfer);
                AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                AddParameter("RELEASEDFARE" + index, DBNull.Value);
            }
            else
            {
                throw new NotImplementedException();
            }
            if (flight.AssociateFlight.HasValue)
            {
                AddParameter("AssociateFlight" + index, flight.AssociateFlight.Value);
            }                                                                                                                                                                                  
            else
            {
                AddParameter("AssociateFlight" + index, DBNull.Value);
            }
            AddParameter("ReservateFlight" + index, flight.ReservateFlight);
            AddParameter("Ticket" + index, flight.Ticket.Serial);
            AddParameter("IsShare" + index, flight.IsShare);
            AddParameter("ArrivalTerminal" + index, flight.ArrivalTerminal??string.Empty);
            AddParameter("DepartureTerminal" + index, flight.DepartureTerminal??string.Empty);
            return result.ToString();
        }
        void cancelPassengers(decimal orderId)
        {
            ClearParameters();
            var sql = "UPDATE [dbo].[T_Passenger] SET [PNR]=@PNR WHERE [OrderId]=@ORDERID";
            AddParameter("PNR", DBNull.Value);
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql);
        }
        void resetPassengersRelation(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = new StringBuilder();
            var pnrIndex = 0;
            var passengerIndex = 0;
            foreach (var pnr in order.PNRInfos)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    sql.AppendFormat("UPDATE [dbo].[T_Passenger] SET [PNR]=@PNR{0} WHERE [Id]=@PASSENGER{1};", pnrIndex, passengerIndex);
                    AddParameter("PASSENGER" + passengerIndex, passenger.Id);
                    passengerIndex++;
                }
                AddParameter("PNR" + pnrIndex, pnr.Id);
                pnrIndex++;
            }
            if (sql.Length > 0)
            {
                ExecuteNonQuery(sql);
            }
        }
        void savePassengers(decimal orderId, IEnumerable<PNRInfo> pnrInfos)
        {
            ClearParameters();
            var sql = new StringBuilder();
            int pnrIndex = 0, passengerIndex = 0;
            foreach (var pnr in pnrInfos)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    sql.AppendFormat(" SELECT @ORDERID,@PNR{0},@ID{1},@NAME{1},@PASSENGERTYPE{1},@CREDENTIALS{1},@CREDENTIALSTYPE{1},@PHONE{1},@BIRTHDAY{1} UNION ALL", pnrIndex, passengerIndex);
                    AddParameter("ID" + passengerIndex, passenger.Id);
                    AddParameter("NAME" + passengerIndex, passenger.Name ?? string.Empty);
                    AddParameter("PASSENGERTYPE" + passengerIndex, (byte)passenger.PassengerType);
                    AddParameter("CREDENTIALS" + passengerIndex, passenger.Credentials ?? string.Empty);
                    AddParameter("CREDENTIALSTYPE" + passengerIndex, (byte)passenger.CredentialsType);
                    AddParameter("PHONE" + passengerIndex, passenger.Phone ?? string.Empty);
                    if (passenger.Birthday.HasValue)
                    {
                        AddParameter("BIRTHDAY" + passengerIndex, passenger.Birthday.Value);
                    }
                    else
                    {
                        AddParameter("BIRTHDAY" + passengerIndex, DBNull.Value);
                    }
                    passengerIndex++;
                }
                AddParameter("PNR" + pnrIndex, pnr.Id);
                pnrIndex++;
            }
            AddParameter("ORDERID", orderId);
            if (sql.Length > 0)
            {
                sql.Insert(0, "INSERT INTO [dbo].[T_Passenger] ([OrderId],[PNR],[Id],[Name],[PassengerType],[Credentials],[CredentialsType],[Phone],[BirthDay])");
            }
            ExecuteNonQuery(sql.Remove(sql.Length - 10, 10));
        }
        void saveTickets(decimal orderId, IEnumerable<PNRInfo> pnrInfos)
        {
            ClearParameters();
            var sql = new StringBuilder();
            int passengerIndex = 0, ticketIndex = 0;
            foreach (var pnr in pnrInfos)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    foreach (var ticket in passenger.Tickets)
                    {
                        sql.AppendFormat(" SELECT @PASSENGER{0},@SERIAL{1},@SETTLECODE{1},@TICKETNO{1},@ETDZTIME{1},@ETDZMODE{1},@Fare{1},@AirportFee{1},@BAF{1} UNION ALL", passengerIndex, ticketIndex);
                        AddParameter("SERIAL" + ticketIndex, ticket.Serial);
                        if (string.IsNullOrWhiteSpace(ticket.SettleCode))
                        {
                            AddParameter("SETTLECODE" + ticketIndex, DBNull.Value);
                        }
                        else
                        {
                            AddParameter("SETTLECODE" + ticketIndex, ticket.SettleCode.Trim());
                        }
                        if (string.IsNullOrWhiteSpace(ticket.No))
                        {
                            AddParameter("TICKETNO" + ticketIndex, DBNull.Value);
                        }
                        else
                        {
                            AddParameter("TICKETNO" + ticketIndex, ticket.No.Trim());
                        }
                        if (ticket.ETDZTime.HasValue)
                        {
                            AddParameter("ETDZTIME" + ticketIndex, ticket.ETDZTime.Value);
                        }
                        else
                        {
                            AddParameter("ETDZTIME" + ticketIndex, DBNull.Value);
                        }
                        if (ticket.ETDZMode.HasValue)
                        {
                            AddParameter("ETDZMODE" + ticketIndex, (byte)ticket.ETDZMode.Value);
                        }
                        else
                        {
                            AddParameter("ETDZMODE" + ticketIndex, DBNull.Value);
                        }
                        AddParameter("Fare" + ticketIndex, ticket.Price.Fare);
                        AddParameter("AirportFee" + ticketIndex, ticket.Price.AirportFee);
                        AddParameter("BAF" + ticketIndex, ticket.Price.BAF);
                        ticketIndex++;
                    }
                    AddParameter("PASSENGER" + passengerIndex, passenger.Id);
                    passengerIndex++;
                }
            }
            if (sql.Length > 0)
            {
                sql.Insert(0, "INSERT INTO [dbo].[T_Ticket] ([Passenger],[Serial],[SettleCode],[TicketNo],[ETDZTime],[ETDZMode],Fare,AirportFee,BAF)");
                ExecuteNonQuery(sql.Remove(sql.Length - 10, 10));
            }
        }
        void updateTicketNos(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = new StringBuilder();
            int passengerIndex = 0, ticketIndex = 0;
            foreach (var pnr in order.PNRInfos)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    foreach (var ticket in passenger.Tickets)
                    {
                        sql.AppendFormat("UPDATE [dbo].[T_Ticket] SET SettleCode=@SettleCode{0},[TicketNo]=@TICKETNO{0},[ETDZTime]=@ETDZTIME{0},[ETDZMode]=@ETDZMODE{0} WHERE [Passenger]=@PASSENGER{1} AND [Serial]=@SERIAL{0};", ticketIndex, passengerIndex);
                        AddParameter("SettleCode" + ticketIndex, ticket.SettleCode);
                        AddParameter("TICKETNO" + ticketIndex, ticket.No);
                        AddParameter("ETDZTIME" + ticketIndex, ticket.ETDZTime.Value);
                        AddParameter("ETDZMODE" + ticketIndex, (byte)ticket.ETDZMode.Value);
                        AddParameter("SERIAL" + ticketIndex, ticket.Serial);
                        ticketIndex++;
                    }
                    AddParameter("PASSENGER" + passengerIndex, passenger.Id);
                    passengerIndex++;
                }
            }
            ExecuteNonQuery(sql);
        }
        void updatePNRForSupplyResource(Order.Domain.Order order)
        {
            var pnrs = updatePNRMainInfo(order.PNRInfos.First());
            saveOrderPNRItems(order.Id, pnrs);
            updateOrderForPNR(order.Id, order.ReservationPNR, true);
        }
        IEnumerable<string> updatePNRMainInfo(PNRInfo pnrInfo)
        {
            ClearParameters();
            var pnrs = new List<string>();
            var sql = "UPDATE [dbo].[T_OrderPNR] SET [BPNR]=@BPNR,[PNR]=@PNR,[PNRContent]=@PNRContent,[PatContent]=@PatContent WHERE [Id]=@PNRID";
            AddParameter("PNRID", pnrInfo.Id);
            if (string.IsNullOrWhiteSpace(pnrInfo.Code.BPNR))
            {
                AddParameter("BPNR", DBNull.Value);
            }
            else
            {
                AddParameter("BPNR", pnrInfo.Code.BPNR);
                pnrs.Add(pnrInfo.Code.BPNR);
            }
            if (string.IsNullOrWhiteSpace(pnrInfo.Code.PNR))
            {
                AddParameter("PNR", DBNull.Value);
            }
            else
            {
                AddParameter("PNR", pnrInfo.Code.PNR);
                pnrs.Add(pnrInfo.Code.PNR);
            }
            if (string.IsNullOrEmpty(pnrInfo.PNRContent))
            {
                AddParameter("@PNRContent", pnrInfo.PNRContent);
            }
            else
            {
                AddParameter("@PNRContent", DBNull.Value);
            }
            if (string.IsNullOrEmpty(pnrInfo.PatContent))
            {
                AddParameter("@PatContent", DBNull.Value);
            }
            else
            {
                AddParameter("@PatContent", pnrInfo.PatContent);
            }
            ExecuteNonQuery(sql);
            return pnrs;
        }
        void updatePNRForETDZ(Order.Domain.Order order)
        {
            if (!PNRPair.Equals(order.ReservationPNR, order.ETDZPNR))
            {
                var pnrs = updatePNRMainInfo(order.PNRInfos.First());
                saveOrderPNRItems(order.Id, pnrs);
            }
            updateOrderForPNR(order.Id, order.ETDZPNR, false);
        }
        void saveNewPNRMainInfos(Order.Domain.Order order)
        {
            ClearParameters();
            var sql = new StringBuilder();
            var index = 0;
            foreach (var pnr in order.PNRInfos)
            {
                sql.AppendFormat("IF NOT EXISTS(SELECT NULL FROM [dbo].[T_OrderPNR] WHERE [Id]=@PNRID{0}) ", index);
                sql.AppendFormat("INSERT INTO [dbo].[T_OrderPNR] ([Id],[OrderId],[BPNR],[PNR]) VALUES (@PNRID{0},@ORDERID,@BPNR{0},@PNR{0});", index);
                AddParameter("PNRID" + index.ToString(), pnr.Id);
                AddParameter("BPNR" + index.ToString(), pnr.Code.BPNR);
                AddParameter("PNR" + index.ToString(), pnr.Code.PNR);
                index++;
            }
            AddParameter("ORDERID", order.Id);
            if (sql.Length > 0)
            {
                ExecuteNonQuery(sql);
            }
        }
        void savePNRContents(decimal orderId, IEnumerable<PNRInfo> pnrInfos)
        {
            if (!pnrInfos.Any()) return;
            var pnrItems = savePNRMainInfos(orderId, pnrInfos);
            saveOrderPNRItems(orderId, pnrItems);
            savePassengers(orderId, pnrInfos);
            saveTickets(orderId, pnrInfos);
            saveFlights(orderId, pnrInfos);
        }
        IEnumerable<string> savePNRMainInfos(decimal orderId, IEnumerable<PNRInfo> pnrInfos)
        {
            ClearParameters();
            var pnrList = new List<string>();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[T_OrderPNR] ([Id],[OrderId],[BPNR],[PNR])");
            int index = 0;
            foreach (var pnr in pnrInfos)
            {
                sql.AppendFormat(" SELECT @PNRID{0},@ORDERID,@BPNR{0},@PNR{0} UNION ALL", index);
                AddParameter("PNRID" + index, pnr.Id);
                if (pnr.Code == null || string.IsNullOrWhiteSpace(pnr.Code.BPNR))
                {
                    AddParameter("BPNR" + index, DBNull.Value);
                }
                else
                {
                    AddParameter("BPNR" + index, pnr.Code.BPNR.Trim());
                    pnrList.Add(pnr.Code.BPNR);
                }
                if (pnr.Code == null || string.IsNullOrWhiteSpace(pnr.Code.PNR))
                {
                    AddParameter("PNR" + index, DBNull.Value);
                }
                else
                {
                    AddParameter("PNR" + index, pnr.Code.PNR.Trim());
                    pnrList.Add(pnr.Code.PNR);
                }
                index++;
            }
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql.Remove(sql.Length - 10, 10));
            return pnrList;
        }
        void saveOrderPNRItems(decimal orderId, IEnumerable<string> pnrs)
        {
            ClearParameters();
            if (!pnrs.Any()) return;
            var sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[T_OrderPNRItem] ([OrderId],[PNR])");
            int index = 0;
            foreach (var item in pnrs)
            {
                sql.AppendFormat(" SELECT @ORDERID,@PNR{0} UNION ALL", index);
                AddParameter("PNR" + index, item.Trim());
                index++;
            }
            AddParameter("ORDERID", orderId);
            ExecuteNonQuery(sql.Remove(sql.Length - 10, 10));
        }

        public DataTransferObject.Policy.RefundAndReschedulingProvision QueryProviderRefundAndReschedulingProvision(decimal orderId)
        {
            ClearParameters();
            var sql = "SELECT [Refund],[Scrap],[Alteration],[Transfer] FROM [dbo].[T_Provision] WHERE EXISTS(SELECT NULL FROM [dbo].[T_Provider] WHERE Provision=Id AND OrderId=@ORDERID)";
            AddParameter("ORDERID", orderId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    return new DataTransferObject.Policy.RefundAndReschedulingProvision(
                        reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        );
                }
            }
            return null;
        }
        public DataTransferObject.Policy.RefundAndReschedulingProvision QuerySupplierRefundAndReschedulingProvision(decimal orderId)
        {
            ClearParameters();
            var sql = "SELECT [Refund],[Scrap],[Alteration],[Transfer] FROM [dbo].[T_Provision] WHERE EXISTS(SELECT NULL FROM [dbo].[T_Supplier] WHERE Provision=Id AND OrderId=@ORDERID)";
            AddParameter("ORDERID", orderId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    return new DataTransferObject.Policy.RefundAndReschedulingProvision(
                        reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        );
                }
            }
            return null;
        }

        /// <summary>
        /// 通过票号获取订单信息
        /// </summary>
        /// <param name="ticketNo"></param>
        /// <returns></returns>
        public Domain.Order QueryOrderByTicketNo(string ticketNo)
        {
            ClearParameters();
            AddParameter("TicketNo", ticketNo);
            var order = loadOrderMainInfo("TOR.ID =( select top 1 OrderId from T_Passenger where Id =  (select top 1 Passenger from T_Ticket where TicketNo = @TicketNo))",new Domain.Order(0));
            if (order != null)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
            }
            return order;

        }

        public Domain.Order QueryOrder(decimal orderId)
        {
            ClearParameters();
            AddParameter("ORDERID", orderId);
            var order = loadOrderMainInfo("TOR.[Id]=@ORDERID",new Domain.Order(0));
            if (order != null)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
            }
            return order;
        }
        public Domain.Order QueryOrder(string pnrCode, DateTime producedTime)
        {
            ClearParameters();
            AddParameter("PNRCode", pnrCode);
            AddParameter("ProducedTime", producedTime);
            var condition = "TOR.ProducedTime>=@ProducedTime AND EXISTS(SELECT NULL FROM dbo.T_OrderPNRItem TPNR WHERE TOR.Id=TPNR.OrderId AND TPNR.PNR=@PNRCode)";
            var orderby = "TOR.ProducedTime DESC";
            var order = loadOrderMainInfo(condition,new Domain.Order(0), orderby);
            if (order != null)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
            }
            return order;
        }
        Domain.Order loadOrderMainInfo(string condition, Domain.Order order, string orderby = "")
        {
            var sql = new StringBuilder();
            sql.Append("SELECT TOR.[Id],TOR.[Status],TOR.[Source],TPRO.[ProductType],TOR.[ProducedTime],TOR.[ProducedAccount],TOR.[AssociateOrder],TOR.[Remark],");
            sql.Append("TOR.[Purchaser],TOR.[PurchaserName],TOR.[PurchaserRebate],TOR.[PurchaserAmount],");
            sql.Append("TOR.[Provider],TOR.[ProviderName],TOR.[ProviderRebate],TOR.[ProviderAmount],TPRO.[Policy],TPRO.[PolicyType],TPRO.[OfficeNo],TPRO.ETDZMode,");
            sql.Append("TPRO.[RequireChangePNR],TPRO.[SpecialProductType],TPRO.[RequireConfirm],TPRO.[Remark],TPRO.[PurchaserRelationType],");
            sql.Append("TOR.[Supplier],TOR.[SupplierName],TOR.[SupplierRebate],TOR.[SupplierAmount],TSUP.[Product],TSUP.[ProductType],TSUP.[RequireConfirm],TSUP.[Remark],");
            sql.Append("TOR.[BPNR],TOR.[PNR],TOR.[EBPNR],TOR.[EPNR],TOR.[PayTime],TOR.[SupplyTime],TOR.[ETDZTime],");
            sql.Append("TCON.[Name],TCON.[Mobile],TCON.[Email],TOR.[PurchaserCommission],TOR.[ProviderCommission],TOR.[SupplierCommission],TOR.[RevisedFare],");
            sql.Append("TOR.TripType,TOR.IsReduce,TOR.AssociatePNR,TOR.AssociateBPNR,TPRO.Condition,TSUP.Condition,");
            sql.Append("TOR.Choise,TOR.CustomNO,TOR.IsCustomerResource,TOR.IsTeam,TOR.IsStandby,TOR.ProducedAccountName,TOR.VisibleRole,TPRO.IsDefaultPolicy,TOR.PassengerMsgSended,TOR.IsB3BOrder,TOR.ForbidChangPNR,TOR.OEMID");
            sql.Append(" FROM [dbo].[T_Order] TOR");
            sql.Append(" LEFT JOIN [dbo].[T_Provider] TPRO ON TOR.[Id]=TPRO.[OrderId]");
            sql.Append(" LEFT JOIN [dbo].[T_Supplier] TSUP ON TOR.[Id]=TSUP.[OrderId]");
            sql.Append(" INNER JOIN [dbo].[T_OrderContact] TCON ON TOR.[Contact]=TCON.[Id]");
            sql.Append(" WHERE " + condition);
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sql.Append(" ORDER BY " + orderby);
            }

            using (var reader = ExecuteReader(sql.ToString()))
            {
                if (reader.Read())
                {
                    order.Id = reader.GetDecimal(0);
                    order.Status = (OrderStatus)reader.GetInt32(1);
                    order.Source = (OrderSource)reader.GetByte(2);
                    order.Remark = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                    order.RevisedPrice = !reader.IsDBNull(46) && reader.GetBoolean(46);
                    order.TripType = (DataTransferObject.Command.PNR.ItineraryType)reader.GetByte(47);
                    order.IsReduce = reader.GetBoolean(48);
                    order.Choise = (AuthenticationChoise)reader.GetByte(53);
                    order.CustomNo = reader.GetString(54);
                    order.IsCustomerResource = reader.GetBoolean(55);
                    order.IsTeam = reader.GetBoolean(56);
                    order.IsStandby = reader.GetBoolean(57);
                    order.VisibleRole = (OrderRole)reader.GetByte(59);
                    order.PassengerMsgSended = reader.GetBoolean(61);
                    order.IsB3BOrder = reader.GetBoolean(62);
                    order.ForbidChangPNR = reader.GetBoolean(63);
                    if (!reader.IsDBNull(64)) order.OEMID = reader.GetGuid(64);
                    if (!reader.IsDBNull(6))
                    {
                        order.AssociateOrderId = reader.GetDecimal(6);
                    }
                    var associatePNR = reader.IsDBNull(49) ? string.Empty : reader.GetString(49);
                    var associateBPNR = reader.IsDBNull(50) ? string.Empty : reader.GetString(50);
                    if (!string.IsNullOrWhiteSpace(associatePNR) || !string.IsNullOrWhiteSpace(associateBPNR))
                    {
                        order.AssociatePNR = new PNRPair(associatePNR, associateBPNR);
                    }
                    order.Purchaser = new Domain.PurchaserInfo(reader.GetGuid(8), reader.GetString(9))
                    {
                        Rebate = reader.GetDecimal(10),
                        Commission = reader.GetDecimal(43),
                        Amount = reader.GetDecimal(11),
                        ProducedTime = reader.GetDateTime(4),
                        OperatorAccount = reader.GetString(5),
                        OperatorName = reader.GetString(58)
                    };
                    if (!reader.IsDBNull(12))
                    {
                        var productType = (ProductType)reader.GetByte(3);
                        ProductInfo productInfo = null;
                        if (productType == ProductType.Special)
                        {
                            productInfo = new SpeicalProductInfo(order.Id, false)
                            {
                                SpeicalProductType = (SpecialProductType)reader.GetByte(21),
                                RequireConfirm = reader.GetBoolean(22)
                            };
                        }
                        else
                        {
                            productInfo = new Domain.CommonProductInfo(order.Id)
                            {
                                ETDZMode = (ETDZMode)reader.GetByte(19),
                                RequireChangePNR = reader.GetBoolean(20)
                            };
                        }
                        productInfo.Id = reader.GetGuid(16);
                        productInfo.TicketType = reader.IsDBNull(17) ? TicketType.B2B : (TicketType)reader.GetByte(17);
                        productInfo.OfficeNo = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                        productInfo.ProductType = productType;
                        productInfo.IsDefaultPolicy = !reader.IsDBNull(60) && reader.GetBoolean(60);
                        productInfo.Remark = reader.IsDBNull(23) ? string.Empty : reader.GetString(23);
                        productInfo.Condition = reader.IsDBNull(51) ? string.Empty : reader.GetString(51);
                        order.Provider = new Domain.ProviderInfo(reader.GetGuid(12), reader.GetString(13))
                        {
                            Rebate = reader.GetDecimal(14),
                            Commission = reader.GetDecimal(44),
                            Amount = reader.GetDecimal(15),
                            Product = productInfo,
                            PurchaserRelationType = (RelationType)reader.GetByte(24)
                        };
                    }
                    if (!reader.IsDBNull(25))
                    {
                        order.Supplier = new Domain.SupplierInfo(reader.GetGuid(25), reader.GetString(26))
                        {
                            Rebate = reader.GetDecimal(27),
                            Commission = reader.GetDecimal(45),
                            Amount = reader.GetDecimal(28),
                            Product = new Domain.SpeicalProductInfo(order.Id, true)
                            {
                                Id = reader.GetGuid(29),
                                SpeicalProductType = (SpecialProductType)reader.GetByte(30),
                                RequireConfirm = reader.GetBoolean(31),
                                Remark = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                                ProductType = ProductType.Special,
                                IsDefaultPolicy = false,
                                Condition = reader.IsDBNull(52) ? string.Empty : reader.GetString(52)
                            }
                        };
                    }
                    var bpnr = reader.IsDBNull(33) ? string.Empty : reader.GetString(33);
                    var pnr = reader.IsDBNull(34) ? string.Empty : reader.GetString(34);
                    var ebpnr = reader.IsDBNull(35) ? string.Empty : reader.GetString(35);
                    var epnr = reader.IsDBNull(36) ? string.Empty : reader.GetString(36);
                    if (!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr))
                    {
                        order.ReservationPNR = new PNRPair(pnr, bpnr);
                    }
                    if (!string.IsNullOrWhiteSpace(ebpnr) || !string.IsNullOrWhiteSpace(epnr))
                    {
                        order.ETDZPNR = new PNRPair(epnr, ebpnr);
                    }
                    if (!reader.IsDBNull(37))
                    {
                        order.Purchaser.PayTime = reader.GetDateTime(37);
                    }
                    if (!reader.IsDBNull(38))
                    {
                        order.SupplyTime = reader.GetDateTime(38);
                    }
                    if (!reader.IsDBNull(39))
                    {
                        order.ETDZTime = reader.GetDateTime(39);
                    }
                    order.Contact = new Contact()
                    {
                        Name = reader.GetString(40),
                        Mobile = reader.IsDBNull(41) ? string.Empty : reader.GetString(41),
                        Email = reader.IsDBNull(42) ? string.Empty : reader.GetString(42)
                    };
                }
                else
                {
                    order = null;
                }
            }
            return order;
        }


        Domain.ExternalOrder loadExternalOrderMainInfo(string condition, string orderby = "")
        {
            Domain.ExternalOrder order = null;
            var sql = new StringBuilder();
            sql.Append("SELECT TOR.[Id],TOR.[Status],TOR.[Source],TOR.[ProductType],TOR.[ProducedTime],TOR.[ProducedAccount],TOR.[AssociateOrder],TOR.[Remark],");
            sql.Append("TOR.[Purchaser],TOR.[PurchaserName],TOR.[PurchaserRebate],TOR.[PurchaserAmount],");
            sql.Append("TOR.[Provider],TOR.[ProviderName],TOR.[ProviderRebate],TOR.[ProviderAmount],TPRO.[Policy],TPRO.[PolicyType],TPRO.[OfficeNo],TPRO.ETDZMode,");
            sql.Append("TPRO.[RequireChangePNR],TPRO.[SpecialProductType],TPRO.[RequireConfirm],TPRO.[Remark],TPRO.[PurchaserRelationType],");
            sql.Append("TOR.[Supplier],TOR.[SupplierName],TOR.[SupplierRebate],TOR.[SupplierAmount],TSUP.[Product],TSUP.[ProductType],TSUP.[RequireConfirm],TSUP.[Remark],");
            sql.Append("TOR.[BPNR],TOR.[PNR],TOR.[EBPNR],TOR.[EPNR],TOR.[PayTime],TOR.[SupplyTime],TOR.[ETDZTime],");
            sql.Append("TCON.[Name],TCON.[Mobile],TCON.[Email],TOR.[PurchaserCommission],TOR.[ProviderCommission],TOR.[SupplierCommission],TOR.[RevisedFare],");
            sql.Append("TOR.TripType,TOR.IsReduce,TOR.AssociatePNR,TOR.AssociateBPNR,TPRO.Condition,TSUP.Condition,");
            sql.Append("TOR.Choise,TOR.CustomNO,TOR.IsCustomerResource,TOR.IsTeam,TOR.IsStandby,TOR.ProducedAccountName,TOR.VisibleRole,TPRO.IsDefaultPolicy,TOR.PassengerMsgSended,TOR.IsB3BOrder,TOR.ForbidChangPNR");
            sql.Append(" FROM [dbo].[T_Order] TOR");
            sql.Append(" LEFT JOIN [dbo].[T_Provider] TPRO ON TOR.[Id]=TPRO.[OrderId]");
            sql.Append(" LEFT JOIN [dbo].[T_Supplier] TSUP ON TOR.[Id]=TSUP.[OrderId]");
            sql.Append(" INNER JOIN [dbo].[T_OrderContact] TCON ON TOR.[Contact]=TCON.[Id]");
            sql.Append(" WHERE " + condition);
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sql.Append(" ORDER BY " + orderby);
            }

            using (var reader = ExecuteReader(sql.ToString()))
            {
                if (reader.Read())
                {
                    order = new Domain.ExternalOrder(reader.GetDecimal(0))
                    {
                        Status = (OrderStatus)reader.GetInt32(1),
                        Source = (OrderSource)reader.GetByte(2),
                        Remark = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        RevisedPrice = !reader.IsDBNull(46) && reader.GetBoolean(46),
                        TripType = (DataTransferObject.Command.PNR.ItineraryType)reader.GetByte(47),
                        IsReduce = reader.GetBoolean(48),
                        Choise = (AuthenticationChoise)reader.GetByte(53),
                        CustomNo = reader.GetString(54),
                        IsCustomerResource = reader.GetBoolean(55),
                        IsTeam = reader.GetBoolean(56),
                        IsStandby = reader.GetBoolean(57),
                        VisibleRole = (OrderRole)reader.GetByte(59),
                        PassengerMsgSended = reader.GetBoolean(61),
                        ForbidChangPNR = reader.GetBoolean(62)
                    };
                    if (!reader.IsDBNull(6))
                    {
                        order.AssociateOrderId = reader.GetDecimal(6);
                    }
                    var associatePNR = reader.IsDBNull(49) ? string.Empty : reader.GetString(49);
                    var associateBPNR = reader.IsDBNull(50) ? string.Empty : reader.GetString(50);
                    if (!string.IsNullOrWhiteSpace(associatePNR) || !string.IsNullOrWhiteSpace(associateBPNR))
                    {
                        order.AssociatePNR = new PNRPair(associatePNR, associateBPNR);
                    }
                    order.Purchaser = new Domain.PurchaserInfo(reader.GetGuid(8), reader.GetString(9))
                    {
                        Rebate = reader.GetDecimal(10),
                        Commission = reader.GetDecimal(43),
                        Amount = reader.GetDecimal(11),
                        ProducedTime = reader.GetDateTime(4),
                        OperatorAccount = reader.GetString(5),
                        OperatorName = reader.GetString(58)
                    };
                    if (!reader.IsDBNull(12))
                    {
                        var productType = (ProductType)reader.GetByte(3);
                        ProductInfo productInfo = null;
                        if (productType == ProductType.Special && reader.IsDBNull(25))
                        {
                            productInfo = new SpeicalProductInfo(order.Id, false)
                            {
                                SpeicalProductType = (SpecialProductType)reader.GetByte(21),
                                RequireConfirm = reader.GetBoolean(22)
                            };
                        }
                        else
                        {
                            productInfo = new Domain.CommonProductInfo(order.Id)
                            {
                                ETDZMode = (ETDZMode)reader.GetByte(19),
                                RequireChangePNR = reader.GetBoolean(20),
                            };
                        }
                        productInfo.Id = reader.GetGuid(16);
                        productInfo.TicketType = reader.IsDBNull(17) ? TicketType.B2B : (TicketType)reader.GetByte(17);
                        productInfo.OfficeNo = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                        productInfo.ProductType = productType;
                        productInfo.IsDefaultPolicy = !reader.IsDBNull(60) && reader.GetBoolean(60);
                        productInfo.Remark = reader.IsDBNull(23) ? string.Empty : reader.GetString(23);
                        productInfo.Condition = reader.IsDBNull(51) ? string.Empty : reader.GetString(51);
                        order.Provider = new Domain.ProviderInfo(reader.GetGuid(12), reader.GetString(13))
                        {
                            Rebate = reader.GetDecimal(14),
                            Commission = reader.GetDecimal(44),
                            Amount = reader.GetDecimal(15),
                            Product = productInfo,
                            PurchaserRelationType = (RelationType)reader.GetByte(24)
                        };
                    }
                    if (!reader.IsDBNull(25))
                    {
                        order.Supplier = new Domain.SupplierInfo(reader.GetGuid(25), reader.GetString(26))
                        {
                            Rebate = reader.GetDecimal(27),
                            Commission = reader.GetDecimal(45),
                            Amount = reader.GetDecimal(28),
                            Product = new Domain.SpeicalProductInfo(order.Id, true)
                            {
                                Id = reader.GetGuid(29),
                                SpeicalProductType = (SpecialProductType)reader.GetByte(30),
                                RequireConfirm = reader.GetBoolean(31),
                                Remark = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                                ProductType = ProductType.Special,
                                IsDefaultPolicy = false,
                                Condition = reader.IsDBNull(52) ? string.Empty : reader.GetString(52)
                            }
                        };
                    }
                    var bpnr = reader.IsDBNull(33) ? string.Empty : reader.GetString(33);
                    var pnr = reader.IsDBNull(34) ? string.Empty : reader.GetString(34);
                    var ebpnr = reader.IsDBNull(35) ? string.Empty : reader.GetString(35);
                    var epnr = reader.IsDBNull(36) ? string.Empty : reader.GetString(36);
                    if (!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr))
                    {
                        order.ReservationPNR = new PNRPair(pnr, bpnr);
                    }
                    if (!string.IsNullOrWhiteSpace(ebpnr) || !string.IsNullOrWhiteSpace(epnr))
                    {
                        order.ETDZPNR = new PNRPair(epnr, ebpnr);
                    }
                    if (!reader.IsDBNull(37))
                    {
                        order.Purchaser.PayTime = reader.GetDateTime(37);
                    }
                    if (!reader.IsDBNull(38))
                    {
                        order.SupplyTime = reader.GetDateTime(38);
                    }
                    if (!reader.IsDBNull(39))
                    {
                        order.ETDZTime = reader.GetDateTime(39);
                    }
                    order.Contact = new Contact()
                    {
                        Name = reader.GetString(40),
                        Mobile = reader.IsDBNull(41) ? string.Empty : reader.GetString(41),
                        Email = reader.IsDBNull(42) ? string.Empty : reader.GetString(42)
                    };
                }
            }
            return order;
        }
        IEnumerable<PNRInfo> loadPNRInfos(decimal orderId)
        {
            var pnrs = loadPNRMainInfos(orderId);
            loadFlights(orderId, pnrs);
            loadPassengers(orderId, pnrs);
            fillFlightInTicket(pnrs);
            return pnrs;
        }
        IEnumerable<PNRInfo> loadPNRMainInfos(decimal orderId)
        {
            ClearParameters();
            var pnrInfoList = new List<PNRInfo>();
            var pnrsSql = new StringBuilder();
            pnrsSql.Append("SELECT [Id],[BPNR],[PNR],[PNRContent],[PatContent] FROM dbo.T_OrderPNR TPNR WHERE OrderId=@ORDERID");
            AddParameter("ORDERID", orderId);
            using (var reader = ExecuteReader(pnrsSql.ToString()))
            {
                while (reader.Read())
                {
                    var pnrItem = new PNRInfo(reader.GetGuid(0));
                    var bpnr = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    var pnr = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    if (!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr))
                    {
                        pnrItem.Code = new PNRPair(pnr, bpnr);
                    }
                    pnrItem.PNRContent = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    pnrItem.PatContent = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                    pnrInfoList.Add(pnrItem);
                }
            }
            return pnrInfoList;
        }
        void loadFlights(decimal orderId, IEnumerable<PNRInfo> pnrs)
        {
            ClearParameters();
            var flightsSql = new StringBuilder();
            flightsSql.Append("SELECT TPNR.[Id],");
            flightsSql.Append("TFLIGHT.[Id],TFLIGHT.[Serial],TFLIGHT.[Departure],TFLIGHT.[DepartureAirportName],TFLIGHT.[DepartureCityName],TFLIGHT.[Arrival],");
            flightsSql.Append("TFLIGHT.[ArrivalAirportName],TFLIGHT.[ArrivalCityName],TFLIGHT.[Carrier],TFLIGHT.[CarrierName],TFLIGHT.[SettleCode],TFLIGHT.[FlightNo],");
            flightsSql.Append("TFLIGHT.[TakeoffTime],TFLIGHT.[LandingTime],TFLIGHT.[AirCraft],TFLIGHT.[YBPrice],TFLIGHT.[Fare],TFLIGHT.[Discount],TFLIGHT.[ReleasedFare],");
            flightsSql.Append("TFLIGHT.[AirportFee],TFLIGHT.[BAF],TFLIGHT.[Bunk],TFLIGHT.[BunkType],TFLIGHT.[BunkDescription],TFLIGHT.[EI],TFLIGHT.[AssociateFlight],TFLIGHT.ReservateFlight,TFLIGHT.IsShare,TFLIGHT.ArrivalTerminal,TFLIGHT.DepartureTerminal,TFLIGHT.ReleasedFare,TFLIGHT.Increasing");
            flightsSql.Append(" FROM [dbo].[T_OrderPNR] TPNR");
            flightsSql.Append(" INNER JOIN [dbo].[T_Flight] TFLIGHT ON TFLIGHT.[PNR]=TPNR.[Id]");
            flightsSql.Append(" WHERE TPNR.[OrderId]=@ORDERID");
            flightsSql.Append(" ORDER BY TPNR.[Id],TFLIGHT.[Serial]");

            AddParameter("ORDERID", orderId);
            using (var reader = ExecuteReader(flightsSql.ToString()))
            {
                PNRInfo pnr = null;
                while (reader.Read())
                {
                    var currentPNRId = reader.GetGuid(0);
                    if (pnr == null || pnr.Id != currentPNRId)
                    {
                        pnr = pnrs.FirstOrDefault(item => item.Id == currentPNRId);
                        if (pnr == null)
                            continue;
                    }
                    var flightItem = new Flight(reader.GetGuid(1),
                                                new Carrier(reader.GetString(9), reader.GetString(10), reader.GetString(11)),
                                                new Airport(reader.GetString(3), reader.GetString(4), reader.GetString(5)),
                                                new Airport(reader.GetString(6), reader.GetString(7), reader.GetString(8)),
                                                reader.GetDateTime(13))
                                                {
                                                    Serial = reader.GetInt32(2),
                                                    FlightNo = reader.GetString(12),
                                                    LandingTime = reader.GetDateTime(14),
                                                    AirCraft = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                                    YBPrice = reader.GetDecimal(16),
                                                    AirportFee = reader.GetDecimal(20),
                                                    BAF = reader.GetDecimal(21),
                                                    ReleaseFare = reader.IsDBNull(31)?0:reader.GetDecimal(31),
                                                    Increasing = reader.GetDecimal(32)
                                                };
                    Domain.Bunk.BaseBunk bunk = null;
                    var bunkCode = reader.IsDBNull(22) ? string.Empty : reader.GetString(22);
                    var bunkType = (Domain.Bunk.BunkType)reader.GetByte(23);
                    var discount = reader.GetDecimal(18);
                    var fare = reader.GetDecimal(17);
                    var ei = reader.IsDBNull(25) ? string.Empty : reader.GetString(25);
                    var description = reader.IsDBNull(24) ? string.Empty : reader.GetString(24);
                    switch (bunkType)
                    {
                        case Order.Domain.Bunk.BunkType.Economic:
                            bunk = new Order.Domain.Bunk.EconomicBunk(bunkCode, discount, fare, ei);
                            break;
                        case Order.Domain.Bunk.BunkType.FirstOrBusiness:
                            bunk = new Order.Domain.Bunk.FirstOrBusinessBunk(bunkCode, discount, fare, ei, description);
                            break;
                        case Order.Domain.Bunk.BunkType.Promotion:
                            bunk = new Order.Domain.Bunk.PromotionBunk(bunkCode, discount, fare, ei, description);
                            break;
                        case Order.Domain.Bunk.BunkType.Production:
                            bunk = new Order.Domain.Bunk.ProductionBunk(bunkCode, discount, fare, ei);
                            break;
                        case Order.Domain.Bunk.BunkType.Speical:
                            bunk = new Order.Domain.Bunk.SpecialBunk(bunkCode, discount, fare, reader.GetDecimal(19), ei);
                            break;
                        case Order.Domain.Bunk.BunkType.Free:
                            bunk = new Order.Domain.Bunk.FreeBunk(bunkCode, reader.GetDecimal(19), ei, description);
                            break;
                        case Order.Domain.Bunk.BunkType.Team:
                            bunk = new Order.Domain.Bunk.TeamBunk(bunkCode, discount, fare, ei, description);
                            break;
                        case Order.Domain.Bunk.BunkType.Transfer:
                            bunk = new Order.Domain.Bunk.TransferBunk(bunkCode, discount, fare, ei);
                            break;
                        default:
                            break;
                    }
                    flightItem.Bunk = bunk;
                    if (!reader.IsDBNull(26))
                    {
                        flightItem.AssociateFlight = reader.GetGuid(26);
                    }
                    flightItem.ReservateFlight = reader.GetGuid(27);
                    flightItem.IsShare = reader.GetBoolean(28);
                    flightItem.ArrivalTerminal = reader.IsDBNull(29) ? string.Empty : reader.GetString(29);
                    flightItem.DepartureTerminal = reader.IsDBNull(30) ? string.Empty : reader.GetString(30);
                    pnr.AddFlight(flightItem);
                }
            }
        }
        void loadPassengers(decimal orderId, IEnumerable<PNRInfo> pnrs)
        {
            ClearParameters();
            var passengersSql = new StringBuilder();
            passengersSql.Append("SELECT TPNR.[Id],");
            passengersSql.Append("TPASSENGER.[Id],TPASSENGER.[Name],TPASSENGER.[PassengerType],TPASSENGER.[Credentials],TPASSENGER.[CredentialsType],TPASSENGER.[Phone],");
            passengersSql.Append("TTICKET.[Serial],TTICKET.[SettleCode],TTICKET.[TicketNo],TTICKET.[ETDZMode],TTICKET.[ETDZTime],TTICKET.Fare,TTICKET.AirportFee,TTICKET.BAF,TPASSENGER.BirthDay");
            passengersSql.Append(" FROM [dbo].[T_OrderPNR] TPNR");
            passengersSql.Append(" INNER JOIN [dbo].[T_Passenger] TPASSENGER ON TPASSENGER.[PNR]=TPNR.[ID]");
            passengersSql.Append(" INNER JOIN [dbo].[T_Ticket] TTICKET ON TTICKET.[Passenger]=TPASSENGER.[Id]");
            passengersSql.Append(" WHERE TPNR.[OrderId]=@ORDERID");
            passengersSql.Append(" ORDER BY TPNR.[Id],TPASSENGER.[Name],TPASSENGER.Id,TTICKET.[Serial]");
            AddParameter("ORDERID", orderId);
            using (var reader = ExecuteReader(passengersSql))
            {
                PNRInfo pnr = null;
                Passenger passenger = null;
                while (reader.Read())
                {
                    var currentPNRId = reader.GetGuid(0);
                    var currentPassengerId = reader.GetGuid(1);
                    if (pnr == null || pnr.Id != currentPNRId)
                    {
                        pnr = pnrs.FirstOrDefault(item => item.Id == currentPNRId);
                        if (pnr == null) continue;
                    }
                    if (passenger == null || passenger.Id != currentPassengerId)
                    {
                        passenger = new Passenger(currentPassengerId)
                        {
                            Name = reader.GetString(2),
                            PassengerType = (PassengerType)reader.GetByte(3),
                            Credentials = reader.GetString(4),
                            CredentialsType = (CredentialsType)reader.GetByte(5),
                            Phone = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                        };
                        if (!reader.IsDBNull(15))
                        {
                            passenger.Birthday = reader.GetDateTime(15);
                        }
                        pnr.AddPassenger(passenger);
                    }
                    var ticket = new Ticket
                    {
                        Serial = reader.GetInt32(7),
                        SettleCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        No = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Price = new Price(reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                            reader.IsDBNull(13) ? 0 : reader.GetDecimal(13),
                            reader.IsDBNull(14) ? 0 : reader.GetDecimal(14))
                    };
                    if (!reader.IsDBNull(10))
                    {
                        ticket.ETDZMode = (ETDZMode)reader.GetByte(10);
                    }
                    if (!reader.IsDBNull(11))
                    {
                        ticket.ETDZTime = reader.GetDateTime(11);
                    }
                    passenger.AddTicket(ticket);
                }
            }
        }
        void fillFlightInTicket(IEnumerable<PNRInfo> pnrs)
        {
            foreach (var pnr in pnrs)
            {
                foreach (var passenger in pnr.Passengers)
                {
                    foreach (var ticket in passenger.Tickets)
                    {
                        ticket.Load(pnr.Flights);
                    }
                }
            }
        }

        public IEnumerable<OrderListView> QueryOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime)
        {
            ClearParameters();
            IEnumerable<OrderListView> result = null;
            if (condition.OrderId.HasValue)
            {
                AddParameter("@iOrderId", condition.OrderId.Value);
            }
            else
            {
                AddParameter("@iOrderId", DBNull.Value);
            }
            if (condition.Purchaser.HasValue)
            {
                AddParameter("@iPurchaser", condition.Purchaser.Value);
            }
            else
            {
                AddParameter("@iPurchaser", DBNull.Value);
            }
            if (condition.Provider.HasValue)
            {
                AddParameter("@iProvider", condition.Provider.Value);
            }
            else
            {
                AddParameter("@iProvider", DBNull.Value);
            }
            if (condition.Supplier.HasValue)
            {
                AddParameter("@iSupplier", condition.Supplier.Value);
            }
            else
            {
                AddParameter("@iSupplier", DBNull.Value);
            }
            if (condition.Status.HasValue)
            {
                AddParameter("@iStatus", (int)condition.Status.Value);
            }
            else
            {
                AddParameter("@iStatus", DBNull.Value);
            }
            if (condition.ProductType.HasValue)
            {
                AddParameter("@iOrderProductType", (byte)condition.ProductType.Value);
            }
            else
            {
                AddParameter("@iOrderProductType", DBNull.Value);
            }
            if(condition.ProviderProductType.HasValue)
            {
                AddParameter("@iProviderProductType", (byte)condition.ProviderProductType.Value);
            }else
            {
                AddParameter("@iProviderProductType", DBNull.Value);
            }
            if (condition.Source.HasValue)
            {
                AddParameter("@iOrderSource", condition.Source.Value);
            }
            else
            {
                AddParameter("@iOrderSource", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.PNR))
            {
                AddParameter("@iPNR", DBNull.Value);
            }
            else
            {
                AddParameter("@iPNR", condition.PNR.Trim());
            }
            if (condition.OEMID.HasValue)
            {
                AddParameter("@iOEMID", condition.OEMID.Value);
            }
            else
            {
                AddParameter("@iOEMID", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Passenger))
            {
                AddParameter("@iPassenger", DBNull.Value);
            }
            else
            {
                AddParameter("@iPassenger", condition.Passenger.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.OfficeNo))
            {
                AddParameter("@iOfficeNo", DBNull.Value);
            }
            else
            {
                AddParameter("@iOfficeNo", condition.OfficeNo.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.Carrier))
            {
                AddParameter("@iCarrier", DBNull.Value);
            }
            else
            {
                AddParameter("@iCarrier", condition.Carrier.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.ProducedAccount))
            {
                AddParameter("@iProduceAccount", DBNull.Value);
            }
            else
            {
                AddParameter("@iProduceAccount", condition.ProducedAccount.Trim());
            }
            if (null == condition.CustomNo)
            {
                AddParameter("@CustomNo", DBNull.Value);
            }
            else
            {
                AddParameter("@CustomNo", condition.CustomNo.Trim());
            }
            if (extendDateTime)
            {
                AddParameter("@iProducedStartTime", condition.ProducedDateRange.Lower.Date);
                AddParameter("@iProducedEndTime", condition.ProducedDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            }
            else
            {
                AddParameter("@iProducedStartTime", condition.ProducedDateRange.Lower);
                AddParameter("@iProducedEndTime", condition.ProducedDateRange.Upper);
            }
            if (condition.OrderRole.HasValue)
            {
                AddParameter("@iVisibleRole", (byte)condition.OrderRole.Value);
            }
            else
            {
                AddParameter("@iVisibleRole", DBNull.Value);
            }
            if (condition.RelationType.HasValue)
            {
                AddParameter("@iRelation", (byte)condition.RelationType.Value);
            }
            else
            {
                AddParameter("@iRelation", DBNull.Value);
            }
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryOrders", System.Data.CommandType.StoredProcedure))
            {
                result = reconstructOrderListViews(reader);
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        public IEnumerable<OrderListView> QueryWaitOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime)
        {
            ClearParameters();
            IEnumerable<OrderListView> result = null;
            if (condition.OrderId.HasValue)
            {
                AddParameter("@iOrderId", condition.OrderId.Value);
            }
            else
            {
                AddParameter("@iOrderId", DBNull.Value);
            }
            if (condition.Purchaser.HasValue)
            {
                AddParameter("@iPurchaser", condition.Purchaser.Value);
            }
            else
            {
                AddParameter("@iPurchaser", DBNull.Value);
            }
            if (condition.Provider.HasValue)
            {
                AddParameter("@iProvider", condition.Provider.Value);
            }
            else
            {
                AddParameter("@iProvider", DBNull.Value);
            }
            if (condition.Supplier.HasValue)
            {
                AddParameter("@iSupplier", condition.Supplier.Value);
            }
            else
            {
                AddParameter("@iSupplier", DBNull.Value);
            }
            if (condition.Status.HasValue)
            {
                AddParameter("@iStatus", (int)condition.Status.Value);
            }
            else
            {
                AddParameter("@iStatus", DBNull.Value);
            }
            if (condition.ProductType.HasValue)
            {
                AddParameter("@iOrderProductType", (byte)condition.ProductType.Value);
            }
            else
            {
                AddParameter("@iOrderProductType", DBNull.Value);
            }
            if (condition.ProviderProductType.HasValue)
            {
                AddParameter("@iProviderProductType", (byte)condition.ProviderProductType.Value);
            }
            else
            {
                AddParameter("@iProviderProductType", DBNull.Value);
            }
            if (condition.Source.HasValue)
            {
                AddParameter("@iOrderSource", condition.Source.Value);
            }
            else
            {
                AddParameter("@iOrderSource", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.PNR))
            {
                AddParameter("@iPNR", DBNull.Value);
            }
            else
            {
                AddParameter("@iPNR", condition.PNR.Trim());
            }
            if (condition.OEMID.HasValue)
            {
                AddParameter("@iOEMID", condition.OEMID.Value);
            }
            else
            {
                AddParameter("@iOEMID", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Passenger))
            {
                AddParameter("@iPassenger", DBNull.Value);
            }
            else
            {
                AddParameter("@iPassenger", condition.Passenger.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.OfficeNo))
            {
                AddParameter("@iOfficeNo", DBNull.Value);
            }
            else
            {
                AddParameter("@iOfficeNo", condition.OfficeNo.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.Carrier))
            {
                AddParameter("@iCarrier", DBNull.Value);
            }
            else
            {
                AddParameter("@iCarrier", condition.Carrier.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.ProducedAccount))
            {
                AddParameter("@iProduceAccount", DBNull.Value);
            }
            else
            {
                AddParameter("@iProduceAccount", condition.ProducedAccount.Trim());
            }
            if (null == condition.CustomNo)
            {
                AddParameter("@CustomNo", DBNull.Value);
            }
            else
            {
                AddParameter("@CustomNo", condition.CustomNo.Trim());
            }
            if (extendDateTime)
            {
                AddParameter("@iProducedStartTime", condition.ProducedDateRange.Lower.Date);
                AddParameter("@iProducedEndTime", condition.ProducedDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            }
            else
            {
                AddParameter("@iProducedStartTime", condition.ProducedDateRange.Lower);
                AddParameter("@iProducedEndTime", condition.ProducedDateRange.Upper);
            }
            if (condition.OrderRole.HasValue)
            {
                AddParameter("@iVisibleRole", (byte)condition.OrderRole.Value);
            }
            else
            {
                AddParameter("@iVisibleRole", DBNull.Value);
            }
            if (condition.RelationType.HasValue)
            {
                AddParameter("@iRelation", (byte)condition.RelationType.Value);
            }
            else
            {
                AddParameter("@iRelation", DBNull.Value);
            }
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryWaitOrders", System.Data.CommandType.StoredProcedure))
            {
                result = reconstructOrderListViews(reader);
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        IEnumerable<OrderListView> reconstructOrderListViews(DbDataReader reader)
        {
            var result = new List<OrderListView>();
            OrderListView item = null;
            List<Guid> flights = null;
            while (reader.Read())
            {
                decimal currentOrderId = reader.GetDecimal(0);
                if (item == null || item.OrderId != currentOrderId)
                {
                    item = new OrderListView()
                    {
                        OrderId = currentOrderId,
                        Status = (OrderStatus)reader.GetInt32(1),
                        Source = (OrderSource)reader.GetByte(2),
                        ProductType = (ProductType)reader.GetByte(3),
                        ProducedTime = reader.GetDateTime(4),
                        ProducedAccount = reader.GetString(5),
                        Purchaser = reader.GetGuid(6),
                        PurchaserName = reader.GetString(7),
                        SettlementForPurchaser = new Settlement()
                        {
                            Rebate = reader.GetDecimal(8),
                            Commission = reader.GetDecimal(9),
                            Amount = reader.GetDecimal(10)
                        },
                        Choise = (AuthenticationChoise)reader.GetByte(43),
                        ProducedAccountName = reader.GetString(44),
                        Passengers = new List<string>(),
                        PassengerType = (PassengerType)reader.GetByte(47),
                        Flights = new List<FlightListView>(),
                        TicketType = reader.IsDBNull(50) ? TicketType.B2B : (TicketType)reader.GetByte(50),
                        IsEmergentOrder = reader.GetBoolean(60),
                        NeedAUTH = reader.GetBoolean(59)
                    };
                    if (!reader.IsDBNull(48))
                    {
                        item.RefuseETDZTime = reader.GetDateTime(48);
                    }
                    if (!reader.IsDBNull(45))
                    {
                        item.PayTime = reader.GetDateTime(45);
                    }
                    if (!reader.IsDBNull(51))
                    {
                        item.SupplyTime = reader.GetDateTime(51);
                    }
                    if (!reader.IsDBNull(46))
                    {
                        item.ETDZTime = reader.GetDateTime(46);
                    }
                    if (!reader.IsDBNull(11))
                    {
                        item.Provider = reader.GetGuid(11);
                        item.ProviderName = reader.GetString(12);
                        item.SettlementForProvider = new Settlement()
                        {
                            Rebate = reader.GetDecimal(13),
                            Commission = reader.GetDecimal(14),
                            Amount = reader.GetDecimal(15)
                        };
                        item.OfficeNo = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        if (!reader.IsDBNull(17))
                        {
                            item.ETDZMode = (ETDZMode)reader.GetByte(17);
                        }
                    }
                    if (!reader.IsDBNull(18))
                    {
                        item.Supplier = reader.GetGuid(18);
                        item.SupplierName = reader.GetString(19);
                        if (!reader.IsDBNull(20))
                        {
                            item.SettlementForSupplier = new Settlement()
                            {
                                Rebate = reader.GetDecimal(20),
                                Commission = reader.GetDecimal(21),
                                Amount = reader.GetDecimal(22)
                            };
                        }
                    }
                    string bpnr = reader.IsDBNull(23) ? null : reader.GetString(23);
                    string pnr = reader.IsDBNull(24) ? null : reader.GetString(24);
                    string ebpnr = reader.IsDBNull(25) ? null : reader.GetString(25);
                    string epnr = reader.IsDBNull(26) ? null : reader.GetString(26);
                    if (reader.IsDBNull(42))
                    {
                        item.PurcharseProviderRelation = null;
                    }
                    else
                    {
                        item.PurcharseProviderRelation = (RelationType)reader.GetByte(42);
                    }
                    if (!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr))
                    {
                        item.ReservationPNR = new PNRPair(pnr, bpnr);
                    }
                    if (!string.IsNullOrWhiteSpace(ebpnr) || !string.IsNullOrWhiteSpace(epnr))
                    {
                        item.ETDZPNR = new PNRPair(epnr, ebpnr);
                    }
                    item.PassengerMsgSended = reader.GetBoolean(49);
                    //if(!reader.IsDBNull(50)) {
                    //    var emergentOrder = new EmergentOrderView();
                    //    emergentOrder.Id = reader.GetDecimal(50);
                    //    emergentOrder.Type = (OrderStatus)reader.GetInt32(51);
                    //    emergentOrder.Content = reader.GetString(52);
                    //    emergentOrder.Time = reader.GetDateTime(53);
                    //    item.EmergentOrder = emergentOrder;
                    //}
                    if(!reader.IsDBNull(52))
                        item.RemindContent = reader.GetString(52);
                    if(!reader.IsDBNull(53))
                        item.RemindTime = reader.GetDateTime(53);
                    item.IsNeedReminded = reader.GetBoolean(54);
                    if(!reader.IsDBNull(55))
                    {
                        item.ProviderProductType = (ProductType) reader.GetByte(55);
                    }
                    if (!reader.IsDBNull(56)) item.OEMID = reader.GetGuid(56);
                    flights = new List<Guid>();
                    result.Add(item);
                }
                string passengerName = reader.GetString(27);
                if (!item.Passengers.Contains(passengerName))
                {
                    item.Passengers.Add(passengerName);
                }
                Guid flightId = reader.GetGuid(28);
                if (!flights.Contains(flightId))
                {
                    var flightView = new FlightListView()
                    {
                        DepartureCity = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                        DepartureAirport = reader.GetString(30),
                        ArrivalCity = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                        ArrivalAirport = reader.GetString(32),
                        Carrier = reader.GetString(33),
                        FlightNo = reader.GetString(34),
                        Bunk = reader.GetString(35),
                        Discount = reader.GetDecimal(36),
                        Fare = reader.GetDecimal(38),
                        AirportFee = reader.GetDecimal(39),
                        BAF = reader.GetDecimal(40),
                        TakeoffTime = reader.GetDateTime(41),
                        DepartureTeminal = reader.IsDBNull(57)?string.Empty:reader.GetString(57),
                        ArrivalTeminal = reader.IsDBNull(58)?string.Empty:reader.GetString(58)
                    };
                    flightView.ReleasedFare = reader.IsDBNull(37) ? flightView.Fare : reader.GetDecimal(37);
                    item.Flights.Add(flightView);
                    flights.Add(flightId);
                }
            }
            return result;
        }

        public void SaveCredentialsUpdateInfo(Order.Domain.Order order, Passenger passenger, string originalCredentials, string newCredentials, bool success, OperatorRole role, string commitAccount)
        {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("IF EXISTS(SELECT NULL FROM dbo.T_CredentialsUpdateInfo WHERE OrderId=@ORDERID AND Passenger=@PASSENGER)");
            sql.Append(" UPDATE dbo.T_CredentialsUpdateInfo SET OriginalCredentials=@ORIGINALCREDENTIALS,NewCredentials=@NEWCREDENTIALS,Success=@SUCCESS,");
            sql.Append("CommitAccount=@COMMITACCOUNT,CommitTime=@COMMITTIME");
            sql.Append(" WHERE OrderId=@ORDERID AND Passenger=@PASSENGER ");
            sql.Append("ELSE");
            sql.Append(" INSERT INTO dbo.T_CredentialsUpdateInfo(Id,OrderId,Passenger,PassengerName,OriginalCredentials,NewCredentials,Purchaser,PurchaserName,Success,CommitTime,CommitAccount)");
            sql.Append(" VALUES(@ID,@ORDERID,@PASSENGER,@PASSENGERNAME,@ORIGINALCREDENTIALS,@NEWCREDENTIALS,@PURCHASER,@PURCHASERNAME,@SUCCESS,@COMMITTIME,@COMMITACCOUNT);");
            sql.Append("INSERT INTO dbo.T_CredentialsUpdateRecord(OrderId,Passenger,OriginalCredentials,NewCredentials,CommitTime,CommitAccount,CommitRole,Success)");
            sql.Append(" VALUES(@ORDERID,@PASSENGER,@ORIGINALCREDENTIALS,@NEWCREDENTIALS,@COMMITTIME,@COMMITACCOUNT,@COMMITROLE,@SUCCESS)");
            var commitTime = DateTime.Now;
            AddParameter("ID", Guid.NewGuid());
            AddParameter("ORDERID", order.Id);
            AddParameter("PASSENGER", passenger.Id);
            AddParameter("PASSENGERNAME", passenger.Name);
            AddParameter("PURCHASER", order.Purchaser.CompanyId);
            AddParameter("PURCHASERNAME", order.Purchaser.Name);
            AddParameter("ORIGINALCREDENTIALS", originalCredentials);
            AddParameter("NEWCREDENTIALS", newCredentials);
            AddParameter("COMMITROLE", (byte)role);
            AddParameter("SUCCESS", success);
            AddParameter("COMMITTIME", commitTime);
            AddParameter("COMMITACCOUNT", commitAccount);
            ExecuteNonQuery(sql);
        }
        public CredentialsUpdateInfo QueryCredentialsUpdateInfo(Guid credentialsUpdateInfoId)
        {
            ClearParameters();
            var sql = "SELECT OrderId,PassengerName,OriginalCredentials,NewCredentials,Success,CommitTime,CommitAccount FROM dbo.T_CredentialsUpdateInfo WHERE Id=@ID";
            AddParameter("ID", credentialsUpdateInfoId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    return new CredentialsUpdateInfo()
                    {
                        OrderId = reader.GetDecimal(0),
                        Passenger = reader.GetString(1),
                        OriginalCredentials = reader.GetString(2),
                        NewCredentials = reader.GetString(3),
                        Success = reader.GetBoolean(4),
                        CommitTime = reader.GetDateTime(5),
                        CommitAccount = reader.GetString(6)
                    };
                }
            }
            return null;
        }
        public IEnumerable<CredentialsUpdateInfoListView> QueryCredentialsUpdateInfos(CredentialsUpdateInfoQueryCondition condition, Pagination pagination)
        {
            ClearParameters();
            var result = new List<CredentialsUpdateInfoListView>();
            if (condition.OrderId.HasValue)
            {
                AddParameter("@iOrderId", condition.OrderId.Value);
            }
            else
            {
                AddParameter("@iOrderId", DBNull.Value);
            }
            if (condition.Success.HasValue)
            {
                AddParameter("@iSuccess", condition.Success.Value);
            }
            else
            {
                AddParameter("@iSuccess", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Passenger))
            {
                AddParameter("@iPassenger", DBNull.Value);
            }
            else
            {
                AddParameter("@iPassenger", condition.Passenger.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.PNR))
            {
                AddParameter("@iPNR", DBNull.Value);
            }
            else
            {
                AddParameter("@iPNR", condition.PNR.Trim());
            }
            AddParameter("@iCommitStartTime", condition.CommitDateRange.Lower.Date);
            AddParameter("@iCOmmitEndTime", condition.CommitDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryCredentialsUpdateInfo", System.Data.CommandType.StoredProcedure))
            {
                CredentialsUpdateInfoListView item = null;
                List<FlightListView> flights = null;
                while (reader.Read())
                {
                    Guid currentUpdateInfoId = reader.GetGuid(0);
                    if (item == null || item.Id != currentUpdateInfoId)
                    {
                        flights = new List<FlightListView>();
                        item = new CredentialsUpdateInfoListView()
                        {
                            Id = currentUpdateInfoId,
                            OrderId = reader.GetDecimal(1),
                            PassengerName = reader.GetString(2),
                            Passenger = reader.GetGuid(24),
                            OriginalCredentials = reader.GetString(3),
                            NewCredentials = reader.GetString(4),
                            Purchaser = reader.GetGuid(5),
                            PurchaserName = reader.GetString(6),
                            Success = reader.GetBoolean(7),
                            CommitTime = reader.GetDateTime(8),
                            Flights = flights
                        };
                        string bpnr = reader.IsDBNull(9) ? null : reader.GetString(9);
                        string pnr = reader.IsDBNull(10) ? null : reader.GetString(10);
                        item.PNR = new PNRPair(pnr, bpnr);
                        result.Add(item);
                    }
                    var flightView = new FlightListView()
                    {
                        DepartureCity = reader.GetString(11),
                        DepartureAirport = reader.GetString(12),
                        ArrivalCity = reader.GetString(13),
                        ArrivalAirport = reader.GetString(14),
                        Carrier = reader.GetString(15),
                        FlightNo = reader.GetString(16),
                        Bunk = reader.GetString(17),
                        Discount = reader.GetDecimal(18),
                        Fare = reader.GetDecimal(20),
                        AirportFee = reader.GetDecimal(21),
                        BAF = reader.GetDecimal(22),
                        TakeoffTime = reader.GetDateTime(23)
                    };
                    flightView.ReleasedFare = reader.IsDBNull(19) ? flightView.Fare : reader.GetDecimal(19);
                    flights.Add(flightView);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        public IEnumerable<CredentialsUpdateRecordView> QueryCredentialsUpdateRecords(decimal orderId, Guid? passengerId)
        {
            ClearParameters();
            var result = new List<CredentialsUpdateRecordView>();
            var sql = "SELECT TPassenger.Name,TRECORD.[OriginalCredentials],TRECORD.[NewCredentials],TRECORD.[CommitTime],TRECORD.[CommitRole],TRECORD.[CommitAccount],TRECORD.[Success]" +
                " FROM [dbo].[T_CredentialsUpdateRecord] TRECORD INNER JOIN dbo.T_Passenger TPassenger ON TPassenger.Id=TRECORD.Passenger WHERE TRECORD.OrderId=@ORDERID";
            AddParameter("OrderId", orderId);
            if (passengerId.HasValue)
            {
                sql += " AND TRECORD.Passenger=@Passenger";
                AddParameter("Passenger", passengerId.Value);
            }
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    result.Add(reconstructCredentialsUpdateRecord(reader));
                }
            }
            return result;
        }
        CredentialsUpdateRecordView reconstructCredentialsUpdateRecord(DbDataReader reader)
        {
            return new CredentialsUpdateRecordView()
            {
                Passenger = reader.GetString(0),
                OriginalCredentials = reader.GetString(1),
                NewCredentials = reader.GetString(2),
                CommitTime = reader.GetDateTime(3),
                CommitRole = (OperatorRole)reader.GetByte(4),
                CommitAccount = reader.GetString(5),
                Success = reader.GetBoolean(6)
            };
        }

        /// <summary>
        /// 获取员工在指定订单状态下的订单待办数量
        /// </summary>
        /// <param name="orderstatus"></param>
        /// <param name="employeeId"></param>
        /// <param name="providerId"> </param>
        /// <returns></returns>
        public int GetOrderTodoCount(OrderStatus orderstatus, Guid employeeId, Guid providerId)
        {
            ClearParameters();
            AddParameter("@Status", (int)orderstatus);
            AddParameter("@EmployeeId", employeeId);
            AddParameter("@iProvider", providerId);
            var totalCount = AddParameter("@TodoCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            ExecuteNonQuery("P_QueryETDZToDoCount", System.Data.CommandType.StoredProcedure);
            return (int)totalCount.Value;
        }


        public IEnumerable<ExternalOrderListView> QueryExternalOrders(DataTransferObject.Order.External.ExternalOrderCondition condition, Pagination pagiantion)
        {
            ClearParameters();
            IEnumerable<ExternalOrderListView> result = null;
            if (condition.ProviderId.HasValue)
            {
                AddParameter("@iProviderId", condition.ProviderId);
            }
            else
            {
                AddParameter("@iProviderId", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Arrival))
            {
                AddParameter("@iArrival", DBNull.Value);
            }
            else
            {
                AddParameter("@iArrival", condition.Arrival);
            }
            if (string.IsNullOrWhiteSpace(condition.Departure))
            {
                AddParameter("@iDeparture", DBNull.Value);
            }
            else
            {
                AddParameter("@iDeparture", condition.Departure);
            }
            if (condition.EndTime.HasValue)
            {
                AddParameter("@iEndTime", condition.EndTime);
            }
            else
            {
                AddParameter("@iEndTime", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.ExternalOrderId))
            {
                AddParameter("@iExternalOrderId", DBNull.Value);
            }
            else
            {
                AddParameter("@iExternalOrderId", condition.ExternalOrderId);
            }
            if (condition.InternalOrderId.HasValue)
            {
                AddParameter("@iInternalOrderId", condition.InternalOrderId);
            }
            else
            {
                AddParameter("@iInternalOrderId", DBNull.Value);
            }
            if (condition.IsEtdzed.HasValue)
            {
                AddParameter("@iIsEtdzed", condition.IsEtdzed);
            }
            else
            {
                AddParameter("@iIsEtdzed", DBNull.Value);
            }
            if (condition.PayStatus.HasValue)
            {
                AddParameter("@iPayStatus", (byte)condition.PayStatus);
            }
            else
            {
                AddParameter("@iPayStatus", DBNull.Value);
            }
            if (condition.PlatformType.HasValue)
            {
                AddParameter("@iPlatformType", (byte)condition.PlatformType);
            }
            else
            {
                AddParameter("@iPlatformType", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Pnr))
            {
                AddParameter("@iPnr", DBNull.Value);
            }
            else
            {
                AddParameter("@iPnr", condition.Pnr);
            }

            if (condition.StartTime.HasValue)
            {
                AddParameter("@iStartTime", condition.StartTime);
            }
            else
            {
                AddParameter("@iStartTime", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.Passenger))
            {
                AddParameter("@iPassenger", DBNull.Value);
            }
            else
            {
                AddParameter("@iPassenger", condition.Passenger);
            }
            if (pagiantion != null)
            {
                AddParameter("@iPagesize", pagiantion.PageSize);
                AddParameter("@iPageIndex", pagiantion.PageIndex);
                AddParameter("@iGetCount", pagiantion.GetRowCount);
            }
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryExternalOrders", System.Data.CommandType.StoredProcedure))
            {
                result = reconstructExternalOrderListViews(reader);
            }
            if (pagiantion != null && pagiantion.GetRowCount)
            {
                pagiantion.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        IEnumerable<ExternalOrderListView> reconstructExternalOrderListViews(DbDataReader reader)
        {
            var result = new List<ExternalOrderListView>();
            ExternalOrderListView item = null;
            List<Guid> flights = null;
            while (reader.Read())
            {
                decimal currentOrderId = reader.GetDecimal(0);
                if (item == null || item.OrderId != currentOrderId)
                {
                    item = new ExternalOrderListView()
                    {
                        OrderId = currentOrderId,
                        Status = (OrderStatus)reader.GetInt32(1),
                        Source = (OrderSource)reader.GetByte(2),
                        ProductType = (ProductType)reader.GetByte(3),
                        ProducedTime = reader.GetDateTime(4),
                        ProducedAccount = reader.GetString(5),
                        Purchaser = reader.GetGuid(6),
                        PurchaserName = reader.GetString(7),
                        SettlementForPurchaser = new Settlement()
                        {
                            Rebate = reader.GetDecimal(8),
                            Commission = reader.GetDecimal(9),
                            Amount = reader.GetDecimal(10)
                        },
                        Choise = (AuthenticationChoise)reader.GetByte(43),
                        ProducedAccountName = reader.GetString(44),
                        Passengers = new List<string>(),
                        PassengerType = (PassengerType)reader.GetByte(47),
                        Flights = new List<FlightListView>(),
                    };
                    if (!reader.IsDBNull(48))
                    {
                        item.RefuseETDZTime = reader.GetDateTime(48);
                    }
                    if (!reader.IsDBNull(45))
                    {
                        item.PayTime = reader.GetDateTime(45);
                    }
                    if (!reader.IsDBNull(46))
                    {
                        item.ETDZTime = reader.GetDateTime(46);
                    }
                    if (!reader.IsDBNull(53))
                    {
                        item.SupplyTime = reader.GetDateTime(53);
                    }
                    if (!reader.IsDBNull(11))
                    {
                        item.Provider = reader.GetGuid(11);
                        item.ProviderName = reader.GetString(12);
                        item.SettlementForProvider = new Settlement()
                        {
                            Rebate = reader.GetDecimal(13),
                            Commission = reader.GetDecimal(14),
                            Amount = reader.GetDecimal(15)
                        };
                        item.OfficeNo = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        if (!reader.IsDBNull(17))
                        {
                            item.ETDZMode = (ETDZMode)reader.GetByte(17);
                        }
                    }
                    if (!reader.IsDBNull(18))
                    {
                        item.Supplier = reader.GetGuid(18);
                        item.SupplierName = reader.GetString(19);
                        if (!reader.IsDBNull(20))
                        {
                            item.SettlementForSupplier = new Settlement()
                            {
                                Rebate = reader.GetDecimal(20),
                                Commission = reader.GetDecimal(21),
                                Amount = reader.GetDecimal(22)
                            };
                        }
                    }
                    string bpnr = reader.IsDBNull(23) ? null : reader.GetString(23);
                    string pnr = reader.IsDBNull(24) ? null : reader.GetString(24);
                    string ebpnr = reader.IsDBNull(25) ? null : reader.GetString(25);
                    string epnr = reader.IsDBNull(26) ? null : reader.GetString(26);
                    if (reader.IsDBNull(42))
                    {
                        item.PurcharseProviderRelation = null;
                    }
                    else
                    {
                        item.PurcharseProviderRelation = (RelationType)reader.GetByte(42);
                    }
                    if (!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr))
                    {
                        item.ReservationPNR = new PNRPair(pnr, bpnr);
                    }
                    if (!string.IsNullOrWhiteSpace(ebpnr) || !string.IsNullOrWhiteSpace(epnr))
                    {
                        item.ETDZPNR = new PNRPair(epnr, ebpnr);
                    }
                    flights = new List<Guid>();
                    result.Add(item);
                }
                string passengerName = reader.GetString(27);
                if (!item.Passengers.Contains(passengerName))
                {
                    item.Passengers.Add(passengerName);
                }
                Guid flightId = reader.GetGuid(28);
                if (!flights.Contains(flightId))
                {
                    var flightView = new FlightListView()
                    {
                        DepartureCity = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                        DepartureAirport = reader.GetString(30),
                        ArrivalCity = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                        ArrivalAirport = reader.GetString(32),
                        Carrier = reader.GetString(33),
                        FlightNo = reader.GetString(34),
                        Bunk = reader.GetString(35),
                        Discount = reader.GetDecimal(36),
                        Fare = reader.GetDecimal(38),
                        AirportFee = reader.GetDecimal(39),
                        BAF = reader.GetDecimal(40),
                        TakeoffTime = reader.GetDateTime(41)
                    };
                    flightView.ReleasedFare = reader.IsDBNull(37) ? flightView.Fare : reader.GetDecimal(37);
                    item.Flights.Add(flightView);
                    flights.Add(flightId);
                }
                item.PlatformType = (PlatformType)reader.GetByte(49);
                item.ExternalOrderId = reader.GetString(50);
                item.PayStatus = (PayStatus)reader.GetByte(51);
                item.IsAutoPay = reader.GetBoolean(52);
            }
            return result;
        }

        /// <summary>
        /// 保存外平台的生成订单的政策副本
        /// </summary>
        /// <param name="externalPolicy">外平台政策信息</param>
        /// <param name="orderId"> </param>
        /// <returns></returns>
        public bool SaveExternalPolicyCopy(ExternalPolicyView externalPolicy, decimal orderId)
        {
            ClearParameters();
            var sql = "insert into [T_ExternalPolicyCopy] values (@OrderId,@Id,@Platform,@Provider,@TicketType,@RequireChangePNR,@Condition,@ETDZSpeed,@WorkStart,@WorkEnd,@ScrapStart,@ScrapEnd,@OriginalRebate,@Rebate,@ParValue,@OfficeNo,@RequireAuth,@RecordDate,@OriginalPolicyContent,@WorkTimeStart,@WorkTimeEnd,@RestWorkTimeStart,@RestWorkTimeEnd,@WorkRefundTimeStart,@WorkRefundTimeEnd,@RestRefundTimeStart,@RestRefundTimeEnd)";
            AddParameter("@Id", externalPolicy.Id);
            AddParameter("@Platform", (byte)externalPolicy.Platform);
            AddParameter("@Provider", externalPolicy.Provider);
            AddParameter("@TicketType", (byte)externalPolicy.TicketType);
            AddParameter("@RequireChangePNR", externalPolicy.RequireChangePNR);
            AddParameter("@Condition", externalPolicy.Condition);
            AddParameter("@ETDZSpeed", externalPolicy.ETDZSpeed);
            AddParameter("@WorkStart", externalPolicy.WorkStart.ToString());
            AddParameter("@WorkEnd", externalPolicy.WorkEnd.ToString());
            AddParameter("@ScrapStart", externalPolicy.ScrapStart.ToString());
            AddParameter("@ScrapEnd", externalPolicy.ScrapEnd.ToString());
            AddParameter("@OriginalRebate", externalPolicy.OriginalRebate);
            AddParameter("@Rebate", externalPolicy.Rebate);
            AddParameter("@ParValue", externalPolicy.ParValue);
            AddParameter("@OfficeNo", externalPolicy.OfficeNo);
            AddParameter("@RequireAuth", externalPolicy.RequireAuth);
            AddParameter("@OrderId", orderId);
            AddParameter("@RecordDate", DateTime.Now);
            AddParameter("@OriginalPolicyContent", externalPolicy.OriginalContent);
            AddParameter("@WorkTimeStart", externalPolicy.WorkTimeStart.ToString());
            AddParameter("@WorkTimeEnd", externalPolicy.WorkTimeEnd.ToString());
            AddParameter("@RestWorkTimeStart", externalPolicy.RestWorkTimeStart.ToString());
            AddParameter("@RestWorkTimeEnd", externalPolicy.RestWorkTimeEnd.ToString());
            AddParameter("@WorkRefundTimeStart", externalPolicy.WorkRefundTimeStart.ToString());
            AddParameter("@WorkRefundTimeEnd", externalPolicy.WorkRefundTimeEnd.ToString());
            AddParameter("@RestRefundTimeStart", externalPolicy.RestRefundTimeStart.ToString());
            AddParameter("@RestRefundTimeEnd", externalPolicy.RestRefundTimeEnd.ToString());
            return ExecuteNonQuery(sql) > 0;
        }

        /// <summary>
        /// 记录订单已经发送了乘机人出票短信
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool SetPassengerMsgSended(decimal orderId)
        {
            ClearParameters();
            var sql = "UPDATE T_ORDER SET PassengerMsgSended=@PassengerMsgSended WHERE Id = @OrderId";
            AddParameter("@PassengerMsgSended", true);
            AddParameter("@OrderId", orderId);
            return ExecuteNonQuery(sql) > 0;

        }

        public ExternalOrder QueryExternalOrder(decimal orderId)
        {
            ClearParameters();
            AddParameter("ORDERID", orderId);
            var order = loadExternalOrderMainInfo("TOR.[Id]=@ORDERID");
            if (order != null)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
                LoadExternalOrderInfo(order);
            }
            return order;
        }

        public ExternalOrder LoadExternalInfo(ExternalOrder eorder)
        {
            ClearParameters();
            AddParameter("ORDERID", eorder.Id);
            var order = loadExternalOrderMainInfo("TOR.[Id]=@ORDERID");
            if (order != null)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
                LoadExternalOrderInfo(order);
            }
            return order;
        }

        public void PayExternalOrderSuccess(ExternalOrder externalOrderInfo)
        {
            ClearParameters();
            StringBuilder strSql = new StringBuilder("if not exists (select * from T_ExternalOrder where OrderId = @OrderId ");
            strSql.Append(" and PayStatus = @SuccessStatus) ");
            strSql.Append("update T_ExternalOrder set ");
            strSql.Append("PayStatus=@PayStatus,");
            strSql.Append("PayTradNO=@PayTradNO,");
            strSql.Append("PlatformType=@PlatformType,");
            strSql.Append("IsAutoPay=@IsAutoPay,");
            strSql.Append("PayTime=@PayTime,");
            strSql.Append("PayFailReason=@PayFailReason");
            strSql.Append(" where OrderId=@OrderId ");
            AddParameter("@SuccessStatus", (byte)PayStatus.Paied);
            AddParameter("@PayStatus", (byte)externalOrderInfo.PayStatus);
            if (!string.IsNullOrWhiteSpace(externalOrderInfo.PayTradNO))
            {
                AddParameter("@PayTradNO", externalOrderInfo.PayTradNO);
            }
            else
            {
                AddParameter("@PayTradNO", DBNull.Value);
            }
            AddParameter("@PlatformType", externalOrderInfo.Platform);
            AddParameter("@IsAutoPay", externalOrderInfo.IsAutoPay);
            if (externalOrderInfo.PayTime.HasValue)
            {
                AddParameter("@PayTime", externalOrderInfo.PayTime);
            }
            else
            {
                AddParameter("@PayTime", DBNull.Value);
            }
            AddParameter("@PayFailReason", externalOrderInfo.FaildInfo ?? string.Empty);
            AddParameter("@OrderId", externalOrderInfo.Id);
            ExecuteNonQuery(strSql);
        }

        public ExternalOrder LoadExternalInfo(string externalOrderId, ExternalOrder order)
        {
            ClearParameters();
            var sql = "select top 1 OrderId,ExternalOrderId,ECommission,PayStatus,PayTradNO,PlatformType,IsAutoPay,Amount,PayTime,PayFailReason from T_ExternalOrder  where ExternalOrderId=@externalOrderId ";
            AddParameter("externalOrderId", externalOrderId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    order.Id = reader.GetDecimal(0);
                    order.ExternalOrderId = reader.GetString(1);
                    order.ECommission = reader.GetDecimal(2);
                    order.PayStatus = (PayStatus)reader.GetByte(3);
                    order.PayTradNO = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                    order.Platform = (PlatformType)reader.GetByte(5);
                    order.IsAutoPay = reader.GetBoolean(6);
                    if (!reader.IsDBNull(7))
                    {
                        order.Amount = reader.GetDecimal(7);
                    }
                    if (!reader.IsDBNull(8))
                    {
                        order.PayTime = reader.GetDateTime(8);
                    }
                    order.FaildInfo = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                }
            }
            return order;
        }

        public ExternalOrder QueryExternalOrder(string externalOrderId)
        {
            var order = new ExternalOrder(0);
            LoadExternalInfo(externalOrderId, order);
            loadOrderMainInfo("TOR.Id = " + order.Id, order);
            if (order.Id!=0)
            {
                var pnrInfos = loadPNRInfos(order.Id);
                foreach (var pnr in pnrInfos)
                {
                    order.AddPNRInfo(pnr);
                }
            }
            else
            {
                return null;
            }
            return order;
        }

        /// <summary>
        /// 验证在指定时间内是否有存在相同编码的订单
        /// </summary>
        /// <param name="pnr"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="companyId"> </param>
        /// <returns></returns>
        public decimal ExistsPNR(PNRPair pnr, DateTime timeFrom, DateTime timeTo, Guid companyId) {
            ClearParameters();
            var sql = @"select top(1) ID from T_Order where ProducedTime < @TimeTo and ProducedTime > @TimeFrom
                    AND Purchaser = @CompanyId AND (Status = 8 OR Status = 32 OR Status = 256) 
                    AND (@PNR is null OR PNR = @PNR) and (@BPNR is null OR BPNR = @BPNR)";
            AddParameter("TimeFrom", timeFrom);
            AddParameter("TimeTo", timeTo);
            AddParameter("CompanyId", companyId);
            if (!string.IsNullOrEmpty(pnr.PNR))
            {
                AddParameter("PNR", pnr.PNR);
            }
            else
            {
                AddParameter("PNR", DBNull.Value);
            }
            if (!string.IsNullOrEmpty(pnr.BPNR))
            {
                AddParameter("BPNR", pnr.BPNR);
            }
            else
            {
                AddParameter("BPNR", DBNull.Value);
            }
            var result = ExecuteScalar(sql);
            if (result == null) return 0;
            return (decimal)ExecuteScalar(sql) ;
        }

        public ExternalPolicyView QueryExternalPolicy(decimal orderId) {
            ClearParameters();
            ExternalPolicyView policy = null;
            var sql = @"SELECT OrderId, Id, Platform, Provider, TicketType, RequireChangePNR, Condition, ETDZSpeed, WorkStart, WorkEnd, ScrapStart, ScrapEnd, OriginalRebate, Rebate, ParValue, 
                      OfficeNo, RequireAuth, RecordDate, OriginalPolicyContent, WorkTimeStart, WorkTimeEnd, RestWorkTimeStart, RestWorkTimeEnd, WorkRefundTimeStart,WorkRefundTimeEnd, RestRefundTimeStart, RestRefundTimeEnd
FROM         T_ExternalPolicyCopy  where OrderId=@OrderId ";
            AddParameter("OrderId", orderId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    policy = new ExternalPolicyView();
                    policy.Platform = (PlatformType) reader.GetByte(2);
                    policy.Provider = reader.GetGuid(3);
                    policy.Id = reader.GetString(1);
                    policy.TicketType = (TicketType) reader.GetByte(4);
                    policy.PolicyType = PolicyType.Normal;
                    policy.RequireChangePNR = reader.GetBoolean(5);
                    policy.Remark = string.Empty;
                    policy.Condition = reader.GetString(6);
                    policy.ETDZSpeed = reader.GetInt32(7);
                    policy.WorkStart = ParseTime((TimeSpan?) (reader.GetValue(8)));
                    policy.WorkEnd = ParseTime((TimeSpan?) reader.GetValue(9));
                    policy.ScrapStart = ParseTime((TimeSpan?) reader.GetValue(10));
                    policy.ScrapEnd = ParseTime((TimeSpan?) reader.GetValue(11));
                    policy.WorkTimeStart = ParseTime((TimeSpan?)(reader.IsDBNull(19) ? (TimeSpan?)null : reader.GetValue(19)));
                    policy.WorkTimeEnd = ParseTime((TimeSpan?)(reader.IsDBNull(20) ? (TimeSpan?)null : reader.GetValue(20)));
                    policy.RestWorkTimeStart = ParseTime((TimeSpan?)(reader.IsDBNull(21) ? (TimeSpan?)null : reader.GetValue(21)));
                    policy.RestWorkTimeEnd = ParseTime((TimeSpan?)(reader.IsDBNull(22) ? (TimeSpan?)null : reader.GetValue(22)));
                    policy.WorkRefundTimeStart = ParseTime((TimeSpan?)(reader.IsDBNull(23) ? (TimeSpan?)null : reader.GetValue(23)));
                    policy.WorkRefundTimeEnd = ParseTime((TimeSpan?)(reader.IsDBNull(24) ? (TimeSpan?)null : reader.GetValue(24)));
                    policy.RestRefundTimeStart = ParseTime((TimeSpan?)(reader.IsDBNull(25) ? (TimeSpan?)null : reader.GetValue(25)));
                    policy.RestRefundTimeEnd = ParseTime((TimeSpan?)(reader.IsDBNull(26) ? (TimeSpan?)null : reader.GetValue(26)));
                    policy.OriginalRebate = reader.GetDecimal(12);
                    policy.Rebate = reader.GetDecimal(13);
                    policy.ParValue = reader.GetDecimal(14);
                    policy.OfficeNo = reader.GetString(15);
                    policy.RequireAuth = reader.GetBoolean(16);
                    policy.OriginalContent = reader.GetString(18);
                }
            }
            return policy;
        
        }

        private Time ParseTime(TimeSpan? time)
        {
            if (time.HasValue)
            {
                return new Time(time.Value.Hours, time.Value.Minutes,0);
            }
            return new Time(0, 0,0);
        }

        private void LoadExternalOrderInfo(ExternalOrder order)
        {
            ClearParameters();
            var sql = "select top 1 OrderId,ExternalOrderId,ECommission,PayStatus,PayTradNO,PlatformType,IsAutoPay,Amount,PayTime,PayFailReason from T_ExternalOrder  where OrderId=@OrderId ";
            AddParameter("OrderId", order.Id);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    order.ExternalOrderId = reader.GetString(1);
                    order.ECommission = reader.GetDecimal(2);
                    order.PayStatus = (PayStatus)reader.GetByte(3);
                    if (!reader.IsDBNull(4))
                    {
                        order.PayTradNO = reader.GetString(4);
                    }
                    order.Platform = (PlatformType)reader.GetByte(5);
                    order.IsAutoPay = reader.GetBoolean(6);
                    if (!reader.IsDBNull(7))
                    {
                        order.Amount = reader.GetDecimal(7);
                    }
                    if (!reader.IsDBNull(8))
                    {
                        order.PayTime = reader.GetDateTime(8);
                    }
                    order.FaildInfo = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                }
            }
        }

        public bool Reminded(decimal orderId, string remindContent)
        {
            ClearParameters();
            var sql = "UPDATE T_ORDER SET RemindTime=@RemindTime,RemindContent=@RemindContent,IsNeedReminded=@IsNeedReminded WHERE Id = @OrderId";
            AddParameter("@RemindTime", DateTime.Now);
            AddParameter("@RemindContent",remindContent);
            AddParameter("@IsNeedReminded",true);
            AddParameter("@OrderId", orderId);
            return ExecuteNonQuery(sql) > 0;
        }


        public bool UpdateRemindStatus(decimal orderId)
        {
            ClearParameters();
            var sql = "UPDATE T_ORDER SET IsNeedReminded=@IsNeedReminded WHERE Id = @OrderId";
            AddParameter("@IsNeedReminded", false);
            AddParameter("@OrderId", orderId);
            return ExecuteNonQuery(sql) > 0;
        }

    }
}
