<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.OrderList"
    EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单查询</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <style type="text/css">
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

<body>
    <form runat="server" id="form">
    <h3 class="titleBg clearfix">
        <span class="fl">订单查询</span><a href="/About/download.aspx" target="_parent" class="download fr obvious-a">下载订单提醒工具</a></h3>
    <ul class="orderNav clearfix" id="ulProduct" runat="server">
    </ul>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <span>创建日期：
                <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>-
                <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
            </span><span>乘机人：
                <asp:TextBox runat="server" ID="txtPassenger" CssClass="text"></asp:TextBox>
            </span><span>PNR：
                <asp:TextBox runat="server" ID="txtPNR" CssClass="text text-s"></asp:TextBox>
            </span><span>订单编号：
                <asp:TextBox runat="server" ID="txtOrderId" CssClass="text"></asp:TextBox>
            </span>
            <input type="button" value="查询" id="btnSerach" class="btn class1 fl" />
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
                        状态<br />
                        效率(分钟)
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
    <div runat="server" id="divEfficiency" class="importantBox broaden">
        <p class="important">出票效率中5分钟以内为绿色；5-10分钟为橙色；10分钟以上为红色；出票效率将直接影响到您的票量</p>
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
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script type="text/javascript">
    var IsFirstLoad= <%=IsFirstLoad?"true":"false" %>;
    var ServerTime = Date.fromString('<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm") %>');
</script>
<script src="/Scripts/OrderModule/Provide/OrderList.aspx.js?2013030801" type="text/javascript"></script>
<script src="/Scripts/OrderModule/EmergentOrder.js" type="text/javascript"></script>
