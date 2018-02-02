<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeGroupRestrictionSettingGlobal.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.IncomeGroupRestrictionSettingGlobal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        收益设置</h3>
    <div class="O_formBox">
        <div class="importantBox broaden">
            <span class="obvious">提示：本页所设置的规则将对您的所有用户生效，请认真核对后再提交！</span><br />
            全局收益设置就是对您所有的下级用户进行普通票返点扣除或特殊票加价销售的操作设置后,下级用户在平台购票时您将得到您所设置的交易利润
        </div>
        <div>
            <asp:RadioButton ID="rbnPurchaseEach" runat="server" Text="允许各用户组分别设置收益规则" GroupName="purchase" /><br />
            <asp:RadioButton ID="rbnPurchaseGlobal" runat="server" Text="使用全局的统一收益设置" GroupName="purchase" />
        </div>
    </div>
    <div id="divGlobal" style="display:none;" runat="server">
        <table class="mini-table">
            <tr>
                <td class="title">
                    扣点设置
                </td>
                <td>
                    <asp:RadioButton runat="server" ID="radQujian" Checked="true" GroupName="radKou"
                        Text="区间扣点" />
                    <asp:RadioButton runat="server" ID="radTongyi" GroupName="radKou" Text="统一返点" />
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
                    <div id="tongyi" runat="server" class="condition" style="display: none">
                    <div class="importantBox broaden">
                        <div class="obvious">
                            设置后您的所有下级用户都将看到同一返点，当供应商发布的返点低于您设置的统一返点时将显示供应商的实际返点，<br />
                            当供应商发布了高于您设置的同一返点时，超出部分的返点都将会做为利润分配到您的账户。</div></div>
                        统一返点值<asp:TextBox runat="server" CssClass="text text-s" ID="txtTongyi" />%</div>
                </td>
                <td><div class="obvious1">请注意，本栏目的设置仅对普通单程政策进行扣点</div></td>
            </tr>
            <tr>
                <td class="title">
                    加价设置
                </td>
                <td>
                    每张票加价<asp:TextBox runat="server" CssClass="text text-s" ID="txtPrice" />元进行出售
                </td>
                <td><div class="obvious1">请注意，本栏目的设置仅对特殊政策生效</div></td>
            </tr>
            <tr>
                <td class="title">
                    备注
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="text" Rows="10" Columns="60" TextMode="MultiLine"
                        ID="txtRemark" />
                </td>
                <td><div class="obvious1">备注本操作的理由，没有则可以为空</div></td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidRanges" />
        <asp:HiddenField runat="server" ID="hidSettingId" />
        <asp:HiddenField runat="server" ID="hidIsFirst" />
        <asp:HiddenField runat="server" ID="hidVId" />
    </div>
    <div>
        <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#rbnPurchaseGlobal").is(":checked")) {
            $("#divGlobal").show();
        }
        $("input[type='radio'][name='radKou']").click(function () {
            if ($(this).attr("Id") == "radQujian") {
                $("#qujian").show();
                $("#tongyi").hide();
            } else {
                $("#qujian").hide();
                $("#tongyi").show();
            }
        });
        $("input[type='radio'][name='purchase']").click(function () {
            if ($(this).attr("Id") == "rbnPurchaseGlobal") {
                $("#divGlobal").show();
            } else {
                $("#divGlobal").hide();
            }
        });
        $("#btnSave").click(function () {
            if ($("#divGlobal").css("display") != "none") {
                return vail();
            } return true;
        });
        $("#btnCancel").click(function () {
            window.location.href = 'IncomeGroupList.aspx?Search=Back';
        }); 
    });
    function vail() {
        if (!$("#rbnPurchaseEach").is(":checked") && !$("#rbnPurchaseGlobal").is(":checked")) {
            alert("必须选择一种设置方式后才能保存！");
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
