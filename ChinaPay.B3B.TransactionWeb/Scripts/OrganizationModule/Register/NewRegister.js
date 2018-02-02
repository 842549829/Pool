$(document).ready(function () {
    var person = function () {
        $('.personContent').show(); $('.companyContent').hide();
        if ($("#pIsPrivateAccountNo").length > 0) {
            $("#pIsPrivateAccountNo").hide();
            $("#chkIsPersonAccountNo").removeAttr("checked");
        }
        $("#trCompanyIDCard").hide();
    };
    var company = function () {
        $('.personContent').hide(); $('.companyContent').show();
        if ($("#pIsPrivateAccountNo").length > 0) {
            $("#pIsPrivateAccountNo").show();
        }
        $("#trCompanyIDCard").hide();
    };
    //账户角色切换
    var role = function (self) {
        var platformName = $("#hfdPlatformName").val();
        person();
        $("div#enrol ul li").removeClass("e1");
        self.addClass("e1");
        $("#person").show().next("label").show().end().attr("checked", true);
        if (self.attr("tip") == "1") {
            $("#hidCpmpanyType").val("Purchaser");
            $("#enrolPenrolContent").html(platformName + "平台兼容目前国内B2B机票平台的高返点业务，也是国内首家支持所有行程编码交易的唯一平台，更可以实现普通票、特价票、特殊票的交易。单程、往返程、多段联程、缺口程，散客和团队全航支持。增值服务申领实名POS机更是立马用客户的钱来做活生意。价比三家、低者为赢、现在加入采购，享高额采购积分换取免票......");
        } else if (self.attr("tip") == "2") {
            $("#hidCpmpanyType").val("Supplier");
            $("#enrolPenrolContent").html(platformName + "平台全国首创产品供应商。由平台完成编码订单交易。适合长期掌握低价航线产品、奖励免票销售资源，能控制座位资源的个人。比如航空公司座位控制人员、航空公司航线经理人、包机经营人、有计划机位的旅行社、有价格协调能力的黄牛党自然人、勤于观察航班肯吃苦耐劳控票的个人或机构均可以成为本平台的产品供应商，在" + platformName + "上主要优势是不用出票，功能只是简单处理编码状态既可完成订单交易。稀奇吧？现在注册，让天下没有难卖的机票......");
        } else if (self.attr("tip") == "3") {
            company();
            $("#enrolPenrolContent").html(platformName + "平台诚邀全国具备IATA资质、拥有B2B出票和BSP出票的代理公司加入出票商队伍。适合为各航空公司完成总量任务需要在平台冲量为主要业务的机票代理公司，在" + platformName +"上的功能主要是明折明扣返点订单的出票处理。与其它B2B平台相比，还可以发布以盈利为目的的各种特殊价格产品。" + platformName + "多达十五种机票交易模块让传统的出票商增加无限盈利之道。");
            $("#person").hide().next("label").hide().next(":radio").attr("checked", true);
            $("#hidCpmpanyType").val("Provider");
            $("#hidAccountType").val("company");
        }
    };
    $("div#enrol ul li").click(function () { role($(this)); });
    if ($("div#enrol ul li").length > 0) {
        role($("div#enrol ul li:eq(" + ($("#hidCpmpanyType").val() != "Purchaser" ? ($("#hidCpmpanyType").val() != "Supplier" ? 2 : 1) : 0) + ")"));
    }
    //账户类型切换
    $("#person").click(function () {
        person(); $("#hidAccountType").val("person");
    });
    $("#company").click(function () {
        company(); $("#hidAccountType").val("company");
    });
    if ($("#hidAccountType").val() == "person") {
        person(); $("#person").attr("checked", true);
    } else {
        company(); $("#company").attr("checked", true);
    }
    //获取验证码
    $("#btnPhoneCode").click(function () {
        var phone = $("#txtPhone").val(), account = $("#txtAccount").val();
        if (account.length < 1 || (account.length > 30 || !/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(account))) { alert("请输入正确的用户名"); return false; }
        if (phone.length < 1 || !/^1[3458]\d{9}$/.test(phone)) { alert("请输入正确的手机号码"); } else {
            $("#btnPhoneCode").attr("disabled", true);
            sendPostRequest("/OrganizationHandlers/Address.ashx/SendSMS", JSON.stringify({ "phone": phone, "account": account }),
            function (result) {
                if (result == "") { CountDown(120); } else { alert(result); $("#btnPhoneCode").attr("disabled", false); }
            }, function () { alert("发送验证码失败"); $("#btnPhoneCode").attr("disabled", false); });
        }
    });
});
//短信倒计时
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