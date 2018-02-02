<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommodityAddOrUpdate.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.IntegralCommodity.CommodityAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品添加/修改</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 runat="server" class="titleBg" id="title">
            添加商品</h3>
        <table class="table">
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w10" />
                <col class="w40" />
            </colgroup>
            <tr>
                <td class="title">
                    商品类型
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlType">
                        <asp:ListItem Text="实物商品" Value="1"></asp:ListItem>
                        <asp:ListItem Text="短信商品" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <span id="spanNum" runat="server">兑换短信条数</span>
                </td>
                <td>
                    <span id="spanNumber" runat="server">
                        <asp:TextBox ID="txtNum" MaxLength="5" CssClass="text text-s" runat="server">1</asp:TextBox>条</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    商品名称
                </td>
                <td>
                    <asp:TextBox ID="txtCoommodityName" MaxLength="20" CssClass="text" runat="server"></asp:TextBox>
                    <asp:Label runat="server" ID="lblName">短信套餐 1 条</asp:Label>
                </td>
                <td colspan="2">
                    输入商品名称，最多20字
                </td>
            </tr>
            <tr>
                <td class="title">
                    显示顺序
                </td>
                <td>
                    <asp:TextBox ID="txtSort" CssClass="text text-s" MaxLength="5" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    只能输入整数
                </td>
            </tr>
            <tr>
                <td class="title">
                    兑换所需积分
                </td>
                <td>
                    <asp:TextBox ID="txtNeedIntegral" CssClass="text text-s" runat="server"></asp:TextBox>分
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="title">
                    商品状态
                </td>
                <td>
                    <asp:RadioButton ID="radEnable" Checked="true" GroupName="gongneng" runat="server"
                        Text="启用" />
                    <asp:RadioButton ID="radDisable" GroupName="gongneng" runat="server" Text="禁用" />
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="title">
                    商品图片
                </td>
                <td>
                    <asp:FileUpload ID="fileImg" CssClass="text" runat="server" />
                </td>
                <td colspan="2">
                    支持jpg,jpeg,png,gif图片&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                        class="obvious" runat="server" style="position: relative;" id="OldImgUrlDiv"
                        visible="false"><a href="javascript:;" onmouseover="ShowoldImg();" onmouseout="HideoldImg();">查看原图片</a>
                        <img style="position: absolute; display: none; background-color: white; width: 220px;
                            height: 220px;" class="box" id="oldimg" />
                        <asp:Label runat="server" ID="OldImgUrl"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    图片描述
                </td>
                <td>
                    <asp:TextBox ID="txtRemark" MaxLength="80" CssClass="text" TextMode="MultiLine" Height="50px"
                        Width="80%" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    输入图片描述信息最多80字
                </td>
            </tr>
            <tr>
                <td class="title">
                    有效期至
                </td>
                <td>
                    <asp:TextBox ID="txtTime" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '%y-%M-{%d+1}', maxDate: '2020-10-01' })"
                        runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnAdd" CssClass="btn class1" runat="server" Text="添加" OnClick="btnAdd_Click" />
                    <input type="button" onclick="javascript:window.location.href='./CommodityList.aspx';"
                        value="返回" class="btn class2" />
                </td>
                <td colspan="2">
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidName" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#ddlType").val() == "1") {
            $("#spanNum,#spanNumber,#lblName").hide();
            $("#txtCoommodityName").show();
        } else if ($("#ddlType").val() == "2") {
            $("#spanNum,#spanNumber,#lblName").show();
            $("#txtCoommodityName").hide();
        }
        $("#ddlType").change(function () {
            if ($(this).val() == "1") {
                $("#spanNum,#spanNumber,#lblName").hide();
                $("#txtCoommodityName").show();
            } else if ($(this).val() == "2") {
                $("#spanNum,#spanNumber,#lblName").show();
                $("#txtCoommodityName").hide();
            }
        });
        $("#txtNum").keyup(function () {
            var reg = /^[0-9]{1,20}?$/;
            if ($("#txtNum").val() == "" || !reg.test($("#txtNum").val()) || parseInt($("#txtNum").val()) <= 0) {
                return;
            }
            $("#lblName").html("短信套餐 " + $("#txtNum").val() + " 条");
            $("#hidName").val("短信套餐 " + $("#txtNum").val() + " 条");
        });
        $("#btnAdd").click(function () {
            var reg = /^[0-9]{1,20}?$/;
            if ($("#ddlType").val() == "1") {
                if ($("#txtCoommodityName").val() == "") {
                    alert("请先填写商品名称");
                    return false;
                }
            } else if ($("#ddlType").val() == "2") {
                if ($("#txtNum").val() == "") {
                    alert("请先填写兑换短信条数");
                    return false;
                }
                if (!reg.test($("#txtNum").val())) {
                    alert("兑换短信条数只能为整数");
                    return false;
                }
                if (parseInt($("#txtNum").val()) <= 0) {
                    alert("兑换短信条数只能为整数");
                    return false;
                }
            }
            if ($("#txtSort").val() == "") {
                alert("请先填写商品显示顺序");
                return false;
            }
            if (!reg.test($("#txtSort").val())) {
                alert("商品显示顺序只能为整数");
                return false;
            }
            if ($("#txtNeedIntegral").val() == "") {
                alert("请先填写商品所需积分");
                return false;
            }
            if (!reg.test($("#txtNeedIntegral").val())) {
                alert("商品所需积分只能为整数");
                return false;
            }
            if ($("#fileImg").val() == "" && $("#OldImgUrl").text() == "") {
                alert("请先选择展示图片");
                return false;
            }
            return true;
        });
    });
    function ShowoldImg() {
        var url = $("#OldImgUrl").html();
        $("#oldimg").attr("src", url);
        $("#oldimg").css("display", "");
    };
    function HideoldImg() {
        $("#oldimg").css("display", "none");
    };
</script>
