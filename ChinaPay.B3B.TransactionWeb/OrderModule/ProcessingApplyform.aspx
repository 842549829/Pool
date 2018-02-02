<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ProcessingApplyform.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.ProcessingApplyform" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>B3B机票平台</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="column table">
        <h3 class="titleBg">订单编号：<asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></h3>
        <div runat="server" id="divContent"></div>
        <div class="btns"><button class="btn class2" runat="server" id="btnBack">返 回</button></div>
    </div>
    </form>
</body>
</html>