var orderId = $("#lblOrderId").html();
var processType = $("#hidProcessType").val();
var pnrPattern = /^[a-z,A-Z,0-9]{6}$/;
var farePattern = /^[1-9][0-9]*$/;
var selectedPolicyId = "00000000-0000-0000-0000-000000000001";
var isThridRelation = $("#hidIsThirdRelation").val() == "1";
var commission = 0;
var settleAmount = 0;
function supplyResource()
{
    var pnrCode = $.trim($("#txtPNRCode").val());
    var bpnrCode = $.trim($("#txtBPNRCode").val());
    var pnrPair = pnrCode + "|" + bpnrCode;
    $("#lbPNR").text(pnrCode);
    if ($(".bianmaTip").css("display") != "none")
    {
        if (pnrCode == '' && bpnrCode == '')
        {
            alert("请输入编码(小编码或大编码至少填一个)");
            return;
        }
        if (pnrCode != '' && !pnrPattern.test(pnrCode))
        {
            alert("小编码格式错误");
            $("#txtPNRCode").select();
            return;
        }
        if (bpnrCode != '' && !pnrPattern.test(bpnrCode))
        {
            alert("大编码格式错误");
            $("#txtBPNRCode").select();
            return;
        }

    } else
    {
        if (pnrCode == '')
        {
            alert("请输入编码(小编码是必填项)");
            $("#txtPNRCode").select();
            return;
        }
        if (pnrCode != '' && !pnrPattern.test(pnrCode))
        {
            alert("小编码格式错误");
            $("#txtPNRCode").select();
            return;
        }
        if (bpnrCode != '' && !pnrPattern.test(bpnrCode))
        {
            alert("大编码格式错误");
            $("#txtBPNRCode").select();
            return;
        }
    }
    setSupplyOperationEnable(false);
    var newPrice = "-1";
    if ($("#hidIsFree").val() != "1")
    {
        $("#errorInfo").text("此产品是特殊产品，需要输入票面价");
        if (isThridRelation)
        {
            $('#chooseProviderPop').click();
            loadPolicys(orderId);
        } else
        {
            $('#showPriceInputPop').click();
        }
        return false;
    }

    pnrSubmit(pnrPair, newPrice);
}

function pnrSubmit(pnrPair, newPrice)
{
    //eval($("#JsParameter").val());
    var method = processType == "0" ? "ConfirmAndSupply" : "Supply";
    sendPostRequest("/OrderHandlers/Order.ashx/" + method, JSON.stringify({ "orderId": orderId, "pnrPair": pnrPair, "newPrice": newPrice, policyId: selectedPolicyId }), function (e)
    {
        var officeNo = e;
        if (officeNo == "")
        {
            alert("提交成功！");
            window.location.href = $("#returnUrl").val();
        }
        else if (e.length == 6)
        {
            $("#showPop").click();
            $("#pnr").text(officeNo);
            $("#btnSure").click(function ()
            {
                alert("编码提交成功！");
                window.location.href = $("#returnUrl").val();
            });
        } 

    }, function (e)
    {
        setSupplyOperationEnable(true);
        var errorMessage = e.responseText;
        if (errorMessage == "真实票面价不能高于发布价格")
        {
            drawPriceErrorInfo();
            $("#priceErrorInfo").click();
            //$("#divPriceError").show();
        } else
        {
            alert(errorMessage);
        }
    });
}


function SupplyPNR()
{
    var pnrCode = $.trim($("#txtPNRCode").val());
    var bpnrCode = $.trim($("#txtBPNRCode").val());
    var pnrPair = pnrCode + "|" + bpnrCode;

    var newPrice = isThridRelation ? $.trim($("#txtYBPrice").val()) : $.trim($("#newPrice").val());
    if (!/^\d{1,5}0$/.test(newPrice))
    {
        alert("输入价格信息不正确,票面价必须四舍五入到10位");
        return;
    }
    if ($("#IsThirdRelation").val() != "1" && parseInt(newPrice, 10) > parseInt($(".releasedFare").text(), 10))
    {
        alert("真实票面价不能高于发布价格");
        return;
    }
    $("a.close").click();
    pnrSubmit(pnrPair, parseInt(newPrice, 10));
}

