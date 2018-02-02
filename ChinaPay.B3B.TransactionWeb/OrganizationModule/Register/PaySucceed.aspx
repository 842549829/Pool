<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaySucceed.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.PaySucceed" %>
<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>账户开户成功</title>
    <link href="../../Styles/core.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/form.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server" id="form1">
        <uc:Header runat="server" ID="ucHeader" />
        <div id="succeed">
            <div class="success-tips box">
                <h3>已为您自动开通了国付通账号！</h3>
                <p>您的国付通账号是：<span class="obvious" id="lblAccounNo" runat="server"></span></p>
                <p>您的登录密码是：同B3B网登录密码</p>
                <p>您的支付密码是：您的身份证后6位</p>
                <p><a href="http://www.poolpay.cn" class="obvious">点击此处</a>可登录并管理您的账户系统。</p>
                <p>
                   <input type="button" value="返回" class="btn class1" onclick="window.location.href='../../Logon.aspx'" />
                </p>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFotter" />
    </form>
    <script src="../../Scripts/setting.js" type="text/javascript"></script>
</body>
</html>
