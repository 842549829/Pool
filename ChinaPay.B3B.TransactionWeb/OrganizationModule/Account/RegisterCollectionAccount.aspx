<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterCollectionAccount.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Account.RegisterCollectionAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>注册收款账号</title>
    
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head><link href="/Styles/register.css" rel="stylesheet" type="text/css" />
<body>
    <div id="smallbd1">
        <form runat="server" id="form1">
        <h3 class="titleBg">
            注册国付通收款账号
        </h3>
        <table class="formTable form">
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w50" />
            </colgroup>
            <tbody>
            <tr>
                <td class="title">请选择账户类型：</td>
                <td colspan="3">
                    <asp:RadioButton runat="server" ID="rdoIndividual" GroupName="roleType" Text="个人" Checked="true"/>
                    <asp:RadioButton runat="server" ID="rdoEnterprise" GroupName="roleType" Text="企业"/>
                </td>
            </tr>
            <tr>
                <td class="title">
                    <i class="must">*</i>国付通账号：
                </td>
                <td>
                    <asp:TextBox ID="txtAccount" runat="server" CssClass="longText text"></asp:TextBox>
                </td>
                <td>
                    请输入国付通账号
                </td>
            </tr>
            <tr>
                <td class="title">
                    <i class="must">*</i>登录密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="longText text" TextMode="Password"
                        onpaste="return false;"></asp:TextBox>
                </td>
                <td>
                    请输入登录密码
                </td>
            </tr>
            <tr>
                <td class="title">
                    <i class="must">*</i>确认登录密码：
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="longText text" TextMode="Password"
                        onpaste="return false"></asp:TextBox>
                </td>
                <td>
                    请输入确认登录密码
                </td>
            </tr>
            <tr>
                <td class="title">
                    <i class="must">*</i>支付密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPayPassowrd" runat="server" CssClass="longText text" TextMode="Password"
                        onpaste="return false;"></asp:TextBox>
                </td>
                <td>
                    请输入支付密码
                </td>
            </tr>
            <tr>
                <td class="title">
                    <i class="must">*</i>确认支付密码：
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPayPassowrd" runat="server" CssClass="longText text" TextMode="Password"
                        onpaste="return false;"></asp:TextBox>
                </td>
                <td>
                    请输入确认支付密码
                </td>
            </tr>
            </tbody>
            <tbody id="preson">
                <tr>
                    <td class="title">
                        <i class="must">*</i>真实姓名：
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入真实姓名
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>身份证号：
                    </td>
                    <td>
                        <asp:TextBox ID="txtIDCard" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入身份证号
                    </td>
                </tr>
            </tbody>
            <tbody id="company">
                <tr>
                    <td class="title">
                        <i class="must">*</i>企业名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入企业名称
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>机构代码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrganizationCode" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入机构代码
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>法人手机号：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入手机号码
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>法人姓名：
                    </td>
                    <td>
                        <asp:TextBox ID="txtLegalPersonName" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入法人姓名
                    </td>
                </tr>
            </tbody>
            <tbody>
                <tr>
                    <td class="title">
                        <i class="must">*</i>联系人手机号码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="longText text"></asp:TextBox>
                    </td>
                    <td>
                        请输入手机号码
                    </td>
                </tr>
            </tbody>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button Text="提交" CssClass="class1 btn" runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" />
                </td>
                <td>
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#preson").show();
        $("#company").hide();
        $("#rdoIndividual").click(function () {
            $("#preson").show();
            $("#company").hide();
        });
        $("#rdoEnterprise").click(function () {
            $("#preson").hide();
            $("#company").show();
        });
        $("input").blur(function () {
            Vaildate($(this).attr("id"));
        });

        $("#btnSubmit").click(function () {
            var falg = true;
            if (document.getElementById("rdoIndividual").checked) {
                for (var i = 0; i < $("tbody[id!=company] input").length; i++) {
                    if (!Vaildate($($("tbody[id!=company] input")[i]).attr("id"))) {
                        falg = Vaildate($($("tbody[id!=company] input")[i]).attr("id"));
                        break;
                    }
                }
            } else {
                for (var i = 0; i < $("tbody[id!=preson] input").length; i++) {
                    if (!Vaildate($($("tbody[id!=preson] input")[i]).attr("id"))) {
                        falg = Vaildate($($("tbody[id!=preson] input")[i]).attr("id"));
                        break;
                    }
                }
            }
            return falg;
        });

    });
    function Vaildate(id) {
        var obj = $("#" + id).parent().parent().find("td").eq(2);
        var reg;
        //验证账号
        if (id == "txtAccount") {
            reg = /^[\da-zA-z]{5,30}$/;
            if ($("#" + id).val() == "") {
                obj.html("账号名字不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入账号名字格式不正确,只能为6到30位的数字和字母!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证密码 
        else if (id == "txtPassword") {
            reg = /^[<>]{5,20}$/;
            if ($("#" + id).val() == "") {
                obj.html("密码不能为空!").css("color", "red");
                return false;
            }
            if (reg.test($("#" + id).val())) {
                obj.html("输入密码格式不正确,只能为6到20位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证确认密码 
        else if (id == "txtConfirmPassword") {
            reg = /^[<>]{5,20}$/;
            if ($("#" + id).val() == "") {
                obj.html("确认密码不能为空!").css("color", "red");
                return false;
            }
            if (reg.test($("#" + id).val())) {
                obj.html("输入确认密码格式不正确,只能为6到20位!").css("color", "red");
                return false;
            }
            if ($("#txtConfirmPassword").val() != $("#txtPassword").val()) {
                obj.html("输入确认密码和密码不一致,请重新输入!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        }
        //验证支付密码 
        else if (id == "txtPayPassowrd") {
            reg = /^[<>]{5,20}$/;
            if ($("#" + id).val() == "") {
                obj.html("支付密码不能为空!").css("color", "red");
                return false;
            }
            if (reg.test($("#" + id).val())) {
                obj.html("输入支付密码格式不正确,只能为6到20位!").css("color", "red");
                return false;
            }
            if ($("#txtPayPassowrd").val() == $("#txtPassword").val()) {
                obj.html("输入支付密码登录密码不能一致,请重新输入!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证支付确认密码 
        else if (id == "txtConfirmPayPassowrd") {
            reg = /^[<>]{5,20}$/;
            if ($("#" + id).val() == "") {
                obj.html("支付确认密码不能为空!").css("color", "red");
                return false;
            }
            if (reg.test($("#" + id).val())) {
                obj.html("输入支付确认密码格式不正确,只能为6到20位!").css("color", "red");
                return false;
            }
            if ($("#txtConfirmPayPassowrd").val() != $("#txtPayPassowrd").val()) {
                obj.html("输入支付确认密码和支付密码不一致,请重新输入!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证姓名 
        else if (id == "txtName") {
            reg = /^[a-zA-z\u4e00-\uf9a5]{1,20}$/;
            if ($("#" + id).val() == "") {
                obj.html("真实姓名不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入真实姓名格式不正确,只能为汉字并且2到20位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //身份证号码 
        else if (id == "txtIDCard") {
            if ($("#" + id).val() == "") {
                obj.html("身份证不能为空!").css("color", "red");
                return false;
            }
            if (!isCardID($("#" + id).val())) {
                obj.html("输入身份证格式不正确,只能为18位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证手机号码 
        else if (id == "txtCellPhone") {
            reg = /^1[3458]\d{9}$/;
            if ($("#" + id).val() == "") {
                obj.html("手机号码不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入手机号码格式不正确,只能为11位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证邮箱
        else if (id == "txtEmail") {
            reg = /^\w+@\w+(\.\w{2,4}){1,2}$/;
            if ($("#" + id).val() == "") {
                obj.html("邮箱不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入邮箱格式不正确,如www.xxx@xx.com!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证企业名称
        else if (id == "txtCompanyName") {
            reg = /^[\da-zA-z\u4e00-\u9fa5]{1,50}$/;
            if ($("#" + id).val() == "") {
                obj.html("企业名称不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入企业名称格式不正确,只能为汉字并且2到20位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证组织机构代码
        else if (id == "txtOrganizationCode") {
            reg = /^\d{8}-[\dXx]{1}$/;
            if ($("#" + id).val() == "") {
                obj.html("组织机构代码不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入组织机构代码格式不正确").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证企业电话
        else if (id == "txtCompanyPhone") {
            reg = /^1[3458]\d{9}$/; ;
            if ($("#" + id).val() == "") {
                obj.html("法人手机不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入法人手机格式不正确!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        } //验证法人姓名
        else if (id == "txtLegalPersonName") {
            reg = /^[\u4e00-\u9fa5]{2,50}$/;
            if ($("#" + id).val() == "") {
                obj.html("企业法人姓名不能为空!").css("color", "red");
                return false;
            }
            if (!reg.test($("#" + id).val())) {
                obj.html("输入企业法人姓名格式不正确,只能为汉字并且2到20位!").css("color", "red");
                return false;
            }
            obj.html("√").css("color", "green");
        }
        return true;
    };

</script>
