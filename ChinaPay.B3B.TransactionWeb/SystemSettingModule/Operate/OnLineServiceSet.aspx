<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="OnLineServiceSet.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.OnLineServiceSet" %>

<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        #ftbContainer {
            border: none;
        }
        #ftbContainer td,#ftbContainer table {
            margin: 0;
            padding: 0;
            border: none;
            height: auto;
        }
        #ftbContainer * {
            vertical-align: middle;   
        }
        select {/*Fix Global select css*/
            height: auto;
            width: auto;
            color: black;   
        }
    </style>
<body>
    <form id="form1" runat="server">
<div class="hd">
   <h3 class="titleBg">在线客服设置</h3>
 </div>
   <div class="form">
		<table>
			<tr>
				<td class="title">标题</td>
				<td><asp:TextBox ID="txtTitle" runat="server" CssClass="text"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="title">内容</td>
				<td id="ftbContainer">
                <FTB:FreeTextBox ID="ftbContent" runat="server" Width="650px">
            </FTB:FreeTextBox>
                </td>
			</tr>
            
            <tr>
             <td colspan="2" class="btns">
               <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" 
                        onclick="btnSave_Click" />
             </td>
            </tr>
		</table>

</div>

<h3 class="titleBg">分组管理</h3>
<div class="table">
    <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" 
        onrowcommand="dataSource_RowCommand">
        <Columns>
            <asp:BoundField HeaderText="分组名称" DataField="Name" />
            <asp:BoundField HeaderText="分组描述" DataField="Description" />
            <asp:TemplateField HeaderText="操作">
              <ItemTemplate>
                <a href='GroupInfoAddOrUpdate.aspx?devideGroupId=<%#Eval("Id") %>'>修改</a>
                <a href='MemberManager.aspx?devideGroupId=<%#Eval("Id") %>'>成员管理</a>
                <asp:LinkButton ID="lnkDel" CommandName="del" CommandArgument='<%#Eval("Id") %>' runat="server" OnClientClick= 'return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
              </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="btns">
        <input type="button" class="btn class1" value="新增" onclick="javascript:window.location.href='GroupInfoAddOrUpdate.aspx'" />
    </div>
</div>
</form>
</body>
</html>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {
            var titleObj = $("#txtTitle");
            var title = $.trim(titleObj.val());
            if (title == "") {
                alert("请输入标题！");
                titleObj.focus();
                return false;
            } else {
                if (title.length > 100) {
                    alert("标题字数不能超过100！");
                    titleObj.select();
                    return false;
                }
            }
            var contentObj = $("#ftbContent");
            var content = $.trim(contentObj.val());
            if (content == "") {
                alert("请输入内容！");
                contentObj.focus();
                return false;
            } else {
                if (content.length > 20000) {
                    alert("内容字数不能超过20000！");
                    contentObj.select();
                    return false;
                }
            }
        });
    })
</script>
