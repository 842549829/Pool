<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductMaintain.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain.ProductMaintain" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <form id="form1" runat="server">
        <h2>基础信息</h2>
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
                    姓名</td>
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
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    地址</td>
                <td>
                    <asp:Label ID="lblUserAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    &nbsp;邮政编码
                    </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
                <td class="title">
                    传真
                    </td>
                <td>
                    
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblPostCode">
                    </span>
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
                    <asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label>
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
            <tr>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblQQ"></span>
                </td>
                <td class="title">
                    使用期限</td>
                <td>
                    <asp:Label ID="lblBeginDeadline" runat="server" ></asp:Label>至
                    <asp:Label ID="lblEndDeadline" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button  ID="btnSvaeCompanyInfo" CssClass="btn class1" Text="保存" runat="server" onclick="btnSaveCompanyInfo_Click"/>
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
            <tr>
                <td class="title">
                    可提供资源航空公司
                </td>
                <td colspan="3">
                    <asp:CheckBoxList ID="chklAirline" runat="server" RepeatColumns="12" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false"></asp:CheckBoxList>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSaveChilder" runat="server" CssClass="btn class1" Text="保存"  onclick="btnSaveChilder_Click" />
                </td>
            </tr>
        </table>
        </form>
    </div>
    <script src="../../../Scripts/selector.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/CompanyInfoMaintain.js" type="text/javascript"></script>
</body>
</html>
