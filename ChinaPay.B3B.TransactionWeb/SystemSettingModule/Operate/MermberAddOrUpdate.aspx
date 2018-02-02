<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MermberAddOrUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.MermberAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
<form id="form1" runat="server">
<div class="column">
<div class="hd">
 <h2><asp:Label ID="lblAddOrUpdate" runat="server">新增</asp:Label>成员</h2>
 </div>
<div class="form">
		<table>
			<tr>
				<td class="title">成员说明</td>
				<td>
					<textarea id="txtMemberExplain" cols="30" rows="10" class="text" runat="server"></textarea>
				</td>
			</tr>
            <tr>
               <td class="title">排序字段</td>
               <td>
                 <asp:TextBox ID="txtSortLevel" runat="server" CssClass="text"></asp:TextBox>
               </td>
            </tr>
			<tr class="qqItem">
				<td class="title">QQ号码</td>
				<td>
                    <input type="text" class="text qq" />
					<span class="iconfont"><a href="#this" class="addQQ">添加</a> &nbsp;&nbsp;<a href="#this" class="removeQQ">删除</a>&nbsp;&nbsp;<a onclick="javascript:window.open('http://wp.qq.com/index.html')" class="qqOnline">开通“QQ在线状态”</a></span>
				</td>
			</tr>
		</table>
    <asp:HiddenField ID="hfdQQ" runat="server"/>
    </div>
    </div>
    <div class="btns">
        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" OnClick="btnSave_Click" />
        <asp:Button ID="btnReturn" runat="server" CssClass="btn class2" Text="返回" 
            onclick="btnReturn_Click" />
    </div>
	</form>
</body>
</html>
<script type="text/javascript">
    function initQQItems() {
        var qqItems = $.trim($("#hfdQQ").val());
        if (qqItems.length > 0) {
            var defaultItem = $(".qqItem");
            var items = qqItems.split(',');
            for(var i = 0 ;i<items.length;i++){
                if (i == 0) {
                    $(".qq", defaultItem).val(items[i]);
                } else {
                    var newItem = defaultItem.clone();
                    $(".qq", newItem).val(items[i]);
                    $(".removeQQ", newItem).show();
                    $(".qqOnline", newItem).hide();
                    $(".form table").append(newItem);
                }
            }
        }
    };
    $(function () {
        $(".removeQQ").hide();
        $(".addQQ").live("click", function () {
            var operateItem = $(this).parent().parent().parent();
            var newItem = operateItem.clone();
            $(".qq", newItem).val('');
            $(".removeQQ", newItem).show();
            $(".qqOnline", newItem).hide();
            operateItem.after(newItem);
        });
        $(".removeQQ").live("click", function () {
            var operateItem = $(this).parent().parent().parent();
            operateItem.remove();
        });
        $("#btnSave").click(function () {
            var remarkObj = $("#txtMemberExplain");
            var remark = $.trim(remarkObj.val());
            if (remark == "") {
                alert("请输入成员说明！");
                remarkObj.focus();
                return false;
            } else {
                if (remark.length > 25) {
                    alert("成员说明不能超过25位！");
                    remarkObj.select();
                    return false;
                }
            }
            var groupOrderObj = $("#txtSortLevel");
            var groupOrder = $.trim(groupOrderObj.val());
            if (groupOrder == "") {
                alert("请输入排序字段！");
                groupOrderObj.focus();
                return false;
            } else {
                var orderPattern = /^[0-9]+$/;
                if (!orderPattern.test(groupOrder)) {
                    alert("排序字段只能为数字！");
                    groupOrderObj.select();
                    return false;
                }
            }
            var qqPattern = /^\d{5,}$/;
            var qqItems = '';
            var validateFlg = true;
            $(".qq").each(function (index, item) {
                var qq = $.trim($(this).val());
                if (!qqPattern.test(qq)) {
                    alert("QQ号格式错误");
                    $(this).select();
                    validateFlg = false;
                    return false;
                } else {
                    if (index > 0) {
                        qqItems += ",";
                    }
                    qqItems += qq;
                }
            });
            if (validateFlg) {
                $("#hfdQQ").val(qqItems);
                return true;
            } else {
                return false;
            }
        });
        initQQItems();
    });
</script>