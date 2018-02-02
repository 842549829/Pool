<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="PostponeApplyformDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.PostponeApplyformDetail" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
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
            <tbody>
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
                        <a runat="server" id="linkOrderId" class="obvious-a"></a>
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
                    <td>
                        <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        申请时间：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedTime" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        处理时间：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProcessedTime" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        改期费：
                    </td>
                    <td> <asp:Label runat="server" ID="lblPostPoneFee" CssClass="obvious"></asp:Label>
                       
                    </td>
                    <td class="title">
                        申请原因：
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblAppliedReason"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <%--原航班、机票信息--%>
    <uc:Voyage runat="server" ID="originalVoyages" />
    <uc:Passenger runat="server" ID="originalPassengers" />
    <%--新航班、申请信息--%>
    <uc:Voyage runat="server" ID="newVoyages" />
    <div class="column table">
        <div class="hd">
            <h2>
                申请信息</h2>
        </div>
        <asp:Repeater runat="server" ID="applyInfos">
            <headertemplate><table><tr><th>乘客姓名</th><th>乘客类型</th><th>证件号</th></tr></headertemplate>
            <itemtemplate><tr><td><%# Eval("Name") %></td><td><%# Eval("Type") %></td><td><%# Eval("Credentials") %></td></tr></itemtemplate>
            <footertemplate></table></footertemplate>
        </asp:Repeater>
    </div>
    </form>
    <div class="btns">
        <button runat="server" id="btnBack" class="btn class2">
            返&nbsp;&nbsp;&nbsp;&nbsp;回</button>
    </div>
</body>
</html>
