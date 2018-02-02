var providerOfficeNo;
var PolicyType;
var selectedPolicy;
var PolicyId;
var PolicyOwner;
var OriginalPolicyIsSpecial;
var NeedCheckOfficeNOAuth;
$(Init);

function Init() {
    PolicyType = $("#hidPolicyType").val();
    OriginalPolicyIsSpecial = $("#OriginalPolicyIsSpecial").val() == "1";
    $("#btnChageProvider").click(function () {
        if ($("#PolicyContainers input:checked").size() == 0) {
            alert("请先选择政策");
            return false;
        }
        if ($("#hfdPnrImport").val() == "true" && $("#hfdOfficeIsAuth").val() == "1" && $("#hfdAgreeAuth").val() == "true") {
            $("#officeNo").html($("#hfdNeedAuthOffice").val());
            $("#ChangeProvider").css("z-index", 1900);
            $("#divOption").click();
            $("#btnSure").removeClass("close");
            $("#Tips h4 a").removeClass("close");
        } else {
            var parameters = JSON.stringify({ "policyId": PolicyId, "policyType": selectedPolicy, "provider": PolicyOwner, "officeNo": providerOfficeNo, "orderId": $("#lblOrderId").text(), needAUTH: NeedCheckOfficeNOAuth, forbidChangePNR: $("#cbForbidChangePNR").is(":checked") });
            var method = "/FlightHandlers/ChangeProvider.ashx/ChangeProviderETDZ";
            sendPostRequest(method, parameters, function (data) {
                window.location.href = '/OrderModule/Operate/OrderDetail.aspx?id=' + $("#lblOrderId").text() + "&choise=0";
            }, function (error) {
                $(".close").click();
                alert(error.responseText);
            });

        }
    });
    $("#cbForbidChangePNR").click(FliterPolicyByChangePNR);

    $("#Tips h4 a").click(function () {
        $("#ChangeProvider").css("z-index", 9999);
        $("#divOption").hide();
        $("#Tips").hide();
    });
    $("#btnSure").click(function () {
        $("#divOption").hide();
        $("#Tips").hide();
        var parameters = JSON.stringify({ "policyId": PolicyId, "policyType": selectedPolicy, "provider": PolicyOwner, "officeNo": providerOfficeNo, "orderId": $("#lblOrderId").text(), needAUTH: NeedCheckOfficeNOAuth, forbidChangePNR: $("#cbForbidChangePNR").is(":checked") });
        var method = "/FlightHandlers/ChangeProvider.ashx/ChangeProviderETDZ";
        sendPostRequest(method, parameters, function (data) {
            window.location.href = '/OrderModule/Operate/OrderDetail.aspx?id=' + $("#lblOrderId").text() + "&choise=0";
        }, function (error) {
            $(".close").click();
            alert(error.responseText);
        });
    });
    //关闭换出票方遮罩层并且释放锁定的订单
    $("#btnCacelLock,#btnCacelLock1").click(function () {
        ControlPageHeight(); //重新计算页面高度
        $("#ChangeProvider,#mask").hide(); //手动关闭遮住层避免事件冲突
        unLockOrder();
        return false;
    });
}

function FliterPolicyByChangePNR() {
    //当勾选不允许换编码出票时候过滤需要换编码的政策
    if ($("#cbForbidChangePNR").attr("checked") == "checked") {
        $(".policys").has("input[value='false']").hide();
        if ($("#PolicyContainers .policys").has("input[type='radio']").first().is(":hidden")) {
            $("#PolicyContainers input[type='radio']:visible").first().trigger("click");
        }
    } else {
        $(".policys").has("input[value='false']").show();
    }
}

//换出票方
function ChangeProvider() {
    //锁定订单
    sendPostRequest("/FlightHandlers/ChangeProvider.ashx/LockOrder", getLockParameters(), function (result) {
        if (result && result.length > 0) {
            alert(result);
        } else {
            $("#changeProviderPop").click();
            RefrashPolicy();
        }
    });
}
//刷新政策
function RefrashPolicy() {
    PolicyType = $("#hidPolicyType").val();
    $("#policys").remove();
    matchPolicies();
}
function getLockParameters() {
    var orderId = $.trim($("#lblOrderId").text());
    var parameters = JSON.stringify({ "orderId": orderId });
    return parameters;
}
//解锁订单
function unLockOrder() {
    sendPostRequest("/FlightHandlers/ChangeProvider.ashx/UnLockOrder", getLockParameters());
}
function matchPolicies() {
    var parameters = { "policyType": PolicyType, "policyCount": 10, "policyOwner": $("#hidOriginalPolicyOwner").val()
        , rate: $("#rate").text(), showTip: !OriginalPolicyIsSpecial
    };
    sendPostRequest("/FlightHandlers/ChangeProvider.ashx/QueryPolicies", JSON.stringify(parameters), function (data) {
        if (data.length == 0) {
            $("#PolicyContainers").html("<div class=\"box\">没有匹配到相关政策结果</div>");
            $("#btnChageProvider").hide();
        } else {
            BindPolicys(data);
        }
    }, function (error) {
        $("#PolicyContainers").html("查询政策失败<br />失败原因:" + error.responseText);
    });
}

function BindPolicys(policyData) {
    $(".policys").remove();
    $("#nomalPolicyTmpl").tmpl(policyData, { TranslateRelationName: TranslateRelationName }).appendTo("#PolicyContainers");
    setTimeout(function () {
        $("#PolicyContainers input[type='radio']").first().trigger("click");
        FliterPolicyByChangePNR();
    }, 400);
}

function chooseNormalPolicy(sender, policyId, policyOwner, policyType, officeNo, settleAmount, commission, needCheckOfficeNOAuth, relation) {
    if (relation == 2) {
        alert("该政策是采购的上级发布的政策，不允许指向给该出票方，请重新选择");
        return false;
    }
    $(".provision").hide();
    $(sender).parent().parent().next().show();
    $(".curr").removeClass("curr");
    $(sender).parents("table").addClass("curr");
    choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth);
}

function choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth) {
    PolicyId = policyId;
    PolicyOwner = policyOwner;
    $("#hidPolicyType").val(policyType);
    $("#profitInfo").show();
    if (needCheckOfficeNOAuth) {
        $("#hfdOfficeIsAuth").val("1");
    } else {
        $("#hfdOfficeIsAuth").val("0");
    }
    $("#hfdNeedAuthOffice").val(officeNo);
    selectedPolicy = policyType;
    providerOfficeNo = officeNo;
    NeedCheckOfficeNOAuth = needCheckOfficeNOAuth;
}

function TranslateRelationName(relationType) {
    switch (relationType) {
        case 1: return "";
        case 4: return "<a href='#' class='sup-p fr'>上级</a>";
        default: return "";
    }
}