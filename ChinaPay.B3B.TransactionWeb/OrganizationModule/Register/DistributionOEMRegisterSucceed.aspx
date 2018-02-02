<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMRegisterSucceed.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.DistributionOEMRegisterSucceed" %>

<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Head" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Fotter" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>注册成功</title>
    <link href="../../Styles/page.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../Styles/core.css?20121205" rel="stylesheet" type="text/css" />
    <link href="../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../Styles/register.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
  <form id="form1" runat="server">
   <uc:Head runat="server" ID="head" />
    <div id="bd">
        <div id="registerSuccess">
            <h3>注册成功</h3>
            <div class="successBox" id="divNotProvider" runat="server">
                <span id="success">恭喜，您的采购帐号注册成功 </span>
                <p class="successText">
                    <span>您的用户名为：</span><span id="lblNotProviderAccount" runat="server"></span><br />
                    <span id="lblNotProviderCompanyName" runat="server"></span><br />
                    <span id="lbpoolpayAccount1" runat="server" Visible="False"></span><br />
                    <span style="color:#525a6b" id="accountTip1" runat="server">该帐号既是您的购票帐号，也是您的付款帐号</span>
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