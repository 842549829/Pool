<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.Logon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <title>B3B运维管理系统</title>
    <link href="css/logon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        if (window.top != window.self) {
            window.top.location = window.location;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
        <div class="title">B3B运维管理系统</div>
        <table>
             <tr>
                <td class="name">用户名：</td>
                <td>
                    <div class="userName">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="name">密&nbsp;码：</td>
                <td>
                    <div class="password">
                        <asp:TextBox ID="txtPassWord" runat="server" TextMode="Password" CssClass="text"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="name">验证码：</td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="text2"></asp:TextBox>
                    <img class="validateCode" id="imgValidateCode" onclick="javascript:loadValidateCode()" alt="换一张"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSubmit" runat="server" Text="登  录" CssClass="submit" 
                        OnClientClick="return validate()" onclick="btnSubmit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    function loadValidateCode() {
        $("#imgValidateCode").attr("src", '/ValidateCode.aspx?' + Math.random());
    }
    $(document).ready(function () {
        loadValidateCode();
    });
    function validate() {
        if ($("#txtUserName").val() == "") {
            alert("请输入帐号！");
            $("#txtName").focus();
            return false;
        }
        if ($("#txtCode").val() == "") {
            alert("请输入验证码！");
            $("#txtCode").focus();
            return false;
        }
        return true;
    }
</script>