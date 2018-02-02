<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="notch_policy_info.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.notch_policy_info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>缺口政策查看</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        缺口政策查看</h3>
    <div class="form">
        <table class="box">
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        航空公司
                    </td>
                    <td>
                        <asp:Label ID="lblAirline" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        行程类型
                    </td>
                    <td>
                        <asp:Label ID="lblVoyage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        OFFICE号
                    </td>
                    <td>
                        <asp:Label ID="lblOffice" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        票证类型
                    </td>
                    <td>
                        <asp:Label ID="lblTicket" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        是否有限制
                    </td>
                    <td colspan="3">
                        <label runat="server" id="lblDeparture"></label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        航班日期
                    </td>
                    <td>
                        <asp:Label ID="lblDepartureDate" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        班期限制
                    </td>
                    <td>
                        <asp:Label ID="lblDepartureWeekFilter" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        排除日期
                    </td>
                    <td>
                        <asp:Label ID="lblExceptDay" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        自定义编号
                    </td>
                    <td>
                        <asp:Label ID="lblCutomerCode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        出票时间
                    </td>
                    <td>
                        <asp:Label ID="lblCreateTime" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        适合舱位
                    </td>
                    <td class="postion_tr">
                        <asp:Label ID="lblBunks" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <asp:Label ID="lblDepartureShowOrHide" runat="server" Text="去程"></asp:Label>航班限制
                    </td>
                    <td>
                        <asp:Label ID="lblDepartureFilght" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        排除航段
                    </td>
                    <td>
                        <asp:Label ID="lblExceptAirlines" runat="server" class="text-auto"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        是否锁定
                    </td>
                    <td>
                        <asp:Label ID="lblLock" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        下级佣金
                    </td>
                    <td>
                        <asp:Label ID="lblXiaJi" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title" id="internalTitle" runat="server">
                        内部返点
                    </td>
                    <td id="internalValue" runat="server">
                        <asp:Label ID="lblNeiBu" runat="server"></asp:Label>
                    </td>
                    <td class="title" id="proffessionTitle" runat="server">
                        同行返点
                    </td>
                    <td id="proffessionValue" runat="server">
                        <asp:Label ID="lblTongHang" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        是否需要换编码出票
                    </td>
                    <td>
                        <asp:Label ID="lblChang" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        起飞前2小时内可用B2B出票
                    </td>
                    <td>
                        <asp:Label ID="lblPrintBeforeTwoHours" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2" class="box">
                        <h2>
                            出票条件</h2>
                    </td>
                    <td colspan="2" class="box">
                        <h2>
                            政策备注</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background: #FFF;" class="box text-auto">
                        <asp:Label ID="lblDrawerCondition" runat="server"></asp:Label>
                    </td>
                    <td colspan="2" class="box text-auto">
                        <asp:Label ID="lblRemaek" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="text-align: center;">
        <asp:HiddenField ID="hidIds" runat="server" />
        <input type="button" id="lock" class="btn class1" runat="server" value="锁&nbsp;&nbsp;&nbsp;定" />
        <input type="button" id="unlock" class="btn class1" runat="server" value="解&nbsp;&nbsp;&nbsp;锁" />
        <asp:Button ID="btnLog" CssClass="btn class1" runat="server" Text="查看日志" OnClick="btnLog_Click" />
        <input type="button" id="btnBack" class="btn class2" value="返回" onclick="javascript:window.location.href='notch_policy_manage.aspx'" />
    </div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            解锁政策</h2>
        <table>
            <colgroup>
                <col class="w30" />
                <col class="w70" />
            </colgroup>
            <tr>
                <td class="title">
                    请输入解锁原因
                </td>
                <td>
                    <asp:TextBox ID="txtlockReason" TextMode="MultiLine" Height="150px" Width="300px"
                        runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtunlockReason" TextMode="MultiLine" Height="150px" Width="300px"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnlock" CssClass="btn class1" runat="server" Text="锁&nbsp;&nbsp;&nbsp;定"
                        OnClick="btnlock_Click" />
                    <asp:Button ID="btnunlock" CssClass="btn class1" runat="server" Text="解&nbsp;&nbsp;&nbsp;锁"
                        OnClick="btnunlock_Click" />
                    <label class="close btn class2" title="关闭">
                        关闭</label>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policylock.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#lblDepartureFilght,#lblRetnrnFilght").tipTip({ maxWidth: "400px", limitLength: 60 });
            $("#lblDeparture").tipTip({ maxWidth: "400px", limitLength: 100 });
        });
    </script>
</body>
</html>
