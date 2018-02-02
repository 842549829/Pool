<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommodityOEMExchange.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.CommodityOEMExchange" %>
    
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兑换处理列表</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        兑换处理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td colspan="2">
                        时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>至
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;处理状态：
                        <select id="XiaoFei" runat="server">
                            <option value="99">所有</option>
                            <option value="0">未处理</option>
                            <option value="1">已处理(成功兑换)</option>
                            <option value="2">已处理(拒绝兑换)</option>
                        </select>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查 询" OnClick="btnQuery_Click" />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table column">
        <asp:GridView ID="grv_xiaofei" runat="server" AutoGenerateColumns="false" EnableViewState="false">
            <Columns>
                <asp:BoundField DataField="ExchangeTiem" HeaderText="时间" />
                <asp:BoundField DataField="CommodityName" HeaderText="商品名称" />
                <asp:BoundField DataField="Count" HeaderText="兑换数量（件）" />
                <asp:BoundField DataField="Integral" HeaderText="所需积分" />
                <asp:BoundField DataField="CompanyShortName" HeaderText="公司简称" />
                <asp:BoundField DataField="AccountNo" HeaderText="申请账号" />
                <asp:BoundField DataField="Phone" HeaderText="手机号码" />
                <asp:BoundField DataField="Exchange" HeaderText="处理状态" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <%#Eval("Remark")%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <br />
    <div class="btns">
        <uc:pager runat="server" id="pager" visible="false"></uc:pager>
    </div>
    <asp:HiddenField runat="server" ID="id" />
    <asp:Button runat="server"  ID="btnSave" onclick="btnSave_Click" CssClass="hide" />
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    function shengqi(id) {
        $("#id").val(id);
        $("#btnSave").click();
     }
</script>