<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="team_policy_edit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.team_policy_edit" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="OneCity" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>团队政策修改</title>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
</head>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
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
        .fl
        {
            margin-left: 20px;
            line-height: 28px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        <span id="tip" runat="server">团队政策修改</span> —— <span id="titlePolicy" runat="server">
            单程</span></h3>
    <div class="form">
        <table>
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
                        <asp:Label ID="lblAirline" CssClass="fl" runat="server"></asp:Label>
                        <asp:Label ID="lblCustomerCode" runat="server" Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlAirline" CssClass="fl" runat="server">
                        </asp:DropDownList>
                        <span id="cutomeTh" runat="server" class="fl">自定义编号</span>
                        <asp:DropDownList ID="ddlCustomCode" CssClass="fl" runat="server">
                        </asp:DropDownList>
                        <span id="selOfficeTd">
                            <label class="fl">
                                OFFICE号</label>
                            <asp:DropDownList ID="dropOffice" CssClass="fl" runat="server">
                            </asp:DropDownList>
                            <span class="obvious2 fl">需授权</span>
                            <asp:HiddenField runat="server" ID="hidOfficeNo" />
                        </span>
                    </td>
                </tr>
                <tr>
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
                <tr id="zhongzhuanTh" runat="server" visible="False">
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
                <tr>
                    <td class="title">
                        排除航段
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text text_width" ID="txtExceptAirways"></asp:TextBox>
                    </td>
                    <td>
                        <p class="obvious1" id="paichutishi" runat="server">
                            提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）；支持多城市混合输入，如KMG/SHACTU/KMGPEKXIY这样的航段均会被排除
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span runat="server" id="quchengFlight">航班限制</span>
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
                <tr id="returnFilghtDates" runat="server" class="class_display">
                    <td class="title">
                        <span runat="server" id="huichengFlight">回程航班</span>
                    </td>
                    <td colspan="2">
                        <asp:RadioButton ID="radReturnBuXian" runat="server" Text="不限" GroupName="ReturnFilght"
                            Checked="true" />
                        <asp:RadioButton ID="radReturnYiXia" runat="server" Text="仅适用以下航班" GroupName="ReturnFilght" />
                        <asp:RadioButton ID="radReturnBuYiXia" runat="server" Text="不适用以下航班" GroupName="ReturnFilght" />
                        <br />
                        <asp:TextBox runat="server" CssClass="text text_width" ID="txtReturnFilght"></asp:TextBox>
                    </td>
                    <td>
                        <p class="obvious1">
                            提示： 不加航空公司二字码，在输入框中输入限定的航班号，支持 * 匹配（如580*即支持5801到5809）。多航班请用“/ ”分隔，如：4590/580*
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        出票条件
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text textarea_width" ID="txtDrawerCondition"
                            TextMode="MultiLine" MaxLength="200">
                        </asp:TextBox>
                    </td>
                    <td class="obvious1">
                        提示：如果有特别要说明的出票条件，请填写，若没有，则不用填。本内容会展示给买家，请谨慎填写
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
                        提示：本内容只有卖家才能查看，主要是提示出票员核对政策使用规定。避免犯错，提高操作标准。若没有，则不用填
                    </td>
                </tr>
            </tbody>
        </table>
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
            <div class="pd_right groupLine">
                航班排除日期：
                <asp:TextBox runat="server" CssClass="text text_width" ID="txtPaiChu"></asp:TextBox>
                <p class="obvious1 pd">
                    请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开
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
            <br />
            <div class="pd_left zhidingcangwei">
                舱位指定：<input type='radio' value='0' class='zhidingcang' id='zhiding' runat="server"
                    name='zhiding' /><label for='zhiding'>指定团队舱位</label><input type='radio' value='1'
                        id='buzhiding' runat="server" name='zhiding' class='zhidingcang' /><label for='buzhiding'>普通舱位</label>
            </div>
        </div>
        <div class="groupBox2 clearfix">
            <table>
                <tbody>
                    <tr>
                        <th>
                            舱位
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
                        <th id="tonghangTh" runat="server">
                            同行佣金
                        </th>
                    </tr>
                    <tr>
                        <td class="BunksRad">
                            <div id="BunksChooice" runat="server">
                                <input type="radio" id="radAll" name="choice" value="0" /><label for="radAll">全选</label>
                                <input type="radio" id="radNot" name="choice" value="1" /><label for="radNot">反选</label>
                            </div>
                            <div id="Bunks" runat="server">
                                <label class='refBtnBunks btn class3'>
                                    点击获取舱位</label>
                            </div>
                            <div id="ZhidingBunks" runat="server">
                                <label class='refBtnBunks btn class3'>
                                    点击获取舱位</label>
                            </div>
                        </td>
                        <td>
                            <asp:HiddenField ID="hidBunks" runat="server" />
                            <asp:HiddenField ID="hidZiding" runat="server" />
                            <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked="true" />
                            <div style='margin-left:140px'><asp:CheckBox runat="server" Text="起飞前2小时内可出票" ID="chkPrintBeforeTwoHours" /></div>
                        </td>
                        <td id="neibufanyong" runat="server">
                            <asp:TextBox runat="server" CssClass="text text_short" ID="txtInternalCommission"></asp:TextBox>%
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="text text_short" ID="txtSubordinateCommission"></asp:TextBox>%
                        </td>
                        <td id="tonghang" runat="server">
                            <asp:TextBox runat="server" CssClass="text text_short" ID="txtProfessionCommission"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="clearfix">
                            <div class="fl">
                                <asp:CheckBox runat="server" Text="直接审核" ID="chkAuto" />
                            </div>
                            <div class="fl">
                                <asp:CheckBox runat="server" Text="需换编码出票" ID="chkChangePNR" /></div>
                            <div id="wangfan" runat="server" class="class_display fl">
                                <asp:CheckBox ID="chkRound" runat="server" Text="适用于往返降舱政策" /></div>
                            <div id="ddlc" runat="server" class="class_display fl">
                                <asp:CheckBox ID="chkddlc" runat="server" Text="适用多段联程" /></div>
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
        <asp:Button CssClass="btn class1" Text="修       改" ID="btnModify" runat="server"
            OnClick="btnModify_Click" />
        <asp:Button CssClass="btn class2" Text="返       回" ID="btnReturn" runat="server"
            OnClick="btnReturn_Click" />
    </div>
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_edit_vaildate.js?20130417" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_team_edit_vaildate.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //给文本框绑定日期控件
            $("#txtDepartrueStart").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks'>点击获取舱位</label>"); $("#ddlBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", maxDate: '#F{$dp.$D(\'txtDepartrueEnd\')||\'2020-10-01\'}' });
            });
            $("#txtDepartrueEnd").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks'>点击获取舱位</label>"); $("#ddlBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", minDate: '#F{$dp.$D(\'txtDepartrueStart\')}', maxDate: '2020-10-01' });
            });
            $("#txtProvideDate").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", minDate: '1900-10-01', onpicked: function () { $("#Bunks").html("<label class='refBtnBunks'>点击获取舱位</label>"); $("#ddlBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", maxDate: '#F{$dp.$D(\'txtDepartrueStart\')}' });
            });
            $("input[name='ReturnDate']").click(function () {
                if ($(this).val() == "radReturnData") {
                    $("#ReturnDataTextTip").css("display", "");
                    $("#ReturnDataText").css("display", "");
                    $("#ReturnDataWeekTip").css("display", "none");
                    $("#ReturnDataWeek").css("display", "none");
                }
                if ($(this).val() == "radReturnWeek") {
                    $("#ReturnDataWeekTip").css("display", "");
                    $("#ReturnDataWeek").css("display", "");
                    $("#ReturnDataTextTip").css("display", "none");
                    $("#ReturnDataText").css("display", "none");
                }
            });
            $("input[name='DepartureDate']").click(function () {
                if ($(this).val() == "radDepartureDate") {
                    $("#DepartureTextTip").css("display", "");
                    $("#DepartureText").css("display", "");
                    $("#DepartureWeekTip").css("display", "none");
                    $("#DepartureWeek").css("display", "none");
                }
                if ($(this).val() == "radDepartureWeek") {
                    $("#DepartureWeekTip").css("display", "");
                    $("#DepartureWeek").css("display", "");
                    $("#DepartureTextTip").css("display", "none");
                    $("#DepartureText").css("display", "none");
                }
            });
            $("input[type='radio'][name='choice']").click(function () {
                if ($(this).val() == "0") {
                    for (var i = 0; i < $("#Bunks input[type='checkbox']").length; i++) {
                        $("#Bunks input[type='checkbox']").eq(i).attr("checked", "checked");
                    }
                }
                if ($(this).val() == "1") {
                    for (var i = 0; i < $("#Bunks input[type='checkbox']").length; i++) {
                        if ($("#Bunks input[type='checkbox']").eq(i).is(":checked")) {
                            $("#Bunks input[type='checkbox']").eq(i).removeAttr("checked");
                        } else {
                            $("#Bunks input[type='checkbox']").eq(i).attr("checked", "checked");
                        }

                    }
                }

                var str = "";
                for (var i = 0; i < $("#Bunks input[type='checkbox']:checked").length; i++) {
                    if (i > 0) {
                        str += ",";
                    }
                    str = (str + $("#Bunks input[type='checkbox']:checked").eq(i).val());
                }
                $("#hidBunks").val(str);
            });
            $("#Bunks input[type='checkbox']").live("click", function () {
                var str = "";
                for (var i = 0; i < $("#Bunks input[type='checkbox']:checked").length; i++) {
                    if (i > 0) {
                        str += ",";
                    }
                    str = (str + $("#Bunks input[type='checkbox']:checked").eq(i).val());
                }
                $("#hidBunks").val(str);
            });
            $("#btnModify").click(function () {
                var flag = false;
                if ($("input[type='radio'][name='zhiding']:checked").val() == "0") {
                    $("#hidBunks").val($("#ddlBunks option:selected").val());
                }
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
                return flag;
            });
            $("#btnCopy").click(function () {
                var flag = false;
                if ($("input[type='radio'][name='zhiding']:checked").val() == "0") {
                    $("#hidBunks").val($("#ddlBunks option:selected").val());
                }
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
                return flag;
            });
            $("#btnRef").click(function () {
                if (vaildate_bunks_time()) {
                    GetBunks();
                }
            });
            $("input[name='radAirportPairType']").click(function () {
                if ($(this).val() == "radOneWay") {
                    $(".class_display").css("display", "none");
                }
                else {
                    $(".class_display").css("display", "");
                }
            });
            $("#ddlAirline").change(function () {
                $("#Bunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>"); $("#ddlBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>"); $("#hidBunks").val("");
            });
            $("input[type='radio'][name='zhiding']").click(function () {
                if ($("#zhiding").is(":checked")) {
                    $("#BunksChooice").hide();
                    $("#Bunks").hide();
                    $("#ZhidingBunks").show();
                } else {
                    $("#BunksChooice").show();
                    $("#Bunks").show();
                    $("#ZhidingBunks").hide();
                }
            });
            if ($("#zhiding").is(":checked")) {
                $("#BunksChooice").hide();
                $("#Bunks").hide();
                $("#ZhidingBunks").show(); GetBunks();
            } else {
                $("#BunksChooice").show();
                $("#Bunks").show();
                $("#ZhidingBunks").hide();
            }
        });
        function GetBunks() {
            var airline = "";
            if ($("#ddlAirline").val() != null) {
                airline = $("#ddlAirline").val();
            }
            else {
                airline = $("#lblAirline").html();
            }
            var voyage = $.trim($("#titlePolicy").html());

            var value = $("input[type='radio'][name='zhiding']:checked").val();
            var url; var param;
            //查询舱位
            if (value == "0") {
                if (voyage == "单程") {
                    voyage = "OneWay";
                } else if (voyage == "往返") {
                    voyage = "RoundTrip";
                } else if (voyage == "中转联程") {
                    voyage = "TransitWay";
                }
                param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "voyage": voyage };
                url = "/PolicyHandlers/PolicyManager.ashx/QueryTeamBunksPolicy";
            }
            else {
                if (voyage == "单程") {
                    voyage = "OneWay";
                } else if (voyage == "往返") {
                    voyage = "RoundTrip";
                } else if (voyage == "中转联程") {
                    voyage = "OneWayOrRound";
                }
                param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "voyage": voyage };
                url = "/PolicyHandlers/PolicyManager.ashx/QueryTeamNormalBunksPolicy";
            }
            sendPostRequest(url, JSON.stringify(param), function (e) {
                if (value == "0") {
                    var str = "<select id='ddlBunks' style='width: 50px;'>";
                    str = "<select id='ddlBunks' style='width: 50px;'>";
                    $.each(eval(e), function (i, item) {
                        if (i == 0 && $("#hidBunks").val() == "") {
                            $("#hidBunks").val(item);
                        } if ($.trim($("#hidBunks").val()) == $.trim(item)) {
                            str += "<option  value='" + item + "' selected='selected' >" + item + "</option>";
                        } else {
                            str += "<option  value='" + item + "' >" + item + "</option>";
                        }
                    });
                    str += " </select>";
                    $("#ZhidingBunks").html(str);
                    $("#hidBunks").val("");
                } else {
                    var str = "";
                    $.each(eval(e), function (i, item) {
                        str += "<input type='checkbox' value='" + item + "'/>" + item;
                        if ((i + 1) % 4 == 0 && i > 0) {
                            str += "<br />";
                        }
                    });
                    $("#Bunks").html(str);
                    $("#hidBunks").val("");
                }

            }, function (e) {
            });
        }
    </script>
</body>
</html>
