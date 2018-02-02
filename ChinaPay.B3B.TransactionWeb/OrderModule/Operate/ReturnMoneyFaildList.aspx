<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnMoneyFaildList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ReturnMoneyFaildList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>退款失败管理【运营商】</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        .remark {
            text-align: left !important;
            word-break: break-all;
            word-wrap: break-word;
        }
        .break {
           word-break: break-all;
           word-wrap: break-word;   
        }
    </style>
<body>
    <form id="query" runat="server" DefaultButton="btnQuery">
        <h3 class="titleBg">
            退款失败管理</h3>
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
                            <span class="name">申请单号：</span><asp:TextBox runat="server" class="text" ID="txtApplyFormId"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">退款日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">业务类型：</span>
                            <asp:DropDownList runat="server" ID="ddlBusinessType" AppendDataBoundItems="true">
                                <asp:ListItem runat="server" Value="" Text="全部"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
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
                        <col span="2" class="w15"/>
                        <col class="w10"/>
                        <col class="w15"/>
                        <col />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>订单号</th>
                            <th>申请单号</th>
                            <th>业务类型</th>
                            <th>退款时间</th>
                            <th>退款失败信息</th>
<%--                            <th>退款状态</th>
                            <th>操作</th>
--%>                        </tr>
                    </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
                            <td>
                                <a href="javascript:Go('OrderDetail.aspx?id=<%#Eval("OrderId") %>')">
                                    <%#Eval("OrderId")%>
                                </a>
                            </td>
                            <td>
                                <a href="javascript:Go('RefundApplyformDetail.aspx?id=<%#Eval("ApplyformId")%>')">
                                    <%#Eval("ApplyformId")%>
                                </a>
                            </td>
                            <td><%#Eval("BusinessType")%></td>
                            <td><%#Eval("RefundTime","{0:yyyy-MM-dd HH:mm}")%></td>
                            <td class="remark"><%#Eval("RefundFailedInfo")%></td>
<%--                            <td>退款状态</td>
                            <td>操作</td>
--%>
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
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".remark").tipTip({limitLength:48});
        SaveDefaultData();
        $("#txtOrderId").OnlyNumber().LimitLength(13);
    });
</script>
