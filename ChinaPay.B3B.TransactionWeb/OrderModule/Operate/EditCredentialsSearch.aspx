<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCredentialsSearch.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.EditCredentialsSearch" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>修改证件号查询【运营方】</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
        <h3 class="titleBg">
            修改证件号</h3>
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
                            <span class="name">订单编号：</span>
                            <asp:TextBox id="txtOrderformId" class="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span>
                            <asp:TextBox runat="server" ID="txtPNR" class="text">
                            </asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="input">
                            <span class="name">航班日期：</span>
                            <asp:TextBox id="txtDateStart" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})" />
                            <asp:TextBox id="txtDateEnd" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})" /><asp:HiddenField runat="server" ID="hdDefaultDate"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button type="button" class="btn class1 submit" runat="server" ID="btnQuery"
                            Text="查&nbsp;&nbsp;&nbsp;&nbsp;询" onclick="btnQuery_Click" />
                        <input type="button" value="清空条件" class="btn class2" onclick="ResetSearchOption()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" OnItemCommand="DealOrder" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                    <table>
		<thead>
			<tr>
				<th>订单号</th>
				<th>PNR</th>
				<th>航程</th>
				<th>航班号<br />舱位/折扣</th>
				<th>起飞时间</th>
				<th>乘机人</th>
                <th>原证件号</th>
                <th>采购方</th>
                <th>新证件号</th>
				<th>提交时间</th>
                <th>状态</th>
				<th>操作</th>
			</tr>
		</thead> 
		<tbody>

    </headertemplate>
            <itemtemplate>
			 <tr>
                    <td><a href="javascript:Go('OrderDetail.aspx?id=<%# Eval("OrderId") %>')" class="obvious-a"><%# Eval("OrderId")%></a></td>
				<td><span class="obvious"><%#Eval("PNR")%></span></td>
				<td><%# Eval("AirportPair") %></td>
				 <td><%# Eval("FlightInfo") %></td>
				 <td><%# Eval("TakeoffTime") %></td>
                    <td><%# Eval("PassengerName") %></td>
                    <td><%#Eval("OriginalCredentials")%></td>
                   <td><%#Eval("PurchaserName")%></td>
                   <td><%#Eval("NewCredentials")%></td>
				<td><%#Eval("CommitTime")%></td>
                <td><%#Eval("Status") %></td>
				<td>
				    <asp:LinkButton text="已修改" CssClass="obvious" runat="server" Visible='<%#Eval("IsFail") %>' CommandName="Deal" CommandArgument='<%# Eval("Id") %>' />
                    <br />
                <a href="javascript:ViewLog('<%# Eval("OrderId") %>','<%#Eval("Passenger") %>')">查看日志</a>
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
    <script type="text/javascript" src="/Scripts/core/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/widget/template.js"></script>
    <script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script type="text/javascript">
        function Update(orderId) {
            location.href = '../UpdateCredentials.aspx?id=' + orderId + "&returnUrl=" + location.href;
        }
        function ViewLog(orderId, passengerId) {
            location.href = '../CredentialsUpdateLog.aspx?order=' + orderId + "&passenger=" + passengerId + "&returnUrl=" + location.href;
        }

        SaveDefaultData();
        $("#txtOrderformId").OnlyNumber().LimitLength(13);
    </script>
    </form>
</body>
</html>
