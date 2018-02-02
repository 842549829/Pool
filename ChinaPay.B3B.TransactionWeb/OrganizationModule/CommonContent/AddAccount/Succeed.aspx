<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Succeed.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount.Succeed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开户成功</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="bd">
        <div id="registerSuccess">
            <h3 class="titleBg">开户成功</h3>
            <div class="successBox" id="divNotProvider" runat="server">
                <span id="success">恭喜，您的<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>采购账号开户成功 </span>
                <p class="successText">
                    <span>您的账户名为：</span><span id="lblNotProviderAccount" runat="server"></span><br />
                    <span id="lblNotProviderCompanyName" runat="server"></span><br />
                    该账号既是您的购票账号，也是您的付款账号
                </p>
                <input type="button" class="btn class1" value="返回" runat="server" id="btnGoBack" />
            </div>
            <div class="successBox" id="divProvider" runat="server">
                <span id="important">恭喜，您的<asp:Literal runat="server" ID="lblPlatformName1"></asp:Literal>出票代理<span id="success0">账</span>号开户成功，我们将在24小时内与您联系审核事宜 </span>
                <p class="successText">
                    <span>您的账户名为：</span><span id="lblProviderAccount" runat="server"></span><br />
                    <span id="lblProviderCompanyName" runat="server"></span><br />
                    该账号既是您的购票账号，也是您的付款账号
                </p>
                <input type="button" class="btn class1" value="返回" runat="server" id="btnGoBacks"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
