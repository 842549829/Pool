<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="policy_set_addormodify.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.policy_set_addormodify"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="AirLines" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>政策设置管理</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <style type="text/css">
        #AirLines_txtAirports
        {
            width: 400px;
        }
        #txtTiedianStart, #txtTiedianEnd
        {
            width: 30px;
        }
        #bunks input
        {
            margin: 0 5px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <div id="smallbd1">
        <ul class="navType1" id="sel" runat="server">
            <li><a href="javascript:;" class="navType1Selected" id="koudianTip" runat="server">扣点</a></li>
            <li><a href="javascript:" id="tiedianTip" runat="server">贴点</a></li>
        </ul>
            <h3 class="titleBg">
                政策设置 -
                <label id="navTip" runat="server">
                    扣点</label></h3>
        <div class="form">
            <table class=" box">
                <colgroup>
                    <col class="w20" />
                    <col class="w80" />
                </colgroup>
                <tbody>
                    <tr>
                        <td class="title">
                            航空公司：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAirLine" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            始发地：
                        </td>
                        <td>
                            <uc:City runat="server" ID="txtDeparture"></uc:City>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            目的地：
                        </td>
                        <td>
                            <uc:AirLines runat="server" ID="AirLines"></uc:AirLines>
                            <br />
                            <br />
                            <br />
                            <br />
                            <div class="tips-box radius">
                                <i class="icon icon-attention-circle"></i><strong>温馨提示：</strong> <span>支持多选，可用Shift连选，也支持Ctrl间隔选择。可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK</span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            航班日期：
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="datepicker datefrom btn class3"></asp:TextBox>
                            <span class="fl-l">至</span>
                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="datepicker datefrom btn class3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            适用舱位：
                        </td>
                        <td>
                            <div>
                                <input type="radio" name="radBunks" id="radAll" value="0" /><label for="radAll">全选</label>
                                <input type="radio" name="radBunks" id="radNot" value="1" /><label for="radNot">反选</label>
                                <br />
                                <span id="bunks" runat="server"></span>
                                <input type="button" id="btnBerths" class="btn class1" value="获取舱位" style="float: right" />
                            </div>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td class="title">
                            政策设置：
                        </td>
                        <td>
                            <asp:RadioButton ID="radKoudian" runat="server" Text="扣点" GroupName="raddian" Checked="true" />
                            <asp:RadioButton ID="radTiedian" runat="server" Text="贴点" GroupName="raddian" />
                        </td>
                    </tr>--%>
                    <tr runat="server" id="koudian">
                        <td class="title">
                        </td>
                        <td>
                            <div class="parent_div" style="width: 660px">
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
                        </td>
                    </tr>
                    <%--                    <tr runat="server" id='tiedianTime'>
                        <td class="title">
                            贴点时段：
                        </td>
                        <td>
                            <asp:TextBox ID="txtTiedianStart" runat="server" CssClass="datepicker datefrom btn class3"
                                MaxLength="8" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate: '#F{$dp.$D(\'txtTiedianEnd\')}'})"></asp:TextBox>
                            至
                            <asp:TextBox ID="txtTiedianEnd" runat="server" CssClass="datepicker datefrom btn class3"
                                MaxLength="8" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate: '#F{$dp.$D(\'txtTiedianStart\')}'})"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr runat="server" id='tiedian'>
                        <td class="title">
                            贴点值：
                        </td>
                        <td>
                            <asp:TextBox ID="txtTiedian" runat="server" CssClass="text text-s"></asp:TextBox>%
                            <div class="obvious1">
                                该数值为采购最终所见返点</div>
                        </td>
                    </tr>
                    <tr runat="server" id='maxtiedian'>
                        <td class="title">
                            最高贴点：
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxTiedian" runat="server" CssClass="text text-s"></asp:TextBox>%
                            <div class="obvious1">
                                当平台需要贴点大于该数值时贴点字段失效</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            启用：
                        </td>
                        <td>
                            <asp:RadioButton runat="server" ID="radEnable" Text="启用" Checked="true" GroupName="rad" />
                            <asp:RadioButton runat="server" ID="radDisable" Text="禁用" GroupName="rad" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            备注：
                        </td>
                        <td>
                            <textarea style="width: 400px; height: 110px;" id="txtRemark" class="text" cols="10"
                                rows="10" runat="server"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保存" OnClick="btnSave_Click" />
                            <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='policy_set_manage.aspx'" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidRanges" />
    <asp:HiddenField runat="server" ID="hidBunks" />
    <asp:HiddenField runat="server" ID="hidAddOrUpdate" />
    <asp:HiddenField ID="hidKoudianOrTiedian" runat="server" Value="1" />
    </form>
