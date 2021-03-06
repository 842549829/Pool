﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeProcessList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ChangeProcessList" EnableViewState="False"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>退票处理【运营商】</title>
</head>    <link rel="stylesheet" href="/Styles/public.css?20121118" />

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        退票处理</h3>
    <ul class="orderNav clearfix" id="refundType" runat="server">
    </ul>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <span>申请日期：
                <asp:TextBox ID="txtAppliedDateStart" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})" />
                <asp:TextBox ID="txtAppliedDateEnd" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})" />
            </span><span>乘机人：<asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox></span>
            <span>PNR：<asp:TextBox runat="server" ID="txtPNR" class="text  text-s"></asp:TextBox></span>
            <span>申请单号：<asp:TextBox ID="txtApplyformId" class="text" runat="server" /></span>
            <input type="button" class="btn class1 fl" id="btnQuery" value="查询" />
        </div>
        <ul class="orderSubnav clearfix" id="refundStatus" runat="server">
            <li><a href='javascript:void(0)' class='curr'>全部</a></li>
            <li><a href='javascript:void(0)'>等待平台处理</a></li>
            <li><a href='javascript:void(0)'>等待平台取消编码</a></li>
            <li><a href='javascript:void(0)'>待出票方处理</a></li>
            <li><a href='javascript:void(0)'>审核通过等待退款</a></li>
            <li><a href='javascript:void(0)'>拒绝退/废票待平台处理</a></li>
            <li><a href='javascript:void(0)'>退款审核不通过待处理</a></li>
        </ul>
    </div>
    <div class="table" id='data-list'>
        <table id="dataListTable" style="width: 100%">
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
                    <%--<th>
                        申请类型
                    </th>--%>
                    <th>
                        退票类型
                    </th>
                    <th>
                        状态
                    </th>
                    <th>
                        申请时间
                    </th>
                    <th>
                        锁定信息
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
    <div id="emptyInfo" class="box" style="display: none">
        没有任何符合条件的查询结果
    </div>
    <div class="btns" id="pager">
    </div>
    <asp:HiddenField ID="hfdRefundType" runat="server" />
    <asp:HiddenField ID="hfdRefundStatus" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
</script>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js?2013117"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/PlatformProcessApplyform.js?20130412" type="text/javascript"></script>