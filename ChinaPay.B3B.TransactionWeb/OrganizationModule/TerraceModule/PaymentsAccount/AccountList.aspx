<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.PaymentsAccount.AccountList" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8">
	<title>公司信息管理</title>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
</head>
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
   <h3 class="titleBg">收付款账号查询</h3>
	<div class="box-a">

		<div class="condition">
			<table>
				<colgroup>
					<col class="w30" />
					<col class="w30" />
					<col class="w30" />
					<col class="w10" />
				</colgroup>
				<tr>
					<td>
						<div class="input">
							<span class="name">公司简称：</span>
							<asp:TextBox id="txtAbbreviateName"  class="text"  runat="server"/>
						</div>
					</td>
			    <td>
						<div class="input">
							<span class="name">支付账号：</span>
							<asp:TextBox id="txtPaymentAccount" class="text"  runat="server"/>
							<i class="icon" title="选择城市"></i>					        
					  </div>
				  </td>
   					<td>
						<div class="input">
							<span class="name">用户名：</span>
							<asp:TextBox id="txtAdministrator" class="text" runat="server"/>
					  </div>
					</td>                 
				</tr>
				<tr>
					<td>
						<div class="input up-index">
							<span class="name">账号状态：</span>
                            <asp:DropDownList runat="server" ID="ddlEnabled">
                                <asp:listitem text="全部" Value="" />
                                <asp:listitem text="有效" Value="1" />
                                <asp:ListItem Text="无效" Value="0" />
                            </asp:DropDownList>
					  </div>
					</td>
                    <td>
                        <asp:Button class="btn class1" ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />&nbsp;&nbsp;
                        <input type="button" class="btn class2" onclick="ResetSearchOption()" value="清空条件"/>
                    </td>
				</tr>                
			</table>
		</div>
	</div>
	<div class="column table" id="data-list">
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate" EnableViewState="False">
            <HeaderTemplate>
                     	  <table>
			<tr>
				<th>公司类别</th>
				<th>公司简称</th>
				<th>用户名</th>
				<%--<th>账户信息</th>--%>
				<th>账号类型</th>
				<th>账号</th>
				<th>账号状态</th>
				<th>账号添加时间</th>
				<th>操作</th>             
		  </tr><tbody>
            </HeaderTemplate>
            <ItemTemplate>
			<tr>
				<td><%#Eval("CompanyType")%></td>
				<td><%#Eval("AbbreviateName")%></td>
				<td><%#Eval("Administrator")%></td>
				<%--<td><%#Eval("PaymentInterface")%></td>--%>
				<td><%#Eval("AccountType")%></td>
				<td><%#Eval("Account")%></td>
				<td><div><%#Eval("Enabled")%></div></td>
                <td><div><%#Eval("CreateTime", "{0:yyyy-MM-dd HH:mm}")%></div></td>
                <td><a href="PaymentsAccount.aspx?CompanyId=<%#Eval("CompanyId") %>" class="el-text" style='display:<%#(bool)Eval("CanAudit")?"inline":"none;" %>'>审核</a></td>
			</tr>
            </ItemTemplate>
            <FooterTemplate>
               </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
	</div>      
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <script type="text/javascript" src="/Scripts/core/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/widget/jquery-ui-1.8.21.custom.min.js"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    </form>
</body>
</html>
