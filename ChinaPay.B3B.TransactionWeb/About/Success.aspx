<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.About.Success" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 修改成功</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div class="lostPwdContent box">
        <div class="lostPwdTitle">
            <p class="lostPwdSuccess">
                    修改密码成功！
            </p>
        </div>
        <a href="/Logon.aspx" class="lostPwdGoback">返回登录</a>
    </div>
    <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </form>
</body>
</html>
