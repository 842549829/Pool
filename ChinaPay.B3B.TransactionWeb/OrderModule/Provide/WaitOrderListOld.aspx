<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaitOrderListOld.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.WaitOrderListOld" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>待出票订单</title>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
 </head>
   <style type="text/css">
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
    </style>
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <div class="hd">
        <h3 class="titleBg">
            待出票订单</h3>
    </div>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w40" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">订单编号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index" style="z-index: 51">
                            <span class="name">航空公司：</span>
                            <asp:DropDownList runat="server" ID="ddlFlightCompany" AppendDataBoundItems="True">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">创建日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span><asp:TextBox runat="server" ID="txtPNR" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">OFFICE号：</span>
                            <asp:DropDownList runat="server" ID="ddlOfficeNumber" AppendDataBoundItems="True">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">产品类型：</span><asp:DropDownList runat="server" ID="ddlProduct" AppendDataBoundItems="True">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text  textarea"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button type="button" ID="btnQuery" class="btn class1 submit" runat="server"
                            Text="查 询" OnClick="btnQuery_Click" OnClientClick="SaveSearchCondition('WaitOrderList')"/>
                        <input type="button" value="清空条件" onclick="ResetSearchOption()" class="btn class2" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="speedTip">您的出票效率直接影响您的票量，请尽快出票<a href="/ReportModule/ProvideETDZSpeedStatisticReport.aspx" class="pad-l">查看出票效率&gt;&gt;</a></div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate" EnableViewState="False">
            <headertemplate>
              <table id="dataListTable">
              <colgroup>
                  <col class="w10"/>
                  <col class="w8"/>
                  <col class="w20"/>
                  <col class="w8"/>
                  <col class="w10"/>
                  <col class="w10"/>
                  <col class="w8"/>
                  <col class="w7"/>
                  <col class="w15"/>
                  <col/>
              </colgroup>
		<thead>
		<tr>
			<th>订单号</th>
			<th>PNR</th>
			<th>航程</th>
			<th>乘机人</th>
			<th>票面价（元）<br />民航基金/燃油</th>
			<th>结算价（元）<br />返佣</th>
			<th>创建时间</th>
            <th>采购商</th>
			<th>OFFICE号<br/>
                效率(分钟)
            </th>
		    <th>锁定信息<br />
			    操作
			</th>
		</tr>
		</thead>
		<tbody>
        </headertemplate>
            <itemtemplate>
		    <tr <%#(bool)Eval("TodaysFlight")?"class='redBackground'":"" %>>
		        <td><a href="OrderDetail.aspx?id=<%# Eval("OrderId") %>" class="obvious-a"><%# Eval("OrderId")%></a><br />
                <%#Eval("ProductType")%><span <%#(bool)Eval("IsChildTicket")?"class='greenBolder'":"class='Bolder'" %>>(<%#Eval("PassengerType")%>)</span>
                </td>
                <td><span class="obvious"><%# Eval("PNR") %></span></td>
                <td><%#Eval("AirportPair")%></td>
                <td><%# Eval("Passenger") %></td>
                <td><%# Eval("Fare") %><br /><%# Eval("AirportFee") %>/<%# Eval("BAF") %></td>
                <td><%# Eval("SettleAmount") %><br /><%# Eval("Rebate") %>/<%# Eval("Commission") %></td>
				<td><%#Eval("ProducedTime")%></td>
                <td><span class="purchaseTypes <%#(bool)Eval("PurchaseIsBother")?"brother":"lower" %>"><%#Eval("Relation")%></span>
                   <span style='display:<%#(bool)Eval("RemindIsShow")?"":"none"%>'><a href='javascript:;' class='obvious urgent_btn urgent_order' content='<%#Eval("RemindContent") %>'>采购催单</a></span>
                </td>
		        <td><%#Eval("OfficeNum") %> <%#Eval("TicketType")%>  <br /> <%#Eval("EmergentOrderContnt")%>  <span class="waitTime"><%#Eval("ETDZTime")%></span></td>
			    <td>
                    <%#Eval("LockInfo")%>
                    <a href="ETDZ.aspx?id=<%#Eval("OrderId") %>" class="obvious-a font-c b">出票</a>
                </td>
		</tr>
        </itemtemplate>
        <footertemplate></tbody></table></footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="importantBox" style="margin:15px 0;">
        <p class="important">红色字样订单为当天订单,建议酌情优先处理；出票效率中5分钟以内为绿色；5-10分钟为橙色；10分钟以上为红色；出票效率将直接影响到您的票量</p>
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
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script type="text/javascript">
    SaveDefaultData();
    //FixTable("dataListTable", 10, 760);
    $("#txtOrderId").OnlyNumber().LimitLength(13);
    pageName = 'WaitOrderList';
    $(".waitTime").each(function ()
    {
        var that = $(this);
        var waitMininute = parseFloat(that.text());
        if (waitMininute <= 5)
        {
            that.css("color", "#009900");
        } else if (waitMininute <= 10)
        {
            that.css("color", "#ff8c01");
        } else
        {
            that.css("color", "#ed1c24");
        }

    });
    $(".tips_btn").live("mouseenter", function () {
        var self = $(this);
        var the = self.next();
        $(".tips_box1").addClass("tips_box").removeClass("tips_box1");
        the.removeClass("hidden").css({ "left": self.offset().left - 110, "top": self.offset().top + 10 });
        var h = the.height();
        var top = the.offset().top;
        var sor = $(window).scrollTop();
        var wh = $(window).height();
        if (h + top - sor > wh) {
            the.css({ top: (top - h - 32) }).addClass("tips_box1").removeClass("tips_box");
        };
    }).live("mouseleave", function () {
        $(".tips_box").mouseenter(function () { $(this).removeClass("hidden"); }).mouseleave(function () { $(this).addClass("hidden"); });
        $(".tips_box1").mouseenter(function () { }).mouseleave(function () { $(this).addClass("hidden"); });
        $(".tips_box1,.tips_box").addClass("hidden");
    });
    $(function () {
        var urg = $(".urgent_orderDiv");
        $(".urgent_order").mouseenter(function () {
            $(".urgent_content p").html($(this).attr("content"));
            urg.removeClass("hidden");
            urg.css("left", $(this).offset().left - 100);
            urg.css("top", $(this).offset().top + 12);
        })
        urg.mouseleave(function () {
            urg.addClass("hidden");
        });
    })
</script>