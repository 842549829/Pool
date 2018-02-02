<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.Header" %>
<div id="hd">
    <div id="hgroup" style="width:98%">
        <h1 id="logo"><a href="/Index.aspx"><img src="/Images/logo.png" alt="" id="imgLogo"  style="width: 220px;  height: 30px;"  runat="server" /></a></h1>
        <%-- 登录前 --%>
        <div runat="server" id="divLogoff" class="corner login-bar">
            <a href="/Logon.aspx" runat="server" id="logonPage">登录</a>
            <a href="/OrganizationModule/Register/Register.aspx" runat="server" id="registerPage">注册</a>
            <a href="#this" onclick='setHomePage(this);'>设为首页</a>
            <a href="#this" onclick="addFavorite('我的机票平台');">加入收藏</a>
        </div>
        <%-- 登录后 --%>
        <div runat="server" id="divLogon" class="corner login-bar">
            <a href="/PurchaseDefault.aspx" runat="server" id="IwillBuy">我要买</a>
            <a href="/Index.aspx?redirectUrl=/FlightReserveModule/PNRImport.aspx" id="ImportPNR" runat="server" >编码导入</a>
            <a href="javascript:;" runat="server" id="logonUserName"></a>
            <a href="/Index.aspx?redirectUrl=/OrganizationModule/RoleModule/UpdatePassword.aspx">修改密码</a>
            <a href="/About/help.aspx" class="help">帮助</a>
            <a href="#this" runat="server" id="exit" class="exit" onserverclick="exit_ServerClick">退出</a>
        </div>
    </div>
</div>
<div id="nav" style="width:98%">
    <a href="/Default.aspx" runat="server" id="homePageLink" visible="false">首页</a>
    <a href="/About/ThreeCharCode.aspx">三字码查询</a>
    <a href="/About/OnLineService.aspx" runat="server" id="lnkCustomerService">在线客服</a>
    <a href="/Index.aspx?redirectUrl=/IntegralCommodity/CommodityShowList.aspx" Visible="False" runat="server" id="lnkJifen">积分商城</a>
    <span runat="server" id="links"></span>
    <a href="javascript:returnUrl()" runat="server"  id="mypoolpay" class="myPoolpay">我的国付通</a>
</div>
<script type="text/javascript">
    function returnUrl() {
           window.location.href = "/Index.aspx?redirectUrl=" + encodeURIComponent("/OrganizationModule/Account/AccountInformation.aspx");
    }
</script>