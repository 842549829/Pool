<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlatformSpreadStatisticReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.PlatformSpreadStatisticReport" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户推广明细表</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        用户推广明细表</h3>
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
                            <span class="name">日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">公司类型：</span>
                            <select runat="server" id="selCompanyType">
                                <option value="0">--所有--</option>
                                <option value="1">出票方</option>
                                <option value="2">采购商</option>
                                <option value="4">产品方</option>
                            </select>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">员工账号：</span>
                            <asp:TextBox runat="server" ID="txtEmpolyeeNo" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                        <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div runat="server" id="counts" visible="false" style="margin:10px 0;">
        采购方：<asp:Label CssClass="obvious b" runat="server" ID="lblPurchaseCount"></asp:Label>个
        （交易总额:<asp:Label CssClass="obvious b" runat="server" ID="lblPurchaseAmount"></asp:Label>）
        产品方：<asp:Label CssClass="obvious b" runat="server" ID="lblSupplyCount"></asp:Label>个
        （交易总额:<asp:Label CssClass="obvious b" runat="server" ID="lblSupplyAmount"></asp:Label>）
        出票方：<asp:Label CssClass="obvious b" runat="server" ID="lblProvideCount"></asp:Label>个
        （交易总额:<asp:Label CssClass="obvious b" runat="server" ID="lblProvideAmount"></asp:Label>）
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                员工账号
                            </th>
                            <th>
                                账号
                            </th>
                            <th>
                                简称
                            </th>
                            <th>
                                公司类型
                            </th>
                            <th>
                                注册时间
                            </th>
                            <th>
                                采购总额
                            </th>
                            <th>
                                出票总额
                            </th>
                            <th>
                                产品总额
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("EmployeeUserName")%>
                    </td>
                    <td>
                        <%#Eval("UserName")%>
                    </td>
                    <td>
                        <%#Eval("Name")%>
                    </td>
                    <td>
                        <%#Eval("CompanyType")%>
                    </td>
                    <td>
                        <%#Eval("RegisterTime")%>
                    </td>
                    <td>
                        <%#Eval("PurchaseAmount")%>
                    </td>
                    <td>
                        <%#Eval("ProvideAmount")%>
                    </td>
                    <td>
                        <%#Eval("SupplyAmount")%>
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
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Report/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/Report/EmployeeSpreadStatisticReport.js" type="text/javascript"></script>
<%--<script type="text/javascript">
    SaveDefaultData(null, '.text-s1');
</script>--%>
