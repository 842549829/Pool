<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.OEM.OrderDetail" %>

<%@ Reference Control="~/OrderModule/UserControls/PNRInfo.ascx" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagName="Bill" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server" >
    <%--订单头部信息--%>
    <h3 class="titleBg">订单详情</h3>
    <div runat="server" id="divHeader" class="column form">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">订单编号</td>
                <td><asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></td>
                <td class="title">订单状态</td>
                <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
                <td class="title">订单金额</td>
                <td><asp:Label runat="server" ID="lblAmount" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">产品类型</td>
                <td><asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label></td>
                <td class="title">客票类型</td>
                <td><asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label></td>
                <td class="title">原订单号</td>
                <td><asp:Label runat="server" ID="lblOriginalOrderId" CssClass="obvious-a"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">创建时间</td>
                <td><asp:Label runat="server" ID="lblProducedTime" CssClass="obvious"></asp:Label></td>
                <td class="title">支付时间</td>
                <td><asp:Label runat="server" ID="lblPayTime" CssClass="obvious"></asp:Label></td>
                <td class="title">出票时间</td>
                <td><asp:Label runat="server" ID="lblETDZTime" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">预订人</td>
                <td colspan="5"><asp:Label runat="server" ID="lblProducter" CssClass="obvious"></asp:Label></td>
            </tr>
        </table>
       <div runat="server" id="divFailed" visible="false">拒绝出票：<asp:Label runat="server" ID="lblFailedReason"></asp:Label></div>
    </div>
    <%--编码组信息--%>
    <div runat="server" id="divPNRGroups"></div>
    <%--账单信息--%>
    <uc:Bill runat="server" ID="bill"/>
    <%--联系人信息--%>
    <div runat="server" id="divContact" class="column">
        <div class="tips-info">
            <div class="hd"><h2>联系人信息</h2></div>
            <div class="con tips">
                <div class="col-3">
                    <div class="col">联系人：<asp:Label runat="server" ID="lblContact"></asp:Label></div>
                    <div class="col">手机号码：<asp:Label runat="server" ID="lblContactMobile"></asp:Label></div>
                    <div class="col">电子邮箱：<asp:Label runat="server" ID="lblContactEmail"></asp:Label></div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" id="hidRenderPrice" Value="1" />
    </form>
    <%--底部操作按钮--%>
    <div class="btns">
        <%--<button class="btn class1" runat="server" visible="false" id="btnOrderHistory">订单历史记录</button>
        <button class="btn class1" runat="server" visible="false" id="btnProcessingApplyforms">进行中的申请</button>
        <button class="btn class1" runat="server" visible="false"  id="btnApply">申请退改签</button>
        <button class="btn class1" runat="server" visible="false" id="btnPay">支&nbsp;&nbsp;&nbsp;付</button>--%>
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
     <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none; width:800px; height:400px;">
    </div>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        var url = $("#btnApply").attr("onclick");
        $("#btnApply").attr("onclick", "").click(function () {
            sendPostRequest("../../OrganizationHandlers/AddInfo.ashx/CheckInfo", "", function (result) {
                if (!result) {
                    $("#divOpcial").click();
                    $("#mask").css("display", "block");
                    $.ajaxSetup({
                        cache: false //关闭AJAX相应的缓存
                    });
                    $("#divPolicy").load("../../OrganizationModule/CommonContent/AddInfo.aspx", "#form1");
                } else {
                    eval(url);
                }
            }, function () { alert("系统异常"); });
        });

        if ($("#hidRenderPrice").val() != "1") {
            $(".ticketPrice").text("出票后可见");
        }
    });
    $(document).ready(function () {
        var parameter = getRequest();
        $("#divPNRGroups #passengers").each(function () {
            var self = $(this);
            self.find("table th :checkbox").bind("click", function () {
                var chkSelf = $(this);
                if (chkSelf.is(":checked")) {
                    chkSelf.parents("table").find("td :checkbox").attr("checked", "checked");
                } else {
                    chkSelf.parents("table").find("td :checkbox").removeAttr("checked");
                }
            });
            self.find("table td :checkbox").bind("click", function () {
                if (self.find("table td :checkbox:not(:checked)").size() == 0) {
                    self.find("table th :checkbox").attr("checked", "checked");
                } else {
                    self.find("table th :checkbox").removeAttr("checked");
                }
            });
            self.find("div :button").bind("click", function () {
                var btnSelf = $(this);
                var name = new Array();
                btnSelf.parent().prev().find("tr").each(function () {
                    if ($(this).find("td :checkbox").is(":checked")) {
                        name.push($(this).find("td:eq(0)").text());
                    }
                });

                if (name.length <= 0) {
                    alert("请选择乘机人");
                    return false;
                }
                if (parameter != "undefined" && parameter.id != "") {

                    window.location.href = "./Itinerary.html?OrderId=" + encodeURI(parameter.id) + "&Passenger=" + encodeURI(name.join(","));
                } else {
                    alert("订单号有误");
                    return false;
                }
            });
        });
    });
</script>