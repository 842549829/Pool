<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ApplyformLog.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.ApplyformLog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="column table">

            <h3 class="titleBg">申请单号：<asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label></h3>
        <asp:Repeater runat="server" ID="logContent" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>操作项</th>
                        <th>操作时间</th>
                        <th>详情</th>
                        <th>操作人</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "Keyword") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "OperateTime")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Detail")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Operator")%></td>
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