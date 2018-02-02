<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.ConfigurationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
	<link rel="stylesheet" href="/Styles/skin.css" />
<body>
    <form id="form1" runat="server">
	<div class="column table">
         <asp:GridView ID="rptConfig" runat="server" AutoGenerateColumns="false" 
             onrowcommand="rptConfig_RowCommand">
            <Columns>
              <asp:BoundField DataField="Login" HeaderText="用户名" />
              <asp:BoundField DataField="OfficeNumber" HeaderText="Office号" />
              <asp:BoundField DataField="Host" HeaderText="服务器" />
              <asp:BoundField DataField="Port" HeaderText="端口" />
              <asp:BoundField DataField="SI" HeaderText="SI" />
              <asp:BoundField DataField="PrinterSN" HeaderText="打票机" />
              <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                   <a href="ConfigurationAddOrUpdate.aspx?configId=<%#Eval("Id") %>">修改</a>
                   <asp:LinkButton ID="lnkDel" runat="server" OnClientClick="return confirm('确定要删除嘛？')" CommandName="del" CommandArgument='<%#Eval("Id") %>'>删除</asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
         </asp:GridView>
         <table>
           <tr>
				<td colspan="8"><a href="ConfigurationAddOrUpdate.aspx">立即添加出票配置</a></td>
			</tr>
         </table>
	</div>
	<div class="tips-box radius">
		<i class="icon icon-attention-circle"></i> <strong>技术支持</strong> <span>如果您在使用中出现任何问题，欢迎咨询我公司客服QQ：<asp:Label runat="server" ID="lblEnterpriseQQ"></asp:Label> 或致电全国免费客服热线：<asp:Label runat="server" ID="lblServicePhone"></asp:Label>
</span>
	</div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
