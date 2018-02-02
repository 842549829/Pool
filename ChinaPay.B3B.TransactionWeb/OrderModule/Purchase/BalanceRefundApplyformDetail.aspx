<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BalanceRefundApplyformDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.BalanceRefundApplyformDetail" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/UserControl/OutPutImage.ascx" TagName="OutPutImage" TagPrefix="uc" %>
<%@ Register TagPrefix="uc" TagName="Bill" Src="~/OrderModule/UserControls/OrderBill.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/core.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
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
                    编码
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <uc:Voyage runat="server" ID="voyages" />
    <uc:Passenger runat="server" ID="passengers" />
    <!--附件-->
    <div runat="server" id="divApplyAttachment" class="column table">
        <uc:OutPutImage runat="server" ID="ucOutPutImage" ClientIDMode="Static" />
        <script type="text/javascript">
            $(function ()
            {
                $("#divOutPutImage img.buttonImg").click(function ()
                {
                    var aid = $(this).attr("dataType");
                    var filePath = $(this).attr("FilePath");
                    $("#divLayerImage img").attr("src", filePath);
                    $("#a" + aid).click();
                });
            });
        </script>
    </div>
    <div class="column">
        <div class="hd">
            <h2>
                申请/处理信息</h2>
        </div>
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
    <%--账单信息--%>
    <uc:Bill runat="server" ID="bill" />
    <h3 class="titleBg">
        退票时处理信息</h3>
    <div class="column form table" runat="server" id="RefundInfo">
    </div>
    <div class="column form table" runat="server" id="BalanceRefundInfo" Visible="false"></div>
    <%--联系人信息--%>
    <div runat="server" id="divContact" class="column">
        <div class="tips-info">
            <div class="hd">
                <h2>
                    联系人信息</h2>
            </div>
            <div class="con tips">
                <div class="col-3">
                    <div class="col">
                        联系人：<asp:Label runat="server" ID="lblContact"></asp:Label></div>
                    <div class="col">
                        手机号码：<asp:Label runat="server" ID="lblContactMobile"></asp:Label></div>
                    <div class="col">
                        电子邮箱：<asp:Label runat="server" ID="lblContactEmail"></asp:Label></div>
                </div>
            </div>
        </div>
    </div>
    <div class="btns">

        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <asp:HiddenField ID="hfdApplyId" runat="server" />
    <asp:HiddenField ID="hfdBalanceRefunded" runat="server" />
    <asp:HiddenField ID="hfdErrorRemark" runat="server" />

    </form>
</body>
<script src="/Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $("#BalanceRefundError").click(function ()
        {
            var loadinfo = $(this).attr("add");
            var applyNo = loadinfo.split('|')[0];
            var fare = loadinfo.split('|')[1];
            $("#txtApplyNo").val(applyNo);
            $("#txtBalanceRefunded").val(fare);
        });
    });

    function LockOrder()
    {
        sendPostRequest("/OrderHandlers/Order.ashx/LockOrder", JSON.stringify(
            { orderId: parseInt($("#linkOrderId").text()), reason: "差额退款申请" }),
        function (msg)
        {
            if (msg == "")
            {
                $("#errorInputer").click();
            } else
            {
                alert(msg);
            }
        }, function (msg)
        {
            alert(msg.responseText);
        });
    }

    function UnLockOrder()
    {
        sendPostRequest("/OrderHandlers/Order.ashx/UnLock", JSON.stringify(
            { orderId: parseInt($("#linkOrderId").text()) }),
        $.noop, $.noop);
    }
</script>
</html>
