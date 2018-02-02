<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyformList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ApplyformList" EnableEventValidation="false"
    EnableViewState="false" %>

<%@ Register Src="../../UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>退改签查询【运营商】</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20130301" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        select
        {
            width: 130px;
            height: 28px;
            padding: 2px;
            background-color: #FFFFFF;
            border: 1px solid #DDDDDD;
            color: #666666;
            outline: medium none;
        }
        .input
        {
            padding-left: 70px;
        }
        .input .name
        {
            width: 70px;
        }
        .text.fl
        {
            float: none;
        }
        .orderCondition span
        {
            padding-right: 0;
            width: 260px;
        }
        .orderCondition span b
        {
            display: inline-block;
            text-align: right;
            width: 65px;
        }
        .orderCondition span .btn
        {
            margin-left: 70px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        退改签查询</h3>
    <ul class="orderNav clearfix" id="applyType" runat="server">
    </ul>
    <div class="orderBox">
        <ul class="orderSubnav clearfix" id="refundType" runat="server">
        </ul>
        <div class="orderCondition clearfix">
            <span><b>申请日期：</b>
                <asp:TextBox ID="txtAppliedDateStart" class="text text-s" value="日期插件" runat="server"
                    onfocus="WdatePicker({isShowClear:false,readOnly:true})" />
                <asp:TextBox ID="txtAppliedDateEnd" class="text text-s" value="日期插件" runat="server"
                    onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})" />
            </span><span><b>乘机人：</b>
            <asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox></span>
            <span><b>PNR：</b>
            <asp:TextBox runat="server" ID="txtPNR" class="text">
            </asp:TextBox></span><span><b>申请单号：</b>
                <asp:TextBox ID="txtApplyformId" class="text" runat="server" /></span>
            <span>
                <b>采购方：</b>
                <uc1:Company ID="PurchaseCompany" runat="server" />
            </span>
            <span><b>出票方：</b><uc1:Company ID="AgentCompany" runat="server" /></span>
            <span><b>产品方：</b><uc1:Company ID="SupplierCompany" runat="server" /></span>
            <span><input type="button" class="btn class1 fl" id="btnQuery" value="查询" /></span>
        </div>
        <ul class="orderSubnav clearfix" id="refundStatus" runat="server">
        </ul>
        <ul class="orderSubnav clearfix" id="posponeStauts" runat="server">
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
                    <th>
                        申请类型
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
    <asp:HiddenField ID="hfdApplyformType" runat="server" />
    <asp:HiddenField ID="hfdRefundType" runat="server" />
    <asp:HiddenField ID="hfdRefundStatus" runat="server" />
    <asp:HiddenField ID="hfdPosponeStatus" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
</script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js?2013117"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20130228" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/PlatformQueryApplyform.js?20130522" type="text/javascript"></script>
<script src="/Scripts/OrderModule/PlatformPosponeOrRefundApplyform.js?20130522" type="text/javascript"></script>
