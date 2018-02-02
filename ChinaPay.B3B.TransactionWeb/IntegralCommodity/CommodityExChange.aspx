<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommodityExChange.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.CommodityExChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兑换处理</title>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        商品详情</h3>
    <div class="form">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w20" />
                <col class="w60" />
            </colgroup>
            <tr>
                <td class="title">
                    商品名称
                </td>
                <td>
                    <asp:Label ID="lblCommodityName" runat="server"></asp:Label>
                    <asp:Label ID="lblId" Visible="false" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    兑换数量
                </td>
                <td>
                    <asp:Label ID="lblCount" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所需积分
                </td>
                <td>
                    <asp:Label ID="lblIntegral" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司简称
                </td>
                <td>
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    申请账号
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    申请人
                </td>
                <td>
                    <asp:Label ID="lblAccountName" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    手机号码
                </td>
                <td>
                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    收货地址
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" CssClass="text" runat="server"></asp:TextBox>
                </td>
                <td>
                    请填写收货地址以便日后查证，虚拟物品留空即可
                </td>
            </tr>
            <tr>
                <td class="title">
                    快递公司
                </td>
                <td>
                    <asp:TextBox ID="txtCompany" CssClass="text" runat="server"></asp:TextBox>
                </td>
                <td>
                    请填写快递公司以便日后查证，虚拟物品留空即可
                </td>
            </tr>
            <tr>
                <td class="title">
                    快递单号
                </td>
                <td>
                    <asp:TextBox ID="txtNo" CssClass="text" runat="server"></asp:TextBox>
                </td>
                <td>
                    请填写快递单号以便日后查证，虚拟物品留空即可
                </td>
            </tr>
            <tr>
                <td class="title">
                    备注
                </td>
                <td colspan="2">
                    <asp:TextBox CssClass="text" ID="txtRemark" TextMode="MultiLine" Height="100px" Width="80%"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:Button runat="server" CssClass="btn class1" Text="提供兑换" ID="btnSave" OnClick="btnSave_Click" />
                    <asp:Button runat="server" CssClass="btn class1" Text="拒绝兑换" ID="btnRefuse" OnClick="btnRefuse_Click" />
                    <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='CommodityExChangeList.aspx';" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
