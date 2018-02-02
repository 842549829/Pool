<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommodityList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.CommodityList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品维护列表</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        商品管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td colspan="2">
                        <center>
                            <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查 询" OnClick="btnQuery_Click" />
                            <input type="button" onclick="javascript:window.location.href='./CommodityAddOrUpdate.aspx';"
                                value="添加商品" class="btn class2" />
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table column">
        <asp:GridView ID="grv_commodity" runat="server" AutoGenerateColumns="false" OnRowCommand="grv_commodity_RowCommand">
            <Columns>
                <asp:BoundField DataField="Num" HeaderText="序号" />
                <asp:BoundField DataField="CommodityName" HeaderText="商品名称" />
                <asp:BoundField DataField="NeedIntegral" HeaderText="兑换积分" />
                <asp:BoundField DataField="StockNumber" HeaderText="库存数量" />
                <asp:BoundField DataField="SurplusNumber" HeaderText="剩余数量" />
                <asp:BoundField DataField="ExchangeNumber" HeaderText="已兑换数量" />
                <asp:BoundField DataField="State" HeaderText="商品状态" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <%#Eval("ShelvesInfo")%>
                        <asp:LinkButton runat="server" CommandArgument='<%#Eval("ID") %>' CommandName='<%#Eval("StateCmd") %>'><%#Eval("StateInfo")%></asp:LinkButton>
                        <a href='./CommodityAddOrUpdate.aspx?id=<%#Eval("ID") %>'>编辑</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <br />
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <asp:HiddenField ID="hidId" runat="server" />
    <asp:HiddenField ID="hidNum" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divCommodity'}"></a>
    <div id="divCommodity" class="form layer" style="display: none">
        <h4>
            商品上下架确认</h4>
        <table>
            <tr>
                <td class="title">
                    操作类型：
                </td>
                <td class="content">
                    <select runat="server" id="selOption">
                        <option value="0">上架</option>
                        <option value="1">下架</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">
                    数量：
                </td>
                <td class="content">
                    <asp:TextBox ID="txtNum" CssClass="text text-s" runat="server" />
                    件
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnOk" CssClass="btn class1" runat="server" Text="确定" OnClick="btnOk_Click" />
                    <input type="button" value="关闭" class="close btn class2" title="关闭" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnOk").click(function () {
            if ($("#selOption").val() == 1) {
                if (parseInt($("#txtNum").val()) > parseInt($("#hidNum").val())) {
                    alert("下架数量不能超过现有数量！");
                    $("#txtNum").val("");
                    $("#txtNum").focus();
                    return false;
                }
            }
            if ($("#txtNum").val() == "") {
                alert("上下架数量不能为空！");
                $("#txtNum").focus();
                return false;
            }
            var reg = /^[0-9]{1,10}?$/;
            if (!reg.test($("#txtNum").val())) {
                alert("上下架数量必须为整数！");
                $("#txtNum").val("");
                $("#txtNum").focus();
                return false;
            } return true;
        });
    });
    function ShangJia(id, num) {
        $("#hidId").val(id);
        $("#hidNum").val(num);
        $("#divOpcial").click();
    } 
</script>
