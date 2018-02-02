﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostPoneView.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.UserControls.PostPoneView" %>


<div class="column hover-tips">
		<h3 class="titleBg">改期</h3>
    <div class="column form">
        <table>
             <colgroup>
                <col class="w15" />
                <col class="w20" />
                <col class="w15" />
                <col class="w20" />
                <col class="w15" />
                <col class="w15" />
            </colgroup>
            <tr>
                <td class="title">申请单号：</td>
                <td><asp:LinkButton runat="server" ID="lblApplyformId" CssClass="obvious"></asp:LinkButton></td>
                <td class="title">申请时间：</td>
                <td><asp:Label runat="server" ID="lblAppliedTime" CssClass="obvious"></asp:Label></td>
                <td class="title">处理结果：</td>
                <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">改期费：</td>
                <td><asp:Label runat="server" ID="lblRefundFee" CssClass="obvious"></asp:Label></td>
                <td class="title">支付时间：</td>
                <td><asp:Label runat="server" ID="lblPaydTime" CssClass="obvious"></asp:Label></td>
                <td class="title">处理时间：</td>
                <td><asp:Label runat="server" ID="lblProcessedTime" CssClass="obvious"></asp:Label></td>
            </tr>
        </table>
    </div>
		<h3 class="titleBg">航班信息</h3>
	<div class="table">
		<table>
			<tbody>
				<tr>
					<td>乘机人：<asp:Label runat="server" ID="lblPassengers" CssClass="obvious"></asp:Label></td>
					<td>编码：<asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                    </td>
				</tr>
			</tbody>
		</table>
	</div>
	<div class="table">
		<table cellspacing="0">
			<tr>
				<th>航空公司</th>
				<th>原航班号</th>
				<th>机型</th>
				<th>航位/折扣</th>
				<th>出发城市</th>
				<th>到达城市</th>
				<th>航班日期</th>
				<th>起抵时间</th>
			</tr>
			<asp:Repeater runat="server" ID="dataList">
            <ItemTemplate>
        			 <tr>
				        <td><%#Eval("Carrier")%></td>
				        <td><%#Eval("FlightNo")%></td>
				        <td><%#Eval("AirCraft") %></td>
				        <td><%#Eval("Bank")%></td>
				        <td><%#Eval("Departure")%></td>
				        <td><%#Eval("Arrival")%></td>
				        <td><%#Eval("TakeoffTime","{0:yyyy-MM-dd}")%></td>
				        <td><%#Eval("TakeoffTime", "{0:HH:mm}")%> - <%#Eval("LandingTime","{0:HH:mm}")%></td>
			        </tr>
            </ItemTemplate>
        </asp:Repeater>
		</table>
		<table cellspacing="0">
			<tr>
				<th>航空公司</th>
				<th>航班号</th>
				<th>机型</th>
				<th>航位/折扣</th>
				<th>出发城市</th>
				<th>到达城市</th>
				<th>航班日期</th>
				<th>起抵时间</th>
			</tr>
			<asp:Repeater runat="server" ID="dataListNew">
            <ItemTemplate>
        			 <tr>
				        <td><%#Eval("Carrier")%></td>
				        <td><%#Eval("FlightNo")%></td>
				        <td><%#Eval("AirCraft") %></td>
				        <td><%#Eval("Bank")%></td>
				        <td><%#Eval("Departure")%></td>
				        <td><%#Eval("Arrival")%></td>
				        <td><%#Eval("TakeoffTime","{0:yyyy-MM-dd}")%></td>
				        <td><%#Eval("TakeoffTime", "{0:HH:mm}")%> <%#Eval("LandingTime","{0:HH:mm}")%></td>
			        </tr>
            </ItemTemplate>
        </asp:Repeater>
		</table>

	</div>
</div>