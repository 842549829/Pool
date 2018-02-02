<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandleBalanceRefund.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.HandleBalanceRefund" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagPrefix="uc" TagName="Bill" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>处理差错退款</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/core.css" rel="stylesheet" type="text/css" />
</head>
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
    <h3 class="titleBg">
        退票时处理信息</h3>
    <div class="column form table" runat="server" id="RefundInfo">
    </div>
    <div class="column form table" runat="server" id="BalanceRefundInfo" Visible="false">
    </div>
    <div class="btns">
        <asp:Button Text="同意退款" ID="btnAgreeRefund" runat="server" CssClass="btn class1" OnClick="AgreeRefund"/>
       <%-- <asp:Button Text="拒绝退款" ID="btnRefuse" runat="server" OnClick="DenyRefund" CssClass="btn class1" />--%>
        <button class="btn class1" id="btnRefuse" type="button">拒绝退款</button>
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <a id="divRefuse" style="display: none;" data="{type:'pop',id:'divRefuseRefund'}"></a>
    <div class="layer3" style="width: 500px;display:none;" id="divRefuseRefund">
        <h4>拒绝退款<a href="javascript:;" class="close">关闭</a></h4>
        <table class="mini-table">
            <tr>
                <th>
                    <label class="title">拒绝理由：</label>
                </th>
                <td>
                    <textarea class="text" rows="4" cols="50" runat="server" id="txtRefuseReason"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button Text="拒绝退款" ID="Button1" runat="server" OnClick="DenyRefund" CssClass="btn class1" />
                    <button class="btn class2 close" type="button">
                        取消</button>
                </td>
            </tr>
        </table>
    </div>
        </form>

</body>
</html>
<script type="text/javascript" src="../../Scripts/Global.js?20130522"></script>
<script type="text/javascript" src="../../Scripts/widget/common.js?20130522"></script>
<script type="text/javascript">
    $(function () {
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
    })
</script>