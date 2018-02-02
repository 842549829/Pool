<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.OEM.OrderList"
    EnableViewState="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单查询</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .text.fl
        {
            float: none;
        }
        .orderCondition .col
        {
            float: left;
            min-width: 255px;
            width: 33.3%;
        }
        .orderCondition .col span
        {
            padding-right: 0;
            float: none;
        }
        .orderCondition span b
        {
            display: inline-block;
            text-align: right;
            width: 65px;
        }
        .orderCondition .btns
        {
            margin: 0 auto;
        }
        .waitTime, .waitedTime
        {
            display:inline-block;
            font-family: "微软雅黑" , "宋体";
            font-weight: 900;
            height: 20px;
            vertical-align:middle;
        }
        .waitTime, .waitedTime
        {
            *display:inline;
        }
    </style>
</head>
<body>
    <form runat="server" id="form">
    <h3 class="titleBg">
        订单查询</h3>
    <ul class="orderNav clearfix" id="ulProduct" runat="server">
    </ul>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <div class="col">
                <span><b>创建时间：</b>
                    <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtStartTime" Text="00:00" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm'})"
                        CssClass="text text-s" />
                </span><span><b>截止时间：</b>
                    <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtEndTime" Text="23:59" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm'})"
                        CssClass="text text-s" />
                </span><span><b>订单编号：</b>
                    <asp:TextBox runat="server" ID="txtOrderId" CssClass="text"></asp:TextBox>
                </span>
            </div>
            <div class="col">
                <span><b>航空公司：</b>
                    <asp:DropDownList runat="server" ID="Carrier" AppendDataBoundItems="True">
                        <asp:ListItem Text="全部" Value="" />
                    </asp:DropDownList>
                </span><span><b>乘机人：</b>
                    <asp:TextBox runat="server" ID="txtPassenger" CssClass="text"></asp:TextBox>
                </span><span><b>PNR：</b>
                    <asp:TextBox runat="server" ID="txtPNR" CssClass="text"></asp:TextBox>
                </span>
            </div>
            <div class="col">
                <span>
                    <div style="z-index:3;" class="uc">
                            <b class="name">采购方：</b><uc:Company runat="server" ID="ucPurchaser" />
                    </div>
                </span>
                <span><input type="button" value="查询" id="btnSerach" class="btn class1" /></span>
            </div>
        </div>
        <ul class="orderSubnav clearfix" id="ulOrderStatus" runat="server">
        </ul>
    </div>
    <div class="table" id='data-list'>
        <table id='dataList'>
            <thead>
                <tr>
                    <th>
                        订单编号
                    </th>
                    <th>
                        PNR
                    </th>
                    <th>
                        行程
                    </th>
                    <th>
                        乘机人
                    </th>
                    <th>
                        票面价<br />
                        民航基金/燃油
                    </th>
                    <th>
                        结算价<br />
                        返佣
                    </th>
                    <th>
                        创建时间
                    </th>
                    <th>
                        采购
                    </th>
                    <th>
                        状态<br />
                        效率(分钟)
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
    <div id="emptyInfo" class="box hidden">
        没有任何符合条件的查询结果
    </div>
    <div class="btns" id="divPagination">
    </div>
     <div class="tips_box urgent_tips hidden urgent_orderDiv">
        <h2>催单内容</h2>
        <div class="tips_bd urgent_content">
            <p>催单内容</p>
        </div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?2013117" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20130228" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
    var ServerTime = Date.fromString('<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm") %>');
    var oemId = '<%=OEMID %>';
    $(function(){
       for (var i = 0; i <  $(".uc").length; i++) {
            $(".uc").eq(i).find("div").css("z-index",5-parseInt(i));
        }
    });
</script>
<script src="/Scripts/OrderModule/OEM/OrderList.aspx.js?2013032701" type="text/javascript"></script>
<script src="/Scripts/OrderModule/EmergentOrder.js" type="text/javascript"></script>
