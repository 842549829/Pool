using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Distribution.Domain;
using ChinaPay.B3B.Service.Distribution.Domain.Bill;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone;
using ChinaPay.B3B.Service.Distribution.Domain.Role;
using ChinaPay.B3B.Service.Distribution.Domain.Tradement;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Distribution.Repository.SqlServer {
    class DistributionRepository : SqlServerTransaction, IDistributionRepository {
        private const int _payDefailBillMaxCount = 150;
        private const int _refundDefailBillMaxCount = 150;

        public DistributionRepository(DbOperator dbOperator)
            : base(dbOperator) {
        }

        public void SaveNormalPayBill(Domain.Bill.Pay.NormalPayBill payBill) {
            insertPayment(payBill);
            insertNormalPayRoleBill(payBill.Tradement.Id, payBill.Purchaser);
            insertRoyaltiesPayBill(payBill);
        }

        public void ReSaveNormalPayBill(Domain.Bill.Pay.NormalPayBill payBill) {
            deleteNormalPayBill(payBill.OrderId);
            SaveNormalPayBill(payBill);
        }

        public void SaveRoyaltiesPayBill(Domain.Bill.Pay.NormalPayBill payBill) {
            deleteNormalRoyaltiesPayRoleBill(payBill.Tradement.Id);
            insertRoyaltiesPayBill(payBill);
        }

        public void SavePostponePayBill(Domain.Bill.Pay.PostponePayBill payBill) {
            insertPayment(payBill);
            insertPostponePayRoleBill(payBill.Tradement.Id, payBill.Applier);
            insertPostponePayRoleBill(payBill.Tradement.Id, payBill.Accepter.Deduction);
            insertTradementProfit(payBill.Tradement.Id, TradementType.Pay, payBill.Accepter.Premium, payBill.Accepter.TradeFee, payBill.Accepter.Account, payBill.Accepter.Success);
        }

        public void UpdatePayBillForPurchaserPaySuccess(Domain.Bill.Pay.NormalPayBill payBill) {
            ClearParameters();
            if(payBill.Purchaser.Success) {
                var sql = prepareUpdatePaymentForPaySuccessSql(payBill.Tradement, 0) +
                          prepareUpdatePayRoleBillForPaySuccessSql(payBill.Purchaser.Id, payBill.Purchaser.Owner.Account, payBill.Purchaser.Time.Value, 1);
                ExecuteNonQuery(sql);
            }
        }

        public void UpdatePayBillPriceInfo(Domain.Bill.Pay.NormalPayBill payBill) {
            updateNormalPayRoleBillPriceInfo(payBill.Purchaser);
            updateNormalPayRoleBillPriceInfo(payBill.Provider);
            updateNormalPayRoleBillPriceInfo(payBill.Supplier);
            foreach(var royalty in payBill.Royalties) {
                updateNormalPayRoleBillPriceInfo(royalty);
            }
            if(payBill.Platform != null) {
                updateNormalPayRoleBillPriceInfo(payBill.Platform.Deduction);
            }
        }

        public void UpdatePayBillFare(Domain.Bill.Pay.NormalPayBill payBill) {
            updateFareOfNormalPayRoleBill(payBill.Purchaser);
            updateFareOfNormalPayRoleBill(payBill.Provider);
            updateFareOfNormalPayRoleBill(payBill.Supplier);
            foreach(var royalty in payBill.Royalties) {
                updateFareOfNormalPayRoleBill(royalty);
            }
            if(payBill.Platform != null) {
                updateFareOfNormalPayRoleBill(payBill.Platform.Deduction);
            }
        }

        public void UpdatePayBillForPurchaserPayPostponeFeeSuccess(Domain.Bill.Pay.PostponePayBill payBill) {
            ClearParameters();
            if(payBill.Applier.Success) {
                var serial = 0;
                var sql = prepareUpdatePaymentForPaySuccessSql(payBill.Tradement, serial++) +
                          prepareUpdatePayRoleBillForPaySuccessSql(payBill.Applier.Id, payBill.Applier.Owner.Account, payBill.Applier.Time.Value, serial++);
                if(payBill.Accepter.Deduction != null && payBill.Accepter.Deduction.Success) {
                    sql += prepareUpdatePayRoleBillForTradeSuccessSql(payBill.Accepter.Deduction.Id, payBill.Accepter.Deduction.Time.Value, serial++);
                }
                if(payBill.Accepter.Success) {
                    sql += prepareUpdateTradementProfitForStatusSql(payBill.Tradement.Id, TradementType.Pay, serial++);
                }
                ExecuteNonQuery(sql);
            }
        }

        public void UpdatePayBillForRoyaltiesTradeSuccess(Domain.Bill.Pay.NormalPayBill payBill) {
            ClearParameters();
            var sql = string.Empty;
            var serial = 0;
            if(payBill.Provider != null && payBill.Provider.Success) {
                sql += prepareUpdatePayRoleBillForTradeSuccessSql(payBill.Provider.Id, payBill.Provider.Time.Value, serial++);
            }
            if(payBill.Supplier != null && payBill.Supplier.Success) {
                sql += prepareUpdatePayRoleBillForTradeSuccessSql(payBill.Supplier.Id, payBill.Supplier.Time.Value, serial++);
            }
            foreach(var royalty in payBill.Royalties) {
                if(royalty.Success) {
                    sql += prepareUpdatePayRoleBillForTradeSuccessSql(royalty.Id, royalty.Time.Value, serial++);
                }
            }
            if(payBill.Platform != null) {
                if(payBill.Platform.Deduction != null && payBill.Platform.Deduction.Success) {
                    sql += prepareUpdatePayRoleBillForTradeSuccessSql(payBill.Platform.Deduction.Id, payBill.Platform.Deduction.Time.Value, serial++);
                }
                if(payBill.Platform.Success) {
                    sql += prepareUpdateTradementProfitForStatusSql(payBill.Tradement.Id, TradementType.Pay, serial++);
                }
            }
            if(sql.Length > 0) {
                ExecuteNonQuery(sql);
            }
        }

        public void SaveRefundBill(Domain.Bill.Refund.NormalRefundBill refundBill) {
            DeleteRefundBill(refundBill);
            insertRefundment(refundBill);
            insertNormalRefundRoleBill(refundBill.Tradement.Id, refundBill.Purchaser);
            insertNormalRefundRoleBill(refundBill.Tradement.Id, refundBill.Provider);
            insertNormalRefundRoleBill(refundBill.Tradement.Id, refundBill.Supplier);
            foreach(var royalty in refundBill.Royalties) {
                insertNormalRefundRoleBill(refundBill.Tradement.Id, royalty);
            }
            insertNormalRefundRoleBill(refundBill.Tradement.Id, refundBill.Platform.Deduction);
            insertTradementProfit(refundBill.Tradement.Id, TradementType.Refund, refundBill.Platform.Premium, refundBill.Platform.TradeFee, refundBill.Platform.Account, refundBill.Platform.Success);
        }

        public void SaveRefundBill(Domain.Bill.Refund.PostponeRefundBill refundBill) {
            insertRefundment(refundBill);
            insertPostponeRefundRoleBill(refundBill.Tradement.Id, refundBill.Applier);
            insertPostponeRefundRoleBill(refundBill.Tradement.Id, refundBill.Accepter.Deduction);
            insertTradementProfit(refundBill.Tradement.Id, TradementType.Refund, refundBill.Accepter.Premium, refundBill.Accepter.TradeFee, refundBill.Accepter.Account, refundBill.Accepter.Success);
        }

        public void UpdateRefundBillForRefundSuccess(Domain.Bill.Refund.NormalRefundBill refundBill) {
            ClearParameters();
            var sql = string.Empty;
            var serial = 0;
            if(refundBill.Purchaser.Success) {
                sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Purchaser.Id, refundBill.Purchaser.Time.Value, serial++);
            }
            if(refundBill.Provider != null && refundBill.Provider.Success) {
                sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Provider.Id, refundBill.Provider.Time.Value, serial++);
            }
            if(refundBill.Supplier != null && refundBill.Supplier.Success) {
                sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Supplier.Id, refundBill.Supplier.Time.Value, serial++);
            }
            foreach(var royalty in refundBill.Royalties) {
                if(royalty.Success) {
                    sql += prepareUpdateRefundRoleBillForTradeSuccessSql(royalty.Id, royalty.Time.Value, serial++);
                }
            }
            if(refundBill.Platform != null) {
                if(refundBill.Platform.Deduction != null && refundBill.Platform.Deduction.Success) {
                    sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Platform.Deduction.Id, refundBill.Platform.Deduction.Time.Value, serial++);
                }
                if(refundBill.Platform.Success) {
                    sql += prepareUpdateTradementProfitForStatusSql(refundBill.Tradement.Id, TradementType.Refund, serial++);
                }
            }
            if(!string.IsNullOrWhiteSpace(sql)) {
                ExecuteNonQuery(sql);
            }
        }

        public void UpdateRefundBillForRefundSuccess(Domain.Bill.Refund.PostponeRefundBill refundBill) {
            ClearParameters();
            var serial = 0;
            var sql = string.Empty;
            if(refundBill.Applier.Success) {
                sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Applier.Id, refundBill.Applier.Time.Value, serial++);
            }
            if(refundBill.Accepter.Deduction != null && refundBill.Accepter.Deduction.Success) {
                sql += prepareUpdateRefundRoleBillForTradeSuccessSql(refundBill.Accepter.Deduction.Id, refundBill.Accepter.Deduction.Time.Value, serial++);
            }
            if(refundBill.Accepter.Success) {
                sql += prepareUpdateTradementProfitForStatusSql(refundBill.Tradement.Id, TradementType.Refund, serial++);
            }
            if(!string.IsNullOrWhiteSpace(sql)) {
                ExecuteNonQuery(sql);
            }
        }

        public void DeleteRefundBill(Domain.Bill.Refund.NormalRefundBill refundBill) {
            ClearParameters();
            if(refundBill != null) {
                var refundmentId = getRefundmentId(refundBill.ApplyformId);
                if(refundmentId.HasValue) {
                    deleteRefundBill(refundmentId.Value);
                }
            }
        }

        public Domain.Bill.Pay.NormalPayBill QueryNormalPayBill(decimal orderId) {
            ClearParameters();
            Domain.Bill.Pay.NormalPayBill result = null;
            var paymentSql = "SELECT Id,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,PayTradeNo,IsPoolpay,PayInterface,PayAccountType,ChannelTradeNo,Remark FROM dbo.T_Payment WHERE OrderId=@OrderId AND [Type]=@PaymentType";
            var roleBillSql = "SELECT Id,[Owner],[Role],TradeRate,TROLE.Account,ReleasedFare,Fare,AirportFee,BAF,Rebate,ServiceCharge,Commission,TROLE.Increasing,"
                + "TROLE.Anticipation,TROLE.TradeFee,Amount,TROLE.Success,TradeTime,TPROFIT.Premium,TPROFIT.TradeFee,TPROFIT.Account,TPROFIT.Success"
                + " FROM dbo.T_PayBill TROLE"
                + " LEFT JOIN dbo.T_TradementProfit TPROFIT ON TPROFIT.TradementType=@TradementType AND TPROFIT.TradementId=TROLE.PaymentId"
                + " WHERE TROLE.PaymentId=@PaymentId";
            AddParameter("OrderId", orderId);
            AddParameter("PaymentType", (byte)PaymentType.Normal);
            Guid? paymentId = null;
            using(var reader = ExecuteReader(paymentSql)) {
                if(reader.Read()) {
                    paymentId = reader.GetGuid(0);
                    result = new Domain.Bill.Pay.NormalPayBill(orderId) {
                        Tradement = new Payment(paymentId.Value) {
                            Amount = reader.GetDecimal(1),
                            TradeRate = reader.GetDecimal(2),
                            TradeFee = reader.GetDecimal(3),
                            PayeeAccount = reader.GetString(4),
                            PayAccount = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            TradeNo = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            IsPoolpay = !reader.IsDBNull(7) && reader.GetBoolean(7),
                            ChannelTradeNo = reader.IsDBNull(10) ? string.Empty : reader.GetString(10)
                        },
                        Remark = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                    };
                    if(!reader.IsDBNull(8)) {
                        result.Tradement.PayInterface = (PayInterface)reader.GetByte(8);
                    }
                    if(!reader.IsDBNull(9)) {
                        result.Tradement.PayAccountType = (PayAccountType)reader.GetByte(9);
                    }
                }
            }
            if(paymentId.HasValue) {
                ClearParameters();
                AddParameter("PaymentId", paymentId.Value);
                AddParameter("TradementType", (byte)TradementType.Pay);
                using(var reader = ExecuteReader(roleBillSql)) {
                    while(reader.Read()) {
                        if(result.Platform == null) {
                            result.Platform = new PlatformBill<NormalPayRoleBill, NormalPayRoleBillSource, NormalPayDetailBill> {
                                Premium = reader.IsDBNull(18) ? 0 : reader.GetDecimal(18),
                                TradeFee = reader.IsDBNull(19) ? 0 : reader.GetDecimal(19),
                                Success = !reader.IsDBNull(21) && reader.GetBoolean(21)
                            };
                            if(!reader.IsDBNull(20)) {
                                result.Platform.Account = reader.GetString(20);
                            }
                        }
                        var roleBill = constructNormalPayRoleBill(reader);
                        if(roleBill.Owner is Purchaser) {
                            result.Purchaser = roleBill;
                        } else if(roleBill.Owner is Provider) {
                            result.Provider = roleBill;
                        } else if(roleBill.Owner is Supplier) {
                            result.Supplier = roleBill;
                        } else if(roleBill.Owner is PlatformRoyalty) {
                            result.Platform.Deduction = roleBill;
                        } else if(roleBill.Owner is Royalty) {
                            result.AddRoyalty(roleBill);
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<Domain.Bill.Pay.PostponePayBill> QueryPostponePayBills(decimal orderId) {
            ClearParameters();
            var result = new List<Domain.Bill.Pay.PostponePayBill>();
            var sql = new StringBuilder();
            sql.Append("SELECT TROLE.Id,TROLE.[Owner],TROLE.[Role],TROLE.TradeRate,TROLE.Account,TROLE.PostponeFee,");
            sql.Append("TROLE.Anticipation,TROLE.TradeFee,TROLE.Amount,TROLE.Success,TROLE.TradeTime,");
            sql.Append("TPAY.ApplyformId,TPAY.Amount,TPAY.TradeRate,TPAY.TradeFee,TPAY.PayeeAccount,TPAY.PayAccount,TPAY.PayTradeNo,");
            sql.Append("TPAY.IsPoolpay,TPAY.PayInterface,TPAY.PayAccountType,TPAY.Id,TPAY.ChannelTradeNo,TPAY.Remark,");
            sql.Append("tProfit.Premium,tProfit.TradeFee,tProfit.Account,tProfit.Success");
            sql.Append(" FROM dbo.T_Payment TPAY");
            sql.Append(" INNER JOIN dbo.T_PayBill TROLE ON TPAY.Id=TROLE.PaymentId");
            sql.Append(" LEFT JOIN dbo.T_TradementProfit tProfit ON tProfit.TradementType=@TradementType AND tProfit.TradementId=TROLE.PaymentId");
            sql.Append(" WHERE TPAY.OrderId=@OrderId AND TPAY.[Type]=@PaymentType");
            sql.Append(" ORDER BY TPAY.ApplyformId");
            AddParameter("PaymentType", (byte)PaymentType.Postpone);
            AddParameter("OrderId", orderId);
            AddParameter("TradementType", (byte)TradementType.Pay);
            using(var reader = ExecuteReader(sql.ToString())) {
                Domain.Bill.Pay.PostponePayBill payBill = null;
                while(reader.Read()) {
                    var currentApplyformId = reader.GetDecimal(11);
                    if(payBill == null || payBill.ApplyformId != currentApplyformId) {
                        payBill = new Domain.Bill.Pay.PostponePayBill(orderId, currentApplyformId) {
                            Tradement = new Payment(reader.GetGuid(21)) {
                                Amount = reader.GetDecimal(12),
                                TradeRate = reader.GetDecimal(13),
                                TradeFee = reader.GetDecimal(14),
                                PayeeAccount = reader.GetString(15),
                                PayAccount = reader.GetString(16),
                                TradeNo = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                ChannelTradeNo = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                                IsPoolpay = !reader.IsDBNull(18) && reader.GetBoolean(18)
                            },
                            Remark = reader.IsDBNull(23) ? string.Empty : reader.GetString(23)
                        };
                        if(!reader.IsDBNull(19)) {
                            payBill.Tradement.PayInterface = (PayInterface)reader.GetByte(19);
                        }
                        if(!reader.IsDBNull(20)) {
                            payBill.Tradement.PayAccountType = (PayAccountType)reader.GetByte(20);
                        }
                        result.Add(payBill);
                    }
                    var roleBill = constructPostponePayRoleBill(reader);
                    if(roleBill.Owner is Purchaser) {
                        payBill.Applier = roleBill;
                    } else if(roleBill.Owner is PlatformRoyalty) {
                        payBill.Accepter = new PlatformBill<PostponePayRoleBill, PostponePayRoleBillSource, PostponePayDetailBill> {
                            Deduction = roleBill,
                            Premium = reader.IsDBNull(24) ? 0 : reader.GetDecimal(24),
                            TradeFee = reader.IsDBNull(25) ? 0 : reader.GetDecimal(25),
                            Account = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                            Success = !reader.IsDBNull(27) && reader.GetBoolean(27)
                        };
                    }
                }
            }
            return result;
        }

        public Domain.Bill.Pay.PostponePayBill QueryPostponePayBill(decimal postponeApplyformId) {
            ClearParameters();
            Domain.Bill.Pay.PostponePayBill result = null;
            var paymentSql = "SELECT Id,OrderId,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,PayTradeNo,IsPoolpay,PayInterface,PayAccountType,ChannelTradeNo,Remark FROM dbo.T_Payment WHERE ApplyformId=@PostponeApplyformId AND [Type]=@PaymentType";
            var roleBillSql = "SELECT tRoleBill.Id,tRoleBill.[Owner],tRoleBill.[Role],tRoleBill.TradeRate,tRoleBill.Account,tRoleBill.PostponeFee,tRoleBill.Anticipation," +
                              "tRoleBill.TradeFee,tRoleBill.Amount,tRoleBill.Success,tRoleBill.TradeTime,tProfit.Premium,tProfit.TradeFee,tProfit.Account,tProfit.Success" +
                              " FROM dbo.T_PayBill tRoleBill" +
                              " LEFT JOIN dbo.T_TradementProfit tProfit ON tProfit.TradementType=@TradementType AND tProfit.TradementId=tRoleBill.PaymentId" +
                              " WHERE PaymentId=@PaymentId";
            AddParameter("PostponeApplyformId", postponeApplyformId);
            AddParameter("PaymentType", (byte)PaymentType.Postpone);
            Guid? paymentId = null;
            using(var reader = ExecuteReader(paymentSql)) {
                if(reader.Read()) {
                    paymentId = reader.GetGuid(0);
                    result = new Domain.Bill.Pay.PostponePayBill(reader.GetDecimal(1), postponeApplyformId) {
                        Tradement = new Payment(paymentId.Value) {
                            Amount = reader.GetDecimal(2),
                            TradeRate = reader.GetDecimal(3),
                            TradeFee = reader.GetDecimal(4),
                            PayeeAccount = reader.GetString(5),
                            PayAccount = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            TradeNo = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            ChannelTradeNo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            IsPoolpay = !reader.IsDBNull(8) && reader.GetBoolean(8)
                        },
                        Remark = reader.IsDBNull(12) ? string.Empty : reader.GetString(12)
                    };
                    if(!reader.IsDBNull(9)) {
                        result.Tradement.PayInterface = (PayInterface)reader.GetByte(9);
                    }
                    if(!reader.IsDBNull(10)) {
                        result.Tradement.PayAccountType = (PayAccountType)reader.GetByte(10);
                    }
                }
            }
            if(paymentId.HasValue) {
                ClearParameters();
                AddParameter("PaymentId", paymentId.Value);
                AddParameter("TradementType", (byte)TradementType.Pay);
                using(var reader = ExecuteReader(roleBillSql)) {
                    while(reader.Read()) {
                        var roleBill = constructPostponePayRoleBill(reader);
                        if(roleBill.Owner is Purchaser) {
                            result.Applier = roleBill;
                        } else if(roleBill.Owner is PlatformRoyalty) {
                            result.Accepter = new PlatformBill<PostponePayRoleBill, PostponePayRoleBillSource, PostponePayDetailBill> {
                                Premium = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                                TradeFee = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                                Account = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                Success = !reader.IsDBNull(14) && reader.GetBoolean(14)
                            };
                            result.Accepter.Deduction = roleBill;
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<NormalPayDetailBill> QueryNormalPayDetailBills(Guid normalPayRoleBillId) {
            ClearParameters();
            var result = new List<NormalPayDetailBill>();
            var sql = "SELECT Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,ServiceCharge,Commission,Increasing,Anticipation,TradeFee,Amount" +
                        " FROM dbo.T_PayDetailBill WHERE BillId=@PayRoleBillId";
            AddParameter("PayRoleBillId", normalPayRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                while(reader.Read()) {
                    var passenger = reader.GetGuid(0);
                    var flight = new Flight(reader.GetGuid(1)) {
                        ReleasedFare = reader.GetDecimal(2),
                        Fare = reader.GetDecimal(3),
                        AirportFee = reader.GetDecimal(4),
                        BAF = reader.GetDecimal(5)
                    };
                    result.Add(new NormalPayDetailBill(passenger, flight) {
                        ServiceCharge = reader.GetDecimal(6),
                        Commission = reader.GetDecimal(7),
                        Increasing = reader.GetDecimal(8),
                        Anticipation = reader.GetDecimal(9),
                        TradeFee = reader.GetDecimal(10),
                        Amount = reader.GetDecimal(11)
                    });
                }
            }
            return result;
        }

        public IEnumerable<PostponePayDetailBill> QueryPostponePayDetailBills(Guid postponePayRoleBillId) {
            ClearParameters();
            var result = new List<PostponePayDetailBill>();
            var sql = "SELECT Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,PostponeFee,Anticipation,TradeFee,Amount" +
                        " FROM dbo.T_PayDetailBill WHERE BillId=@PayRoleBillId";
            AddParameter("PayRoleBillId", postponePayRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                while(reader.Read()) {
                    var passenger = reader.GetGuid(0);
                    var flight = new Flight(reader.GetGuid(1)) {
                        ReleasedFare = reader.GetDecimal(2),
                        Fare = reader.GetDecimal(3),
                        AirportFee = reader.GetDecimal(4),
                        BAF = reader.GetDecimal(5)
                    };
                    result.Add(new PostponePayDetailBill(passenger, flight) {
                        PostponeFee = reader.GetDecimal(6),
                        Anticipation = reader.GetDecimal(7),
                        TradeFee = reader.GetDecimal(8),
                        Amount = reader.GetDecimal(9)
                    });
                }
            }
            return result;
        }

        public Domain.Bill.Refund.NormalRefundBill QueryNormalRefundBill(decimal normalRefundApplyformId) {
            ClearParameters();
            Domain.Bill.Refund.NormalRefundBill result = null;
            var refundmentSql = "SELECT Id,OrderId,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,RefundTradeNo,IsPoolpay,PayInterface,PayAccountType,ChannelTradeNo,Remark FROM dbo.T_Refundment WHERE ApplyformId=@RefundApplyformId";
            var roleBillSql = "SELECT Id,[Owner],[Role],TradeRate,TROLE.Account,ReleasedFare,Fare,AirportFee,BAF,Commission,Increasing,ServiceCharge,RefundFee," +
                                "Anticipation,TROLE.TradeFee,Amount,TROLE.Success,TradeTime,TPROFIT.Premium,TPROFIT.TradeFee,TPROFIT.Account,TPROFIT.Success" +
                                " FROM dbo.T_RefundBill TROLE" +
                                " LEFT JOIN dbo.T_TradementProfit TPROFIT ON TPROFIT.TradementType=@TradementType AND TPROFIT.TradementId=TROLE.RefundmentId" +
                                " WHERE TROLE.RefundmentId=@RefundmentId";
            AddParameter("RefundApplyformId", normalRefundApplyformId);
            Guid? refundmentId = null;
            using(var reader = ExecuteReader(refundmentSql)) {
                if(reader.Read()) {
                    refundmentId = reader.GetGuid(0);
                    result = new Domain.Bill.Refund.NormalRefundBill(reader.GetDecimal(1), normalRefundApplyformId) {
                        Tradement = new Refundment(refundmentId.Value) {
                            Amount = reader.GetDecimal(2),
                            TradeRate = reader.GetDecimal(3),
                            TradeFee = reader.GetDecimal(4),
                            PayeeAccount = reader.GetString(5),
                            PayAccount = reader.GetString(6),
                            TradeNo = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            ChannelTradeNo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            IsPoolpay = !reader.IsDBNull(8) && reader.GetBoolean(8)
                        },
                        Remark = reader.IsDBNull(12) ? string.Empty : reader.GetString(12)
                    };
                    if(!reader.IsDBNull(9)) {
                        result.Tradement.PayInterface = (PayInterface)reader.GetByte(9);
                    }
                    if(!reader.IsDBNull(10)) {
                        result.Tradement.PayAccountType = (PayAccountType)reader.GetByte(10);
                    }
                }
            }
            if(refundmentId.HasValue) {
                ClearParameters();
                AddParameter("RefundmentId", refundmentId.Value);
                AddParameter("TradementType", (byte)TradementType.Refund);
                using(var reader = ExecuteReader(roleBillSql)) {
                    while(reader.Read()) {
                        if(result.Platform == null) {
                            result.Platform = new PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> {
                                Premium = reader.GetDecimal(18),
                                TradeFee = reader.GetDecimal(19),
                                Account = reader.GetString(20),
                                Success = reader.GetBoolean(21)
                            };
                        }
                        var roleBill = constuctNormalRefundRoleBill(reader);
                        if(roleBill.Owner is Purchaser) {
                            result.Purchaser = roleBill;
                        } else if(roleBill.Owner is Provider) {
                            result.Provider = roleBill;
                        } else if(roleBill.Owner is Supplier) {
                            result.Supplier = roleBill;
                        } else if(roleBill.Owner is PlatformRoyalty) {
                            result.Platform.Deduction = roleBill;
                        } else if(roleBill.Owner is Royalty) {
                            result.AddRoyalty(roleBill);
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<Domain.Bill.Refund.NormalRefundBill> QueryNormalRefundBills(decimal orderId) {
            ClearParameters();
            var result = new List<Domain.Bill.Refund.NormalRefundBill>();
            var sql = new StringBuilder();
            sql.Append("SELECT TROLE.Id,TROLE.[Owner],TROLE.[Role],TROLE.TradeRate,TROLE.Account,TROLE.ReleasedFare,TROLE.Fare,TROLE.AirportFee,TROLE.BAF,");
            sql.Append("TROLE.Commission,TROLE.Increasing,TROLE.ServiceCharge,TROLE.RefundFee,TROLE.Anticipation,TROLE.TradeFee,TROLE.Amount,TROLE.Success,TROLE.TradeTime,");
            sql.Append("TPROFIT.Premium,TPROFIT.TradeFee,TPROFIT.Account,TPROFIT.Success,TREFUND.ApplyformId,TREFUND.Amount,TREFUND.TradeRate,TREFUND.TradeFee,");
            sql.Append("TREFUND.PayeeAccount,TREFUND.PayAccount,TREFUND.RefundTradeNo,TREFUND.IsPoolpay,TREFUND.PayInterface,TREFUND.PayAccountType,TREFUND.Id," +
                       "TREFUND.ChannelTradeNo,TREFUND.Remark");
            sql.Append(" FROM dbo.T_Refundment TREFUND");
            sql.Append(" INNER JOIN dbo.T_RefundBill TROLE ON TREFUND.ID=TROLE.RefundmentId");
            sql.Append(" LEFT JOIN dbo.T_TradementProfit TPROFIT ON TREFUND.ID=TPROFIT.TradementId AND TPROFIT.TradementType=@TradementType");
            sql.Append(" WHERE TREFUND.OrderId=@OrderId");
            sql.Append(" ORDER BY TREFUND.Id");
            AddParameter("TradementType", (byte)TradementType.Refund);
            AddParameter("OrderId", orderId);
            using(var reader = ExecuteReader(sql)) {
                Domain.Bill.Refund.NormalRefundBill refundBill = null;
                while(reader.Read()) {
                    var currentApplyformId = reader.GetDecimal(22);
                    if(refundBill == null || refundBill.ApplyformId != currentApplyformId) {
                        refundBill = new Domain.Bill.Refund.NormalRefundBill(orderId, currentApplyformId) {
                            Tradement = new Refundment(reader.GetGuid(32)) {
                                Amount = reader.GetDecimal(23),
                                TradeRate = reader.GetDecimal(24),
                                TradeFee = reader.GetDecimal(25),
                                PayeeAccount = reader.GetString(26),
                                PayAccount = reader.GetString(27),
                                TradeNo = reader.GetString(28),
                                ChannelTradeNo =reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                                IsPoolpay = !reader.IsDBNull(29) && reader.GetBoolean(29)
                            },
                            Remark = reader.IsDBNull(34) ? string.Empty : reader.GetString(34)
                        };
                        if(!reader.IsDBNull(30)) {
                            refundBill.Tradement.PayInterface = (PayInterface)reader.GetByte(30);
                        }
                        if(!reader.IsDBNull(31)) {
                            refundBill.Tradement.PayAccountType = (PayAccountType)reader.GetByte(31);
                        }
                        result.Add(refundBill);
                    }
                    if(refundBill.Platform == null) {
                        refundBill.Platform = new PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> {
                            Premium = reader.GetDecimal(18),
                            TradeFee = reader.GetDecimal(19),
                            Account = reader.GetString(20),
                            Success = reader.GetBoolean(21)
                        };
                    }
                    var roleBill = constuctNormalRefundRoleBill(reader);
                    if(roleBill.Owner is Purchaser) {
                        refundBill.Purchaser = roleBill;
                    } else if(roleBill.Owner is Provider) {
                        refundBill.Provider = roleBill;
                    } else if(roleBill.Owner is Supplier) {
                        refundBill.Supplier = roleBill;
                    } else if(roleBill.Owner is PlatformRoyalty) {
                        refundBill.Platform.Deduction = roleBill;
                    } else if(roleBill.Owner is Royalty) {
                        refundBill.AddRoyalty(roleBill);
                    }
                }
            }
            return result;
        }

        public Domain.Bill.Refund.PostponeRefundBill QueryPostponeRefundBill(decimal postponeRefundApplyformId) {
            ClearParameters();
            Domain.Bill.Refund.PostponeRefundBill result = null;
            var refundmentSql = "SELECT Id,OrderId,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,RefundTradeNo,IsPoolpay,PayInterface,PayAccountType,ChannelTradeNo,Remark FROM dbo.T_Refundment WHERE ApplyformId=@RefundApplyformId";
            var roleBillSql = "SELECT tRoleBill.Id,[Owner],tRoleBill.[Role],tRoleBill.TradeRate,tRoleBill.Account,tRoleBill.Anticipation,tRoleBill.TradeFee," +
                              "tRoleBill.Amount,tRoleBill.[Success],tRoleBill.TradeTime,tProfit.Premium,tProfit.TradeFee,tProfit.Account,tProfit.Success" +
                              " FROM dbo.T_RefundBill tRoleBill" +
                              " LEFT JOIN dbo.T_TradementProfit tProfit ON tProfit.TradementType=@TradementType AND tProfit.TradementId=tRoleBill.RefundmentId" +
                              " WHERE RefundmentId=@RefundmentId";
            AddParameter("RefundApplyformId", postponeRefundApplyformId);
            Guid? refundmentId = null;
            using(var reader = ExecuteReader(refundmentSql)) {
                if(reader.Read()) {
                    refundmentId = reader.GetGuid(0);
                    result = new Domain.Bill.Refund.PostponeRefundBill(postponeRefundApplyformId) {
                        Tradement = new Refundment(refundmentId.Value) {
                            Amount = reader.GetDecimal(2),
                            TradeRate = reader.GetDecimal(3),
                            TradeFee = reader.GetDecimal(4),
                            PayeeAccount = reader.GetString(5),
                            PayAccount = reader.GetString(6),
                            TradeNo = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            ChannelTradeNo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            IsPoolpay = !reader.IsDBNull(8) && reader.GetBoolean(8)
                        },
                        Remark = reader.IsDBNull(12) ? string.Empty : reader.GetString(12)
                    };
                    if(!reader.IsDBNull(9)) {
                        result.Tradement.PayInterface = (PayInterface)reader.GetByte(9);
                    }
                    if(!reader.IsDBNull(10)) {
                        result.Tradement.PayAccountType = (PayAccountType)reader.GetByte(10);
                    }
                }
            }
            if(refundmentId.HasValue) {
                ClearParameters();
                AddParameter("RefundmentId", refundmentId.Value);
                AddParameter("TradementType", (byte)TradementType.Refund);
                using(var reader = ExecuteReader(roleBillSql)) {
                    while(reader.Read()) {
                        var roleBill = constuctPostponeRefundRoleBill(reader);
                        if(roleBill != null) {
                            if(roleBill.Owner is Purchaser) {
                                result.Applier = roleBill;
                            } else if(roleBill.Owner is PlatformRoyalty) {
                                result.Accepter = new PlatformBill<PostponeRefundRoleBill, PostponeRefundRoleBillSource, PostponeRefundDetailBill> {
                                    Premium = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                                    TradeFee = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                                    Account = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                    Success = !reader.IsDBNull(13) && reader.GetBoolean(13)
                                };
                                result.Accepter.Deduction = roleBill;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<NormalRefundDetailBill> QueryNormalRefundDetailBills(Guid normalRefundRoleBillId) {
            ClearParameters();
            var result = new List<NormalRefundDetailBill>();
            var sql = "SELECT Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,Anticipation,TradeFee,Amount,RefundRate,RefundFee" +
                         " FROM dbo.T_RefundDetailBill WHERE BillId=@RefundRoleBillId";
            AddParameter("RefundRoleBillId", normalRefundRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                while(reader.Read()) {
                    var passenger = reader.GetGuid(0);
                    var flight = new Flight(reader.GetGuid(1)) {
                        ReleasedFare = reader.GetDecimal(2),
                        Fare = reader.GetDecimal(3),
                        AirportFee = reader.GetDecimal(4),
                        BAF = reader.GetDecimal(5)
                    };
                    result.Add(new NormalRefundDetailBill(passenger, flight) {
                        Anticipation = reader.GetDecimal(6),
                        TradeFee = reader.GetDecimal(7),
                        Amount = reader.GetDecimal(8),
                        RefundRate = reader.GetDecimal(9),
                        RefundFee = reader.GetDecimal(10)
                    });
                }
            }
            return result;
        }

        public IEnumerable<PostponeRefundDetailBill> QueryPostponeRefundDetailBills(Guid postponeRefundRoleBillId) {
            ClearParameters();
            var result = new List<PostponeRefundDetailBill>();
            var sql = "SELECT Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,Anticipation,TradeFee,Amount FROM dbo.T_RefundDetailBill WHERE BillId=@RefundRoleBillId";
            AddParameter("RefundRoleBillId", postponeRefundRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                while(reader.Read()) {
                    var passenger = reader.GetGuid(0);
                    var flight = new Flight(reader.GetGuid(1)) {
                        ReleasedFare = reader.GetDecimal(2),
                        Fare = reader.GetDecimal(3),
                        AirportFee = reader.GetDecimal(4),
                        BAF = reader.GetDecimal(5)
                    };
                    result.Add(new PostponeRefundDetailBill(passenger, flight) {
                        Anticipation = reader.GetDecimal(6),
                        TradeFee = reader.GetDecimal(7),
                        Amount = reader.GetDecimal(8)
                    });
                }
            }
            return result;
        }

        public Payment QueryPaymentByRefundTradeNo(string refundTradeNo) {
            ClearParameters();
            Payment result = null;
            var sql = "SELECT Id,Amount,TradeRate,TradeFee,PayAccount,PayeeAccount,PayTradeNo,IsPoolpay,PayInterface,PayAccountType,ChannelTradeNo" +
                          " FROM dbo.T_Payment WHERE EXISTS(SELECT NULL FROM dbo.T_Refundment WHERE " +
                          "RefundTradeNo=@RefundTradeNo AND T_Refundment.PayTradeNo=T_Payment.PayTradeNo)";
            AddParameter("RefundTradeNo", refundTradeNo);
            using(var reader = ExecuteReader(sql)) {
                if(reader.Read()) {
                    result = new Payment(reader.GetGuid(0)) {
                        Amount = reader.GetDecimal(1),
                        TradeRate = reader.GetDecimal(2),
                        TradeFee = reader.GetDecimal(3),
                        PayAccount = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        PayeeAccount = reader.GetString(5),
                        TradeNo = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        ChannelTradeNo = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        IsPoolpay = !reader.IsDBNull(7) && reader.GetBoolean(7)
                    };
                    if(!reader.IsDBNull(8)) {
                        result.PayInterface = (PayInterface)reader.GetByte(8);
                    }
                    if(!reader.IsDBNull(9)) {
                        result.PayAccountType = (PayAccountType)reader.GetByte(9);
                    }
                }
            }
            return result;
        }

        public NormalPayRoleBill QueryNormalPayRoleBillByRefundRoleBillId(Guid normalRefundRoleBillId) {
            ClearParameters();
            var sql = "SELECT Id,[Owner],[Role],TradeRate,Account,ReleasedFare,Fare,AirportFee,BAF,Rebate,ServiceCharge,Commission,Increasing," +
                                "Anticipation,TradeFee,Amount,Success,TradeTime FROM dbo.T_PayBill" +
                                " WHERE EXISTS(SELECT NULL FROM dbo.T_RefundBill WHERE T_RefundBill.Id=@RefundRoleBillId AND T_RefundBill.PayBillId=T_PayBill.Id)";
            AddParameter("RefundRoleBillId", normalRefundRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                if(reader.Read()) {
                    return constructNormalPayRoleBill(reader);
                }
            }
            return null;
        }

        public PostponePayRoleBill QueryPostponePayRoleBillByRefundRoleBillId(Guid postponeRefundRoleBillId) {
            ClearParameters();
            var sql = "SELECT Id,[Owner],[Role],TradeRate,Account,PostponeFee,Anticipation,TradeFee,Amount,Success,TradeTime" +
                         " FROM dbo.T_PayBill WHERE EXISTS(SELECT NULL FROM dbo.T_RefundBill WHERE T_RefundBill.Id=@RefundRoleBillId AND T_RefundBill.PayBillId=T_PayBill.Id)";
            AddParameter("RefundRoleBillId", postponeRefundRoleBillId);
            using(var reader = ExecuteReader(sql)) {
                if(reader.Read()) {
                    return constructPostponePayRoleBill(reader);
                }
            }
            return null;
        }

        private void deleteNormalPayBill(decimal orderId) {
            var paymentId = getNormalPaymentId(orderId);
            if(paymentId.HasValue) {
                deletePayBill(paymentId.Value);
            }
        }
        private void deleteNormalRoyaltiesPayRoleBill(Guid paymentId) {
            ClearParameters();
            var royalties = new List<TradeRoleType> { TradeRoleType.Royalty, TradeRoleType.Supplier, TradeRoleType.Provider, TradeRoleType.Platform };
            var royaltiesString = royalties.Join(",", item => ((byte)item).ToString());
            var sql = "DELETE FROM dbo.T_PayDetailBill WHERE EXISTS(SELECT NULL FROM dbo.T_PayBill WHERE Id=BillId AND PaymentId=@PaymentId AND [Role] IN (" + royaltiesString + "));" +
                "DELETE FROM dbo.T_PayBill WHERE PaymentId=@PaymentId AND [Role] IN (" + royaltiesString + ");" +
                "DELETE FROM dbo.T_TradementProfit WHERE TradementId=@PaymentId AND TradementType=@TradementType;";
            AddParameter("PaymentId", paymentId);
            AddParameter("TradementType", (byte)TradementType.Pay);
            ExecuteNonQuery(sql);
        }
        private void insertRoyaltiesPayBill(Domain.Bill.Pay.NormalPayBill payBill) {
            insertNormalPayRoleBill(payBill.Tradement.Id, payBill.Provider);
            insertNormalPayRoleBill(payBill.Tradement.Id, payBill.Supplier);
            foreach(var royalty in payBill.Royalties) {
                insertNormalPayRoleBill(payBill.Tradement.Id, royalty);
            }
            if(payBill.Platform != null) {
                insertNormalPayRoleBill(payBill.Tradement.Id, payBill.Platform.Deduction);
                insertTradementProfit(payBill.Tradement.Id, TradementType.Pay, payBill.Platform.Premium, payBill.Platform.TradeFee, payBill.Platform.Account, payBill.Platform.Success);
            }
        }
        private void insertNormalPayRoleBill(Guid paymentId, NormalPayRoleBill roleBill) {
            if(roleBill == null) return;
            insertNormalPayRoleMainBill(paymentId, roleBill);
            var dataCount = roleBill.Source.Details.Count();
            var saveCount = (dataCount / _payDefailBillMaxCount) + 1;
            for(int i = 0; i < saveCount; i++) {
                insertNormalPayDetailBill(roleBill.Id, roleBill.Source.Details.Skip(i * _payDefailBillMaxCount).Take(_payDefailBillMaxCount));
            }
        }
        private void insertNormalPayRoleMainBill(Guid paymentId, NormalPayRoleBill roleBill) {
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            ClearParameters();
            var roleBillSql = new StringBuilder();
            roleBillSql.Append("INSERT INTO dbo.T_PayBill (Id,PaymentId,[Owner],[Role],TradeRate,Account,Rebate,ReleasedFare,Fare,");
            roleBillSql.Append("AirportFee,BAF,ServiceCharge,Commission,Increasing,Anticipation,TradeFee,Amount,Success,TradeTime)");
            roleBillSql.Append(" VALUES ");
            roleBillSql.Append("(@Id,@PaymentId,@Owner,@Role,@TradeRate,@Account,@Rebate,@ReleasedFare,@Fare,");
            roleBillSql.Append("@AirportFee,@BAF,@ServiceCharge,@Commission,@Increasing,@Anticipation,@TradeFee,@Amount,@Success,@TradeTime);");
            AddParameter("Id", roleBill.Id);
            AddParameter("PaymentId", paymentId);
            AddParameter("Owner", roleBill.Owner.Id);
            AddParameter("Role", (byte)getTradeRoleType(roleBill.Owner));
            AddParameter("TradeRate", roleBill.Owner.Rate);
            AddParameter("Account", roleBill.Owner.Account ?? string.Empty);
            AddParameter("Rebate", roleBill.Source.Rebate);
            AddParameter("ReleasedFare", roleBill.Source.ReleasedFare);
            AddParameter("Fare", roleBill.Source.Fare);
            AddParameter("AirportFee", roleBill.Source.AirportFee);
            AddParameter("BAF", roleBill.Source.BAF);
            AddParameter("ServiceCharge", roleBill.Source.ServiceCharge);
            AddParameter("Commission", roleBill.Source.Commission);
            AddParameter("Increasing", roleBill.Source.Increasing);
            AddParameter("Anticipation", roleBill.Source.Anticipation);
            AddParameter("TradeFee", roleBill.Source.TradeFee);
            AddParameter("Amount", roleBill.Amount);
            AddParameter("Success", roleBill.Success);
            if(roleBill.Time.HasValue) {
                AddParameter("TradeTime", roleBill.Time.Value);
            } else {
                AddParameter("TradeTime", DBNull.Value);
            }
            ExecuteNonQuery(roleBillSql);
        }
        private void insertNormalPayDetailBill(Guid owner, IEnumerable<NormalPayDetailBill> bills) {
            ClearParameters();
            if(bills == null || !bills.Any()) return;
            var roleDetailBillSql = new StringBuilder();
            roleDetailBillSql.Append("INSERT INTO dbo.T_PayDetailBill ");
            roleDetailBillSql.Append("(BillId,Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,ServiceCharge,Commission,Increasing,Anticipation,TradeFee,Amount)");
            var detailIndex = 0;
            foreach(var detail in bills) {
                roleDetailBillSql.AppendFormat(" SELECT @BillId,@Passenger{0},@Flight{0},@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@ServiceCharge{0},@Commission{0},@Increasing{0},@Anticipation{0},@TradeFee{0},@Amount{0} UNION ALL", detailIndex);
                AddParameter("Passenger" + detailIndex, detail.Passenger);
                AddParameter("Flight" + detailIndex, detail.Flight.Id);
                AddParameter("ReleasedFare" + detailIndex, detail.Flight.ReleasedFare);
                AddParameter("Fare" + detailIndex, detail.Flight.Fare);
                AddParameter("AirportFee" + detailIndex, detail.Flight.AirportFee);
                AddParameter("BAF" + detailIndex, detail.Flight.BAF);
                AddParameter("ServiceCharge" + detailIndex, detail.ServiceCharge);
                AddParameter("Commission" + detailIndex, detail.Commission);
                AddParameter("Increasing" + detailIndex, detail.Increasing);
                AddParameter("Anticipation" + detailIndex, detail.Anticipation);
                AddParameter("TradeFee" + detailIndex, detail.TradeFee);
                AddParameter("Amount" + detailIndex, detail.Amount);
                detailIndex++;
            }
            AddParameter("BillId", owner);
            roleDetailBillSql.Remove(roleDetailBillSql.Length - 10, 10);
            ExecuteNonQuery(roleDetailBillSql);
        }
        private void updateFareOfNormalPayRoleBill(NormalPayRoleBill roleBill) {
            ClearParameters();
            if(roleBill == null) return;
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            var sql = new StringBuilder();
            sql.Append("UPDATE dbo.T_PayBill SET Fare=@TotalFare,ServiceCharge=@TotalServiceCharge WHERE Id=@RoleBillId;");
            var index = 0;
            foreach(var detail in roleBill.Source.Details) {
                sql.AppendFormat("UPDATE dbo.T_PayDetailBill SET Fare=@Fare{0},ServiceCharge=@ServiceCharge{0} WHERE BillId=@RoleBillId AND Flight=@Flight{0};", index);
                AddParameter("Fare" + index, detail.Flight.Fare);
                AddParameter("ServiceCharge" + index, detail.ServiceCharge);
                AddParameter("Flight" + index, detail.Flight.Id);
                index++;
            }
            AddParameter("TotalFare", roleBill.Source.Fare);
            AddParameter("TotalServiceCharge", roleBill.Source.ServiceCharge);
            AddParameter("RoleBillId", roleBill.Id);
            ExecuteNonQuery(sql);
        }
        private void updateNormalPayRoleBillPriceInfo(NormalPayRoleBill roleBill) {
            if(roleBill == null) return;
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            if(!roleBill.Source.Details.Any()) return;
            foreach(var detailBill in roleBill.Source.Details) {
                updateNormalPayDetailBill(roleBill.Id, detailBill);
            }
        }
        private void updateNormalPayDetailBill(Guid roleBillId, NormalPayDetailBill detailBill) {
            ClearParameters();
            var sql = "UPDATE dbo.T_PayDetailBill SET ReleasedFare=@ReleasedFare,Fare=@Fare,AirportFee=@AirportFee,BAF=@BAF,ServiceCharge=@ServiceCharge,Commission=@Commission," +
                "Anticipation=@Anticipation,TradeFee=@TradeFee,Amount=@Amount WHERE BillId=@BillId AND Flight=@Flight AND Passenger=@Passenger";
            AddParameter("BillId", roleBillId);
            AddParameter("Passenger", detailBill.Passenger);
            AddParameter("Flight", detailBill.Flight.Id);
            AddParameter("ReleasedFare", detailBill.Flight.ReleasedFare);
            AddParameter("Fare", detailBill.Flight.Fare);
            AddParameter("AirportFee", detailBill.Flight.AirportFee);
            AddParameter("BAF", detailBill.Flight.BAF);
            AddParameter("ServiceCharge", detailBill.ServiceCharge);
            AddParameter("Commission", detailBill.Commission);
            AddParameter("Anticipation", detailBill.Anticipation);
            AddParameter("TradeFee", detailBill.TradeFee);
            AddParameter("Amount", detailBill.Amount);
            ExecuteNonQuery(sql);
        }
        private void insertPostponePayRoleBill(Guid paymentId, PostponePayRoleBill roleBill) {
            ClearParameters();
            if(roleBill == null) return;
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            var roleBillSql = new StringBuilder();
            roleBillSql.Append("INSERT INTO dbo.T_PayBill");
            roleBillSql.Append(" (Id,PaymentId,[Owner],[Role],TradeRate,Account,PostponeFee,Anticipation,TradeFee,Amount,Success,TradeTime)");
            roleBillSql.Append(" VALUES ");
            roleBillSql.Append("(@Id,@PaymentId,@Owner,@Role,@TradeRate,@Account,@PostponeFee,@Anticipation,@TradeFee,@Amount,@Success,@TradeTime);");
            AddParameter("Id", roleBill.Id);
            AddParameter("PaymentId", paymentId);
            AddParameter("Owner", roleBill.Owner.Id);
            AddParameter("Role", (byte)getTradeRoleType(roleBill.Owner));
            AddParameter("TradeRate", roleBill.Owner.Rate);
            AddParameter("Account", roleBill.Owner.Account ?? string.Empty);
            AddParameter("PostponeFee", roleBill.Source.PostponeFee);
            AddParameter("Anticipation", roleBill.Source.Anticipation);
            AddParameter("TradeFee", roleBill.Source.TradeFee);
            AddParameter("Amount", roleBill.Amount);
            AddParameter("Success", roleBill.Success);
            if(roleBill.Time.HasValue) {
                AddParameter("TradeTime", roleBill.Time.Value);
            } else {
                AddParameter("TradeTime", DBNull.Value);
            }
            var roleDetailBillSql = new StringBuilder();
            if(roleBill.Source.Details.Any()) {
                roleDetailBillSql.Append("INSERT INTO dbo.T_PayDetailBill ");
                roleDetailBillSql.Append("(BillId,Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,PostponeFee,Anticipation,TradeFee,Amount)");
                var detailIndex = 0;
                foreach(var detail in roleBill.Source.Details) {
                    roleDetailBillSql.AppendFormat(" SELECT @BillId,@Passenger{0},@Flight{0},@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@PostponeFee{0},@Anticipation{0},@TradeFee{0},@Amount{0} UNION ALL", detailIndex);
                    AddParameter("Passenger" + detailIndex, detail.Passenger);
                    AddParameter("Flight" + detailIndex, detail.Flight.Id);
                    AddParameter("ReleasedFare" + detailIndex, detail.Flight.ReleasedFare);
                    AddParameter("Fare" + detailIndex, detail.Flight.Fare);
                    AddParameter("AirportFee" + detailIndex, detail.Flight.AirportFee);
                    AddParameter("BAF" + detailIndex, detail.Flight.BAF);
                    AddParameter("PostponeFee" + detailIndex, detail.PostponeFee);
                    AddParameter("Anticipation" + detailIndex, detail.Anticipation);
                    AddParameter("TradeFee" + detailIndex, detail.TradeFee);
                    AddParameter("Amount" + detailIndex, detail.Amount);
                    detailIndex++;
                }
                AddParameter("BillId", roleBill.Id);
                roleDetailBillSql.Remove(roleDetailBillSql.Length - 10, 10);
                roleDetailBillSql.Append(";");
            }
            ExecuteNonQuery(roleBillSql.ToString() + roleDetailBillSql);
        }
        private void insertNormalRefundRoleBill(Guid refundmentId, NormalRefundRoleBill roleBill) {
            if(roleBill == null) return;
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            insertNomalRefundRoleMainBill(refundmentId, roleBill);

            var dataCount = roleBill.Source.Details.Count();
            var saveCount = (dataCount / _refundDefailBillMaxCount) + 1;
            for(int i = 0; i < saveCount; i++) {
                insertNormalRefundDetailBill(roleBill.Id, roleBill.Source.Details.Skip(i * _refundDefailBillMaxCount).Take(_refundDefailBillMaxCount));
            }
        }
        private void insertNomalRefundRoleMainBill(Guid refundmentId, NormalRefundRoleBill roleBill) {
            ClearParameters();
            var roleBillSql = new StringBuilder();
            roleBillSql.Append("INSERT INTO dbo.T_RefundBill (Id,RefundmentId,PayBillId,[Owner],[Role],TradeRate,Account,ReleasedFare,Fare,");
            roleBillSql.Append("AirportFee,BAF,Commission,Increasing,ServiceCharge,RefundFee,Anticipation,TradeFee,Amount,Success,TradeTime)");
            roleBillSql.Append(" VALUES ");
            roleBillSql.Append("(@Id,@RefundmentId,@PayBillId,@Owner,@Role,@TradeRate,@Account,@ReleasedFare,@Fare,@AirportFee,@BAF,");
            roleBillSql.Append("@Commission,@Increasing,@ServiceCharge,@RefundFee,@Anticipation,@TradeFee,@Amount,@Success,@TradeTime)");
            AddParameter("Id", roleBill.Id);
            AddParameter("RefundmentId", refundmentId);
            AddParameter("PayBillId", roleBill.PayRoleBill.Id);
            AddParameter("Owner", roleBill.Owner.Id);
            AddParameter("Role", (byte)getTradeRoleType(roleBill.Owner));
            AddParameter("TradeRate", roleBill.Owner.Rate);
            AddParameter("Account", roleBill.Owner.Account);
            AddParameter("ReleasedFare", roleBill.Source.ReleasedFare);
            AddParameter("Fare", roleBill.Source.Fare);
            AddParameter("AirportFee", roleBill.Source.AirportFee);
            AddParameter("BAF", roleBill.Source.BAF);
            AddParameter("Commission", roleBill.Source.Commission);
            AddParameter("Increasing", roleBill.Source.Increasing);
            AddParameter("ServiceCharge", roleBill.Source.ServiceCharge);
            AddParameter("RefundFee", roleBill.Source.RefundFee);
            AddParameter("Anticipation", roleBill.Source.Anticipation);
            AddParameter("TradeFee", roleBill.Source.TradeFee);
            AddParameter("Amount", roleBill.Amount);
            AddParameter("Success", roleBill.Success);
            if(roleBill.Time.HasValue) {
                AddParameter("TradeTime", roleBill.Time.Value);
            } else {
                AddParameter("TradeTime", DBNull.Value);
            }
            ExecuteNonQuery(roleBillSql);
        }
        private void insertNormalRefundDetailBill(Guid roleBillId, IEnumerable<NormalRefundDetailBill> roleBillDetail) {
            ClearParameters();
            if(roleBillDetail == null || !roleBillDetail.Any()) return;
            var roleDetailBillSql = new StringBuilder();
            roleDetailBillSql.Append("INSERT INTO dbo.T_RefundDetailBill");
            roleDetailBillSql.Append(" (BillId,Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,Commission,Increasing,ServiceCharge,Anticipation,TradeFee,Amount,RefundRate,RefundFee)");
            var detailIndex = 0;
            foreach(var detail in roleBillDetail) {
                roleDetailBillSql.AppendFormat(" SELECT @BillId,@Passenger{0},@Flight{0},@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@Commission{0},@Increasing{0},", detailIndex);
                roleDetailBillSql.AppendFormat("@ServiceCharge{0},@Anticipation{0},@TradeFee{0},@Amount{0},@RefundRate{0},@RefundFee{0} UNION ALL", detailIndex);
                AddParameter("Passenger" + detailIndex, detail.Passenger);
                AddParameter("Flight" + detailIndex, detail.Flight.Id);
                AddParameter("ReleasedFare" + detailIndex, detail.Flight.ReleasedFare);
                AddParameter("Fare" + detailIndex, detail.Flight.Fare);
                AddParameter("AirportFee" + detailIndex, detail.Flight.AirportFee);
                AddParameter("BAF" + detailIndex, detail.Flight.BAF);
                AddParameter("Commission" + detailIndex, detail.Commission);
                AddParameter("Increasing" + detailIndex, detail.Increasing);
                AddParameter("ServiceCharge" + detailIndex, detail.ServiceCharge);
                AddParameter("Anticipation" + detailIndex, detail.Anticipation);
                AddParameter("TradeFee" + detailIndex, detail.TradeFee);
                AddParameter("Amount" + detailIndex, detail.Amount);
                AddParameter("RefundRate" + detailIndex, detail.RefundRate);
                AddParameter("RefundFee" + detailIndex, detail.RefundFee);
                detailIndex++;
            }
            AddParameter("BillId", roleBillId);
            roleDetailBillSql.Remove(roleDetailBillSql.Length - 10, 10);
            roleDetailBillSql.Append(";");

            ExecuteNonQuery(roleDetailBillSql);
        }
        private void insertPostponeRefundRoleBill(Guid refundmentId, PostponeRefundRoleBill roleBill) {
            ClearParameters();
            if(roleBill == null) return;
            if(roleBill.Owner == null) throw new Core.CustomException("缺少账单的所有者");
            if(roleBill.Source == null) throw new Core.CustomException("缺少账单来源");
            var roleBillSql = new StringBuilder();
            roleBillSql.Append("INSERT INTO dbo.T_RefundBill");
            roleBillSql.Append(" (Id,RefundmentId,PayBillId,[Owner],[Role],TradeRate,Account,Anticipation,TradeFee,Amount,Success,TradeTime)");
            roleBillSql.Append(" VALUES ");
            roleBillSql.Append("(@Id,@RefundmentId,@PayBillId,@Owner,@Role,@TradeRate,@Account,@Anticipation,@TradeFee,@Amount,@Success,@TradeTime)");
            AddParameter("Id", roleBill.Id);
            AddParameter("RefundmentId", refundmentId);
            AddParameter("PayBillId", roleBill.PayRoleBill.Id);
            AddParameter("Owner", roleBill.Owner.Id);
            AddParameter("Role", (byte)getTradeRoleType(roleBill.Owner));
            AddParameter("TradeRate", roleBill.Owner.Rate);
            AddParameter("Account", roleBill.Owner.Account);
            AddParameter("Anticipation", roleBill.Source.Anticipation);
            AddParameter("TradeFee", roleBill.Source.TradeFee);
            AddParameter("Amount", roleBill.Amount);
            AddParameter("Success", roleBill.Success);
            if(roleBill.Time.HasValue) {
                AddParameter("TradeTime", roleBill.Time.Value);
            } else {
                AddParameter("TradeTime", DBNull.Value);
            }
            var roleDetailBillSql = new StringBuilder();
            if(roleBill.Source.Details.Any()) {
                roleDetailBillSql.Append("INSERT INTO dbo.T_RefundDetailBill");
                roleDetailBillSql.Append(" (BillId,Passenger,Flight,ReleasedFare,Fare,AirportFee,BAF,Anticipation,TradeFee,Amount)");
                int detailIndex = 0;
                foreach(var detail in roleBill.Source.Details) {
                    roleDetailBillSql.AppendFormat(" SELECT @BillId,@Passenger{0},@Flight{0},@ReleasedFare{0},@Fare{0},@AirportFee{0},@BAF{0},@Anticipation{0},@TradeFee{0},@Amount{0} UNION ALL", detailIndex);
                    AddParameter("Passenger" + detailIndex, detail.Passenger);
                    AddParameter("Flight" + detailIndex, detail.Flight.Id);
                    AddParameter("ReleasedFare" + detailIndex, detail.Flight.ReleasedFare);
                    AddParameter("Fare" + detailIndex, detail.Flight.Fare);
                    AddParameter("AirportFee" + detailIndex, detail.Flight.AirportFee);
                    AddParameter("BAF" + detailIndex, detail.Flight.BAF);
                    AddParameter("Anticipation" + detailIndex, detail.Anticipation);
                    AddParameter("TradeFee" + detailIndex, detail.TradeFee);
                    AddParameter("Amount" + detailIndex, detail.Amount);
                    detailIndex++;
                }
                AddParameter("BillId", roleBill.Id);
                roleDetailBillSql.Remove(roleDetailBillSql.Length - 10, 10);
                roleDetailBillSql.Append(";");
            }
            ExecuteNonQuery(roleBillSql.ToString() + roleDetailBillSql);
        }
        private void insertPayment(Domain.Bill.Pay.NormalPayBill payBill) {
            insertPayment(payBill.OrderId, null, payBill.Tradement, PaymentType.Normal, payBill.Remark);
        }
        private void insertPayment(Domain.Bill.Pay.PostponePayBill payBill) {
            insertPayment(payBill.OrderId, payBill.ApplyformId, payBill.Tradement, PaymentType.Postpone, payBill.Remark);
        }
        private void insertRefundment(Domain.Bill.Refund.NormalRefundBill refundBill) {
            insertRefundment(refundBill.OrderId, refundBill.ApplyformId, refundBill.Tradement, refundBill.Remark);
        }
        private void insertRefundment(Domain.Bill.Refund.PostponeRefundBill refundBill) {
            insertRefundment(refundBill.ApplyformId, refundBill.ApplyformId, refundBill.Tradement, refundBill.Remark);
        }
        private void insertPayment(decimal orderId, decimal? applyformId, Payment payment, PaymentType paymentType, string remark) {
            ClearParameters();
            var sql = "INSERT INTO dbo.T_Payment (Id,OrderId,ApplyformId,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,PayTradeNo,ChannelTradeNo,[Type],IsPoolpay,PayInterface,PayAccountType,Remark)" +
                " VALUES (@Id,@OrderId,@ApplyformId,@Amount,@TradeRate,@TradeFee,@PayeeAccount,@PayAccount,@PayTradeNo,@ChannelTradeNo,@PaymentType,@IsPoolpay,@PayInterface,@PayAccountType,@Remark)";
            AddParameter("Id", payment.Id);
            AddParameter("OrderId", orderId);
            if(applyformId.HasValue) {
                AddParameter("ApplyformId", applyformId.Value);
            } else {
                AddParameter("ApplyformId", DBNull.Value);
            }
            AddParameter("Amount", payment.Amount);
            AddParameter("TradeRate", payment.TradeRate);
            AddParameter("TradeFee", payment.TradeFee);
            AddParameter("PayeeAccount", payment.PayeeAccount ?? string.Empty);
            AddParameter("PayAccount", payment.PayAccount ?? string.Empty);
            if(string.IsNullOrWhiteSpace(payment.TradeNo)) {
                AddParameter("PayTradeNo", string.Empty);
            } else {
                AddParameter("PayTradeNo", payment.TradeNo.Trim());
            }
            //2013-5-2 wangsl 新增
            if(string.IsNullOrWhiteSpace(payment.TradeNo)) {
                AddParameter("ChannelTradeNo", string.Empty);
            } else {
                AddParameter("ChannelTradeNo", payment.ChannelTradeNo.Trim());
            }
            AddParameter("PaymentType", (byte)paymentType);
            AddParameter("IsPoolpay", payment.IsPoolpay);
            if(payment.PayInterface.HasValue) {
                AddParameter("PayInterface", (byte)payment.PayInterface.Value);
            } else {
                AddParameter("PayInterface", DBNull.Value);
            }
            if(payment.PayAccountType.HasValue) {
                AddParameter("PayAccountType", (byte)payment.PayAccountType.Value);
            } else {
                AddParameter("PayAccountType", DBNull.Value);
            }
            AddParameter("Remark", remark ?? string.Empty);
            ExecuteNonQuery(sql);
        }
        private void insertRefundment(decimal orderId, decimal refundApplyformId, Refundment refundment, string remark) {
            ClearParameters();
            var sql = "INSERT INTO dbo.T_Refundment ([Id],OrderId,PayTradeNo,ChannelTradeNo,ApplyformId,Amount,TradeRate,TradeFee,PayeeAccount,PayAccount,RefundTradeNo,IsPoolpay,PayInterface,PayAccountType,Remark)" +
                    " VALUES (@Id,@OrderId,@PayTradeNo,@PayChannelTradeNo,@ApplyformId,@Amount,@TradeRate,@TradeFee,@PayeeAccount,@PayAccount,@RefundTradeNo,@IsPoolpay,@PayInterface,@PayAccountType,@Remark)";
            AddParameter("Id", refundment.Id);
            AddParameter("OrderId", orderId);
            AddParameter("PayTradeNo", refundment.Payment.TradeNo ?? string.Empty);
            AddParameter("PayChannelTradeNo", refundment.Payment.ChannelTradeNo ?? string.Empty);
            AddParameter("ApplyformId", refundApplyformId);
            AddParameter("Amount", refundment.Amount);
            AddParameter("TradeRate", refundment.TradeRate);
            AddParameter("TradeFee", refundment.TradeFee);
            AddParameter("PayeeAccount", refundment.PayeeAccount ?? string.Empty);
            AddParameter("PayAccount", refundment.PayAccount ?? string.Empty);
            AddParameter("RefundTradeNo", refundment.TradeNo ?? string.Empty);
            AddParameter("IsPoolpay", refundment.IsPoolpay);
            if(refundment.PayInterface.HasValue) {
                AddParameter("PayInterface", (byte)refundment.PayInterface.Value);
            } else {
                AddParameter("PayInterface", DBNull.Value);
            }
            if(refundment.PayAccountType.HasValue) {
                AddParameter("PayAccountType", (byte)refundment.PayAccountType.Value);
            } else {
                AddParameter("PayAccountType", DBNull.Value);
            }
            AddParameter("Remark", remark ?? string.Empty);
            ExecuteNonQuery(sql);
        }
        private void insertTradementProfit(Guid tradementId, TradementType tradementType, decimal premium, decimal tradeFee, string account, bool success) {
            ClearParameters();
            var sql = "INSERT INTO dbo.T_TradementProfit (TradementId,TradementType,Premium,TradeFee,Account,Success)"
                + " VALUES (@TradementId,@TradementType,@Premium,@TradeFee,@Account,@Success)";
            AddParameter("TradementId", tradementId);
            AddParameter("TradementType", (byte)tradementType);
            AddParameter("Premium", premium);
            AddParameter("TradeFee", tradeFee);
            AddParameter("Account", account);
            AddParameter("Success", success);
            ExecuteNonQuery(sql);
        }
        private NormalPayRoleBill constructNormalPayRoleBill(DbDataReader reader) {
            NormalPayRoleBill result = null;
            var ownerId = reader.GetGuid(1);
            var roleType = (TradeRoleType)reader.GetByte(2);
            var tradeRate = reader.GetDecimal(3);
            var account = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            result = new NormalPayRoleBill(reader.GetGuid(0), getTradeRole(roleType, ownerId, tradeRate, account)) {
                Source = new NormalPayRoleBillSource {
                    ReleasedFare = reader.GetDecimal(5),
                    Fare = reader.GetDecimal(6),
                    AirportFee = reader.GetDecimal(7),
                    BAF = reader.GetDecimal(8),
                    Rebate = reader.GetDecimal(9),
                    ServiceCharge = reader.GetDecimal(10),
                    Commission = reader.GetDecimal(11),
                    Increasing = reader.GetDecimal(12),
                    Anticipation = reader.GetDecimal(13),
                    TradeFee = reader.GetDecimal(14)
                },
                Amount = reader.GetDecimal(15),
                Success = reader.GetBoolean(16)
            };
            if(!reader.IsDBNull(17)) {
                result.Time = reader.GetDateTime(17);
            }
            return result;
        }
        private PostponePayRoleBill constructPostponePayRoleBill(DbDataReader reader) {
            PostponePayRoleBill result = null;
            var ownerId = reader.GetGuid(1);
            var roleType = (TradeRoleType)reader.GetByte(2);
            var tradeRate = reader.GetDecimal(3);
            var account = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            result = new PostponePayRoleBill(reader.GetGuid(0), getTradeRole(roleType, ownerId, tradeRate, account)) {
                Source = new PostponePayRoleBillSource {
                    PostponeFee = reader.GetDecimal(5),
                    Anticipation = reader.GetDecimal(6),
                    TradeFee = reader.GetDecimal(7)
                },
                Amount = reader.GetDecimal(8),
                Success = reader.GetBoolean(9)
            };
            if(!reader.IsDBNull(10)) {
                result.Time = reader.GetDateTime(10);
            }
            return result;
        }
        private NormalRefundRoleBill constuctNormalRefundRoleBill(DbDataReader reader) {
            NormalRefundRoleBill result = null;
            var ownerId = reader.GetGuid(1);
            var roleType = (TradeRoleType)reader.GetByte(2);
            var tradeRate = reader.GetDecimal(3);
            var account = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            result = new NormalRefundRoleBill(reader.GetGuid(0), getTradeRole(roleType, ownerId, tradeRate, account)) {
                Source = new NormalRefundRoleBillSource() {
                    ReleasedFare = reader.GetDecimal(5),
                    Fare = reader.GetDecimal(6),
                    AirportFee = reader.GetDecimal(7),
                    BAF = reader.GetDecimal(8),
                    Commission = reader.GetDecimal(9),
                    ServiceCharge = reader.GetDecimal(10),
                    RefundFee = reader.GetDecimal(11),
                    Increasing = reader.GetDecimal(12),
                    Anticipation = reader.GetDecimal(13),
                    TradeFee = reader.GetDecimal(14)
                },
                Amount = reader.GetDecimal(15),
                Success = reader.GetBoolean(16)
            };
            if(!reader.IsDBNull(17)) {
                result.Time = reader.GetDateTime(17);
            }
            return result;
        }
        private PostponeRefundRoleBill constuctPostponeRefundRoleBill(DbDataReader reader) {
            PostponeRefundRoleBill result = null;
            var ownerId = reader.GetGuid(1);
            var roleType = (TradeRoleType)reader.GetByte(2);
            var tradeRate = reader.GetDecimal(3);
            var account = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            result = new PostponeRefundRoleBill(reader.GetGuid(0), getTradeRole(roleType, ownerId, tradeRate, account)) {
                Source = new PostponeRefundRoleBillSource {
                    Anticipation = reader.GetDecimal(5),
                    TradeFee = reader.GetDecimal(6)
                },
                Amount = reader.GetDecimal(7),
                Success = reader.GetBoolean(8)
            };
            if(!reader.IsDBNull(9)) {
                result.Time = reader.GetDateTime(9);
            }
            return result;
        }
        private Guid? getNormalPaymentId(decimal orderId) {
            ClearParameters();
            var sql = "SELECT Id FROM dbo.T_Payment WHERE OrderId=@OrderId AND [Type]=@PaymentType";
            AddParameter("OrderId", orderId);
            AddParameter("PaymentType", (byte)PaymentType.Normal);
            var id = ExecuteScalar(sql);
            if(id == null || id == DBNull.Value) {
                return null;
            } else {
                return (Guid)id;
            }
        }
        private Guid? getRefundmentId(decimal refundApplyformId) {
            ClearParameters();
            var sql = "SELECT Id FROM dbo.T_Refundment WHERE ApplyformId=@ApplyformId";
            AddParameter("ApplyformId", refundApplyformId);
            var id = ExecuteScalar(sql);
            if(id == null || id == DBNull.Value) {
                return null;
            } else {
                return (Guid)id;
            }
        }
        private void deletePayBill(Guid paymentId) {
            ClearParameters();
            var deleteSql = "DELETE FROM dbo.T_PayDetailBill WHERE EXISTS(SELECT NULL FROM dbo.T_PayBill WHERE PaymentId=@PaymentId AND BillId=Id);" +
                            "DELETE FROM dbo.T_PayBill WHERE PaymentId=@PaymentId;" +
                            "DELETE FROM dbo.T_Payment WHERE Id=@PaymentId;" +
                            "DELETE FROM dbo.T_TradementProfit WHERE TradementId=@PaymentId AND TradementType=@TradementType;";
            AddParameter("PaymentId", paymentId);
            AddParameter("TradementType", (byte)TradementType.Pay);
            ExecuteNonQuery(deleteSql);
        }
        private void deleteRefundBill(Guid refundmentId) {
            ClearParameters();
            var deleteSql = "DELETE FROM dbo.T_RefundDetailBill WHERE EXISTS(SELECT NULL FROM dbo.T_RefundBill WHERE RefundmentId=@RefundmentId AND BillId=Id);" +
                        "DELETE FROM dbo.T_RefundBill WHERE RefundmentId=@RefundmentId;" +
                        "DELETE FROM dbo.T_TradementProfit WHERE TradementId=@RefundmentId AND TradementType=@TradementType;" +
                        "DELETE FROM dbo.T_Refundment WHERE Id=@RefundmentId;";
            AddParameter("RefundmentId", refundmentId);
            AddParameter("TradementType", (byte)TradementType.Refund);
            ExecuteNonQuery(deleteSql);
        }

        private TradeRole getTradeRole(TradeRoleType roleType, Guid ownerId, decimal tradeRate, string account) {
            switch(roleType) {
                case TradeRoleType.Purchaser:
                    return new Purchaser(ownerId, account, tradeRate);
                case TradeRoleType.Supplier:
                    return new Supplier(ownerId, account, tradeRate);
                case TradeRoleType.Provider:
                    return new Provider(ownerId, account, tradeRate);
                case TradeRoleType.Platform:
                    return new PlatformRoyalty(ownerId, account, tradeRate);
                case TradeRoleType.Royalty:
                    return new Royalty(ownerId, account, tradeRate);
                default:
                    throw new Core.CustomException("未知交易角色");
            }
        }
        private TradeRoleType getTradeRoleType(TradeRole tradeRole) {
            return tradeRole.RoleType;
            //if(tradeRole is Purchaser) {
            //    return TradeRoleType.Purchaser;
            //} else if(tradeRole is Provider) {
            //    return TradeRoleType.Provider;
            //} else if(tradeRole is Supplier) {
            //    return TradeRoleType.Supplier;
            //} else if(tradeRole is PlatformRoyalty) {
            //    return TradeRoleType.Platform;
            //} else if(tradeRole is Royalty) {
            //    return TradeRoleType.Royalty;
            //} else {
            //    throw new Core.CustomException("未知角色类型");
            //}
        }
        private string prepareUpdatePaymentForPaySuccessSql(Payment payment, int serial) {
            AddParameter("PaymentId" + serial, payment.Id);
            AddParameter("PayAccount" + serial, payment.PayAccount);
            AddParameter("PayTradeNo" + serial, payment.TradeNo);
            AddParameter("ChannelTradeNo" + serial, payment.ChannelTradeNo);
            AddParameter("IsPoolpay" + serial, payment.IsPoolpay);
            if(payment.PayInterface.HasValue) {
                AddParameter("PayInterface" + serial, (byte)payment.PayInterface.Value);
            } else {
                AddParameter("PayInterface" + serial, DBNull.Value);
            }
            if(payment.PayAccountType.HasValue) {
                AddParameter("PayAccountType" + serial, (byte)payment.PayAccountType.Value);
            } else {
                AddParameter("PayAccountType" + serial, DBNull.Value);
            }
            return string.Format("UPDATE dbo.T_Payment SET PayAccount=@PayAccount{0},PayTradeNo=@PayTradeNo{0},ChannelTradeNo=@ChannelTradeNo{0}," +
                "IsPoolpay=@IsPoolpay{0},PayInterface=@PayInterface{0},PayAccountType=@PayAccountType{0} WHERE Id=@PaymentId{0};", serial);
        }
        private string prepareUpdatePayRoleBillForPaySuccessSql(Guid billId, string payAccount, DateTime payTime, int serial) {
            AddParameter("TradeTime" + serial, payTime);
            AddParameter("Success" + serial, true);
            AddParameter("PayAccount" + serial, payAccount);
            AddParameter("BillId" + serial, billId);
            return string.Format("UPDATE dbo.T_PayBill SET Success=@Success{0},TradeTime=@TradeTime{0},Account=@PayAccount{0} WHERE Id=@BillId{0};", serial);
        }
        private string prepareUpdatePayRoleBillForTradeSuccessSql(Guid billId, DateTime successTime, int serial) {
            AddParameter("Success" + serial, true);
            AddParameter("Id" + serial, billId);
            AddParameter("PayTime" + serial, successTime);
            return string.Format("UPDATE dbo.T_PayBill SET Success=@Success{0},TradeTime=@PayTime{0} WHERE [Id]=@Id{0};", serial);
        }
        private string prepareUpdateRefundRoleBillForTradeSuccessSql(Guid billId, DateTime successTime, int serial) {
            AddParameter("Success" + serial, true);
            AddParameter("Id" + serial, billId);
            AddParameter("RefundTime" + serial, successTime);
            return string.Format("UPDATE dbo.T_RefundBill SET Success=@Success{0},TradeTime=@RefundTime{0} WHERE [Id]=@Id{0};", serial);
        }
        private string prepareUpdateTradementProfitForStatusSql(Guid tradementId, TradementType tradementType, int serial) {
            AddParameter("Success" + serial, true);
            AddParameter("TradementId" + serial, tradementId);
            AddParameter("TradementType" + serial, (byte)tradementType);
            return string.Format("UPDATE dbo.T_TradementProfit SET Success=@Success{0} WHERE TradementId=@TradementId{0} AND TradementType=@TradementType{0};", serial);
        }
    }
}