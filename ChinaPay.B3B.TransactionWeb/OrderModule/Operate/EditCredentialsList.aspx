<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCredentialsList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.EditCredentialsList" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <title>修改证件号【运营方】</title>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <div class="hd">
        <h3 class="titleBg">
            修改证件号</h3>
    </div>
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
                            <span class="name">订单编号：</span>
                            <asp:TextBox ID="txtOrderformId" class="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span>
                            <asp:TextBox runat="server" ID="txtPNR" class="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="input">
                            <span class="name">创建日期：</span>
                            <asp:TextBox ID="txtDateStart" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})" />
                            <asp:TextBox ID="txtDateEnd" class="text text-s" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})" /><asp:HiddenField
                                runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button type="button" class="btn class1 submit" runat="server" ID="btnQuery"
                            Text="查&nbsp;&nbsp;&nbsp;&nbsp;询" OnClick="btnQuery_Click" OnClientClick="SaveSearchCondition('EditNo')" />
                        <input value="清空条件" type="button" class="btn class2" onclick="ResetSearchOption()"></input>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
                    <colgroup>
                        <col class="w12" />
                        <col class="w15" />
                        <col class="w10" />
                        <col class="w10" />
                        <col class="w10" />
                        <col class="w10" />
                        <col class="w10" />
                        <col class="w12" />
                        <col class="w10" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                订单号
                            </th>
                            <th>
                                PNR
                            </th>
                            <th>
                                航程
                            </th>
                            <th>
                                航班号<br />
                                舱位/折扣
                            </th>
                            <th>
                                起飞时间
                            </th>
                            <th>
                                乘机人
                            </th>
                            <th>
                                采购方
                            </th>
                            <th>
                                创建时间
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
                        <a href="javascript:Go('OrderDetail.aspx?id=<%# Eval("OrderId") %>')" class="obvious-a">
                            <%# Eval("OrderId")%></a>
                    </td>
                    <td>
                        <span class="obvious">
                            <%#Eval("PNR")%></span>
                    </td>
                    <td>
                        <%# Eval("AirportPair") %>
                    </td>
                    <td>
                        <%# Eval("FlightInfo") %>
                    </td>
                    <td>
                        <%# Eval("TakeoffTime") %>
                    </td>
                    <td>
                        <%# Eval("Passengers") %>
                    </td>
                    <td>
                        <%#Eval("PurchaserName")%>
                    </td>
                    <td>
                        <%#Eval("ProducedTime")%>
                    </td>
                    <td>
                        <a style=' display:<%#(bool)Eval("allTakeOff")?"none;":""%>' href="javascript:Update('<%# Eval("OrderId") %>')" class="obvious">修改</a><br />
                        <a href="javascript:ViewLog('<%# Eval("OrderId") %>')">日志</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    function Update(orderId) {
        location.href = '../UpdateCredentials.aspx?id=' + orderId + "&returnUrl=" + location.href;
    }
    function ViewLog(orderId) {
        location.href = '../CredentialsUpdateLog.aspx?order=' + orderId + "&returnUrl=" + location.href;
    }
    $(function () {
        SaveDefaultData();
        $("#txtOrderformId").OnlyNumber().LimitLength(13);
        pageName = 'EditNo';
    });
</script>
