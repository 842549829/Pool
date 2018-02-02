<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" ValidateRequest="false"
    Inherits="ChinaPay.B3B.InterfaceTest.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnPnrImport" Text="编码内容导入" OnClick="btnPnrImport_Click" />
        <asp:Button runat="server" ID="btnProductOrder" Text="生成订单" OnClick="btnProductOrder_Click" />
        <asp:Button runat="server" ID="btnPayOrder" Text="订单支付" OnClick="btnPayOrder_Click" />
        <asp:Button runat="server" ID="btnOrderDateil" Text="查询订单详情" OnClick="btnOrderDateil_Click" />
        <asp:Button runat="server" ID="btnJiexi" Text="解析支付地址" OnClick="btnJiexi_Click" />
        <asp:Button runat="server" ID="btnApplyRefund" Text="申请退废票" OnClick="btnApplyRefund_Click" />
        <asp:Button runat="server" ID="btnApplyPostpone" Text="申请改期" OnClick="btnApplyPostpone_Click" />
        <asp:Button runat="server" ID="btnPayApplyform" Text="改期支付" OnClick="btnPayApplyform_Click" />
        <asp:Button runat="server" ID="btnQueryApplyform" Text="查询申请详情" OnClick="btnQueryApplyform_Click" />
        <asp:Button runat="server" ID="btnPNRImportNoNeedPat" Text="不需要PAT内容导入" OnClick="btnPNRImportNoNeedPat_Click" />
        <asp:Button runat="server" ID="btnOnLineOrderPay" Text="在线支付订单" OnClick="btnOnLineOrderPay_Click" />
        <asp:Button runat="server" ID="btnPayApplyformByPayType" Text="在线改期支付" OnClick="btnPayApplyformByPayType_Click" />
        <asp:Button runat="server" ID="btnAutoPay" Text="代扣支付" OnClick="btnAutoPay_Click" />
        <asp:Button runat="server" ID="btnQueryFlights" Text="查询航班信息（多个）" OnClick="btnQueryFlights_Click" />
        <asp:Button runat="server" ID="btnQueryFlightStop" Text="查询经停" OnClick="btnQueryFlightStop_Click" />
        <asp:Button runat="server" ID="btnQueryFlight" Text="查询航班信息（舱位价格）" OnClick="btnQueryFlight_Click" />
        <asp:Button runat="server" ID="btnProduceOrder2" Text="预定生成订单" OnClick="btnProduceOrder2_Click" />
    </div>
    <div>
        <asp:Panel ID="PNRImportNoNeedPat" runat="server" Visible="false">
            <b>政策导入(PNRImport)：</b><br />
            编码信息(pnrContext):<asp:TextBox ID="pnrContext3" Columns="30" Rows="30" runat="server"
                TextMode="MultiLine" Height="300px" Width="100%"></asp:TextBox>编码详细信息和pat信息<br />
            <br />
        </asp:Panel>
        <asp:Panel ID="PNRImport" runat="server">
            <b>政策导入(PNRImport)：</b><br />
            编码信息(pnrContext):<asp:TextBox ID="pnrContext" Columns="30" Rows="30" runat="server"
                TextMode="MultiLine" Height="300px" Width="100%"></asp:TextBox>编码详细信息和pat信息<br />
            <br />
        </asp:Panel>
        <asp:Panel ID="ProduceOrder" runat="server" Visible="false">
            <b>生成订单(ProduceOrder)：</b><br />
            编码信息(pnrContext):<asp:TextBox ID="pnrContext1" CssClass="context" TextMode="MultiLine"
                Height="300px" Columns="30" Rows="30" Width="100%" runat="server"></asp:TextBox>编码详细信息和pat信息<br />
            <br />
            关联编码(associatePNR):<asp:TextBox ID="associatePNR" runat="server"></asp:TextBox><br />
            <br />
            联系信息(contact):<asp:TextBox ID="contact" runat="server"></asp:TextBox><br />
            <br />
            政策编号(policyId):<asp:TextBox ID="policyId" runat="server"></asp:TextBox><br />
            <br />
            导入批次(batchNo):<asp:TextBox ID="batchNo" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="OrderPay" runat="server" Visible="false">
            <b>订单支付(OrderPay)：</b><br />
            订单号(id):<asp:TextBox ID="orderId" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="QueryOrder" runat="server" Visible="false">
            <b>查询订单详情(QueryOrder)：</b><br />
            订单号(id):<asp:TextBox ID="orderDId" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="Jiexi" runat="server" Visible="false">
            <b>解析支付地址：</b><br />
            支付地址:<asp:TextBox ID="zhifu" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="ApplyRefund" runat="server" Visible="false">
            <b>申请退废票(ApplyRefund)：</b><br />
            订单号:<asp:TextBox ID="txtOrderId2" runat="server"></asp:TextBox><br />
            <br />
            乘机人信息:<asp:TextBox ID="txtPersons" runat="server"></asp:TextBox><br />
            <br />
            航段信息:<asp:TextBox ID="txtFlights" runat="server"></asp:TextBox><br />
            <br />
            申请类型:<asp:TextBox ID="txtRefundType" runat="server"></asp:TextBox><br />
            <br />
            申请原因:<asp:TextBox ID="txtReason" runat="server"></asp:TextBox><br />
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="ApplyPostpone" runat="server" Visible="false">
            <b>申请改期(ApplyPostpone)：</b><br />
            订单号:<asp:TextBox ID="txtOrderId3" runat="server"></asp:TextBox><br />
            <br />
            乘机人信息:<asp:TextBox ID="txtPassengers" runat="server"></asp:TextBox><br />
            <br />
            航段信息:<asp:TextBox ID="txtVoyages" runat="server"></asp:TextBox><br />
            <br />
            申请原因:<asp:TextBox ID="txtReason1" runat="server"></asp:TextBox><br />
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="PayApplyform" runat="server" Visible="false">
            <b>改期支付(PayApplyform)：</b><br />
            申请单号:<asp:TextBox ID="txtOrderId4" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel ID="QueryApplyform" runat="server" Visible="false">
            <b>查询申请详情(QueryApplyform)：</b><br />
            申请单号:<asp:TextBox ID="txtOrderId5" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" ID="OnLineOrderPay" Visible="false">
            <b>在线支付订单(PayOrderByPayType)：</b> 订单号:<asp:TextBox ID="txtOrderId6" runat="server"></asp:TextBox><br />
            支付通道:<asp:TextBox runat="server" ID="txtBankInfo"></asp:TextBox><br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="PayApplyformByPayType">
            <b>在线改期支付(PayApplyformByPayType)</b> 订单号:<asp:TextBox ID="txtOrderId7" runat="server"></asp:TextBox><br />
            支付通道:<asp:TextBox runat="server" ID="txtPayType"></asp:TextBox><br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="AutoPay">
            <br />
            <b>代扣支付(AutoPay)</b><br />
            <br />
            订单号:<asp:TextBox ID="txtOrderId8" runat="server"></asp:TextBox><br />
            <br />
            支付通道:<asp:TextBox runat="server" ID="txtPayType1"></asp:TextBox><br />
            <br />
            订单类型:<asp:TextBox ID="txtOrderType" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="QueryFlights">
            <br />
            <b>查询航班信息（QueryFlights）</b><br />
            <br />
            出发地:<asp:TextBox ID="txtDeparture" runat="server"></asp:TextBox><br />
            <br />
            到达地:<asp:TextBox runat="server" ID="txtArrival"></asp:TextBox><br />
            <br />
            航班日期:<asp:TextBox ID="txtFlightDate" runat="server"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="QueryFlight">
            <br />
            <b>查询航班信息（QueryFlight）</b><br />
            <br />
            批次号:<asp:TextBox ID="txtBatchNo" runat="server"></asp:TextBox><br />
            <br />
            航空公司代码:<asp:TextBox ID="txtAirlineCode" runat="server"></asp:TextBox><br />
            <br />
            航班号:<asp:TextBox runat="server" ID="txtFlightNo"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="QueryFlightStop">
            <br />
            <b>查询经停信息（QueryFlightStop）</b><br />
            <br />
            航空公司代码:<asp:TextBox ID="txtAirlineCode1" runat="server"></asp:TextBox><br />
            <br />
            航班号:<asp:TextBox ID="txtFlightNo1" runat="server"></asp:TextBox><br />
            <br />
            航班日期:<asp:TextBox runat="server" ID="txtFlightDate1"></asp:TextBox><br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="ProduceOrder2">
            <br />
            <b>生成订单（ProduceOrder2）</b><br />
            <br />
            航班信息:<asp:TextBox ID="txtFlights1" runat="server"></asp:TextBox><br />
            <br />
            乘机人:<asp:TextBox ID="txtPassengers1" runat="server"></asp:TextBox><br />
            <br />
            联系方式:<asp:TextBox runat="server" ID="txtContact"></asp:TextBox><br />
            <br />
            政策类型:<asp:TextBox runat="server" ID="txtPolicyType"></asp:TextBox><br />
            <br />
        </asp:Panel>
        用户名(userName):<asp:TextBox ID="userName" runat="server" Text="123321"></asp:TextBox><br />
        <br />
        签名(sign):<asp:TextBox ID="sign" runat="server" Text="319bbb277fe24cdbb417e3ea3feceeb2"></asp:TextBox>
        <br />
        <br />
        <asp:Button runat="server" ID="btnOk" Text="确定" OnClick="btnOk_Click" />
        <br />
        <br />
        <b>返回结果：</b><br />
        <asp:TextBox TextMode="MultiLine" runat="server" Columns="30" Rows="30" Height="200px"
            Width="100%" ReadOnly="true" ID="txtMsg"></asp:TextBox>
        <br />
        <br />
        <br />
        <br />
    </div>
    </form>
</body>
</html>
