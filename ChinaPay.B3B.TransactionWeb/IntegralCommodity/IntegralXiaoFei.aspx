<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralXiaoFei.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.IntegralXiaoFei" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分消费记录</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        积分消费记录</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td>
                        时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>至
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;消费类型：
                        <select id="XiaoFei" runat="server">
                            <option value="99">所有</option>
                            <option value="2">未登录减少</option>
                            <option value="3">兑换商品</option>
                            <option value="4">退票减少</option>
                            <option value="7">兑换短信数量</option>
                        </select>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查 询" OnClick="btnQuery_Click" />  </span> 
                          <div class='fr'><a href="IntegralZengZhang.aspx">积分增长记录</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="CommodityShowList.aspx">积分商城</a></div>
                    </td> 
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table column">
        <asp:GridView ID="grv_xiaofei" runat="server" AutoGenerateColumns="false"  EnableViewState="false"  >
            <Columns>
                <asp:BoundField DataField="ExchangeTiem" HeaderText="时间" />
                <asp:BoundField DataField="Way" HeaderText="类型" />
                <asp:BoundField DataField="ConsumptionIntegral" HeaderText="消费积分数量" />
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
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divCommodity'}" ></a>
    <div id="divCommodity"  class="form layer" style="display: none; background-color:White"> 
        <h3>兑换详细</h3>
        <table>
            <colgroup>
                <col class="w20"/>
                <col class="w30"/>
                <col class="w20"/>
                <col class="w30"/>
            </colgroup>
            <tr>
                <td  class="title">商品名称</td>
                <td id="td_commodityname"></td>
                <td class="title">兑换数量</td>
                <td id="td_commoditynum"></td>
            </tr>
            <tr>
                <td  class="title">消费积分</td>
                <td id="td_commodityintegral"></td>
                <td class="title"><label class="showCommodity">收货地址</label></td>
                <td><label id="td_address" class="showCommodity"></label></td>
            </tr>
            <tr class="showCommodity">
                <td  class="title">快递公司</td>
                <td id="td_company"></td>
                <td class="title">快递单号</td>
                <td id="td_no"></td>
            </tr>
            <tr class="showChuli"> 
                <td colspan="4">兑换商品正在处理，我们将尽快与你联系。谢谢</td>
            </tr>
            <tr> 
                <td colspan="4" align="center"><input type="button" class="btn class2 close" value="关闭" /></td>
            </tr>
        </table> 
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {

    });
    function show(CommodityName, CommodityCount, ConsumptionIntegral, DeliveryAddress, ExpressCompany, ExpressDelivery, Exchange) { 
        if (Exchange == "Processing") {
            $(".showCommodity").css("display", "none");
            $(".showChuli").css("display", "");
        } else {
            $(".showChuli").css("display", "none");
            $(".showCommodity").css("display", "");
        }
        $("#td_commodityname").html(CommodityName);
        $("#td_commoditynum").html(CommodityCount);
        $("#td_commodityintegral").html(ConsumptionIntegral);
        $("#td_address").html(DeliveryAddress);
        $("#td_company").html(ExpressCompany);
        $("#td_no").html(ExpressDelivery); 
        $("#divOpcial").click();
    }
</script>