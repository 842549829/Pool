<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProviderMaintain.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain.ProviderMaintain" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <form id="form1" runat="server">
        <h2>公司基础信息</h2>
        <table>
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                    公司类型
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司名称</td>
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
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblQQ"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司地址&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblCompanyAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    邮政编码
                    </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span  id="lblPostCode"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司电话
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span  id="lblCompanyPhone"></span>
                </td>
                <td class="title">
                    传真
                    </td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblFaxes"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    负责人
                </td>
                <td>
                    <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                </td>
                <td class="title">
                    负责人手机
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
                    联系人手机
                </td>
                <td>
                    <asp:Label ID="lblLinkManPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    紧急联系人 
                </td>
                <td>
                    <asp:Label ID="lblUrgencyLinkMan" runat="server"></asp:Label>
                </td>
                <td class="title">
                    紧急联系人手机
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
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblEmail"></span>
                </td>
                <td class="title">
                    MSN
                </td>
                <td>
                    <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblMSN"></span>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSvaeCompanyInfo" runat="server" Text="保存" 
                        CssClass="btn class1" onclick="btnSvaeCompanyInfo_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <h2>公司工作信息</h2>
                </td>
            </tr>
            <tr>
                <td class="title">默认出发城市</td>
                <td>
                    <uc:Airport ID="Departure" runat="server" />
                </td>
                <td class="title">默认到达城市</td>
                <td>
                    <uc:Airport ID="Arrival" runat="server" />
                </td>
            </tr>
        </table>
        <div class="btns">
                <asp:Button ID="btnSaveChilder" runat="server" CssClass="btn class1" Text="保存" onclick="btnSaveChilder_Click" />
        </div>
        </form>
    </div>
    <script src="../../../Scripts/selector.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/CompanyInfoMaintain.js" type="text/javascript"></script>
</body>
</html>
