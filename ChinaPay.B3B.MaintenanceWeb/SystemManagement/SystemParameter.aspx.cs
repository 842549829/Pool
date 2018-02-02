using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using System.Linq;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class SystemParameter : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefResh();
            }
        }
        private void RefResh()
        {
            this.gvSystemPerameter.DataSource = SystemParamService.Query().Select(item => new {
                SystemParmType = item.Type.GetDescription(),
                Value = item.Value,
                Remark = item.Remark,
                TypeOf = item.Type.ToString()
            });
           this.DataBind();
        }
        #endregion
    }
}
