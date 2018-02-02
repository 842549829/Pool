<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LostPassword.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.About.LostPassword" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 忘记密码</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div class="lostPwdContent box">
        <div class="lostPwdTitle">
            <p>
                您可以在此找回登录密码，请填写以下信息。
            </p>
        </div>
        <ul>
            <li>
                <label>
                    请输入B3B帐号：</label>
                <input type="text" class="text textarea" id="txtAccountNo" maxlength="30" runat="server" /><label id="txtAccountNoTip" runat="server" class="obvious"></label>
            </li>
            <li>
                <label>
                    重复输入B3B帐号：</label>
                <input type="text" class="text textarea" id="txtReAccountNo" maxlength="30" runat="server" /><label id="txtReAccountNoTip" runat="server"  class="obvious"></label>
            </li>
            <li>
                <label>
                    验证码：</label>
                <input type="text" class="text textarea" id="txtCode" maxlength="5" runat="server" /><label id="txtCodeTip" runat="server"  class="obvious"></label>
            </li>
        </ul>
        <p class="codeMsg">
            请输入图中字符，不区分大小写
        </p>
        <img src="" alt="验证码" id="imgValidateCode" onclick="javascript:loadValidateCode();"
            title="换一张" />
        <asp:Button ID="btnNext" runat="server" CssClass="btn class1 lostPwdBtn" Text="下一步"
            OnClick="btnNext_Click" />
    </div>
    <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </form>
</body>
</html>
<script type="text/javascript">
    //window.location.href = window.location.href;
    $(function () {
        $("#btnNext").click(function () { 
            var account = $("#txtAccountNo").val();
            var reAccount = $("#txtReAccountNo").val();
            var code = $("#txtCode").val();
            if (account.length < 1 || (account.length > 30 || !/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(account))) { $("#txtAccountNoTip").html("请输入正确的B3B帐号"); $("#txtAccountNo").focus(); return false; }
            $("#txtAccountNoTip").html("");
            if (reAccount.length < 1 || (account.length > 30 || !/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(reAccount))) { $("#txtReAccountNoTip").html("请输入正确的重复B3B帐号"); $("#txtAccountNo").focus(); return false; }
            $("#txtReAccountNoTip").html("");
            if (account != reAccount) { $("#txtReAccountNoTip").html("两次输入的账号不一致"); $("#txtReAccountNo").focus(); return false; }
            if (code=="") { ("#txtCodeTip").html("验证码不能为空"); $("#txtCode").focus(); return false; }
            $("#txtReAccountNoTip").html("");
            //CountDown(60);
            return true;
        });
    });
    loadValidateCode();
    function loadValidateCode() {
        $("#imgValidateCode").attr("src", '/VerifyCode.ashx?verifyType=lostCode&round=' + Math.random());
    }
    function CountDown(num) {
        $("#btnNext").attr("disabled", true);
        if (num == -1) {
            $("#btnNext").attr("disabled", false).val("下一步");
            return;
        } else {
            $("#btnNext").val(num + "秒后可获取");
            setTimeout("CountDown(" + --num + ")", 1000);
        }
    }
</script>
