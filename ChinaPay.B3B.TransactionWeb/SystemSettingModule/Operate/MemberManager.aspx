<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberManager.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.MemberManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>公司信息管理</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="../../Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body>
<form id="form1" runat="server">
<div class="hd">
  <h2>成员管理</h2>
  </div>
<div class="table">

    <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" 
        onrowcommand="dataSource_RowCommand">
        <Columns>
            <asp:BoundField HeaderText="成员说明" DataField="Remark" />
            <asp:TemplateField HeaderText="QQ号码">
              <ItemTemplate>
                <%#Eval("QQ") %>
              </ItemTemplate>
              <ItemStyle CssClass="QQ" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
             <ItemTemplate>
               <a href='MermberAddOrUpdate.aspx?devideGroupId=<%=Request.QueryString["devideGroupId"] %>&memberId=<%#Eval("Id") %>'>修改</a>
               <asp:LinkButton ID="lnkDel" runat="server" CommandName="del" CommandArgument='<%#Eval("Id") %>' OnClientClick= 'return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
             </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button Text="新增" CssClass="btn class1" ID="btnAdd" runat="server" 
        onclick="btnAdd_Click" />
    <input type="button" value="返回" id="btnBack" class="btn class2" onclick="javascript:window.location.href='OnLineServiceSet.aspx'"/>
</div>
</form>
</body>
</html>
<script src="../../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../../Scripts/PolicyModule/City_ShowOrHidden.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".QQ").tipTip({ limitLength: 10 });
    })
</script>