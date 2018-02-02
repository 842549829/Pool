using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.AirlineConfig.Domain;
using ChinaPay.B3B.Service.AirlineConfig.Repository;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.AirlineConfig
{
    public class OEMAirlineConfigService
    {
        public static OEMAirlineConfig QueryConfig(Guid? oemId)
        {
            IAirlineConfigRepository repository = Factory.CreateAirlineConfigRepository();
            return repository.QueryConfig(oemId);
        }

        public static bool SaveConfig(Guid oemId, Dictionary<ConfigUseType, Tuple<string, string>> config, string opAccount)
        {
            bool success = false;
            IAirlineConfigRepository repository = Factory.CreateAirlineConfigRepository();
            success = repository.SaveConfig(oemId, config);
            LogService.SaveOperationLog(new OperationLog(OperationModule.OEM信息设置, OperationType.Update, opAccount, OperatorRole.Platform, "修改OEM指令配置", config.Aggregate(string.Empty, (result, pair) =>
                                                                                                                                                                                      string.Format("{0}配置用途：{1},用户名{2};OfficeNO:{3} ", result, EnumExtension.GetDescription(pair.Key), pair.Value.Item1, pair.Value.Item2))
                                                                                                                                                       +"修改"+(success?"是":"否")));
            return success;
        }
    }
}