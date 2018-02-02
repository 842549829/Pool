using System;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.AnnounceHandlers
{
    /// <summary>
    /// 平台公告管理列表
    /// </summary>
    public class TerraceAnnounce : BaseHandler
    {
        /// <summary>
        /// 公告列表查看
        /// </summary>
        /// <param name="announce"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public object TerraceAnnounceQuery(AnnounceQueryCondition announce, Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = Pagination.Default;
            }
            return AnnounceService.Query(new Guid(), announce);
        }

        /// <summary>
        /// 得到单条公告信息
        /// </summary>
        /// <param name="id">公告编号</param>
        /// <returns></returns>
        public object QueryAnnounceOnce(string id)
        {
            AnnounceView view = AnnounceService.Query(Guid.Parse(id));
            return new
            {
                Title = view.Title,
                PublishTime = view.PublishTime.ToShortDateString(),
                Content = view.Content
            };
        }

        /// <summary>
        /// 得到发出的所有紧急公告的id集合
        /// </summary>
        /// <returns></returns>
        public object QueryEmergenceIds()
        {
            return AnnounceService.QueryEmergencyIdsByOem(BasePage.IsOEM?BasePage.OEM.CompanyId:this.CurrentCompany.IsOem?this.CurrentCompany.CompanyId:Guid.Empty, BasePage.IsOEM, this.CurrentCompany.IsOem);
        }

        /// <summary>
        /// 修改公告
        /// </summary>
        /// <param name="ann"></param>
        public void ModifyAnnounce(AnnounceView ann)
        {
            AnnounceService.Update(new Guid(), ann, Common.Enums.PublishRole.平台, new Guid().ToString());
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        public void RegisterAnnounce(AnnounceView view)
        {
            AnnounceService.InsertPlatform(new Guid(), view, new Guid().ToString());
        }
        public object GetColseSession()
        {
            return Session["ShowNotice"] == null;
        }
        public void SetColseSession()
        {
            Session["ShowNotice"] = null;
        }
    }
}