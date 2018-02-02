<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="FlightQueryResult.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FlightQueryResult" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Register Src="FlightQueryNew.ascx" TagName="FlightQueryNew" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link rel="stylesheet" href="/Styles/skin.css" />
    <link href="/Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/airflag.css" rel="stylesheet" type="text/css" />
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
                <div class="clearfix" id="flightHeader">
                    <h4>
                        选择航班 <span class="departureName num"></span>→ <span class="arrivalName num"></span>
                        (单程:<span id="pickDate"></span>)
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
                <%-- 航班结果信息 --%>
                <div class="column">
                    <div class="order-info">
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
<script src="../Scripts/Global.js?20121115" type="text/javascript"></script>
<script src="../Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script src="../Scripts/FlightModule/flightQuery.js?20130124002" type="text/javascript"></script>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    $(function ()
    {
        $(".departureName").text($("#hidDepartureName").val());
        $(".arrivalName").text($("#hidArrivalName").val());
        $("#pickDate").text($("#hidDate").val());
        queryFlights("11", $("#hidFlightValidyMinutes").val(), "divSearching", "divContent", "divDateTitle", "FlightList", "divError");

        LoadTipEvents();
    });

    function LoadTipEvents() {
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
            };
        }).live("mouseleave", function ()
        {
            $(".tgq_box1").addClass("tgq_box").removeClass("tgq_box1");
            $(".tgq_box").addClass("hidden");
        });
    }
</script>
