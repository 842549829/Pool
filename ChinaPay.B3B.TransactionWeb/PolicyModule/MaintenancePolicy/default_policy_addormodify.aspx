<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default_policy_addormodify.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.default_policy_addormodify" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加/修改默认政策</title>
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>	<link rel="stylesheet" href="/Styles/public.css?20130301" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />

<body>
    <form id="form1" class="form" runat="server">
		<h3 class="titleBg"><asp:Label ID="lblAddOrUpdate" runat="server" Text="添加"></asp:Label>普通默认政策</h3>
				<table class="table">
					<tr>
						<td class="title">航空公司：</td>
						<td colspan="3">
						  <asp:DropDownList ID="ddlAirlines" runat="server"></asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td class="title">成人默认出票方：</td>
						<td>
							<uc1:Company_2 ID="AdultAgentCompany" runat="server" />
						</td>
						<td class="title">成人默认返佣：</td>
						<td>
                            <asp:TextBox ID="txtAdult" runat="server" CssClass="text null"></asp:TextBox>%
						</td>
					</tr>
					<tr>
						<td class="title">儿童默认出票方：</td>
						<td>
							<uc1:Company_2 ID="ChildAgentCompany" runat="server" />
						</td>
						<td class="title">儿童默认返佣：</td>
						<td>
                            <asp:TextBox ID="txtChild" runat="server" CssClass="text null"></asp:TextBox>%
						</td>
					</tr>
				</table>
                <div class="btns">
                  <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" 
                                onclick="btnSave_Click" />
                            <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='default_policy_manage.aspx'" />
                </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {
            if ($.trim($("#ddlAirlines").val()) == "") {
                alert("请选择航空公司！");
                return false;
            }
            var aduitCommission = $.trim($("#txtAdult").val());
            var policyValuePattern = /^[0-9]{1,2}(\.[0-9])?$/;
            if ($.trim($("#AdultAgentCompany_txtCompanyName").val()) == "") {
                alert("请选择成人默认出票方！");
                return false;
            }
            if (aduitCommission == "") {
                alert("请填写成人默认返佣！");
                $("#txtAdult").focus();
                return false;
            } else {
                if (!policyValuePattern.test(aduitCommission)) {
                    alert("成人默认返佣格式错误！");
                    $("#txtAdult").select();
                    return false;
                }
            }
            var childCommission = $.trim($("#txtChild").val());
            if ($.trim($("#ChildAgentCompany_txtCompanyName").val()) == "") {
                alert("请选择儿童默认出票方！");
                return false;
            }
            if (childCommission == "") {
                alert("请填写儿童默认返佣！");
                $("#txtChild").focus();
                return false;
            } else {
                if (!policyValuePattern.test(childCommission)) {
                    alert("儿童默认返佣格式错误！");
                    $("#txtChild").select();
                    return false;
                }
            }
        });
    })
</script>