function denySupply()
{
    //$("#divDeny").show();
    $("#txtDenyReason").val("");
    $("#denySupplyInfo1").click();
}
function reviseReleasedFare()
{
    $("#btnSupply").hide();
    $("#btnDeny").hide();
    $("#btnReviseReleasedFare").hide();
    $("#btnBack").hide();
    $("#divHKCode").hide();
    $("#divPNRCode").hide();
    $("#btnCommitReviseReleasedFare").show();
    $("#btnCancelReviseReleasedFare").show();
    setFareWriteableState(true);
}
function cancelReviseReleasedFare()
{
    $("#btnCommitReviseReleasedFare").hide();
    $("#btnCancelReviseReleasedFare").hide();
    setFareWriteableState(false);
    $("#btnSupply").show();
    $("#btnDeny").show();
    $("#btnReviseReleasedFare").show();
    $("#btnBack").show();
    $("#divHKCode").show();
    $("#divPNRCode").show();
}
function cancelDenySupply()
{
    //$("#divDeny").hide();
    closeLayer();
}
function commitReviseReleasedFare()
{
    var releasedFare = $.trim($("#passengers .newFare").val());
    if (releasedFare == '')
    {
        alert('请输入票面价');
        $("#passengers .releasedFare").select();
        return;
    } else if (!farePattern.test(releasedFare))
    {
        alert("票面价格式错误");
        $("#passengers .releasedFare").select();
        return;
    }
    setReviseFareOperationEnable(false);
    sendPostRequest("/OrderHandlers/Order.ashx/ReviseReleasedFare", JSON.stringify({ "orderId": orderId, "releasedFare": releasedFare }), function (e)
    {
        alert("操作成功");
        updatePrice(releasedFare);
        cancelReviseReleasedFare();
    }, function (e)
    {
        alert(JSON.parse(e.responseText));
    });
    setReviseFareOperationEnable(true);
}
function commitDenySupply()
{
    var reason = $.trim($("#txtDenyReason").val());
    if (reason == "")
    {
        alert("请输入拒绝原因");
        $("#txtDenyReason").select();
        return;
    } else if (reason.length > 200)
    {
        alert("拒绝原因不能超过200字");
        $("#txtDenyReason").select();
        return;
    } else if ($("#divDenySupply .check :checked").size() == 0)
    {
        alert("请输入拒绝原因类型");
        return;
    }
    var orderId = $("#lblOrderId").html();
    setDenyOperationEnable(false);
    var method = processType == "0" ? "ConfirmFailed" : "DenySupply";
    sendPostRequest("/OrderHandlers/Order.ashx/" + method, JSON.stringify({ "orderId": orderId, "reason": reason }), function (e)
    {
        alert("操作成功");
        window.location.href = $("#returnUrl").val();
    }, function (e)
    {
        alert(JSON.parse(e.responseText));
        setDenyOperationEnable(true);
    });
}
function resupply()
{
    hidePriceErrorMessage();
    $("#txtPNRCode").val('');
    $("#txtBPNRCode").val('');
    $("#txtPNRCode").focus();
}

