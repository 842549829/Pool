<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyformDetail.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.OEM.ApplyformDetail" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register TagPrefix="uc" TagName="Bill" Src="~/OrderModule/UserControls/OrderBill.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>申请单详情</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server" >
    <%--订单头部信息--%>
    <h2>申请单详情</h2>
    <div class="column form">
        <table>
            <colgroup>
                <col  class="w10"/>
                <col  class="w20"/>
                <col  class="w10"/>
                <col  class="w20"/>
                <col  class="w10"/>
                <col  class="w20"/>
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
                <td><a runat="server" id="linkOrderId" class="obvious-a"></a></td>
                <td class="title">产品类型</td>
                <td><asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label></td>
                <td class="title">客票类型</td>
                <td><asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">编码</td>
                <td colspan="5"><asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label></td>
            </tr>
        </table>
    </div>
    <uc:Voyage runat="server" ID="voyages" />
    <uc:Passenger runat="server" ID="passengers" />
    

    <div class="column">
        <div class="hd"><h2>申请/处理信息</h2></div>
        <div class="table">
            <table>
                <tr><th>提交时间</th><th>原因</th><th>处理时间</th><th>处理结果</th></tr>
                <tr>
                    <td><asp:Label runat="server" ID="lblAppliedTime"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblAppliedReason"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblProcessedTime"></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblProcessedResult"></asp:Label></td>
                </tr>
            </table>
        </div>
        <div class="table" runat="server" id="divRefundFeeInfo"></div>
    </div>
    
        <%--账单信息--%>
    <uc:Bill runat="server" ID="bill"/>

    </form>
    <div class="btns">
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
</body>
</html>
<script src="/Scripts/core/jquery.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
