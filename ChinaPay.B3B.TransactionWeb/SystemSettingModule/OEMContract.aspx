<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OEMContract.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.OEMContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>联系方式管理</title>
 </head>
   <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server" defaultbutton="btnSubmit" >
        <h3 class="titleBg">联系方式管理</h3>
        <div class="O_formBox">
		    <span>企业QQ：</span><br />
            <asp:TextBox runat="server" ID="txtEnterpriseQQ" CssClass="text"></asp:TextBox>
		    <span class="muted">填写您的企业QQ，若无则填写您的个人常用QQ号码，使用个人QQ需要开通允许临时会话功能，<a target="_blank" href="http://wp.qq.com/index.html">点此开通</a></span>
	    </div>
        <div class="O_formBox">
            <span class="titleBg">儿童机票受理传真：</span><br />
            <asp:TextBox runat="server" ID="txtFax" CssClass="text"></asp:TextBox>
            <span class="muted">主要是为了处理儿童所需要的户籍证明或儿童身份证复印件，号码格式为区号-号码</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">票务综合处理电话：</span><br />
            <asp:TextBox runat="server" ID="txtServicePhone" CssClass="text"></asp:TextBox>
            <span class="muted">提供催单\退票\废票\改期\退款\等综合查询以及处理或催办业务</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">退票组电话：</span><br />
            <asp:TextBox runat="server" ID="txtRefundPhone" CssClass="text"></asp:TextBox>
            <span class="muted">提供退票相关问题处理以及咨询服务</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">废票组电话 ：</span><br />
            <asp:TextBox runat="server" ID="txtScrapPhone" CssClass="text"></asp:TextBox>
            <span class="muted">提供废票相关问题处理以及咨询服务</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">支付相关服务电话 ：</span><br />
            <asp:TextBox runat="server" ID="txtPayServicePhone" CssClass="text"></asp:TextBox>
            <span class="muted">为客户提供账单支付状态查询及支付帮助</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">紧急业务受理电话 ：</span><br />
            <asp:TextBox runat="server" ID="txtEmergencyPhone" CssClass="text"></asp:TextBox>
            <span class="muted">提供临时起飞、航班延误、取消、变更等多种紧急业务的处理</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">投诉监督电话 ：</span><br />
            <asp:TextBox runat="server" ID="txtComplainPhone" CssClass="text"></asp:TextBox>
            <span class="muted">接受用户的投诉、举报、意见和建议</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">是否由B3B代为处理客服事宜 ：</span><span class="muted">若使用平台客服，上方所设联系方式都将无效，且相关客户服务由平台进行处理</span><br />
            <asp:RadioButton runat="server" ID="rdoAllowUseB3BServicePhone" Text="使用平台客服"  GroupName="useB3BServicePhone"/>
            <asp:RadioButton runat="server" ID="rdoAllowNotUseB3BServicePhone" Text="使用自建客服" GroupName="useB3BServicePhone" />
            <span class="muted">使用平台客服则下方不能禁止平台联系采购</span>
        </div>
        <div class="O_formBox">
            <span class="titleBg">是否允许平台直接联系采购处理订单 ：</span><br />
            <asp:RadioButton runat="server" ID="rdoAllowPlatformContractPurchaser" Text="允许平台联系采购" GroupName="platformContractPurchaser" />
            <asp:RadioButton runat="server" ID="rdoNotAllowPlatformContractPurchaser" Text="禁止平台联系采购" GroupName="platformContractPurchaser"/>
            <span class="muted">若选择禁止平台联系采购，平台不会以任何方式受理采购的请求也不会主动联系采购</span>
        </div>
        <asp:Button runat="server" ID="btnSubmit" Text="提交" CssClass="btn class1" 
            onclick="btnSubmit_Click" />
        <input runat="server" type="button" id="btnCancel" value="取消" class="btn class2" onclick="javascript:window.location.href='/OrganizationModule/TerraceModule/DistributionOemAuthorizationList.aspx?Search=Back';" />
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        if ($("#rdoAllowUseB3BServicePhone").is(":checked")) {
            $("#rdoNotAllowPlatformContractPurchaser").attr("disabled", "disabled");
        }
    });
    $("#rdoAllowUseB3BServicePhone").click(function () {
        $("#rdoNotAllowPlatformContractPurchaser").attr("disabled", "disabled");
        $("#rdoAllowPlatformContractPurchaser").attr("checked","checked");
    });
    $("#rdoAllowNotUseB3BServicePhone").click(function () {
        $("#rdoNotAllowPlatformContractPurchaser").removeAttr("disabled");
    });
    var phoneReg = /(^1[345789]\d{9}$)|(^\d{7,8}$)|(^\d{3,4}-\d{7,8}$)|(^\d{3,4}-\d{7,8}-\d{1,4}$)|(^\d{7,8}-\d{1,4}$)/;
    var qqReg = /^\d{4,13}$/;
    var faxReg = /(^\d{7,8}$)|(^\d{3,4}-\d{7,8}$)|(^\d{3,4}-\d{7,8}-\d{1,4}$)|(^\d{7,8}-\d{1,4}$)/;
    $("#btnSubmit").click(validation);
    function validation() {
        var useB3bServicePhone = $("#rdoAllowNotUseB3BServicePhone");
        //使用自建手机号码
        if (useB3bServicePhone.is(":checked")) {
            return validationEnterpriseQQ(true) && validationFaxNumber(true) && validationServicePhone(true) && validationRefundPhone(true) && validationScrapPhone(true) && validationPayServicePhone(true) && validationEmergencyPhone(true) && validationComplainPhone(true);
        } else {
            return validationEnterpriseQQ(false) && validationFaxNumber(false) && validationServicePhone(false) && validationRefundPhone(false) && validationScrapPhone(false) && validationPayServicePhone(false) && validationEmergencyPhone(false) && validationComplainPhone(false);
        }
    }
    function validationEnterpriseQQ(isRegExp) {
        var enterpriseQQ = $("#txtEnterpriseQQ");
        if (isRegExp && $.trim(enterpriseQQ.val()).length <= 0) {
            alert("QQ号码不能为空");
            enterpriseQQ.select();
            return false;
        }
        if ($.trim(enterpriseQQ.val()).length > 0 && !qqReg.test(enterpriseQQ.val())) {
            alert("QQ号码格式不正确");
            enterpriseQQ.select();
            return false;
        }
        return true;
    }
    function validationFaxNumber(isRegExp) {
        var fax = $("#txtFax");
        if (isRegExp && $.trim(fax.val()).length <= 0) {
            alert("传真号码不能为空");
            fax.select();
            return false;
        }
        if ($.trim(fax.val()).length > 0 && !faxReg.test(fax.val())) {
            alert("传真号码格式不正确");
            fax.select();
            return false;
        }
        return true;
    }
    function validationServicePhone(isRegExp) {
        var servicephone = $("#txtServicePhone");
        if (isRegExp && $.trim(servicephone.val()).length <= 0) {
            alert("票务综合处理电话不能为空");
            servicephone.select();
            return false;
        }
        if ($.trim(servicephone.val()).length > 0 && !phoneReg.test(servicephone.val())) {
            alert("票务综合处理电话格式不正确");
            servicephone.select();
            return false;
        }
        return true;
    }
    function validationRefundPhone(isReg) {
        var refundPhone = $("#txtRefundPhone");
        if (isReg && $.trim(refundPhone.val()).length <= 0) {
            alert("退票组电话不能为空");
            refundPhone.select();
            return false;
        }
        if ($.trim(refundPhone.val()).length > 0 && !phoneReg.test(refundPhone.val())) {
            alert("退票组电话格式不正确");
            refundPhone.select();
            return false;
        }
        return true;
    }
    function validationScrapPhone(isReg) {
        var scrapPhone = $("#txtScrapPhone");
        if (isReg && $.trim(scrapPhone.val()).length <= 0) {
            alert("废票组电话不能为空");
            scrapPhone.select();
            return false;
        }
        if ($.trim(scrapPhone.val()).length > 0 && !phoneReg.test(scrapPhone.val())) {
            alert("废票组电话格式不正确");
            scrapPhone.select();
            return false;
        }
        return true;
    }
    function validationPayServicePhone(isReg) {
        var payServicePhone = $("#txtPayServicePhone");
        if (isReg && $.trim(payServicePhone.val()).length <= 0) {
            alert("支付相关服务电话不能为空");
            payServicePhone.select();
            return false;
        }
        if ($.trim(payServicePhone.val()).length > 0 && !phoneReg.test(payServicePhone.val())) {
            alert("支付相关服务电话格式不正确");
            payServicePhone.select();
            return false;
        }
        return true;
    }
    function validationEmergencyPhone(isReg) {
        var emergencyPhone = $("#txtEmergencyPhone");
        if (isReg && $.trim(emergencyPhone.val()).length <= 0) {
            alert("紧急业务受理电话不能为空");
            emergencyPhone.select();
            return false;
        }
        if ($.trim(emergencyPhone.val()).length > 0 && !phoneReg.test(emergencyPhone.val())) {
            alert("紧急业务受理电话格式不正确");
            emergencyPhone.select();
            return false;
        }
        return true;
    }
    function validationComplainPhone(isReg) {
        var complainPhone = $("#txtComplainPhone");
        if (isReg && $.trim(complainPhone.val()).length <= 0) {
            alert("投诉监督电话不能为空");
            complainPhone.select();
            return false;
        }
        if ($.trim(complainPhone.val()).length > 0 && !phoneReg.test(complainPhone.val())) {
            alert("投诉监督电话格式不正确");
            complainPhone.select();
            return false;
        }
        return true;
    }
</script>