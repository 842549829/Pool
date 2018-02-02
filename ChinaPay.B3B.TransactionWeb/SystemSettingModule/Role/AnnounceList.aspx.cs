using System;
using System.Linq;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Announce;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class AnnounceList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageSize = pager.PageSize,
                        PageIndex = 1,
                        GetRowCount = true
                    };
                    queryAnnounces(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryAnnounces(pagination);
        }

        private void queryAnnounces(Pagination pagination)
        {
            try
            {
                IEnumerable<AnnounceListView> result = new List<AnnounceListView>();
                result = Service.Announce.AnnounceService.UserQuery(BasePage.IsOEM ? BasePage.OEM.CompanyId : this.CurrentCompany.IsOem ? this.CurrentCompany.CompanyId : Guid.Empty, BasePage.IsOEM, this.CurrentCompany.IsOem, pagination);
                var announces = from item in result
                                select new
                                {
                                    Id = item.Id,
                                    Title = item.Title,
                                    PublishTime = item.PublishTime.Date.ToShortDateString(),
                                    PublishAccount = item.PublishAccount,
                                    AnnounceType = item.AnnounceLevel.GetDescription(),
                                };
                this.dataSource.DataSource = announces;
                this.dataSource.DataBind();
                if (announces.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.emptyDataInfo.Visible = true;
                    this.dataSource.Visible = false;
                    this.pager.Visible = false;

                }
            }
            catch (Exception)
            {
            }
        }
    }
}