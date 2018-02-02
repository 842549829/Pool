<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSPackageManage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSPackageManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>短信套餐设置</title>
    <link href="/Styles/icon/fontello.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        短信套餐设置</h3>
    <div class="box-a">
        <div class="condition" style="height: 25px;">
            <input type="button" class="btn class1 fr" id="add" value="添加短信套餐" />
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_Packge" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false" OnRowCommand="grv_Packge_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="金额" DataField="Amount" />
                <asp:BoundField HeaderText="可发条数" DataField="Count" />
                <asp:BoundField HeaderText="单位（元/条）" DataField="UnitPrice" />
                <asp:BoundField HeaderText="兑换所需积分" DataField="ExChangeIntegral" />
                <asp:TemplateField HeaderText="商品状态">
                    <ItemTemplate>
                        <%#Convert.ToBoolean(Eval("Enable")) ? "启用" : "停用"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href="javascript:edit('<%#Eval("id") %>','<%#Eval("Amount") %>','<%#Eval("Count") %>','<%#Eval("EffectiveDate") %>','<%#Eval("ExpiredDate") %>','<%#Eval("SortLevel") %>','<%#Eval("ExChangeIntegral") %>');">
                            编辑</a>
                        <asp:LinkButton runat="server" CommandName="DisEndable" CommandArgument='<%#Eval("Id") + "|" + Eval("Enable") %>'>
                        <%#Convert.ToBoolean(Eval("Enable")) ? "停用":"启用" %></asp:LinkButton>
                        <a href='SMSBuyRecord.aspx?id=<%#Eval("id") %>'>查看购买记录</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="hidFlag" />
    <asp:HiddenField runat="server" ID="hidId" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divSms'}"></a>
    <div id="divSms" class="form layer" style="display: none; width: 500px;">
        <h2>
            添加短信套餐</h2>
        <table>
            <tr>
                <td class="title">
                    设置购买金额
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAounmt" CssClass="text text-s"></asp:TextBox>元
                </td>
                <td class="title">
                    设置可发条数
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNum" CssClass="text text-s"></asp:TextBox>条
                </td>
                <td style="width: 80px;">
                    <label class="obvious1" id="lblDanwei">
                        &nbsp;</label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    设置套餐期限
                </td>
                <td colspan="4">
                    <asp:TextBox runat="server" ID="txtStartTime" CssClass="text text-s1 fl" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}', readOnly:true })"></asp:TextBox><label
                        class="fl">至</label>
                    <asp:TextBox runat="server" ID="txtEndTime" CssClass="text text-s1 fl" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartTime\')}', readOnly:true})"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    设置顺序
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSort" CssClass="text text-s"></asp:TextBox>
                </td>
                <td colspan="3" class="obvious1">
                    排序顺序将按照您输入的数字从小到大的升序排列
                </td>
            </tr>
            <tr>
                <td class="title">
                    所需积分
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtExChangeIntegral" CssClass="text text-s"></asp:TextBox>
                </td>
                <td colspan="3" class="obvious1">
                    请输入该套餐若用积分兑换所需要的积分<br />不填则表示不支持积分兑换
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    <asp:Button runat="server" ID="btnSave" Text="提交" CssClass="btn class1" OnClick="btnSave_Click" />
                    <input type="button" value="取消" class="btn class2 close" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    function edit(id, aounmt, num, startTime, endTime, sort, integral) {
        $("#txtAounmt").val(aounmt);
        $("#txtNum").val(num);
        $("#hidId").val(id);
        $("#txtStartTime").val(startTime);
        $("#txtEndTime").val(endTime);
        $("#txtSort").val(sort);
        $("#txtExChangeIntegral").val(integral);

        $("#txtNum , #txtAounmt").attr("readonly", "readonly");
        $("#txtNum").keyup();
        $("#divOpcial").click();
    }
    $(function () {
        if ($("#hidFlag").val() == "1") {
            $("#divOpcial").click();
        }
        $("#add").click(function () {
            $("#divSms input[type='text']").val("");
            $("#lblDanwei").html("");
            $("#txtNum , #txtAounmt").removeAttr("readonly");
            $("#hidId").val("");
            $("#divOpcial").click();
        });
        $("#btnSave").click(function () {
            var reg = /^[0-9]{1,10}?$/;
            if ($.trim($("#txtAounmt").val()) == "") {
                alert("设置购买金额不能为空！请输入金额。");
                return false;
            }
            if (!reg.test($.trim($("#txtAounmt").val()))) {
                alert("设置购买金额只能为整数！请输入金额。");
                return false;
            }
            if ($.trim($("#txtNum").val()) == "") {
                alert("设置可发条数不能为空！请输入可发条数。");
                return false;
            }
            if (!reg.test($.trim($("#txtNum").val()))) {
                alert("设置可发条数只能为整数！请输入可发条数。");
                return false;
            }
            if ($.trim($("#txtStartTime").val()) == "" || $.trim($("#txtEndTime").val()) == "") {
                alert("套餐期限不能为空！请选择套餐期限时间。");
                return false;
            }
            if ($.trim($("#txtSort").val()) == "") {
                alert("设置顺序不能为空！请输入顺序。");
                return false;
            }
            if (!reg.test($.trim($("#txtSort").val()))) {
                alert("设置顺序只能为整数！请输入设置顺序。");
                return false;
            }
            if ($.trim($("#txtExChangeIntegral").val()) != "") {
                if (!reg.test($.trim($("#txtExChangeIntegral").val()))) {
                    alert("兑换积分只能为整数并且大于零！请输入兑换积分。");
                    return false;
                }
                if (parseInt($("#txtExChangeIntegral").val()) <= 0) {
                    alert("兑换积分只能为整数并且大于零！请输入兑换积分。");
                    return false;
                }
            }
        });
        $("#txtNum , #txtAounmt").keyup(function () {
            var reg = /^[0-9]{1,10}?$/;
            if ($.trim($("#txtNum").val()) != "" && reg.test($.trim($("#txtNum").val())) && $.trim($("#txtAounmt").val()) != "" && reg.test($.trim($("#txtAounmt").val()))) {
                $("#lblDanwei").html(fillZero(Round(parseFloat($.trim($("#txtAounmt").val())) / parseFloat($.trim($("#txtNum").val())), 2)) + "元/条");
            }
        });
    });
</script>
