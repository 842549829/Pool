<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="UpdateTicketNo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.UpdateTicketNo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server">
    <div class="column table">
        <div class="hd"><h2>修改票号</h2></div>
        <div class="table">
            <asp:Repeater runat="server" ID="ticketContents">
                <HeaderTemplate>
                    <table><tr><th>乘机人姓名</th><th>乘机人类型</th><th>证件号</th><th>原票号</th><th>新票号</th><th></th></tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Type") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Credentials")%></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "SettleCode") %>-<label id="originalTicketNo"><%# DataBinder.Eval(Container.DataItem, "Original") %></label></td>
                    <td><input type="text" class="text" id="newTicketNo" style="display:none;" /></td>
                    <td><input type="button" class="btn class1 editButton" value="修&nbsp;&nbsp;&nbsp;改" onclick="editTicketNo(this);" />
                        <input type="button" class="btn class1 updateButton" value="保&nbsp;&nbsp;&nbsp;存" style="display:none" onclick="updateTicketNo(this);" />
                        <input type="button" class="btn class2 cancelButton" value="取&nbsp;&nbsp;&nbsp;消" style="display:none" onclick="cancelEditTicketNo(this);" /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidOrderId" />
    </form>
    <div class="btns"><button class="btn class1" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button></div>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/OrderModule/updateTicketNo.js" type="text/javascript"></script>