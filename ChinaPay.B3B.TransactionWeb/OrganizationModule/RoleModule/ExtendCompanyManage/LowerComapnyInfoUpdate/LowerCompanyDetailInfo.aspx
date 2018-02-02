<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LowerCompanyDetailInfo.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate.LowerCompanyDetailInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head><link href="../../../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
<body>
    <div class="form">
        <h2>
            基础信息查看</h2>
        <form runat="server" id="form1">
        <table>
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名：
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                    公司类型：
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                    (<asp:Label ID="lblAccountType" runat="server"></asp:Label>)
                    <asp:Label ID="lblRelation" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="lblIndividual" runat="server">
                <td class="title">
                    真实姓名：
                </td>
                <td>
                    <asp:Label ID="lblTrueName" runat="server"></asp:Label>
                </td>
                <td class="title">
                    身份证号：
                </td>
                <td>
                    <asp:Label ID="lblCertNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tbody id="lblCompany" runat="server">
                <tr>
                    <td class="title">
                        公司名称：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        公司简称：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        公司电话：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        组织机构：
                    </td>
                    <td>
                        <asp:Label ID="lblOrginationCode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        负责人：
                    </td>
                    <td>
                        <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        负责人手机：
                    </td>
                    <td>
                        <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        紧急联系人：
                    </td>
                    <td>
                        <asp:Label ID="lblUrgencyLinkMan" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        紧急联系手机：
                    </td>
                    <td>
                        <asp:Label ID="lblUrgencyLinkManPhone" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
              <tr>
                <td class="title">
                    联系人：
                </td>
                <td>
                    <asp:Label ID="lblLinkman" runat="server"></asp:Label>
                </td>
                <td class="title">
                    联系人手机：
                </td>
                <td>
                    <asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地：
                </td>
                <td>
                    <asp:Label ID="lblLoaction" runat="server"></asp:Label>
                </td>
                <td class="title">
                    地址：
                </td>
                <td>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮政编码：
                </td>
                <td>
                    <asp:Label ID="lblPostCode" runat="server"></asp:Label>
                </td>
                <td class="title">
                    EMail：
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ：
                </td>
                <td>
                    <asp:Label ID="lblQQ" runat="server"></asp:Label>
                </td>
                <td class="title">
                    传真：
                </td>
                <td>
                    <asp:Label ID="lblFax" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
              <td class="title">是否已启用数据接口：</td>
              <td>
                <asp:Label ID="lblIsOpenExternalInterface" runat="server"></asp:Label>
              </td>
              <td class="title" id="fixedPhoneTitle" runat="server" visible="false">固定电话：</td>
              <td id="fixedPhoneValue" runat="server" visible="false">
                <asp:Label ID="lblFixedPhone" runat="server"></asp:Label>
              </td>
            </tr>
        </table>
        <div class="btns">
            <input type="button" class="btn class2" value="返回" id="btnBack" runat="server" />
        </div>
        </form>
    </div>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js"
        type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/UpdateLowerCompany.js"
        type="text/javascript"></script>
</body>
</html>