$(".close").click(function ()
{
    setSupplyOperationEnable(true);
});
function setSupplyOperationEnable(enable)
{
    if (enable)
    {
        $("#btnSupply, #btnReviseReleasedFare, #btnDeny, #btnBack, #btnReleaseLockAndBack").removeAttr("disabled");
    } else
    {
        $("#btnSupply, #btnReviseReleasedFare, #btnDeny, #btnBack, #btnReleaseLockAndBack").attr("disabled", "disabled");
    }
}
function setDenyOperationEnable(enable)
{
    if (enable)
    {
        $("#btnCommitDeny, #btnCancelDeny").removeAttr("disabled");
    } else
    {
        $("#btnCommitDeny, #btnCancelDeny").attr("disabled", "disabled");
    }
}
function setReviseFareOperationEnable(enable)
{
    if (enable)
    {
        $("#btnCommitReviseReleasedFare, #btnCancelReviseReleasedFare").removeAttr("disabled");
    } else
    {
        $("#btnCommitReviseReleasedFare, #btnCancelReviseReleasedFare").attr("disabled", "disabled");
    }
}
function setFareWriteableState(writeable)
{
    $("#passengers .releasedFare").each(function (index)
    {
        if (writeable)
        {
            $(this).hide();
            var inputText = $(this).next();
            inputText.show();
            inputText.val('');
            if (index == 0)
            {
                inputText.blur(function ()
                {
                    var inputFare = $.trim($(this).val());
                    if (farePattern.test(inputFare))
                    {
                        $("#passengers .newFare").val(inputFare);
                    }
                });
            } else
            {
                inputText.attr("readonly", "readonly");
            }
        } else
        {
            $(this).show();
            $(this).next().hide();
        }
    });
}
function drawPriceErrorInfo()
{
    if ($("#divPriceError").html() == '')
    {
        var errorAttentionInfo = "<div class='success-tips box'><i class='ico'></i>";
        errorAttentionInfo += "<p>抱歉，根据您提供的PNR编码获取的价格大于实际产品发布价格，为了保证您的实际利益，可以执行以下操作：</p><ul>";
        errorAttentionInfo += "<li>[拒绝提供]后平台将直接退回该订单；</li>";
        if (processType == "0")
        {
            errorAttentionInfo += "<li>[修改价格]将修改该订单的票面价；</li>";
        }
        errorAttentionInfo += "<li>[重新提供编码]将重新提供编码；</li>";
        errorAttentionInfo += "<li>[修改政策]将跳转至政策管理页面进行政策修改。</li></ul></div>";
        errorAttentionInfo += "<div class='btns'><input type='button' class='btn class1' onclick='hidePriceErrorMessage();denySupply();' value='拒绝提供'/> ";
        if (processType == "0")
        {
            errorAttentionInfo += "<input type='button' class='btn class1' onclick='hidePriceErrorMessage();reviseReleasedFare();' value='修改价格'/> ";
        }
        errorAttentionInfo += "<input type='button' class='btn class1' value='重新提供编码' onclick='resupply();'/> ";
        errorAttentionInfo += "<input type='button' class='btn class1' value='修改政策' onclick='hidePriceErrorMessage();location.href=\"/PolicyModule/TransactionPolicy/special_policy_edit.aspx?Id=" + PolicyId + "&Type=Update\";'/></div>";
        $("#divPriceError").html(errorAttentionInfo);
    }
}
function hidePriceErrorMessage()
{
    $("#divPriceError,#mask").hide();
}
function updatePrice(releasedFare)
{
    $("#passengers .releasedFare").each(function ()
    {
        $(this).html(releasedFare);
        var airportFee = $(this).parent().next().children().eq(0).html();
        var baf = $(this).parent().next().children().eq(1).html();
        var totalPrice = parseInt(releasedFare) + parseInt(airportFee) + parseInt(baf);
        $(this).parent().next().next().children().eq(0).html(totalPrice);
    });
}

function loadPolicys(orderId)
{
    sendPostRequest("/OrderHandlers/Order.ashx/MatchedPolicy", JSON.stringify({ "orderId": orderId, pnr: $.trim($("#txtPNRCode").val()) }), function (data)
    {
        $("#PolicyContainers table").has(".provision").remove();
        $("#lbBunk").text(data.Bunk);
        $("#nomalPolicyTmpl").tmpl(data.Policys).appendTo("#PolicyContainers");
        $("#PolicyContainers input[type='radio']").first().click();
    }, function (e)
    {
        setDenyOperationEnable(true);
    });
}

function CalculateFee(Price, Commission)
{
    var payPrice = parseFloat($("#passengers .releasedFare").first().text());
    var passengerCount = $("#passengers tr").size() - 1;
//    var airportFee = parseFloat($("#passengers tr").last().children().eq(4).children().eq(0).text());
    //    var baf = parseFloat($("#passengers tr").last().children().eq(4).children().eq(1).text());
    var settleAmount = Price * (1 - Commission);

    var serviceFee = payPrice  - Price; // - baf- airportFee;
    var commissionFee = Price*Commission;
    var profit = (payPrice - settleAmount);
    RenderFee(Round(serviceFee * passengerCount, 2), Round(commissionFee * passengerCount, 2), Round(profit * passengerCount, 2));
}

function RenderFee(serviceFee, commissionFee, profit)
{
    $("#spServiceFee").text(serviceFee);
    $("#spCommissionFee").text(commissionFee);
    $("#spProfit").text("盈利" + profit);
}

    
