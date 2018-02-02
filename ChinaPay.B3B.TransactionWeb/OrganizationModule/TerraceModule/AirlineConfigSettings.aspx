<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirlineConfigSettings.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.AirlineConfigSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <title>指令配置设置</title>
</head>    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />

<body>
    <form runat="server" id="form1">
    <h3 class="titleBg">
        指令配置设置</h3>
    <asp:Repeater runat="server" ID="SettingList">
        <ItemTemplate>
            <div class="O_formBox">
                <span>
                    <%#Eval("UseType") %>：</span><br />
                <asp:HiddenField runat="server" ID='hdUserTypeValue' Value='<%#Eval("UseTypeValue") %>' />
                <asp:TextBox runat="server" ID="txtUserName" CssClass="text" Value='<%#Eval("UserName") %>' />
                OfficeNO:
                <asp:TextBox runat="server" ID="txtOfficeNO" CssClass="text"  Width="50px" Value='<%#Eval("OfficeNO") %>' />

                <span class="muted">
                    <%#Eval("UseType") %></span>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button Text="提交" ID="btnSaveConfig" CssClass="btn class1" OnClick="btnSaveConfig_Click" runat="server" />
    <input type="button" runat="server" id="back" class="btn class2" name="back" value="取消" onclick="location.href='DistributionOemAuthorizationList.aspx?Search=Back'" />
    </form>
</body>
</html>
