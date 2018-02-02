<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundApplyformDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.RefundApplyformDetail" %>

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
    <div class="btns addition-box">
        <div class="addition">
            <a style="display: block;" runat="server" id="BalanceRefundError" onclick="LockOrder()">
                <i class="g-icon-warning-sign"></i>退/废票金额错误</a> <a data="{type:'pop',id:'divBalanceRefundError'}"
                    style="display: none" id="errorInputer" /><br />
        </div>
    </div>
    <div style="clear: both">
        1</div>
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
    <div class="btns addition-box">
        <div class="addition">
            <asp:LinkButton ID="btnOrderHistory" Visible="true" runat="server"><i class="g-icon-time"></i>订单历史记录</asp:LinkButton>
            <asp:LinkButton ID="btnProcessingApplyforms" Visible="true" runat="server"><i class="g-icon-list"></i>进行中的申请</asp:LinkButton>
        </div>
    </div>
    <div class="form layer2 hidden" id="divLayerImage">
        <h4>
            原图<a href="javascript:void(0);" class="close">关闭</a></h4>
        <img style="max-height: 500px; max-width: 500px" />
    </div>
    <div class="btns">
        <%--<button class="btn class1" runat="server" visible="true" id="btnApply">
            申请退改签</button>--%>
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <asp:HiddenField ID="hfdApplyId" runat="server" />
    <asp:HiddenField ID="hfdBalanceRefunded" runat="server" />
    <asp:HiddenField ID="hfdErrorRemark" runat="server" />
    <div id="divBalanceRefundError" class="form layer3" style="display: none">
        <h4>
            退废票金额错误争议处理申请<a href="javascript:UnLockOrder();" class="close">关闭</a></h4>
        <table class="mini-table">
            <tr>
                <th>
                    <label class="title">
                        申请单号：</label>
                </th>
                <td>
                    <label id="txtApplyNo"></label><span class="warning-box dis-ib">请注意：一个申请单只能申请一次差错退款</span>
                </td>
            </tr>
            <tr>
                <th>
                    <label class="title">
                        已退款金额：</label>
                </th>
                <td>
                    <label id="txtBalanceRefunded"></label><span
                        class="pad-l obvious1">(该运价是生成订单时的运价历史记录，不一定是当前运价)</span>
                </td>
            </tr>
            <tr>
                <th>
                    <label class="title">
                        差错备注：</label>
                </th>
                <td>
                    <asp:TextBox ID="txtErrorRemork" CssClass="text" TextMode="MultiLine" Rows="4" Width="500px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmitErrorHandle" CssClass="btn class1" runat="server" Text="提交"
                        OnClick="btnSubmit_Click" OnClientClick="return CheckInput()" />
                    <button class="btn class2 mar-l close" onclick="UnLockOrder()" type="button">
                        取消</button>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
<script src="/Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $("#BalanceRefundError").click(function ()
        {
            var loadinfo = $(this).attr("add");
            var applyNo = loadinfo.split('|')[0];
            var fare = loadinfo.split('|')[1];
            $("#txtApplyNo").text(applyNo);
            $("#txtBalanceRefunded").text(fare);
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

    function CheckInput() {
        var result = true;
        if ($("#txtErrorRemork").val().length > 500) {
            alert("备注信息过长！");
            $("#txtErrorRemork").select();
            result = false;
        }
        return result;
    }
</script>
</html>
