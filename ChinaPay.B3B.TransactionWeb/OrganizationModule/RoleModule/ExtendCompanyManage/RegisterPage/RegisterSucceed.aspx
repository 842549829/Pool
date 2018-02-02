<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterSucceed.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage.RegisterSucceed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <title>开户成功</title>
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/page.css?20121118" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="success-tips box">
        <h3>开户成功</h3>
        <p>你的B3B公司名是：<span class="obvious" runat="server" id="lblCompanyName"></span></p>
        <p>你的B3B账户名是：<span class="obvious" runat="server" id="lblAccountNo"></span></p>
      <%--  <ul>
            <li>温馨提示</li>
            <li>1.温馨提示</li>
            <li>2.温馨提示</li>
        </ul>--%>
        <p>
            <form id="form1" runat="server">
                <button class="btn class1" type="button"  onclick="return window.location.href='PerfectPayInfo.aspx?AccountType=<%=AccountType %>';">
                开通国付通账号</button>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnGoBack" runat="server" Text="返回" CssClass="btn class1"/>
            </form>
        </p>
    </div>
    <script type="text/javascript">        window.history.go(1);</script>
</body>
</html>
