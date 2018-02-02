using System;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.MaintenanceWeb.UserControl {
    public partial class Pager : System.Web.UI.UserControl {
        public event CurrentPageChangedEventHandler CurrentPageChanged;

        public Pager() {
            this.PageSize = 10;
        }
        
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize {
            get {
                if(ViewState["PageSize"] == null) {
                    ViewState["PageSize"] = 0;
                }
                return (int)ViewState["PageSize"];
            }
            set {
                if(value >= 0) {
                    ViewState["PageSize"] = value;
                    if(this.lblPageSize != null) {
                        this.lblPageSize.Text = value.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPageIndex {
            get {
                if(ViewState["CurrentPageIndex"] == null) {
                    ViewState["CurrentPageIndex"] = 1;
                }
                return (int)ViewState["CurrentPageIndex"];
            }
            set {
                if(CurrentPageIndex != value) {
                    ViewState["CurrentPageIndex"] = value;
                    OnCurrentPageChanged();
                    CreateLinkButton();
                    setPageControl();
                }
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount {
            get {
                return (RowCount + PageSize - 1) / PageSize;
            }
        }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RowCount {
            get {
                if(ViewState["RowCount"] == null) {
                    ViewState["RowCount"] = 0;
                }
                return (int)ViewState["RowCount"];
            }
            set {
                if(value >= 0) {
                    ViewState["RowCount"] = value;
                    CreateLinkButton();
                    setPageControl();
                }
            }
        }

        /// <summary>
        /// 创建页码按钮
        /// </summary>
        private void CreateLinkButton() {
            pPages.Controls.Clear();
            int m = CurrentPageIndex - 4;
            if(m < 1) {
                m = 1;
            }
            int n = CurrentPageIndex + 4;
            if(n > PageCount) {
                n = PageCount;
            }
            for(int i = m; i <= n; i++) {
                CreateLinkButton(i);
            }
            RegisterLinkButtonOnclick(CurrentPageIndex);
        }
        /// <summary>
        /// 逐个创建页码按钮
        /// </summary>
        /// <param name="i"></param>
        private void CreateLinkButton(int i) {
            LinkButton lb = new LinkButton();
            lb.Style.Add("margin-left", "5px");
            lb.Style.Add("margin-right", "5px");
            lb.ID = "lbPage" + i;
            lb.Text = i.ToString();
            lb.Click += lbPage_Click;
            if (CurrentPageIndex == i)
            {
                lb.CssClass = "yemaa";
                lb.Enabled = false;
                lb.Style.Add("text-decoration", "none");
            }
            else {
                lb.Click += lbPage_Click;
                lb.OnClientClick = GetSearchConditionWhenPagging(i);
            }
            pPages.Controls.Add(lb);
        }
        /// <summary>
        /// 注册保存cookie事件
        /// </summary>
        /// <param name="i"></param>
        private void RegisterLinkButtonOnclick(int i)
        {
            LinkButton prevLink = FindControl("lbnPrev") as LinkButton;
            if (prevLink != null)
                prevLink.OnClientClick = GetSearchConditionWhenPagging(i - 1);
            LinkButton nextLink = FindControl("lbnNext") as LinkButton;
            if (nextLink != null)
                nextLink.OnClientClick = GetSearchConditionWhenPagging(i + 1);
            LinkButton lastLink = FindControl("lbnLast") as LinkButton;
            if (nextLink != null)
                lastLink.OnClientClick = GetSearchConditionWhenPagging(PageCount);
        }
        private string GetSearchConditionWhenPagging(int currentPage) {
            return "if(typeof(saveSearchConditionWhenPagging)!='undefined')saveSearchConditionWhenPagging(" + currentPage + ")";
        }
        private void OnCurrentPageChanged() {
            if(CurrentPageChanged != null) {
                CurrentPageChanged(this, CurrentPageIndex);
            }
        }

        /// <summary>
        /// 点击页码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPage_Click(object sender, EventArgs e) {
            CurrentPageIndex = Convert.ToInt32((sender as LinkButton).Text);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e) {
            CreateLinkButton();
            this.lblPageSize.Text = this.PageSize.ToString();
        }
        protected void lbnFirst_Click(object sender, EventArgs e) {
            CurrentPageIndex = 1;
        }
        protected void lbnPrev_Click(object sender, EventArgs e) {
            if(CurrentPageIndex > 1) {
                CurrentPageIndex--;
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e) {
            if(CurrentPageIndex < PageCount) {
                CurrentPageIndex++;
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e) {
            if(PageCount > 0) {
                CurrentPageIndex = PageCount;
            }
        }
        private void setPageControl() {
            this.lbnFirst.Visible = true;
            this.lbnPrev.Visible = true;
            this.lbnLast.Visible = true;
            this.lbnNext.Visible = true;
            if(this.CurrentPageIndex == 1) {
                this.lbnFirst.Visible = false;
                this.lbnPrev.Visible = false;
            }
            if(this.CurrentPageIndex == this.PageCount) {
                this.lbnLast.Visible = false;
                this.lbnNext.Visible = false;
            }
            lblTotalPage.Text = this.PageCount.ToString();
            lblCurrentPage.Text = this.CurrentPageIndex.ToString();
            lblTotalCount.Text = this.RowCount.ToString();
        }
    }
    public delegate void CurrentPageChangedEventHandler(Pager sender, int newPage);
}