<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreezeStatusList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.FreezeStatusList" %>

<%@ Import Namespace="ChinaPay.B3B.Service.Order.Domain.Applyform" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>冻结状态查询【运营商】</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
<body>
    <form id="query" runat="server" defaultbutton="btnQuery">
        <h3 class="titleBg">
            冻结状态查询</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w40" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">订单编号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">申请单号：</span><asp:TextBox runat="server" ID="txtApplyFormId" class="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">申请日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'#F{$dp.$D(\'txtEndDate\')}'})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">类型：</span>
                            <asp:DropDownList runat="server" ID="ddlFreezeType" AppendDataBoundItems="True">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">状态：</span>
                            <asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="True">
                                <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="1">成功</asp:ListItem>
                                <asp:ListItem Value="0">失败</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
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
        <asp:Repeater runat="server" ID="dataList" onitemcommand="dataList_ItemCommand"  OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table>
                    <thead>
                        <tr>
                            <th>订单号</th>
                            <th>申请单号</th>
                            <th>类型</th>
                            <th>账号</th>
                            <th>金额</th>
                            <th>提交时间</th>
                            <th>状态</th>
                            <th>处理时间</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
                    <td><a href="javascript:Go('OrderDetail.aspx?id=<%# Eval("OrderId") %>')" class="obvious-a"><%# Eval("OrderId")%></a></td>
                    <td><a href="javascript:Go('RefundApplyformDetail.aspx?id=<%# Eval("ApplyformId") %>')" class="obvious-a"><%# Eval("ApplyformId")%></a></td>
                    <td><%#(GetDataItem() is FreezeInfo)?"冻结":"解冻" %></td>
                    <td><%#Eval("Account")%></td>
                    <td><%#Eval("Amount")%></td>
                    <td><%#Eval("RequestTime")%></td>
                    <td><%#(GetDataItem() is FreezeInfo)?"冻结":"解冻" %><%#(bool)Eval("Success")?"成功":"失败"%></td>
                    <td><%#Eval("ProcessedTime")%></td>
                    <td>
                        <asp:LinkButton text="解冻" ID="UnFreeze" runat="server" Visible='<%#(GetDataItem() is UnfreezeInfo) && !(bool)Eval("Success") %>' CommandArgument='<%# Eval("ApplyformId") %>' CommandName="UnFreeze"/>
                    </td>
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
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">        SaveDefaultData();
    $("#txtOrderIds").OnlyNumber().LimitLength(13);
</script>