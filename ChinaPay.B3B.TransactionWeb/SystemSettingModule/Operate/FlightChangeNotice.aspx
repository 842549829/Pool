<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightChangeNotice.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.FlightChangeNotice" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js?2013117"></script>
    <script src="../../Scripts/selector.js" type="text/javascript"></script>
</head>    <style type="text/css">
        .text.fl
        {
            float: none;
        }
        .tempSpan
        {
            margin-left:-70px;
            display:inline-block;
            width:70px;
            text-align:right;
        }
        .tempSpan
        {
            *display:inline;
            }
    </style>

<body>
    <form id="form1" runat="server" autocomplete="off">
    <h3 class="titleBg">
        航班变更通知</h3>
    <ul class="clearQ-nav clearfix">
        <li class="curr" id="flightChange">航班变动通知</li>
        <li id="queryNotice">查看通知记录</li>
    </ul>
    <div class="clearQ-box table" id="flightChangeNotice">
        <p class="box">
            <span style="font-weight: 900">本次</span>清Q共影响<asp:Label ID="lblAirlineCount" runat="server" CssClass="obvious"></asp:Label>个航空公司共计
            <asp:Label ID="lblFlightCount" runat="server" CssClass="obvious"></asp:Label>
            个航班，有
            <asp:Label ID="lblPuchaseCount" runat="server" CssClass="obvious"></asp:Label>
            个采购账号受到影响，还有
             <asp:Label ID="lblToBeInformCount" runat="server" CssClass="obvious"></asp:Label>
            个采购未通知，上次清Q执行时间：<asp:Label ID="lblFlightChangeTime" runat="server" CssClass="pad-r"></asp:Label>
        </p>
        <div class="table" id="dataSource" runat="server">
            <asp:Repeater runat="server" ID="dataList">
                <HeaderTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th class="w15">
                                    采购账号
                                </th>
                                <th class="w20">
                                    采购账户名
                                </th>
                                <th class="w20">
                                    联系电话
                                </th>
                                <th class="w15">
                                    受影响航班数
                                </th>
                                <th class="w15">
                                    受影响订单数
                                </th>
                                <th class="w15">
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("PurchaserAccount")%>
                        </td>
                        <td>
                            <%#Eval("PurchaserName")%>
                        </td>
                        <td>
                            <%#Eval("ContractPhone")%>
                        </td>
                        <td>
                            <%#Eval("FlightCount")%>
                        </td>
                        <td>
                            <%#Eval("OrderCount")%>
                        </td>
                        <td>
                            <a href='NotifyPurchase.aspx?purchaserId=<%#Eval("PurchaserId") %>&purchaserAccount=<%#Eval("PurchaserAccount")%>&purchaserName=<%#Eval("PurchaserName")%>&contractPhone= <%#Eval("ContractPhone")%>&flightCount=<%#Eval("FlightCount")%>&orderCount= <%#Eval("OrderCount")%>'>
                                通知采购</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="box" runat="server" visible="false" id="emptyDataInfo">
                通知已完成，没有任何需要通知的记录
            </div>
        </div>
        <div class="btns">
            <uc:Pager ID="pager" runat="server" Visible="false" />
        </div>
    </div>
    <div class="clearQ-box table" id="queryNoticeRecord" style="display:none">
        <div class="clearfix col-3">
            <div class="col">
                <p>
                    <span>航空公司：</span><asp:DropDownList ID="ddlAirlines" runat="server">
                    </asp:DropDownList>
                </p>
                <p>
                    <span>航班号：</span><asp:TextBox ID="txtFlightNo" CssClass="text" runat="server"></asp:TextBox></p>
                <p>
                    <span>变更类型：</span><asp:DropDownList ID="ddlChangeType" runat="server">
                    </asp:DropDownList>
                </p>
            </div>
            <div class="col">
                <p>
                    <span>出发城市：</span>
                    <uc:Airport ID="txtDepartureCity" runat="server" />
                </p>
                <p>
                    <span>到达城市：</span>
                    <uc:Airport ID="txtArrivalCity" runat="server" />
                </p>
                <div style="padding:5px 0px 5px 70px;">
                    <span class="tempSpan" style="\">采购账号：</span>
                    <uc:Company ID="txtPurchase" runat="server" />
                </div>
            </div>
            <div class="col">
                <p>
                    <span>通知方式：</span><asp:DropDownList ID="ddlNoticeWay" runat="server">
                    </asp:DropDownList>
                </p>
                <p>
                    <span>通知结果：</span><asp:DropDownList ID="ddlNoticeResult" runat="server">
                    </asp:DropDownList>
                </p>
                <p>
                    <span>通知时间：</span><asp:TextBox ID="txtNoticeLowerTime" runat="server" CssClass="width-90 text"
                        onfocus="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtNoticeUpperTime\')||\'%y-%M-%d\'}'})"></asp:TextBox>至
                    <asp:TextBox ID="txtNoticeUpperTime" runat="server" CssClass="width-90 text" onfocus="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtNoticeLowerTime\')}', maxDate:'%y-%M-%d'})"></asp:TextBox>
                </p>
            </div>
        </div>
        <div class="btns">
            <asp:Button ID="btnQuery" runat="server" Text="查&nbsp;&nbsp;询" CssClass="btn class1"
                OnClick="btnQuery_Click" />
            <input id="downLoad" type="button" value="下&nbsp;&nbsp;载" class="btn class2" />
        </div>
        <div class="table" id="noticeSource" runat="server">
            <asp:Repeater runat="server" ID="noticeRecords" EnableViewState="false" OnPreRender="AddEmptyTemplate">
                <HeaderTemplate>
                    <table>
                        <tr>
                            <th class="w10">
                                采购账号
                            </th>
                            <th class="w10">
                                航空公司
                            </th>
                            <th class="w20">
                                航线
                            </th>
                            <th class="w8">
                                航班号
                            </th>
                            <th class="w8">
                                变更类型
                            </th>
                            <th class="w15">
                                通知时间
                            </th>
                            <th class="w10">
                                通知方式
                            </th>
                            <th class="w10">
                                通知结果
                            </th>
                            <th class="w8">
                                操作人
                            </th>
                        </tr>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("PurchaserAccount")%>
                        </td>
                        <td>
                            <%#Eval("CarrierName")%>
                        </td>
                        <td>
                            <%#Eval("DepartureCityName")%><%#Eval("DepartureName")%>-<%#Eval("ArrivalCityName")%><%#Eval("ArrivalName")%>
                        </td>
                        <td>
                            <%#Eval("FlightNO")%>
                        </td>
                        <td>
                            <%#Eval("TransferType")%>
                        </td>
                        <td>
                            <%#Eval("InformTime")%>
                        </td>
                        <td>
                            <%#Eval("InformMethod")%>
                        </td>
                        <td>
                            <%#Eval("InformResult")%>
                        </td>
                        <td>
                            <%#Eval("InfromerName")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="btns">
            <uc:Pager ID="recordPager" runat="server" Visible="false" />
        </div>
    </div>
    <asp:HiddenField ID="hfdType" runat="server" />
    </form>