</body>
</html>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#hidKoudianOrTiedian").val() == "1") {
            $("#tiedian").hide();
            $("#maxtiedian").hide();
            //            $("#tiedian input").hide();
            $("#koudian").show();
            //            $("#koudian input").show();
        } else {
            $("#koudian").hide();
            $("#maxtiedian").show();
            //            $("#koudian input").hide();
            $("#tiedian").show();
            //            $("#tiedian input").show();
        }
        $("input[type='radio'][name='radBunks']").click(function () {
            if ($(this).val() == "0") {
                for (var i = 0; i < $("#bunks input[type='checkbox']").length; i++) {
                    $("#bunks input[type='checkbox']").eq(i).attr("checked", "checked");
                }
            }
            if ($(this).val() == "1") {
                for (var i = 0; i < $("#bunks input[type='checkbox']").length; i++) {
                    if ($("#bunks input[type='checkbox']").eq(i).is(":checked")) {
                        $("#bunks input[type='checkbox']").eq(i).removeAttr("checked");
                    } else {
                        $("#bunks input[type='checkbox']").eq(i).attr("checked", "checked");
                    }
                }
            }
            var bunks = new Array();
            $("#bunks :checked").each(function () {
                bunks.push($(this).val());
            });
            $("#hidBunks").val(bunks.join(','));
        });
        $("#sel li a").click(function () {
            $("#sel li a").removeClass("navType1Selected");
            $(this).addClass("navType1Selected");
            $("#navTip").html($(this).html());

            if ($(this).attr("id") == "koudianTip") {
                $("#hidKoudianOrTiedian").val("1");
                $("#tiedian").hide();
                $("#maxtiedian").hide();
                $("#tiedian input").hide();
                $("#koudian").show();
                $("#koudian input").show();
            } else if ($(this).attr("id") == "tiedianTip") {
                $("#hidKoudianOrTiedian").val("2");
                $("#koudian").hide();
                $("#koudian input").hide();
                $("#maxtiedian").show();
                $("#tiedian").show();
                $("#tiedian input").show();
            }
        });
        $("#ddlAirLine").change(function () {
            $("#bunks").html("");
        });
        $("#btnSave").click(function () {
            if ($("#ddlAirLine").val() == "") {
                alert("请选择航空公司！");
                return false;
            }
            if ($.trim($("#txtDeparture_txtAirport").val()) == "") {
                alert("请选择出发地！");
                return false;
            }
            if ($.trim($("#AirLines_txtAirports").val()) == "") {
                alert("请选择目的地！");
                return false;
            }
            if (($("#AirLines_divInclude :radio").eq(1).attr("checked") == "checked")) {
                if ($("#AirLines_lbSource option").length == 0) {
                    alert("不能不包含所有目的地！");
                    return false;
                }
            }
            if ($("#hidBunks").val() == "") {
                alert("请选择适用舱位！");
                return false;
            }
            if ($("#tiedian").size() != 0 && $("#tiedian").css("display") != "none") {
                var reg = /^[0-9]{1,3}(\.[0-9])?$/;
                if (!reg.test($("#txtTiedian").val())) {
                    alert("贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                } if (!reg.test($("#txtTiedian").val())) {
                    alert("贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                }
                if (parseFloat($("#txtTiedian").val()) > 100 || parseFloat($("#txtTiedian").val()) < 0) {
                    alert("贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                }
                if (!reg.test($("#txtMaxTiedian").val())) {
                    alert("最高贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                } if (!reg.test($("#txtMaxTiedian").val())) {
                    alert("最高贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                }
                if (parseFloat($("#txtMaxTiedian").val()) > 100 || parseFloat($("#txtMaxTiedian").val()) < 0) {
                    alert("最高贴点值必须为整数或小数，不能超过100，且不能小于零！");
                    return false;
                }
            }
            if ($.trim($("#txtRemark").val()).length > 100) {
                alert("备注信息位数不能超过100位！");
                $("#txtRemark").select();
                return false;
            }
        });
    })
</script>
