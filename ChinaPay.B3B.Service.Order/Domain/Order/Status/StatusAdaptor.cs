using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain {
    internal abstract class StatusAdapter<TSystemStatus> {
        Dictionary<TSystemStatus, StatusInfo<TSystemStatus>> _collection = null;

        public Dictionary<TSystemStatus, StatusInfo<TSystemStatus>> Statuses {
            get {
                if(_collection == null) {
                    var statusInfos = GetStatusInfos();
                    if(statusInfos != null) {
                        _collection = statusInfos.ToDictionary(item => item.System);
                    } else {
                        _collection = new Dictionary<TSystemStatus, StatusInfo<TSystemStatus>>();
                    }
                }
                return _collection;
            }
        }

        public StatusInfo<TSystemStatus> GetStatusInfo(TSystemStatus systemStatus) {
            StatusInfo<TSystemStatus> result = null;
            this.Statuses.TryGetValue(systemStatus, out result);
            return result;
        }

        public string GetStatus(TSystemStatus systemStatus, OrderRole role) {
            var statusInfo = GetStatusInfo(systemStatus);
            return getRoleStatus(statusInfo, role);
        }

        public Dictionary<TSystemStatus, string> GetRoleStatus(OrderRole role) {
            var result = new Dictionary<TSystemStatus, string>();
            foreach(var item in this.Statuses) {
                var status = getRoleStatus(item.Value, role);
                if(!string.IsNullOrWhiteSpace(status))
                    result.Add(item.Key, status);
            }
            return result;
        }

        private string getRoleStatus(StatusInfo<TSystemStatus> statusInfo, OrderRole role) {
            if(statusInfo != null) {
                switch(role) {
                    case OrderRole.Platform:
                        return statusInfo.Platform;
                    case OrderRole.Purchaser:
                        return statusInfo.Purchaser;
                    case OrderRole.Provider:
                        return statusInfo.Provider;
                    case OrderRole.Supplier:
                        return statusInfo.Supplier;
                    case OrderRole.OEMOwner:
                        return statusInfo.DistributionOEM;
                    default:
                        break;
                }
            }
            return string.Empty;
        }

        protected abstract IEnumerable<StatusInfo<TSystemStatus>> GetStatusInfos();
    }
}