<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupInfoAddOrUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.GroupInfoAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>公司信息管理</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
<div class="hd">
   <h2><asp:Label ID="lblAddOrUpdate" runat="server">添加</asp:Label>分组信息</h2>	
</div>
<div class="form">
	 <form id="form1" runat="server">
		<table>
			<tr>
				<td class="title">分组名称</td>
				<td><asp:TextBox ID="txtGroupName" runat="server" CssClass="text"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="title">分组排序</td>
				<td><asp:TextBox ID="txtGroupOrder" runat="server" CssClass="text"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="title">分组描述</td>
				<td><textarea id="txtDescription" cols="30" rows="10" class="text" runat="server"></textarea></td>
			</tr>
		</table>
        <div class="btns">
           <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" 
                        onclick="btnSave_Click" />
                    <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='OnLineServiceSet.aspx'" />
        </div>
	</form>
</div>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {
            var groupNameObj = $("#txtGroupName");
            var groupName = $.trim(groupNameObj.val());
            if (groupName == "") {
                alert("请输入分组名称！");
                groupNameObj.focus();
                return false;
            } else {
                if (groupName.length > 25) {
                    alert("分组名称位数不能超过25位！");
                    groupNameObj.select();
                    return false;
                }
            }
            var groupOrderObj = $("#txtGroupOrder");
            var groupOrder = $.trim(groupOrderObj.val());
            if (groupOrder == "") {
                alert("请输入分组排序！");
                groupOrderObj.focus();
                return false;
            } else {
                var orderPattern = /^[0-9]+$/;
                if (!orderPattern.test(groupOrder)) {
                    alert("分组排序只能为数字！");
                    groupOrderObj.select();
                    return false;
                }
            }
            if ($.trim($("#txtDescription").val()).length > 100) {
                alert("分组描述不能超过100位！");
                $("#txtDescription").select();
                return false;
            }
        });
    })
</script>
