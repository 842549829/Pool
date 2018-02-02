<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaitOrderListNew.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.WaitOrderListNew" %>

<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>待出票订单</title>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <style type="text/css">
          .addTicketNo{ margin-left:5px; display:inline-block; width:20px; height:22px; background:red; color:#ffffff; cursor:pointer; text-align:center;}
        .removeTicketNo{margin-left:5px; display:inline-block; width:20px; height:22px; background:red; color:#ffffff; cursor:pointer; text-align:center;}
        .settleCode
        {
             width:30px;
        }
          .ticketNOend
        {
            width: 40px;
        }
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
          .greenBolder
        {
            font-weight: 900;
            color:#009900;
        }
        .Bolder
        {
            font-weight: 900;
        }
        .redBackground,.redBackground td
        {
            color:red !important;
        }
        .redBackground .waitTime
        {
            color:red !important;
        }
        .speedTip
        {
            text-align:right;
            padding:0 0 10px;
            color: #4A5164;
        }
        .speedTip a
        {
            color:blue;
        }
        div .speedTip
        {
             float:right;
        }
    </style>
</head>
<body>
    <form runat="server" id="form">
    <h3 class="titleBg">
        待处理订单</h3>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <div class="col">
                <span><b>订单编号：</b>
                    <asp:TextBox runat="server" ID="txtOrderId" class="text textarea"></asp:TextBox>
                </span><span><b>PNR：</b>
                    <asp:TextBox runat="server" ID="txtPNR" class="text textarea"></asp:TextBox>
                </span><span><b>乘机人：</b>
                    <asp:TextBox runat="server" ID="txtPassenger" class="text  textarea"></asp:TextBox>
                </span>
            </div>
            <div class="col">
                <span><b>航空公司：</b>
                    <asp:DropDownList runat="server" ID="ddlCarrier" AppendDataBoundItems="True">
                        <asp:ListItem Text="全部" Value="" />
                    </asp:DropDownList>
                </span><span><b>OFFICE号：</b>
                    <asp:DropDownList runat="server" ID="ddlOfficeNumber" AppendDataBoundItems="True">
                        <asp:ListItem Value="">全部</asp:ListItem>
                    </asp:DropDownList>
                </span><span>
                    <b>  订单状态：</b><asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="True">
                                                           </asp:DropDownList>
                   
                </span>
            </div>
            <div class="col">
                <span><b class="name">创建日期：</b>
                    <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                    </asp:TextBox>
                    -
                    <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdDefaultDate" />
                </span><span><b class="name">产品类型：</b><asp:DropDownList runat="server" ID="ddlProduct"
                    AppendDataBoundItems="True">
                    <asp:ListItem Value="">全部</asp:ListItem>
                </asp:DropDownList>
                </span>
                <span>
                   <input type="button" value="查询" id="btnSerach" class="btn class1" />
                </span>
            </div>
        </div>
    </div>
    <div>
    </div>
    <div>
        <input type="checkbox" id="autoRefresh" /><label for="autoRefresh" id="autoRefreshlabel">自动刷新订单</label>
        <span id="countdownSize" style="display: none;">&nbsp;&nbsp;&nbsp; 倒计时<label id="countdownId"></label>
        </span>
        <label class="speedTip" id="speed" runat="server">
            您的出票效率直接影响您的票量，请尽快出票<a href="/ReportModule/ProvideETDZSpeedStatisticReport.aspx"
                class="pad-l">查看出票效率&gt;&gt;</a></label></div>
    <div class="table" id='data-list'>
        <table id='dataList'>
            <thead>
                <tr>
                    <th>
                        订单号
                    </th>
                    <th>
                        PNR
                    </th>
                    <th>
                        航程
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
                  <%--  <th>
                      支付时间
                    </th>--%>
                    <th>
                        采购商
                    </th>
                    <th>
                        订单状态
                    </th>
                    <th>
                        OFFICE号<br />
                        效率(分钟)
                    </th>
                    <th>
                        锁定信息<br />
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
        <h2>
            催单内容</h2>
        <div class="tips_bd urgent_content">
            <p>
                催单内容</p>
        </div>
    </div>
    <asp:HiddenField ID="hfdOrderId" runat="server" />
    <asp:HiddenField ID="hfdRequireChangePNR" runat="server" />
    <asp:HiddenField ID="hfdForbidChangPNR" runat="server" />
    <%--拒绝出票原因--%>
    <a id="lnkDeny" data="{type:'pop',id:'divDeny'}" style="display: none"></a>
    <div id="divDeny" class="form" style="display: none; background-color: white; padding: 20px;
        width: 500px;">
        <h2>
            拒绝出票</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tr>
                <td class="title">
                    类型
                </td>
                <td>
                    <div class="check">
                        <input type="radio" id="reasonType1" value='<%=((int)SystemDictionaryType.RefuseETDZSelfReason).ToString() %>'
                            name="radioone" /><label for="reasonType1">自身原因</label>
                        <input type="radio" id="reasonType2" value='<%=((int)SystemDictionaryType.RefuseETDZPlatformReason).ToString() %>'
                            name="radioone" /><label for="reasonType2">平台原因</label>
                        <input type="radio" id="reasonType3" value='<%=((int)SystemDictionaryType.RefuseETDZPurchaseReason).ToString() %>'
                            name="radioone" /><label for="reasonType3">采购原因</label>
                        <input type="radio" id="reasonType4" value='<%=((int)SystemDictionaryType.RefuseETDZOtherReason).ToString() %>'
                            name="radioone" /><label for="reasonType4">其他原因</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">
                    原因
                </td>
                <td>
                    <select id="selDenyReason">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">
                    描述详情
                </td>
                <td>
                    <textarea id="txtDenyReason" rows="5" cols="50" class="text"></textarea>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <input class="btn class1" type="button" value="提&nbsp;&nbsp;&nbsp;交" onclick="commitDenyETDZ();" />
                    <input class="btn class2 close" type="button" value="返&nbsp;&nbsp;&nbsp;回" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfdDenyETDZOrderId" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?2013117" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20130403" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
    var ServerTime = Date.fromString('<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm") %>');
    $(function(){
       for (var i = 0; i <  $(".uc").length; i++) {
            $(".uc").eq(i).find("div").css("z-index",5-parseInt(i));
        }
    });
</script>
<script src="/Scripts/OrderModule/Provide/WaitOrderList.aspx.js?2013041804" type="text/javascript"></script>
<script type="text/javascript" src="../../Scripts/OrderModule/Provide/WaitOrderListOperator.js?2013051001"></script>
<script src="/Scripts/OrderModule/EmergentOrder.js" type="text/javascript"></script>
