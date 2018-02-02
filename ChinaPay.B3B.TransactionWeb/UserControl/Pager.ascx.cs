using System;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.UserControl {
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
                    ViewState["PageSize"] = dropPageSize.SelectedValue;
                }
                return (int)ViewState["PageSize"];
            }
            set
            {
                if (value >= 0)
                {
                    ViewState["PageSize"] = value;
                    if (dropPageSize != null)
                    {
                        dropPageSize.SelectedValue = value.ToString();
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
            var m = CurrentPageIndex - 4;
            if(m < 1) {
                m = 1;
            }
            var n = CurrentPageIndex + 4;
            if(n > PageCount) {
                n = PageCount;
            }
            for(var i = m; i <= n; i++) {
                CreateLinkButton(i);
            }
        }
        /// <summary>
        /// 逐个创建页码按钮
        /// </summary>
        /// <param name="i"></param>
        private void CreateLinkButton(int i) {
            var lb = new LinkButton();
            lb.Style.Add("margin-left", "5px");
            lb.Style.Add("margin-right", "5px");
            lb.ID = "lbPage" + i;
            lb.Text = i.ToString();
            if(CurrentPageIndex == i) {
                lb.CssClass = "yemaa";
                lb.Enabled = false;
                lb.Style.Add("text-decoration", "none");
            }else {
                lb.Click += lbPage_Click;
                lb.OnClientClick = "if(typeof(SaveSearchConditionWhenPagging)!='undefined')SaveSearchConditionWhenPagging("+i+")";
            }
            pPages.Controls.Add(lb);
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
        protected void Page_Load(object sender, EventArgs e) {
            CreateLinkButton();
            //this.lblPageSize.Text = this.PageSize.ToString();
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
        //点击每页显示多少条
        protected void dropPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageSize = int.Parse(dropPageSize.SelectedValue);
            PageSize = pageSize;
            CurrentPageIndex=1;
            OnCurrentPageChanged();
        }
    }
    public delegate void CurrentPageChangedEventHandler(Pager sender, int newPage);
}