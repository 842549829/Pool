<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteSet.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.DistributionOEMSiteSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>站点信息</title>
</head>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        站点信息</h3>
    <div class="O_formBox">
        <span>网站名称：</span><br />
        <asp:Label runat="server" ID="lblName"></asp:Label>
        <span class="muted">网站名称，将显示在浏览器窗口标题等位置</span>
    </div>
    <div class="O_formBox">
        <span>网站域名：</span><br />
        <asp:Label runat="server" ID="lblDomain"></asp:Label>
        <span class="muted">用户可用通过该域名访问您的站点，也作为站内首页跳转URL</span>
    </div>
    <div class="O_formBox">
        <span>logo设置：</span><br />
        <%--<asp:Label runat="server" ID="lblLogo"></asp:Label>--%>
        <asp:HiddenField runat="server" ID="imgUrl" />
        <a href="javascript:;" onmouseover="ShowoldImg();" onmouseout="HideoldImg();">查看图片</a>
        <img style="position: absolute; display: none; background-color: white; width: 220px;
            height: 30px;" class="box" id="oldimg" />
        <span class="muted">建议使用200x30像素的png图片，最大体积不能超过50kb</span>
    </div>
    <div class="O_formBox">
        <span>管理员邮箱：</span><br />
        <asp:Label runat="server" ID="lblEmail"></asp:Label>
        <span class="muted">管理员Email，将作为系统发邮件的时候的发件人地址</span>
    </div>
    <div class="O_formBox">
        <span>网站备案信息代码：</span><br />
        <asp:TextBox runat="server" ID="txtICP" TextMode="MultiLine" Rows="3" Columns="60"
            CssClass="text" ReadOnly="true"></asp:TextBox>
        <span class="muted">页面底部可用显示ICP备案信息，在此输入您的授权码</span>
    </div>
    <div class="O_formBox">
        <span>网站第三方统计代码：</span><br />
        <asp:TextBox runat="server" ID="txtEmbedCode" TextMode="MultiLine" Rows="3" Columns="60"
            CssClass="text" ReadOnly="true"></asp:TextBox>
        <span class="muted">页面底部可用显示第三方统计帮助您查看网站的相关访问统计信息</span>
    </div>
    <div class="O_formBox">
        <span>是否关闭站点：</span><br />
        <asp:Label runat="server" ID="lblEnable"></asp:Label>
    </div>
    <div class="O_formBox">
        <span>是否允许用户自行注册：</span><br />
        <asp:Label runat="server" ID="lblReg"></asp:Label>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function ShowoldImg() {
        var url = $("#imgUrl").val();
        $("#oldimg").attr("src", url);
        $("#oldimg").css("display", "");
    };
    function HideoldImg() {
        $("#oldimg").css("display", "none");
    };
</script>