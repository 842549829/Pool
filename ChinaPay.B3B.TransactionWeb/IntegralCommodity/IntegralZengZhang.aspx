<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralZengZhang.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.IntegralZengZhang" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分增长记录</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server"> 
    <h3 class="titleBg">
        积分增长记录</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td colspan="2">
                        时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>至
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;增长类型：
                        <select id="XiaoFei" runat="server">
                            <option value="99">所有</option>
                            <option value="0">登录奖励</option>
                            <option value="1">购票积分</option> 
                            <option value="6">开户奖励</option>
                            <option value="8">拒绝兑换</option>
                        </select>
                          <span>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查 询" OnClick="btnQuery_Click" /></span> 
                          <div class='fr'><a href="IntegralXiaoFei.aspx">积分消费记录</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="CommodityShowList.aspx">积分商城</a></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table column">
        <asp:GridView ID="grv_zengzhang" runat="server" AutoGenerateColumns="false"  EnableViewState="false"  >
            <Columns>
                <asp:BoundField DataField="ExchangeTiem" HeaderText="时间" />
                <asp:BoundField DataField="Way" HeaderText="类型" />
                <asp:BoundField DataField="ConsumptionIntegral" HeaderText="增长积分数量" />
                <asp:TemplateField HeaderText="备注">
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
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>

<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>