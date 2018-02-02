<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChoosePolicyWithImport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.ChoosePolicyWithImport" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Register Src="~/FlightReserveModule/Flights.ascx" TagPrefix="uc" TagName="Flights" %>
<%@ Register TagPrefix="uc1" TagName="FlightQueryNew" Src="~/FlightReserveModule/FlightQueryNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
    <link href="../Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/airflag.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
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
        .orderId
        {
            color:blue;
            text-decoration: underline;
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
                请注意，您所预定的是儿童机票（编码），请在支付后尽快将儿童身份证复印件或户籍证明传真至<asp:Label runat="server" ID="lblFax"></asp:Label>并告知工作人员您的订单号</p>
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
                                <col class="w15" />
                                <col class="w10" />
                                <col class="w10" />
                                <col class="w15" />
                                <col class="w15" />
                                <col class="w15" />
                                <col class="w10" />
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
            <div class="column table" id="ExternalPolicyContainer" style="display:none;margin-top: 50px;">
                <ul class="policyInfo">
                    <li><a href="javascript:;">更多政策<span class="price_ico price">￥</span><span
                        id="exterPolicyLowerPrice" class="price b"></span></a></li>
                </ul>
                <div class="policyInfoBox" id="ExternalPolicyBox">
                    <p style="width: 300px; height: 30px; margin: 10px auto;">
                        <img src="/images/progress.gif"></p>
                    <p class="obvious1 txt-c">
                        正在为您匹配更多政策，时间可能稍长，请您耐心等待</p>
                </div>
            </div>
            <div class="btns closes" style="display: none">
                    <input type="button" class="btn class2" id="btnQueryMorePolicies" value="查看更多政策" /></div>
                    
       <div class="column txt-r" id="purchaserAllow">
        <label class="obvious-red"><input type="checkbox" class="mar-r" id="chbChangePNRChoise" />不允许换编码出票</label>
        <p class="obvious1 mar-t">如果您已经选择了带有“换编码出票”提示的政策，请不要勾选此项，否则无法提交订单；稍后请别忘了对供应商的OFFICE号授权</p>
    </div>
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
    <asp:HiddenField runat="server" ID="hidHBtip" Value="您的订单提交后，需要供应商为您进行座位申请并会尽快为您进行座位确认，当供应商为您确认座位后您就可以直接支付并等待供应商为您出票，感谢您对平台的支持。" />
    <asp:HiddenField runat="server" ID="hidSQtip" Value="如遇旅客所需航班无座位情况下，需在航班截止办理登机手续后到机票候补柜台申请补票，使用身份证到候补柜台办理登机手续，若无空座则顺延至下一航班（空座泛指其他购票人未进行登机时留下的空座)" />
    <input type="hidden" id="hidPolicyId" />
    <input type="hidden" id="hidPolicyOwner" />
    <input type="hidden" id="hidPolicyType" />
    <!--特殊票授权提示 -->
    <a data="{type:'pop',id:'OfficeNOTip'}" style="display: none;" id="showPop"></a>
    <div class="layer3" id="OfficeNOTip" style="display: none;">
        <h4>
            授权提醒： <a href="javascript:void(0)" class="close" style="display: none;">关闭</a>
        </h4>
        <p class="layer3Tips">
            提示：因您未及时对OFFICE号进行授权而造成的NO位或拒单，平台不承担责任。
        </p>
        <div class="layer3Content">
            <input type="radio" value="1" name="choise" id="agree" checked="checked" />
            <label for="agree">
                本政策需要授权，授权代码付款后可见，请付款后在已支付待出票分类进行查看.</label>
            <%--<p class="code">
                <span id="AUTHCommand">RMK TJ AUTH <span id="PNRCode"></span></span><a href="javascript:copyToClipboard($('#AUTHCommand').text())">
                    复制</a>
            </p>--%>
        </div>
        <div class="layer3Content">
            <input type="radio" name="choise" value="2" id="agreeAUTH" />
            <label for="agreeAUTH">
                我未对编码授权，但同意换编码出票</label>
        </div>
        <div class="layer3Content">
            <input type="radio" id="NoAUTHAgree" name="choise" />
            <label for="NoAUTHAgree">
                我要重新选择不需要授权的政策重新生成订单</label>
        </div>
        <div class="layer3Btns">
            <a href="javascript:void(0)" class="layerbtn btn1 fl" id="SureAUTH">确定</a> <a href="javascript:void(0)"
                class="layerbtn btn2 fr close">取消</a>
        </div>
    </div>
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
    <a data="{type:'pop',id:'SamePNRTip'}" style="display: none;" id="SamePNRTipTrigger"></a>
    <div class="layer3" id="SamePNRTip" style="display:none;">
			<h4>操作提示：<a class="close">关闭</a></h4>
			<p class="layerTips">
				您所导入的PNR在过去24小时中已经生成过订单，订单号为：<a href="#" class="orderId"></a><br />
			    您可以点击蓝色链接查看订单详情或进入订单查询中查看订单，如有疑问清致电<br />
                <asp:Label runat="server" ID="lblServicePhone"></asp:Label>，感谢您对<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal> 机票交易平台的支持.
			</p>
			<div class="layerBtns text-c">
				<a href="/Index.aspx?redirectUrl=/FlightReserveModule/PNRImport.aspx" class="btn class1">返回导入编码</a> &nbsp;
				<a href="#" class="btn class1" id="viewOrder">查看订单详情</a> &nbsp;
				<a class="btn class2 close">取消</a>
			</div>
		</div>
    </form>
    <script type="text/x-jquery-tmpl" id="PolicyTmpl">
        <tr{{if IsRepeated}} class="more" {{/if}}>
    					<td style="" >
    						<span class="flag flag_${AirlineCode}">
                                <span>${AirlineName}</span>
                                <br>
                                <span>${AirlineCode}${FlightNo}</span>
                            </span>
    					</td>
    					<td>
    						<span class="b">${TakeoffTime}</span>
    						<br>
    						<span>${LandingTime}</span>
    					</td>
    					<td>
    						<span>${Departure.AirportCode}${Departure.AbbrivateName}机场${Departure.Terminals}</span>
    						<br>
    						<span>${Arrival.AirportCode}${Arrival.AbbrivateName}机场${Arrival.Terminal}</span>
    					</td>
    					<td>
    						<span>${SeatCount}</span>
    					</td>
    					<td>
    						<span class="fontBlodRed">￥${LowerPrice}</span>
    					</td>
    					<td>
    						<span>${OrderSuccessRate}</span>
    					</td>
    					<td>
    						<span>${SuccessOrderCount}</span>
    					</td>
    					<td>
    						<span class="grade">
                                <a title="${gradeFirst}.${gradeSecond}分" class="grade_${gradeFirst}d${gradeSecond}" href="#">${gradeFirst}.${gradeSecond}分</a>
                            </span>
    					</td>
    					<td>
    						<button type="button" class="SR_btn" onclick="ReserveFlight('${Departure.Code}', '${Arrival.Code}', '${TakeoffTime}', '${LandingTime}', '${FlightDate}', '${AirlineCode}', '${AirlineName}', '${FlightNo}', '${AirCraft}', ${YBPrice}, ${AirportFee}, ${BAF}, 0, '', ${LowerPrice},${SeatCount}, ${AdultBAF}, ${ChildBAF}, '${Departure.Name}', '${Departure.AirportCode}', '${Departure.Terminal}', '${Arrival.Name}', '${Arrival.AirportCode}', '${Arrival.Terminal}','${policyId}', ${policyType}, '${publisher}', '${officeNo}', ${needAUTH},this,'${Condition}','${spType}','${specialPolicy}','${PolicyDesc}')">{{if needApplication}}申请{{else}}预订{{/if}} </button>
                            <br/><span>
                                {{html WarnInfo}}
                            </span>
                           
                            <div style='display:none;'>{{tmpl(EIList) "#rules"}}</div>
    					</td>
    				</tr>
    </script>
    <script type="text/x-jquery-tmpl" id="nomalPolicyTmpl">

        <table class='PolicyItem'>
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
                    <td>${TicketType}(${PolicyTypes})  <input type='hidden' value='${setChangePNREnable}' /> </td>
                    <td>
                    <input type="radio" onclick="chooseNormalPolicy(this,'${PolicyId}','${PolicyOwner}','${PolicyType}','${OfficeNo}',${SettleAmount},${Commission},${NeedAUTH},'${Condition}',${HasSubsidized},${setChangePNREnable})" name="policies" />
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
        
        <table class="curr PolicyItem">
                <colgroup>
                       <col class="w15" />
                       <col class="w10" />
                       <col class="w10" />
                       <col class="w15" />
                       <col class="w15" />
                       <col class="w15" />
                       <col class="w10" />
                       <col class="w10" />
                </colgroup>
                <tbody><tr>
                    <td><input type='hidden' value='${Fare}' />  ${SettleAmount}
                    {{html $item.TranslateRelationName(RelationType)}} </td>
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
<script src="../Scripts/Global.js?2013011601" type="text/javascript"></script>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="../Scripts/FlightModule/choosePolicy.js?2013022501" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/widget/template.js" type="text/javascript"></script>
<script src="../Scripts/widget/d.tabs.js" type="text/javascript"></script>
<script src="../Scripts/FlightModule/ChoosePolicyWithImport.aspx.js?2013030701" type="text/javascript"></script>
