<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="low_price_policy_edit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.low_price_policy_edit" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="OneCity" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特价政策修改</title>
</head>
<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/airport.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20130416" type="text/javascript"></script>
<style type="text/css">
    .text_width, .text
    {
        width: 300px;
    }
    .text_short
    {
        width: 50px;
    }
    .textarea_width
    {
        height: 60px;
        width: 98%;
    }
    .fl_th
    {
        margin-left: 20px;
    }
    .fl
    {
        line-height: 28px;
    }
</style>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg" runat="server">
            <span id="tip" runat="server">特价政策修改</span> —— <span id="titlePolicy" runat="server">
                单程</span>
        </h3>
        <table class="tab-mid">
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
                    <td colspan="3">
                        <asp:Label ID="lblAirline" runat="server" CssClass="fl"></asp:Label>
                        <asp:Label ID="lblCustomerCode" CssClass="fl" runat="server" Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlAirline" runat="server" CssClass="fl">
                        </asp:DropDownList>
                        <span id="cutomeTh" runat="server" class="fl fl_th">自定义编号</span>
                        <asp:DropDownList ID="ddlCustomCode" CssClass="fl fl_th" runat="server">
                        </asp:DropDownList>
                        <span id="selOfficeTd">
                            <label class="fl fl_th">
                                OFFICE号</label>
                            <asp:DropDownList ID="dropOffice" CssClass="fl fl_th" runat="server">
                            </asp:DropDownList>
                            <span class="obvious2 fl">需授权</span>
                            <asp:HiddenField runat="server" ID="hidOfficeNo" />
                        </span>
                    </td>
                </tr>
                <tr id="dancheng" runat="server">
                    <td class="title" style="vertical-align: top; min-width: 50px;">
                        始发地
                    </td>
                    <td>
                        <uc:City ID="txtDepartureAirports" runat="server" />
                        <br />
                    </td>
                    <td class="title" style="display: block; height: 317px; vertical-align: top; min-width: 50px;
                        position: relative;">
                        <span id="zhongzhuandi" runat="server">目的地</span>
                        <div>
                            <a href='javascript:;' id='duihuan' runat="server" class='policyBtn2'>←调换→</a></div>
                    </td>
                    <td>
                        <uc:City ID="txtArrivalAirports" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr id="shifadi" runat="server" class="diaohuan" visible="False">
                    <td class="title">
                        始发地
                    </td>
                    <td colspan="2" style="display: block; position: relative;">
                        <uc:OneCity ID="txtShifaAirports" runat="server" />
                        <%--<label class="policyBtn3" id="diaohuanshifa">
                            调换始发地和目的地</label>--%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="zhongzhuanTh" class="diaohuan" runat="server" visible="False">
                    <td class="title">
                        目的地
                    </td>
                    <td colspan="2">
                        <uc:City ID="txtZhongzhuanAirports" runat="server" />
                        <br />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <p class="obvious1">
                            <br />
                            温馨提示： 支持多选，可用Shift连选，也支持Ctrl间隔选择。 可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。 输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK
                        </p>
                    </td>
                </tr>
                <tr id="paichu" runat="server">
                    <td class="title">
                        排除航段
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text text_width" ID="txtOutWithFilght"></asp:TextBox>
                    </td>
                    <td>
                        <p class="obvious1" id="paichutishi" runat="server">
                            提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）；支持多城市混合输入，如KMG/SHACTU/KMGPEKXIY这样的航段均会被排除
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <label runat="server" id="qucheng">
                            去程航班</label>
                    </td>
                    <td colspan="2">
                        <asp:RadioButton ID="radBuXian" runat="server" Text="不限" GroupName="DepartureFilght"
                            Checked="true" />
                        <asp:RadioButton ID="radYiXia" runat="server" Text="仅适用以下航班" GroupName="DepartureFilght" />
                        <asp:RadioButton ID="radBuYiXia" runat="server" Text="不适用以下航班" GroupName="DepartureFilght" />
                        <br />
                        <asp:TextBox runat="server" CssClass="text text_width" ID="txtDepartrueFilght"></asp:TextBox>
                    </td>
                    <td>
                        <p class="obvious1">
                            提示： 不加航空公司二字码，在输入框中输入限定的航班号，支持 * 匹配（如580*即支持5801到5809）。多航班请用“/ ”分隔，如：4590/580*
                        </p>
                    </td>
                </tr>
                <tr id="returnFilghtDates" runat="server">
                    <td class="title">
                        <label runat="server" id="huicheng">
                            回程航班</label>
                    </td>
                    <td colspan="2">
                        <asp:RadioButton ID="radReturnBuXian" runat="server" Text="不限" GroupName="ReturnFilght"
                            Checked="true" />
                        <asp:RadioButton ID="radReturnYiXia" runat="server" Text="仅适用以下航班" GroupName="ReturnFilght" />
                        <asp:RadioButton ID="radReturnBuYiXia" runat="server" Text="不适用以下航班" GroupName="ReturnFilght" />
                        <br />
                        <asp:TextBox runat="server" CssClass="text text_width" ID="txtReturnFilght">
                        </asp:TextBox>
                    </td>
                    <td>
                        <p class="obvious1">
                            提示： 不加航空公司二字码，在输入框中输入限定的航班号，支持 * 匹配（如580*即支持5801到5809）。多航班请用“/ ”分隔，如：4590/580*
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        退改约定
                    </td>
                    <td colspan="3">
                        更改规定：
                        <asp:DropDownList ID="selChangeRegulation" runat="server" Style="width: 500px;">
                        </asp:DropDownList>
                        <br />
                        <br />
                        作废规定：
                        <asp:DropDownList ID="selInvalidRegulation" runat="server" Style="width: 500px;">
                        </asp:DropDownList>
                        <br />
                        <br />
                        退票规定：
                        <asp:DropDownList ID="selRefundRegulation" runat="server" Style="width: 500px;">
                        </asp:DropDownList>
                        <br />
                        <br />
                        签转规定：
                        <asp:DropDownList ID="selEndorseRegulation" runat="server" Style="width: 500px;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        出票条件
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text textarea_width" ID="txtDrawerCondition"
                            MaxLength="200" TextMode="MultiLine">
                        </asp:TextBox>
                    </td>
                    <td class="obvious1">
                        提示：如果有特别要说明的出票条件，请填写，若没有，则不用填。本内容会展示给买家，请谨慎填写。
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        政策备注
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text textarea_width" ID="txtRemark" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="obvious1">
                        提示：本内容只有卖家才能查看，主要是提示出票员核对政策使用规定。避免犯错，提高操作标准。若没有，则不用填。
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField runat="server" ID="hidShifa" />
    </div>
    <div class="groupCt">
        <div class="groupBox1">
            <div class="clearfix">
                <div class="fl pd_right groupLine">
                    去程航班日期：
                    <asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtDepartrueStart"></asp:TextBox>
                    <span class="fl-l">至</span>
                    <asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtDepartrueEnd"></asp:TextBox>
                </div>
                <div class="fl pd_right groupLine">
                    开始出票日期：<asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtProvideDate"></asp:TextBox>
                </div>
                <div class="fl groupLine">
                    <input type="button" value="刷新舱位" id="btnRef" class="btn class2" />
                </div>
            </div>
            <div class="groupLine">
                航班排除日期：
                <asp:TextBox runat="server" CssClass="text text_width" ID="txtPaiChu"></asp:TextBox>
                <p class="obvious1 pd">
                    请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开。
                </p>
            </div>
            <div class="pd_left">
                适用班期：
                <input type="checkbox" id="mon" value="1" runat="server" />
                <label for="mon">
                    周一</label>
                <input type="checkbox" id="tue" value="2" runat="server" />
                <label for="tue">
                    周二</label>
                <input type="checkbox" id="wed" value="3" runat="server" />
                <label for="wed">
                    周三</label>
                <input type="checkbox" id="thur" value="4" runat="server" />
                <label for="thur">
                    周四</label>
                <input type="checkbox" id="fri" value="5" runat="server" />
                <label for="fri">
                    周五</label>
                <input type="checkbox" id="sat" value="6" runat="server" />
                <label for="sat">
                    周六</label>
                <input type="checkbox" id="sun" value="7" runat="server" />
                <label for="sun">
                    周日</label><span class="obvious1">提示：若某个班期不适用，请将周期前的勾去掉</span>
            </div>
            <div class="pd_left groupLine" id="tiqianTh" runat="server">
                最少提前天数：
                <asp:TextBox runat="server" CssClass="text text_short" ID="txtTiQianDays"></asp:TextBox>天<span
                    class="obvious1">提示：请在黑屏输入相应航段NFD后NFN内最晚天数（即最晚提前预定天数）；若无限制则留空</span>
            </div>
            <div class="pd_left groupLine" id="zuiduo" runat="server">
                最多提前天数：
                <asp:TextBox runat="server" CssClass="text text_short" ID="txtMostTiQianDays"></asp:TextBox>天<span
                    class="obvious1">提示：请在黑屏输入相应航段NFD后NFN内最早天数（即最早提前预定天数）；若无限制则留空</span>
            </div>
            <div class="pd_left groupLine" id="chuxingTh" runat="server">
                出行天数：
                <asp:TextBox runat="server" CssClass="text text_short" ID="txtChuxing"></asp:TextBox>
                天 <span class="obvious1">提示：在输入框输入出行天数，没有可以不填 </span>
            </div>
        </div>
        <div class="groupBox2">
            <table>
                <tbody>
                    <tr>
                        <th>
                            舱位
                        </th>
                        <th id="discountTh" runat="server">
                            价格/折扣
                        </th>
                        <th>
                            客票类型
                        </th>
                        <th id="neibuTh" runat="server">
                            内部佣金
                        </th>
                        <th>
                            下级佣金
                        </th>
                        <th id="tongTh" runat="server">
                            同行佣金
                        </th>
                    </tr>
                    <tr>
                        <td class="BunksRad">
                            <asp:HiddenField runat="server" ID="hidBunks" />
                            <select id="ddlBunks" style="width: 50px;">
                            </select>
                        </td>
                        <td class="priceORdiscount" id="discount" runat="server">
                            <select onchange="selPriceOrDiscount(this)" runat="server" id="selPrice">
                                <option value="0">按价格发布</option>
                                <option value="1">按折扣发布</option>
                                <option value='3'>按返佣发布</option>
                            </select>
                            <span class="price0" runat="server" id="price0">
                                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtPrice"></asp:TextBox>元</span>
                            <span class="discount0" style="display: none;" runat="server" id="discount0">
                                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtDiscount"></asp:TextBox>折</span>
                            <span class="fanyong" style="display: none;" runat="server" id="fanyong">&nbsp;</span>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked="true" />
                            <div style='margin-left: 140px'>
                                <asp:CheckBox runat="server" Text="起飞前2小时内可出票" ID="chkPrintBeforeTwoHours" /></div>
                        </td>
                        <td id="neibufanyong" runat="server">
                            <asp:TextBox runat="server" CssClass="text_short text" ID="txtInternalCommission"></asp:TextBox>%
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="text_short text" ID="txtSubordinateCommission"></asp:TextBox>%
                        </td>
                        <td id="tong" runat="server">
                            <asp:TextBox runat="server" CssClass="text_short text" ID="txtProfessionCommission"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="clearfix">
                                <div class="fl policy_check">
                                    <asp:CheckBox runat="server" Text="直接审核" ID="chkAuto" />
                                    <asp:CheckBox runat="server" Text="需换编码出票" ID="chkChangePNR" />
                                    <asp:CheckBox ID="chkddlc" runat="server" Text="适用多段联程" />
                                    <span class="obvious1" id="wangfantishi" runat="server" style="margin-left: 50px;">该价格为往返总价格（不含税费），价格不确定时可以为空</span>
                                    <span class="obvious1" id="zejiagetishi" runat="server" style="margin-left: 50px;">折扣以100为换算单位，如全价Y舱为100折</span>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <br />
    <div class="text-c">
        <asp:Button CssClass="btn class1" Text="复制政策发布" ID="btnCopy" runat="server" OnClick="btnCopy_Click" />
        <asp:Button CssClass="btn class1" Text="修 改" ID="btnModify" runat="server" OnClick="btnModify_Click" />
        <asp:Button CssClass="btn class1" Text="返 回" ID="btnReturn" runat="server" OnClick="btnReturn_Click" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policy_edit_vaildate.js?20130417" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policy_bargian_edit_vaildate.js?20130416" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#txtDepartrueStart").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", doubleCalendar: "true", onpicked: function () { $("#ddlBunks").html("<option> </option>"); $("#hidBunks").val(""); }, maxDate: '#F{$dp.$D(\'txtDepartrueEnd\')||\'2020-10-01\'}' });
        });
        $("#txtDepartrueEnd").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", doubleCalendar: "true", onpicked: function () { $("#ddlBunks").html("<option> </option>"); $("#hidBunks").val(""); }, minDate: '#F{$dp.$D(\'txtDepartrueStart\')}', maxDate: '2020-10-01' });
        });
        $("#txtProvideDate").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", doubleCalendar: "true", minDate: '1900-10-01', onpicked: function () { $("#ddlBunks").html("<option> </option>"); $("#hidBunks").val(""); }, maxDate: '#F{$dp.$D(\'txtDepartrueStart\')}' });
        });
        $("#btnModify").click(function () {
            var flag = false;
            if (publicVaild()) {
                if (vaildate()) {
                    flag = vaildate_bunks_time();
                }
            }
            if ($("#txtDrawerCondition").val().length > 200) {
                alert("出票条件不能超过200个字！");
                $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
                return false;
            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }
            if ($("#hidBunks").val() != "") {
                $("#hidBunks").val($("#ddlBunks").val());
                if ($("#hidBunks").val() == "") {
                    alert("舱位不能为空");
                    return false;
                }
            }
            return flag;
        });
        $("#btnCopy").click(function () {
            var flag = false;
            if (publicVaild()) {
                if (vaildate()) {
                    flag = vaildate_bunks_time();
                }
            }
            if ($("#txtDrawerCondition").val().length > 200) {
                alert("出票条件不能超过200个字！");
                $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
                return false;
            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }
            if ($("#hidBunks").val() != "") {
                $("#hidBunks").val($("#ddlBunks").val());
                if ($("#hidBunks").val() == "") {
                    alert("舱位不能为空");
                    return false;
                }
            }
            return flag;
        });

        $("#ddlAirline").change(function () {
            $("#ddlBunks").html("<option> </option>"); $("#hidBunks").val("");
        });
        $("#btnRef").click(function () {
            if (vaildate_bunks_time()) {
                GetBunks();
            }
        });
        GetBunks();

    });
    function GetBunks() {
        var airline = "";
        if ($("#ddlAirline").val() != null) {
            airline = $("#ddlAirline").val();
        }
        else {
            airline = $("#lblAirline").html();
        }
        var id = $("#titlePolicy").html();
        var type = "";
        if (id == "单程") {
            type = "OneWay";
        } else if (id == "往返") {
            type = "RoundTrip";
        } else if (id == "中转联程") {
            type = "TransitWay";
        }
        //查询舱位
        var param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "voyageType": type };
        sendPostRequest("/PolicyHandlers/PolicyManager.ashx/QueryBargainBunksPolicy", JSON.stringify(param), function (e) {
            var str = "";
            $.each(eval(e), function (i, item) {
                if (i == 0 && $("#hidBunks").val() == "") {
                    $("#hidBunks").val(item);
                } if ($.trim($("#hidBunks").val()) == $.trim(item)) {
                    str += "<option  value='" + item + "' selected='selected' >" + item + "</option>";
                } else {
                    str += "<option  value='" + item + "' >" + item + "</option>";
                }
            });
            if (str == "") {
                alert("当前舱位为空,请重新选择航空公司或时间段!");
                return;
            }
            $("#ddlBunks").html("");
            $("#ddlBunks").append(str);
        }, function (e) {
            alert(JSON.parse(e.responseText));
        });
    }
    function selPriceOrDiscount(parame) {
        if ($(parame).val() == "0") {
            $(parame).parent().find(".price0").css("display", "");
            $(parame).parent().find(".discount0").css("display", "none");
            $(parame).parent().find(".fanyong").css("display", "none");
        }
        if ($(parame).val() == "1") {
            $(parame).parent().find(".price0").css("display", "none");
            $(parame).parent().find(".discount0").css("display", "");
            $(parame).parent().find(".fanyong").css("display", "none");
        }
        if ($(parame).val() == "3") {
            $(parame).parent().find(".price0").css("display", "none");
            $(parame).parent().find(".discount0").css("display", "none");
            $(parame).parent().find(".fanyong").css("display", "inline-block");
        }
    }
</script>
