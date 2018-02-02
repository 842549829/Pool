<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayOrderList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.PayOrderList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>待支付订单</title>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
</head>
<body>
    <form id="query" runat="server" DefaultButton="btnQuery">
    <h3 class="titleBg">待支付订单</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">订单编号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span><asp:TextBox runat="server" ID="txtPNR" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">创建日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index" style="z-index: 99">
                            <span class="name">产品类型：</span>
                            <asp:DropDownList runat="server" ID="ddlProduct" AppendDataBoundItems="True" Width="160px">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">员工帐号：</span>
                            <asp:DropDownList runat="server" ID="ddlOperator" AppendDataBoundItems="True" Width="160px">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click"   OnClientClick="SaveSearchCondition('PayOrder')"/>
                        <input type="button" onclick="ResetSearchOption()" class="btn class2" value="清空条件" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate" onitemcommand="dataList_ItemCommand">
            <headertemplate>
                <table id="dataListTable" >
                <colgroup>
                    <col class="w10"/>
                    <col class="w7"/>
                    <col class="w8"/>
                    <col class="w10"/>
                    <col class="w8"/>
                    <col class="w10"/>
                    <col class="w8"/>
                    <col class="w8"/>
                    <col class="w10"/>
                    <col class="w8"/>
                    <col class="w8"/>
                    <col/>
                </colgroup>
                    <thead>
                        <tr>
                            <th>订单号</th>
                            <th>产品<br />类型</th><th>PNR</th><th>航程</th><th>航班号<br />舱位/折扣</th><th>起飞时间</th><th>乘机人</th><th>票面价<br />民基/燃油</th><th>结算价<br />返佣</th><th>预订人</th><th>创建时间</th><th>操作</th></tr>
                    </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
                    <td><a href="javascript:GoTo('OrderDetail.aspx','<%#Eval("OrderId") %>')" class="obvious-a"><%# Eval("OrderId")%></a></td>
                    <td><%# Eval("Product") %></td>
                    <td><span class="obvious"><%# Eval("PNR") %></span></td>
                    <td><%# Eval("AirportPair") %></td>
                    <td><%# Eval("FlightInfo") %></td>
                    <td><%# Eval("TakeoffTime") %></td>
                    <td><%# Eval("Passenger") %></td>
                    <td><%# Eval("Fare") %><br /><%# Eval("AirportFee") %>/<%# Eval("BAF") %></td>
                    <td><%# Eval("SettleAmount") %><br /><%# Eval("RebateAndCommission")%></td>
                    <td><%# Eval("ProducedAccount") %><br /><%#Eval("ProducedAccountName")%></td>
                    <td><%# Eval("ProducedTime") %></td>
                    <td><a href="javascript:GoTo('OrderPay.aspx','<%# Eval("OrderId") %>')" class="obvious-a">支付</a>
                        <asp:LinkButton text="解锁并支付" ID="lbUnlockAndPay" runat="server" Visible='<%#Eval("UnLockEnable") %>' CommandName="UnlockAndPay" CommandArgument='<%#Eval("OrderId") %>' />
                    </td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody>
                </table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script type="text/javascript">
    SaveDefaultData();
    //FixTable("dataListTable", 11, 760);
    function GoTo(url,orderId) {
        location.href = url+"?id="+orderId+"&returnUrl="+location.href;
    }
    $("#txtOrderId").OnlyNumber().LimitLength(13);
    pageName = 'PayOrder';
</script>