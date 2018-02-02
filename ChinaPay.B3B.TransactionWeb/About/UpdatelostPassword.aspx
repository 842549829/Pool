<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatelostPassword.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.About.UpdatelostPassword" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 输入新密码</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div class="lostPwdContent box">
        <div class="lostPwdTitle">
            <p>
                请输入您的新密码！
            </p>
        </div>
        <ul>
            <li>
                <label>
                    新密码：</label>
                <input type="password" class="text textarea" id="txtPwd" runat="server" maxlength="30" />
            </li>
            <li>
                <label>
                    确认密码：</label>
                <input type="password" class="text textarea" id="txtRePwd" runat="server" maxlength="30" />
            </li>
        </ul>
        <asp:Button ID="btnNext" runat="server" CssClass="btn class1 lostPwdBtn" Text="下一步"
            OnClick="btnNext_Click" />
    </div>
    <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnNext").click(function () {
            if ($("#txtPwd").val() == "") {
                alert("请输入密码。");
                return false;
            }
            if ($("#txtPwd").val().length() < 6) {
                alert("密码至少六位");
                return false;
            } 
            if ($("#txtRePwd").val() == "") {
                alert("请输入确认密码。");
                return false;
            }
            if ($("#txtRePwd").val().length() < 6) {
                alert("确认密码至少六位");
                return false;
            }
            if ($("#txtPwd").val() != $("#txtRePwd").val()) {
                alert("确认密码和确认密码不一致。");
                return false;
            }
        });
    });
</script>
