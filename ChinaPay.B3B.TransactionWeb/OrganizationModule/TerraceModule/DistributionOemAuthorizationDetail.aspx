<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOemAuthorizationDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOemAuthorizationDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
</head> <link href="../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
<body>
    <div class="form">
        <h2>
            查看授权信息</h2>
        <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hfdCompayId" />
        <asp:HiddenField runat="server" ID="hfdOperatorId" />
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr>
                <td class="title">
                    授权时间：
                </td>
                <td>
                    <asp:Label ID="lblAuthorizationTime" runat="server"></asp:Label>
                </td>
                <td class="title">
                    开户数量：
                </td>
                <td>
                    <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
                    <a href="javascript:window.location.href = './CompanyInfoManage/LookUpComapanyRelation.aspx?CompanyId=' + document.getElementById('hfdCompayId').value + '&IsOem=true';">查看列表</a>
                </td>
            </tr>
            <tr>
                <td class="title">
                    用户名：
                </td>
                <td>
                    <asp:Label ID="lblUserNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                        授权状态：
                    </td>
                    <td>
                        <asp:Label ID="lblAuthorizationStatus" runat="server"></asp:Label>
                    </td>
                <%--<td class="title">
                    订单数量：
                </td>
                <td>
                    <asp:Label ID="lblCertNo" runat="server"></asp:Label>
                    <a href="/OrderModule/OEM/OrderList.aspx?OEMId=<%=OEMID %>">查看列表</a>
                </td>--%>
            </tr>
                <tr>
                    <td class="title">
                        公司简称：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        授权到期：
                    </td>
                    <td>
                        <asp:Label ID="lblAuthorizationDeadline" runat="server"></asp:Label>
                    </td>
                    <%--<td class="title">
                        月均交易额：
                    </td>
                    <td>
                        <asp:Label ID="lblMonthAmount" runat="server"></asp:Label>
                        <a href="#">查看最近一个月报表</a>
                    </td>--%>
                </tr>
                <tr>
                    <td class="title">
                        OEM名称：
                    </td>
                    <td>
                        <asp:Label ID="lblOemName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        授权操作人：
                    </td>
                    <td>
                        <a href="javascript:window.location.href = 'CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId='+ document.getElementById('hfdOperatorId').value;"> <asp:Literal ID="lblAuthorizationOperator" runat="server"></asp:Literal></a>
                    </td>
                    <%--<td class="title">
                        日均交易额：
                    </td>
                    <td>
                        <asp:Label ID="lblDayAmount" runat="server"></asp:Label>
                        <a href="#">查看昨日报表</a>
                    </td>--%>
                </tr>
            <tr>
                <td class="title">
                    授权域名：
                </td>
                <td>
                    <asp:Label ID="lblAuthorizationDomain" runat="server"></asp:Label>
                </td>
                <%--<td class="title">
                    交易峰值日期：
                </td>
                <td>
                    <asp:Label ID="lblTradingDate" runat="server"></asp:Label>
                    <a href="#">查看当日订单</a>
                </td>--%>
                <td class="title">
                        授权保证金：
                    </td>
                    <td>
                        <asp:Label ID="lblAuthorizationDeposit" runat="server"></asp:Label>
                    </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <input type="button" class="btn class2" value="返回" onclick="javascript:window.location.href='DistributionOemAuthorizationList.aspx'" />
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>