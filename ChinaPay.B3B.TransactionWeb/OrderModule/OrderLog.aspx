<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderLog.aspx.cs" EnableViewState="false"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.OrderLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>B3B机票平台</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="column table">
        <h3 class="titleBg">
            订单号：
            <asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></h3>
        <asp:Repeater runat="server" ID="logContent">
            <HeaderTemplate>
                <table>
                    <colgroup>
                        <col class="w8" />
                        <col class="w8" />
                        <col />
                        <col class="w8" />
                        <col class="w8" />
                    </colgroup>
                    <tr>
                        <th>
                            操作项
                        </th>
                        <th>
                            操作时间
                        </th>
                        <th>
                            详情
                        </th>
                        <th>
                            操作人
                        </th>
                        <th>
                            申请单号
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Keyword") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "OperateTime")%>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Detail")%>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Operator")%>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Applyform")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="btns">
            <button class="btn class2" runat="server" id="btnBack">
                返&nbsp;&nbsp;&nbsp;回</button></div>
    </div>
    </form>
</body>
</html>
