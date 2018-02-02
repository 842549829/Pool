<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalPlatformList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.ExternalPlatformList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
      外平台接口
      </h3>
    <div class="table">
        <asp:GridView ID="grdExternalPlatform" runat="server" 
            AutoGenerateColumns="false" 
            onrowcommand="grdExternalPlatform_RowCommand" 
            onrowdatabound="grdExternalPlatform_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Platform" HeaderText="接口平台" />
                <asp:BoundField DataField="Enabled" HeaderText="状态" />
                <asp:BoundField DataField="Deduct" HeaderText="留点%" />
                <asp:BoundField DataField="PayInterfaceText" HeaderText="支付方式" />
                <asp:BoundField DataField="RebateBalance" HeaderText="政策差%" />
                <asp:BoundField DataField="ProviderAccount" HeaderText="出票方账号" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href='ExternalPlatformSetting.aspx?Platform=<%#Eval("PlatformValue") %>'>修改</a>
                        <asp:LinkButton runat="server" ID="lnkEnable" CommandArgument='<%#Eval("PlatformValue") %>'
                            CommandName="enable">启用</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkDisable" CommandArgument='<%#Eval("PlatformValue") %>'
                            CommandName="disable">禁用</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
