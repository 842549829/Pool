<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelostPasswordCode.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.About.RelostPasswordCode" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 确认验证码</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div class="lostPwdContent box">
        <div class="lostPwdTitle">
            <p>
                验证码已发至<label id="lblphone" runat="server">135********</label>手机中，请注意查收。
            </p>
        </div>
        <ul>
            <li>
                <label>
                    请输入短信验证码：</label>
                <input type="text" class="text textarea" id="txtReCode" maxlength="6" runat="server" />
                <asp:Button runat="server" id="btnPhoneCode" class="btn class2" Text="重新获取验证码" 
                    onclick="btnPhoneCode_Click"  /> 
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
//        //获取验证码
//        $("#btnPhoneCode").click(function () {
//            var phone = $("#txtPhone").val(), account = $("#txtAccount").val();
//            if (account.length < 1 || (account.length > 30 || !/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(account))) { alert("请输入正确的用户名"); return false; }
//            if (phone.length < 1 || !/^1[3458]\d{9}$/.test(phone)) { alert("请输入正确的手机号码"); } else {
//                $("#btnPhoneCode").attr("disabled", true);
//                sendPostRequest("/OrganizationHandlers/Address.ashx/SendSMS", JSON.stringify({ "phone": phone, "account": account }),
//            function (result) {
//                if (result == "") { CountDown(60); } else { alert(result); $("#btnPhoneCode").attr("disabled", false); }
//            }, function () { alert("发送验证码失败"); $("#btnPhoneCode").attr("disabled", false); });
//            }
//            return true;
//        });
    });
    function CountDown(num) {
        $("#btnPhoneCode").attr("disabled", true);
        if (num == -1) {
            $("#btnPhoneCode").attr("disabled", false).val("重新获取");
            return;
        } else {
            $("#btnPhoneCode").val(num + "秒后可获取");
            setTimeout("CountDown(" + --num + ")", 1000);
        }
    } 
</script>
