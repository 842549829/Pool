namespace ChinaPay.B3B.Service.Organization.Domain {
    using System;
    using Common.Enums;
    using Data;
    using Data.DataMapping;
    using Izual;
    using CompanyInfo = Data.DataMapping.Company;

    /// <summary>
    ///     表示在系统中两个实体之间的关系
    /// </summary>
    public class Relationship {
        private readonly Guid id;
        private readonly object initiator;
        private readonly object responser;
        private readonly RelationshipType type;


        /// <summary>
        ///     关系类型 的默认构造方法
        /// </summary>
        /// <param name="initiator"> 发起方 </param>
        /// <param name="responser"> 接收方 </param>
        /// <param name="type"> </param>
        public Relationship(object initiator, object responser, RelationshipType type) {
            Validate(initiator, responser, type);
            id = Guid.NewGuid();
            this.initiator = initiator;
            this.responser = responser;
            this.type = type;
        }

        public Guid Id {
            get { return id; }
        }

        /// <summary>
        ///     发起方
        /// </summary>
        public object Initiator {
            get { return initiator; }
        }

        /// <summary>
        ///     响应方
        /// </summary>
        public object Responser {
            get { return responser; }
        }

        /// <summary>
        ///     关系类型
        /// </summary>
        public RelationshipType Type {
            get { return type; }
        }

        private static void Validate(object ittr, object rspr, RelationshipType t) {
            if (ittr == null)
                throw new InvalidOperationException("未指定关系发起方。");
            if (rspr == null)
                throw new InvalidOperationException("未指定关系响应方。");

            switch (t) {
                case RelationshipType.Organization: // 发起方为 Agent，响应方为不是平台和 Agent 的公司
                    if (!(ittr is CompanyInfo && ((CompanyInfo)ittr).Type == CompanyType.Provider))
                        throw new InvalidOperationException("关系发起方类型不正确。");
                    if (!(rspr is CompanyInfo && ((CompanyInfo)rspr).Type != CompanyType.Platform && ((CompanyInfo)rspr).Type != CompanyType.Provider))
                        throw new InvalidOperationException("关系响应方类型不正确。");
                    break;
                case RelationshipType.Distribution: // 发起方为 Agent，响应方为 Purchaser
                    if (!(ittr is CompanyInfo && ((CompanyInfo)ittr).Type != CompanyType.Platform))
                        throw new InvalidOperationException("关系发起方类型不正确。");
                    if (!(rspr is CompanyInfo && ((CompanyInfo)rspr).Type == CompanyType.Purchaser))
                        throw new InvalidOperationException("关系响应方类型不正确。");
                    break;
                case RelationshipType.Spread: // 发起方和响应方均为不是平台的公司或 Provider
                    if (!(ittr is CompanyInfo && ((CompanyInfo)ittr).Type != CompanyType.Platform || ittr is Provider))
                        throw new InvalidOperationException("关系发起方类型不正确。");
                    if (!(rspr is CompanyInfo && ((CompanyInfo)rspr).Type != CompanyType.Platform || rspr is Provider))
                        throw new InvalidOperationException("关系响应方类型不正确。");
                    break;
                case RelationshipType.ServiceProvide: // 发起方为平台，响应方为不是平台的公司或 Provider
                    if (!(ittr is Platform))
                        throw new InvalidOperationException("关系发起方类型不正确。");
                    if (!(rspr is CompanyInfo && ((CompanyInfo)rspr).Type != CompanyType.Platform || rspr is Provider))
                        throw new InvalidOperationException("关系响应方类型不正确。");
                    break;
            }
        }

        public bool Save() {
            var relation = new Data.DataMapping.Relationship { Id = id, Initiator = (Guid)initiator.Get("Id"), Responser = (Guid)responser.Get("Id"), Type = type, CreateTime = DateTime.Now };
            //return relation.InsertOrUpdate(r => r.Initiator == relation.Initiator && r.Responser == relation.Responser && r.Type == relation.Type);
            return relation.Insert();
        }
    }
}