</body>
</html> 
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script> 
<script type="text/javascript">
    $(function () {
        if ($("#hfdType").val() != "records") {
            $("#queryNoticeRecord").css("display", "none");
            $("#flightChangeNotice").css("display", "");
            $("ul > li").removeClass("curr");
            $("#flightChange").addClass("curr");
        } else {
            $("#queryNoticeRecord").css("display", "");
            $("#flightChangeNotice").css("display","none");
            $("ul > li").removeClass("curr");
            $("#queryNotice").addClass("curr");
        }
        $("#flightChange").click(function () {
            $("ul > li").removeClass("curr");
            $(this).addClass("curr");
            $("#queryNoticeRecord").css("display","none");
            $("#flightChangeNotice").css("display", "");
            $("#hfdType").val("");
        });
        $("#queryNotice").click(function () {
            $("ul > li").removeClass("curr");
            $(this).addClass("curr");
            $("#queryNoticeRecord").css("display", "");
            $("#flightChangeNotice").css("display", "none");
            $("#txtFlightNo").LimitLength(4);
        });
        $("#btnQuery").click(function () {
            if (!valiate()) {
                return false;
            }
        });
        $("#downLoad").click(function () {
            if (!valiate()) {
                return false;
            } else {
                var airline = $("#ddlAirlines").val();
                var fightNo = $("#txtFlightNo").val();
                var changeType = $("#ddlChangeType").val();
                var departure = $("#txtDepartureCity_ddlAirports").val();
                var arrival = $("#txtArrivalCity_ddlAirports").val();
                var purchase = $("#txtPurchase_hidCompanyId").val();
                var noticeWay = $("#ddlNoticeWay").val();
                var noticeResult = $("#ddlNoticeResult").val();
                var noticeLowerTime = $("#txtNoticeLowerTime").val();
                var noticeUpplerTime = $("#txtNoticeUpperTime").val();
                var flightChangeNoticeCondition = airline + ',' + fightNo + ',' + changeType + ',' + departure + ',' + arrival + ',' + purchase + ','
                 + noticeWay + ',' + noticeResult + ',' + noticeLowerTime + ','
                + noticeUpplerTime;
                window.open("../../ReportModule/ReportDownload.aspx?flightChangeNoticeCondition=" + flightChangeNoticeCondition, "newwindow");
            }
        });
    });
    function valiate() {
        var flightPattern = /^[a-zA-Z0-9]{4}$/;
        if ($("#txtFlightNo").val().length > 0 && !flightPattern.test($("#txtFlightNo").val())) {
            $("#txtFlightNo").select();
            alert("航班号格式错误");
            return false;
        }
        return true;
    }
</script>
