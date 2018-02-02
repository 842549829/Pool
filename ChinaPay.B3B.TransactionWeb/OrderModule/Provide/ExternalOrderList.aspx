<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalOrderList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ExternalOrderList" %>

<%@ Register TagName="City" Src="~/UserControl/Airport.ascx" TagPrefix="uc" %>
<%@ Register TagName="Pager" Src="~/UserControl/Pager.ascx" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        订单查询</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">创建时间：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">来源平台：</span>
                            <asp:DropDownList ID="ddlPlatfromType" runat="server">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span>
                            <asp:TextBox ID="txtPnr" CssClass="text" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">出发城市：</span>
                            <uc:City ID="txtDepartureCity" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">支付状态：</span>
                            <asp:DropDownList ID="ddlPayStatus" runat="server">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span>
                            <asp:TextBox ID="txtPassenger" CssClass="text" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">到达城市：</span>
                            <uc:City ID="txtArrivalCity" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">出票状态：</span>
                            <asp:DropDownList ID="ddlPrintStatus" runat="server">
                                <asp:ListItem Text="请选择" Value=""></asp:ListItem>
                                <asp:ListItem Text="未出票" Value="0"></asp:ListItem>
                                <asp:ListItem Text="已出票" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">外部订单号：</span>
                            <asp:TextBox ID="txtExternalOrderId" CssClass="text" runat="server"></asp:TextBox><br />
                        </div>
                        <div class="input">
                            <span class="name">内部订单号：</span>
                            <asp:TextBox ID="txtInternalOrderId" CssClass="text" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" OnClientClick="SaveSearchCondition('ExternalOrderList')" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                来源平台
                            </th>
                            <th>
                                订单号
                            </th>
                            <th>
                                编码
                            </th>
                            <th>
                                行程
                            </th>
                            <th>
                                乘机人
                            </th>
                            <th>
                                结算价/返佣
                            </th>
                            <th>
                                创建时间
                            </th>
                            <th>
                              内部状态
                            </th>
                            <th>
                                支付状态
                            </th>
                            <th>
                                出票状态
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("PlatformType")%>
                    </td>
                    <td>
                        <%# Eval("OrderId")%>
                    </td>
                    <td>
                        <%# Eval("PNR")%>
                    </td>
                    <td>
                        <%# Eval("FlightInfo")%>
                    </td>
                    <td>
                        <%# Eval("Passengers")%>
                    </td>
                    <td>
                        <%# Eval("Commission")%>
                    </td>
                    <td>
                        <%# Eval("ProducedTime")%>
                    </td>
                    <td class="obvious b">
                        <%# Eval("InternalPayStatus")%>
                    </td>
                    <td class="obvious b">
                        <%# Eval("PayStatus")%>
                    </td>
                    <td>
                        <%# Eval("Status")%>
                    </td>
                    <td>
                        <a href='ExternalOrderDetail.aspx?returnUrl=ExternalOrderList.aspx&id=<%#Eval("InternalOrderId") %>&PlatformTypeValue=<%#Eval("PlatformTypeValue") %>'>详情</a><br />
                        <a href='../OrderLog.aspx?id=<%#Eval("InternalOrderId") %>'>日志</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
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
<script src="../../Scripts/selector.js" type="text/javascript"></script>
<script src="../../Scripts/OrderModule/QueryList.js" type="text/javascript"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
     var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
        $("#txtPnr").LimitLength(6);
        $("#txtInternalOrderId").OnlyNumber().LimitLength(13);
        $("#txtExternalOrderId").LimitLength(20);
        $("#txtPassenger").LimitLength(10);
        SaveDefaultData();
        pageName = 'ExternalOrderList';
    })
</script>
