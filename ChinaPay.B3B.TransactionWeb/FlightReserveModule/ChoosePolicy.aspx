<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChoosePolicy.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.ChoosePolicy" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Register Src="~/FlightReserveModule/Flights.ascx" TagPrefix="uc" TagName="Flights" %>
<%@ Register TagPrefix="uc1" TagName="FlightQueryNew" Src="~/FlightReserveModule/FlightQueryNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="../Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/ticket.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/airflag.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tab-a .tab-con
        {
            border: none;
            padding: 0;
        }
        
        .tab
        {
            overflow: visible;
        }
        .provision
        {
            color: #999999;
        }
        .more
        {
            display: none;
        }
    </style>

<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader" FlightText="我的机票"></uc:Header>
        <div id="bd">
            <%--快捷查询航班--%>
            <div class="box-a">
                <%--<uc:FlightQuery runat="server" ID="ucFlightQuery"></uc:FlightQuery>--%>
                <uc1:FlightQueryNew ID="ucFlightQueryNew" runat="server" />
            </div>
            <%--航段信息--%>
            <div class="column" style="padding-top: 25px;">
                <uc:Flights runat="server" ID="ucFlights"></uc:Flights>
                <%--进度条--%>
                <div class="corner">
                    <div class="step">
                        <div class="line-bg">
                        </div>
                        <div style="width: 50%;" class="active-line-bg">
                        </div>
                        <i class="active">●<span>1</span></i> <i class="active" style="left: 48%;">●<span>2</span></i>
                        <i style="left: 174px">●<span>3</span></i>
                        <label style="left: -20px;">
                            选择航班</label>
                        <label style="left: 40%">
                            补全信息</label>
                        <label style="left: 75%">
                            付款完成订单</label>
                    </div>
                </div>
            </div>
            <%-- 乘机人信息 --%>
            <div class="column table">
                <div class="hd">
                    <h2 class="passengersInfo">
                        乘机人信息</h2>
                </div>
                <div runat="server" id="divPassengers" class="passengersInfoBox1">
                </div>
            </div>
            <p id="ChildTip" style="padding: 10px 0 10px 40px; width: auto; display: none;" class="layer3Tips">
                请注意，您所预定的是儿童机票（编码），请在支付后尽快将儿童身份证复印件或户籍证明传真至<asp:Label runat="server" ID="lblFax"></asp:Label> 并告知工作人员您的订单号</p>
            <div class="column table" id="specialRecommend" style="display: none;">
                <div class="specialRecommendBg">
                    <div class="specialRecommend tab tab-a">
                        <h4>
                            特惠推荐</h4>
                        <ul class="clearfix" id="suggestDates">
                            <li id="prev"><a href="#prevDay"><span>前一天</span><span id="prevDate">10-16</span><span
                                id="prevDatePrice" class="price b">￥1100</span></a> </li>
                            <li id="current"><a href="#currentDay"><span>当天</span><span id="currentDate">10-16</span><span
                                id="currentDatePrice" class="price b">￥900</span></a> </li>
                            <li id="next"><a href="#nextDay"><span>后一天</span><span id="nextDate">10-16</span><span
                                id="nextDatePrice" class="price b">￥980</span></a> </li>
                        </ul>
                        <div id="prevDay" class="tab-con">
                            <table id="prevTable">
                                <tr>
                                    <th>
                                        航班号
                                    </th>
                                    <th>
                                        起抵时间
                                    </th>
                                    <th>
                                        起抵机场
                                    </th>
                                    <th>
                                        座位数
                                    </th>
                                    <th>
                                        结算价
                                    </th>
                                    <th>
                                        订单成功率
                                    </th>
                                    <th>
                                        已出票张数
                                    </th>
                                    <th>
                                        信誉等级
                                    </th>
                                    <th>
                                        操作
                                    </th>
                                </tr>
                                <tbody id="prevDayContrainer">
                                </tbody>
                            </table>
                        </div>
                        <div id="currentDay">
                            <table id="currentTable">
                                <tr>
                                    <th>
                                        航班号
                                    </th>
                                    <th>
                                        起抵时间
                                    </th>
                                    <th>
                                        起抵机场
                                    </th>
                                    <th>
                                        座位数
                                    </th>
                                    <th>
                                        结算价
                                    </th>
                                    <th>
                                        订单成功率
                                    </th>
                                    <th>
                                        已出票张数
                                    </th>
                                    <th>
                                        信誉等级
                                    </th>
                                    <th>
                                        操作
                                    </th>
                                </tr>
                                <tbody id="currentDayContainer">
                                </tbody>
                            </table>
                        </div>
                        <div id="nextDay">
                            <table id="nextTable">
                                <tr>
                                    <th>
                                        航班号
                                    </th>
                                    <th>
                                        起抵时间
                                    </th>
                                    <th>
                                        起抵机场
                                    </th>
                                    <th>
                                        座位数
                                    </th>
                                    <th>
                                        结算价
                                    </th>
                                    <th>
                                        订单成功率
                                    </th>
                                    <th>
                                        已出票张数
                                    </th>
                                    <th>
                                        信誉等级
                                    </th>
                                    <th>
                                        操作
                                    </th>
                                </tr>
                                <tbody id="nextDayContainer">
                                </tbody>
                            </table>
                        </div>
                        <div class="close">
                            <a href="javascript:closeSuggest()">关闭</a>
                        </div>
                        <div class="allopen">
                            <a href="javascript:showMore()">更多航班</a>
                        </div>
                    </div>
                </div>
            </div>
            <%-- 政策信息 --%>
            <div class="column table">
                <%--                <div class="hd">
                    <h2 class="policyInfo">
                        政策信息</h2>
                </div>--%>
                <ul class="policyInfo" id="policyInfo">
                    <li id="nomal"><a href="#nomalPolicyConatiner">普通政策<span class="price_ico price">￥</span><span
                        id="nomalPrice" class="price b"></span></a> </li>
                    <li id="spical"><a href="#specialPolicyContainer">特殊政策<span class="price_ico price">￥</span><span
                        id="specialPrice" class="price b"></span></a> </li>
                </ul>
                <div class="policyInfoBox" id="PolicyList">
                    <div id="nomalPolicyConatiner" style="display: none;">
                        <table>
                            <colgroup>
                                <col class="w10">
                                <col class="w20">
                                <col class="w10">
                                <col class="w15">
                                <col class="w15">
                                <col class="w10">
                                <col class="w12">
                                <col class="w8">
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        票面价
                                    </th>
                                    <th class="txt-l">
                                        政策返点
                                    </th>
                                    <th>
                                        单张结算价
                                    </th>
                                    <th>
                                        工作时间
                                    </th>
                                    <th>
                                        废票时间
                                    </th>
                                    <th>
                                        出票效率
                                    </th>
                                    <th>
                                        政策类型
                                    </th>
                                    <th>
                                        政策选择
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="specialPolicyContainer" style="display: none;">
                        <table>
                            <colgroup>
                                <col class="w10" />
                                <col class="w10" />
                                <col class="w10" />
                                <col class="w15" />
                                <col class="w15" />
                                <col class="w15" />
                                <col class="w15" />
                                <col class="w10" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        单张结算价
                                    </th>
                                    <th>
                                        订单成功率
                                    </th>
                                    <th>
                                        已出票张数
                                    </th>
                                    <th>
                                        工作时间
                                    </th>
                                    <th>
                                        政策类型
                                    </th>
                                    <th>
                                        信誉评级
                                    </th>
                                    <th>
                                    </th>
                                    <th>
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                    

                </div>
                <div class="btns closes" style="display: none">
                    <input type="button" class="btn class2" id="btnQueryMorePolicies" value="查看更多政策" /></div>
            </div>
            <div class="column">
                <div class="large-font priceInfoBox">
                    <div class="large-font">
                        <strong>订单金额：<span class="price" id="spTotalAmount"></span></strong> <strong id="profitInfo">
                            订单总利润：<span class="price" id="spTotalProfit"></span></strong>
                    </div>
                </div>
            </div>
            <div class="btn-box btns btnBox">
                <input type="button" class="btn class1" id="btnProduce" style="display: none;" value="生成订单" />
                <input type="button" class="btn class1" id="btnProduce1" value="生成订单" />
                <input type="button" class="btn class1" style="display: none;" id="Refresh" value="政策选择超时，需要重新匹配政策，请点击刷新"
                    onclick="location.reload()" />
                <input type="button" class="btn class2" id="btnProcessing" value="处理中..." disabled="disabled"
                    style="display: none" />
            </div>
            <!--特价票退改签提示-->
            <a id="Confirm1" style="display: none" data="{type:'pop',id:'tips1'}"></a>
            <div class="layer1" id="tips1" style="display: none;">
                <p class="importantInfo">
                    您所购买的票种为： <span>[特价票]</span>
                </p>
                <p class="detail">
                    支付金额：<span class="price" id="PayPrice1"></span>元
                </p>
                <p class="detail">
                    <span class="layerTitle">出票条件:</span> <span id="Condition1"></span>
                </p>
                <h4>
                    退改签规定：</h4>
                <div class="btns" id="UserSelect1" style="margin-left: 30px;">
                    <a id="argee1" class="btn class1 larer1Btn close">我同意</a>
                    <input type="button" class="btn class2 larer1Btn" id="btnProcessing11" value="处理中..."
                        disabled="disabled" style="display: none" />
                    <a id="notAgree1" class="btn class2 larer1Btn close">不，重新选择</a>
                </div>
            </div>
            <!--特殊票退改签提示-->
            <a id="Confirm" style="display: none" data="{type:'pop',id:'tips'}"></a>
            <div class="layer1" id="tips" style="display: none;">
                <p class="importantInfo">
                    您所购买的票种为： <span>特殊票</span>- <span id="productName"></span>
                </p>
                <p class="detail">
                    请详细阅读该票种的特殊说明： <a href="#" id="productName1">什么是散充团产品</a> <a id="hbTip" style="display: none;"
                        class="tips_btn standby_ticket">什么是候补票免票？</a>
                </p>
                <p class="detail">
                    支付金额：<span class="price" id="PayPrice"></span>元
                </p>
                <%--<p class="detail">
                    行程单：不提供行程单
                </p>--%>
                <p class="detail">
                    <span class="layerTitle">描述:</span> <span id="PolicyDesc"></span>
                </p>
                <p class="detail">
                    <span class="layerTitle">出票条件:</span> <span id="Condition"></span>
                </p>
                <h4>
                    退改签规定：</h4>
                <div class="btns" id="UserSelect" style="margin-left: 30px;">
                    <a id="argee" class="btn class1 larer1Btn">我同意</a>
                    <input type="button" class="btn class2 larer1Btn" id="btnProcessing1" value="处理中..."
                        disabled="disabled" style="display: none" />
                    <a id="notAgree" class="btn class2 larer1Btn close">不，重新选择</a>
                </div>
            </div>
            <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
        </div>
    </div>
    <div class="policyConner" id="policyConner" style="display: none;">
        <a href="javascript:showSuggest()"><span>平台推荐</span><span>特惠精选</span></a>
    </div>
    <asp:HiddenField runat="server" ID="hidPassengerCount" />
    <asp:HiddenField runat="server" ID="hidPassengerType" />
    <asp:HiddenField runat="server" ID="hidDefaultPolicyType" />
    <asp:HiddenField runat="server" ID="hidSource" />
    <asp:HiddenField runat="server" ID="hidOriginalOrderId" />
    <asp:HiddenField runat="server" ID="hidOriginalPolicyOwner" />
    <asp:HiddenField runat="server" ID="hidPNRCode" />
    <asp:HiddenField runat="server" ID="hidFlightCount" />
    <asp:HiddenField runat="server" ID="hidFlishtInfos" />
    <asp:HiddenField runat="server" ID="hidIsTeam" />
    <asp:HiddenField runat="server" ID="hidisFreeTicket" Value="0" />
    <asp:HiddenField runat="server" ID="IsUsePatPrice" />
    <asp:HiddenField runat="server" ID="hidHBtip" Value="您的订单提交后，需要供应商为您进行座位申请并会尽快为您进行座位确认，当供应商为您确认座位后您就可以直接支付并等待供应商为您出票，感谢您对平台的支持。" />
    <asp:HiddenField runat="server" ID="hidSQtip" Value="如遇旅客所需航班无座位情况下，需在航班截止办理登机手续后到机票候补柜台申请补票，使用身份证到候补柜台办理登机手续，若无空座则顺延至下一航班（空座泛指其他购票人未进行登机时留下的空座)" />
    <input type="hidden" id="hidPolicyId" />
    <input type="hidden" id="hidPolicyOwner" />
    <input type="hidden" id="hidPolicyType" />
    <a data="{type:'pop',id:'cantDo'}" style="display: none;" id="showCantDoPop"></a>
    <div class="layer4" style="display: none;" id="cantDo">
        <h4>
            操作提示：<a href="javascript:void(0);" class="close">关闭</a></h4>
        <p class="layerTips">
            您所预定的航班剩余座位数小于您的编码乘客数量，无法预订该航班<br />
            建议您点击更多查看该航线的其他航班或重新查询
        </p>
        <div class="layerBtns text-c">
            <a class="btn class1" href="#" id="OtherFlight">重新查询航班</a> <a class="btn class2 close"
                href="javascript:void(0);">取消</a>
        </div>
    </div>
    <div class="tips_box hidden" style="z-index: 9999">
        <div class="tips_bd">
            <p>
                您的订单提交后，需要供应商为您进行座位申请并会尽快为您进行座位确认，当供应商为您确认座位后您就可以直接支付并等待供应商为您出票，感谢您对平台的支持。</p>
        </div>
    </div>
    <div class="sup-p_box hidden">
        <div class="tips_bd">
            <p>
                该政策是您的<span id="relationTip">上级</span>发布的内部政策请根据您的实际情况做选择。</p>
        </div>
    </div>
    </form>
    <script type="text/x-jquery-tmpl" id="nomalPolicyTmpl">

        <table>
                <colgroup>
                    <col class="w10">
                    <col class="w20">
                    <col class="w10">
                    <col class="w15">
                    <col class="w15">
                    <col class="w10">
                    <col class="w12">
                    <col class="w8">
                </colgroup>
                <tbody><tr>
                    <td><input type='hidden' value='${Fare}' />${Fare}</td>
                    <td class="txt-l"><span class="obvious fl font-a">${Rebate} (${Commission})</span>
                    {{html $item.TranslateRelationName(RelationType)}}</td>
                    <td>${SettleAmount}</td>
                    <td>${WorkingTime}</td>
                    <td>${ScrapTime}</td>
                    <td>${ETDZEfficiency}<span class="{{if IsBusy}}busy{{else}}free{{/if}}">{{if IsBusy}}忙{{else}}闲{{/if}}</span></td>
                    <td>${TicketType}(${PolicyTypes})</td>
                    <td>
                    <input type="radio" onclick="chooseNormalPolicy(this,'${PolicyId}','${PolicyOwner}','${PolicyType}','${OfficeNo}',${SettleAmount},${Commission},${NeedAUTH},'${Condition}',${HasSubsidized})" name="policies" />
                    <div style='display:none;'>{{tmpl(EIList) "#rules"}}</div>
                    </td>
                </tr>
                <tr class="provision">
                    <td class="tips" colspan="8">
                        <p>
                        {{html EI}}
                        </p>
                        <p>出票条件：{{html Condition}}</p>
                    </td>
                </tr>
            </tbody></table>
    </script>
    <script type="text/x-juqery-tmpl" id="specialPolicyTmpl">
        
        <table class="curr">
                <colgroup>
                       <col class="w10" />
                       <col class="w10" />
                       <col class="w10" />
                       <col class="w15" />
                       <col class="w15" />
                       <col class="w15" />
                       <col class="w15" />
                       <col class="w10" />
                </colgroup>
                <tbody><tr>
                    <td><input type='hidden' value='${Fare}' />  ${SettleAmount}
                     {{html $item.TranslateRelationName(RelationType)}}</td>
                    <td>${OrderSuccessRate}</td>
                    <td>${SuccessOrderCount}</td>
                    <td>${WorkingTime}</td>
                    <td>${spType}</td>
                    <td>
                        <span class="grade">
                            <a title="${gradeFirst}.${gradeSecond}分" class="grade_${gradeFirst}d${gradeSecond}" href="#">${gradeFirst}.${gradeSecond}分</a>
                        </span>
                    </td>
                    <td>{{html WarnInfo}}</td>
                    <td><input type="radio" onclick="chooseSpecialPolicy(this,'${PolicyId}','${PolicyOwner}','${PolicyType}',${SettleAmount},'${spType}','${PolicyDesc}','${specialPolicy}','{{html Condition}}',${NeedAUTH},${needApplication},'${OfficeNO}',${RenderTicketPrice},${IsFreeTicket},${IsNOSeat})" name="policies" />
                    <div style='display:none;'>{{tmpl(EIList) "#rules"}}</div>
                    </td>
                </tr>
                <tr class="provision">
                    <td class="tips" colspan="8">
                        <p>
                        {{html EI}}
                        </p>
                        <p>出票条件：{{html Condition}}</p>
                    </td>
                </tr>
            </tbody></table>

    </script>
    <script type="text/x-jquery-tmpl" id="rules">
        <p class='i'> 	<span class="layerTitle">${key}：</span> 	<span>${value}</span> </p>
    </script>
