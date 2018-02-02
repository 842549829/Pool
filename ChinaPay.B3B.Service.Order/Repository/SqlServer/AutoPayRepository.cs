using System;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer
{
    public class AutoPayRepository : SqlServerTransaction, IAutoPayRepository
    {
        public AutoPayRepository(DbOperator dbOperator)
            : base(dbOperator)
        {

        }
        public void Insert(Domain.AutoPay.AutoPay pay)
        {
            AddParameter("OrderId", pay.OrderId);
            AddParameter("PayAccountNo", pay.PayAccountNo);
            AddParameter("PayType", pay.PayType);
            AddParameter("OrderType", pay.OrderType);
            AddParameter("Time", pay.Time);
            AddParameter("Success", pay.Success);
            AddParameter("ProcessState", pay.ProcessState);
            ExecuteNonQuery("INSERT INTO [dbo].[T_AutoPay]([OrderId],[PayAccountNo],[PayType],[OrderType],[Time],[Success],[ProcessState])VALUES (@OrderId ,@PayAccountNo ,@PayType ,@OrderType,@Time ,@Success ,@ProcessState);");
        }

        public void Update(Domain.AutoPay.AutoPay pay)
        {
            throw new NotImplementedException();
        }

        public Domain.AutoPay.AutoPay Query(decimal orderId)
        {
            using (var reader = ExecuteReader("SELECT OrderId,PayAccountNo,PayType,Time,Success,ProcessState,OrderType FROM T_AutoPay WHERE ProcessState=0 AND OrderId = " + orderId))
            {
                Domain.AutoPay.AutoPay auto = null;
                if (reader.Read())
                {
                    auto = new Domain.AutoPay.AutoPay();
                    auto.OrderId = reader.GetDecimal(0);
                    auto.PayAccountNo = reader.GetString(1);
                    auto.PayType = (WithholdingAccountType)reader.GetByte(2);
                    auto.Time = reader.GetDateTime(3);
                    auto.Success = reader.GetBoolean(4);
                    auto.ProcessState = reader.GetBoolean(5);
                    auto.OrderType = (OrderType)reader.GetByte(6);
                }
                return auto;
            }
        }

        public List<Domain.AutoPay.AutoPay> Query(DataTransferObject.Order.AutoPayCondition condition, Core.Pagination pagination)
        {
            throw new NotImplementedException();
        }


        public void UpdateSuccess(decimal orderId)
        {
            ExecuteNonQuery("UPDATE T_AutoPay SET Success = 1 WHERE OrderId = " + orderId);
        }


        public void UpdateProcess(decimal orderId)
        {
            ExecuteNonQuery("UPDATE T_AutoPay SET ProcessState = 1 WHERE OrderId = " + orderId);
        }

        public List<Domain.AutoPay.AutoPay> QueryNoPorcess()
        {
            string sql = "SELECT OrderId,PayAccountNo,PayType,Time,Success,ProcessState,OrderType FROM T_AutoPay WHERE ProcessState = 0 ORDER BY Time";
           
                using (var reader = ExecuteReader(sql))
                {
                    List<Domain.AutoPay.AutoPay> list = new List<Domain.AutoPay.AutoPay>();
                    while (reader.Read())
                    {
                        Domain.AutoPay.AutoPay auto = new Domain.AutoPay.AutoPay();
                        auto.OrderId = reader.GetDecimal(0);
                        auto.PayAccountNo = reader.GetString(1);
                        auto.PayType = (WithholdingAccountType)reader.GetByte(2);
                        auto.Time = reader.GetDateTime(3);
                        auto.Success = reader.GetBoolean(4);
                        auto.ProcessState = reader.GetBoolean(5);
                        auto.OrderType = (OrderType)reader.GetByte(6);
                        list.Add(auto);
                    }
                    return list;
                }
        }
    }
}
