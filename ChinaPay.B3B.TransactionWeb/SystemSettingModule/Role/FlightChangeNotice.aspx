<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightChangeNotice.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.FlightChangeNotice" %>

<%@ Register TagName="pager" Src="~/UserControl/Pager.ascx" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <style type="text/css">
        .text.fl
        {
            float: none;
        }
        .takeOffTime
        {
            color: red;
            font-weight: bold;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        航班变更通知</h3>
    <div class="clearQ-box table" id="flightChangeNotice">
        <p class="box importantBox broaden" id="flightChangeHasData" runat="server">
            上次清Q执行时间：<asp:Label ID="lblFlightChangeTime" runat="server" CssClass="pad-r"></asp:Label>
            <span class="important p_title">您共有<asp:Label ID="lblFlightCount" runat="server"></asp:Label>个航班：
                <asp:Label ID="lblOrderCount" runat="server"></asp:Label>个订单受影响，请根据下方列表进行相关操作 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                若您发现清Q结果错误，请点击<a href="javascript:void(0);" class="obvious-a" id="sendMistake">报告错误</a>
            </span>
        </p>
        <p class="importantBox broaden" id="flightChangeWithoutData" runat="server" visible="false">
            上次清Q执行时间：<asp:Label ID="lblFlightTime" runat="server" CssClass="pad-r"></asp:Label>
            <span class="ok p_title">恭喜，暂时没有对您造成影响的航班变动&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a
                class="obvious-a" href="FlightTranferInformation.aspx">继续查看清Q结果</a> </span>
        </p>
        <div class="table" id="dataSource" runat="server">
            <asp:Repeater ID="rptNotify" runat="server">
                <HeaderTemplate>
                    <table class="mar-t">
                        <thead>
                            <tr>
                                <th class="w8">
                                    PNR
                                </th>
                                <th class="w10">
                                    订单号
                                </th>
                                <th class="w8">
                                    原航空公司
                                </th>
                                <th class="w8">
                                    新航空公司
                                </th>
                                <th class="w8">
                                    变更类型
                                </th>
                                <th class="w8">
                                    原航班号
                                </th>
                                <th class="w8">
                                    新航班号
                                </th>
                                <th class="w15">
                                    原起抵时间
                                </th>
                                <th class="w15">
                                    新起抵时间
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <span class="obvious">
                                <%#Eval("PNR")%></span>
                        </td>
                        <td>
                            <a target="_blank" href='../../OrderModule/Purchase/OrderDetail.aspx?id=<%#Eval("OrderId") %>'>
                                <%#Eval("OrderId")%></a>
                        </td>
                        <td>
                            <%#Eval("OriginalCarrierName")%>
                        </td>
                        <td>
                            <%#Eval("CarrierName")%>
                        </td>
                        <td>
                            <%#Eval("TransferType")%>
                        </td>
                        <td>
                            <%#Eval("OriginalFlightNo")%>
                        </td>
                        <td>
                            <%#Eval("FlightNo")%>
                        </td>
                        <td class='<%#(bool)Eval("IsBeyondThreeDay")?"takeOffTime":"" %>'>
                            <%#Eval("OriginalTakeoffTime", "{0:yyyy-MM-dd HH:mm}")%><br />
                            <%#Eval("OriginalArrivalTime","{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                        <td>
                            <%#Eval("TakeoffTime", "{0:yyyy-MM-dd HH:mm}")%>
                            <br />
                            <%#Eval("ArrivalTime", "{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                        <td>
                            <a href="javascript:queryTemplate('<%#Eval("OrderId") %>','<%#Eval("OriginalFlightNo") %>','<%#Eval("OriginalTakeoffTime") %>','<%#Eval("TransferTypeValue") %>','<%#Eval("FlightNo") %>','<%#Eval("TakeoffTime") %>','<%#Eval("TransferId") %>');"
                                class="sendMsgToPassenger">短信通知乘机人</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody></table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="box" style="text-align: center" runat="server" visible="false" id="emptyDataInfo">
                <asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>建议您经常关注本页面以确保您能第一时间获取航班变动结果<br />
                感谢您对<asp:Literal runat="server" ID="lblPlatformName1"></asp:Literal>的支持，希望您能够将<asp:Literal
                    runat="server" ID="lblPlatformName2"></asp:Literal>平台推荐给您的朋友
            </div>
        </div>
        <div class="btns">
            <uc:pager ID="pager" runat="server" Visible="false" />
        </div>
    </div>
    <a id="divOption" style="display: none;" data="{type:'pop',id:'divMessage'}"></a>
    <div class="layer4 hidden" id="divMessage" style="width: 700px;">
        <h4>
            短信通知<a href="#" class="close">关闭</a></h4>
        <table style="width: 600px;">
            <colgroup>
                <col class="w20" />
                <col />
            </colgroup>
            <tr>
                <td class="title">
                    手机号码
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="text" Width="50%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    短信内容
                </td>
                <td>
                    <textarea runat="server" class="text fl" id="txtContext" style="width: 100%; min-height: 100px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="fr">
                    您的短信剩余条数：<asp:Label ID="lblMessageCount" runat="server"></asp:Label>条，您可以继续<a href="../../SmsModule/SMSBuy.aspx">立即购买</a>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="obvious1">已输入<span
                        id="spanSmsNum" class="obvious font-b">0</span>字，将分<span id="spanNum" class="obvious">1</span>条发送。</span>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <input type="button" id="btnSubmit" class="btn class1" value="提交" />
                    <input type="button" id="btnCancel" value="取消" class="btn class2 close" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfdPurchasePhone" runat="server" />
    <asp:HiddenField ID="hfdTransferId" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#sendMistake").click(function () {
            if ($("#help_trigger_box", parent.document).size() > 0) {
                $("#send_msg", $("#help_trigger_box", parent.document)).click();
            }
        });
        $("#txtContext").keyup(function () {
            $("#spanSmsNum").html($.trim($("#txtContext").val()).length);
            $("#spanNum").html(parseInt(Math.ceil(parseFloat($.trim($("#txtContext").val()).length) / 61)));
        });

        $("#btnSubmit").click(function () {
            var transferId = $("#hfdTransferId").val();
            var phone = $("#txtPhone").val();
            var content = $("#txtContext").val();
            if ($.trim(phone).length == 0) {
                alert("电话号码不能为空");
                return false;
            }
            if ($.trim(content).length == 0) {
                alert("内容不能为空");
                return false;
            }
            var targetUrl = "/OrderHandlers/Order.ashx/SendFlightTranferByPurchase";
            var parameters = JSON.stringify({ "transferId": transferId, "phone": phone, "content": content });
            sendPostRequest(targetUrl, parameters, function () {
                alert("短信发送成功");
                window.location.href = "/SystemSettingModule/Role/FlightChangeNotice.aspx";
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
        });
    })

    function queryTemplate(orderId, originalFlightNo, originalTakeoffTime, transferTypeValue, newFlightNo, newTakeoffTime, transferId) {
        $("#hfdTransferId").val(transferId);
        var purchasePhone = $("#hfdPurchasePhone").val();
        var targetUrl = "/OrderHandlers/Order.ashx/QueryFlightTransferTemplate";
        var parameters = JSON.stringify({ "orderId": orderId, "originalFlightNo": originalFlightNo, "originalTakeoffTime": originalTakeoffTime, "transferTypeValue": transferTypeValue, "newFlightNo": newFlightNo, "newTakeOffTimeText": newTakeoffTime, "purchasePhone": purchasePhone });
        sendPostRequest(targetUrl, parameters, function (result) {
            if (result.IsNeedSend) {
                $("#txtPhone").val(result.FirstPassengerPhone);
                $("#txtContext").val(result.Template);
                $("#spanSmsNum").html($.trim($("#txtContext").val()).length);
                $("#spanNum").html(parseInt(Math.ceil(parseFloat($.trim($("#txtContext").val()).length) / 61)));
                $("#divOption").click();
            } else {
                alert("该订单乘机人已退完");
            }
        }, function (e) {
            if (e.statusText == "timeout") {
                alert("服务器忙");
            } else {
                alert(e.responseText);
            }
        });
    }
</script>
