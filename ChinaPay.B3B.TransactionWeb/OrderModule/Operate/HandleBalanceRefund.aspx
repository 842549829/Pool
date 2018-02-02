<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandleBalanceRefund.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.HandleBalanceRefund" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagPrefix="uc" TagName="Bill" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>处理差错退款</title>
</head>
 <script src="/Scripts/jquery-1.7.2.min.js?20130522" type="text/javascript"></script>
    <link href="../../Styles/public.css?20130522" rel="Stylesheet" type="text/css" />
    <link href="../../Styles/tipTip.css?20130522" rel="stylesheet" type="text/css" />
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
     <link rel="stylesheet" href="../../Styles/icon/fontello.css?20130522" />
    <link rel="stylesheet" href="../../Styles/skin.css?20130522" />
    <link href="../../Styles/core.css?20130522" rel="stylesheet" type="text/css" />
    <link href="../../Styles/page.css?20130522" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <%--订单头部信息--%>
    <h3 class="titleBg">
        申请单详情</h3>
    <div class="column form">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">
                    申请单号
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    申请类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    状态
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单编号
                </td>
                <td>
                    <a runat="server" id="linkOrderId" class="obvious-a"></a>
                </td>
                <td class="title">
                    产品类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    客票类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    采购方
                </td>
                <td>
                    <a runat="server" id="linkPurchaser" class="obvious-a"></a>
                </td>
                <td class="title">
                    出票方
                </td>
                <td colspan="3">
                    <a runat="server" id="linkProvider" class="obvious-a"></a>
                </td>
            </tr>
            <tr>
                <td class="title">
                    编码
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <h3 class="titleBg">申请信息</h3>
    <div runat="server" id="divApplication" class="table">
    </div>
    <uc:Voyage runat="server" ID="voyages" />
    <uc:Passenger runat="server" ID="passengers" />
    <!--附件-->
    <div runat="server" id="divApplyAttachment" class="column table">
    </div>
    <div class="column">
        <h3 class="titleBg">
            退票时处理信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>
                        提交时间
                    </th>
                    <th>
                        原因
                    </th>
                    <th>
                        处理时间
                    </th>
                    <th>
                        处理结果
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedReason"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProcessedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProcessedResult"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="table" runat="server" id="divRefundFeeInfo">
        </div>
    </div>
    <uc:Bill runat="server" ID="bill" />
     
    
    <div class="column table returnMoney" id="feeinput">
            <h3 class="titleBg">
                退票信息</h3>
        
        <asp:Repeater runat="server" ID="RefundInfos" onitemdatabound="RefundInfos_ItemDataBound">
            <itemtemplate>
            <div class="divinfo">
        <p>
            <asp:HiddenField runat="server" id="voyageId" Value='<%#Eval("flightId") %>' />
            <span class="flagType"><%#Eval("TripType")%></span>
            <span class="flagType flagType1">第<%#Eval("Seaial") %>程</span>
            <%#Eval("Departure")%>-<%#Eval("Arrival")%> <%#Eval("Carrier")%><%#Eval("FlightNo")%> <%#Eval("TakeoffTime")%> <%#Eval("Bunk")%>
            <span style="display:inline-block;position:relative">
                <a class="flightEI" target="_blank" href='/Index.aspx?redirectUrl=/SystemSettingModule/Role/AirlineRetreatChangeNew.aspx?Carrier=<%#Eval("Carrier")%>'>退改签;</a>
            </span>
            上次退款金额：<%#Eval("Refunded")%>   
            (收取<%#Eval("RefundReate")%>%手续费<%#Eval("RefundFee")%>元);
            本次退款：

            <asp:DropDownList runat="server" CssClass="selCompanyType" onchange="selCompanyTypeChange(this);" id="FeeType">
                <asp:ListItem Text="按金额" Value="0"/>
                <asp:ListItem Text="按比率" Value="1"/>
            </asp:DropDownList>
            <asp:TextBox runat="server" class="text" id="balance" CssClass="faxInput text" onblur="ReCalc(this)" />
            <span id="sunit">元</span>
            <div class="tgq_box hidden">
                <h2>退改签规定</h2>
                <div class="tgq_bd" id="Tip">
                    <%#Eval("EI") %>
                </div>
            </div>
       </p>
        <table class="Passengers ClearAlternate" id="Content">
            <tbody>
                <tr>
                    <th>
                        姓名
                    </th>
                    <th>
                        类型
                    </th>
                    <th>
                        票号
                    </th>
                    <th>
                        票面价
                    </th>
                    <th>
                        民航基金
                    </th>
                    <th>
                        燃油
                    </th><%if (!IsSpeical)
                           { %>
                    <th>
                        返佣
                    </th><% } %>
                    <th>
                        收款金额
                    </th>
                    <th>
                        首次退款金额
                    </th>
                    <th>
                        本次退款金额 
                    </th>
                    <th>
                        退款金额总计
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="Passengers">
                    <itemtemplate>
                <tr class="passengerstr">
                    <td>
                        <%#Eval("Name")%>
                    </td>
                    <td>
                        <%#Eval("PassengerType")%>
                    </td>
                    <td>
                        <%#Eval("No")%>
                    </td>
                    <td class="ticketPrice">
                       <%#Eval("TicketPrice")%>
                    </td>
                    <td>
                        <%#Eval("AirportFee")%>
                    </td>
                    <td>
                        <%#Eval("BAF")%>
                    </td><%if (!IsSpeical)
                           { %>
                    <td>
                        <%#Eval("Commission")%>
                    </td><%} %>
                    <td>
                        <%#Eval("YingShou", "{0:0.00}")%>
                    </td>
                    <td>
                        <span class="first"><%#Eval("first", "{0:0.00}")%></span>
                    </td>
                    <td>
                        <label class="fee"></label>
                    </td>
                    <td>
                        <label class="totalfee"></label>
                    </td>
                </tr>
                    </itemtemplate>
                </asp:Repeater>
                </tbody>
            </table></div>
            </itemtemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <asp:Button class="btn class1" runat="server" id="btnProcess" visible="true" OnClientClick="return CheckPrice()" OnClick="btnProcess_click" Text="指向出票方" />
       <%-- <asp:Button Text="拒绝退款" class="btn class1" runat="server" id="btnRefuse" OnClick="btnRefuse_click"/>--%>
            <input type="button" id="btnRefuse" value="拒绝退款" class="btn class1" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>

     <a id="divRefuse" style="display: none;" data="{type:'pop',id:'divRefuseRefund'}">
    </a>
    <div class="layer3" style="display:none;" id="divRefuseRefund">
        <h4>
            拒绝退款<a href="javascript:void(0);" class="close">关闭</a></h4>
        <table class="mini-table">
            <tr>
                <th>
                    <label class="title">拒绝理由：</label>
                </th>
                <td>
                    <textarea class="text" rows="4" cols="50" id="refuseReason" runat="server"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                        <asp:Button Text="提交" ID="btnSubmit" runat="server" OnClick="btnSubmit_click" CssClass="btn class1" />
                    <button class="btn class2 close" type="button">
                        取消</button>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField id="hdPassengerCount" runat="server" />
        </form>

</body>
</html>
    <script src="/Scripts/Global.js?20130522" type="text/javascript"></script>
    <script src="/Scripts/json2.js?20130522" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js?20130522" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js?20130522" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".tgq_box").appendTo($("body"));
        LoadTipEvents();
        $("#btnRefuse").click(function () {
            $("#divRefuse").click();
        })
        $("#btnSubmit").click(function () {
            var reason = ("#refuseReason").val();
            if ($.trim(reason).length == 0) {
                alert("请输入拒绝理由");
                return false;
            }
        });
    });

    function selCompanyTypeChange(obj) {
        var caclTypeContainer = $(obj);
        if (caclTypeContainer.val() == 0) {
            caclTypeContainer.next().next().html("元");
        }
        else {
            caclTypeContainer.next().next().html("%");
        }
    }

    function ReCalc(obj) {
        var that = $(obj);
        if ($.trim(that.val()) == "") return;
        if (that.parent().find(".selCompanyType").val() == 0) {
            if (parseInt(that.val()) < 0) {
                alert("退款金额输入错误！");
                return;
            }
            else {
                that.parent().next().next().find("tr.passengerstr").each(function (index, item) {
                    $(item).find(".fee").text(that.val());
                    var totalFee = parseFloat(that.val()) + parseFloat($(item).find(".first").text());
                    $(item).find(".totalfee").text(totalFee);
                });
            }
        }
        else {
            if (parseInt(that.val()) < 0 || parseInt(that.val()) > 100) {
                alert("退款比率输入错误！");
                return;
            }
            else {
                that.parent().next().next().find("tr.passengerstr").each(function (index, item) {
                    var par = parseFloat($(item).find(".ticketPrice").text());
                    $(item).find(".fee").text(Math.round(that.val() * par / 100, 2));
                    var fee = $(item).find(".fee").text();
                    var totalFee = parseFloat(fee) + parseFloat($(item).find(".first").text());
                    $(item).find(".totalfee").text(totalFee);
                });
            }
        }
    }

    function LoadTipEvents() {
        $(".flightEI").live("mouseenter", function () {
            $(".tgq_box").removeClass("hidden");
            $(".tgq_box").css("left", $(this).offset().left - 125);
            $(".tgq_box").css("top", $(this).offset().top + 15);
            var h = $(".tgq_box").height();
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if (h + top - sor > wh) {
                $(".tgq_box").css({ top: (top - h - 35) });
                $(".tgq_box").addClass("tgq_box1").removeClass("tgq_box");
            };
        }).live("mouseleave", function () {
            $(".tgq_box1").addClass("tgq_box").removeClass("tgq_box1");
            $(".tgq_box").addClass("hidden");
        });
    }
    function CheckPrice() {
        var allRight = true;
        $("#feeinput .faxInput").each(function (index, item)
        {
            var that = $(item);
            if (that.val() == "") allRight = false;
            if (!/\d+(\.\d+)?/.test(that.val())) allRight = false;
            if (!allRight)
            {
                alert("输入的价格不正确！");
                that.select();
                return false;
            }
        });
        return allRight;
    }

</script>