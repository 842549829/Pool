<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="notch_policy_edit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.notch_policy_edit" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>缺口政策修改</title>
</head>
<link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/airport.js" type="text/javascript"></script>
<style type="text/css">
    .text_width, .text
    {
        width: 310px;
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
    }.notch_block_btn
        {
            background-color: #D9D9D9;
            border-radius: 3px;
            color: #333;
            display:inline-block;
            height: 30px;
            line-height: 30px;
            margin: 0 10px;
            text-align: center;
            width: 200px;
        }
        .notch_block_btn
        {
            *display:inline;
        }
        .notch_block_btn:hover
        {
            text-decoration:none;
        }
        .showTxt
        {
            margin: 5px 10px;
        }
        .notch_air
        {
            border-top:1px dashed #a3a3a3;
            border-bottom:1px dashed #a3a3a3;
            padding: 10px 0;
        }
        .notch_air label
        {
            float:left;
            line-height: 1.8;
            padding-right: 5px;
            text-align:right;
            width:65px;
        }
        .notch_air p
        {
            line-height: 1.8;
            margin-left:70px;
        }
        .notch_air .clearfix
        {
            margin-bottom: 10px;
        }
</style>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            <span id="tip" runat="server">缺口政策修改</span></h3>
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
                            <asp:Label ID="lblCustomerCode" CssClass="fl" runat="server" Visible="false"></asp:Label>
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
                        <td class="title">
                            航段
                        </td>
                        <td colspan="3">
                            <a href="javascript:tianxiehanduan(0);" class="notch_block_btn">填写适用航段</a> <a href="javascript:tianxiehanduan(1);"
                                class="notch_block_btn">填写排除航段</a>
                            <div class="showTxt" runat="server" id="showTxt">
                            </div>
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
                        请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开。
                    </p>
                </div>
                <div class="pd_left shiyongbanqi">
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
            </div>
            <div class="groupBox2">
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
                                <input type="radio" id="radAll" name="choice" value="0" /><label for="radAll">全选</label>
                                <input type="radio" id="radNot" name="choice" value="1" /><label for="radNot">反选</label>
                                <br />
                                <div id="Bunks" runat="server">
                                </div>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidBunks" runat="server" />
                                <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked="true" />
                                <div style='margin-left: 140px'>
                                    <asp:CheckBox runat="server" Text="起飞前2小时内可出票" ID="chkPrintBeforeTwoHours" /></div>
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
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="inputTxtvalue" runat="server" />
    <br />
    <div class="text-c">
        <asp:Button CssClass="btn class1" Text="复制政策发布" ID="btnCopy" runat="server" OnClick="btnCopy_Click" />
        <asp:Button CssClass="btn class1" Text="修       改" ID="btnModify" runat="server"
            OnClick="btnModify_Click" />
        <asp:Button CssClass="btn class2" Text="返       回" ID="btnReturn" runat="server"
            OnClick="btnReturn_Click" />
    </div>
    <a id="divHangduan" style="display: none;" data="{type:'pop',id:'hangduanxinxi'}">
    </a>
    <div id="hangduanxinxi" class="form layer3" style="display: none; width: 800px;">
        <div class='hd'>
            <h4>
                <span class="tip">设置缺口程政策所包含的航段信息</span> <a class='close' href="javascript:void(0)">
                    关闭</a></h4>
            <div class="layer3Tips">
                只有编码中包含/排除您设置的航段，您的政策才会展示给采购并进行选择操作。</div>
        </div>
        <table>
            <tr class="inputTxt" style="display: none;">
                <td>
                    <div class="fl" style="width: 350px; border-right: 1px dashed #c1c1c1;">
                        <h4 style="text-align: center;">
                            始发地</h4>
                        <uc:City ID="txtDepartureAirports" runat="server" />
                    </div>
                </td>
                <td>
                    <div class="fl">
                        <h4 style="text-align: center;">
                            目的地</h4>
                        <uc:City ID="txtArrivalAirports" runat="server" />
                    </div>
                </td>
            </tr>
            <tr class="inputTxt" style="display: none;">
                <td colspan="2" class="txt-c">
                    <input type="button" value="确认" class="btn class1 btnQueren" />
                    <input type="button" value="取消" class="btn class2 btnQuexiao close" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_notch_edit.js?1213123" type="text/javascript"></script>
    <script type="text/javascript">
        function tianxiehanduan(p) {
            $(".inputTxt").show();
            if (p == 0) {
                $(".tip").html("设置缺口程政策所包含的航段信息");
            } else {
                $(".tip").html("设置缺口程政策所排除的航段信息");
            }
            $("#divHangduan").click(); $("#hangduanxinxi").css("top", "100px");
        }

        $("#dropOffice").change(function () {
            if ($.trim($("#dropOffice option:selected").val()) == "") {
                $("#selOfficeTd span").hide();
            } else {
                $("#selOfficeTd span").show();
            }
            if ($("#dropOffice option:selected").val() == "True") {
                $("#selOfficeTd span").html("需授权").removeClass("obvious3").addClass("obvious2");
            } else {
                $("#selOfficeTd span").html("无需授权").removeClass("obvious2").addClass("obvious3");
            }
            $("#hidOfficeNo").val($("#dropOffice option:selected").text());
        });

        $(function () {
            $("#dropOffice").change();
            $(".inputTxt .btnQuexiao").click(function () {
                $(".inputTxt").hide();
            });
            $(".inputTxt .btnQueren").click(function () {
                var kaishi = $("#txtDepartureAirports_txtAirports").val().toUpperCase();
                var jiesu = $("#txtArrivalAirports_txtAirports").val().toUpperCase();
                if (kaishi == "") {
                    alert("出发地不能为空！");
                    return;
                }
                if (jiesu == "") {
                    alert("到达地不能为空！");
                    return;
                }

                var val = $("#inputTxtvalue").val();
                var tipText = "";
                var valTxt = "";
                if ($(".tip").html() == "设置缺口程政策所包含的航段信息") {
                    tipText = "适用航段 ";
                    valTxt = 1;
                } else {
                    tipText = "排除航段 ";
                    valTxt = 0;
                }
                var curr = valTxt + "|" + kaishi + "|" + jiesu;
                var le = val.split(',');
                var f = false;
                for (var l = 0; l < le.length; l++) {
                    if ($.trim(le[l]) == curr) {
                        f = true;
                        break;
                    }
                }
                if (f) {
                    alert("当前 " + tipText + " 存在相同的航段，请勿重复添加！");
                    return;
                }
                if (val == "") {
                    $("#inputTxtvalue").val(curr);
                } else {
                    $("#inputTxtvalue").val(val + "," + curr);
                }
                $(".showTxt").html($(".showTxt").html() + "<div class='notch_air'><div class='clearfix'><label>" + tipText + "</label><p class='text-auto'>" + kaishi + "</p></div><div class='clearfix'><label>至：</label><p class='text-auto'>" + jiesu + "</p></div><p><input type='button' value='删除' class='btn class2 btnShanchu' airports='" + valTxt + "|" + kaishi + "|" + jiesu + "' /></p></div>");
                $("#txtDepartureAirports_btnRemoveAll").click();
                $("#txtArrivalAirports_btnRemoveAll").click();
                $(".inputTxt").hide();

                $(".close").click();
            });
            $(".btnShanchu").live("click", function () {
                $(this).parent().parent().remove();
                var val = $("#inputTxtvalue").val();
                var values = val.split(',');
                var arry = "";
                for (var i = 0; i < values.length; i++) {
                    if (values[i] != $(this).attr("airports")) {
                        if (arry != "") {
                            arry += ",";
                        }
                        arry += values[i];
                    }
                }
                $("#inputTxtvalue").val(arry);

            });

            //给文本框绑定日期控件
            $("#txtDepartrueStart").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3 btnRefresh'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", maxDate: '#F{$dp.$D(\'txtDepartrueEnd\')||\'2020-10-01\'}' });
            });
            $("#txtDepartrueEnd").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3 btnRefresh'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", minDate: '#F{$dp.$D(\'txtDepartrueStart\')}', maxDate: '2020-10-01' });
            });
            $("#txtProvideDate").focus(function () {
                WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", minDate: '1900-10-01', onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3 btnRefresh'>点击获取舱位</label>"); $("#hidBunks").val(""); }, doubleCalendar: "true", maxDate: '#F{$dp.$D(\'txtDepartrueStart\')}' });
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
                return vailNotchPolicy();
            });
            $("#btnCopy").click(function () {
                return vailNotchPolicy();
            });
            $("#btnRef").click(function () {
                if (vaildate_bunks_time()) {
                    GetBunks();
                }
            });
            $("#ddlAirline").change(function () {
                $("#Bunks").html("<label class='refBtnBunks btn class3 btnRefresh'>点击获取舱位</label>");
                $("#hidBunks").val("");
            });
            $(".refBtnBunks").live("click", function () {
                if (vaildate_bunks_time()) {
                    GetBunks();
                }
            });
        });
        function vaildate_bunks_time() {
            var policyDepartureFilghtDataStart = $("#txtDepartrueStart").val();
            var policyDepartureFilghtDataEnd = $("#txtDepartrueEnd").val();

            var policyStartPrintDate = $("#txtProvideDate").val();

            if (policyDepartureFilghtDataStart == "" || policyDepartureFilghtDataEnd == "" || policyStartPrintDate == "") {
                alert("政策的 航班日期，开始出票日期不能为空!");
                return false;
            }
            if (valiateDateTime(policyDepartureFilghtDataStart, policyDepartureFilghtDataEnd)) {
                alert("政策的航班日期范围有误！结束时间不能小于开始时间");
                return false;
            }
            if (valiateDateTime(policyStartPrintDate, policyDepartureFilghtDataEnd)) {
                alert("政策的出票时间不能大于去程的结束时间!");
                return false;
            }
            return true;
        }
        function GetBunks() {
            var airline = "";
            if ($("#ddlAirline").val() != null) {
                airline = $("#ddlAirline").val();
            }
            else {
                airline = $("#lblAirline").html();
            }
            var policyDepartureFilghtDataStart = $("#txtDepartrueStart").val();
            var policyDepartureFilghtDataEnd = $("#txtDepartrueEnd").val();

            var policyStartPrintDate = $("#txtProvideDate").val();
            //查询舱位
            var param = { "airline": airline, "startTime": policyDepartureFilghtDataStart, "endTime": policyDepartureFilghtDataEnd, "startETDZDate": policyStartPrintDate };
            sendPostRequest("/PolicyHandlers/PolicyManager.ashx/QueryNotchBunksPolicy", JSON.stringify(param), function (e) {
                var str = " ";
                $.each(eval(e), function (i, item) {
                    str += "<input type='checkbox' value='" + item + "'/>" + item;
                    if ((i + 1) % 4 == 0 && i > 0) {
                        str += "<br />";
                    }
                });
                $("#Bunks").html(str);
                $("#hidBunks").val("");
            }, function (e) {
                if (e.status == 300) {
                    alert(JSON.parse(e.responseText));
                } else {
                    alert(e.statusText);
                }
            });
        }
    </script>
</body>
</html>
