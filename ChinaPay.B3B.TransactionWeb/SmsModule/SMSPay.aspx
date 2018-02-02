<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSPay.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSPay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link href="/Styles/bank.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .con p
        {
            line-height: 1.5em;
            margin-bottom: 15px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="payContent">
        <div class="column">
                <h3 class="titleBg">
                    支付方式</h3>
            <div class="tab tab-a">
                <ul runat="server" id="divPayTypes" class="payNav">
                </ul>
                <div class="tab-con">
                    <strong class="large-font">应付金额：<asp:Label runat="server" ID="lblPayAmount" CssClass="price"></asp:Label></strong>
                    <div runat="server" id="divPayTypeDetails">
                    </div>
                </div>
            </div>
        </div>
        <div class="btns" runat="server" id="divOperations">
            <a class="btn class1" id="btnPay" target="_blank">确认支付</a>
            <input type="button" class="btn class2" id="btnWaiting" value="执行中" disabled="disabled"
                style="display: none" />
        </div>
    </div>
    <%-- 支付跳转提示 --%>
    <a id="payAttention" style="display: none" data="{type:'pop',id:'divPayAttention'}">
    </a>
    <div id="divPayAttention" class="layer">
        <h3>
            支付</h3>
        <div class="con">
            <strong>请您在新打开的支付页面上完成付款。</strong>
            <p>
                付款完成前请不要关闭此窗口。
            </p>
            <p>
                完成付款后请根据您的情况点击下面的按钮完成操作。</p>
        </div>
        <div class="btns">
            <a id="btnPayComplete" class="btn class1">已完成付款</a> <a id="btnPayError" class="btn class2">
                付款遇到问题</a>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidBusinessId" />
    <asp:HiddenField runat="server" ID="PayPoolBindAccount" />
    <asp:HiddenField runat="server" ID="hidBusinessType" />
   <%-- <asp:HiddenField runat="server" ID="hidPayHost" />--%>
    <asp:HiddenField runat="server" ID="hidUserName" />
    </form>
</body>
</html>

<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/widget/d.tabs.js" type="text/javascript"></script>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/pay.js?20130513" type="text/javascript"></script>