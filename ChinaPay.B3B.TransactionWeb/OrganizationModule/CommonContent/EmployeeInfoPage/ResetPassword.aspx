<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage.ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>重置密码</title>
</head>
<body>
    <form runat="server">
    <div class="btns">
        <h2>重置密码原因：</h2>
        <textarea class="text" id="remark" runat="server"></textarea>
    </div>
    <div class="btns">
        <asp:Button ID="btnReset" runat="server" CssClass="class1 btn" Text="重置" OnClick="btnReset_Click" />
        <input type="button" onclick="return window.location.href='./StaffInfoMgr.aspx'" class="btn class2" value="返回" />
    </div>
    </form>
</body>
</html>
