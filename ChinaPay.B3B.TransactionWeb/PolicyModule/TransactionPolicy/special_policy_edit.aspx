<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="special_policy_edit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.special_policy_edit" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="OneCity" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特殊政策修改</title>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <style type="text/css">
        .text_width, .text
        {
            width: 300px;
        }
        .text_short
        {
            width: 73px;
        }
        .fl_th
        {
            margin-left: 20px;
            line-height: 28px;
        }
        .textarea_width
        {
            height: 60px;
            width: 98%;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            <span id="tip" runat="server">特殊政策修改</span> —— <span id="titlePolicy" runat="server">
                单程控位</span></h3>
        <asp:HiddenField runat="server" ID="specialType" />
        <table class="tab-mid">
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tbody>
                <tr style="height: 60px;">
                    <td colspan="4" class="obvious1" id="miaopiao">
                        <div id="specialtypeSpan" runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        航空公司
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblAirline" runat="server" CssClass="fl"></asp:Label>
                        <asp:Label ID="lblCustomerCode" runat="server" Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlAirline" runat="server" CssClass="fl fl_th">
                        </asp:DropDownList>
                        <span id="cutomeTh" runat="server" class="fl fl_th">自定义编号</span>
                        <asp:DropDownList ID="ddlCustomCode" CssClass="fl fl_th" runat="server">
                        </asp:DropDownList>
                        <span id="selOfficeTd" runat="server">
                            <label class="fl fl_th">
                                OFFICE号</label>
                            <asp:DropDownList ID="dropOffice" CssClass="fl fl_th" runat="server">
                            </asp:DropDownList>
                            <span class="obvious2 fl">需授权</span>
                            <asp:HiddenField runat="server" ID="hidOfficeNo" />
                        </span>
                    </td>
                </tr>
                <tr id="shifa" runat="server">
                    <td class="title" style="vertical-align: top; min-width: 50px;">
                        始发地
                    </td>
                    <td>
                        <uc:City ID="txtShifaAirports" runat="server" />
                        <br />
                    </td>
                    <td class="title" style="display: block; height: 317px; vertical-align: top; min-width: 50px;
                        position: relative;">
                        <label runat="server" id="zhongzhuan">
                            目的地</label>
                        <br />
                        <a href='javascript:;' id='duihuan' runat="server" class='policyBtn2'>←调换→</a>
                    </td>
                    <td>
                        <uc:City ID="txtZhongzhuanAirports" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr id="danchengThShifa" runat="server" class="diaohuan">
                    <td class="title">
                        始发地
                    </td>
                    <td colspan="3" style="display: block; position: relative;">
                        <uc:OneCity ID="txtDepartureAirports" runat="server" />
                        <%--<label class="policyBtn3" id="diaohuanshifa" runat="server">
                            调换始发地和目的地</label>--%>
                        <label class="policyBtn1" runat="server" id="danchengduihuan">
                            ↓调换↑</label>
                    </td>
                </tr>
                <tr id="danchengMudifa" runat="server">
                    <td class="title">
                        目的地
                    </td>
                    <td colspan="3">
                        <uc:OneCity ID="txtArrivalAirports" runat="server" />
                    </td>
                </tr>
                <%-- <tr id="wangfanThShifa" runat="server" class="diaohuan">
                    <td class="title">
                        目的地
                    </td>
                    <td colspan="3">
                        <uc:AirportCode ID="txtMuDi" runat="server" />
                        <br />
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <p class="obvious1">
                            <br />
                            温馨提示： 支持多选，可用Shift连选，也支持Ctrl间隔选择。 可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。 输入三字码时，如果有多个，请用“/”分隔，如：CTU/PEK
                        </p>
                    </td>
                </tr>
                <tr id="paichu" runat="server" visible="false">
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
                <tr id="dancheng" runat="server">
                    <td class="title">
                        航班限制
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
                <tr id="dropDrawerConditionTh" runat="server">
                    <td class="title">
                        出票条件
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="dropDrawerCondition" runat="server" Style="width: 500px;">
                            <asp:ListItem>--请选择--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="obvious1">
                        提示：请仔细选择出票的条件
                    </td>
                </tr>
                <tr id="txtDrawerConditionTh" runat="server">
                    <td class="title">
                        出票条件
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" CssClass="text textarea_width" ID="txtDrawerCondition"
                            MaxLength="200" TextMode="MultiLine">
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
    <asp:HiddenField runat="server" ID="hidShifa" />
    <div class="groupCt">
        <div class="groupBox1">
        <div class="clearfix">
            <div class="fl pd_right groupLine" style="margin-left: 0px;">
                去程航班日期：
                <asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtDepartrueStart"></asp:TextBox>
                <span class="fl-l">至</span>
                <asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtDepartrueEnd"></asp:TextBox>
            </div>
            <div class="fl pd_right groupLine">
                开始出票日期：<asp:TextBox runat="server" CssClass="datepicker datefrom btn class3" ID="txtProvideDate"></asp:TextBox>
            </div>
            <div class="fl groupLine">
                <input type="button" value="刷新舱位" id="btnRef" runat="server" class="btn class2" />
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
                    周日</label>
            </div>
            <div class="pd_left groupLine" id="tiqianTh" runat="server">
                提前天数：
                <asp:TextBox runat="server" CssClass="text text_short" ID="txtTiQianDays"></asp:TextBox>天<span
                    class="obvious1">&nbsp; 提示：如果有提前订票限制，请输入限制天数 </span>
            </div>
            <div class="pd_left groupLine" id="chanpinBunks" runat="server">
                产品舱位：
                <asp:TextBox runat="server" CssClass="text text_short" ID="txtBunks" MaxLength="2"></asp:TextBox><span
                    class="obvious1">&nbsp; 提示：请输入舱位 </span>
            </div>
            <div class='pd_left groupLine' id="cangweiTh" runat="server">
                舱位选择：<input type='radio' id='normal' name='chooiceBunks' value="0" runat="server" /><label
                    for='normal'>普通舱位</label>
                <input type='radio' id='bargain' name='chooiceBunks' value="1" runat="server" /><label
                    for='bargain'>特价舱位</label>
            </div>
            <div class="pd_left groupLine heipingtongbu" runat="server" id="hptbTh">
                黑屏同步：
                <div class="pd_left groupLine">
                    <input id="hptb" type="radio" class='hptb1' value="0" name="tongbu" runat="server" /><label
                        for="hptb">同&nbsp;&nbsp; 步</label><span class="BunksRad hptb shuliangBunks" runat="server"
                            id="selBunksSpan">
                            <label class='refBtnBunks btn class3'>
                                点击获取舱位</label>
                        </span><span class="obvious1">提示：选择同步后系统将在用户导入编码或查询时自动查询剩余机票张数 </span>
                </div>
                <div class="pd_left groupLine">
                    <input id="bhptb" type="radio" class='hptb1' value="1" name="tongbu" runat="server" /><label
                        for="bhptb">不同步</label>
                    <span class="obvious1">提示：如果您能确保为购买该政策机票的客户出票，则选择不同步 </span>
                    <div class='pd_left groupLine shuliangBunks bhptb' id="youweichupiao" runat="server">
                        <input id='youwei' type='radio' value='0' name='youwei' runat="server" /><label for='youwei'>有位出票</label>
                        <span class='obvious1'>提示：须保证编码状态为OK </span>
                        <br />
                        <input id='wuwei' type='radio' value='1' name='youwei' runat="server" /><label for='wuwei'>无位出票</label>
                        <span class='obvious1'>提示：即候补状态可以保证出票 </span>
                    </div>
                </div>
            </div>
            <%--<div class="fl pd_left groupLine chanpinPrice" style="margin-left: 0px;" id="priceDiv"
                runat="server">
                产品价格：
                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtPrice0"></asp:TextBox>
                元
            </div>--%>
            <div class="pd_left groupLine shuliang shuliangBunks bhptb" style="margin-left: 0px;"
                id="amountDiv" runat="server">
                提供产品数量：
                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtResourceAmount"></asp:TextBox>
                张&nbsp;&nbsp;&nbsp;&nbsp; <span class="obvious1 tishi" id="tishi" runat="server">提示：请输入1-9的纯数字
                </span>
            </div>
            <br />
            <div class="clearfix">
            <div class='fl pd_left groupLine' id="neibuPrice" runat="server">
                内部结 算 价 ：
                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="5" ID="txtPriceNeibu"></asp:TextBox>
                元
            </div>
            <div class='fl pd_left groupLine' id="xiajiPrice" runat="server">
                下级结 算 价 ：
                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="5" ID="txtPriceXiaji"></asp:TextBox>
                元
            </div>
            <div class='fl pd_left groupLine' id="tonghangPrice" runat="server">
                同行结 算 价 ：
                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="5" ID="txtPriceTonghang"></asp:TextBox>
                元 <span class='obvious1 chanpinPrice'>&nbsp;&nbsp;&nbsp;&nbsp;提示：请输入整数的价格 </span>
            </div>
            </div>
            <asp:HiddenField runat="server" ID="hidBunks" />
        </div>
        <div class="groupBox2" runat="server" id="tableDiv">
            <table>
                <tbody>
                    <tr>
                        <th>
                            舱位
                        </th>
                        <th id="discountTh" runat="server">
                            票面价格
                        </th>
                        <th id="kepiaoTh" runat="server">
                            客票类型
                        </th>
                        <th id="neibuTh" runat="server">
                            内部结算价
                        </th>
                        <th>
                            下级结算价
                        </th>
                        <th id="tongTh" runat="server">
                            同行结算价
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div class="BunksRad" id="Bunks" runat="server">
                                <label class='refBtnBunks btn class3'>
                                    点击获取舱位</label></div>
                            <div class="BunksBargain" id="BunksBargain" style='display: none;'>
                                <label class='refBtnBunks btn class3'>
                                    点击获取舱位</label></div>
                        </td>
                        <td class="priceORdiscount" id="discount" runat="server">
                            <select onchange="selPriceOrDiscount(this)" runat="server" id="selPrice">
                                <option value="0">按价格发布</option>
                                <option value="1">按直减发布</option>
                            </select>
                            <span class="price0" runat="server" id="price0">
                                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtPrice"></asp:TextBox>元</span>
                            <span class="discount0" style="display: none;" runat="server" id="discount0">
                                <asp:TextBox runat="server" CssClass="text_short text" MaxLength="4" ID="txtDiscount"></asp:TextBox>%</span>
                        </td>
                        <td id="kepiao" runat="server">
                            <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked="true" />
                            <div style='margin-left: 140px'>
                                <asp:CheckBox runat="server" Text="起飞前2小时内可出票" ID="chkPrintBeforeTwoHours" /></div>
                        </td>
                        <td id="neibufanyong" runat="server">
                            <span class="zhijianSpan" style='visibility: hidden;'>直减</span><asp:TextBox runat="server"
                                CssClass="text_short text" ID="txtInternalCommission"></asp:TextBox><span class="priceOrSub">元</span>
                        </td>
                        <td>
                            <span class="zhijianSpan" style='visibility: hidden;'>直减</span><asp:TextBox runat="server"
                                CssClass="text_short text" ID="txtSubordinateCommission"></asp:TextBox><span class="priceOrSub">元</span>
                        </td>
                        <td id="tong" runat="server">
                            <span class="zhijianSpan" style='visibility: hidden;'>直减</span><asp:TextBox runat="server"
                                CssClass="text_short text" ID="txtProfessionCommission"></asp:TextBox><span class="priceOrSub">元</span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="policy_check">
            <asp:CheckBox runat="server" Text="直接审核" ID="chkAuto" />
            <asp:CheckBox runat="server" Text="采购时需要确认座位" ID="chkConfirmResource" />
            <br />
            <asp:CheckBox runat="server" ID="chkdjbc" CssClass="djbcChk" Text="价格限制 " />
            <span id="djbcTxt" runat="server">票面价区间<asp:TextBox ID="txtdjbc" runat="server" CssClass='text text_short'
                MaxLength='5'></asp:TextBox><b style="color:Red;"> * </b>元(包含)至<asp:TextBox ID="txtdj" runat="server" CssClass='text text_short' MaxLength='5'></asp:TextBox>元(包含)</span> 
                <br /><span id="djbcTip" class="obvious1" runat="server" style="margin: 50px;">您可以设置您所愿意出票的票面价区间；后面一个输入框留空则表示不限票面价；如1000-留空表示仅出票面价1000以上的航线</span>
            <br />
            <span class="obvious1" id="jiagetishi" runat="server" style="margin: 50px;">票面价格就是行程单上的真实价格；直减发布就是在您选择的舱位所默认的折扣上直接减少您设置的百分比</span>
        </div>
        <br />
        <br />
    </div>
    <div class="importantBox" id="importantBox" runat="server">
        <p class="imTips">
            温馨提示：您所修改/复制成功之后的政策需要平台审核后可见，请您耐心等待审核；您可以通过<asp:Label runat="server" ID="lblServicePhone"></asp:Label>联系客服进行审核事宜查询及催办，感谢您对我们平台的支持，谢谢！</p>
    </div>
    <div class="text-c">
        <asp:Button CssClass="btn class1" Text="复制政策发布" ID="btnCopy" runat="server" OnClick="btnCopy_Click" />
        <asp:Button CssClass="btn class1" Text="修 改" ID="btnModify" runat="server" OnClick="btnModify_Click" />
        <asp:Button CssClass="btn class1" Text="返 回" ID="btnReturn" runat="server" OnClick="btnReturn_Click" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policy_edit_vaildate.js?20130417" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#chkdjbc").click(function () {
            if ($("#chkdjbc").is(":checked")) {
                $("#djbcTxt").css("display", "");
                $("#djdfTxt").css("display", "none");
                $("#chkdjdf").removeAttr("checked");
            } else {
                $("#djbcTxt").css("display", "none");
                $("#djdfTxt").css("display", "none");
            }
        });
        $("input[type='radio'][name='1radio']").live("click", function () {
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
        $("#duihuan").click(function () {
            var lbSourceText = $("#txtShifaAirports_txtAirports").val();
            $("#txtShifaAirports_txtAirports").val($("#txtZhongzhuanAirports_txtAirports").val());
            $("#txtZhongzhuanAirports_txtAirports").val(lbSourceText);
            $("#txtShifaAirports_txtAirports").blur();
            $("#txtZhongzhuanAirports_txtAirports").blur();
        });
        $("input[type='radio'][name='chooiceBunks']").click(function () {
            if ($(this).val() == "0") {
                $(".BunksRad").css("display", "");
                $("#selPrice").css("display", "");
                $(".BunksBargain").css("display", "none");
                if ($("#selPrice").val() == "0") {
                    $(".discount0").css("display", "none");
                    $(".price0").css("display", "");
                    $(".priceOrSub").html("元");
                    $(".zhijianSpan").css("display", "none");
                    $("#selPrice").parent().parent().find("input[type='text']").css("width", "73px");
                } else {
                    $(".discount0").css("display", "");
                    $(".price0").css("display", "none");
                    $(".priceOrSub").html("%");
                    $(".zhijianSpan").css("display", "");
                    $("#selPrice").parent().parent().find("input[type='text']").css("width", "49px");
                }
                var str = "";
                for (var i = 0; i < $("#Bunks input[type='checkbox']:checked").length; i++) {
                    if (i > 0) {
                        str += ",";
                    }
                    str = (str + $("#Bunks input[type='checkbox']:checked").eq(i).val());
                }
                $("#hidBunks").val(str);
            } else {
                $(".BunksRad").css("display", "none");
                $(".BunksBargain").css("display", "");
                $("#selPrice").css("display", "none");
                $(".discount0").css("display", "none");
                $(".price0").css("display", "");
                $(".priceOrSub").html("元");
                $(".zhijianSpan").css("display", "none");
                $("#selPrice").parent().parent().find("input[type='text']").css("width", "73px");
            }
        });
        $("#danchengduihuan").click(function () {
            var shifa = $("#txtDepartureAirports_txtAirport").val();
            var mudi = $("#txtArrivalAirports_txtAirport").val();
            $("#txtDepartureAirports_txtAirport").val(mudi);
            $("#txtArrivalAirports_txtAirport").val(shifa);
            $("#txtDepartureAirports_txtAirport").keyup();
            $("#txtArrivalAirports_txtAirport").keyup();
        });
        $(".hptb1").click(function () {
            if ($(this).val() == 0) {
                $(".hptb").show();
                $(".bhptb").hide();
                $("#btnRef").show();
                $("#chkConfirmResource").hide();
            } else {
                $(".hptb").hide();
                $(".bhptb").show();
                $("#btnRef").hide();
                $("#chkConfirmResource").show();
            }
        });
        $("#btnRef").click(function () {
            GetBunks();
        });
        $(".refBtnBunks").live("click", function () {
            GetBunks();
        });
        $("#txtDepartrueStart").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", doubleCalendar: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#BunksBargain").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#selBunksSpan").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#hidBunks").val(""); }, maxDate: '#F{$dp.$D(\'txtDepartrueEnd\')||\'2020-10-01\'}' });
        });
        $("#txtDepartrueEnd").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", doubleCalendar: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#BunksBargain").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#selBunksSpan").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#hidBunks").val(""); }, minDate: '#F{$dp.$D(\'txtDepartrueStart\')}', maxDate: '2020-10-01' });
        });
        $("#txtProvideDate").focus(function () {
            WdatePicker({ isShowClear: false, skin: 'default', readOnly: "true", minDate: '1900-10-01', doubleCalendar: "true", onpicked: function () { $("#Bunks").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#BunksBargain").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#selBunksSpan").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>"); $("#hidBunks").val(""); }, maxDate: '#F{$dp.$D(\'txtDepartrueStart\')}' });
        });
        $("#btnModify").click(function () {
            if ($("#txtDrawerCondition").size() != 0) {
                if ($("#txtDrawerCondition").val().length > 200) {
                    alert("出票条件不能超过200个字！");
                    $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
                    return false;
                }
            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }
            var id = $("#specialType").val();
            if (id == 2) {
                if ($("input[type='radio'][name='tongbu']:checked").val() == "0") {
                    $("#hidBunks").val($("#ddlBunks").val());
                    if ($.trim($("#hidBunks").val()) == "") {
                        alert("舱位不能为空，请选择一个舱位");
                        return false;
                    }
                }
            } else if (id == 3 || id == 4) {
                if ($("input[type='radio'][name='chooiceBunks']:checked").val() == "1") {
                    $("#hidBunks").val($("#ddlBunks").val());
                    if ($.trim($("#hidBunks").val()) == "") {
                        alert("舱位不能为空，请选择一个舱位");
                        return false;
                    }
                } else {
                    var str = "";
                    for (var i = 0; i < $("#Bunks input[type='checkbox']:checked").length; i++) {
                        if (i > 0) {
                            str += ",";
                        }
                        str = (str + $("#Bunks input[type='checkbox']:checked").eq(i).val());
                    }
                    $("#hidBunks").val(str);
                }
            } else if (id == 5) {
                $("#hidBunks").val($("#txtBunks").val());
            }
            var f = vaildate();
            if (f) {
                var reg = /^[0-9]{1,10}?$/;
                if ((id == 3 || id == 4) && $("#selPrice").val() == "0" && $("#txtPrice").val() == "") {
                    alert("价格不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4) && $("#selPrice").val() == "0" && !reg.test($("#txtPrice").val())) {
                    alert("价格不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && $("#txtDiscount").val() == "") {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && !reg.test($("#txtDiscount").val())) {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && parseInt($("#txtDiscount").val()) < 0) {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if (id == 3 && $("#chkdjbc").is(":checked")) {
                    if ($("#txtdjbc").val() == "") {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if (!reg.test($("#txtdjbc").val())) {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if (parseInt($("#txtdjbc").val()) < 0) {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if ($("#txtdj").val() != "") {
                        if (!reg.test($("#txtdj").val())) {
                            alert("票面价区间上限的价格格式错误。只能输入10位以内的整数!");
                            return false;
                        }
                        if (parseInt($("#txtdj").val()) < 0) {
                            alert("票面价区间上限的价格格式错误。只能输入10位以内的整数!");
                            return false;
                        }
                        if (parseInt($("#txtdj").val()) < parseInt($("#txtdjbc").val())) {
                            alert("票面价区间前面的价格不能大于后面的价格!");
                            return false;
                        }
                    }
                }
            }
            return f;
        });
        $("#btnCopy").click(function () {
            if ($("#txtDrawerCondition").size() != 0) {
                if ($("#txtDrawerCondition").val().length > 200) {
                    alert("出票条件不能超过200个字！");
                    $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
                    return false;
                }
            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }

            var id = $("#specialType").val();
            if (id == 2) {
                if ($("input[type='radio'][name='tongbu']:checked").val() == "0") {
                    $("#hidBunks").val($("#ddlBunks").val());
                    if ($.trim($("#hidBunks").val()) == "") {
                        alert("舱位不能为空，请选择一个舱位");
                        return false;
                    }
                }
            } else if (id == 3 || id == 4) {
                if ($("input[type='radio'][name='chooiceBunks']:checked").val() == "1") {
                    $("#hidBunks").val($("#ddlBunks").val());
                    if ($.trim($("#hidBunks").val()) == "") {
                        alert("舱位不能为空，请选择一个舱位");
                        return false;
                    }
                }
            } else if (id == 5) {
                $("#hidBunks").val($("#txtBunks").val());
            }

            var reg = /^[0-9]{1,10}?$/;
            var f = vaildate();
            if (f) {
                if ((id == 3 || id == 4) && $("#selPrice").val() == "0" && $("#txtPrice").val() == "") {
                    alert("价格不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4) && $("#selPrice").val() == "0" && !reg.test($("#txtPrice").val())) {
                    alert("价格不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && $("#txtDiscount").val() == "") {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && !reg.test($("#txtDiscount").val())) {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if ((id == 3 || id == 4 || id == 6) && $("#selPrice").val() == "1" && parseInt($("#txtDiscount").val()) < 0) {
                    alert("直减或返佣不能为空。只能输入整数!");
                    return false;
                }
                if (id == 3 && $("#chkdjbc").is(":checked")) {
                    if ($("#txtdjbc").val() == "") {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if (!reg.test($("#txtdjbc").val())) {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if (parseInt($("#txtdjbc").val()) < 0) {
                        alert("票面价区间下限的价格格式错误。只能输入10位以内的整数!");
                        return false;
                    }
                    if ($("#txtdj").val() != "") {
                        if (!reg.test($("#txtdj").val())) {
                            alert("票面价区间上限的价格格式错误。只能输入10位以内的整数!");
                            return false;
                        }
                        if (parseInt($("#txtdj").val()) < 0) {
                            alert("票面价区间上限的价格格式错误。只能输入10位以内的整数!");
                            return false;
                        }
                        if (parseInt($("#txtdj").val()) < parseInt($("#txtdjbc").val())) {
                            alert("票面价区间前面的价格不能大于后面的价格!");
                            return false;
                        }
                    }
                }
            }
            return f;
        });
        var id = $("#specialType").val();
        if (id == "2") {
            GetBunks();
        }
        if (id == "3" || id == "4") {
            if ($("input[type='radio'][name='chooiceBunks']:checked").val() == "1") {
                GetBunks();

                $(".BunksRad").css("display", "none");
                $(".BunksBargain").css("display", "");
                $("#selPrice").css("display", "none");
                $(".discount0").css("display", "none");
                $(".price0").css("display", "");
                $(".priceOrSub").html("元");
            }
        }
        if ($("#selPrice").val() == "0") {
            $(".priceOrSub").html("元");
            $(".zhijianSpan").css("visibility", "hidden");
        } else {
            $(".priceOrSub").html("%");
            if (id != "6") {
                $(".zhijianSpan").css("visibility", "");
            }
        }

        $("#ddlAirline").change(function () {
            $("#Bunks").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>");
            $("#BunksBargain").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>");
            $("#selBunksSpan").html("<label class='refBtnBunks btn class3'> 点击获取舱位</label>");
            $("#hidBunks").val("");
        });
    });
    function selPriceOrDiscount(parame) {
        if ($(parame).val() == "0") {
            $(parame).parent().find(".price0").css("display", "");
            $(parame).parent().find(".discount0").css("display", "none");
            $(".priceOrSub").html("元");
            $(".zhijianSpan").css("visibility", "hidden");
        }
        if ($(parame).val() == "1") {
            $(parame).parent().find(".price0").css("display", "none");
            $(parame).parent().find(".discount0").css("display", "");
            $(".priceOrSub").html("%");
            $(".zhijianSpan").css("visibility", "");
        }
        if ($(parame).val() == "0") {
            $(".djbcChk").css("display", "none");
            $("#djbcTxt").css("display", "none");
            $("#djbcTip").css("display", "none");
            //            $(".djbcChk").css("display", "none");
            $("#djdfTxt").css("display", "none");
            $("#djdfTip").css("display", "none");
        } else {
            $(".djbcChk").css("display", "");
            $("#djbcTxt").css("display", "none");
            $("#djbcTip").css("display", "");
            //            $("#djbcChk").css("display", "");
            $("#djdfTxt").css("display", "none");
            $("#djdfTip").css("display", "");
        }
        $("#chkdjbc,#chkdjdf").removeAttr("checked");
    }
    function GetBunks() {
        var airline = "";
        if ($("#ddlAirline").val() != null) {
            airline = $("#ddlAirline").val();
        }
        else {
            airline = $("#lblAirline").html();
        }
        var id = $("#specialType").val();
        if (id == 4) {
            id = 3;
        }
        //查询舱位
        if (id == 2) {
            var type = "CostFree";
            var url = "/PolicyHandlers/PolicyManager.ashx/QuerySpecialBunksPolicy";
            var param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "specialProductType": type };
            sendPostRequest(url, JSON.stringify(param), function (e) {
                var str = "<select id='ddlBunks' class='select' style='width:50px;'>";
                $.each(eval(e), function (i, item) {
                    if (i == 0 && $("#hidBunks").val() == "") {
                        $("#hidBunks").val(item);
                    } if ($.trim($("#hidBunks").val()) == $.trim(item)) {
                        str += "<option  value='" + item + "' selected='selected' >" + item + "</option>";
                    } else {
                        str += "<option  value='" + item + "' >" + item + "</option>";
                    }
                });
                str += "</select>";
                $("#selBunksSpan").html(str);
            }, function (e) {

            });
        }
        else {
            var voyage = "";
            var isOneWay = true;
            //判断选择的是什么舱位 
            var value = $("input[type='radio'][name='chooiceBunks']:checked").val();
            if (value == "0") {
                voyage = "OneWay";
                url = "/PolicyHandlers/PolicyManager.ashx/QueryNormalBunksPolicy";
                param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "voyage": voyage, "isOneWay": isOneWay };
                sendPostRequest(url, JSON.stringify(param), function (e) {
                    var str = "<input type='radio' value='0' name='1radio' id='1all'  class='choice' /><label for='1all'> 全选</label> <input type='radio' value='1' name='1radio' id='1not' class='choice' /><label for='1not'> 反选</label><br />";
                    $.each(eval(e), function (i, item) {
                        str += "<input type='checkbox' value='" + item + "'/>" + item;
                        if ((i + 1) % 4 == 0 && i > 0) {
                            str += "<br />";
                        }
                    });
                    $(".BunksRad").html(str);
                    $("#hidBunks").val("");
                }, function (e) {

                });
            } else {
                type = "OneWay";
                url = "/PolicyHandlers/PolicyManager.ashx/QueryBargainBunksPolicy";
                param = { "airline": airline, "startTime": $("#txtDepartrueStart").val(), "endTime": $("#txtDepartrueEnd").val(), "startETDZDate": $("#txtProvideDate").val(), "voyageType": type };
                sendPostRequest(url, JSON.stringify(param), function (e) {
                    var str = "<select id='ddlBunks' class='select' style='width:50px;'>";
                    $.each(eval(e), function (i, item) {
                        if (i == 0 && $("#hidBunks").val() == "") {
                            $("#hidBunks").val(item);
                        } if ($.trim($("#hidBunks").val()) == $.trim(item)) {
                            str += "<option  value='" + item + "' selected='selected' >" + item + "</option>";
                        } else {
                            str += "<option  value='" + item + "' >" + item + "</option>";
                        }
                    });
                    str += "</select>";
                    $(".BunksBargain").html(str);
                }, function (e) {

                });
            }
        }
    }

</script>
