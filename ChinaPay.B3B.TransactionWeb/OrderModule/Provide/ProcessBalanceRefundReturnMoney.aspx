<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ProcessBalanceRefundReturnMoney.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ProcessBalanceRefundReturnMoney" %>

<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagPrefix="uc" TagName="Bill" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <h3 class="titleBg">申请单详情</h3>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <div class="form">
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
                    申请单号：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    申请类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    状态：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单编号：
                </td>
                <td>
                    <a id="linkOrderId" runat="server" class="obvious-a"></a>
                </td>
                <td class="title">
                    产品类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    客票类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    编码：
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                </td>
               
            </tr>
        </table>
    </div>
    <uc:Voyage runat="server" ID="voyages" Tip="航班信息" />
    <uc:Passenger runat="server" ID="passengers" Tip="机票信息" />
    <%--政策备注--%>
    <div id="divPolicyRemark" class="column table">
            <h3 class="titleBg">
                政策备注</h3>
        <div>
            <asp:Literal ID="lblPolicyRemark" runat="server" />
        </div>
    </div>
    <div class="column">
            <h3 class="titleBg">
                申请/处理信息</h3>
        <div class="table">
            <table class="ClearAlternate">
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
    <h3 class="titleBg">
        退票时处理信息</h3>
    <div class="column form table" runat="server" id="RefundInfo">
    </div>
    <div class="column form table" runat="server" id="BalanceRefundInfo" Visible="false"></div>
    <div class="btns">
        <%--<a href="ReturnMoney.aspx" class="btn class1">同意退款</a>--%>
        <asp:Button text="同意退款" ID="btnAgreeRefund" runat="server" CssClass="btn class1"
            onclick="btnAgreeRefund_Click" />
        <input type="button" id="btnDenyInputReason" class="btn class2" onclick="InputReason()" value="拒绝退款" />
        <asp:Button class="btn class2" runat="server" Text="解锁并返回" ID="btnReleaseLock" onclick="btnReleaseLock_Click" />
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <a style="display:none" data="{type:'pop',id:'test_layer'}" id="DenyTrigger"></a>
    <div id="test_layer" class="layer4" style="display: none">
            <h4>
                拒绝退款<a href="javascript:void(0);" class="close">关闭</a>
            </h4>
                    <table class="mini-table">
            <tr>
                <td>
                    <label>拒绝理由：</label>
                </td>
                <td>
                     <asp:TextBox style="width: 345px; height: 60px; overflow: auto;" TextMode="MultiLine"
                    runat="server" ID="txtReason" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                        <%--<asp:Button Text="提交" ID="btnSubmit" runat="server" OnClick="btnSubmit_click" CssClass="btn class1" />--%>
                        <asp:Button text="提交" ID="btnDenyRefund" runat="server" CssClass="btn class1" OnClick="denyRefund_Submited" OnClientClick="return CheckReasonInput()" />
                    <button class="btn class2 close" type="button">
                        取消</button>
                </td>
            </tr>
        </table>
        </div>
       <%-- <div class="btns">
            
            <input type="button" class="btn class2" value="返回" onclick="Cancle()" />
        </div>--%>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Provide/ProcessReturnMoney.aspx.js" type="text/javascript"></script>
