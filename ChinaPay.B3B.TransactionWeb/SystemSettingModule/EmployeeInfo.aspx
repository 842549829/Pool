<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.EmployeeInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看员工信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            查看员工信息</h3>
        <table>
            <colgroup>
                <col class="w20" />
                <col  />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        姓名：
                    </td>
                    <td>
                         <asp:Label ID="lblName" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblLook" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        性别 ：</td>
                    <td>
                        <asp:Label runat="server" ID="lblSex"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        用户名：
                    </td>
                    <td>
                       <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        手机：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCellphone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        座机：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPhone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        E-mail：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        备注：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblRemark"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <input type="button" onclick="return window.location.href='./SuggestList.aspx'" class="btn class2" value="返回" />
        </div>
    </div>
    </form>
</body>
</html>
