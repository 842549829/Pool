<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalInterfaceSetting.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.ExternalInterfaceSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据接口设置</title>
    <link href="/Styles/public.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        数据接口设置</h3>
    <div class="form">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr>
                <td class="title">
                    数据开关:
                </td>
                <td colspan="3">
                    <asp:RadioButton ID="rbnOpen" runat="server" GroupName="externalInterface" Text="开启数据推送" />
                    <asp:RadioButton ID="rbnClose" runat="server" GroupName="externalInterface" Checked="true"
                        Text="关闭数据推送" />
                </td>
            </tr>
            <tbody id="externalInterface">
                <tr>
                    <td class="title">
                        安全码:
                    </td>
                    <td>
                        <asp:Label ID="lblSecurityCode" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        绑定IP:
                    </td>
                    <td>
                        <asp:TextBox ID="txtIP" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        可调用方法：
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkPNRImport" runat="server" Text="政策导入" />
                        <asp:CheckBox ID="chkPNRImportWithoutPat" runat="server" Text="导入内容不需要pat信息" />
                        <asp:CheckBox ID="chkProduceOrder" runat="server" Text="生成订单" />
                        <asp:CheckBox ID="chkProduceOrder2" runat="server" Text="生成订单（预定）" />
                        <asp:CheckBox ID="chkApplyRefund" runat="server" Text="申请退废票" />
                        <asp:CheckBox ID="chkApplyPostpone" runat="server" Text="申请改期" />
                        <asp:CheckBox ID="chkAutoPay" runat="server" Text="代扣支付" />
                        <asp:CheckBox ID="chkOrderPay" runat="server" Text="订单支付" />
                        <asp:CheckBox ID="chkManualPay" runat="server" Text="手动支付" />
                        <asp:CheckBox ID="chkPayApplyform" runat="server" Text="改期支付" />
                        <asp:CheckBox ID="chkPayOrderByPayType" runat="server" Text="订单在线支付" />
                        <asp:CheckBox ID="chkPayApplyformByPayType" runat="server" Text="改期在线支付" />
                        <asp:CheckBox ID="chkQueryOrder" runat="server" Text="查询订单详情" />
                        <asp:CheckBox ID="chkQueryApplyform" runat="server" Text="申请单查询" />
                        <asp:CheckBox ID="chkQueryFlights" runat="server" Text="查询航班列表" />
                        <asp:CheckBox ID="chkQueryFlight" runat="server" Text="查询航班" />
                        <asp:CheckBox ID="chkQueryFlightStop" runat="server" Text="查询经停" />
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        可使用的政策类型：
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkNormal" runat="server" Text="普通政策" />
                        <asp:CheckBox ID="chkBargain" runat="server" Text="特价政策" />
                        <asp:CheckBox ID="chkTeam" runat="server" Text="团队政策" />
                        <asp:CheckBox ID="chkSpecial" runat="server" Text="特殊政策" />
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        确认资源成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmSuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        退废票退款成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefundSuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        确认资源失败通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmFailAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        退废票处理成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtReturnTicketSuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        订单支付成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPaySuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        改期成功通知地址:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtReschedulingAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        出票成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrawSuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        拒绝退废票通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefuseTicketAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        取消出票通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefuseAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        同意改期通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAgreedAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        订单退款成功通知地址：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCanceldulingAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        改期支付成功通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtReschPaymentAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        拒绝改期通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefuseChangeAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                    <td class="title">
                        拒绝改期退款通知地址:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefundApplySuccessAddress" runat="server" CssClass="text selectarea"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button CssClass="btn class1" Text="保存" runat="server" ID="btnSave" OnClick="btnSave_Click" />
                    <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='ExternalInterfaceList.aspx?Search=Back'" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="../../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        showOrHide();
        $("input[type=radio]").click(function () {
            showOrHide();
        });
        $("#btnSave").click(function () {
            if ($.trim($("#txtConfirmSuccessAddress").val()).length > 150) {
                $("#txtConfirmSuccessAddress").select();
                alert("确认成功通知地址长度不能超过150！");
                return false;
            }
            if ($.trim($("#txtConfirmFailAddress").val()).length > 150) {
                $("#txtConfirmFailAddress").select();
                alert("确认失败通知地址长度不能超过150！");
                return false;
            }
            if ($.trim($("#txtPaySuccessAddress").val()).length > 150) {
                $("#txtPaySuccessAddress").select();
                alert("支付成功通知地址长度不能超过150！");
                return false;
            }
            if ($.trim($("#txtDrawSuccessAddress").val()).length > 150) {
                $("#txtDrawSuccessAddress").select();
                alert("出票成功通知地址长度不能超过150！");
                return false;
            }
            if ($.trim($("#txtRefuseAddress").val()).length > 150) {
                $("#txtRefuseAddress").select();
                alert("拒绝出票通知地址长度不能超过150！");
                return false;
            }
            if ($.trim($("#txtCanceldulingAddress").val()).length > 150) {
                $("#txtCanceldulingAddress").select();
                alert("取消出票退款成功通知地址长度不能超过150！");
                return false;
            }
        });
    })
    function showOrHide() {
        if ($("#rbnOpen").attr("checked") == "checked") {
            $("#externalInterface").show();
        }
        if ($("#rbnClose").attr("checked") == "checked") {
            $("#externalInterface").hide();
        }
    }
</script>
