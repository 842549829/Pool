<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalOrderDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ExternalOrderDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">
        外部平台订单详情</h3>
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
                    外部订单号
                </td>
                <td>
                    <asp:Label runat="server" ID="lblExtenalOrderId" CssClass="obvious"></asp:Label>（<asp:Label
                        ID="lblPlatformType" runat="server"></asp:Label>）
                </td>
                <td class="title">
                    创建时间
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProduceTime" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    采购商
                </td>
                <td>
                    <asp:Label runat="server" ID="lblRelation" CssClass="obvious"></asp:Label>
                    <a href="#" runat="server" id="hrefPurchaseName"></a>
                </td>
            </tr>
            <tr>
                <td class="title">
                    内部订单号
                </td>
                <td>
                    <a href="#" runat="server" id="lnkInternalOrderId">
                        <asp:Label runat="server" ID="lblInternalOrderId"></asp:Label></a>
                </td>
                <td class="title">
                    出票状态
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPrintStatus" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    外部返点
                </td>
                <td>
                    <asp:Label runat="server" ID="lblExternalCommission" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单金额
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOrderAmount" CssClass="obvious"></asp:Label><asp:Label
                        ID="lblPurchaseAmount" runat="server"></asp:Label>
                </td>
                <td class="title">
                    客票类型
                </td>
                <td>
                    <asp:Label ID="lblTicketType" runat="server" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    产品类型
                </td>
                <td>
                    <asp:Label ID="lblProductType" runat="server" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    对外支付状态
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPayStatus" CssClass="obvious"></asp:Label>
                </td>
                <td class="title" id="payTimeTitle" runat="server">
                    对外支付时间
                </td>
                <td id="payTimeValue" runat="server">
                    <asp:Label runat="server" ID="lblPayTime" CssClass="obvious"></asp:Label>
                </td>
                <td class="title" id="payTradeNoTitle" runat="server">
                    对外支付流水号
                </td>
                <td id="payTradeNoValue" runat="server">
                    <asp:Label runat="server" ID="lblPayTradeNo" CssClass="obvious"></asp:Label>
                </td>
                <td class="title" id="failedReasonTitle" runat="server" visible="false">失败原因</td>
                <td colspan="3" id="failedReasonValue" runat="server" visible="false"><asp:Label ID="lblFailedReason" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">
                 内部状态
                </td>
                <td>
                  <asp:Label ID="lblInternalPayStatus" runat="server"></asp:Label>
                </td>
              </tr>
        </table>
        <div class="importantBox">
            <p class="important p-title">
                订单信息来源于外部平台，请及时跟进支付与出票状态，点击内部订单号及采购商可查看内部订单及采购商信息</p>
        </div>
    </div>
    <%--编码组信息--%>
    <div runat="server" id="divPNRGroups">
    </div>
    <%--底部操作按钮--%>
    <div class="btns">
        <input class="btn class1" type="button" runat="server" visible="false" id="btnPayOrder"
            value="对外支付订单" />
        <asp:Button CssClass="btn class1" runat="server" Visible="false" ID="btnGetPayInfo"
            Text="获取支付信息" OnClick="btnGetPayInfo_Click" />
        <asp:Button CssClass="btn class1" runat="server" Visible="false" ID="btnGetTicketNos"
            Text="获取票号信息" OnClick="btnGetTicketNos_Click" />
        <button class="btn class2" runat="server" type="button" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <asp:HiddenField runat="server" ID="hidReturnUrl" />
    <a id="divOption" style="display: none;" data="{type:'pop',id:'divChoice'}"></a>
    <div class="layer4 hidden" id="divChoice">
        <h4>
            对外支付订单<a href="#" class="close">关闭</a></h4>
        <div class="handle-box">
            <p style="text-align: center;">
                <asp:RadioButton ID="rbnAutoPay" runat="server" Text="自动支付" GroupName="payType" Checked="true" />
                <asp:RadioButton ID="rbnHanderPay" runat="server" Text="手动支付" GroupName="payType" />
                <asp:DropDownList ID="ddlYeexingPlatform" runat="server" CssClass="hidden">
                </asp:DropDownList>
            </p>
        </div>
        <div class="btns">
            <a id="btnSubmit" class="btn class1" target="_blank">提交</a>
            <input type="button" class="btn class1" value="提交" id="btnReplace" style="display:none" />
            <input type="button" class="btn class1 close" value="关&nbsp;&nbsp;闭" />
        </div>
    </div>
       <%-- 支付跳转提示 --%>
    <a id="payAttention" style="display: none" data="{type:'pop',id:'divPayAttention'}">
    </a>
    <div id="divPayAttention" class="layer">
        <h3>
            支付</h3>
        <div class="con">
            <strong>请您在新打开的支付页面上完成付款。</strong>
            <p>
                付款完成前请不要关闭此窗口。
            </p>
            <p>
                完成付款后请根据您的情况点击下面的按钮完成操作。</p>
        </div>
        <div class="btns">
            <a id="btnPayComplete" class="btn class1">已完成付款</a> <a id="btnPayError" class="btn class2">
                付款遇到问题</a>
        </div>
    </div>
    <asp:HiddenField ID="hfdPlatformValue" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js?20130125" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#ddlYeexingPlatform option").length <= 1) {
            $("#rbnHanderPay").hide();
            $("label[for=rbnHanderPay]").hide();
        }
        $("#btnPayOrder").click(function () {
            $("#btnSubmit").show();
            $("#btnReplace").hide();
            $("#divOption").click();
        });
        $("#rbnHanderPay").click(function () {
            $("#ddlYeexingPlatform").show();
            $("#btnSubmit").show();
            $("#btnReplace").hide();
        });
        $("#ddlYeexingPlatform").change(function () {
            $("#btnSubmit").show();
            $("#btnReplace").hide();
            if ($("#ddlYeexingPlatform").val() != "") {
                var platformText = $("#lblPlatformType").text();
                var payInterfaceValue = $("#ddlYeexingPlatform").val();
                var internalOrderId = $("#lblInternalOrderId").text();
                var externalOrderId = $("#lblExtenalOrderId").text();
                var orderAmount = $("#lblOrderAmount").text();
                $("#btnSubmit").attr("href", "pay.aspx?platformText=" + encodeURI(platformText) + "&payInterfaceValue=" + payInterfaceValue + "&internalOrderId=" + internalOrderId
                                       + "&externalOrderId=" + externalOrderId + "&orderAmount=" + orderAmount);
            }
        });
        $("#rbnAutoPay").click(function () {
            $("#ddlYeexingPlatform").hide();
            $("#btnSubmit").show();
            $("#btnReplace").hide();
        });
        $("#btnSubmit").click(function () {
            if ($("#rbnAutoPay").is(":checked")) {
                $("#btnSubmit").hide();
                $("#btnReplace").show();
                $("#btnSubmit").removeAttr("href");
                var orderId = $("#lblInternalOrderId").text();
                sendPostRequest("/OrderHandlers/Order.ashx/AutoPayExternalOrderId", JSON.stringify({ "orderId": orderId }), function (result) {
                    if (result.PayStatusValue == 0) {
                        window.location.href = '/OrderModule/Provide/ExternalOrderDetail.aspx?id=' + $("#lblInternalOrderId").text() + '&PlatformTypeValue=' + $("#hfdPlatformValue").val() + '&returnUrl=ExternalOrderList.aspx';
                    } else {
                        $("#btnSubmit").show();
                        $("#btnReplace").hide();
                        if (result.Reason != "") {
                            alert(result.Reason);
                        } else {
                            alert("自动支付失败");
                        }
                        window.location.href = '/OrderModule/Provide/ExternalOrderDetail.aspx?id=' + $("#lblInternalOrderId").text() + '&PlatformTypeValue=' + $("#hfdPlatformValue").val() + '&returnUrl=ExternalOrderList.aspx';
                    }
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                    window.location.href = '/OrderModule/Provide/ExternalOrderDetail.aspx?id=' + $("#lblInternalOrderId").text() + '&PlatformTypeValue=' + $("#hfdPlatformValue").val() + '&returnUrl=ExternalOrderList.aspx';
                });
            }
            if ($("#rbnHanderPay").is(":checked")) {
                if ($("#ddlYeexingPlatform").val() == "") {
                    alert("请选择手动支付方式");
                    return false;
                }
                $("#payAttention").click();
            }
        });
        $("#btnPayError").click(function () {
            $("#payAttention").hide();
            $("#divPayAttention").hide();
        });
        $("#btnPayComplete").click(function () {
            window.location.href = '/OrderModule/Provide/ExternalOrderDetail.aspx?id=' + $("#lblInternalOrderId").text() + '&PlatformTypeValue=' + $("#hfdPlatformValue").val() + '&returnUrl=ExternalOrderList.aspx';
        });
    })
</script>
