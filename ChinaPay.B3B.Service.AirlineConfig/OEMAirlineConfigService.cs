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
            LogService.SaveOperationLog(new OperationLog(OperationModule.OEM��Ϣ����, OperationType.Update, opAccount, OperatorRole.Platform, "�޸�OEMָ������", config.Aggregate(string.Empty, (result, pair) =>
                                                                                                                                                                                      string.Format("{0}������;��{1},�û���{2};OfficeNO:{3} ", result, EnumExtension.GetDescription(pair.Key), pair.Value.Item1, pair.Value.Item2))
                                                                                                                                                       +"�޸�"+(success?"��":"��")));
            return success;
        }
    }
}