<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaAddOrUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.AreaAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>     <link rel="stylesheet" href="/Styles/public.css?20121118" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

<body>
    <form id="form1" runat="server">
       <h3 class="titleBg"><asp:Label ID="lblAddOrUpdate" runat="server">新增</asp:Label>销售区域</h3>	
    <div class="form">
       <table>
			<tr>
				<td class="title">区域名称</td>
				<td><asp:TextBox ID="txtAreaName" runat="server" CssClass="text"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="title">区域备注</td>
				<td><textarea name="" id="txtRemark" cols="30" rows="10" class="text" runat="server"></textarea></td>
			</tr>
			<tr>
				<td></td>
				<td>
                    <asp:Button CssClass="btn class1" ID="btnSave" runat="server" Text="保存" 
                        onclick="btnSave_Click" />
                     <input type="button" id="btnBack" class="btn class2" onclick="javascript:window.location.href='MarketingAreaList.aspx'" value="返回" />
				</td>
			</tr>
		</table>
        <asp:HiddenField ID="hfdAddOrUpdate" runat="server" />
     </div>
     <div id="errorMessage" runat="server" style="visibility:hidden"></div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {
            var areaNameObj = $("#txtAreaName")
            var areaName = $.trim(areaNameObj.val());
            if (areaName == "") {
                alert("请输入区域名称！");
                areaNameObj.focus();
                return false;
            } else {
                if (areaName.length > 25) {
                    alert("区域名称位数不能超过25位！");
                    areaNameObj.select();
                    return false;
                }
            }
            if ($.trim($("#txtRemark").val()).length > 50) {
                alert("区域备注位数不能超过50位！");
                $("#txtRemark").select();
                return false;
            }
        });
    })
</script>

