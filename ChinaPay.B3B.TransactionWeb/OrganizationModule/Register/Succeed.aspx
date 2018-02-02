<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Succeed.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.Succeed" %>
<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
    <title>开户成功</title>
    <link href="../../Styles/core.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/form.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header runat="server"  ID="ucHeader"/>
        <div id="succeed">
            <div class="success-tips box">
                <h3>开户成功</h3>
                <p>你的B3B公司名是：<span class="obvious" runat="server" id="lblCompanyName"></span></p>
                <p>你的B3B账户名是：<span class="obvious" runat="server" id="lblAccountNo"></span></p>
               <%-- <ul>
                    <li>温馨提示</li>
                    <li>1.温馨提示</li>
                    <li>2.温馨提示</li>
                </ul>--%>
                <p>
                       <button class="btn class1" type="button"  onclick="return window.location.href='PerfectPayInfo.aspx?AccountType=<%=AccountType %>';">
                        开通国付通账号</button>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnGoBack" runat="server" Text="返回" CssClass="btn class1"/>
                </p>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter" />
    </form>
    <script src="../../Scripts/setting.js" type="text/javascript"></script>
</body>
</html>
