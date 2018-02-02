<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BalanceRefundReturnMoneyList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.BalanceRefundReturnMoneyList" %>
<%@ Import Namespace="ChinaPay.B3B.DataTransferObject.Order" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
	<meta charset="UTF-8">
	<title>差错退款查询【出票方】</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
<div class="hd">
	<h3 class="titleBg">差错退款查询</h3>
</div>
<div class="box-a">
	<div class="condition">
		<table>
			<colgroup>
				<col class="w35" />
				<col class="w32" />
				<col class="w33" />
			</colgroup>
			<tr>
				<td>
					<div class="input">
						<span class="name">申请日期：</span>
					    <asp:TextBox id="txtAppliedDateStart" class="text text-s" value="日期插件"  runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})"/>-
						<asp:TextBox id="txtAppliedDateEnd" class="text text-s" value="日期插件"  runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"/><asp:HiddenField runat="server" ID="hdDefaultDate"/>
					</div>
				</td>
				<td>
					<div class="input">
						<span class="name">产品类型：</span>
                        <asp:DropDownList runat="server" ID="ddlProductType" AppendDataBoundItems="True">
                            <asp:listitem text="全部" Value="" />
                       </asp:DropDownList>
					</div>
				</td>
				<td>
					
					<div class="input">
						<span class="name">申请单号：</span>
						<asp:TextBox id="txtApplyformId"  class="text"  runat="server"/>
					</div>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input">
<span class="name">PNR：</span>
<asp:TextBox runat="server" ID="txtPNR" class="text"></asp:TextBox>		
			</div>
				</td>
				<td>
					<div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox>
                        </div>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td class="btns" colspan="3">
				    <asp:Button type="button" class="btn class1 submit" runat="server" ID="btnQuery"
                        Text="查&nbsp;&nbsp;&nbsp;&nbsp;询" onclick="btnQuery_Click"  />
				    <input type="button" onclick="ResetSearchOption()" class="btn class2" value="清空条件" />
				</td>
			</tr>
		</table>
	</div>
</div>
<div class="table" id='data-list' >
    <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate" EnableViewState="false" >
    <HeaderTemplate>
                    <table>
		<thead>
			<tr>
				<th>申请单号</th>
				<th>PNR</th>
				<th>航程</th>
				<th>航班号<br />舱位/折扣</th>
				<th>起飞时间</th>
				<th>乘机人</th>
				<th>申请类型</th>
				<th>状态</th>
				<th>申请时间</th>
                <th>锁定信息</th>
				<th>操作</th>
			</tr>
		</thead>
		<tbody>

    </HeaderTemplate>
        <ItemTemplate>
			 <tr>
                    <td><a href='<%#(ApplyformType)Eval("ApplyformType")==ApplyformType.BlanceRefund?"BalanceRefundApplyformDetail.aspx":"RefundApplyformDetail.aspx" %>?id=<%# Eval("ApplyformId") %>' class="obvious-a"><%# Eval("ApplyformId")%></a></td>
				<td><span class="obvious"><%#Eval("PNR") %></span></td>
				<td><%# Eval("AirportPair") %></td>
				 <td><%# Eval("FlightInfo") %></td>
				 <td><%# Eval("TakeoffTime") %></td>
                 <td><%# Eval("Passengers") %></td>
				<td><%#Eval("ApplyType") %></td>
				<td><%#Eval("ProcessStatus")%></td>
				<td><%#Eval("AppliedTime")%></td>
                <td style="color: red;"><%#Eval("LockInfo") %></td>
				<td><a href='ProcessBalanceRefundReturnMoney.aspx?id=<%# Eval("ApplyformId") %>' class="obvious">退款</a></td>
			</tr>
        </ItemTemplate>
        <FooterTemplate>
            		</tbody>
	</table>

        </FooterTemplate>
    </asp:Repeater>
</div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>


<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/template.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
 <script type="text/javascript">     SaveDefaultData();
     $("#txtApplyformId").OnlyNumber().LimitLength(13);
 </script>
    </form>
</body>
</html>
