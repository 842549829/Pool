<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="UpdateCredentials.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.UpdateCredentials" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <div class="column table">
        <h3 class="titleBg">修改证件号</h3>
        <div class="table" runat="server" id="divPassengers">
            <asp:Repeater runat="server" ID="passengerContents">
                <HeaderTemplate>
                    <table><tr><th>乘机人姓名</th><th>乘机人类型</th><th>原证件号</th><th>新证件号</th><th></th></tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                    <td><label id='passengerName'><%# DataBinder.Eval(Container.DataItem, "Name") %></label></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Type") %></td>
                    <td><label id="originalCredentials"><%# DataBinder.Eval(Container.DataItem, "Credentials") %></label></td>
                    <td><input type="text" class="text" id="newCredentials" style="display:none;" /></td>
                    <td><input type="button" class="btn class1 editButton" value="修&nbsp;&nbsp;&nbsp;改" onclick="editCredentials(this);" />
                        <input type="button" class="btn class1 updateButton" value="保&nbsp;&nbsp;&nbsp;存" style="display:none" onclick="updateCredentials(this);" />
                        <input type="button" class="btn class2 cancelButton" value="取&nbsp;&nbsp;&nbsp;消" style="display:none" onclick="cancelEditCredentials(this);" /></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="btns"><button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button></div>
    <asp:HiddenField runat="server" ID="hidOrderId" />
    </form>
</body>
</html>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script src="../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/OrderModule/updateCredentials.js" type="text/javascript"></script>