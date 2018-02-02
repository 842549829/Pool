<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderHistoryRecord.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.OrderHistoryRecord" %>

<%@ Register src="UserControls/RefundFormView.ascx" tagname="RefundFormView" tagprefix="uc1" %>
<%@ Register src="UserControls/PostPoneView.ascx" tagname="PostPoneView" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8">
	<title>订单历史记录</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <style type="text/css">
        .hover-tips {
            bottom: -10px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">订单历史记录</h3>
     <ul class="form" style="line-height:30px; border-bottom:2px solid #cecece; border-top:2px solid #cecece; padding:0px;">
        <li class="title name">订单编号：</li>
        <li>
             <asp:Label runat="server" CssClass="obvious" ID="lblOrderId"></asp:Label>
        </li>
    </ul>
    <asp:Repeater runat="server" ID="applyFormList" 
        onitemdatabound="applyFormList_ItemDataBound">
        <itemtemplate>
            <uc2:PostPoneView ID="ucPostPoneView" runat="server" />
            <uc1:RefundFormView ID="ucRefundFormView" runat="server" />
        </itemtemplate>
    </asp:Repeater>
    <div class="btns">
        <button class="btn class2" type="button" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
<script type="text/javascript" src="/Scripts/widget/jquery-ui-1.8.21.custom.min.js"></script>
<script type="text/javascript" src="/Scripts/widget/form-ui.js"></script>
<script type="text/javascript" src="/Scripts/widget/template.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script> 
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
   <script type="text/javascript">
        function copyToClipboard(data) {
            window.clipboardData.setData('text', data);
        }

    </script>
   </form>
</body>
</html>
