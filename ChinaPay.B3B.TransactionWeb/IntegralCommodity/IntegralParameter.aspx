<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralParameter.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.IntegralParameter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分设置</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            积分设置
        </h3>
        <table class="table">
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w50" />
            </colgroup>
            <tr>
                <td class="title">
                    签到积分功能：
                </td>
                <td>
                    <asp:RadioButton ID="radEnableGongNeng" Checked="true" GroupName="gongneng" runat="server"
                        Text="启用" />
                    <asp:RadioButton ID="radDisableGongNeng" GroupName="gongneng" runat="server" Text="禁用" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    签到积分设置：
                </td>
                <td>
                    <asp:TextBox ID="txtSheZhi" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>分
                </td>
                <td>
                    <span>该分值将决定用户当日登录所获得的积分数量</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    未登录积分下降：
                </td>
                <td>
                    <asp:RadioButton ID="radEnableXiaJia" GroupName="XiaJia" Checked="true" runat="server"
                        Text="启用" />
                    <asp:RadioButton ID="radDisableXiaJia" GroupName="XiaJia" runat="server" Text="禁用" />
                </td>
                <td>
                    <span>启用后用户未登录将减少一天的签到积分，直到0为止</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    消费积分设置：
                </td>
                <td>
                    <asp:TextBox ID="txtXiaoFei" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>分
                </td>
                <td>
                    <span>用户每消费100元钱可获得的积分，不足100元的消费不积累积分，如5即消费1万可获得500积分</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    可用积分比例：
                </td>
                <td>
                    <asp:TextBox ID="txtKeYong" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>%
                </td>
                <td>
                    <span>可用积分=总积分 × 可用积分比例%</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    最多扣去积分：
                </td>
                <td>
                    <asp:TextBox ID="txtMostBuckle" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>分
                </td>
                <td>
                    <span>扣掉积分超过最多扣去积分值，可用积分比例无效，只扣去本值。可用积分=总积分 - 最多扣去积分</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    国付通奖励积分：
                </td>
                <td>
                    <asp:TextBox ID="txtJianLi" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>倍
                </td>
                <td>
                    <span>所得积分将以此数值倍增(只能输入一以上的整数)</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    积分循环周期：
                </td>
                <td>
                    <select runat="server" id="selZhouQi" class=" fl">
                        <option value="0">永不</option>
                        <option value="1">五年</option>
                        <option value="2">二年</option>
                        <option value="3">一年</option>
                        <option value="4">半年</option>
                        <option value="5">一季度</option>
                        <option value="6">一月</option>
                        <option value="99">指定日期</option>
                    </select>
                    <asp:TextBox ID="txtZhiDing" CssClass="text text-s fl" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '%y-%M-{%d+1}', maxDate: '2020-10-01' })"
                        runat="server"></asp:TextBox>
                </td>
                <td>
                    <span>该时间为一次循环对积分进行清零</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保存" OnClick="btnSave_Click" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#selZhouQi").change(function () {
            if ($(this).val() == "99") {
                $("#txtZhiDing").css("display", "");
            } else {
                $("#txtZhiDing").css("display", "none");
            }
        });
        $("#btnSave").click(function () {
            var reg = /^[0-9]{1,4}?$/;
            if (!reg.test($("#txtSheZhi").val())) {
                alert("签到积分设置必须是整数，且在四位数以内");
                $("#txtSheZhi").focus();
                return false;
            }
            if (!reg.test($("#txtXiaoFei").val())) {
                alert("消费积分设置必须是整数，且在四位数以内");
                $("#txtXiaoFei").focus();
                return false;
            }
            reg = /^[0-9]{1,3}?$/;
            if (!reg.test($("#txtKeYong").val())) {
                alert("可用积分比例必须是整数，且在三位数以内");
                $("#txtKeYong").focus();
                return false;
            }
            reg = /^[0-9]{1,5}?$/;
            if (!reg.test($("#txtMostBuckle").val())) {
                alert("最多扣去积分必须是整数，且在五位数以内");
                $("#txtMostBuckle").focus();
                return false;
            }
            reg = /^[1-9]{1,2}(\.[0-9])?$/;
            if (!reg.test($("#txtJianLi").val())) {
                alert("国付通奖励积分必须是大于1的数字，且在一位数以内的小数");
                $("#txtJianLi").focus();
                return false;
            } 
        });
    });
</script>
