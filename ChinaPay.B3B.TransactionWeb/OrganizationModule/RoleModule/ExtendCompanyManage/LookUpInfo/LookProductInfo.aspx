<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookProductInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LookUpInfo.LookProductInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>基本信息查看</title>
</head>    <link href="/Styles/icon/main.css" rel="stylesheet" type="text/css" />

<body>
    <div class="form">
        <h2>基础修改信息：</h2>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="25" />
                <col class="w25" />
                <col class="w15" />
                <col class="w45" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server" ></asp:Label>
                </td>
                <td class="title">
                    公司类型
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    姓名 
                </td>
                <td>
                    <asp:Label ID="lblUserName" runat="server"></asp:Label>
                </td>
                <td class="title">
                    昵称
                </td>
                <td>
                    <asp:Label ID="lblPetName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地
                    </td>
                <td>
                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                </td>
                <td class="title">
                    地址
                </td>
                <td>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮政编码
                </td>
                <td>
                    <asp:Label ID="lblPostCode" runat="server"></asp:Label>
                </td>
                <td class="title">
                    传真</td>
                <td>
                    <asp:Label ID="lblFaxes" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    联系人
                </td>
                <td>
                    <asp:Label ID="lblLinkman" runat="server"></asp:Label>
                </td>
                <td class="title">
                    联系人电话
                </td>
                <td>
                    <asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    E_Mail
                    </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </td>
                <td class="title">
                    MSN
                </td>
                <td>
                    <asp:Label ID="lblMSN" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:Label ID="lblQQ" runat="server"></asp:Label>
                </td>
                <td class="title">
                    使用期限
                </td>
                <td>
                    <asp:Label ID="lblBeginDeadline" runat="server" Text="2012-12-12"></asp:Label>至
                    <asp:Label ID="lblEndDeadline" runat="server" Text="2012-12-12"></asp:Label>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnGoBack" runat="server" Text="返回" CssClass="btn class2" />
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
