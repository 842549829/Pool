using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    internal static class PolicySpliter {
        public static IEnumerable<NormalPolicyInfo> Execute(NormalPolicyInfo policy) {
            if(policy.VoyageType == VoyageType.OneWayOrRound) {
                var owPolicy = policy.Copy() as NormalPolicyInfo;
                owPolicy.VoyageType = VoyageType.OneWay;
                var rtPolicy = policy.Copy() as NormalPolicyInfo;
                rtPolicy.VoyageType = VoyageType.RoundTrip;
                return new[] { owPolicy, rtPolicy };
            } else {
                return new[] { policy };
            }
        }
    }
}