<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="special_policy_info.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.special_policy_info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 </head>   <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        特殊政策查看</h3>
    <div class="form">
        <table class="box">
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    航空公司
                </td>
                <td>
                    <asp:Label ID="lblAirline" runat="server"></asp:Label>
                </td>
                <td class="title">
                    行程类型
                </td>
                <td>
                    <asp:Label ID="lblVoyage" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    OFFICE号
                </td>
                <td>
                    <asp:Label ID="lblOfficeNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                    特殊票类型
                </td>
                <td>
                    <asp:Label ID="lblSpecialType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    出发城市
                </td>
                <td>
                    <asp:Label ID="lblDeparture" runat="server"></asp:Label>
                </td>
                <td class="title">
                    到达城市
                </td>
                <td>
                    <asp:Label ID="lblArrival" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    航班日期
                </td>
                <td>
                    <asp:Label ID="lblDepartureDate" runat="server"></asp:Label>
                </td>
                <td class="title">
                    航班限制
                </td>
                <td>
                    <asp:Label ID="lblOutWithFilght" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    排除日期
                </td>
                <td>
                    <asp:Label ID="lblExceptDay" runat="server"></asp:Label>
                </td>
                <td class="title">
                    班期限制
                </td>
                <td colspan="3">
                    <asp:Label ID="lblDepartureWeekFilter" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    提前天数
                </td>
                <td>
                    <asp:Label ID="lblDays" runat="server"></asp:Label>
                </td>
                <td class="title">
                    出票日期
                </td>
                <td>
                    <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    价格
                </td>
                <td>
                    <asp:Label ID="lblPrice" runat="server"></asp:Label>
                </td>
                <td class="title">
                    自定义编号
                </td>
                <td>
                    <asp:Label ID="lblCustomCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="freeTicket" runat="server" visible="false">
                <td class="title">
                    排除航段
                </td>
                <td>
                    <asp:Label ID="lblExceptAirlines" runat="server" class="text-auto"></asp:Label>
                </td>
                <td class="title">
                    黑屏是否同步
                </td>
                <td>
                    <asp:Label ID="lblIsSynsy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    采购时需要确认座位
                </td>
                <td>
                    <asp:Label ID="lblChang" runat="server"></asp:Label>
                </td>
                <td class="title" id="productNumberTitle" runat="server">
                    可提供产品数量
                </td>
                <td id="productNumberValue" runat="server">
                    <asp:Label ID="lblNum" runat="server"></asp:Label>
                </td>
            </tr>
            <tbody id="bussiness" runat="server">
                <tr>
                    <td class="title">
                        下级结算价
                    </td>
                    <td>
                        <asp:Label ID="lblSubOrdinate" runat="server"></asp:Label>
                    </td>
                    <td class="title" id="professionTitle" runat="server">
                        同行结算价
                    </td>
                    <td id="professionValue" runat="server">
                        <asp:Label ID="lblProfession" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title" id="internalTitle" runat="server">
                        内部结算价
                    </td>
                    <td id="internalValue" runat="server">
                        <asp:Label ID="lblInternal" runat="server"></asp:Label>
                    </td>
                    <td class="title" id="ticketTypeTitle" runat="server">
                        客票类型
                    </td>
                    <td id="ticketTypeValue" runat="server">
                        <asp:Label ID="lblTicketType" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    舱位
                </td>
                <td>
                    <asp:Label ID="lblBunks" runat="server"></asp:Label>
                </td>
                <td class="title">
                    起飞前2小时内可用B2B出票
                </td>
                <td>
                    <asp:Label ID="lblPrintBeforeTwoHours" runat="server"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="lowTr">
                <td class="title">
                    选择低价类型
                </td>
                <td>
                    <asp:Label ID="lblLowtype" runat="server"></asp:Label>
                </td>
                <td class="title">
                    价格限制
                </td>
                <td>
                    <asp:Label ID="lblLowPrice" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="box">
                    <h2>
                        出票条件</h2>
                </td>
                <td colspan="2" class="box">
                    <h2>
                        内部备注</h2>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background: #FFF;" class="box text-auto">
                    <asp:Label ID="lblDrawerCondition" runat="server"></asp:Label>
                </td>
                <td colspan="2" class="wLimit box text-auto">
                    <asp:Label ID="lblRemark" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <h2>
                        退改签条件</h2>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblRetreat" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <center>
        <input type="button" id="btnBack" class="btn class2" value="返回" runat="server"/>
    </center>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#lblDeparture,#lblArrival,#lblOutWithFilght").tipTip({ maxWidth: "400px", limitLength: 30 });
    })
</script>
