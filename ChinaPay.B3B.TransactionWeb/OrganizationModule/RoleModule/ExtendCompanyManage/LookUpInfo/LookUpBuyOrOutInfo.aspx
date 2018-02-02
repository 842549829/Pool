<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookUpBuyOrOutInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LookUpInfo.LookUpBuyOrOutInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>基本信息查看</title>
    
</head><link href="../../../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
<body>
    <div class="form">
        <h2>基础修改信息：</h2>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr>
                <td class="title">用户名</td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server" Text="xhadmin"></asp:Label>
                </td>
                <td class="title">公司类型</td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server" Text="采购方"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">公司名称</td>
                <td>
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </td>
                <td class="title">
                    公司简称 
                </td>
                <td>
                    <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地
                </td>
                <td>
                    <asp:Label runat="server" ID="lblLocation"></asp:Label>
                </td>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:Label ID="lblQQ" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司地址 
                </td>
                <td>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    邮政编码
                    </td>
                <td>
                        <asp:Label ID="lblPostCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司电话</td>
                <td>
                    <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>
                </td>
                <td class="title">
                    传真
                    </td>
                <td>
                        <asp:Label ID="lblFaxes" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">负责人</td>
                <td>
                    <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                </td>
                <td class="title">
                    负责人电话
                </td>
                <td>
                    <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
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
                    <asp:Label ID="lblLinkManPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    紧急联系人人</td>
                <td>
                        <asp:Label ID="lblUrgencyLinkMan" runat="server"></asp:Label>
                </td>
                <td class="title">
                    紧急联系人电话
                </td>
                <td>
                    <asp:Label ID="lblUrgencyLinkManPhone" runat="server"></asp:Label>
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
            <tr runat="server" id="trTime">
                <td class="title">
                    使用期限
                    </td>
                <td>
                    <asp:Label ID="lblBeginDeadline" runat="server"></asp:Label>至
                    <asp:Label ID="lblEndDeadline" runat="server"></asp:Label>
                </td>
                <td class="title"></td><td></td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <button class="btn class2" runat="server" id="btnGoBack">返回</button>
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
