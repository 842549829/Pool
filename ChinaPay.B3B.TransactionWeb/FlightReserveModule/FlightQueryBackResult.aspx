<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightQueryBackResult.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FlightQueryBackResult" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Register Src="FlightQuery.ascx" TagPrefix="uc" TagName="FlightQuery" %>
<%@ Register TagPrefix="uc1" TagName="FlightQueryNew" Src="~/FlightReserveModule/FlightQueryNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link rel="stylesheet" href="/Styles/skin.css" />
    <link href="/Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/airflag.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="flightForm" method="post">
    <input type="hidden" id="flightsArgs" runat="server" />
    <input type="hidden" id="policyArgs" runat="server" />
    </form>
    <form id="form1" runat="server">
    <div class="wrap" id="divContent">
        <uc:Header runat="server" ID="ucHeader" FlightText="我的机票"></uc:Header>
        <div id="bd">
            <div class="flow">
                <%--航班快速查询控件--%>
                <div class="box-a">
                    <%--<uc:FlightQuery runat="server" ID="ucFlightQuery" />--%>
                    <uc1:FlightQueryNew ID="ucFlightQueryNew" runat="server" />
                </div>
                <%-- 航段查询信息 --%>
                <div class="column">
                    <%--             <div class="hd"><h2><span class="departureName"></span>到<span class="arrivalName"></span>(往返)</h2></div>
           <div class="flight-info">
                <div runat="server" id="divGoFlightInfo"></div>
			</div>
                    --%>
                    <div id="gotoflight">
                        <span id="gotoTittle">去程航班已选择</span>
                        <table id="gotoTable">
                            <tbody>
                                <tr>
                                    <td>
                                        <span class="flightContent">
                                            <asp:literal ID="txtAirLineName" runat="server" />
                                            <strong>
                                                <asp:literal text="" ID="txtAirLineCode" runat="server" /></strong>
                                            <br>
                                            <span class="fontgray">
                                                <asp:literal text="" ID="txtAirCraftType" runat="server" />
                                                <a class="tooltip" href="javascript:void(0)">
                                                    <asp:literal text="" ID="txtAirCraft" runat="server" />
                                                    <span>说明加载中……</span> </a></span></span>
                                    </td>
                                    <td>
                                        起飞：<strong><asp:literal text="" ID="txtTakeOffTime" runat="server" /></strong><br>
                                        抵达：<span><asp:literal text="" ID="txtLanddingTime" runat="server" /></span>
                                    </td>
                                    <td>
                                        <asp:literal text="" ID="txtGoAirPort" runat="server" /><br>
                                        <asp:literal text="" ID="txtBackAirPort" runat="server" />
                                    </td>
                                    <td>
                                        单程票价：<asp:Label text="" ID="lblPrice" CssClass="fontBlodRed" runat="server" />
                                        <a class="flightEI" href="javascript:void(0)">退改签<span style="display:none"><asp:literal text="" ID="txtEI"
                                            runat="server" /></span> </a>
                                        <br>
                                        民航基金燃油：<asp:literal text="" ID="txtFare" runat="server" />元
                                    </td>
                                    <td>
                                        <input type="button" id="RePick" value="重新选择去程" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="clearfix" id="flightHeader">
                        <h4>
                            选择回程航班: <span class="departureName num"></span>→ <span class="arrivalName num"></span>
                            (<span id="pickDate"></span>)
                            <p>
                                共<span id="flightsCount" class="num"></span>个航班信息</p>
                        </h4>
                        <div class="corner">
                            <div class="step">
                                <div class="line-bg">
                                </div>
                                <i class="active">●<span>1</span></i> <i style="left: 48%;">●<span>2</span></i>
                                <i style="left: 174px">●<span>3</span></i>
                            <label style="left: -20px;">选择航班</label>
                            <label style="left: 40%">补全信息</label>
                            <label style="left: 75%">付款完成订单</label>
                            </div>
                        </div>
                    </div>
                </div>
                <%-- 航班结果信息 --%>
                <div>
                    <div class="order-info">
                        <div id="flightChooseTop">
                            <h4>
                                请选择回程航线</h4>
                        </div>
                        <div class="hd" id="divDateTitle">
                        </div>
                    </div>
                    <div id="divError">
                    </div>
                    <div id="divFlightContent">
                        <div id="FlightList">
                            <table class="TitleTable">
                                <tbody>
                                    <tr>
                                        <th class="flightCompany">
                                            航班信息
                                        </th>
                                        <th class="startTime">
                                            起抵时间
                                        </th>
                                        <th class="airport">
                                            起抵机场
                                        </th>
                                        <th class="flightPrice">
                                            结算价格(不含税费)
                                        </th>
                                        <th class="flightOperate">
                                        </th>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div id="divSearching" class="loading">
                        <img src="/Images/fly.gif" width="213px" height="32px" alt="航班查询中，请稍候..." />
                        <p>
                            航班查询中，请稍候...</p>
                    </div>
                </div>
            </div>
            <input type="hidden" id="hidFlightValidyMinutes" runat="server" />
            <input type="hidden" id="hidDepartureName" runat="server" />
            <input type="hidden" id="hidArrivalName" runat="server" />
            <input type="hidden" id="hidDate" runat="server" />
            <input type="hidden" value='<%=Request.UrlReferrer!=null?Request.UrlReferrer.OriginalString:string.Empty %>' id="ReSearchURL" />
            <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
        </div>
    </div>
        <div class="tgq_box hidden">
        <h2>退改签规定</h2>
        <div class="tgq_bd" id="Tip">
            <p><span class="b">退票规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的5%。</p>
            <p><span class="b">更改规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的10%。</p>
            <p><span class="b">签转规定：</span>允许。</p>
        </div>
    </div>

    </form>
</body>
</html>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script src="../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/FlightModule/flightQuery.js?20130124002" type="text/javascript"></script>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    $(function ()
    {
        $(".departureName").text($("#hidDepartureName").val());
        $(".arrivalName").text($("#hidArrivalName").val());
        $("#pickDate").text($("#hidDate").val());
        queryFlights("22", $("#hidFlightValidyMinutes").val(), "divSearching", "divContent", "divDateTitle", "FlightList", "divError");
        $("#RePick").click(function ()
        {
            location.href = $("#ReSearchURL").val().replace(/goDate=\d+-\d+-\d+/, "goDate=" + $("#txtGoDate").val()).replace(/backDate=\d+-\d+-\d+/, "backDate=" + $("#txtBackDate").val());
        });
        LoadTipEvents();
    });

    function LoadTipEvents()
    {
        $(".flightEI").live("mouseenter", function ()
        {
            var content = $(this).find("span").html();
            $("#Tip").html(content);
            $(".tgq_box").removeClass("hidden");
            $(".tgq_box").css("left", $(this).offset().left - 125);
            $(".tgq_box").css("top", $(this).offset().top + 15);
            var h = $(".tgq_box").height();
            var top = $(".tgq_box").offset().top;
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if (h + top - sor > wh)
            {
                $(".tgq_box").css({ top: (top - h - 35) });
                $(".tgq_box").addClass("tgq_box1").removeClass("tgq_box");
            }
            ;
        }).live("mouseleave", function ()
        {
            $(".tgq_box1").addClass("tgq_box").removeClass("tgq_box1");
            $(".tgq_box").addClass("hidden");
        });
    }

</script>
