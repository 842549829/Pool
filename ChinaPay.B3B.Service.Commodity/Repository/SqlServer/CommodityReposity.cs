using System;
using ChinaPay.Core; 

namespace ChinaPay.B3B.Service.Commodity.Repository.SqlServer
{
    using System.Collections.Generic;
    using DataAccess;
    using DataTransferObject.Commodity;
    using ChinaPay.Repository;
    using ChinaPay.B3B.Common.Enums;
    class CommodityReposity : SqlServerTransaction, ICommodityReposity
    {
        public CommodityReposity(DbOperator command)
            : base(command) { }
        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <param name="falg">是true的时候就是查询展示商品</param>
        /// <returns></returns>
        public IEnumerable<CommodityView> GetCommodityList(bool falg, Pagination pagination)
        {
            List<CommodityView> result = null;

            var fields = " ROW_NUMBER() OVER (order by [ValidityTime]) as rownums,[ID] ,[CommodityName] ,[CoverImgUrl]  ,[NeedIntegral] ,[StockNumber]  ,[ExchangeNumber] ,[State] ,[ValidityTime],([StockNumber] - [ExchangeNumber]) as b ,Remark,ExchangSmsNumber,Type";
            string iCondition = " [State] = 1 AND (StockNumber- ExchangeNumber) > 0 AND ValidityTime>=CONVERT(DATE,GETDATE()) ";

            var catelog = "[dbo].[T_Commodity]";
            AddParameter("@iField", fields);
            AddParameter("@iCatelog", catelog);
            if (falg)
            {
                AddParameter("@iCondition", iCondition);
                AddParameter("@iOrderBy", "SortNum");
            }
            else
            {
                AddParameter("@iCondition", "");
                AddParameter("@iOrderBy", "ValidityTime");
            }
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
            {
                result = new List<CommodityView>();
                while (reader.Read())
                {
                    CommodityView view = new CommodityView();
                    view.Num = reader.GetValue(0);
                    view.ID = reader.GetGuid(1);
                    view.CommodityName = reader.GetString(2);
                    view.CoverImgUrl = reader.GetString(3);
                    view.NeedIntegral = reader.GetInt32(4);
                    view.StockNumber = reader.GetInt32(5);
                    view.ExchangeNumber = reader.GetInt32(6);
                    view.State = reader.GetBoolean(7);
                    view.ValidityTime = reader.GetDateTime(8);
                    view.SurplusNumber = reader.GetInt32(9);
                    view.Remark = reader.GetString(10);
                    view.ExchangSmsNumber = reader.GetInt32(11);
                    view.Type = (CommodityType)reader.GetByte(12);
                    result.Add(view);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTransferObject.Commodity.CommodityView GetCommodity(Guid id)
        {
            CommodityView result = null;
            string sql = @"SELECT [ID] ,[CommodityName] ,[CoverImgUrl]  ,[NeedIntegral] ,[StockNumber]  ,[ExchangeNumber]  ,[State]   ,[ValidityTime],([StockNumber] - [ExchangeNumber]),Remark,SortNum,ExchangSmsNumber,Type FROM  [dbo].[T_Commodity] WHERE ID=@ID ";

            AddParameter("ID", id);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new CommodityView();
                    result.ID = reader.GetGuid(0);
                    result.CommodityName = reader.GetString(1);
                    result.CoverImgUrl = reader.GetString(2);
                    result.NeedIntegral = reader.GetInt32(3);
                    result.StockNumber = reader.GetInt32(4);
                    result.ExchangeNumber = reader.GetInt32(5);
                    result.State = reader.GetBoolean(6);
                    result.ValidityTime = reader.GetDateTime(7);
                    result.SurplusNumber = reader.GetInt32(8);
                    result.Remark = reader.GetString(9);
                    result.SortNum = reader.GetInt32(10);
                    result.ExchangSmsNumber = reader.GetInt32(11);
                    result.Type = (CommodityType)reader.GetByte(12);
                }
            }
            return result;
        }

        public void Update(DataTransferObject.Commodity.CommodityView view)
        {
            string sql = @"UPDATE [dbo].[T_Commodity] SET [CommodityName] =@CommodityName1,[CoverImgUrl]=@CoverImgUrl1  ,[NeedIntegral] =@NeedIntegral1,[StockNumber] =@StockNumber1 ,[ExchangeNumber] =@ExchangeNumber1 ,[State] =@State1 ,[ValidityTime]=@ValidityTime1,Remark=@Remark1,SortNum=@SortNum1,ExchangSmsNumber=@ExchangSmsNumber1,Type =@Type1 WHERE ID=@ID1 ";

            AddParameter("CommodityName1", view.CommodityName);
            AddParameter("CoverImgUrl1", view.CoverImgUrl);
            AddParameter("NeedIntegral1", view.NeedIntegral);
            AddParameter("StockNumber1", view.StockNumber);
            AddParameter("ExchangeNumber1", view.ExchangeNumber);
            AddParameter("State1", view.State);
            AddParameter("ValidityTime1", view.ValidityTime);
            AddParameter("Remark1", view.Remark);
            AddParameter("SortNum1", view.SortNum);
            AddParameter("ID1", view.ID);
            AddParameter("ExchangSmsNumber1", view.ExchangSmsNumber);
            AddParameter("Type1", view.Type);
            ExecuteNonQuery(sql);
        }

        public void Insert(CommodityView view)
        {
            string sql = @"INSERT INTO  [dbo].[T_Commodity] ([ID] ,[CommodityName] ,[CoverImgUrl] ,[NeedIntegral] ,[StockNumber] ,[ExchangeNumber] ,[State],[ValidityTime],Remark,SortNum,ExchangSmsNumber,Type) VALUES(@ID2, @CommodityName2, @CoverImgUrl2, @NeedIntegral2, @StockNumber2, @ExchangeNumber2, @State2, @ValidityTime2,@Remark2,@SortNum2,@ExchangSmsNumber2,@Type2)";

            AddParameter("ID2", view.ID);
            AddParameter("CommodityName2", view.CommodityName);
            AddParameter("CoverImgUrl2", view.CoverImgUrl);
            AddParameter("NeedIntegral2", view.NeedIntegral);
            AddParameter("StockNumber2", view.StockNumber);
            AddParameter("ExchangeNumber2", view.ExchangeNumber);
            AddParameter("State2", view.State);
            AddParameter("ValidityTime2", view.ValidityTime);
            AddParameter("Remark2", view.Remark);
            AddParameter("SortNum2", view.SortNum);
            AddParameter("ExchangSmsNumber2", view.ExchangSmsNumber);
            AddParameter("Type2", view.Type);
            ExecuteNonQuery(sql);

        }

        public void Delete(CommodityView view)
        {
            string sql = @"DELETE FROM [dbo].[T_Commodity] WHERE ID = @ID";

            AddParameter("ID", view.ID);
            ExecuteNonQuery(sql);

        }
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public void UpdateState(Guid id, bool State)
        {
            string sql = @"UPDATE [dbo].[T_Commodity] SET  [State] =@State  WHERE ID=@ID ";

            AddParameter("State", State);
            AddParameter("ID", id);
            ExecuteNonQuery(sql);

        }

        /// <summary>
        /// 修改上下架数量
        /// </summary> 
        public void UpdateShelvesNum(Guid id, int StockNumber)
        {
            string sql = @"UPDATE [dbo].[T_Commodity] SET  [StockNumber] = StockNumber+@StockNumber   WHERE ID=@ID ";

            AddParameter("StockNumber", StockNumber);
            AddParameter("ID", id);
            ExecuteNonQuery(sql);

        }

        /// <summary>
        /// 更新用户购买数量到数据库
        /// </summary> 
        public void UpdateBuyNum(Guid id, int Number)
        {
            string sql = @"UPDATE [dbo].[T_Commodity] SET [ExchangeNumber] = ExchangeNumber + @Number WHERE ID=@ID ";

            AddParameter("Number", Number);
            AddParameter("ID", id);
            ExecuteNonQuery(sql);

        }



    }
}
