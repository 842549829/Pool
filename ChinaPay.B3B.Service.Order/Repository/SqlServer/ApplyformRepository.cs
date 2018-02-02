using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class ApplyformRepository : SqlServerTransaction, IApplyformRepository {
        public ApplyformRepository(DbOperator dbOperator)
            : base(dbOperator) {
        }

        public void Insert(Order.Domain.Applyform.BaseApplyform applyform) {
            var pnrs = insertApplyformMainInfo(applyform);
            insertApplyformPNRItems(applyform.OrderId, applyform.Id, pnrs);
            insertPassengersOfApplyform(applyform);
            insertFlightsOfApplyform(applyform);
        }
        IEnumerable<string> insertApplyformMainInfo(Order.Domain.Applyform.BaseApplyform applyform) {
            ClearParameters();
            var pnrList = new List<string>();
            var applyformMainInfoSql = "INSERT INTO dbo.T_Applyform (Id,OrderId,BPNR,PNR,ProductType,[Type],RefundType,[Status],ProcessStatus,[Provider],[ProviderName]," +
                        "Purchaser,PurchaserName,Supplier,SupplierName,ApplyRemark,ApplierAccount,ApplierAccountName,AppliedTime,IsInterior,RequireRevisePrice,OEMID,AssociateApplyform)" +
                      " VALUES (@Id,@OrderId,@BPNR,@PNR,@ProductType,@Type,@RefundType,@Status,@ProcessStatus,@Provider,@ProviderName,@Purchaser,@PurchaserName," +
                      "@Supplier,@SupplierName,@ApplyRemark,@ApplierAccount,@ApplierAccountName,@AppliedTime,@IsInterior,@RequireRevisePrice,@OEMID,@AssociateApplyform)";
            ClearParameters();
            AddParameter("Id", applyform.Id);
            AddParameter("OrderId", applyform.OrderId);
            if(string.IsNullOrWhiteSpace(applyform.OriginalPNR.BPNR)) {
                AddParameter("BPNR", DBNull.Value);
            } else {
                AddParameter("BPNR", applyform.OriginalPNR.BPNR);
                pnrList.Add(applyform.OriginalPNR.BPNR);
            }
            if(string.IsNullOrWhiteSpace(applyform.OriginalPNR.PNR)) {
                AddParameter("PNR", DBNull.Value);
            } else {
                AddParameter("PNR", applyform.OriginalPNR.PNR);
                pnrList.Add(applyform.OriginalPNR.PNR);
            }
            if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Order.Domain.Applyform.RefundOrScrapApplyform;
                AddParameter("Status", (byte)refundOrScrapApplyform.Status);
                if(applyform is Order.Domain.Applyform.ScrapApplyform) {
                    AddParameter("Type", (byte)ApplyformType.Scrap);
                    AddParameter("RefundType", DBNull.Value);
                } else {
                    var refundApplyform = applyform as Order.Domain.Applyform.RefundApplyform;
                    AddParameter("Type", (byte)ApplyformType.Refund);
                    AddParameter("RefundType", (byte)refundApplyform.RefundType);
                    if (refundApplyform.ApplyAttachment!=null) insertApplyAttachment(refundApplyform.ApplyAttachment, refundApplyform.Id);
                }
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                var postponeApplyform = applyform as Order.Domain.Applyform.PostponeApplyform;
                AddParameter("Status", (byte)postponeApplyform.Status);
                AddParameter("Type", (byte)ApplyformType.Postpone);
                AddParameter("RefundType", DBNull.Value);
            }
            else if(applyform is BalanceRefundApplyform)
            {
                var balanceRefundApplyform = applyform as Order.Domain.Applyform.BalanceRefundApplyform;
                AddParameter("Status", (byte)balanceRefundApplyform.BalanceRefundStatus);
                AddParameter("Type", (byte)ApplyformType.BlanceRefund);
                AddParameter("RefundType", DBNull.Value);

            }
            AddParameter("ProductType", (byte)applyform.ProductType);
            AddParameter("ProcessStatus", (byte)applyform.ProcessStatus);
            AddParameter("Provider", applyform.ProviderId);
            AddParameter("ProviderName", applyform.ProviderName);
            AddParameter("Purchaser", applyform.PurchaserId);
            AddParameter("PurchaserName", applyform.PurchaserName);
            if(applyform.Order.Supplier == null) {
                AddParameter("Supplier", DBNull.Value);
                AddParameter("SupplierName", DBNull.Value);
            } else {
                AddParameter("Supplier", applyform.Order.Supplier.CompanyId);
                AddParameter("SupplierName", applyform.Order.Supplier.Name);
            }
            AddParameter("ApplyRemark", applyform.ApplyRemark);
            AddParameter("ApplierAccount", applyform.ApplierAccount);
            AddParameter("ApplierAccountName", applyform.ApplierAccountName);
            AddParameter("AppliedTime", applyform.AppliedTime);
            AddParameter("IsInterior", applyform.IsInterior);
            AddParameter("RequireRevisePrice", applyform.RequireRevisePrice);
            AddParameter("AssociateApplyform", applyform.AssociateApplyformId);
            if (applyform.OEMID.HasValue)
            {
                AddParameter("OEMID", applyform.OEMID);
            }
            else
            {
                AddParameter("OEMID", DBNull.Value);
            }
            ExecuteNonQuery(applyformMainInfoSql);
            return pnrList;
        }
        void insertApplyAttachment(List<ApplyAttachmentView> applyAttachmentView, decimal? ApplyformId)
        {
            foreach (var item in applyAttachmentView)
            {
                string sql = "INSERT INTO T_ApplyAttachment(Id,ApplyformId,FilePath,Thumbnail) VALUES(@ApplyAttachmentId,@ApplyformId,@FilePath,@Thumbnail);";
                AddParameter("ApplyAttachmentId", Guid.NewGuid());
                AddParameter("ApplyformId", ApplyformId == null ? item.ApplyformId : ApplyformId);
                if (string.IsNullOrEmpty(item.FilePath))
                {
                    AddParameter("FilePath", DBNull.Value, System.Data.DbType.String);
                }
                else 
                {
                    AddParameter("FilePath", item.FilePath, System.Data.DbType.String);
                }
                if (item.Thumbnail == null)
                {
                    AddParameter("Thumbnail", DBNull.Value, System.Data.DbType.Binary);
                }
                else 
                {
                    AddParameter("Thumbnail", item.Thumbnail, System.Data.DbType.Binary);
                }
                ExecuteNonQuery(sql);
            }
        }
        void insertApplyformPNRItems(decimal orderId, decimal applyformId, IEnumerable<string> pnrs) {
            ClearParameters();
            if(!pnrs.Any()) return;
            var sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.T_OrderPNRItem (OrderId,ApplyformId,PNR)");
            int index = 0;
            foreach(var item in pnrs) {
                sql.AppendFormat(" SELECT @OrderId,@ApplyformId,@PNR{0} UNION ALL", index);
                AddParameter("PNR" + index, item.Trim());
                index++;
            }
            sql.Remove(sql.Length - 10, 10);
            AddParameter("OrderId", orderId);
            AddParameter("ApplyformId", applyformId);
            ExecuteNonQuery(sql);
        }
        void insertPassengersOfApplyform(Order.Domain.Applyform.BaseApplyform applyform) {
            if (!applyform.Passengers.Any()) return;
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.T_ApplyPassenger (ApplyformId,Passenger,Name)");
            var index = 0;
            foreach(var passenger in applyform.Passengers) {
                sql.AppendFormat(" SELECT @ApplyformId,@Passenger{0},@Name{0} UNION ALL", index);
                AddParameter("Passenger" + index, passenger.Id);
                AddParameter("Name" + index, passenger.Name);
                index++;
            }
            AddParameter("ApplyformId", applyform.Id);
            sql.Remove(sql.Length - 10, 10);
            ExecuteNonQuery(sql);
        }
        void insertFlightsOfApplyform(Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                insertRefundFlightsOfApplyform(applyform.Id, (applyform as Order.Domain.Applyform.RefundOrScrapApplyform).Flights);
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                insertPostponeFlightsOfApplyform(applyform.OrderId, applyform.Id, (applyform as Order.Domain.Applyform.PostponeApplyform).Flights);
            } else if (applyform is BalanceRefundApplyform)
            {}else 
            {
                throw new CustomException("未知申请单类型");
            }
        }
        void insertRefundFlightsOfApplyform(decimal refundApplyformId, IEnumerable<Order.Domain.Applyform.RefundFlight> refundFlights) {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.T_ApplyFlight (ApplyformId,OriginalFlight,DepartureCityName,DepartureAirportName,ArrivalCityName,");
            sql.Append("ArrivalAirportName,Carrier,FlightNo,TakeoffTime,LandingTime,Bunk,Discount,ReleasedFare,Fare,AirportFee,BAF,RefundServiceCharge)");
            var index = 0;
            foreach(var item in refundFlights) {
                sql.AppendFormat(" SELECT @ApplyformId,@OriginalFlight{0},@DepartureCityName{0},@DepartureAirportName{0},@ArrivalCityName{0},@ArrivalAirportName{0},", index);
                sql.AppendFormat("@Carrier{0},@FlightNo{0},@TakeoffTime{0},@LandingTime{0},@Bunk{0},@Discount{0},@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@RefundServiceCharge{0} UNION ALL", index);
                AddParameter("OriginalFlight" + index, item.OriginalFlight.Id);
                AddParameter("DepartureCityName" + index, item.OriginalFlight.Departure.City);
                AddParameter("DepartureAirportName" + index, item.OriginalFlight.Departure.Name);
                AddParameter("ArrivalCityName" + index, item.OriginalFlight.Arrival.City);
                AddParameter("ArrivalAirportName" + index, item.OriginalFlight.Arrival.Name);
                AddParameter("Carrier" + index, item.OriginalFlight.Carrier.Code);
                AddParameter("FlightNo" + index, item.OriginalFlight.FlightNo);
                AddParameter("TakeoffTime" + index, item.OriginalFlight.TakeoffTime);
                AddParameter("LandingTime" + index, item.OriginalFlight.LandingTime);
                AddParameter("Bunk" + index, item.OriginalFlight.Bunk.Code);
                AddParameter("Discount" + index, item.OriginalFlight.Bunk.Discount);
                AddParameter("Fare" + index, item.OriginalFlight.Bunk.Fare);
                AddParameter("AirportFee" + index, item.OriginalFlight.AirportFee);
                AddParameter("BAF" + index, item.OriginalFlight.BAF);
                if(item.OriginalFlight.Bunk is Domain.Bunk.SpecialBunk) {
                    AddParameter("ReleasedFare" + index, (item.OriginalFlight.Bunk as Domain.Bunk.SpecialBunk).ReleasedFare);
                } else {
                    AddParameter("ReleasedFare" + index, DBNull.Value);
                }
                if(item.RefundServiceCharge.HasValue)
                {
                    AddParameter("RefundServiceCharge" + index, item.RefundServiceCharge.Value);
                }else
                {
                    AddParameter("RefundServiceCharge" + index, DBNull.Value);
                }
                index++;
            }
            AddParameter("ApplyformId", refundApplyformId);
            sql.Remove(sql.Length - 10, 10);
            ExecuteNonQuery(sql);
        }
        void insertPostponeFlightsOfApplyform(decimal orderId, decimal postponeApplyformId, IEnumerable<Order.Domain.Applyform.PostponeFlight> postponeFlights) {
            insertPostponeApplyformOriginalFlights(postponeApplyformId, postponeFlights);
            insertPostponeApplyformNewFlights(orderId, postponeFlights);
        }
        void insertPostponeApplyformOriginalFlights(decimal postponeApplyformId, IEnumerable<Order.Domain.Applyform.PostponeFlight> postponeFlights) {
            ClearParameters();
            var originalFlightsSql = new StringBuilder();
            originalFlightsSql.Append("INSERT INTO dbo.T_ApplyFlight");
            originalFlightsSql.Append("(ApplyformId,OriginalFlight,DepartureCityName,DepartureAirportName,ArrivalCityName,ArrivalAirportName,");
            originalFlightsSql.Append("Carrier,FlightNo,TakeoffTime,LandingTime,Bunk,Discount,ReleasedFare,Fare,AirportFee,BAF,PostponeFlight)");
            var index = 0;
            foreach(var item in postponeFlights) {
                originalFlightsSql.AppendFormat(" SELECT @ApplyformId,@OriginalFlight{0},@DepartureCityName{0},@DepartureAirportName{0},@ArrivalCityName{0},", index);
                originalFlightsSql.AppendFormat("@ArrivalAirportName{0},@Carrier{0},@FlightNo{0},@TakeoffTime{0},@LandingTime{0},@Bunk{0},@Discount{0},", index);
                originalFlightsSql.AppendFormat("@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@PostponeFlight{0} UNION ALL", index);
                AddParameter("OriginalFlight" + index, item.OriginalFlight.Id);
                AddParameter("DepartureCityName" + index, item.OriginalFlight.Departure.City);
                AddParameter("DepartureAirportName" + index, item.OriginalFlight.Departure.Name);
                AddParameter("ArrivalCityName" + index, item.OriginalFlight.Arrival.City);
                AddParameter("ArrivalAirportName" + index, item.OriginalFlight.Arrival.Name);
                AddParameter("Carrier" + index, item.OriginalFlight.Carrier.Code);
                AddParameter("FlightNo" + index, item.OriginalFlight.FlightNo);
                AddParameter("TakeoffTime" + index, item.OriginalFlight.TakeoffTime);
                AddParameter("LandingTime" + index, item.OriginalFlight.LandingTime);
                AddParameter("Bunk" + index, item.OriginalFlight.Bunk.Code);
                AddParameter("Discount" + index, item.OriginalFlight.Bunk.Discount);
                AddParameter("Fare" + index, item.OriginalFlight.Bunk.Fare);
                AddParameter("AirportFee" + index, item.OriginalFlight.AirportFee);
                AddParameter("BAF" + index, item.OriginalFlight.BAF);
                if(item.OriginalFlight.Bunk is Domain.Bunk.SpecialBunk) {
                    AddParameter("ReleasedFare" + index, (item.OriginalFlight.Bunk as Domain.Bunk.SpecialBunk).ReleasedFare);
                } else {
                    AddParameter("ReleasedFare" + index, DBNull.Value);
                }
                AddParameter("PostponeFlight" + index, item.NewFlight.Id);
                index++;
            }
            AddParameter("ApplyformId", postponeApplyformId);
            originalFlightsSql.Remove(originalFlightsSql.Length - 10, 10);
            ExecuteNonQuery(originalFlightsSql);
        }
        void insertPostponeApplyformNewFlights(decimal orderId, IEnumerable<Order.Domain.Applyform.PostponeFlight> postponeFlights) {
            ClearParameters();
            var postponeFlightsSql = new StringBuilder();
            postponeFlightsSql.Append("INSERT INTO [dbo].[T_Flight]");
            postponeFlightsSql.Append("([OrderId],[Id],[Serial],[Departure],[DepartureCityName],[DepartureAirportName],[Arrival],[ArrivalCityName],[ArrivalAirportName],");
            postponeFlightsSql.Append("[Carrier],[CarrierName],[SettleCode],[FlightNo],[TakeoffTime],[LandingTime],[AirCraft],[Bunk],[Discount],[BunkType],");
            postponeFlightsSql.Append("[BunkDescription],[EI],[YBPrice],[ReleasedFare],[Fare],[AirportFee],[BAF],[AssociateFlight],ReservateFlight,Ticket,IsShare,Increasing)");
            var index = 0;
            foreach(var item in postponeFlights) {
                postponeFlightsSql.AppendFormat(" SELECT @ORDERID,@ID{0},@SERIAL{0},@DEPARTURE{0},@DEPARTURECITYNAME{0},@DEPARTUREAIRPORTNAME{0},@ARRIVAL{0},@ARRIVALCITYNAME{0},@ARRIVALAIRPORTNAME{0},", index);
                postponeFlightsSql.AppendFormat("@CARRIER{0},@CARRIERNAME{0},@SETTLECODE{0},@FLIGHTNO{0},@TAKEOFFTIME{0},@LANDINGTIME{0},@AIRCRAFT{0},@BUNK{0},@DISCOUNT{0},", index);
                postponeFlightsSql.AppendFormat("@BUNKTYPE{0},@BUNKDESCRIPTION{0},@EI{0},@YBPRICE{0},@RELEASEDFARE{0},@FARE{0},@AIRPORTFEE{0},@BAF{0},@ASSOCIATEFLIGHT{0},@ReservateFlight{0},@Ticket{0},@IsShare{0},@Increasing{0} UNION ALL", index);
                AddParameter("ID" + index, item.NewFlight.Id);
                AddParameter("SERIAL" + index, item.NewFlight.Serial);
                AddParameter("DEPARTURE" + index, item.NewFlight.Departure.Code);
                AddParameter("DEPARTURECITYNAME" + index, item.NewFlight.Departure.City);
                AddParameter("DEPARTUREAIRPORTNAME" + index, item.NewFlight.Departure.Name);
                AddParameter("ARRIVAL" + index, item.NewFlight.Arrival.Code);
                AddParameter("ARRIVALCITYNAME" + index, item.NewFlight.Arrival.City);
                AddParameter("ARRIVALAIRPORTNAME" + index, item.NewFlight.Arrival.Name);
                AddParameter("CARRIER" + index, item.NewFlight.Carrier.Code);
                AddParameter("CARRIERNAME" + index, item.NewFlight.Carrier.Name);
                AddParameter("SETTLECODE" + index, item.NewFlight.Carrier.SettleCode);
                AddParameter("FLIGHTNO" + index, item.NewFlight.FlightNo);
                AddParameter("TAKEOFFTIME" + index, item.NewFlight.TakeoffTime);
                AddParameter("LANDINGTIME" + index, item.NewFlight.LandingTime);
                AddParameter("AIRCRAFT" + index, item.NewFlight.AirCraft ?? string.Empty);
                AddParameter("YBPRICE" + index, item.NewFlight.YBPrice);
                AddParameter("AIRPORTFEE" + index, item.NewFlight.AirportFee);
                AddParameter("BAF" + index, item.NewFlight.BAF);
                AddParameter("BUNK" + index, item.NewFlight.Bunk.Code);
                AddParameter("FARE" + index, item.NewFlight.Bunk.Fare);
                AddParameter("DISCOUNT" + index, item.NewFlight.Bunk.Discount);
                AddParameter("EI" + index, item.NewFlight.Bunk.EI);
                AddParameter("Increasing" + index, item.NewFlight.Increasing);
                if(item.NewFlight.Bunk is Domain.Bunk.FirstOrBusinessBunk) {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.FirstOrBusiness);
                    AddParameter("BUNKDESCRIPTION" + index, (item.NewFlight.Bunk as Domain.Bunk.FirstOrBusinessBunk).Description ?? string.Empty);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                } else if(item.NewFlight.Bunk is Domain.Bunk.EconomicBunk) {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Economic);
                    AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                } else if(item.NewFlight.Bunk is Domain.Bunk.PromotionBunk) {
                    var promotionBunk = item.NewFlight.Bunk as Domain.Bunk.PromotionBunk;
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Promotion);
                    AddParameter("BUNKDESCRIPTION" + index, promotionBunk.Description ?? string.Empty);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                } else if(item.NewFlight.Bunk is Domain.Bunk.ProductionBunk) {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Production);
                    AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                } else if(item.NewFlight.Bunk is Domain.Bunk.SpecialBunk) {
                    var specialBunk = item.NewFlight.Bunk as Domain.Bunk.SpecialBunk;
                    if(item.NewFlight.Bunk is Domain.Bunk.FreeBunk) {
                        AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Free);
                        AddParameter("BUNKDESCRIPTION" + index, (item.NewFlight.Bunk as Domain.Bunk.FreeBunk).Description ?? string.Empty);
                    } else {
                        AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Speical);
                        AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                    }
                    AddParameter("RELEASEDFARE" + index, specialBunk.ReleasedFare);
                } else if(item.NewFlight.Bunk is Domain.Bunk.TeamBunk) {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Team);
                    AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                }
                else if (item.NewFlight.Bunk is Domain.Bunk.TransferBunk)
                {
                    AddParameter("BUNKTYPE" + index, (byte)Order.Domain.Bunk.BunkType.Transfer);
                    AddParameter("BUNKDESCRIPTION" + index, DBNull.Value);
                    AddParameter("RELEASEDFARE" + index, DBNull.Value);
                }
                AddParameter("ASSOCIATEFLIGHT" + index, item.OriginalFlight.Id);
                AddParameter("ReservateFlight" + index, item.OriginalFlight.ReservateFlight);
                AddParameter("Ticket" + index, item.OriginalFlight.Ticket.Serial);
                AddParameter("IsShare" + index, item.NewFlight.IsShare);
                index++;
            }
            AddParameter("OrderId", orderId);
            postponeFlightsSql.Remove(postponeFlightsSql.Length - 10, 10);
            ExecuteNonQuery(postponeFlightsSql);
        }

        public void Update(Domain.Applyform.BaseApplyform applyform) {
            updateApplyformMainInfo(applyform);
            updateFlightsOfApplyform(applyform);
        }
        void updateApplyformMainInfo(Order.Domain.Applyform.BaseApplyform applyform) {
            ClearParameters();
            string sql;
            if(string.IsNullOrEmpty(applyform.OperatorAccount)) {
                sql = "UPDATE [dbo].[T_Applyform] SET [NewBPNR]=@NEWBPNR,[NewPNR]=@NEWPNR,[ApplyRemark]=@APPLYREMARK,[Status]=@STATUS,[ProcessStatus]=@PROCESSSTATUS," +
                    "[RefundType]=@REFUNDTYPE,[ProcessedTime]=@PROCESSEDTIME,[ProcessedFailedReason]=@PROCESSEDFAILEDREASON," +
                    "RequireRevisePrice=@RequireRevisePrice WHERE [Id]=@APPLYFORMID";
            } else {
                sql = "UPDATE [dbo].[T_Applyform] SET [NewBPNR]=@NEWBPNR,[NewPNR]=@NEWPNR,[ApplyRemark]=@APPLYREMARK,[Status]=@STATUS,[ProcessStatus]=@PROCESSSTATUS," +
                    "[RefundType]=@REFUNDTYPE,[ProcessedTime]=@PROCESSEDTIME,[ProcessedFailedReason]=@PROCESSEDFAILEDREASON," +
                    "[OperatorName]=@Operator,[OperatorAccount]=@OperatorAccount,RequireRevisePrice=@RequireRevisePrice WHERE [Id]=@APPLYFORMID";
            }
            if(PNRPair.IsNullOrEmpty(applyform.NewPNR) || string.IsNullOrWhiteSpace(applyform.NewPNR.BPNR)) {
                AddParameter("NEWBPNR", DBNull.Value);
            } else {
                AddParameter("NEWBPNR", applyform.NewPNR.BPNR);
            }
            if(PNRPair.IsNullOrEmpty(applyform.NewPNR) || string.IsNullOrWhiteSpace(applyform.NewPNR.PNR)) {
                AddParameter("NEWPNR", DBNull.Value);
            } else {
                AddParameter("NEWPNR", applyform.NewPNR.PNR);
            }
            AddParameter("APPLYREMARK", applyform.ApplyRemark ?? string.Empty);
            AddParameter("PROCESSSTATUS", (byte)applyform.ProcessStatus);
            if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Order.Domain.Applyform.RefundOrScrapApplyform;
                AddParameter("STATUS", (byte)refundOrScrapApplyform.Status);
                if(refundOrScrapApplyform is Order.Domain.Applyform.RefundApplyform) {
                    var refundApplyform = refundOrScrapApplyform as Order.Domain.Applyform.RefundApplyform;
                    AddParameter("REFUNDTYPE", (byte)refundApplyform.RefundType);
                } else {
                    AddParameter("REFUNDTYPE", DBNull.Value);
                }
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                var postponeApplyform = applyform as Order.Domain.Applyform.PostponeApplyform;
                AddParameter("STATUS", (byte)postponeApplyform.Status);
                AddParameter("AMOUNT", postponeApplyform.PostponeFee);
                AddParameter("REFUNDTYPE", DBNull.Value);
            }
            AddParameter("PROCESSEDFAILEDREASON", applyform.ProcessedFailedReason ?? string.Empty);
            if(applyform.ProcessedTime.HasValue) {
                AddParameter("PROCESSEDTIME", applyform.ProcessedTime.Value);
            } else {
                AddParameter("PROCESSEDTIME", DBNull.Value);
            }
            AddParameter("APPLYFORMID", applyform.Id);
            if(!string.IsNullOrEmpty(applyform.OperatorAccount)) {
                AddParameter("Operator", applyform.@Operator ?? string.Empty);
                AddParameter("OperatorAccount", applyform.OperatorAccount);
            }
            AddParameter("RequireRevisePrice", applyform.RequireRevisePrice);
            ExecuteNonQuery(sql);
        }
        void updateFlightsOfApplyform(Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                updateRefundOrScrapApplyformFlights(applyform as Order.Domain.Applyform.RefundOrScrapApplyform);
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                updatePostponeFlights(applyform as Order.Domain.Applyform.PostponeApplyform);
            }
        }
        void updateRefundOrScrapApplyformFlights(Order.Domain.Applyform.RefundOrScrapApplyform refundOrScrapApplyform) {
            ClearParameters();
            var sql = new StringBuilder();
            var index = 0;
            foreach(var flight in refundOrScrapApplyform.Flights) {
                sql.AppendFormat("UPDATE dbo.T_ApplyFlight SET RefundRate=@RefundRate{0},RefundServiceCharge=@RefundServiceCharge{0}", index);
                sql.AppendFormat(" WHERE [ApplyformId]=@ApplyformId AND [OriginalFlight]=@FlightId{0};", index);
                AddParameter("RefundRate" + index, flight.RefundRate);
                if(flight.RefundServiceCharge.HasValue)
                {
                    AddParameter("RefundServiceCharge" + index, flight.RefundServiceCharge.Value);
                }else
                {
                    AddParameter("RefundServiceCharge" + index, DBNull.Value);
                }
                AddParameter("FlightId" + index, flight.OriginalFlight.Id);
                index++;
            }
            AddParameter("ApplyformId", refundOrScrapApplyform.Id);
            ExecuteNonQuery(sql);
        }
        void updatePostponeFlights(Order.Domain.Applyform.PostponeApplyform postponeApplyform) {
            if(postponeApplyform.Flights.FirstOrDefault(item => item.NewFlight.TakeoffTime != item.OriginalFlight.TakeoffTime && item.NewFlight.LandingTime != DateTime.MinValue) == null)
                return;
            ClearParameters();
            var sql = new StringBuilder();
            var index = 0;
            foreach(var flight in postponeApplyform.Flights) {
                sql.AppendFormat("UPDATE dbo.T_ApplyFlight SET PostponeFee=@PostponeFee{0} WHERE ApplyformId=@ApplyformId AND OriginalFlight=@OriginalFlightId{0};", index);
                sql.AppendFormat("UPDATE dbo.T_Flight SET FlightNo=@FlightNo{0},IsShare=@IsShare{0},AirCraft=@AirCraft{0},TakeoffTime=@TakeoffTime{0},LandingTime=@LandingTime{0} WHERE Id=@PostponeFlightId{0};", index);
                AddParameter("PostponeFee" + index, flight.PostponeFee);
                AddParameter("OriginalFlightId" + index, flight.OriginalFlight.Id);
                AddParameter("FlightNo" + index, flight.NewFlight.FlightNo);
                AddParameter("IsShare" + index, flight.NewFlight.IsShare);
                AddParameter("AirCraft" + index, flight.NewFlight.AirCraft ?? string.Empty);
                AddParameter("TakeoffTime" + index, flight.NewFlight.TakeoffTime);
                AddParameter("LandingTime" + index, flight.NewFlight.LandingTime);
                AddParameter("PostponeFlightId" + index, flight.NewFlight.Id);
                index++;
            }
            AddParameter("ApplyformId", postponeApplyform.Id);
            ExecuteNonQuery(sql);
        }

        public void UpdateApplyformForRevicePrice(Domain.Applyform.BaseApplyform applyform) {
            ClearParameters();
            var sql = "UPDATE dbo.T_Applyform SET RequireRevisePrice=@RequireRevisePrice WHERE OrderId=@OrderId";
            AddParameter("RequireRevisePrice", applyform.RequireRevisePrice);
            AddParameter("OrderId", applyform.OrderId);
            ExecuteNonQuery(sql);
        }

        public Order.Domain.Applyform.BaseApplyform QueryApplyform(decimal applyformId) {
            var applyform = loadApplyformMainInfo(applyformId);
            if(applyform != null) {
                loadPassengersOfApplyform(applyform);
                loadFlightsOfApplyform(applyform);
                fillFlightInTicket(applyform);
            }
            return applyform;
        }
        Order.Domain.Applyform.BaseApplyform loadApplyformMainInfo(decimal applyformId) {
            ClearParameters();
            Order.Domain.Applyform.BaseApplyform applyform = null;
            var sql = "SELECT OrderId,Id,[Type],ProductType,BPNR,PNR,NewBPNR,NewPNR,[Provider],[ProviderName],Purchaser,PurchaserName,ApplyRemark,ApplierAccount," +
                "AppliedTime,ProcessedFailedReason,ProcessedTime,IsInterior,[Status],Amount,RefundType,RequireRevisePrice,ApplierAccountName,OEMID,HasBalanceRefund,AssociateApplyform FROM dbo.T_Applyform WHERE Id=@ApplyformId";
            AddParameter("ApplyformId", applyformId);
            using(var reader = ExecuteReader(sql)) {
                if(reader.Read()) {
                    applyform = constructApplyformMainInfo(reader);
                }
            }
            return applyform;
        }
        Order.Domain.Applyform.BaseApplyform constructApplyformMainInfo(DbDataReader reader) {
            Order.Domain.Applyform.BaseApplyform applyform = null;
            var orderId = reader.GetDecimal(0);
            var applyformId = reader.GetDecimal(1);
            var applyformType = (ApplyformType)reader.GetByte(2);
            if(applyformType == ApplyformType.Refund) {
                applyform = loadRefundApplyformMainInfo(orderId, applyformId, reader);
            } else if(applyformType == ApplyformType.Scrap) {
                applyform = loadScrapApplyformMainInfo(orderId, applyformId, reader);
            } else if(applyformType == ApplyformType.Postpone) {
                applyform = loadPostponeApplyformMainInfo(orderId, applyformId, reader);
            }
            else if(applyformType == ApplyformType.BlanceRefund)
            {
                applyform = loadBalanceRefundApplyformMainInfo(orderId, applyformId, reader);
            }
            if(applyform != null) {
                applyform.ProductType = (ProductType)reader.GetByte(3);
                var originalBPNR = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                var originalPNR = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                var newBPNR = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                var newPNR = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                if(!string.IsNullOrWhiteSpace(originalBPNR) || !string.IsNullOrWhiteSpace(originalPNR)) {
                    applyform.OriginalPNR = new PNRPair(originalPNR, originalBPNR);
                }
                if(!string.IsNullOrWhiteSpace(newBPNR) || !string.IsNullOrWhiteSpace(newPNR)) {
                    applyform.NewPNR = new PNRPair(newPNR, newBPNR);
                }
                applyform.ProviderId = reader.GetGuid(8);
                applyform.ProviderName = reader.GetString(9);
                applyform.PurchaserId = reader.GetGuid(10);
                applyform.PurchaserName = reader.GetString(11);
                applyform.ApplyRemark = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                applyform.ApplierAccount = reader.GetString(13);
                applyform.AppliedTime = reader.GetDateTime(14);
                applyform.ProcessedFailedReason = reader.IsDBNull(15) ? string.Empty : reader.GetString(15);
                if(!reader.IsDBNull(16)) {
                    applyform.ProcessedTime = reader.GetDateTime(16);
                }
                applyform.IsInterior = reader.GetBoolean(17);
                applyform.RequireRevisePrice = reader.GetBoolean(21);
                applyform.ApplierAccountName = reader.GetString(22);
                applyform.AssociateApplyformId = reader.GetDecimal(25);
                if (!reader.IsDBNull(23)) applyform.OEMID = reader.GetGuid(23);
            }
            return applyform;
        }
        Order.Domain.Applyform.RefundApplyform loadRefundApplyformMainInfo(decimal orderId, decimal applyformId, DbDataReader reader) {
            return new Order.Domain.Applyform.RefundApplyform(orderId, applyformId) {
                Status = (RefundApplyformStatus)reader.GetByte(18),
                RefundType = (RefundType)reader.GetByte(20),
                HasBalanceRefund = reader.GetBoolean(24)
            };
        }
        Order.Domain.Applyform.ScrapApplyform loadScrapApplyformMainInfo(decimal orderId, decimal applyformId, DbDataReader reader) {
            return new Order.Domain.Applyform.ScrapApplyform(orderId, applyformId) {
                Status = (RefundApplyformStatus)reader.GetByte(18),
                HasBalanceRefund = reader.GetBoolean(24)
            };
        }
        Order.Domain.Applyform.PostponeApplyform loadPostponeApplyformMainInfo(decimal orderId, decimal applyformId, DbDataReader reader) {
            var result = new Order.Domain.Applyform.PostponeApplyform(orderId, applyformId) {
                Status = (PostponeApplyformStatus)reader.GetByte(18)
            };
            if(!reader.IsDBNull(19)) {
                result.PostponeFee = reader.GetDecimal(19);
            }
            return result;
        }
        Order.Domain.Applyform.BalanceRefundApplyform loadBalanceRefundApplyformMainInfo(decimal orderId, decimal applyformId, DbDataReader reader)
        {
            return new Order.Domain.Applyform.BalanceRefundApplyform(orderId, applyformId)
            {
                BalanceRefundStatus = (BalanceRefundProcessStatus) reader.GetByte(18),
            };
        }

        void loadPassengersOfApplyform(Order.Domain.Applyform.BaseApplyform applyform) {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("SELECT [T_Passenger].[Id],[T_Passenger].[Name],[PassengerType],[Credentials],[CredentialsType],[Phone],");
            sql.Append("[T_Ticket].[Serial],[T_Ticket].[SettleCode],[T_Ticket].[TicketNo],[T_Ticket].[ETDZMode],[T_Ticket].[ETDZTime],[T_Ticket].Fare,[T_Ticket].AirportFee,[T_Ticket].BAF,T_Passenger.BirthDay");
            sql.Append(" FROM [dbo].[T_Passenger]");
            sql.Append(" INNER JOIN [dbo].[T_ApplyPassenger] ON [Id]=[Passenger]");
            sql.Append(" INNER JOIN [dbo].[T_Ticket] ON [T_Ticket].[Passenger]=[T_Passenger].[Id]");
            sql.Append(" WHERE [ApplyformId]=@APPLYFORMID");
            sql.Append(" AND EXISTS(SELECT NULL FROM dbo.T_ApplyFlight");
            sql.Append(" INNER JOIN dbo.T_Flight ON T_Flight.Id=T_ApplyFlight.OriginalFlight AND T_Flight.Ticket=T_Ticket.Serial");
            sql.Append(" WHERE T_ApplyFlight.ApplyformId=T_ApplyPassenger.ApplyformId)");
            sql.Append(" ORDER BY [T_Passenger].[Name],[T_Ticket].[Serial]");
            AddParameter("APPLYFORMID", applyform.Id);
            using(var reader = ExecuteReader(sql.ToString())) {
                Order.Domain.Passenger passenger = null;
                while(reader.Read()) {
                    var currentPassengerId = reader.GetGuid(0);
                    if(passenger == null || passenger.Id != currentPassengerId) {
                        passenger = new Order.Domain.Passenger(currentPassengerId) {
                            Name = reader.GetString(1),
                            PassengerType = (Common.Enums.PassengerType)reader.GetByte(2),
                            Credentials = reader.GetString(3),
                            CredentialsType = (Common.Enums.CredentialsType)reader.GetByte(4),
                            Phone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };
                        if (!reader.IsDBNull(14))
                        {
                            passenger.Birthday = reader.GetDateTime(14);
                        }
                        applyform.AddPassenger(passenger);
                    }
                    var ticket = new Order.Domain.Ticket() {
                        Serial = reader.GetInt32(6),
                        SettleCode = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        No = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Price = new Order.Domain.Price(reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                            reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                            reader.IsDBNull(13) ? 0 : reader.GetDecimal(13))
                    };
                    if(!reader.IsDBNull(9)) {
                        ticket.ETDZMode = (Common.Enums.ETDZMode)reader.GetByte(9);
                    }
                    if(!reader.IsDBNull(10)) {
                        ticket.ETDZTime = reader.GetDateTime(10);
                    }
                    passenger.AddTicket(ticket);
                }
            }
        }
        void loadFlightsOfApplyform(Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                loadFlightsOfRefundOrScrapApplyform(applyform as Order.Domain.Applyform.RefundOrScrapApplyform);
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                loadFlightsOfPostponeApplyform(applyform as Order.Domain.Applyform.PostponeApplyform);
            }
        }
        void loadFlightsOfRefundOrScrapApplyform(Order.Domain.Applyform.RefundOrScrapApplyform applyform) {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("SELECT [Id],[Serial],[T_Flight].[Departure],[T_Flight].[DepartureAirportName],[T_Flight].[DepartureCityName],[T_Flight].[Arrival],");
            sql.Append("[T_Flight].[ArrivalAirportName],[T_Flight].[ArrivalCityName],[T_Flight].[Carrier],[T_Flight].[CarrierName],[SettleCode],[T_Flight].[FlightNo],");
            sql.Append("[T_Flight].[TakeoffTime],[T_Flight].[LandingTime],[T_Flight].[AirCraft],[T_Flight].[YBPrice],[T_Flight].[AirportFee],[T_Flight].[BAF],");
            sql.Append("[T_Flight].[Bunk],[T_Flight].[BunkType],[T_Flight].[Fare],[T_Flight].[Discount],[EI],[BunkDescription],[T_Flight].[ReleasedFare],");
            sql.Append("[T_Flight].AssociateFlight,ReservateFlight,IsShare,RefundRate,RefundServiceCharge,[T_Flight].Increasing,[T_ApplyFlight].BanlanceFare");
            sql.Append(" FROM [dbo].[T_ApplyFlight]");
            sql.Append(" INNER JOIN [dbo].[T_Flight] ON [Id]=[OriginalFlight]");
            sql.Append(" WHERE [ApplyformId]=@APPLYFORMID");
            sql.Append(" ORDER BY [Serial]");
            AddParameter("APPLYFORMID", applyform.Id);
            using(var reader = ExecuteReader(sql.ToString())) {
                while(reader.Read()) {
                    var originalFlight = constructFlight(reader, 0);
                    var refundFlight = new Order.Domain.Applyform.RefundFlight() {
                        OriginalFlight = originalFlight,
                        RefundRate = reader.IsDBNull(28) ? 0 : reader.GetDecimal(28),
                        BanlanceFare = reader.IsDBNull(31) ? 0 : reader.GetDecimal(31)
                   };
                    if(!reader.IsDBNull(29)) refundFlight.RefundServiceCharge = reader.GetDecimal(29);
                    originalFlight.Increasing = reader.GetDecimal(30);
                    applyform.AddFlight(refundFlight);
                }
            }
        }
        void loadFlightsOfPostponeApplyform(Order.Domain.Applyform.PostponeApplyform applyform) {
            ClearParameters();
            var sql = new StringBuilder();
            sql.Append("SELECT TORIGINALFlight.[Id],TORIGINALFlight.[Serial],TORIGINALFlight.[Departure],TORIGINALFlight.[DepartureAirportName],TORIGINALFlight.[DepartureCityName],");
            sql.Append("TORIGINALFlight.[Arrival],TORIGINALFlight.[ArrivalAirportName],TORIGINALFlight.[ArrivalCityName],TORIGINALFlight.[Carrier],TORIGINALFlight.[CarrierName],");
            sql.Append("TORIGINALFlight.[SettleCode],TORIGINALFlight.[FlightNo],TORIGINALFlight.[TakeoffTime],TORIGINALFlight.[LandingTime],TORIGINALFlight.[AirCraft],");
            sql.Append("TORIGINALFlight.[YBPrice],TORIGINALFlight.[AirportFee],TORIGINALFlight.[BAF],TORIGINALFlight.[Bunk],TORIGINALFlight.[BunkType],TORIGINALFlight.[Fare],");
            sql.Append("TORIGINALFlight.[Discount],TORIGINALFlight.[EI],TORIGINALFlight.[BunkDescription],TORIGINALFlight.[ReleasedFare],TORIGINALFlight.AssociateFlight,TORIGINALFlight.ReservateFlight,TORIGINALFlight.IsShare,");
            sql.Append("TNEWFLIGHT.[Id],TNEWFLIGHT.[Serial],TNEWFLIGHT.[Departure],TNEWFLIGHT.[DepartureAirportName],TNEWFLIGHT.[DepartureCityName],TNEWFLIGHT.[Arrival],");
            sql.Append("TNEWFLIGHT.[ArrivalAirportName],TNEWFLIGHT.[ArrivalCityName],TNEWFLIGHT.[Carrier],TNEWFLIGHT.[CarrierName],TNEWFLIGHT.[SettleCode],TNEWFLIGHT.[FlightNo],");
            sql.Append("TNEWFLIGHT.[TakeoffTime],TNEWFLIGHT.[LandingTime],TNEWFLIGHT.[AirCraft],TNEWFLIGHT.[YBPrice],TNEWFLIGHT.[AirportFee],TNEWFLIGHT.[BAF],TNEWFLIGHT.[Bunk],TNEWFLIGHT.[BunkType],");
            sql.Append("TNEWFLIGHT.[Fare],TNEWFLIGHT.[Discount],TNEWFLIGHT.[EI],TNEWFLIGHT.[BunkDescription],TNEWFLIGHT.[ReleasedFare],TNEWFLIGHT.AssociateFlight,TNEWFLIGHT.ReservateFlight,TNEWFLIGHT.IsShare,[PostponeFee],TORIGINALFlight.Increasing,TNEWFLIGHT.Increasing");
            sql.Append(" FROM [dbo].[T_ApplyFlight]");
            sql.Append(" INNER JOIN [dbo].[T_Flight] TORIGINALFlight ON TORIGINALFlight.[Id]=[OriginalFlight]");
            sql.Append(" INNER JOIN [dbo].[T_Flight] TNEWFLIGHT ON TNEWFLIGHT.[Id]=[PostponeFlight]");
            sql.Append(" WHERE [ApplyformId]=@APPLYFORMID");
            sql.Append(" ORDER BY TORIGINALFlight.[Serial]");
            AddParameter("APPLYFORMID", applyform.Id);
            using(var reader = ExecuteReader(sql.ToString())) {
                while(reader.Read()) {
                    var originalFlight = constructFlight(reader, 0);
                    var newFlight = constructFlight(reader, 28);
                    var postponeFee = reader.IsDBNull(56) ? 0 : reader.GetDecimal(56);
                    originalFlight.Increasing = reader.GetDecimal(57);
                    newFlight.Increasing = reader.GetDecimal(58);
                    var postponeFlight = new Order.Domain.Applyform.PostponeFlight() {
                        OriginalFlight = originalFlight,
                        NewFlight = newFlight,
                        PostponeFee = postponeFee
                    };
                    applyform.AddFlight(postponeFlight);
                }
            }
        }
        Order.Domain.Flight constructFlight(DbDataReader reader, int startIndex) {
            var flight = new Order.Domain.Flight(reader.GetGuid(startIndex + 0),
                                                    new Order.Domain.Carrier(reader.GetString(startIndex + 8), reader.GetString(startIndex + 9), reader.GetString(startIndex + 10)),
                                                    new Order.Domain.Airport(reader.GetString(startIndex + 2), reader.GetString(startIndex + 3), reader.GetString(startIndex + 4)),
                                                    new Order.Domain.Airport(reader.GetString(startIndex + 5), reader.GetString(startIndex + 6), reader.GetString(startIndex + 7)),
                                                    reader.GetDateTime(startIndex + 12)) {
                                                        Serial = reader.GetInt32(startIndex + 1),
                                                        FlightNo = reader.IsDBNull(startIndex + 11) ? string.Empty : reader.GetString(startIndex + 11),
                                                        LandingTime = reader.IsDBNull(startIndex + 13) ? DateTime.MinValue : reader.GetDateTime(startIndex + 13),
                                                        AirCraft = reader.IsDBNull(startIndex + 14) ? string.Empty : reader.GetString(startIndex + 14),
                                                        YBPrice = reader.GetDecimal(startIndex + 15),
                                                        AirportFee = reader.GetDecimal(startIndex + 16),
                                                        BAF = reader.GetDecimal(startIndex + 17),
                                                        Increasing = 0,
                                                        ReleaseFare =  reader.GetDecimal(startIndex+24)
                                                    };
            Order.Domain.Bunk.BaseBunk bunk = null;
            var bunkCode = reader.IsDBNull(startIndex + 18) ? string.Empty : reader.GetString(startIndex + 18);
            var bunkType = (Order.Domain.Bunk.BunkType)reader.GetByte(startIndex + 19);
            var fare = reader.GetDecimal(startIndex + 20);
            var discount = reader.GetDecimal(startIndex + 21);
            var ei = reader.IsDBNull(startIndex + 22) ? string.Empty : reader.GetString(startIndex + 22);
            var description = reader.IsDBNull(startIndex + 23) ? string.Empty : reader.GetString(startIndex + 23);
            switch(bunkType) {
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
                    bunk = new Order.Domain.Bunk.SpecialBunk(bunkCode, discount, fare, reader.GetDecimal(startIndex + 24), ei);
                    break;
                case Order.Domain.Bunk.BunkType.Free:
                    bunk = new Order.Domain.Bunk.FreeBunk(bunkCode, reader.GetDecimal(startIndex + 24), ei, description);
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
            flight.Bunk = bunk;
            if(!reader.IsDBNull(25)) {
                flight.AssociateFlight = reader.GetGuid(startIndex + 25);
            }
            flight.ReservateFlight = reader.GetGuid(startIndex + 26);
            flight.IsShare = reader.GetBoolean(startIndex + 27);
            return flight;
        }
        void fillFlightInTicket(Domain.Applyform.BaseApplyform applyform) {
            foreach(var passenger in applyform.Passengers) {
                foreach(var ticket in passenger.Tickets) {
                    ticket.Load(applyform.OriginalFlights);
                }
            }
        }

        public IEnumerable<Order.Domain.Applyform.BaseApplyform> QueryApplyforms(decimal orderId) {
            var applyforms = loadApplyformsForMainInfo(orderId);
            foreach(var item in applyforms) {
                loadPassengersOfApplyform(item);
                loadFlightsOfApplyform(item);
                fillFlightInTicket(item);
            }
            return applyforms;
        }
        IEnumerable<Order.Domain.Applyform.BaseApplyform> loadApplyformsForMainInfo(decimal orderId) {
            ClearParameters();
            var result = new List<Order.Domain.Applyform.BaseApplyform>();
            var sql = "SELECT [OrderId],[Id],[Type],[ProductType],[BPNR],[PNR],[NewBPNR],[NewPNR],[Provider],[ProviderName],[Purchaser],[PurchaserName],[ApplyRemark],[ApplierAccount]," +
                "[AppliedTime],[ProcessedFailedReason],[ProcessedTime],[IsInterior],[Status],[Amount],[RefundType],RequireRevisePrice,ApplierAccountName,OEMID,HasBalanceRefund,AssociateApplyform FROM [dbo].[T_Applyform] WHERE [OrderId]=@ORDERID";
            AddParameter("ORDERID", orderId);
            using(var reader = ExecuteReader(sql)) {
                while(reader.Read()) {
                    Order.Domain.Applyform.BaseApplyform applyform = constructApplyformMainInfo(reader);
                    if(applyform != null) {
                        result.Add(applyform);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 当天航班优先排序
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<ApplyformListView> QueryApplyformsNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            IEnumerable<ApplyformListView> result = null;
            ClearParameters();
            if (condition.ApplyformId.HasValue)
            {
                AddParameter("@iApplyformId", condition.ApplyformId.Value);
            }
            else
            {
                AddParameter("@iApplyformId", DBNull.Value);
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
            if (condition.ApplyformType.HasValue)
            {
                AddParameter("@iApplyformType", (byte)condition.ApplyformType.Value);
            }
            else
            {
                AddParameter("@iApplyformType", DBNull.Value);
            }
            if (condition.RefundStatuses.HasValue)
            {
                AddParameter("@iRefundStatus", (byte)condition.RefundStatuses.Value);
            }
            else
            {
                AddParameter("@iRefundStatus", DBNull.Value);
            }
            if (condition.PostponeStatuses.HasValue)
            {
                AddParameter("@iPostponeStatus", (byte)condition.PostponeStatuses.Value);
            }
            else
            {
                AddParameter("@iPostponeStatus", DBNull.Value);
            }
            if (condition.ProcessStatus.HasValue)
            {
                AddParameter("@iProcessStatus", (byte)condition.ProcessStatus.Value);
            }
            else
            {
                AddParameter("@iProcessStatus", DBNull.Value);
            }
            if (condition.ProductType.HasValue)
            {
                AddParameter("@iProductType", condition.ProductType.Value);
            }
            else
            {
                AddParameter("@iProductType", DBNull.Value);
            }
            if (string.IsNullOrWhiteSpace(condition.PNR))
            {
                AddParameter("@iPNR", DBNull.Value);
            }
            else
            {
                AddParameter("@iPNR", condition.PNR.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.Passenger))
            {
                AddParameter("@iPassenger", DBNull.Value);
            }
            else
            {
                AddParameter("@iPassenger", condition.Passenger.Trim());
            }
            if (string.IsNullOrWhiteSpace(condition.Applier))
            {
                AddParameter("@iApplier", DBNull.Value);
            }
            else
            {
                AddParameter("@iApplier", condition.Applier.Trim());
            }
            if (condition.RequireRevisePrice.HasValue)
            {
                AddParameter("@iRequireRevisePrice", condition.RequireRevisePrice.Value);
            }
            else
            {
                AddParameter("@iRequireRevisePrice", DBNull.Value);
            }
            if (condition.ApplyDetailStatus.HasValue)
            {
                AddParameter("@iStatus", condition.ApplyDetailStatus);
            }
            if (condition.RefundType.HasValue)
            {
                AddParameter("@iRefundType", (byte)condition.RefundType);
            }
            if (condition.OEMID.HasValue)
            {
                AddParameter("@iOEMID", condition.OEMID.Value);
            }
            AddParameter("@iIsStatusAndRequireRevisePrice", condition.IsStatusAndRequireRevisePrice);
            AddParameter("@iAppliedStartTime", condition.AppliedDateRange.Lower.Date);
            AddParameter("@iAppliedEndTime", condition.AppliedDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryApplyformsNew", System.Data.CommandType.StoredProcedure))
            {
                result = reconstructApplyformListViews(reader);
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        public IEnumerable<ApplyformListView> QueryApplyforms(ApplyformQueryCondition condition, Pagination pagination) {
            IEnumerable<ApplyformListView> result = null;
            ClearParameters();
            if(condition.ApplyformId.HasValue) {
                AddParameter("@iApplyformId", condition.ApplyformId.Value);
            } else {
                AddParameter("@iApplyformId", DBNull.Value);
            }
            if(condition.Purchaser.HasValue) {
                AddParameter("@iPurchaser", condition.Purchaser.Value);
            } else {
                AddParameter("@iPurchaser", DBNull.Value);
            }
            if(condition.Provider.HasValue) {
                AddParameter("@iProvider", condition.Provider.Value);
            } else {
                AddParameter("@iProvider", DBNull.Value);
            }
            if(condition.Supplier.HasValue) {
                AddParameter("@iSupplier", condition.Supplier.Value);
            } else {
                AddParameter("@iSupplier", DBNull.Value);
            }
            if(condition.ApplyformType.HasValue) {
                AddParameter("@iApplyformType", (byte)condition.ApplyformType.Value);
            } else {
                AddParameter("@iApplyformType", DBNull.Value);
            }
            if(condition.RefundStatuses.HasValue) {
                AddParameter("@iRefundStatus", (byte)condition.RefundStatuses.Value);
            } else {
                AddParameter("@iRefundStatus", DBNull.Value);
            }
            if(condition.PostponeStatuses.HasValue) {
                AddParameter("@iPostponeStatus", (byte)condition.PostponeStatuses.Value);
            } else {
                AddParameter("@iPostponeStatus", DBNull.Value);
            }
            if(condition.ProcessStatus.HasValue) {
                AddParameter("@iProcessStatus", (byte)condition.ProcessStatus.Value);
            } else {
                AddParameter("@iProcessStatus", DBNull.Value);
            }
            if(condition.ProductType.HasValue) {
                AddParameter("@iProductType", condition.ProductType.Value);
            } else {
                AddParameter("@iProductType", DBNull.Value);
            }
            if(string.IsNullOrWhiteSpace(condition.PNR)) {
                AddParameter("@iPNR", DBNull.Value);
            } else {
                AddParameter("@iPNR", condition.PNR.Trim());
            }
            if(string.IsNullOrWhiteSpace(condition.Passenger)) {
                AddParameter("@iPassenger", DBNull.Value);
            } else {
                AddParameter("@iPassenger", condition.Passenger.Trim());
            }
            if(string.IsNullOrWhiteSpace(condition.Applier)) {
                AddParameter("@iApplier", DBNull.Value);
            } else {
                AddParameter("@iApplier", condition.Applier.Trim());
            }
            if(condition.RequireRevisePrice.HasValue) {
                AddParameter("@iRequireRevisePrice", condition.RequireRevisePrice.Value);
            } else {
                AddParameter("@iRequireRevisePrice", DBNull.Value);
            }
            if(condition.ApplyDetailStatus.HasValue) {
                AddParameter("@iStatus", condition.ApplyDetailStatus);
            }
            if (condition.RefundType.HasValue)
            {
                AddParameter("@iRefundType", (byte)condition.RefundType);
            }
            if (condition.OEMID.HasValue)
            {
                AddParameter("@iOEMID", condition.OEMID.Value);
            }
            if (condition.BalanceRefundProcessStatus.HasValue)
            {
                AddParameter("iBalanceRefundProcessStatus", (byte)condition.BalanceRefundProcessStatus.Value);
            }
            else
            {
                AddParameter("iBalanceRefundProcessStatus", DBNull.Value);
            }
            AddParameter("@iIncludeBalanceRefund", condition.IncludeBlanceApplyform);
            AddParameter("@iIsStatusAndRequireRevisePrice", condition.IsStatusAndRequireRevisePrice);
            AddParameter("@iAppliedStartTime", condition.AppliedDateRange.Lower.Date);
            AddParameter("@iAppliedEndTime", condition.AppliedDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using(var reader = ExecuteReader("dbo.P_QueryApplyforms", System.Data.CommandType.StoredProcedure)) {
                result = reconstructApplyformListViews(reader);
            }
            if(pagination.GetRowCount) {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        IEnumerable<ApplyformListView> reconstructApplyformListViews(DbDataReader reader) {
            var result = new List<ApplyformListView>();
            ApplyformListView item = null;
            List<Guid> flights = null;
            while(reader.Read()) {
                decimal currentApplyformId = reader.GetDecimal(0);
                if(item == null || item.ApplyformId != currentApplyformId) {
                    item = new ApplyformListView() {
                        ApplyformId = currentApplyformId,
                        ApplyformType = (ApplyformType)reader.GetByte(1),
                        ProductType = (DataTransferObject.Order.ProductType)reader.GetByte(2),
                        ApplyDetailStatus = reader.GetByte(3),
                        Purchaser = reader.GetGuid(4),
                        PurchaserName = reader.GetString(5),
                        Provider = reader.GetGuid(6),
                        ProviderName = reader.GetString(7),
                        AppliedTime = reader.GetDateTime(10),
                        ApplierAccount = reader.GetString(11),
                        RequireRevisePrice = reader.GetBoolean(31),
                        ApplierAccountName = reader.GetString(33),
                        Passengers = new List<string>(),
                        Flights = new List<FlightListView>(),
                    };
                    if(!reader.IsDBNull(8)) {
                        item.Supplier = reader.GetGuid(8);
                        item.SupplierName = reader.GetString(9);
                    }
                    string bpnr = reader.IsDBNull(12) ? null : reader.GetString(12);
                    string pnr = reader.IsDBNull(13) ? null : reader.GetString(13);
                    string newBPNR = reader.IsDBNull(14) ? null : reader.GetString(14);
                    string newPNR = reader.IsDBNull(15) ? null : reader.GetString(15);
                    if(!string.IsNullOrWhiteSpace(bpnr) || !string.IsNullOrWhiteSpace(pnr)) {
                        item.OriginalPNR = new PNRPair(pnr, bpnr);
                    }
                    if(!string.IsNullOrWhiteSpace(newBPNR) || !string.IsNullOrWhiteSpace(newPNR)) {
                        item.NewPNR = new PNRPair(newPNR, newBPNR);
                    }
                    flights = new List<Guid>();
                    result.Add(item);
                }
                string passengerName = reader.GetString(16);
                if(!item.Passengers.Contains(passengerName)) {
                    item.Passengers.Add(passengerName);
                }
                Guid flightId = reader.GetGuid(17);
                if(!flights.Contains(flightId)) {
                    var flightView = new FlightListView() {
                        DepartureAirport = reader.GetString(18),
                        DepartureCity = reader.GetString(19),
                        ArrivalAirport = reader.GetString(20),
                        ArrivalCity = reader.GetString(21),
                        Carrier = reader.GetString(22),
                        FlightNo = reader.GetString(23),
                        Bunk = reader.GetString(24),
                        Discount = reader.GetDecimal(25),
                        Fare = reader.GetDecimal(27),
                        AirportFee = reader.GetDecimal(28),
                        BAF = reader.GetDecimal(29),
                        TakeoffTime = reader.GetDateTime(30)
                    };
                    flightView.ReleasedFare = reader.IsDBNull(26) ? flightView.Fare : reader.GetDecimal(26);
                    item.Flights.Add(flightView);
                    flights.Add(flightId);
                }
                if (!reader.IsDBNull(32)){
                    item.RefundType = (RefundType)reader.GetByte(32);
                }
                if (!reader.IsDBNull(34)) item.OEMID = reader.GetGuid(34);
            }
            return result;
        }

        public List<ApplyAttachmentView> QueryApplyAttachmentView(decimal applyformId)
        {
            List<ApplyAttachmentView> result = new List<ApplyAttachmentView>();
            string sql = "SELECT [Id],[ApplyformId],[FilePath],[Thumbnail],[Time] FROM [T_ApplyAttachment] WHERE [ApplyformId] = @ApplyformId";
            AddParameter("ApplyformId", applyformId);
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    ApplyAttachmentView view = new ApplyAttachmentView();
                    view.Id = reader.GetGuid(0);
                    view.ApplyformId = reader.GetDecimal(1);
                    view.FilePath = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    view.Thumbnail = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3);
                    view.Time = reader.GetDateTime(4);
                    result.Add(view);
                }
            }
            return result;
        }

        public ApplyAttachmentView QueryApplyAttachmentView(Guid applyAttachmentId)
        {
            ApplyAttachmentView result = null;
            string sql = "SELECT [Id],[ApplyformId],[FilePath],[Thumbnail],[Time] FROM [T_ApplyAttachment] WHERE [Id] = @Id";
            AddParameter("Id",applyAttachmentId);
            using (System.Data.Common.DbDataReader reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new ApplyAttachmentView();
                    result.Id = reader.GetGuid(0);
                    result.ApplyformId = reader.GetDecimal(1);
                    result.FilePath = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    result.Thumbnail = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3);
                    result.Time = reader.GetDateTime(4);
                }
            }
            return result;
        }

        public void DeleteApplyAttachmentView(Guid applyAttachmentId)
        {
            string sql = "DELETE FROM [T_ApplyAttachment] WHERE [Id] = @Id";
            AddParameter("Id", applyAttachmentId);
            ExecuteNonQuery(sql);
        }

        public void AddApplyAttachmentView(List<ApplyAttachmentView> applyAttachmentViews)
        {
            insertApplyAttachment(applyAttachmentViews, null);
        }


        #region  差额退款
//        public void InsertBalanceRefundApplyform(BalanceRefundApplyform applyform) {
//            var applyformInsertSql = "INSERT INTO [T_BalanceRefundApplyform] ([Id] ,[OrderId] ,[AssociateApplyformId] ,[RefundRemark] ,[BalanceRefundStatus] ,[ApplyTime] ,[ProcessTime] ,[ProcessorAccount] ,[Processor] ,[ApplyAccount] ,[Applyer] ,[PurchaserId] ,[ProviderId] ,[ProviderName] ,[PurchaserName])      VALUES( @Id ,@OrderId, @AssociateApplyformId, @RefundRemark, @BalanceRefundStatus, @ApplyTime, @ProcessTime ,@ProcessorAccount, @Processor, @ApplyAccount, @Applyer, @PurchaserId, @ProviderId ,@ProviderName ,@PurchaserName) ";
//            AddParameter("Id", applyform.Id);
//            AddParameter("OrderId", applyform.OrderId);
//            AddParameter("AssociateApplyformId", applyform.AssociateApplyformId);
//            AddParameter("RefundRemark", applyform.RefundRemark);
//            AddParameter("BalanceRefundStatus", (byte)applyform.BalanceRefundStatus);
//            AddParameter("ApplyTime", applyform.ApplyTime);
//            AddParameter("ProcessTime", DBNull.Value);
//            AddParameter("ProcessorAccount", DBNull.Value);
//            AddParameter("Processor", DBNull.Value);
//            AddParameter("ApplyAccount", applyform.ApplyAccount);
//            AddParameter("Applyer", applyform.Applyer);
//            AddParameter("PurchaserName", applyform.PurchaserName);
//            AddParameter("PurchaserId", applyform.PurchaserId);
//            AddParameter("ProviderId", applyform.ProviderId);
//            AddParameter("ProviderName", applyform.ProviderName);
//            ExecuteNonQuery(applyformInsertSql);
//        }

//        public void UpdateFlag(RefundOrScrapApplyform applyform) {
//            var sql = "Update T_Applyform set HasBalanceRefund = 1 where id=@applyformId";
//            AddParameter("applyformId", applyform.Id);
//            ExecuteNonQuery(sql);
//        }

//        public BalanceRefundApplyform QueryBalanceRefundApplyform(decimal applyformId) {
//            var loadSql = @"SELECT   Id, OrderId, AssociateApplyformId, RefundRemark, BalanceRefundStatus, ApplyTime, ProcessTime, ProcessorAccount, Processor, ApplyAccount, Applyer, PurchaserId, ProviderId, ProviderName, PurchaserName
//        FROM T_BalanceRefundApplyform WHERE Id = @Id";
//            AddParameter("Id", applyformId);
//            BalanceRefundApplyform result = null;
//            using (var reader = ExecuteReader(loadSql))
//            {
//                while (reader.Read())
//                {
//                    result = constructBalanceRefundApplyformMainInfo(reader);
//                 }
//            }
//            return result;
//        }

//        public void UpdateFlightBalanceRefunfFee(decimal applyformId,IEnumerable<RefundFlight> flights) {
//            var sql = new StringBuilder();
//            int index = 0;
//            foreach (RefundFlight refundFlight in flights)
//            {
//                sql.AppendFormat("update T_ApplyFlight set BanlanceFare = @Fee{0} where ApplyformId = @ApplyformId{0} and OriginalFlight = flightId{0}",index);
//                AddParameter("Fee" + index, refundFlight.BanlanceFare.Value);
//                AddParameter("ApplyformId" + index, applyformId);
//                AddParameter("OriginalFlight" + index, refundFlight.OriginalFlight.Id);
//                index++;
//            }
//            ExecuteNonQuery(sql.ToString());
//        }

//        public void SaveBalanceRefundProcessStatus(BalanceRefundApplyform applyform) {
//            var sql = "update T_BalanceRefundApplyform set BalanceRefundStatus = @status , " +
//                      "ProcessTime =@ProcessTime,Processor=@Processor,ProcessorAccount=@ProcessorAccount " +
//                      " where id =@Id";
//            AddParameter("status", (byte)applyform.BalanceRefundStatus);
//            AddParameter("Id", applyform.Id);
//            AddParameter("ProcessTime", applyform.ProcessTime);
//            AddParameter("Processor", applyform.Processor);
//            AddParameter("ProcessorAccount", applyform.ProcessorAccount);
//            ExecuteNonQuery(sql);
//        }

//        private BalanceRefundApplyform constructBalanceRefundApplyformMainInfo(DbDataReader reader) {
//            var result = new BalanceRefundApplyform(reader.GetDecimal(1), reader.GetDecimal(0));
//            result.AssociateApplyformId = reader.GetDecimal(2);
//            result.RefundRemark = reader.GetString(3);
//            result.BalanceRefundStatus =  (BalanceRefundProcessStatus) reader.GetByte(4);
//            result.ApplyTime = reader.GetDateTime(5);
//            if (!reader.IsDBNull(6)) result.ProcessTime = reader.GetDateTime(6);
//            result.ProcessorAccount = reader.IsDBNull(7)?string.Empty: reader.GetString(7);
//            result.Processor =reader.IsDBNull(8)?string.Empty: reader.GetString(8);
//            result.ApplyAccount = reader.GetString(9);
//            result.Applyer = reader.GetString(10);
//            result.PurchaserName = reader.GetString(14);
//            result.PurchaserId = reader.GetGuid(11);
//            result.ProviderId = reader.GetGuid(12);
//            result.ProviderName = reader.GetString(13);
//            return result;
//        }

        public void InsertBalanceRefundApplyform(BalanceRefundApplyform balanceRefundApplyform)
        {
            insertApplyformMainInfo(balanceRefundApplyform);
        }

        public void UpdateFlag(decimal applyformId)
        {
            ClearParameters();
            var sql = "Update T_Applyform set HasBalanceRefund = 1 where id=@applyformId";
            AddParameter("applyformId", applyformId);
            ExecuteNonQuery(sql);
        }

        public BalanceRefundApplyform QueryBalanceRefundApplyform(decimal applyformId)
        {
            return loadApplyformMainInfo(applyformId) as BalanceRefundApplyform;
        }

        public void UpdateFlightBalanceRefunfFee(decimal applyformId, IEnumerable<RefundFlight> flights)
        {
            var sql = new StringBuilder();
            int index = 0;
            foreach (RefundFlight refundFlight in flights)
            {
                sql.AppendFormat("update T_ApplyFlight set BanlanceFare = @Fee{0} where ApplyformId = @ApplyformId{0} and OriginalFlight = @OriginalFlight{0};", index);
                AddParameter("Fee" + index, refundFlight.BanlanceFare.Value);
                AddParameter("ApplyformId" + index, applyformId);
                AddParameter("OriginalFlight" + index, refundFlight.OriginalFlight.Id);
                index++;
            }
            ExecuteNonQuery(sql.ToString());
        }

        public void SaveBalanceRefundProcessStatus(BalanceRefundApplyform applyform)
        {
            var sql = @"update T_Applyform 
	                    set [Status] = @Status,ProcessedTime = @ProcessedTime,ProcessedFailedReason=@ProcessedFailedReason,
	                    OperatorName=@OperatorName,OperatorAccount = @OperatorAccount
                        where Id = @Id";
            AddParameter("Status", (byte)applyform.BalanceRefundStatus);
            AddParameter("Id", applyform.Id);
            if (applyform.ProcessedTime.HasValue)
            {
                AddParameter("ProcessedTime", applyform.ProcessedTime);
            }
            else
            {
                AddParameter("ProcessedTime", DBNull.Value);
            }
            AddParameter("OperatorName", applyform.Operator??string.Empty);
            AddParameter("OperatorAccount", applyform.OperatorAccount??string.Empty);
            AddParameter("ProcessedFailedReason", applyform.ProcessedFailedReason);
            ExecuteNonQuery(sql);
        }




        #endregion
    }
}