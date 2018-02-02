<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostponeApplyformDetail.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.PostponeApplyformDetail" %>
<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagPrefix="uc" TagName="Bill" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server" >
    <%--订单头部信息--%>
    <h2>申请单详情</h2>
    <div class="column form">
        <table class="breakAll">
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">申请单号</td>
                <td><asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label></td>
                <td class="title">申请类型</td>
                <td><asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label></td>
                <td class="title">状态</td>
                <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
            </tr>
             <tr>
                <td class="title">订单编号</td>
                <td><a runat="server" ID="linkOrderId" class="obvious-a"></a></td>
                <td class="title">产品类型</td>
                <td><asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label></td>
                <td class="title">客票类型</td>
                <td><asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label></td>
            </tr>
             <tr>
                <td class="title">编码</td>
                <td><asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label></td>
                <td class="title">申请时间</td>
                <td><asp:Label runat="server" ID="lblAppliedTime" CssClass="obvious"></asp:Label></td>
                <td class="title">处理时间</td>
                <td><asp:Label runat="server" ID="lblProcessedTime" CssClass="obvious"></asp:Label></td>
            </tr>
             <tr>
                <td class="title">申请原因</td>
                <td colspan="3"><asp:Label runat="server" ID="lblAppliedReason"></asp:Label></td>
                <td class="title">改期费</td>
                <td><asp:Label runat="server" ID="lblPostponeFee" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">拒绝原因</td>
                <td colspan="5"><asp:Label runat="server" ID="lblDeniedReason"></asp:Label></td>
            </tr>
        </table>
    </div>
    <%--原航班、机票信息--%>
    <uc:Voyage runat="server" ID="originalVoyages" />
    <uc:Passenger runat="server" ID="originalPassengers" />
    <%--新航班、申请信息--%>
    <uc:Voyage runat="server" ID="newVoyages" />
    <div class="column table">
        <div class="hd"><h2>申请信息</h2></div>
        <asp:Repeater runat="server" ID="applyInfos">
            <HeaderTemplate><table><tr><th>乘客姓名</th><th>乘客类型</th><th>证件号</th></tr></HeaderTemplate>
            <ItemTemplate><tr><td><%# Eval("Name") %></td><td><%# Eval("Type") %></td><td><%# Eval("Credentials") %></td></tr></ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>
    </div>
    <uc:Bill runat="server" ID="bill"/>
    <div class="btns">
        <button runat="server" id="btnPay" class="btn class1" visible="false">支&nbsp;&nbsp;&nbsp;&nbsp;付</button>
        <asp:Button runat="server" ID="btnCancel" CssClass="btn class2" Visible="false" Text="取消申请" OnClientClick="return confirm('确认取消?');" OnClick="btnCancel_Click" />
        <button runat="server" id="btnBack" class="btn class2">返&nbsp;&nbsp;&nbsp;&nbsp;回</button>
    </div>
    </form>
</body>
<script src="../../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
</html>
