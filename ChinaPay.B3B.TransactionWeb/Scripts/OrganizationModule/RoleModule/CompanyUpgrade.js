$(function () {
    /****企业****/
    /*企业名称*/var txtCompany = function (e) { return Validate(["EmptyOrObject=企业名称不能为空", "CompanyName=输入有误,仅允许输入大于4个字符的汉字"], "txtCompanyName", e); };
    /*企业简称*/var txtAbbreviation = function (e) { return Validate(["EmptyOrObject=企业简称不能为空", "Abbreviation=请填写正确的公司简称"], "txtCompanyAbbreaviateName", e); };
    /*机构代码*/var txtOrganizationCode = function (e) { return Validate(["EmptyOrObject=机构代码不能为空", "OrganizationCode=输入错误,请检查后重新输入"], "txtOrgnationCode", e); };
    /*企业电话*/var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=企业电话不能为空", "Phone=请输入正确的电话如028-8883388"], "txtOfficePhone", e); };
    /*负责人*/var txtMangerName = function (e) { return Validate(["EmptyOrObject=负责人不能为空", "Name=输入有误，仅允许输入大于2个字符"], "txtManagerName", e); };
    /*负责人手机*/var txtManagerCellphone = function (e) { return Validate(["EmptyOrObject=负责人手机不能为空", "CellPhone=输入错误，请输入11位正确的手机号码"], "txtManagerMobile", e); };
    /*紧急联系人*/var txtEmergencyContact = function (e) { return Validate(["EmptyOrObject=紧急联系人不能为空", "Name=输入有误,仅允许输入大于2个字符"], "txtEmergencyName", e); };
    /*紧急联系人手机*/var txtEmergencyPhone = function (e) { return Validate(["EmptyOrObject=紧急联系人手机不能为空", "CellPhone=输入错误,请输入11位正确的手机号码"], "txtEmergecyMobile", e); };
    $("#txtCompanyName").focus(function () { $(this).message("请填写您营业执照上的名称,否则会造成结算失败"); }).blur(function () {
        var result = txtCompany();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtCompanyAbbreaviateName").focus(function () { $(this).message("建议四个汉字,如博宇科技"); }).blur(function () {
        var result = txtAbbreviation();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtOrgnationCode").focus(function () { $(this).message("填写您组织机构代码证上的代码如53038123-x"); }).blur(function () { return txtOrganizationCode(); });
    $("#txtOfficePhone").focus(function () { $(this).message("认真填写您的企业电话方便客服进行相关事宜的通知"); }).blur(function () { return txtCompanyPhone(); });
    $("#txtManagerName").focus(function () { $(this).message("负责人用于备用联系"); }).blur(function () { return txtMangerName(); });
    $("#txtManagerMobile").focus(function () { $(this).message("请填写负责人手机号码"); }).blur(function () { return txtManagerCellphone(); });
    $("#txtEmergencyName").focus(function () { $(this).message("紧急联系人用于航班变更等通知"); }).blur(function () { return txtEmergencyContact(); });
    $("#txtEmergecyMobile").focus(function () { $(this).message("请填写正确的手机号码"); }).blur(function () { return txtEmergencyPhone(); });

    $("body,#btnSubmit,#div_Upgrade").keypress(function (e) {
        if (e.keyCode == 13) {
            return false;
        }
    });

    $("#typeUpgradeApply").click(function () {
        $("#divOpcial").click();
        if ($("#hfdAccountType").val() == "individual") {
            if ($("#rbnSupplierIndividual").attr("checked") == "checked") {
                $("#lblTypeShow").html("产品合作");
                $("#upgradeInfo").hide();
                $("#warnInfo").show();
                $("#hfdSign").val("1");
                $("#hfdValid").val("false");
            }
            else {
                $("#upgradeInfo").show();
                $("#warnInfo").hide();
                $("#hfdValid").val("true");
                if ($("#rbnSupplierEnterprise").attr("checked") == "checked") {
                    $("#hfdSign").val("2");
                    $("#lblTypeShow").html("产品合作");
                } else {
                    $("#hfdSign").val("3");
                    $("#lblTypeShow").html("出票合作");
                }
            }
        }
        if ($("#hfdAccountType").val() == "enterprise") {
            $("#upgradeInfo").hide();
            $("#warnInfo").show();
            $("#hfdValid").val("false");
            if ($("#rbnSupplierEnterprise").attr("checked") == "checked") {
                $("#lblTypeShow").html("产品合作");
                $("#hfdSign").val("2");
            } else {
                $("#hfdSign").val("3");
                $("#lblTypeShow").html("出票合作");
            }
        }
    });
    $("input[name='type']").click(function () {
        var thisValue = $(this).val();
        if (thisValue == "rbnSupplierIndividual") {
            $("#upgradeInfo").hide();
            $("#warnInfo").show();
            $("#hfdValid").val("false");
        }
        if (thisValue == "rbnSupplierEnterprise" || thisValue == "rbnProviderEnterprise") {
            if ($("#hfdAccountType").val() == "individual") {
                $("#upgradeInfo").show();
                $("#warnInfo").hide();
                $("#hfdValid").val("true");
            }
            if ($("#hfdAccountType").val() == "enterprise") {
                $("#upgradeInfo").hide();
                $("#warnInfo").show();
                $("#hfdValid").val("false");
            }
        }
    });
    $(".close").click(function () {
        if ($("#hfdSign").val() == "1") {
            $("#rbnSupplierIndividual").attr("checked", "checked");
        } else {
            if ($("#hfdSign").val() == "2") {
                $("#rbnSupplierEnterprise").attr("checked", "checked");
            } else {
                $("#rbnProviderEnterprise").attr("checked", "checked");
            }
        }
    });
    $("#protocolContent").hide();
    $(".protocol").click(function () {
        $("#protocolContent").toggle();
    });

    //提交
    $("#btnConfirm").click(function () {
        var valid = $("#hfdValid").val();
        if (valid == "false") {
            if (($("#chkProtocol").attr("checked") == "checked")) {
                return true;
            } else {
                alert("请勾选“同意变更协议”");
                return false;
            }
        }

        if (valid == "true") {
            if (txtCompany(true) && txtAbbreviation(true) && txtOrganizationCode(true) && txtCompanyPhone(true) && txtMangerName(true) && txtManagerCellphone(true) && txtEmergencyContact(true) && txtEmergencyPhone(true)) {
                if (($("#chkProtocol").attr("checked") == "checked")) {
                    return true;
                } else {
                    alert("请勾选“同意变更协议”");
                    return false;
                }
            } else {
                return false;
            }
        }
    });

    $("#rbnProviderEnterprise").click(function () {
        $("#lblTypeShow").html("出票合作");
    });
    $("#rbnSupplierIndividual,#rbnSupplierEnterprise").click(function () {
        $("#lblTypeShow").html("产品合作");
    });
});
