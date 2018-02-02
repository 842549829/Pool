using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class AreaRelateList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
            }
            this.dataSource.Visible = false;
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
            queryAreaRelateList(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageSize = pager.PageSize,
                        PageIndex = 1,
                        GetRowCount = true
                    };
                    queryAreaRelateList(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        void queryAreaRelateList(Pagination pagination)
        {
            try
            {
                var areaRelate = from item in AreaService.Query(getCondition(), pagination)
                                 select new
                                 {
                                    ProvinceName = item.ProvinceName,
                                    AreaName = item.AreaName,
                                    ProvinceCode = item.ProvinceCode
                                 };
            this.dataList.DataSource = areaRelate;
                this.dataList.DataBind();
                if(areaRelate.Any()) {
                    this.dataSource.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    this.pager.Visible = true;
                    if(pagination.GetRowCount) {
                        this.pager.RowCount = pagination.RowCount;
                    }
                } else {
                    this.pager.Visible = false;
                    this.dataSource.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private AreaRelationQueryCondtion getCondition()
        {
            var condition = new AreaRelationQueryCondtion()
            {
                 AreaName = this.txtAreaName.Text,
                 ProvinceName = this.txtPronvice.Text
            };
            return condition;
        }

        private bool Valiate()
        {
            if (this.txtAreaName.Text.Trim().Length > 25)
            {
                ShowMessage("区域名称位数不能超过25位！");
                return false;
            }

            if (this.txtPronvice.Text.Trim().Length > 20)
            {
                ShowMessage("省份名称位数不能超过20位！");
                return false;
            }

            return true;
        }
    }
}