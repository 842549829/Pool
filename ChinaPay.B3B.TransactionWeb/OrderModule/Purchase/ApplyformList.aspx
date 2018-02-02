﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyformList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.ApplyformList" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>退改签查询</title>
 </head>
   <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        退改签查询</h3>
    <ul class="orderNav clearfix" id="applyType" runat="server">
    </ul>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <span>申请日期：<asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>-
                <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
            </span><span>乘机人：<asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox></span>
            <span>PNR：<asp:TextBox runat="server" ID="txtPNR" class="text  text-s"></asp:TextBox></span>
            <span>申请单号：<asp:TextBox runat="server" ID="txtApplyformId" class="text"></asp:TextBox></span>
            <input type="button" id="btnQuery" class="btn class1 fl" value="查询" />
        </div>
        <ul class="orderSubnav clearfix" id="refundStatus" runat="server"></ul>
        <ul class="orderSubnav clearfix" id="posponeStauts" runat="server"></ul>
        <ul class="orderSubnav clearfix" id="balanceRefundstatus" runat="server"></ul>
    </div>
    <div class="table" id='data-list'>
        <table>
            <thead>
                <tr>
                    <th>
                        申请单号
                    </th>
                    <th>
                    产品类型
                    </th>
                    <th>
                        PNR
                    </th>
                    <th>
                        航程
                    </th>
                    <th>
                        航班号<br />
                        舱位/折扣
                    </th>
                    <th>
                        起飞时间
                    </th>
                    <th>
                        乘机人
                    </th>
                    <th>
                        申请类型
                    </th>
                    <th>
                        状态
                    </th>
                    <th>
                        申请人
                    </th>
                    <th>
                        申请时间
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div id="emptyInfo" class="box" style="display:none">没有任何符合条件的查询结果</div>
    <div class="btns" id="pager"></div>
    <asp:HiddenField ID="hfdApplyformType" runat="server" />
    <asp:HiddenField ID="hfdRefundStatus" runat="server" />
    <asp:HiddenField ID="hfdPosponeStatus" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
</script>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?2013117" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/PurchaseQueryApplyform.js?20130524" type="text/javascript"></script>
<script src="/Scripts/OrderModule/PosponeOrRefundApplyForm.js?2013117" type="text/javascript"></script>