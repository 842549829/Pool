<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterPaySucceed.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage.RegisterPaySucceed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>账户开户成功</title>
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/page.css?20121118" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="success-tips box">
        <h3>已为您自动开通了国付通账号！</h3>
        <p>您的国付通账号是：<span class="obvious" id="lblAccounNo" runat="server"></span></p>
        <p>您的登录密码是：同B3B网登录密码</p>
        <p>您的支付密码是：您的身份证后6位</p>
        <p><a href="http://www.poolpay.cn" class="obvious" target="_parent">点击此处</a>可登录并管理您的账户系统。</p>
        <p>
            <button class="btn class1" type="button" runat="server" id="btnOk">确定</button>
        </p>
    </div>
</body>
</html>
