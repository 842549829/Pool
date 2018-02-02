<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetRole.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage.SetRole" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设置角色</title>
</head>
<body>
    <div class="form">
        <h3 class="titleBg"> 设置角色</h3>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w20" />
                <col />
            </colgroup>
            <tr>
                <td class="title">
                    用户名：
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    角色列表：
                </td>
                <td>
                    <div class="check">
                        <asp:CheckBoxList ID="chklUserRole" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
        </table>
        <div class="btns">
            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" OnClick="btnSave_Click" />
            <button class="btn class2" runat="server" id="btnGoBack">返回</button>
        </div>
        </form>
    </div>
</body>
</html>
