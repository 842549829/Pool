using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Integral.Repository.SqlServer
{
    using System.Collections.Generic;
    using ChinaPay.B3B.DataTransferObject.Integral;
    using ChinaPay.B3B.Service.Integral.Domain;
    using ChinaPay.DataAccess;
    using ChinaPay.Repository;
    using ChinaPay.B3B.Common.Enums;
    public class IntegralResposity : SqlServerTransaction, IReposity
    {
        public IntegralResposity(DbOperator dbOperator)
            : base(dbOperator)
        {
        }

        public IEnumerable<DataTransferObject.Integral.IntegralInfoView> GetIntegralList(Core.Range<DateTime>? time, Guid? accountID, Common.Enums.IntegralWay? way, Pagination pagination)
        {
            List<IntegralInfoView> result = null;
            var fields = " [CompnayId]  ,[Integral]  ,[IntegralWay] ,[AccessTime]   ,[Remark] ";
            string iCondition = " ";

            var catelog = "[dbo].[T_IntegralInfo]";
            var orderbyFiled = " AccessTime desc";

            iCondition += "  convert(date,[AccessTime]) >=  '" + time.Value.Lower.Date + "'";
            iCondition += " AND convert(date,[AccessTime]) <= '" + time.Value.Upper.Date + "'";
            if (way != Common.Enums.IntegralWay.All)
            {
                iCondition += " AND IntegralWay = " + (byte)way;
            }
            if (accountID != null)
            {
                iCondition += " AND CompnayId = '" + accountID + "'";
            }
            AddParameter("@iField", fields);
            AddParameter("@iCatelog", catelog);
            AddParameter("@iCondition", iCondition);
            AddParameter("@iOrderBy", orderbyFiled);
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");

            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
            {
                result = new List<IntegralInfoView>();
                while (reader.Read())
                {
                    IntegralInfoView view = new IntegralInfoView();
                    view.CompnayId = reader.GetGuid(0);
                    view.Integral = reader.GetInt32(1);
                    view.IntegralWay = (Common.Enums.IntegralWay)reader.GetByte(2);
                    view.AccessTime = reader.GetDateTime(3);
                    view.Remark = reader.GetString(4);
                    result.Add(view);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        public IEnumerable<DataTransferObject.Integral.IntegralConsumptionView> GetIntegralConsumptionList(Core.Range<DateTime>? time, Common.Enums.IntegralWay? way, Guid? accountId, Common.Enums.ExchangeState state, Common.Enums.OEMCommodityState? oemstate , string falg,Guid? oemId, Pagination pagination)
        {
            List<IntegralConsumptionView> result = null;

            var fields = " [ID] ,[CompnayId] ,[AccountNo] ,[AccountName],[AccountPhone],[DeliveryAddress],[ExpressCompany],[ExpressDelivery],[ConsumptionIntegral],[CommodityId],[CommodityName],[ExchangeTiem],[Exchange],[IntegralWay],[CommodityCount],[Remark],[Reason],[CompanyShortName],OEMCommodityState,OEMName ";
            string iCondition = " ";

            var catelog = "[dbo].[T_IntegralConsumption]";
            var orderbyFiled = "ExchangeTiem desc";
            if (oemstate.HasValue)
            {
                iCondition += " OEMCommodityState = " + (byte)oemstate + " AND ";
            }
            if (oemId.HasValue)
            {
                iCondition += " OEMID = '" + oemId.Value + "' AND ";
            }
            iCondition += " CONVERT(DATE,[ExchangeTiem]) >=  '" + time.Value.Lower.Date + "'";
            iCondition += " AND convert(date,[ExchangeTiem]) <= '" + time.Value.Upper.Date + "'";
            if (falg=="1")
            {
                if (way != Common.Enums.IntegralWay.All)
                {
                    iCondition += " AND (IntegralWay = " + (byte)way + " OR IntegralWay = " + (byte)IntegralWay.ExchangeSms + " )";
                }
            }
            else
            {
                if (way != Common.Enums.IntegralWay.All)
                {
                    iCondition += " AND IntegralWay = " + (byte)way + " ";
                }
            }
            if (accountId != null)
            {
                iCondition += " AND CompnayId = '" + accountId + "'";
            }
            else if (state != Common.Enums.ExchangeState.All)
            {
                iCondition += " AND Exchange = " + (byte)state;
            }
            AddParameter("@iField", fields);
            AddParameter("@iCatelog", catelog);
            AddParameter("@iCondition", iCondition);
            AddParameter("@iOrderBy", orderbyFiled);
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");

            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
            {
                result = new List<IntegralConsumptionView>();
                while (reader.Read())
                {
                    IntegralConsumptionView view = new IntegralConsumptionView();
                    view.Id = reader.GetGuid(0);
                    view.CompnayId = reader.GetGuid(1);
                    view.AccountNo = reader.GetString(2);
                    view.AccountName = reader.GetString(3);
                    view.AccountPhone = reader.GetString(4);
                    view.DeliveryAddress = reader.GetString(5);
                    view.ExpressCompany = reader.GetString(6);
                    view.ExpressDelivery = reader.GetString(7);
                    view.ConsumptionIntegral = reader.GetInt32(8);
                    view.CommodityId = reader.GetGuid(9);
                    view.CommodityName = reader.GetString(10);
                    view.ExchangeTiem = reader.GetDateTime(11);
                    view.Exchange = (ChinaPay.B3B.Common.Enums.ExchangeState)reader.GetByte(12);
                    view.Way = (ChinaPay.B3B.Common.Enums.IntegralWay)reader.GetByte(13);
                    view.CommodityCount = reader.GetInt32(14);
                    view.Remark = reader.GetString(15);
                    view.Reason = reader.GetString(16);
                    view.CompanyShortName = reader.GetString(17);
                    view.OEMCommodityState = (OEMCommodityState)reader.GetByte(18);
                    view.OEMName = reader.GetString(19);
                    result.Add(view);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        public IntegralCount GetIntegralCount(Guid id)
        {
            IntegralCount result = null;
            string sql = @"SELECT [CompnayId] ,[IntegralCount]  ,[IntegralSurplus],[IntegralNotDeduct]
  FROM  [dbo].[T_IntegralCount] WHERE CompnayId='" + id + "'";

            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new IntegralCount();
                    result.CompnayId = reader.GetGuid(0);
                    result.IntegralCounts = reader.GetInt32(1);
                    result.IntegralSurplus = reader.GetInt32(2);
                    result.IntegralNotDeduct = reader.GetInt32(3);
                }
            }
            return result;
        }
        public DataTransferObject.Integral.IntegralInfoView GetIntegral(Guid id)
        {
            IntegralInfoView result = null;
            string sql = @"SELECT [ID] ,[CompnayId]  ,[Integral]  ,[IntegralWay] ,[AccessTime]   ,[Remark]  FROM  [dbo].[T_IntegralInfo] WHERE ID=@ID ";

            AddParameter("ID", id);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new IntegralInfoView();
                    result.CompnayId = reader.GetGuid(0);
                    result.Integral = reader.GetInt32(1);
                    result.IntegralWay = (Common.Enums.IntegralWay)reader.GetByte(2);
                    result.AccessTime = reader.GetDateTime(3);
                    result.Remark = reader.GetString(4);
                }
            }
            return result;
        }
        //public DataTransferObject.Integral.IntegralInfoView GetIntegralByAccountId(Guid CompnayId, Common.Enums.IntegralWay? way)
        //{
        //    IntegralInfoView result = null;
        //    string sql = @"SELECT TOP 1 [ID] ,[CompnayId]  ,[Integral]  ,[IntegralWay] ,[AccessTime]   ,[Remark]  FROM  [dbo].[T_IntegralInfo] WHERE CompnayId=@CompnayId ";
        //    using (var dbOperator = new DbOperator(Provider, ConnectionString))
        //    {
        //        AddParameter("CompnayId", CompnayId);
        //        if (way != null)
        //        {
        //            sql += " AND IntegralWay = @IntegralWay";
        //            AddParameter("IntegralWay", (byte)way);
        //        }
        //        sql += " ORDER BY AccessTime DESC";
        //        using (var reader = ExecuteReader(sql))
        //        {
        //            if (reader.Read())
        //            {
        //                result = new IntegralInfoView();
        //                result.CompnayId = reader.GetGuid(0);
        //                result.Integral = reader.GetInt32(1);
        //                result.IntegralWay = (Common.Enums.IntegralWay)reader.GetByte(2);
        //                result.AccessTime = reader.GetDateTime(3);
        //                result.Remark = reader.GetString(4);
        //            }
        //        }
        //    }
        //    return result;
        //}
        public DataTransferObject.Integral.IntegralParameterView GetIntegralParameter()
        {
            IntegralParameterView result = null;
            string sql = @"SELECT [IsSignIn] ,[SignIntegral],[IsDrop],[ConsumptionIntegral],[AvailabilityRatio],[RangeReset],Multiple,SpecifiedDate,MostBuckle FROM  [dbo].[T_IntegralParameter] ";

            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new IntegralParameterView();
                    result.IsSignIn = reader.GetBoolean(0);
                    result.SignIntegral = reader.GetInt32(1);
                    result.IsDrop = reader.GetBoolean(2);
                    result.ConsumptionIntegral = reader.GetInt32(3);
                    result.AvailabilityRatio = reader.GetDecimal(4);
                    result.RangeReset = (ChinaPay.B3B.Common.Enums.IntegralRangeTime)reader.GetByte(5);
                    result.Multiple = reader.GetDecimal(6);
                    result.SpecifiedDate = reader.GetValue(7) == DBNull.Value ? (Nullable<DateTime>)null : reader.GetDateTime(7);
                    result.MostBuckle = reader.GetInt32(8);
                }

            }
            return result;
        }

        public DataTransferObject.Integral.IntegralConsumptionView GetIntegralConsumption(Guid id)
        {
            IntegralConsumptionView result = null;
            string sql = @"SELECT [ID] ,[CompnayId],[AccountNo] ,[AccountName],[AccountPhone],[DeliveryAddress],[ExpressCompany],[ExpressDelivery],[ConsumptionIntegral],[CommodityId],[CommodityName],[ExchangeTiem],[Exchange],[IntegralWay],[CommodityCount],[Remark],[Reason],[CompanyShortName]  FROM [dbo].[T_IntegralConsumption] WHERE ID = @ID1 ";

            AddParameter("ID1", id);
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    result = new IntegralConsumptionView();
                    result.Id = reader.GetGuid(0);
                    result.CompnayId = reader.GetGuid(1);
                    result.AccountNo = reader.GetString(2);
                    result.AccountName = reader.GetString(3);
                    result.AccountPhone = reader.GetString(4);
                    result.DeliveryAddress = reader.GetString(5);
                    result.ExpressCompany = reader.GetString(6);
                    result.ExpressDelivery = reader.GetString(7);
                    result.ConsumptionIntegral = reader.GetInt32(8);
                    result.CommodityId = reader.GetGuid(9);
                    result.CommodityName = reader.GetString(10);
                    result.ExchangeTiem = reader.GetDateTime(11);
                    result.Exchange = (ChinaPay.B3B.Common.Enums.ExchangeState)reader.GetByte(12);
                    result.Way = (ChinaPay.B3B.Common.Enums.IntegralWay)reader.GetByte(13);
                    result.CommodityCount = reader.GetInt32(14);
                    result.Remark = reader.GetString(15);
                    result.Reason = reader.GetString(16);
                    result.CompanyShortName = reader.GetString(17);
                }
            }
            return result;
        }

        public void UpdateIntegralInfo(DataTransferObject.Integral.IntegralInfoView view)
        {
            throw new NotImplementedException();
        }

        public void InsertIntegralInfo(ChinaPay.B3B.Service.Integral.Domain.IntegralInfo view)
        {
            string sql = @"INSERT INTO [dbo].[T_IntegralInfo]  ([ID]  ,[CompnayId]  ,[Integral] ,[IntegralWay]  ,[AccessTime]  ,[Remark]) VALUES (  @ID2, @CompnayId ,@Integral  ,@IntegralWay   ,getdate()  , @Remark)";

            AddParameter("ID2", view.ID);
            AddParameter("CompnayId", view.CompnayId);
            AddParameter("Integral", view.Integral);
            AddParameter("IntegralWay", (byte)view.IntegralWay);
            AddParameter("Remark", view.Remark);
            ExecuteNonQuery(sql);
        }

        public void DeleteIntegralInfo(DataTransferObject.Integral.IntegralInfoView view)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改兑换状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.ExchangeState State, string no, string company, string address, string reason)
        {
            string sql = @"UPDATE [dbo].[T_IntegralConsumption] SET Exchange=@Exchange,[DeliveryAddress]=@DeliveryAddress,[ExpressCompany]=@ExpressCompany,[ExpressDelivery]=@ExpressDelivery,[Reason]=@Reason WHERE ID = @ID";

            AddParameter("Exchange", (byte)State);
            AddParameter("DeliveryAddress", address);
            AddParameter("ExpressCompany", company);
            AddParameter("ExpressDelivery", no);
            AddParameter("Reason", reason);
            AddParameter("ID", id);
            ExecuteNonQuery(sql);

        }

        /// <summary>
        /// OEM修改提交状态
        /// </summary>
        /// <returns></returns>
        public void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.OEMCommodityState State)
        {
            string sql = @"UPDATE [dbo].[T_IntegralConsumption] SET OEMCommodityState=@OEMCommodityState WHERE ID = @ID";
            AddParameter("OEMCommodityState", (byte)State);
            AddParameter("ID", id);
            ExecuteNonQuery(sql);
        }
        public void InsertIntegralConsumption(ChinaPay.B3B.Service.Integral.Domain.IntegralConsumption view)
        {
            string sql = @"INSERT INTO [dbo].[T_IntegralConsumption]
                           ([ID] ,[CompnayId] ,[CompanyShortName],[AccountNo],[AccountName],[AccountPhone],[DeliveryAddress],[ExpressCompany],[ExpressDelivery],[ConsumptionIntegral],[CommodityId],[CommodityName],[ExchangeTiem],[Exchange],[IntegralWay],[CommodityCount],[Remark],[Reason],OEMCommodityState,OEMName,OEMID)
                            VALUES(@ID,@CompnayId,@CompanyShortName,@AccountNo,@AccountName,@AccountPhone,@DeliveryAddress,@ExpressCompany,@ExpressDelivery,@ConsumptionIntegral,@CommodityId,@CommodityName,@ExchangeTiem,@Exchange,@IntegralWay,@CommodityCount,@Remark,@Reason,@OEMCommodityState,@OEMName,@OEMID)";

            AddParameter("ID", view.Id);
            AddParameter("CompnayId", view.CompnayId);
            AddParameter("CompanyShortName", view.CompanyShortName);
            AddParameter("AccountNo", view.AccountNo);
            AddParameter("AccountName", view.AccountName);
            AddParameter("AccountPhone", view.AccountPhone);
            AddParameter("DeliveryAddress", view.DeliveryAddress);
            AddParameter("ExpressCompany", view.ExpressCompany);
            AddParameter("ExpressDelivery", view.ExpressDelivery);
            AddParameter("ConsumptionIntegral", view.ConsumptionIntegral);
            AddParameter("CommodityId", view.CommodityId);
            AddParameter("CommodityName", view.CommodityName);
            AddParameter("ExchangeTiem", view.ExchangeTiem);
            AddParameter("Exchange", (byte)view.Exchange);
            AddParameter("IntegralWay", view.Way);
            AddParameter("CommodityCount", view.CommodityCount);
            AddParameter("Remark", view.Remark);
            AddParameter("Reason", view.Reason);
            AddParameter("OEMCommodityState", (byte)view.OEMCommodityState);
            if ( view.OEMID.HasValue)
            {
                AddParameter("OEMID", view.OEMID);
                AddParameter("OEMName", view.OEMName);
            }
            else
            {
                AddParameter("OEMID", DBNull.Value);
                AddParameter("OEMName","");
            }
            ExecuteNonQuery(sql);
        }

        public void DeleteIntegralConsumption(DataTransferObject.Integral.IntegralConsumptionView view)
        {
            throw new NotImplementedException();
        }

        public void UpdateIntegralParameter(DataTransferObject.Integral.IntegralParameterView view)
        {
            string sql = @" UPDATE [dbo].[T_IntegralParameter]  SET [IsSignIn] = @IsSignIn  ,[SignIntegral] = @SignIntegral  ,[IsDrop] =@IsDrop ,  [ConsumptionIntegral] =  @ConsumptionIntegral ,[AvailabilityRatio] = @AvailabilityRatio ,Multiple=@Multiple ,[RangeReset] = @RangeReset, SpecifiedDate=@SpecifiedDate,MostBuckle=@MostBuckle ";

            AddParameter("IsSignIn", view.IsSignIn);
            AddParameter("SignIntegral", view.SignIntegral);
            AddParameter("IsDrop", view.IsDrop);
            AddParameter("ConsumptionIntegral", view.ConsumptionIntegral);
            AddParameter("AvailabilityRatio", view.AvailabilityRatio);
            AddParameter("Multiple", view.Multiple);
            AddParameter("RangeReset", view.RangeReset);
            AddParameter("MostBuckle", view.MostBuckle);
            if (view.SpecifiedDate.HasValue)
            {
                AddParameter("SpecifiedDate", view.SpecifiedDate.Value);
            }
            else
            {
                AddParameter("SpecifiedDate", DBNull.Value);
            }
            ExecuteNonQuery(sql);

        }



        public void InsertIntegralCount(IntegralCount info)
        {
            string sql = @"INSERT INTO T_IntegralCount([CompnayId],[IntegralCount] , [IntegralSurplus],[IntegralNotDeduct]) VALUES('" + info.CompnayId + "'," + info.Integral + "," + (info.IsNotDeduct?0:info.Integral) + ","+(info.IsNotDeduct?info.Integral:0)+")";
            ExecuteNonQuery(sql);
        }

        public void UpdateIntegralCount(IntegralCount info)
        {
            string sql = @"UPDATE T_IntegralCount SET IntegralCount = IntegralCount+" + info.Integral + " ,IntegralSurplus=IntegralSurplus+" + (info.IsNotDeduct ? 0 : info.Integral) + ",IntegralNotDeduct=IntegralNotDeduct+" + (info.IsNotDeduct ? info.Integral : 0) + " WHERE CompnayId ='" + info.CompnayId + "'";
            ExecuteNonQuery(sql);

        }


        public void InsertIntegralConsumption(IntegralConsumption view, int days, int integral, int integralSurplus)
        {
            int integralCount = 0;
            DateTime tiem = view.ExchangeTiem;
            string sql = "";
            for (var i = 0; i < days; i++)
            {
                integralCount += integral;
                view.ConsumptionIntegral = integralCount > integralSurplus ? integralCount - integralSurplus : integral;
                view.ExchangeTiem = tiem.AddDays(i + 1).Date;

                sql += @"INSERT INTO [dbo].[T_IntegralConsumption]
                           ([ID] ,[CompnayId] ,[CompanyShortName] ,[AccountNo],[AccountName],[AccountPhone],[DeliveryAddress],[ExpressCompany],[ExpressDelivery],[ConsumptionIntegral],[CommodityId],[CommodityName],[ExchangeTiem],[Exchange],[IntegralWay],[CommodityCount],[Remark],[Reason])
                            VALUES('" + Guid.NewGuid() + "','" + view.CompnayId + "','" + view.CompanyShortName + "','" + view.AccountNo + "','" + view.AccountName + "','" + view.AccountPhone + "','" + view.DeliveryAddress + "','" + view.ExpressCompany + "','" + view.ExpressDelivery + "','" + view.ConsumptionIntegral + "','" + view.CommodityId + "','" + view.CommodityName + "','" + view.ExchangeTiem + "'," + (byte)Common.Enums.ExchangeState.Processing + "," + (byte)Common.Enums.IntegralWay.NotSignIn + "," + view.CommodityCount + ",'" + view.Remark + "','" + view.Reason + "');";

                if (integralCount == integralSurplus)
                {
                    break;
                }
            }
            ExecuteNonQuery(sql);

        }


        public void DeleteIntegralCount()
        {
            string sql = @"DELETE FROM T_IntegralCount  ";
            ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 将剩余数量加回去
        /// </summary> 
        public void UpdateShelvesNum(Guid id, int StockNumber)
        {
            string sql = @"UPDATE [dbo].[T_Commodity] SET  [ExchangeNumber] = ExchangeNumber-@StockNumber   WHERE ID=@ID3 ";

            AddParameter("StockNumber", StockNumber);
            AddParameter("ID3", id);
            ExecuteNonQuery(sql);

        }


        public void UpdateIntegralCountByConsumption(IntegralCount orginalInfo, IntegralCount info)
        {
            bool isUseSurplus = orginalInfo.IntegralSurplus - Math.Abs(info.Integral) > 0;
            string sql = @"UPDATE T_IntegralCount SET IntegralCount = IntegralCount+" + info.Integral + " ,IntegralSurplus=IntegralSurplus+" + (isUseSurplus ? info.Integral : (0 - orginalInfo.IntegralSurplus)) + ",IntegralNotDeduct=IntegralNotDeduct+" + (isUseSurplus ? 0 : (info.Integral+orginalInfo.IntegralSurplus)) + " WHERE CompnayId ='" + info.CompnayId + "'";
            ExecuteNonQuery(sql);
        }
    }
}
