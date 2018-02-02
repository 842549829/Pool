var businessId;
var businessType;
var productType;
var payAmount;
var userName;
var payHost;
var orderIsImport = false;
$(function () {
    businessId = $("#hidBusinessId").val();
    businessType = $("#hidBusinessType").val();
    productType = $("#hidProductType").val();
    userName = $("#hidUserName").val();
//    payHost = $("#hidPayHost").val();
    payAmount = $("#lblPayAmount").text();
    orderIsImport = $("#orderIsImport").val() == "1";
    $("#btnPay").click(function () { pay(); });
    $("#btnCancel, #btnPayComplete, #btnPayError").attr("href", getDetailUrl());
    $("#btnPaySuccess, #btnPayOtherOrder").attr("href", getQueryListUrl());
    setProductAttention();
    $("#divPayAttention").hide();
    $("#divPaySuccess").hide();
    $('#divPayTypes').tabs({
        event: "click",
        selected: "payNavCur",
        callback: function (i) { }
    });
    $(".payType_poolpay").click(function () {
        clearPayUrl();
        $(".selectPay").removeClass("selectPay");
        $("#btnPay").removeAttr("disabled");
    });
    $(".payType_Online").click(function () {
        setPayURL($(this).attr("value"));
        $(".selectPay").removeClass("selectPay");
        $("#btnPay").attr("disabled", "disabled").attr("src","");
    });
    $(".payType_Bank").click(function () {
        clearPayUrl();
        //$("#payType_Bank input:radio").attr("checked", false);
        $("#btnPay").attr("disabled", "disabled").attr("src", "");
    });
    $("#payType_Bank .paysList li").click(function () {
        setPayURL($(this).find("input").val());
        $(".selectPay").removeClass("selectPay");
        $(this).addClass("selectPay");
        $("#btnPay").removeAttr("disabled");
    });
    $("#payType_Online .paysList li").click(function () {
        setPayURL($(this).find("input").val());
        $(".selectPay").removeClass("selectPay");
        $(this).addClass("selectPay");
        $("#btnPay").removeAttr("disabled");
    }).first().click();
});
function setProductAttention() {
    if (productType == "2") {
        var showProductAttention = $("#hidShowProductAttention").val();
        if (showProductAttention == "3") {
            $("#divSpecialProductAttention .con").html("该订单修改过价格，请确认后再支付<br/><br/>" + $("#hidPublishRefundRule").val());
            $("#btnSpecialProductConfirm").addClass("close");
            $("#specialProductAttention").click();
        } else if (showProductAttention == "2") {
            // 申请时，不能支付
            $("#divSpecialProductAttention .con p").after($("#hidPublishRefundRule").val());
            $("#btnSpecialProductConfirm").attr("href", getDetailUrl());
            $("#specialProductAttention").click();
        } else if (showProductAttention == "1") {
            // 能支付时
            //$("#divSpecialProductAttention .con").html($("#hidPublishRefundRule").val());
            //$("#btnSpecialProductConfirm").addClass("close");
            //$("#specialProductAttention").click();
        }
    }
}
function setPayURL(bankInfo) {
    var target = '../../OrderModule/Purchase/Pay.aspx?id=' + businessId + '&type=' + businessType + '&bank=' + encodeURI(bankInfo) + '&userName=' + userName;
    $("#btnPay").attr("href", target);
}
function clearPayUrl() {
    $("#btnPay").removeAttr("href");
}
function pay() {
    if ($("#divPayTypes .payNavCur").hasClass("payType_poolpay")) {
        payWithPoolpayType();
    } else {
        $("#payAttention").click();
    }
}
function payWithPoolpayType() {
    var account = $.trim($("#txtPoolPayAccount").val());
    var password = $("#txtPoolPayPassword").val();
    if (account == '') {
        alert("请输入账号");
        return false;
    }
    if (password == '') {
        alert("请输入密码");
        return false;
    }
    $("#btnPay, #btnCancel").hide();
    $("#btnWaiting").show();
    $("#divPayTypes, #divPayTypeDetails").attr("disabled", "disabled");
    sendPostRequest("/OrderHandlers/Order.ashx/Pay",
         JSON.stringify({ "id": businessId, "type": businessType, "account": account, "password": password }),
        function (result) {
            $("#paySuccessAmount").html('您已经付款<span class="price">' + payAmount + '</span>元');
            $("#paySuccess").click();
        }, function (error) {
            if (error.status == 300) {
                if (error.responseText == "\"账户余额不足\"") {
                    $('#payFaild').click();
                    if ($("#txtPoolPayAccount").val() != $("#PayPoolBindAccount").val()) {
                        $("#notSame3,#notSame2,#notSame1").remove();
                        $("#liPayAccount").text($("#txtPoolPayAccount").val());
                        $("#closeHandler").attr("href", "javascript:void(0);").val("确定").addClass("close");
                    }

                } else {
                    $("#payDelayed").click();
                    $("#liErrorTip").html(error.responseText);
                    $("#closeHandler").attr("href", "javascript:void(0);").val("确定").addClass("close");
                    if (error.responseText.indexOf("密码错误") > -1 || orderIsImport) {
                        $("#ErrorOption").hide();
                    } else {
                        $("#ErrorOption").show();
                    }
                }
            } else {
                alert("支付失败，请联系平台工作人员");
            }
            $("#btnPay, #btnCancel").show();
            $("#btnWaiting").hide();
            $("#divPayTypes, #divPayTypeDetails").removeAttr("disabled");
        });

}
function getDetailUrl() {
    var page = "";
    if (businessType == "2") {
        page = "../../OrderModule/Purchase/PostponeApplyformDetail.aspx";
    }else if (businessType == "3") {
        page = "../SmsModule/SMSSendRecord.aspx";
    } else {
        page = "../../OrderModule/Purchase/OrderDetail.aspx";
    }
    //var page = businessType == "2" ? "PostponeApplyformDetail.aspx" : "OrderDetail.aspx";
    return page + '?id=' + businessId;
}
function getQueryListUrl() {
    var url = "";
    if (businessType == "2") {
        url = "../../OrderModule/Purchase/ApplyformList.aspx";
    }else if (businessType == "3") {
        url = "../SmsModule/SMSSendRecord.aspx";
    } else {
        url = "../../OrderModule/Purchase/OrderList.aspx";
    }
    //return businessType == "2" ? "ApplyformList.aspx" : "OrderList.aspx";
    return url;
}
function ReturnUrl(type) {
    sendPostRequest("/OrganizationHandlers/ReturnUrl.ashx/GetUrl", JSON.stringify({ "accountNo": $.trim($("#PayPoolBindAccount").val()), "type": type }), function (result) { /* window.top.location = retult; */window.open(result); });
}