</body>
</html>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script src="../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="../Scripts/FlightModule/choosePolicy.js?2013022501" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/widget/template.js" type="text/javascript"></script>
<script src="../Scripts/widget/d.tabs.js" type="text/javascript"></script>
<script type="text/javascript">
    var minPrice = 99999;
    var recommanded = false;
    var reservePrice = '<%=PatPrice %>';
    var publicFare = '<%=PublicFare %>';


    ///绑定政策列表
    function BindPolicys(nomalPolicy, _specialPolicy)
    {
        var hasData = false;
        if (nomalPolicy && nomalPolicy.length > 0)
        {
            //$("#nomalPolicyConatiner").empty();
            $("#nomalPolicyTmpl").tmpl(nomalPolicy, { TranslateRelationName: TranslateRelationName }).appendTo("#nomalPolicyConatiner");
            var lowerPrice = $.Min(nomalPolicy, function (ele) { return parseFloat(ele.SettleAmount); });
            $("#nomalPrice").text(lowerPrice.toString());
            UpdateMinPrice(lowerPrice);
            hasData = true;
            $("#nomal,#nomalPolicyConatiner").show();
        } else
        {
            $("#nomal,#nomalPolicyConatiner").hide();
        }
        if (_specialPolicy && _specialPolicy.length > 0)
        {
            //$("#specialPolicyContainer").empty();
            $("#specialPolicyTmpl").tmpl(_specialPolicy, { TranslateRelationName: TranslateRelationName }).appendTo("#specialPolicyContainer");
            var lowerPrice1 = $.Min(_specialPolicy, function (ele) { return parseFloat(ele.SettleAmount); });
            $("#specialPrice").text(lowerPrice1.toString());
            UpdateMinPrice(lowerPrice1);
            hasData = true;
            $("#spical,#specialPolicyContainer").show();
        } else
        {
            $("#spical,#specialPolicyContainer").hide();
        }
        if (!hasData)
        {
            $(".policyInfoBox").html("未能匹配到政策");
        }
        $(".provision").hide();
        $("#PolicyList input[type='radio']").click(function ()
        {
            $(".provision").hide();
            $(this).parent().parent().next().show();
        });
        $("#PolicyList input[type='radio']").first().trigger("click");

        //勾选预订时候选定的票面价的政策
        if (reservePrice != "" || publicFare!="")
        {
            var availablePolicyContainer = $("#PolicyList tr")//.has("input[value='" + reservePrice + "']")
                                                               .has("td:contains(" + publicFare + ")"); 
            if(availablePolicyContainer.size()>0) {
                $("input[type='radio'", availablePolicyContainer.first()).click();
            }
        }


        hideMorePolicyButton();

        if ($("#nomal").is(":visible") && $("#spical").is(":visible"))
        {
            $('#policyInfo').tabs({
                event: "click",
                selected: "curr",
                callback: function (i)
                {
                    if (i == 1)
                    {
                        $("#specialPolicyContainer input[type='radio']").first().trigger("click");

                    } else
                    {
                        $("#nomalPolicyConatiner input[type='radio']").first().trigger("click");
                    }
                }
            });
        }

    }

    function LoadRecommand() {
    }

    function UpdateMinPrice(price)
    {
        if (price < minPrice) minPrice = price;
    }

    function showMore()
    {
        $(".more").toggle();
    }

   var callbackCache;
    function showTip(parameter, directRun, tipType, callback)
    {
        callbackCache = parameter || {};
        if (directRun)
        {
            callback(parameter);
            return;
        }
        if (tipType == 2)
        {//特殊票退改签提示
            $("#Confirm").click();
            $("#argee").off().one("click", function ()
            {
                $(this).hide();
                callback(callbackCache);
            });
        } else
        { //特价票退改签提示
            $("#Confirm1").click();
            $("#argee1").off().one("click", function ()
            {
                callback(callbackCache);
            });
        }
        $("#agree").click(function () { callbackCache.choise = 1; });
        $("#agreeAUTH").click(function () { callbackCache.choise = 2; });
    }

    function CheckSeatCount(seatCount, departure, arrival, flightDate)
    {
        if (passengerCount > seatCount)
        {
            $("#showCantDoPop").click();
            $("#OtherFlight").attr("href",
                     "/FlightReserveModule/FlightQueryResult.aspx?source=1&departure=" + departure + "&arrival=" + arrival + "&goDate=" + flightDate);
            return false;
        }
        return true;
    }
    $(function ()
    {
        BindDescriptionTip();
    });

    function BindDescriptionTip()
    {  //绑定申请和候补的详细内容描述浮动显示事件
        $(".tips_btn").live("mouseover", function ()
        {
            $(".tips_box").removeClass("hidden");
            $(".tips_box").css("left", $(this).offset().left - 80);
            $(".tips_box").css("top", $(this).offset().top + 10);
            var h = $(".tips_box").height();
            var top = $(".tips_box").offset().top;
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if ($(this).hasClass("standby_ticket"))
            {
                $(".tips_bd p").html($("#hidHBtip").val());
            } else
            {
                $(".tips_bd p").html($("#hidSQtip").val());
            }
            if (h + top - sor > wh)
            {
                $(".tips_box").css({ top: (top - h - 50) });
                $(".tips_box").addClass("tips_box1").removeClass("tips_box");
            };
        });
        $(".tips_btn").live("mouseout", function ()
        {
            $(".tips_box1").addClass("tips_box").removeClass("tips_box1");
            $(".tips_box").addClass("hidden");
        });
        $(".tips_btn").live("mouseout", function ()
        {
            $(".tips_box1").addClass("tips_box").removeClass("tips_box1");
            $(".tips_box").addClass("hidden");
        });
        $(".sup-p").live("mouseenter", function ()
        {
            $("#relationTip").text($.trim($(this).text().substring(0, 2)));
            $(".sup-p_box1").addClass("sup-p_box").removeClass("sup-p_box1");
            $(".sup-p_box").removeClass("hidden");
            $(".sup-p_box").css("left", $(this).offset().left - 107);
            $(".sup-p_box").css("top", $(this).offset().top + 20);
            var h = $(".sup-p_box").height();
            var top = $(".sup-p_box").offset().top;
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if (h + top - sor > wh)
            {
                $(".sup-p_box").css({ top: (top - h - 37) });
                $(".sup-p_box").addClass("sup-p_box1").removeClass("sup-p_box");
            };
        }).live("mouseleave", function ()
        {
            $(".sup-p_box").mouseenter(function ()
            {
                $(this).removeClass("hidden");
            }).mouseleave(function ()
            {
                $(this).addClass("hidden");
            });
            $(".sup-p_box1").mouseenter(function ()
            {
            }).mouseleave(function ()
            {
                $(this).addClass("hidden");
            });
            $(".sup-p_box1").addClass("hidden");
            $(".sup-p_box").addClass("hidden");
        });
    }

    function BindOrderOption()
    {
        $("#btnProduce").off().click(function ()
        {
            var that = $(this);
            var policyId = $("#hidPolicyId").val();
            var policyOwner = $("#hidPolicyOwner").val();
            var policyType = $("#hidPolicyType").val();
            if (policyId == '')
            {
                alert("请选择政策");
            } else
            {
                if (selectedPolicy == SpecialPoliyType)
                {

                    $(this).add(that).hide();
                    $("#btnProcessing,#btnProcessing1").show();
                    $("#btnProduce,#btnProduce1").hide();
                    $("#notAgree").attr("disabled", "disabled");
                    var method;
                    var parameters;
                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice,forbidChnagePNR:false });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";

                    sendPostRequest(method, parameters, function (data)
                    {
                        Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=1');
                    }, function (error)
                    {
                        if (error.status == '300')
                        {
                            alert(JSON.parse(error.responseText));
                        } else
                        {
                            alert('系统故障，请联系平台');
                        }
                        $("#btnProcessing,#btnProcessing1").hide();
                        $("#btnProduce,#btnProduce1").show();
                        $("#notAgree").removeAttr("disabled");
                        $("#btnProduce,#argee").show();
                    });
                    //});
                } else
                {
                    that.hide();
                    $("#btnProcessing").show();
                    var method;
                    var parameters;

                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: false });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";

                    sendPostRequest(method, parameters, function (data)
                    {
                        Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=1&choise=' + 0);

                    }, function (error)
                    {
                        if (error.status == '300')
                        {
                            alert(JSON.parse(error.responseText));
                        } else
                        {
                            alert('系统故障，请联系平台');
                        }
                        //$("#btnProcessing").hide();
                        //$("#btnProduce").show();

                        $("#btnProcessing,#btnProcessing1").hide();
                        $("#btnProduce,#btnProduce1").show();

                    });
                }
            }
        });
    }

    function chooseNormalPolicy(sender, policyId, policyOwner, policyType, officeNo, settleAmount, commission, needCheckOfficeNOAuth, Condition, HasSubsidized)
    {
        $(".provision").hide();
        $(sender).parent().parent().next().show();
        choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth);
        var totalAmount = Math.round((settleAmount + AirportFee + BAF) * passengerCount * 100) / 100;
        var totalProfit = Math.round(commission * passengerCount * 100) / 100;
        $("#spTotalAmount").text(fillZero(totalAmount) + priceUnit);
        $("#spTotalProfit").text(fillZero(totalProfit) + priceUnit);
        window.HasSubsidized = HasSubsidized;
        if (selectedPolicy == BargianPolicyType)
        {
            $("#UserSelect1").siblings(".i").remove();
            $("#UserSelect1").before($(sender).next().html());
            $("#PayPrice1").text(fillZero(totalAmount));
            $("#Condition1").html(Condition);
        }
    }

    function chooseSpecialPolicy(sender, policyId, policyOwner, policyType, fare, spType, PolicyDesc, specialPolicy, Condition, needCheckOfficeNoAuth, needApplication, OfficeNO, RenderTicketPrice, IsFreeTicket, IsNOSeat)
    {
        $(".provision").hide();
        $("#UserSelect").siblings(".i").remove();
        $("#UserSelect").before($(sender).next().html());
        $(sender).parent().parent().next().show();
        window.IsFreeTicket = IsFreeTicket;
        choosePolicy(policyId, policyOwner, policyType, OfficeNO, needCheckOfficeNoAuth);
        var totalAmount = Math.round((fare + AirportFee + BAF) * passengerCount * 100) / 100;
        $("#PayPrice").text(fillZero(totalAmount));
        $("#spTotalAmount").text(fillZero(totalAmount) + priceUnit);
        $("#productName").text(spType);
        if (IsNOSeat)
        {
            $("#hbTip").show();
            $("#productName").text("候补" + spType);
        }
        $("#productName1").text("什么是" + spType + "?").attr("href", "/About/help.aspx?flag=" + specialPolicy).attr("target", "_blank");
        $("#PolicyDesc").html(PolicyDesc);
        $("#Condition").html(Condition);
        $("#profitInfo").hide();
        renderTicketPrice = RenderTicketPrice;
        $("#btnProduce,#btnProduce1").val(needApplication ? "提交申请" : "生成订单");
    }
    function choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth)
    {
        $("#hidPolicyId").val(policyId);
        $("#hidPolicyOwner").val(policyOwner);
        $("#hidPolicyType").val(policyType);
        $("#profitInfo").show();
        selectedPolicy = policyType;
        providerOfficeNo = officeNo;
        NeedCheckOfficeNOAuth = needCheckOfficeNOAuth;
        setTimeout(function ()
        {
            FillYBPrice();
        }, 100);
    }

</script>
