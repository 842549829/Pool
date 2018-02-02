<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLower.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.AddLower" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下级用户开户</title>
</head>
<body>
    <div class="success-tips box">
        <div class="flow">
            <p class="current">下级公司类型：</p>
            <ul class="box-b">
                <li><input type="radio" value="6x" name="radioone" id="cheInternalOrganization" checked="checked" /><label for="cheInternalOrganization">内部机构</label></li>
                <li><input type="radio" value="6x" name="radioone" id="cheBuy" /><label for="cheBuy">采购方</label></li>
            </ul>
        </div>
        <input type="button" class="btn class1" value="确定" onclick="developLower();"/>
        <input type="button" class="btn class2" value="取消" onclick="window.location.href='./LowerComapnyInfoUpdate/Lower_manage.aspx'"/>
    </div>
    <script type="text/javascript">
        function developLower() {
            window.location.href = "../../CommonContent/AddAccount/ExtendOpenAccount.aspx?Type=" + (document.getElementById("cheInternalOrganization").checked ? "Subordinate" : "Purchaser");
        } 
    </script>
</body>
</html>
