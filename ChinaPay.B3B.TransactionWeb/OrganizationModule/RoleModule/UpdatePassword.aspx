<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePassword.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.UpdatePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
        <div class="form">
            <h3 class="titleBg">修改密码</h3>
            <form  id="form1" runat="server">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col  class="w65"/>
                </colgroup>
                <tbody>
                    <tr>
                        <td class="title">
                            原密码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOriginalPassword" runat="server" CssClass="text" TextMode="Password" onpaste="return false;"></asp:TextBox>
                            <span class="tips" runat="server" id="lblOriginalPassword"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            新密码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="text" TextMode="Password" onpaste="return false;"></asp:TextBox>
                            <span class="tips" runat="server" id="lblNewPassword"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            确认密码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text" TextMode="Password" onpaste="return false;"></asp:TextBox>
                            <span class="tips" runat="server" id="lblConfirmPassword"></span>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btns">
                <asp:Button  ID="btnConfirmUpdate" runat="server" Text="确认修改" CssClass="class1 btn" onclick="btnConfirmUpdate_Click"/>
            </div>
            </form>
        </div>
    <script src="../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/UpdatePassWord.js" type="text/javascript"></script>
</body>
</html>
