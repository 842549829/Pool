<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="CredentialsUpdateLog.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.CredentialsUpdateLog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="column table">
        <h3 class="titleBg">订单编号：<asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></h3>
        <asp:Repeater runat="server" ID="logContent">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>操作时间</th>
                        <th>操作人</th>
                        <th>乘机人</th>
                        <th>原证件号</th>
                        <th>新证件号</th>
                        <th>状态</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "OperateTime")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Operator")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Passenger") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Original")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "New")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Status")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="btns"><button class="btn class2" runat="server" id="btnBack">返 回</button></div>
    </div>
    </form>
</body>
</html>