$(function () {
    $("#btnSubmit").click(function () {
        var namePattern = /^[a-zA-z\u4e00-\uf95a]{1,8}$/;
        var officePhonePattern = /^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$/;
        var phonePattern = /^1[3458]\d{9}$/;
        var postCodePattern = /^\d{6}$/;
        var emailPattern = /^\w+@\w+(\.\w{2,4}){1,2}$/;
        var faxPattern = /^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$/;
        var qqPattern = /^\d{5,12}$/;
        var accountType = $("#hfdAccountType").val();
        var contact = $("#hfdContactName").val();
        if (!namePattern.test(contact)) {
            $("#txtContactName").select();
            alert("联系人格式错误！");
            return false;
        }
        var contactPhone = $("#hfdContactPhone").val();
        if (!phonePattern.test(contactPhone)) {
            $("#txtContactPhone").select();
            alert("联系人手机格式错误！");
            return false;
        }
        if (accountType == "enterprise") {
            var companyPhone = $("#hfdCompanyPhone").val();
            if (!officePhonePattern.test(companyPhone)) {
                $("#txtCompanyPhone").select();
                alert("企业电话格式错误！");
                return false;
            }
            var manager = $("#hfdManager").val();
            if (!namePattern.test(manager)) {
                $("#txtManager").select();
                alert("负责人格式错误！");
                return false;
            }
            var managerPhone = $("#hfdManagerPhone").val();
            if (!phonePattern.test(managerPhone)) {
                $("#txtManagerPhone").select();
                alert("负责人手机格式错误！");
                return false;
            }
            var emergency = $("#hfdEmergency").val();
            if (!namePattern.test(emergency)) {
                $("#txtEmergency").select();
                alert("紧急联系人格式错误！");
                return false;
            }
            var emergencyPhone = $("#hfdEmergencyPhone").val();
            if (!phonePattern.test(emergencyPhone)) {
                $("#txtEmergencyPhone").select();
                alert("紧急联系人手机错误！");
                return false;
            }
        }
        if (accountType == "individual") {
            var fixedPhone = $("#hfdFixedPhone").val();
            if ($.trim(fixedPhone) != "" && !officePhonePattern.test(fixedPhone)) {
                $("#txtFixedPhone").select();
                alert("固定电话格式错误！");
                return false;
            }
        }
        var postCode = $("#hfdPostCode").val();
        if (!postCodePattern.test(postCode)) {
            alert("邮编格式错误！");
            $("#txtPostCode").select();
            return false;
        }
        var email = $("#hfdEmail").val();
        if (!emailPattern.test(email)) {
            alert("Email格式错误！");
            $("#txtEmail").select();
            return false;
        }
        var fax = $("#hfdFax").val();
        if (fax.length > 0 && !faxPattern.test(fax)) {
            alert("传真格式错误！");
            $("#txtFax").focus();
            return false;
        }
        var qq = $("#hfdQQ").val();
        if (qq.length > 0 && !qqPattern.test(qq)) {
            alert("QQ格式错误！");
            $("#txtQQ").select();
            return false;
        }
        var county = $("#ddlCounty option:selected").val();
        if (county == "") {
            alert("请选择所在地！");
            return false;
        }
        var address = $("#hfdAddress").val();
        if (address.length > 25 || address.length <= 0) {
            alert("地址格式错误！")
            $("#txtAddress").select();
            return false;
        }
    });
    tip($("#txtAddress"), $("#hfdAddress"));
    tip($("#txtContactName"), $("#hfdContactName"));
    tip($("#txtContactPhone"), $("#hfdContactPhone"));
    tip($("#txtCompanyPhone"), $("#hfdCompanyPhone"));
    tip($("#txtManager"), $("#hfdManager"));
    tip($("#txtManagerPhone"), $("#hfdManagerPhone"));
    tip($("#txtEmergency"), $("#hfdEmergency"));
    tip($("#txtEmergencyPhone"), $("#hfdEmergencyPhone"));
    tip($("#txtPostCode"), $("#hfdPostCode"));
    tip($("#txtEmail"), $("#hfdEmail"));
    tip($("#txtFax"), $("#hfdFax"));
    tip($("#txtQQ"), $("#hfdQQ"));
    tip($("#txtFixedPhone"), $("#hfdFixedPhone"));
    inputTipText();
});
function tip(infoTipObj, infoTipValueObj) {
    infoTipObj.focus(function () {
        if (infoTipObj.val() == infoTipObj.attr("tip")) {
            infoTipObj.val("");
        }
    }).blur(function () {
        if (infoTipObj.val() == "") {
            infoTipObj.val(infoTipObj.attr("tip"));
            infoTipValueObj.val("");
        } else {
            infoTipValueObj.val(infoTipObj.val());
        }
    });
}