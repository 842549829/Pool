using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using System.Linq;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    interface BunkFilter {
        /// <summary>
        /// 处理单个舱位类型
        /// </summary>
        bool Execute(Foundation.Domain.Bunk bunk);
    }
    class OWBunkFilter : BunkFilter {
        private static OWBunkFilter _instance = null;
        public static OWBunkFilter Instance {
            get {
                if(_instance == null) _instance = new OWBunkFilter();
                return _instance;
            }
        }
        private OWBunkFilter() {
        }

        public bool Execute(Foundation.Domain.Bunk bunk) {
            return (bunk.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay
                && (bunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult
                && (bunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual
                && (bunk is Foundation.Domain.GeneralBunk || bunk is Foundation.Domain.PromotionBunk || bunk is Foundation.Domain.FreeBunk);
        }
    }

    abstract class RTBunkFilter : BunkFilter {
        public bool Execute(Foundation.Domain.Bunk bunk) {
            return (bunk.VoyageType & VoyageTypeValue.RoundTrip) == VoyageTypeValue.RoundTrip
                && (bunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult
                && (bunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual
                && ExecuteCore(bunk);
        }
        public virtual bool ExecuteCore(Foundation.Domain.Bunk bunk) {
            return bunk is Foundation.Domain.GeneralBunk || 
                bunk is Foundation.Domain.ProductionBunk  ||
                bunk is Foundation.Domain.PromotionBunk;
        }
    }
    class RTFirstTripBunkFilter : RTBunkFilter {
        private static RTFirstTripBunkFilter _instance = null;
        public static RTFirstTripBunkFilter Instance {
            get {
                if(_instance == null) _instance = new RTFirstTripBunkFilter();
                return _instance;
            }
        }
        private RTFirstTripBunkFilter() {
        }
    }
    class RTSecondTripBunkFilter : RTBunkFilter {
        private PolicyType m_policyType;
        private Bunk m_departureBunk;

        public RTSecondTripBunkFilter(PolicyType policyType, Bunk departureBunk) {
            m_policyType = policyType;
            m_departureBunk = departureBunk;
        }

        public override bool ExecuteCore(Foundation.Domain.Bunk bunk) {
            if(m_departureBunk is GeneralBunk &&
                (m_policyType == PolicyType.NormalDefault) || m_policyType == PolicyType.OwnerDefault || m_policyType == PolicyType.Normal) {
                return bunk is Foundation.Domain.GeneralBunk;
            }
            if(m_departureBunk is ProductionBunk &&
                (m_policyType == PolicyType.BargainDefault || m_policyType == PolicyType.OwnerDefault || m_policyType == PolicyType.Bargain)) {
                return bunk is Foundation.Domain.ProductionBunk && bunk.Code.Value == m_departureBunk.Code;
            }
            if (m_departureBunk is PromotionBunk&&
                m_policyType== PolicyType.Bargain ||m_policyType==PolicyType.BargainDefault)
            {
                return bunk is Foundation.Domain.PromotionBunk;
            }
            return false;
        }
    }
}