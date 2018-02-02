using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Remind.Model;

namespace ChinaPay.B3B.Remind.Server.LogonModel {
    class Company {
        Dictionary<Guid, User> m_members = null;

        public Company(Guid id) {
            Id = id;
            m_members = new Dictionary<Guid, User>();
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Common.Enums.CompanyType Type { get; set; }

        public IEnumerable<User> Members {
            get {
                return m_members.Values;
            }
        }
        public int MemberCount {
            get {
                return m_members.Count;
            }
        }
        public bool IsEmpty {
            get {
                return MemberCount == 0;
            }
        }

        public IEnumerable<RemindStatus> Statuses {
            get {
                var m_statuses = new List<RemindStatus>();
                m_statuses.Add(RemindStatus.OrderedForPay);
                m_statuses.Add(RemindStatus.AgreedForPostponeFee);
                switch(this.Type) {
                    case Common.Enums.CompanyType.Provider:
                        m_statuses.Add(RemindStatus.AppliedForConfirm);
                        m_statuses.Add(RemindStatus.PaidForSupply);
                        m_statuses.Add(RemindStatus.PaidForETDZ);
                        m_statuses.Add(RemindStatus.AppliedForRefund);
                        m_statuses.Add(RemindStatus.AppliedForScrap);
                        m_statuses.Add(RemindStatus.AgreedForReturnMoney);
                        break;
                    case Common.Enums.CompanyType.Supplier:
                        m_statuses.Add(RemindStatus.AppliedForConfirm);
                        m_statuses.Add(RemindStatus.PaidForSupply);
                        break;
                }
                return m_statuses;
            }
        }

        public void RegisterMember(User member) {
            if(member != null) {
                if(m_members.ContainsKey(member.BatchNo)) {
                    m_members[member.BatchNo] = member;
                } else {
                    m_members.Add(member.BatchNo, member);
                }
            }
        }
        public void Remove(Guid batchNo) {
            if(m_members.ContainsKey(batchNo)) {
                m_members.Remove(batchNo);
            }
        }
        public void Remind(IEnumerable<RemindInfo> remindInfos) {
            var employees = m_members.Values;
            foreach(var member in employees) {
                if(member != null) {
                    member.Remind(remindInfos);
                }
            }
        }
    }
}