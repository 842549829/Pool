<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="policy_coordination_addModify.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.policy_coordination_addModify"  %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/MultipleCity.ascx" TagName="Cities" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>政策协调的添加/修改</title>
</head>
    <link rel="stylesheet" href="../Styles/page.css?20121118" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/airport.js" type="text/javascript"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/MultipleCity.js" type="text/javascript"></script>
    <style type="text/css">
        .text
        {
            width: 300px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        政策协调的添加/修改</h3>
    <div class="form">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w40" />
                <col class="w40" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        航空公司
                    </td>
                    <td>
                        <input id="radchoiceall" type="radio" name="choice" /><label for="radchoiceall">全选</label><input
                            id="radchoiceNotall" type="radio" name="choice" /><label for="radchoiceNotall">反选</label>
                        <asp:CheckBoxList ID="chkAirlineList" RepeatColumns="7" RepeatDirection="Horizontal"
                            runat="server" ondatabound="chkAirlineList_DataBound">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        始发地
                    </td>
                    <td>
                        <uc:City ID="txtDepartureAirports" runat="server"  />
                    </td>
                    <td class="obvious">
                        温馨提示： 支持多选，可用Shift连选，也支持Ctrl间隔选择。 可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。 输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        目的地
                    </td>
                    <td>
                        <uc:City ID="txtArrivalAirports" runat="server" />
                    </td>
                    <td class="obvious">
                        温馨提示： 支持多选，可用Shift连选，也支持Ctrl间隔选择。 可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。 输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        政策类型
                    </td>
                    <td class="policytype">
                        <asp:CheckBox ID="chkPolicyNormal" runat="server" Text="基础政策" />
                        <asp:CheckBox ID="chkPolicyBargin" runat="server" Text="特价政策" />
                    </td>
                    <td>
                    </td>
                </tr>
         <%--       <tr style="height:330px;">
                    <td class="title">
                        受限城市
                    </td>
                    <td>
                        <uc:Cities ID="txtShouXianAirportCode" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>--%>
<%--                <tr>
                    <td class="title">
                        账号类型
                    </td>
                    <td>
                        <asp:RadioButton ID="radVip" GroupName="vip" runat="server" Checked="true" Text="VIP账号" />
                        <asp:RadioButton ID="radNotVip" GroupName="vip" runat="server" Text="非VIP账号" />
                    </td>
                    <td>
                    </td>
                </tr>--%>
                <tr>
                    <td class="title">
                        返佣类型
                    </td>
                    <td>
                        <asp:RadioButton ID="radSubCommission" GroupName="commission" runat="server" Checked="true" Text="下级返佣" />
                        <asp:RadioButton ID="radIntCommission" GroupName="commission" runat="server" Text="同行返佣" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        航班日期
                    </td>
                    <td>
                        <asp:TextBox ID="txtTimeStart" runat="server" CssClass="datepicker datefrom btn class3"></asp:TextBox>
                        <span class="fl-l">至</span>
                        <asp:TextBox ID="txtTimeEnd" runat="server" CssClass="datepicker datefrom btn class3"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        政策协调值
                    </td>
                    <td>
                        <asp:TextBox ID="txtXieTiao" runat="server" CssClass="digit text"></asp:TextBox>
                        %
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        备注
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server" Height="150px" Width="350px" TextMode="MultiLine"
                            CssClass="text"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保 存" 
                            onclick="btnSave_Click" />
                        <asp:Button ID="btnReturn" CssClass="btn class2" runat="server" Text="返 回" 
                            onclick="btnReturn_Click"  />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField runat="server" ID="hidTime" />
    </div>
    </form>
    <script type="text/javascript">
        $(function () { 
            $("#radchoiceall").click(function () {
                $("#chkAirlineList input[type='checkbox']").attr("checked", "checked");
            });
            $("#radchoiceNotall").click(function () {
                for (var i = 0; i < $("#chkAirlineList input[type='checkbox']").length; i++) {
                    if ($("#chkAirlineList input[type='checkbox']").eq(i).is(":checked")) {
                        $("#chkAirlineList input[type='checkbox']").eq(i).removeAttr("checked");
                    } else {
                        $("#chkAirlineList input[type='checkbox']").eq(i).attr("checked", "checked");
                    }
                }
            });
            $("#txtTimeStart").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", maxDate: '#F{$dp.$D(\'txtTimeEnd\')||\'2020-10-01\'}' });
            });
            $("#txtTimeEnd").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", minDate: '#F{$dp.$D(\'txtTimeStart\')}', maxDate: '2020-10-01' });
            });
            $("#btnSave").click(function () {
                var reg = /^[0-9]{1,10}(\.[1-9])?$/;
                if ($("#chkAirlineList input[type='checkbox']:checked").length == 0) {
                    alert("请至少选择一个航空公司!");
                    return false;
                }
                if ($("#txtDepartureAirports_txtAirports").val() == "") {
                    alert("请至少选择一个城市作为始发地!");
                    return false;
                }
                if ($("#txtArrivalAirports_txtAirports").val() == "") {
                    alert("请至少选择一个城市作为目的地!");
                    return false;
                }
                if ($(".policytype input[type='checkbox']:checked").length == 0) {
                    alert("请至少选择一种政策类型!");
                    return false;
                }
                if ($("#txtShouXianCity_txtCitys").val() == "")
                {
                    alert("请至少选择一个限制城市!");
                    return false;
                }
                if ($("#txtXieTiao").val() == "") {
                    alert("协调值不能为空!");
                    return false;
                }
                if ($("#txtXieTiao").val() != "" && !reg.test($("#txtXieTiao").val())) {
                    alert("协调值只能为1到10位的整数或一位小数，请重新输入!");
                    $("#txtXieTiao").val("");
                    return false;
                }
                return true;
            });
        });
    </script>
</body>
</html>
