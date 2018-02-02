<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TodayProvideStatisticReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.TodayProvideStatisticReport" %>

<%@ Register TagPrefix="uc1" TagName="Company_1" Src="~/UserControl/CompanyC.ascx" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<%@ Register TagPrefix="uc" TagName="City" Src="~/UserControl/Airport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>当日票量分析报表</title>
</head>
    <style type="text/css">
        .tatistics-l
        {
            border-right: 2px solid #dedede;
            margin-right: 5%;
            min-width: 140px;
            width: 20%;
        }
        .tatistics-r
        {
            width: 74%;
        }
        .tatistics-l p
        {
            height: 15px;
            padding: 0 10px 10px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="query" runat="server" autocomplete="off">
    <h3 class="titleBg">
        当日票量统计</h3>
    <div class="box-a clearfix">
        <div class="fl tatistics-l">
            <p>
                <asp:CheckBox Text="按航空公司统计" runat="server" ID="ByCarriar" />
            </p>
            <p>
                <asp:CheckBox Text="按航线统计" runat="server" ID="ByVoyage" />
            </p>
            <p>
                <asp:CheckBox Text="按航班号统计" runat="server" ID="ByFlightNo" />
            </p>
            <p>
                <asp:CheckBox Text="按舱位统计" runat="server" ID="ByBunk" />
            </p>
            <p>
                <asp:CheckBox Text="按出票方统计" runat="server" ID="ByProvider" />
            </p>
            <p>
                <asp:CheckBox Text="按销售关系统计" runat="server" ID="ByRelation" />
            </p>
        </div>
        <table class="fl tatistics-r condition">
            <tbody>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">时段：</span>
                            <asp:DropDownList runat="server" ID="TimeStart" AppendDataBoundItems="True" Width="50px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            时 00 分 -
                            <asp:DropDownList runat="server" ID="TimeEnd" AppendDataBoundItems="True" Width="50px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                            </asp:DropDownList>
                            时 59 分
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">出发城市：</span>
                            <uc:City runat="server" ID="txtDeparture"></uc:City>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">产品类型：</span>
                            <asp:DropDownList ID="ddlProductType" runat="server" Width="60px" CssClass="selectarea"
                                AppendDataBoundItems="True">
                                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlSpecialTickType" runat="server" CssClass="selectarea" AppendDataBoundItems="True"
                                Width="94px">
                                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">到达城市：</span>
                            <uc:City runat="server" ID="txtArrival"></uc:City>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">航空公司：</span>
                            <asp:DropDownList ID="ddlAirlines" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input" style="z-index:4;">
                            <span class="name">出票方：</span>
                            <uc1:Company_1 runat="server" ID="txtProviderCompany" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">销售关系：</span>
                            <asp:DropDownList ID="ddlRelation" runat="server" CssClass="selectarea" AppendDataBoundItems="True">
                                <asp:ListItem runat="server" Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input" style="z-index:2;">
                            <asp:Button class="btn class1" Text="查询" runat="server" OnClick="btnQuery_Click" />
                            <asp:Button class="btn class2" Text="下载" ID="btnDownload" runat="server" OnClick="DownData" />
                            <input type="button" class="btn class2" value="清空条件" id="btnEmpty" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div runat="server" id="counts" visible="false" style="margin:10px 0;">
        票量：<asp:Label CssClass="obvious b" runat="server" ID="lblPostponeFee"></asp:Label>
        出票方实收款：<asp:Label CssClass="obvious b" runat="server" ID="lblProviderAmount"></asp:Label>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:GridView runat="server" ID="dataList">
        </asp:GridView>
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
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script type="text/javascript">
    var specialTypeText = "特殊";
    $(function () {

        $("#ddlProductType").change(function () {
            if ($.trim($("#ddlProductType>option:checked").text()) == specialTypeText) {
                $("#ddlSpecialTickType").removeAttr("disabled", "disabled");
            } else {
                $("#ddlSpecialTickType").find("option").first().attr("selected", "selected").end().end().attr("disabled", "disabled");
            }
        }).trigger("change");


        $("#btnEmpty").click(function () {
            ResetSearchOption();
            $("#TimeStart").val("00");
            $("#TimeEnd").val("23");
        });
    });


</script>
