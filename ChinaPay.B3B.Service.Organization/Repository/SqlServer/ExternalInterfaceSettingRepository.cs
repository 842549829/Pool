using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class ExternalInterfaceSettingRepository : SqlServerRepository, IExternalInterfaceSettingRepository
    {
        public ExternalInterfaceSettingRepository(string connectionString)
            : base(connectionString)
        {
        }

        public int Save(Domain.ExternalInterfaceSetting setting)
        {
            string sql = @"IF EXISTS (SELECT NULL FROM dbo.T_ExternalInterfaceSetting WHERE CompanyId =@CompanyId)
                           UPDATE [dbo].[T_ExternalInterfaceSetting] SET [CompanyId] = @CompanyId,[SecurityCode] = @SecurityCode,
                           [ConfirmSuccessAddress] = @ConfirmSuccessAddress,[ConfirmFailAddress] = @ConfirmFailAddress,
                           [PaySuccessAddress] = @PaySuccessAddress,[DrawSuccessAddress] = @DrawSuccessAddress,
                           [RefuseAddress] = @RefuseAddress,AgreedAddress=@AgreedAddress,RefundSuccessAddress=@RefundSuccessAddress,
                           ReturnTicketSuccessAddress=@ReturnTicketSuccessAddress,ReschedulingAddress=@ReschedulingAddress,
                           RefuseTicketAddress=@RefuseTicketAddress,ReschPaymentAddress=@ReschPaymentAddress,RefuseChangeAddress=@RefuseChangeAddress,
                           CanceldulingAddress=@CanceldulingAddress,InterfaceInvokeMethod=@InterfaceInvokeMethod,RefundApplySuccessAddress=@RefundApplySuccessAddress,
                           BindIP=@BindIP,PolicyTypes=@PolicyTypes
                           WHERE [CompanyId]=@CompanyId
                           ELSE 
                           INSERT INTO [dbo].[T_ExternalInterfaceSetting]
                          ([CompanyId],[SecurityCode],[ConfirmSuccessAddress],[ConfirmFailAddress],[PaySuccessAddress],[DrawSuccessAddress],
                            [RefuseAddress],AgreedAddress,RefundSuccessAddress,ReturnTicketSuccessAddress,ReschedulingAddress,RefuseTicketAddress,
                             ReschPaymentAddress,RefuseChangeAddress,RefundApplySuccessAddress,[OpenTime],CanceldulingAddress,InterfaceInvokeMethod,BindIP,PolicyTypes)
                          VALUES (@CompanyId,@SecurityCode,@ConfirmSuccessAddress,@ConfirmFailAddress,@PaySuccessAddress,@DrawSuccessAddress,
                          @RefuseAddress,@AgreedAddress,@RefundSuccessAddress,@ReturnTicketSuccessAddress,@ReschedulingAddress,@RefuseTicketAddress,
                          @ReschPaymentAddress,@RefuseChangeAddress,@RefundApplySuccessAddress,@OpenTime,@CanceldulingAddress,@InterfaceInvokeMethod,@BindIP,@PolicyTypes)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", setting.CompanyId);
                dbOperator.AddParameter("SecurityCode", setting.SecurityCode);
                if (!string.IsNullOrWhiteSpace(setting.ConfirmSuccessAddress))
                {
                    dbOperator.AddParameter("ConfirmSuccessAddress", setting.ConfirmSuccessAddress);
                }
                else
                {
                    dbOperator.AddParameter("ConfirmSuccessAddress", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(setting.ConfirmFailAddress))
                {
                    dbOperator.AddParameter("ConfirmFailAddress", setting.ConfirmFailAddress);
                }
                else
                {
                    dbOperator.AddParameter("ConfirmFailAddress", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(setting.PaySuccessAddress))
                {
                    dbOperator.AddParameter("PaySuccessAddress", setting.PaySuccessAddress);
                }
                else
                {
                    dbOperator.AddParameter("PaySuccessAddress", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(setting.DrawSuccessAddress))
                {
                    dbOperator.AddParameter("DrawSuccessAddress", setting.DrawSuccessAddress);
                }
                else
                {
                    dbOperator.AddParameter("DrawSuccessAddress", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(setting.RefuseAddress))
                {
                    dbOperator.AddParameter("RefuseAddress", setting.RefuseAddress);
                }
                else
                {
                    dbOperator.AddParameter("RefuseAddress", DBNull.Value);
                }

                if (!string.IsNullOrWhiteSpace(setting.AgreedAddress))
                    dbOperator.AddParameter("AgreedAddress", setting.AgreedAddress);
                else
                    dbOperator.AddParameter("AgreedAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.RefundSuccessAddress))
                    dbOperator.AddParameter("RefundSuccessAddress", setting.RefundSuccessAddress);
                else
                    dbOperator.AddParameter("RefundSuccessAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.ReturnTicketSuccessAddress))
                    dbOperator.AddParameter("ReturnTicketSuccessAddress", setting.ReturnTicketSuccessAddress);
                else
                    dbOperator.AddParameter("ReturnTicketSuccessAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.ReschedulingAddress))
                    dbOperator.AddParameter("ReschedulingAddress", setting.ReschedulingAddress);
                else
                    dbOperator.AddParameter("ReschedulingAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.RefuseTicketAddress))
                    dbOperator.AddParameter("RefuseTicketAddress", setting.RefuseTicketAddress);
                else
                    dbOperator.AddParameter("RefuseTicketAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.ReschPaymentAddress))
                    dbOperator.AddParameter("ReschPaymentAddress", setting.ReschPaymentAddress);
                else
                    dbOperator.AddParameter("ReschPaymentAddress", DBNull.Value);

                if (!string.IsNullOrWhiteSpace(setting.RefuseChangeAddress))
                    dbOperator.AddParameter("RefuseChangeAddress", setting.RefuseChangeAddress);
                else
                    dbOperator.AddParameter("RefuseChangeAddress", DBNull.Value);
                if (!string.IsNullOrWhiteSpace(setting.CanceldulingAddress))
                    dbOperator.AddParameter("CanceldulingAddress", setting.CanceldulingAddress);
                else
                    dbOperator.AddParameter("CanceldulingAddress", DBNull.Value);
                if (!string.IsNullOrWhiteSpace(setting.RefundApplySuccessAddress))
                    dbOperator.AddParameter("RefundApplySuccessAddress", setting.RefundApplySuccessAddress);
                else
                    dbOperator.AddParameter("RefundApplySuccessAddress", DBNull.Value);
                dbOperator.AddParameter("InterfaceInvokeMethod", setting.InterfaceInvokeMethod.Join(","));

                if (string.IsNullOrWhiteSpace(setting.BindIP))
                    dbOperator.AddParameter("BindIP", string.Empty);
                else
                    dbOperator.AddParameter("BindIP", setting.BindIP);
                dbOperator.AddParameter("PolicyTypes", (int)setting.PolicyTypes);
                dbOperator.AddParameter("OpenTime", setting.OpenTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.ExternalInterfaceSetting Query(Guid companyId)
        {
            Domain.ExternalInterfaceSetting externalSetting = null;
            string sql = @"SELECT SecurityCode,ConfirmSuccessAddress,ConfirmFailAddress,PaySuccessAddress,DrawSuccessAddress,RefuseAddress,
                             [OpenTime],AgreedAddress,RefundSuccessAddress,ReturnTicketSuccessAddress,ReschedulingAddress,RefuseTicketAddress,ReschPaymentAddress,
                             RefuseChangeAddress,CanceldulingAddress,InterfaceInvokeMethod,BindIP,RefundApplySuccessAddress,PolicyTypes
                          FROM dbo.T_ExternalInterfaceSetting WHERE CompanyId=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        externalSetting = new Domain.ExternalInterfaceSetting(companyId);
                        externalSetting.SecurityCode = reader.GetString(0);
                        externalSetting.ConfirmSuccessAddress = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        externalSetting.ConfirmFailAddress = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        externalSetting.PaySuccessAddress = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        externalSetting.DrawSuccessAddress = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        externalSetting.RefuseAddress = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        externalSetting.OpenTime = reader.GetDateTime(6);
                        externalSetting.AgreedAddress = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        externalSetting.RefundSuccessAddress = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        externalSetting.ReturnTicketSuccessAddress = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        externalSetting.ReschedulingAddress = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        externalSetting.RefuseTicketAddress = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        externalSetting.ReschPaymentAddress = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        externalSetting.RefuseChangeAddress = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                        externalSetting.CanceldulingAddress = reader.IsDBNull(14) ? string.Empty : reader.GetString(14);
                        if (!reader.IsDBNull(15))
                        {
                            externalSetting.InterfaceInvokeMethod = reader.GetString(15).Split(',');
                        }
                        externalSetting.BindIP = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        externalSetting.RefundApplySuccessAddress = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
                        externalSetting.PolicyTypes =(PolicyType)reader.GetInt32(18);
                    }
                }
            }
            return externalSetting;
        }

        public IEnumerable<DataTransferObject.Organization.ExternalInterfaceInfo> Query(DataTransferObject.Organization.ExternalInterfaceQueryCondition condition, Pagination pagination)
        {
            var result = new List<DataTransferObject.Organization.ExternalInterfaceInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                    dbOperator.AddParameter("i_AbbreviateName", condition.AbbreviateName);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                if (condition.IsOpenExternalInterface.HasValue)
                    dbOperator.AddParameter("i_IsOpenExternalInterface", condition.IsOpenExternalInterface);
                if (condition.OpenTimeStart.HasValue)
                    dbOperator.AddParameter("i_OpenTimeStart", condition.OpenTimeStart);
                if (condition.OpenTimeEnd.HasValue)
                    dbOperator.AddParameter("i_OpenTimeEnd", condition.OpenTimeEnd);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                var totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("P_ExternalInterfacePagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var externalInterfaceInfo = new DataTransferObject.Organization.ExternalInterfaceInfo();
                        externalInterfaceInfo.CompanyId = reader.GetGuid(0);
                        externalInterfaceInfo.UserNo = reader.GetString(1);
                        externalInterfaceInfo.AbbreviateName = reader.GetString(2);
                        externalInterfaceInfo.CompanyType = (CompanyType)reader.GetByte(3);
                        externalInterfaceInfo.AccountType = (AccountBaseType)reader.GetByte(4);
                        if (!reader.IsDBNull(5))
                            externalInterfaceInfo.OpenTime = reader.GetDateTime(5);
                        externalInterfaceInfo.IsOpenExternalInterface = reader.GetBoolean(6);
                        result.Add(externalInterfaceInfo);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
    }
}
