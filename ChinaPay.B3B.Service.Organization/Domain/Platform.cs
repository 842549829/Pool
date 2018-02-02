using System;
using Izual.Linq;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.Service.Organization.Domain {
    using System.Linq;
    using Common.Enums;
    using Data.DataMapping;

    /// <summary>
    /// 在系统中表示 "运营方" 的类型，通常在系统中应只存在该类型的唯一实例。
    /// </summary>
    public sealed class Platform : Company {
        /// <summary>
        /// 平台在运行期间的唯一实例
        /// </summary>
        public static readonly Platform Instance;
        /// <summary>
        /// 设置单例
        /// </summary>
        static Platform() {
            //Instance = new Platform { Id = new Guid("3341AD8B-2E0D-46DD-A3F5-1B1406DAAB16"),Type = CompanyType.Platform};
            Instance = (from c in DataContext.Companies
                        where c.Type == CompanyType.Platform
                        join e in DataContext.Employees on new { Company = c.Id, IsAdministrator = true } equals new { Company = e.Owner, e.IsAdministrator } into es
                        from e in es.DefaultIfEmpty()
                        join a in DataContext.Addresses on c.Address equals a.Id into addrs
                        from a in addrs.DefaultIfEmpty()
                        join ct in DataContext.Contacts on c.Contact equals ct.Id into cts
                        from ct in cts.DefaultIfEmpty()
                        join mgr in DataContext.Contacts on c.Contact equals mgr.Id into mgrs
                        from mgr in mgrs.DefaultIfEmpty()
                        join emg in DataContext.Contacts on c.Contact equals emg.Id into emgs
                        from emg in emgs.DefaultIfEmpty()
                        select new Platform {
                            AbbreviateName = c.AbbreviateName,
                            Address = null,//new Address { Area = a.Area, Avenue = a.Avenue, City = a.City, District = a.District, Country = a.Country, Id = a.Id, Province = a.Province, ZipCode = a.ZipCode },
                            Audited = true,
                            Contact = null,
                            EffectTime = DateTime.MaxValue,
                            Name = c.Name,
                            Enabled = true,
                            Faxes = c.Faxes,
                            OfficePhones = c.OfficePhones,
                            IsOem = false,
                            IsVip = false,
                        }).SingleOrDefault();
        }

        /// <summary>
        /// 通过对一个公司的审核
        /// </summary>
        /// <returns>返回审核操作是否成功</returns>
        public bool Accept(Guid companyId) {
            return DataContext.Companies.Update(c => new { Audited = true }, c => c.Id == companyId) > 0;
        }
        ///// <summary>
        ///// 拒绝对一个公司的审核
        ///// </summary>
        ///// <param name="company">要拒绝审核的公司</param>
        ///// <returns>返回拒绝操作是否成功</returns>
        //public bool Deny(Company company) {
        //    if (company == null) {
        //        throw new ArgumentNullException("company");
        //    }
        //    return company.Update(c => c.IsAudited = false);
        //}
        ///// <summary>
        ///// 禁用一个公司
        ///// </summary>
        ///// <param name="company">要禁用的公司</param>
        ///// <returns>返回禁用操作是否成功</returns>
        //public bool Disable(Company company) {
        //    if (company == null) {
        //        throw new ArgumentNullException("company");
        //    }
        //    return company.Update(c => c.IsEnabled = false);
        //}
        ///// <summary>
        ///// 启用一个公司
        ///// </summary>
        ///// <param name="company">要启用的公司</param>
        ///// <returns>返回启用操作是否成功</returns>
        //public bool Enable(Company company) {
        //    if (company == null) {
        //        throw new ArgumentNullException("company");
        //    }
        //    return company.Update(c => c.IsEnabled = true);
        //}
        ///// <summary>
        ///// 将一个公司创建为 OEM
        ///// </summary>
        ///// <param name="company">要创建为 OEM 的公司</param>
        ///// <returns>返回创建操作是否成功</returns>
        //public bool CreateOem(Company company) {
        //    //判断该公司是否为分销节点
        //    if (!(company is IDistributionNode)) {
        //        throw new InvalidOperationException("该公司类型为" + company.Type.Name + ",不能成为OEM");
        //    }
        //    var disNode = company as IDistributionNode;
        //    disNode.OemInfo = new OemInfo(disNode);
        //    return disNode.OemInfo.Insert() && disNode.Update(c => c.IsOem = true);
        //}
        ///// <summary>
        ///// 将一个公司设置为 VIP
        ///// </summary>
        ///// <param name="company">要设置为 VIP 的公司</param>
        ///// <returns>返回设置操作是否成功</returns>
        //public bool SetVip(Company company) {
        //    //判断该公司是否为分销节点
        //    if (!(company is IDistributionNode)) {
        //        throw new InvalidOperationException("该公司类型为" + company.Type.Name + ",不能成为OEM");
        //    }
        //    return company.Update(c => (c as IDistributionNode).IsVip = true);
        //}
        ///// <summary>
        ///// 开设账号（即在平台上创建一个公司）
        ///// </summary>
        ///// <param name="company">要开设账号的公司</param>
        ///// <returns>返回操作是否成功</returns>
        //public bool EstablishAccount(Company company) {
        //    var r = RelationshipFactory.Create(this, company, RelationshipType.ServiceProvide);//创建服务提供关系
        //    return company.Insert() && r.Insert();
        //}
        ///// <summary>
        ///// 按指定的条件谓词，查询满足条件的所有公司
        ///// </summary>
        ///// <param name="predicate">条件谓词</param>
        ///// <returns>返回所有满足条件的公司</returns>
        //public IEnumerable<Dtos.CompanyCreatureInfo> QueryCompanies(Expression<Func<Company, bool>> predicate) {
        //    return B3B.Companies.Where(predicate).Select(c => c.Map<Dtos.Company>());
        //}
        ///// <summary>
        ///// 按指定的条件谓词，查询满足条件的公司间关系
        ///// </summary>
        ///// <param name="predicate">条件谓词</param>
        ///// <returns>返回所有满足条件的公司间关系</returns>
        //public IEnumerable<Relationship> QueryRelationships(Func<Relationship, bool> predicate) {
        //    return B3B.Relationships.Where(predicate);
        //}
        ///// <summary>
        ///// 获取当前公司的类型
        ///// </summary>
        //public override CompanyType Type {
        //    get { return CompanyType.Platform; }
        //}

        ///// <summary>
        ///// 获取一个值，用于指示当前公司是否可修改工作设置
        ///// </summary>
        //public override bool CanEditWorkSettings {
        //    get { return true; }
        //}
        //public bool Reject(Guid companyId) {
        //    return Data.Companies.Update(c => new { Audited = false }, c => c.Id == companyId) > 0;
        //}

        //public bool Disable(Guid companyId) {
        //    return Data.Companies.Update(c => new { Enabled = false }, c => c.Id == companyId) > 0;
        //}

        //public bool Enable(Guid companyId) {
        //    return Data.Companies.Update(c => new { Enabled = true }, c => c.Id == companyId) > 0;
        //}
    }
}
