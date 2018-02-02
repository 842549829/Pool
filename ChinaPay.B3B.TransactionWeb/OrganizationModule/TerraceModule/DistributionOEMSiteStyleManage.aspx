<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteStyleManage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOEMSiteStyleManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>风格管理</title>
</head>    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        风格管理</h3>
    <ul class="O_StyleList clearfix">
        <%
            string FileWeb = ConfigurationManager.AppSettings["FileWeb"];
            foreach (var item in ChinaPay.B3B.Service.Organization.OEMStyleService.QueryOEMStyles())
           {
        %>
        <li>
            <img src="<%=item.ThumbnailPicture %>" width="85" height="95"  alt="<%=item.Remark %>" /><p class="bor">
                <%=item.StyleName %></p>
            <p>
                <a href="DistributionOEMSiteStyleAddorUpdate.aspx?styleId=<%=item.Id %>">编辑</a>&nbsp;&nbsp;&nbsp;<a
                    href="javascript:del('<%=item.Id %>');">删除</a></p>
        </li>
        <%  } %>
    </ul>
    
        <input type="button" value="新增风格" onclick="location.href='DistributionOEMSiteStyleAddorUpdate.aspx'" class="btn class1" /><p class="muted">新增之前请将新的皮肤文件放到网站根目录的skin文件包下再进行新增，否则会添加失败或导致模板不完整</p>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/Widget/common.js" ></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>

<script type="text/javascript">
    function del(styleId)
    {
        if (!confirm('确定要删系统风格吗？')) return;
        sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/DeleteStyle", JSON.stringify({ styleId: styleId }), function (msg)
        {
            alert("删除成功！");
            location.reload();
        }, function (errMessage)
        {
            alert(errMessage);
        });
    
    }

</script>
