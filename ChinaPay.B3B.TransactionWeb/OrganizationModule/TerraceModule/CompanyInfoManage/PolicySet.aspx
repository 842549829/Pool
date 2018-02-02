<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicySet.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.PolicySet" %>

<%@ Register Src="../../../UserControl/MultipleAirport.ascx" TagName="MultipleAirport"
    TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head><script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style>
        .table tr td{border-bottom:0;}
    </style>

<body>
    <form id="form1" runat="server">
    <div class="col-2">
    <div class="col form" style="width:60%;">
        <h3 class="titleBg">可发布航空公司和出港城市设置</h3>
        <table>
            <tr>
                <td class="title">
                    可发布特价政策条数：
                </td>
                <td>
                    <asp:TextBox ID="txtPromotionCount" runat="server" CssClass="text"></asp:TextBox>
                </td>
                <td class="title">
                    可发布单程控位政策条数：
                </td>
                <td>
                    <asp:TextBox ID="txtSinglenessCount" runat="server" CssClass="text"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td class="title"> 可发布散冲团政策条数：</td>
            <td><asp:TextBox ID="txtDisperseCount" runat="server" CssClass="text"></asp:TextBox></td>
            <td class="title"> 可发布免票政策条数：</td>
            <td>  <asp:TextBox ID="txtCostFreeCount" runat="server" CssClass="text"></asp:TextBox></td>
            </tr>
            <tr>
            <td class="title">可发布集团政策条数：</td>
            <td> <asp:TextBox ID="txtBlocCount" runat="server" CssClass="text"></asp:TextBox></td>
            <td class="title">可发布商旅卡政策条数：</td>
            <td><asp:TextBox ID="txtBusinessCount" runat="server" CssClass="text"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="title">可发布其他特殊政策：</td>
                <td>
                    <asp:TextBox ID="txtOtherSpecialCount" runat="server" CssClass="text"></asp:TextBox>
                </td>
                <td class="title">可发布低打高返政策条数：</td>
                <td>
                    <asp:TextBox ID="txtLowToHighCount" runat="server" CssClass="text" ></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <br />
    <div class="col-2" style="width: 98%; height: 300px;">
        <div class="col table" style="width: 45%; height: 290px; border: 1px solid #cecece;">
            <h3 class="titleBg">
                航空公司</h3>
            <input type="radio" value="0" name="choice" id="checkAll" /><label for="checkAll">全选</label>
            <input type="radio" value="1" name="choice" id="Invert" /><label for="Invert">反选</label><br />
            <asp:CheckBoxList ID="chklAirline" runat="server" RepeatColumns="5">
            </asp:CheckBoxList>
        </div>
        <div class="col" style="width: 45%; height: 290px; padding-left: 5%; border: 1px solid #cecece;">
            <h3 class="titleBg">
                出港城市</h3>
            <uc:MultipleAirport ID="ucMultipleAirport" runat="server" />
        </div>
    </div>
    <br />
    <div class="btns">
        <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
        <input type="button" onclick="window.location.href='./CompanyList.aspx?Search=Back'" value="返回"
            class="btn class2" />
    </div>
    </form>
    <script type="text/javascript" src="../../../Scripts/airport.js"></script>
    <script src="../../../Scripts/OrganizationModule/CheckedAll.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            choice("chklAirline");
            if ($("#chklAirline :checkbox:not(:checked)").length == 0) { $("#checkAll").attr("checked", true); } else { $("#Invert").attr("checked", true); }
            $(".col:last :input:text").css("width", "100%");
            /*验证*/
            $("#btnSave").click(function () {
                var reg = /^\d{1,9}$/, PromotionCount = $("#txtPromotionCount"), SinglenessCount = $("#txtSinglenessCount"),
                DisperseCount = $("#txtDisperseCount"), CostFreeCount = $("#txtCostFreeCount"), BlocCount = $("#txtBlocCount"), BusinessCount = $("#txtBusinessCount"),
                OtherSpecialCount = $("#txtOtherSpecialCount");
                if (!reg.test(PromotionCount.val())) {
                    alert("可发布特价政策条数格式错误！");
                    PromotionCount.select();
                    return false;
                }
                if (!reg.test(SinglenessCount.val())) {
                    alert("可发布单程控位政策条数格式错误！");
                    SinglenessCount.select();
                    return false;
                }
                if (!reg.test(DisperseCount.val())) {
                    alert("可发布散冲团政策条数格式错误！");
                    DisperseCount.select();
                    return false;
                }
                if (!reg.test(CostFreeCount.val())) {
                    alert("可发布免票政策条数格式错误！");
                    CostFreeCount.select();
                    return false;
                }
                if (!reg.test(BlocCount.val())) {
                    alert("可发布集团票政策条数格式错误！");
                    BlocCount.select();
                    return false;
                }
                if (!reg.test(BusinessCount.val())) {
                    alert("可发布商旅卡政策条数格式错误！");
                    BusinessCount.select();
                    return false;
                }
                if (!reg.test(OtherSpecialCount.val())) {
                    alert("可发布其他特殊政策条数格式错误！");
                    OtherSpecialCount.select();
                    return false;
                }
                if ($("#chklAirline input[type='checkbox']:checked").length == 0) {
                    alert("必须选择一个航空公司！");
                    return false;
                }
                if ($.trim($("#ucMultipleAirport_txtAirports").val()) == "") {
                    alert("出港城市不能为空！");
                    $("#ucMultipleAirport_txtAirports").select();
                    return false;
                }
            });
        });
    </script>
</body>
</html>
