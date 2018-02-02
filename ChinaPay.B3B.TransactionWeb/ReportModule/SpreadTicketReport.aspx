<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpreadTicketReport.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.SpreadTicketReport" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_1" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <h3 class="titleBg">
            经纪人报表（温馨提示：当日报表数据可于下午18：00后（或20：00、22：00）进行下载，当日全部数据需在次日进行下载。）</h3>

    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col  class="w35"/>
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">时间：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField ID="hdDefaultDate" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">交易角色：</span>
                            <asp:DropDownList ID="ddlCompanyType" runat="server"  CssClass="selectarea1"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                     <div class="input">
                      <span class="name">机票状态：</span>
                      <asp:DropDownList ID="ddlTicketState" runat="server"  CssClass="selectarea"></asp:DropDownList>
                     </div>
                   </td>
                </tr>
                <tr>
                    <td>
                        <div class="input" style="z-index:6;">
                            <span class="name">交易方：</span>
                           <uc1:Company_1 ID="BargainCompany" runat="server" />
                        </div>
                    </td>
                    <td  id="spreadInfo" runat="server" visible="false">
                      <div class="input" style="z-index:5;">
                        <span class="name">推广方：</span>
                        <uc1:Company_1 ID="SpreadCompany" runat="server" />
                      </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                    <div style="z-index:2;">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" 
                            Text="查&nbsp;&nbsp;&nbsp;询" onclick="btnQuery_Click" />
                        <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                        <input type="button" class="btn class2" onclick="ResetSearchOption();" value="清空条件" /></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div runat="server" id="counts" visible="false" style="margin:10px 0;">
        满足条件的查询结果统计：
        交易金额：<asp:Label CssClass="obvious b" runat="server" ID="lblTradeAmount"></asp:Label>
        后返金额：<asp:Label CssClass="obvious b" runat="server" ID="lblAmount"></asp:Label>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                           <th class="spreaderName">
                            推广方
                           </th>
                            <th class="spreaderName">
                                推广方用户名
                            </th>
                            <th>
                                交易角色
                            </th>
                            <th>
                                交易方
                            </th>
                            <th>
                                订单号
                            </th>
                            <th>
                                支付方式
                            </th>
                            <th>
                                航班号
                            </th>
                            <th>
                              航程(中文)
                            </th>
                            <th>
                                舱位
                            </th>
                            <th>
                                人数
                            </th>
                            <th>
                            机票状态
                            </th>
                            <th>
                                时间
                            </th>
                            <th>
                                交易金额
                            </th>
                            <th>
                              后返金额
                            </th> 
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="spreaderName">
                        <%# Eval("SpreaderName")%>
                    </td>
                    <td class="spreaderName">
                        <%# Eval("SpreaderUserName")%>
                    </td>
                    <td>
                        <%# Eval("BargainerType")%>
                    </td>
                    <td>
                        <%# Eval("BargainerName")%>
                    </td>
                    <td>
                        <%# Eval("Id")%>
                    </td>
                    <td>
                        <%# Eval("PayType")%>
                    </td>
                    <td>
                        <%# Eval("FlightNo")%>
                    </td>
                    <td class="obvious b">
                        <%# Eval("Voyage")%>
                    </td>
                    <td>
                        <%# Eval("Bunk")%>
                    </td>
                    <td>
                        <%#Eval("PassengerCount")%>
                    </td>
                    <td>
                        <%#Eval("Type")%>
                    </td>
                    <td>
                     <%#Eval("FinishTime")%>
                    </td>
                    <td>
                      <%#Eval("TradeAmount")%>
                    </td>
                    <td><%#Eval("Amount")%></td> 
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td class="spreaderName">
                        &nbsp;
                    </td>
                    <td class="spreaderName">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                      &nbsp;
                    </td>
                    <td>
                      &nbsp;
                    </td>
                    <td>
                      <%=totalTradeAmount%>
                    </td>
                    <td>
                     <%=totalAmount%>
                    </td>
                </tr>
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
    <div style="border-top:1px dashed #999; padding-top:10px;">
      非常感谢您支持我们平台，后返就是在交易发生日的次月将后返金额通过转账的方式发放到您国付通账户中，您可以开通金额变动短信通知功能便于提醒，平台后返转账后也会通过电话等方式通知到您。感谢您继续支持和关注我们平台，希望您能将平台推广给您的朋友，谢谢！
    </div>
    <asp:HiddenField ID="hfdSpreadCompanyId" runat="server" />
    <asp:HiddenField ID="hfdCompanyType" runat="server" />
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#hfdCompanyType").val() == "roles") {
            $(".spreaderName").hide();
        }
        $("#btnDownload").click(function () {
            var startDate = $("#txtStartDate").val();
            var endDate = $("#txtEndDate").val();
            var bargainCompanyType = $("#ddlCompanyType").val();
            var bargain = $("#BargainCompany_hidCompanyId").val();
            var ticketState = $("#ddlTicketState").val();
            var spreader = $("#SpreadCompany_hidCompanyId").val();
            var hfdSpreader = $("#hfdSpreadCompanyId").val();
            var hfdCompanyType = $("#hfdCompanyType").val();
            var spreaderCondition = startDate + ',' + endDate + ',' + bargainCompanyType + ',' + bargain + ',' + ticketState + ',' + spreader + ',' + hfdSpreader + ',' + hfdCompanyType;
            window.open('ReportDownload.aspx?spreaderCondition=' + spreaderCondition, 'newwindow');
        });
        SaveDefaultData(null, '.text-s1');
    })
</script>