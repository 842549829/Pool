<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.CustomerModule.CustomerList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>常旅客管理</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">常旅客管理</h3>
    <!-- 选择常旅客 -->
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">姓名：</span>
                            <asp:TextBox ID="txtName" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">证件号：</span>
                            <asp:TextBox ID="txtCertId" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">手机：</span>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button ID="btnQuery" CssClass="btn class1" Text="查询" runat="server" OnClick="btnQuery_Click" />
                        <input type="button" value="添加常旅客" class="btn class1" onclick="return window.location.href='./CustomerAddOrUpdate.aspx';" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id="data-list">
        <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" OnRowCommand="dataSource_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="姓名" DataField="Name" />
                <asp:BoundField HeaderText="性别" DataField="Sex" />
                <asp:BoundField HeaderText="乘机人类型" DataField="PassengerType" />
                <asp:BoundField HeaderText="证件类型" DataField="CredentialsType" />
                <asp:BoundField HeaderText="证件号" DataField="Credentials" />
                <asp:BoundField HeaderText="手机号码" DataField="Mobile" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='CustomerAddOrUpdate.aspx?customerId=<%#Eval("Id") %>'>修改</a>
                        <asp:LinkButton ID="lnkDel" CommandName="del" CommandArgument='<%#Eval("Id") %>'
                            runat="server" OnClientClick='return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnQuery").click(function () {
            if ($.trim($("#txtName").val()).length > 25) {
                alert("姓名格式错误！");
                $("#txtName").select();
                return false;
            }
            if ($.trim($("#txtCertId").val()).length > 50) {
                alert("证件号格式错误！");
                $("#txtCertId").select();
                return false;
            }
            var contactPhoneObj = $("#txtPhone");
            var contactPhone = contactPhoneObj.val();
            if ($.trim(contactPhone) != "") {
                var phonePattern = /^1[3458]\d{9}$/;
                if (!phonePattern.test(contactPhone)) {
                    alert("手机号码格式错误！");
                    contactPhoneObj.select();
                    return false;
                }
            }
        });
    })
</script>
