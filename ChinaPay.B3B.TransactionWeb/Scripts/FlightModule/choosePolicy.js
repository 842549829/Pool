var defalutPolicyType;
var policiesContainer;
var providerOfficeNo;
var passengerCount;
var passengerType;
var flightCount;
var source;
var AirportFee = 0;
var BAF = 0;
var NeedCheckOfficeNOAuth = false;
var selectedPolicy;
var SpecialPoliyType = "8";
var BargianPolicyType = "4";
var defaultPolicyCount = 5, maxPolicyCount = 10;
var RulesShowed = false; //只针对特价 
var isImport = false;
var renderTicketPrice = true;
var priceUnit = "元";
var isFreeTicket = false;
var HasSubsidized = false;
var IsUsePatPrice = false;
var IsChildTicket = false;
var needExternalPolicy = true;
var forbidChangePNR = false;  //政策是否允许设置  能否换编码出票
$(function ()
{
    setTimeout(function ()
    {  //订单超时重新需要重新匹配政策
        $("#btnProduce,#btnProduce1").remove();
        $("#Refresh").show();
    }, 60000);
    passengerCount = $("#hidPassengerCount").val();
    passengerType = $("#hidPassengerType").val();
    IsChildTicket = passengerType == "1";
    flightCount = $("#hidFlightCount").val();
    selectedPolicy = defalutPolicyType = $("#hidDefaultPolicyType").val();
    isFreeTicket = $("#hidisFreeTicket").val() == "1";
    source = $("#hidSource").val();
    isImport = source == "2" || source == "3";
    needExternalPolicy = !IsChildTicket && (defalutPolicyType == "2" || defalutPolicyType == "4" || defalutPolicyType == "6");
    if (!needExternalPolicy)
    {
        $("#ExternalPolicyBox").hide();
    }
    if ($("#IsUsePatPrice").size() > 0)
    {
        IsUsePatPrice = $("#IsUsePatPrice").val() == "1";
    }

    if (passengerType == "1")
    {
        $("#ChildTip").show();
    }

    $("#notAgree").click(function ()
    {
        $("#tips,#mask").hide();
    });

    policiesContainer = $("#nomalPolicyConatiner");
    $(".close").click(function ()
    {
        $("#btnProduce1").show();
        $("#btnProduce").hide();
    });
    $("#btnProduce1").click(function ()
    {
        $("#btnProduce1").hide();
        $("#btnProduce").show();
        if (selectedPolicy == BargianPolicyType)
        {
            showTip(null, false, 3, function ()
            {
                setTimeout(function ()
                {
                    $("#btnProduce").unbind("click");
                    BindOrderOption();
                    $("#btnProduce").trigger("click");
                }, 100);
            });
        } else if (selectedPolicy == SpecialPoliyType)
        {

            showTip(null, false, 2, function ()
            {
                setTimeout(function ()
                {
                    $("#btnProduce").unbind("click");
                    BindOrderOption();
                    $("#btnProduce").trigger("click");
                }, 100);
                //$("#notAgree").click(function ()
                // {
                //   $("#btnQueryFlight").click();
                //});
            });
        } else
        {
            $("#btnProduce").unbind("click");
            BindOrderOption();
            $("#btnProduce").trigger("click");
        }
    });
    matchPolicies();
    $("#btnQueryMorePolicies").click(function ()
    {
        $(".hiddenPolicies").show();
        hideMorePolicyButton();
    });
    var fare = $.trim($("#divPassengers table tr").eq(1).children().eq(6).text());
    var match = /(\d+)\/(\d+)/.exec(fare);
    if (match.length > 0)
    {
        AirportFee = parseInt(match[1]);
        BAF = parseInt(match[2]);
    }

    $("#btnProduce").val((source == "3" || source == "4") ? "提交申请" : source == "5" ? "提    交" : "生成订单");
});


function ShowPNRAUTHTip(pnr, callback)
{
    $("#PNRCode").html(pnr);
    $("#showPop").click();
    $("#SureAUTH").click(function ()
    {
        if ($("#agree").is("checked"))
        {
            callback(1);
        } else if ($("#agreeAUTH").is("checked"))
        {
            callback(2);
        }
        $(".close").click();
    });
}

function Redirect(url)
{
    window.location.href = '/Agency.htm?target=' + encodeURIComponent('/Index.aspx?redirectUrl=' + encodeURIComponent(url));
}

function matchPolicies()
{
    var parameters = { "policyType": defalutPolicyType, "policyCount": maxPolicyCount, "source": source, "policyOwner": $("#hidOriginalPolicyOwner").val(), needSubsidize: source == "1" || source == "2", IsUsePatPrice: IsUsePatPrice };
    sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/QueryPolicies", JSON.stringify(parameters), function (data)
    {
        if (data.length == 0)
        {
            if (defalutPolicyType != SpecialPoliyType && passengerType == "0")
            {
                parameters.policyType = SpecialPoliyType;
                sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/QueryPolicies", JSON.stringify(parameters),
                 function (rsp)
                 {
                     if (rsp.length != 0) BindPolicys(null, rsp); else
                     {
                         $("#PolicyList").html("没有相关政策");
                         //$("#btnProduce,#btnProduce1").hide();
                         hideMorePolicyButton();
                         BindPolicys(null, null);
                     }
                     LoadRecommand();
                 });
            }
            else
            {
                $("#PolicyList").html("没有相关政策");
                $("#btnProduce,#btnProduce1").hide();
                hideMorePolicyButton();
                LoadRecommand();
                BindPolicys(null, null);
            }
        } else
        {
            if (defalutPolicyType == SpecialPoliyType)
            {
                BindPolicys(null, data);
                LoadRecommand();
            } else
            {
                parameters.policyType = SpecialPoliyType;
                if (flightCount == 1 && $("#hidIsTeam").val() != "1" && passengerType == "0")
                {
                    sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/QueryPolicies",
                     JSON.stringify(parameters), function (rsp)
                     {

                         BindPolicys(data, rsp);
                         LoadRecommand();
                     });
                } else
                {
                    BindPolicys(data, null);
                    LoadRecommand();
                }
            }

        }
        if (data.length <= defaultPolicyCount)
        {
            hideMorePolicyButton();
        }


    }, function (error)
    {
        showMatchPolicyMessage("查询政策失败<br />失败原因:" + error.responseText);
        hideMorePolicyButton();
    });
    function showMatchPolicyMessage(message)
    {
        var messageHtml = '<tr><td colspan="' + (defalutPolicyType == SpecialPoliyType ? 5 : 9) + '">' + message + '</td></tr>';
        policiesContainer.html(messageHtml);
    }
}

function hideMorePolicyButton()
{
    $("#btnQueryMorePolicies").hide();
}


//将政策价格同步到航段票面价
function FillYBPrice()
{ 
    //填充政策的票面价格
    var selectPolicyPrice = $("#PolicyList tr,#ExternalPolicyBox tr").has("input:checked").children().first().find("input").val();   //当前选择政策的票面价
    $(".Price").each(function (index, item)
    {
        if (selectPolicyPrice == "")
        {
            return;
        }
        var that = $(item);
        that.addClass("ZeorPrice");
        that.text(renderTicketPrice ? selectPolicyPrice : "出票后可见");
        that.next().next().text(BAF + AirportFee + parseInt(selectPolicyPrice, 10));
    });
}

function TranslateRelationName(relationType)
{
    switch (relationType)
    {
        case 1: return "";
        case 4: return "<a href='#' class='sup-p fr'>上级</a>";
        default: return "";
    }
}

