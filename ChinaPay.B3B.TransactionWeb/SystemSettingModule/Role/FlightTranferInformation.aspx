<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightTranferInformation.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.FlightTranferInformation" %>

<%@ Register TagName="pager" TagPrefix="uc" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 </head>
   <style type="text/css">
        .text.fl
        {
            float: none;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        航班变更通知</h3>
    <div class="clearQ-box table" id="flightChangeWithoutSelf">
        <p class="importantBox broaden">
            上次清Q执行时间：<asp:Label ID="Label4" runat="server" CssClass="pad-r"></asp:Label>
            <span class="ok p_title">恭喜，暂时没有对您造成影响的航班变动&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;若您发现清Q结果错误，请点击<a
                href="javascript:void(0);" class="obvious-a" id="sendMistake">报告错误</a></span>
        </p>
        <div class="orderBox">
            <div class="orderCondition clearfix">
                <span>航空公司：<asp:DropDownList ID="ddlAirline" runat="server">
                </asp:DropDownList>
                </span><span>航班号：<asp:TextBox ID="txtFlightNo" CssClass="text" runat="server" Width="80px"></asp:TextBox></span>
                <span>变更类型：<asp:DropDownList ID="ddlTranferType" runat="server">
                </asp:DropDownList>
                </span><span>原航班日期：
                    <asp:TextBox ID="txtTakeOffLowerTime" runat="server" CssClass="width-90 text" onfocus="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtTakeOffUpperTime\')||\'%y-%M-%d\'}'})"></asp:TextBox>至
                    <asp:TextBox ID="txtTakeOffUpperTime" runat="server" CssClass="width-90 text" onfocus="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtTakeOffLowerTime\')}', maxDate:'%y-%M-%d'})"></asp:TextBox>
                </span>
                <asp:Button ID="btnQuery" runat="server" CssClass="btn class1 fl" Text="查询" OnClick="btnQuery_Click" />
            </div>
        </div>
        <div class="table" id="dataList" runat="server">
            <asp:Repeater ID="rptInformation" runat="server">
                <HeaderTemplate>
                    <table class="mar-t">
                        <thead>
                            <tr>
                                <th class="w8">
                                    原航空公司
                                </th>
                                <th class="w8">
                                    新航空公司
                                </th>
                                <th class="w8">
                                    变更类型
                                </th>
                                <th class="w8">
                                    原航班号
                                </th>
                                <th class="w8">
                                    新航班号
                                </th>
                                <th class="w10">
                                    原起飞时间
                                </th>
                                <th class="w10">
                                    新起飞时间
                                </th>
                                <th class="w10">
                                    原到达时间
                                </th>
                                <th class="w10">
                                    新到达时间
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("OriginalCarrierName")%>
                        </td>
                        <td>
                            <%#Eval("CarrierName")%>
                        </td>
                        <td>
                            <%#Eval("TransferType")%>
                        </td>
                        <td>
                            <%#Eval("OriginalFlightNo")%>
                        </td>
                        <td>
                            <%#Eval("FlightNo")%>
                        </td>
                        <td>
                            <%#Eval("OriginalTakeoffTime", "{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                        <td>
                            <%#Eval("TakeoffTime", "{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                        <td>
                            <%#Eval("OriginalArrivalTime","{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                        <td>
                            <%#Eval("ArrivalTime", "{0:yyyy-MM-dd HH:mm}")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody></table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="box" runat="server" visible="false" id="emptyDataInfo">
                <asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>建议您经常关注本页面以确保您能第一时间获取航班变动结果<br />
                感谢您对<asp:Literal runat="server" ID="lblPlatformName1"></asp:Literal>的支持，希望您能够将<asp:Literal
                    runat="server" ID="lblPlatformName2"></asp:Literal>平台推荐给您的朋友
            </div>
        </div>
        <div class="btns">
            <uc:pager ID="Pager" runat="server" Visible="false" />
            <br />
            <a href="FlightChangeNotice.aspx" class="btn class1">返回</a>
        </div>
    </div>
    </form>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#txtFlightNo").LimitLength(6);
        $("#sendMistake").click(function () {
            if ($("#help_trigger_box", parent.document).size() > 0) {
                // $("a[class='selected']", parent.document).parent().parent().children().eq(5).find("a").click();
                $("#send_msg", $("#help_trigger_box", parent.document)).click();
            }
        });
    })
</script>
