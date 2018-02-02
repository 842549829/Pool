<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tkdian_policy.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.tkdian_policy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>对政策贴扣点设置</title>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <div id="smallbd1">
        <ul class="navType1" id="sel" runat="server">
            <li><a href="javascript:;" class="navType1Selected" id="koudianTip" runat="server">扣点</a></li>
            <li><a href="javascript:" id="tiedianTip" runat="server">贴点</a></li>
        </ul>
        <h3 class="titleBg">
            <asp:Label ID="lblAddOrUpdate" runat="server" Text="添加"></asp:Label>政策设置 -
            <label class="koudian">
                扣点</label></h3>
    </div>
    <div class="form">
        <table>
            <col class="w15" />
            <col class="w35" />
            <col class="w15" />
            <col class="w35" />
            <tr>
                <td class="title">
                    政策发布方
                </td>
                <td>
                    <asp:Label ID="lblCreator" runat="server"></asp:Label>
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
                    航空公司
                </td>
                <td>
                    <asp:Label ID="lblAirline" runat="server"></asp:Label>
                </td>
                <td class="title">
                    航班日期
                </td>
                <td>
                    <asp:Label ID="lblDepartureDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    出发城市
                </td>
                <td>
                    <asp:Label ID="lblDeparture" runat="server"></asp:Label>
                </td>
                <td class="title">
                    到达城市
                </td>
                <td>
                    <asp:Label ID="lblArrival" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    航班限制
                </td>
                <td>
                    <asp:Label ID="lblDepartureFilght" runat="server"></asp:Label>
                </td>
                <td class="title">
                    排除航段
                </td>
                <td>
                    <asp:Label ID="lblExceptAirlines" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    <label class="koudian">
                        扣点</label>航线
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtAirlines" CssClass="text text_width"></asp:TextBox>
                    <div class="obvious1">
                        <label class="koudian">
                            扣点</label>航线请按照出发城市达到城市的三字码格式输入，多个请用/隔开，如KMGCTU/SHAPEK、支持*号通配符，如KMG*表示KMG出港到该政策内航线均进行<label
                                class="koudian">扣点</label></div>
                </td>
            </tr>
            <tr>
                <td class="title">
                    适合舱位
                </td>
                <td colspan="3">
                    <asp:CheckBoxList ID="chkBunksList" runat="server" RepeatColumns="15" RepeatDirection="Horizontal"
                        Width="600px" RepeatLayout="Flow">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="title">
                    <label class="koudian">
                        扣点</label>数值
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtCommission" CssClass="text text-s fl"></asp:TextBox>
                    <span class=" fl">该政策的返点：<asp:Label ID="lblCommission" runat="server"></asp:Label>%</span>
                    <span class="obvious1">请注意，按照你输入的<label class="koudian">扣点</label>值，采购所见到的返点将是该政策发布的同行返点加上你的<label
                        class="koudian">扣点</label>值总和 </span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    <label class="koudian">
                        扣点</label>期限
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtStartTime" CssClass="text text-s fl" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>
                    <span class="fl">至</span>
                    <asp:TextBox runat="server" ID="txtEndTime" CssClass="text text-s fl" onclick="WdatePicker({ isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '#F{$dp.$D(\'hidTime\')}' })"></asp:TextBox>
                    <span class="obvious1 fl">
                        <label class="koudian">
                            扣点</label>日期到期后将不再执行<label class="koudian">扣点</label>，请注意，<label class="koudian">扣点</label>截止日期不能大于航班日期。
                    </span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    <label class="koudian">
                        扣点</label>备注
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemark" TextMode="MultiLine" Height="150px" CssClass="text" MaxLength="400"
                        runat="server" Width="80%"></asp:TextBox>
                    <div class="obvious1 fl">
                        请输入<label class="koudian">扣点</label>备注，没有可为空，最大允许输入400字符。
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="3">
                    <asp:Button CssClass="btn class1" runat="server" ID="btnSave" Text="保存" OnClick="btnSave_Click" />
                    <asp:Button CssClass="btn class2" runat="server" ID="btnCancel" Text="返回" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
        <div id="data-list" class="table">
            政策<label class="koudian">扣点</label>调整记录
            <asp:GridView ID="grvTiedian" runat="server" CssClass="nfo-table info" Width="100%"
                AutoGenerateColumns="false" OnRowCommand="grvTiedian_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="航线" DataField="FlightsFilter" />
                    <asp:BoundField HeaderText="舱位" DataField="Berths" />
                    <asp:BoundField HeaderText="扣点值" DataField="Commission" />
                    <asp:TemplateField HeaderText="扣点期限">
                        <ItemTemplate>
                            <%#Eval("StartTime")%>
                            至
                            <%#Eval("EndTime")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="设置时间" DataField="OperationTime" />
                    <asp:BoundField HeaderText="操作员" DataField="Creator" />
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("Id") +"|"+ Eval("Enable") %>'
                                CommandName="disEnable" runat="server">
                           <%#Convert.ToBoolean(Eval("Enable")) ? "禁用" : "启用" %>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="hidKoudianOrTiedian" runat="server" Value="1" />
    <asp:HiddenField runat="server" ID="hidTime" />
    <asp:Button CssClass="btn class1 hidden" runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" />
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#lblDeparture,#lblArrival,#lblTransit,#lblDepartureFilght,#lblExceptAirlines").tipTip({ maxWidth: "400px", limitLength: 30 });
        if ($("#hidKoudianOrTiedian").val() == "1") {
            $(".koudian").html("扣点");
            $("#sel li a").removeClass("navType1Selected");
            $("#koudianTip").addClass("navType1Selected");
            $("#grvTiedian tr th").eq(2).html("扣点值");
            $("#grvTiedian tr th").eq(3).html("扣点期限");
        } else {
            $(".koudian").html("贴点");
            $("#sel li a").removeClass("navType1Selected");
            $("#tiedianTip").addClass("navType1Selected");
            $("#grvTiedian tr th").eq(2).html("贴点值");
            $("#grvTiedian tr th").eq(3).html("贴点期限");
        }
        $("#btnSave").click(function () {
            var falg = $("#hidKoudianOrTiedian").val();
            if ($.trim($("#txtAirlines").val()) == "") {
                alert("航线不能为空，请输入！");
                return false;
            }
            var reg = /^[A-Za-z*\/]+$/;
            if (!reg.test($.trim($("#txtAirlines").val()))) {
                alert("航线只能为英文字母和斜线，请重新输入！");
                return false;
            }
            if ($("#chkBunksList [type='checkbox']:checked").length == 0) {
                alert("适合舱位不能为空，请选择！");
                return false;
            }
            if ($.trim($("#txtCommission").val()) == "") {
                if (falg == "1") {
                    alert("扣点数值不能为空，请输入扣点数！");
                } else {
                    alert("贴点数值不能为空，请输入贴点数！");
                }
                return false;
            }
            reg = /^[0-9]{1,3}(\.[0-9])?$/; ;
            if (!reg.test($.trim($("#txtCommission").val()))) {
                if (falg == "1") {
                    alert("扣点数值只能为数字，且不能小于零，请输入扣点数！");
                } else {
                    alert("贴点数值只能为数字，且不能小于零，请输入贴点数！");
                }
                return false;
            }
            if ($.trim($("#txtCommission").val()) < 0) {
                if (falg == "1") {
                    alert("扣点数值只能为数字，且不能小于零，请输入扣点数！");
                } else {
                    alert("贴点数值只能为数字，且不能小于零，请输入贴点数！");
                }
                return false;
            }
            if (parseFloat($("#txtCommission").val()) > 10) {
                if (falg == "2") {
                    alert("贴点数值只能为数字，且不能小于零，请输入贴点数！");
                    return false;
                }
            }
            if ($("#txtRemark").val().length >= 400) {
                alert("备注信息过长，请重新输入！");
                return false;
            }
        });
        $("#sel li a").click(function () {
            $("#sel li a").removeClass("navType1Selected");
            $(this).addClass("navType1Selected");
            $("#navTip").html($(this).html());

            if ($(this).attr("id") == "koudianTip") {
                $("#hidKoudianOrTiedian").val("1");
                $(".koudian").html("扣点");
            } else if ($(this).attr("id") == "tiedianTip") {
                $("#hidKoudianOrTiedian").val("2");
                $(".koudian").html("贴点");
            }
            $("#btnQuery").click();
        });
    });
</script>
