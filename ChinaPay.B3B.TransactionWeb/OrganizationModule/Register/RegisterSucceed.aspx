<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterSucceed.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.RegisterSucceed" %>

<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Fotter" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>注册成功</title>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc:Head runat="server" ID="head" />
    <div id="bd">
        <div id="registerSuccess">
            <h3>注册成功</h3>
            <div class="successBox" id="divNotProvider" runat="server">
                <span id="success">恭喜，您的
              <asp:Label ID="lblOem" runat="server">B3B</asp:Label>  
                采购帐号注册成功 </span>
                <p class="successText">
                    <span>您的用户名为：</span><span id="lblNotProviderAccount" runat="server"></span><br />
                    <span id="lblNotProviderCompanyName" runat="server"></span><br />
                    <span id="lbpoolpayAccount1" runat="server" Visible="False"></span><br />
                    <span style="color:#525a6b" id="accountTip1" runat="server">该帐号既是您的购票帐号，也是您的付款帐号</span>
                </p>
                <input type="button" class="btn class1" value="返回登录" onclick="window.location.href='../../Logon.aspx'" />
            </div>
            <div class="successBox" id="divProvider" runat="server">
                <span id="important">恭喜，您的<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>出票代理帐号注册成功，我们将在24小时内与您联系审核事宜 </span>
                <p class="successText">
                    <span>您的用户名为：</span><span id="lblProviderAccount" runat="server"></span><br />
                    <span id="lblProviderCompanyName" runat="server"></span><br />
                    <span id="lbpoolpayAccount2" runat="server" Visible="False"></span><br />
                    <span style="color:#525a6b" id="accountTip2" runat="server">该帐号既是您的购票帐号，也是您的付款帐号</span>
                </p>
                <input type="button" class="btn class1" value="返回登录" onclick="window.location.href='../../Logon.aspx'" />
            </div>
        </div>
    </div>
    <uc:Fotter runat="server" ID="fotter" />
    </form>
</body>
</html>
<script src="../../Scripts/setting.js" type="text/javascript"></script>