<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoyaltyFailedList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.RoyaltyFailedList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分润失败管理【运营商】</title>
 </head>
   <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .remark
        {
            text-align: left !important;
            word-break: break-all;
            word-wrap: break-word;
            width: 160px;
        }
        .break
        {
            word-break: break-all;
            word-wrap: break-word;
        }
    </style>
<body>
    <form id="query" runat="server" defaultbutton="btnQuery">
        <h3 class="titleBg">
            分润失败管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w60" />
                    <col class="w40" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">退款日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单编号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" onclick="ResetSearchOption()" class="btn class2" value="清空条件" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table>
                    <colgroup>
                    <col span="4" class="w12"/>
                    <col span="2"/>
                    </colgroup>
                    <thead>
                        <tr>
                            <th>出票时间</th>
                            <th>订单号</th>
                            <th>支付时间</th>
                            <th>支付金额</th>
                            <th>分润账号</th>
                            <th>分润失败原因</th>
                        </tr>
                    </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
                    <td><%#Eval("ETDZTime","{0:yyyy-MM-dd HH:mm}")%></td>
                    <td><a href="javascript:Go('OrderDetail.aspx?id=<%#Eval("OrderId") %>')"><%#Eval("OrderId")%></a></td>
                    <td><%#Eval("PayTime","{0:yyyy-MM-dd HH:mm}")%></td>
                    <td><%#Eval("TradeAmount","{0:0.00}")%></td>
                    <td><%#Eval("RoyaltyInfo")%></td>
                    <td class="remark"><%#Eval("FailedReason")%></td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody>
                </table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $(".remark").tipTip({ limitLength: 21 });
        SaveDefaultData();
        $("#txtOrderId").OnlyNumber().LimitLength(13);
    });
</script>