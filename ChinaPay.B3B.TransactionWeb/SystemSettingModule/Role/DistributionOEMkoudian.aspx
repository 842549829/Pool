<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMkoudian.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.DistributionOEMkoudian" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收益组扣点设置</title>
</head>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <style type="text/css">
        #txtDepartureAirports_txtAirports
        {
            width: 300px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        收益组扣点设置</h3>
    <div class="O_formBox">
        <span>航空公司：</span><br />
        <asp:CheckBoxList runat="server" ID="chkAirlist" RepeatColumns="15" />
        <span class="muted">
            <input type="radio" name="radAir" id="radAll" value="0" /><label for="radAll">全选</label>
            <input type="radio" name="radAir" id="radNot" value="1" /><label for="radNot">反选</label></span>
    </div>
    <div class="O_formBox">
        <span>出港城市：</span><br />
        <uc:City ID="txtDepartureAirports" runat="server" />
        <br />
        <br />
        <strong>温馨提示：</strong> <span>支持多选，可用Shift连选，也支持Ctrl间隔选择。可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK</span>
    </div>
    <div class="O_formBox">
        <span>扣点设置：</span><br />
        <asp:RadioButton runat="server" ID="radQujian" Checked="true" GroupName="radKou"
            Text="区间扣点" />
        <asp:RadioButton runat="server" ID="radTongyi" GroupName="radKou" Text="统一扣点" />
        <%--<input type="radio" name="radKou" id="radQujian" runat="server" value="0" checked="true" /><label for="radQujian">区间扣点</label>
        <input type="radio" name="radKou" id="radTongyi" runat="server" value="1" /><label for="radTongyi">统一扣点</label>--%>
        <div class="parent_div" id="qujian" runat="server" style="width: 660px">
            <div class="condition">
                <table>
                    <tbody class="quyu_table" id="rangeItems">
                        <tr>
                            <td class="title rangeSerial">
                                第1组区域：
                            </td>
                            <td>
                                <input style="width: 30px" class="text rangeStart" disabled="disabled" value="0" />%(<span>含</span>)
                            </td>
                            <td>
                                至
                            </td>
                            <td>
                                <input style="width: 30px" class="text rangeEnd" value="100" />%(含)
                            </td>
                            <td class="title">
                                设置值：
                            </td>
                            <td>
                                <input style="width: 30px" class="text rangeValue" value="0" />%
                            </td>
                            <td>
                                <input type="button" class="btn class1 addRange" value="添加区域" />
                            </td>
                            <td style="width: 50px">
                                <input type="button" class='btn class2 delRange' style='color: White; display: none;'
                                    value="删除&nbsp;&nbsp;X" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="tongyi" runat="server" style="display: none;" class="condition">
            统一扣点值<asp:TextBox runat="server" CssClass="text text-s" ID="txtTongyi" /></div>
    </div>
    <div class="O_formBox">
        <span>加价设置：</span><br />
        每张票加价<asp:TextBox runat="server" CssClass="text text-s" ID="txtPrice" />元进行出售
    </div>
    <div class="O_formBox">
        <span>备注：</span><br />
        <asp:TextBox runat="server" CssClass="text" Rows="3" Columns="60" TextMode="MultiLine"
            ID="txtRemark" />
    </div>
    <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
    <input type="button" id="btnCancel" value="取消" class="btn class2" />
    <asp:HiddenField runat="server" ID="hidRanges" />
    <asp:HiddenField runat="server" ID="hidSettingId" />
    </form>
</body>
</html>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("input[type='radio'][name='radKou']").click(function () {
            if ($(this).attr("Id") == "radQujian") {
                $("#qujian").show();
                $("#tongyi").hide();
            } else {
                $("#qujian").hide();
                $("#tongyi").show();
            }
        });
        $("#btnSave").click(function () {
            return vail();
        });
        $("#btnCancel").click(function () {
            window.location.href = 'IncomeGroupList.aspx?Search=Back';
        });
        $("input[type='radio'][name='radAir']").click(function () {
            if ($(this).val() == "0") {
                $("#chkAirlist input[type='checkbox']").attr("checked", "checked");
            } else {
                for (var i = 0; i < $("#chkAirlist input[type='checkbox']").length; i++) {
                    if ($("#chkAirlist input[type='checkbox']").eq(i).attr("checked")) {
                        $("#chkAirlist input[type='checkbox']").eq(i).removeAttr("checked");
                    } else {
                        $("#chkAirlist input[type='checkbox']").eq(i).attr("checked", "checked");
                    }
                }
            }
        });
    });
    function vail() {
        if ($("#chkAirlist [type='checkbox']:checked").length == 0) {
            alert("必须选择一个航空公司！");
            return false;
        } if ($("#txtDepartureAirports_txtAirports").val() == "") {
            alert("必须选择一个出港城市！");
            return false;
        }
        var reg = /^[0-9]{1,10}(\.[0-9])?$/;
        if ($("#radTongyi").is(":checked") && $("#txtTongyi").val() == "") {
            alert("统一扣点不能为空！");
            return false;
        }
        if ($("#radTongyi").is(":checked") && !reg.test($("#txtTongyi").val())) {
            alert("统一扣点只能为整数或一位小数！");
            return false;
        }
        if ($("#txtPrice").val() == "") {
            alert("每张票加价不能为空！");
            return false;
        }
        reg = /^[0-9]{1,10}?$/;
        if (!reg.test($("#txtPrice").val())) {
            alert("每张票加价只能为整数！");
            return false;
        }
        return true;
    }
</script>
