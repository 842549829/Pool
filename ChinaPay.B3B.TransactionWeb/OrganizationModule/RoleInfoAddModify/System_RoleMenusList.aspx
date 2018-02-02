<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="System_RoleMenusList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleInfoAddModify.System_RoleMenusList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色管理</title>
 </head>
   <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        角色管理</h3>
    <div id="data-list" class="column  table">
        <div class="clearfix">
            <input type="button" value="添加角色" onclick="javascript:window.location.href='System_RoleInfo.htm';"
                class="btn class1" style="float: right; margin-top: 0px;" />
        </div>
        <br />
        <asp:GridView ID="grvRole" runat="server" AutoGenerateColumns="false" OnRowCommand="grvRole_RowCommand">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="角色名称" />
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <%#Eval("Valid").ToString() == "True" ? "启用" : "禁用" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <span class="Remark">
                            <%#Eval("Remark") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEnable" CommandName="enable" CommandArgument='<%#Eval("Id") %>'
                            runat="server"><%#Eval("Valid").ToString() == "True" ? "禁用" : "启用"%></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <a href='System_RoleInfo.htm?id=<%#Eval("Id") %>'>修改角色</a>&nbsp;&nbsp;&nbsp;&nbsp;<a
                            href='System_RoleMenus.htm?id=<%#Eval("Id") %>&name=<%#Eval("Name") %>'> 权限维护</a>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDel" CssClass="del" CommandName="del" CommandArgument='<%#Eval("Id") %>'
                            runat="server">删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
         <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    </form>
    <script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".Remark").tipTip({ limitLength: 20, maxWidth: "300px" });
            $(".del").click(function () {
                return confirm("是否删除？");
            });
        });
    </script>
</body>
</html>
