<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>重置密码</title>
</head>
<body>
    <div class="btns">
    <h2>
        重置密码原因：</h2>
    <textarea class="text" runat="server" id="remark"></textarea>
    </div>
    <div class="btns">
        <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" 
            onclick="btnSave_Click" />
       <input type="button" class="btn class2" onclick="window.location.href='./CompanyList.aspx?Search=Back'"/>
    </div>
</body>
</html>
