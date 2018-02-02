function btnSubmit() {
    $("#seatName").val("");
    var ddlAirline = $("#ddlAirline").val();
    var txtHBStartDate = $("#txtHBStartDate").val();
    var txtCpStartDate = $("#txtCpStartDate").val();
    var ddlDepartCity = $("#ddlDepartCity").val();
    var ddlArriveCity = $("#ddlArriveCity").val();
    var radiolist = $("#radiolist :radio[checked]").val();
    var ddlTJType = $("#ddlTJType").val();
    var txtCwCode = $("#txtCwCode").val();
    var txtDiscount = $("#txtDiscount").val();
    var ddltdType = $("#ddltdType").val();
    var regu = /^[0-9]+(.[0-9]+)?$/; ///^(([1-9]\d)|([1-2](([0-7]\d)|80)))$/;
    var r = /^[A-Za-z][0-9]+$/;
    var txtET = $("#txtET").val();
    var txtCodeExtend = $("#txtCodeExtend").val();
    var dropMpType = $("#dropMpType").val();
    if (ddlAirline <= 0) {
        alert("请选择航空公司!");
        $("#ddlAirline").focus();
        return false;
    }

    if (txtHBStartDate == "") {
        alert("请选择航班日期!");
        $("#txtHBStartDate").focus();
        return false;
    }
    if ($("#txtHBStopDate").val() != "") {
        if ($("#txtHBStopDate").val() < $("#txtHBStartDate").val()) {
            alert("截止时间不能小于开始时间!");
            $("#txtHBStopDate").focus();
            return false;
        }
    }
    if (txtCpStartDate == "") {
        alert("请选择出票日期!");
        $("#txtCpStartDate").focus();
        return false;
    }
//    if (txtCpStartDate > txtHBStartDate) {
//        alert("出票日期不能晚于航班日期!");
//        $("#txtCpStartDate").focus();
//        return false;
//    }
    if (radiolist == "0" || radiolist == "1") {
        if ($("#ddlDepartCity").val() != "0" && $("#ddlArriveCity").val() != "0") {
            if ($("#ddlArriveCity").val() == $("#ddlDepartCity").val()) {
                alert("到达机场不能跟出发机场相同");
                $("#ddlArriveCity").focus();
                return false;
            }
        }
    }
    if ($("#radiolist :radio:checked").length <= 0) {
        alert("请选择舱位类型");
        return false;
    }
    if ($("#chklVoyageType :checkbox:checked").length <= 0) {
        alert("请选择行程类型");
        return false;
    }
    if ($("#chklTravelType :checkbox:checked").length <= 0) {
        alert("请选择旅行类型");
        return false;
    }
    if ($("#chklPassengerType :checkbox:checked").length <= 0) {
        alert("请选择旅客类型");
        return false;
    }
    if (txtCwCode == "") {
        alert("请填写舱位代码!");
        $("#txtCwCode").select();
        return false;
    }
    if (txtCwCode.length > 1) {
        alert("舱位代码只能为一位字符!");
        $("#txtCwCode").select();
        return false;
    }
    if (radiolist != "2" && radiolist != "3" && radiolist != "4" && radiolist != "5" && radiolist !="6") {
        if (txtDiscount == "") {
            alert("请填写舱位折扣!");
            $("#txtDiscount").select();
            return false;
        }
    }
    if (radiolist != "2" && radiolist != "3" && radiolist != "4" && radiolist != "5" && radiolist != "6" && !($.trim(txtDiscount).match(regu))) {
        alert("舱位折扣只能为正整数类型!");
        $("#txtDiscount").select();
        return false;
    }

    if (radiolist == "2" && ddlTJType <= 0) {
        alert("请选择舱位描述!");
        return false;
    }
    if (radiolist == "1" && ddltdType <= 0) {
        alert("请选择头等/舱位类型!");
        return false;
    }
    if (radiolist == "5" && dropMpType <= 0) {
        alert("请选择舱位描述");
        return false;
    }
    var txtseat = "";
    var seatTJ = ""
    for (var i = 0; i < 100; i++) {
        if ($("#txtCodeExtend" + i).length > 0) {
            if ($("#txtCodeExtend" + i).val() == "") {
                alert("扩展舱位不能为空!");
                $("#txtCodeExtend" + i).select();
                return false;
            }
            if ($("#txtCodeExtend" + i).val().length != "2") {
                alert("扩展舱位的长度必须为两位字符!");
                $("#txtCodeExtend" + i).select();
                return false;
            }
            if (!($.trim($("#txtCodeExtend" + i).val()).match(r))) {
                alert("扩展舱位格式不正确!");
                $("#txtCodeExtend" + i).select();
                return false;
            }
            if (radiolist != 2) {
                var subclass = $("#txtRateExtend" + i).val();
                if (subclass == "") {
                    alert("扩展舱位的折扣不能为空!");
                    $("#txtRateExtend" + i).select();
                    return false;
                }
                if (!($.trim(subclass).match(regu))) {
                    alert("扩展舱位的折扣只能为数字类型!");
                    $("#txtRateExtend" + i).select();
                    return false;
                }
            }

            if (txtseat == "") {
                txtseat = $("#txtCodeExtend" + i).val() + "," + $("#txtRateExtend" + i).val();
            } else {
                txtseat = txtseat + "|" + $("#txtCodeExtend" + i).val() + "," + $("#txtRateExtend" + i).val();
            }
            if (seatTJ == "") {
                seatTJ = $("#txtCodeExtend" + i).val();
            } else {
                seatTJ = seatTJ + "|" + $("#txtCodeExtend" + i).val();
            }

        } else {
            break;
        }
    }
    if (seatTJ != "") {
        $("#seatTJ").val(seatTJ);
    }
    if (txtseat != "") {
        $("#seatName").val(txtseat);
    }
    if ($("#txtRefundRegulation").val() == "") {
        $("#txtRefundRegulation").select();
        alert("请输入退票规定");
        return false;
    }
    if ($("#txtChangeRegulation").val() == "") {
        $("#txtChangeRegulation").select();
        alert("请输入更改规定");
        return false;
    }
    if ($("#txtEndorseRegulation").val() == "") {
        $("#txtEndorseRegulation").select();
        alert("请输入签转规定");
        return false;
    }
}
$(document).ready(function () {
    SeatType();
    var str = $("#hiddtr").val().replace(/\(/g, "<").replace(/\)/g, ">");
    if (str != "") {
        var $tr = $(str);
        $('#trFirst').after($tr);
    }
    var index = $("#hiddindex").val();
    $('.add').click(function () {
        var type = $("#radiolist :radio[checked]").val();
        if (type == "0" || type == "1") {
            var $tr = $('<tr class="tr"><th height="23">扩展：</th><td><input type="text" class="input2" id="txtCodeExtend' + index + '" name="txtCodeExtend' + index + '"></td><th height="23">折扣：</th><td><input type="text" class="input2 zhekou" id="txtRateExtend' + index + '" name="txtRateExtend' + index + '" /></td></tr>');
            $('.tr:last').after($tr);
            index++;
        }
        else {
            var $tr = $('<tr class="tr"><th height="23">扩展：</th><td colspan="3"><input type="text" class="input2" id="txtCodeExtend' + index + '" name="txtCodeExtend' + index + '"></td></tr>');
            $('.tr:last').after($tr);
            index++;
        }
    });
    $('.cut').click(function () {
        if (index > 0) {
            $('.tr:last').remove();
            index--;
        }
    });
    $("#radiolist>:radio").click(function () {
        SeatType();
    });
});
//删除子舱位
function DelSubSeatALL() {
    var hiddUpdate = $("#hiddUpdate").val();
    $('.tr').each(function () {
        if ($(this).attr("id") != "trFirst") {
            if (hiddUpdate != "update") { $(this).remove(); }
        }
    });
}
//舱位切换
function SeatType() {
    var seattype = $("#radiolist :radio[checked]").val();
    if (seattype == "0") { //明折明扣
        $("#ddlDepartCity").attr("disabled", "");
        $("#ddlArriveCity").attr("disabled", "");
        $("#txtHBStartDate").attr("disabled", "");
        $("#txtCpStartDate").attr("disabled", "");
        $("#txtDiscount").attr("disabled", "");
        $("#txtET").attr("disabled", "");
        $('.add').show();
        $(".city_new").show(); //机场行
        $(".cut").show();
        $('.yc').show();
        $('.gl').attr('colSpan', '1')
        $('.gl2').attr('colSpan', '3');
        $('.cType').hide();
        voyageType("0");
        DelSubSeatALL();
    }
    else if (seattype == "1") { //头等公务
        $("#ddlDepartCity").attr("disabled", "");
        $("#ddlArriveCity").attr("disabled", "");
        $("#txtHBStartDate").attr("disabled", "");
        $("#txtCpStartDate").attr("disabled", "");
        $("#txtET").attr("disabled", "");
        $("#txtDiscount").attr("disabled", "");
        $('.sptejia').hide()

        $(".city_new").show();
        $('.add').show();
        $(".cut").show();
        $('.yc').show();
        $('.gl').attr('colSpan', '1')
        $('.gl2').attr('colSpan', '1');
        $('.cType').show();
        $('.sptoudeng').show();
        $('.dropMpType').hide();
        voyageType("1");
        DelSubSeatALL();
    } else if (seattype == "2") {//动态特价
        $('.yc').hide(); //折扣
        $('.gl').attr('colSpan', '6'); //舱位类型跨列
        $('.gl2').attr('colSpan', '1'); //舱位代码跨列
        $('.cType').show(); //舱位描述单元格

        $(".city_new").hide(); //机场行
        $("#txtDiscount").attr("disabled", "disabled");
        $('.sptejia').show()
        $('.sptoudeng').hide();
        $('.dropMpType').hide();
        $('.add').show(); //添加子舱位
        $(".cut").show(); //删除子舱位
        voyageType("2");
        DelSubSeatALL();

    } else if (seattype == "3") {//往返产品
        $(".city_new").hide(); //机场行
        $('.sptejia').hide()//特价描述
        $('.sptoudeng').hide(); //头等公务舱描述
        $('.add').hide();
        $(".cut").hide();
        $('.yc').hide();
        $('.gl').attr('colSpan', '3')
        $('.gl2').attr('colSpan', '3');
        $('.cType').hide();
        voyageType("3");
        DelSubSeatALL();
    }
    else if (seattype == "4") {//中转联程
        $(".city_new").hide(); //机场行
        $('.sptejia').hide()//特价描述
        $('.sptoudeng').hide(); //头等公务舱描述
        $('.add').hide();
        $(".cut").hide();
        $('.yc').hide();
        $('.gl').attr('colSpan', '3')
        $('.gl2').attr('colSpan', '3');
        $('.cType').hide();
        voyageType("4");
        DelSubSeatALL();
    } else if (seattype == "5") {//免票
        $('.yc').hide(); //折扣
        $('.gl').attr('colSpan', '6'); //舱位类型跨列
        $('.gl2').attr('colSpan', '1'); //舱位代码跨列
        $('.cType').show(); //舱位描述单元格

        $(".city_new").hide(); //机场行
        $("#txtDiscount").attr("disabled", "disabled");
        $('.sptejia').hide()
        $('.sptoudeng').hide();
        $('.dropMpType').show();

        $('.add,.cut').hide(); //添加子舱位
        voyageType("5");
        DelSubSeatALL();
    } else if (seattype == "6") {//团队
        $(".city_new").hide(); //机场行
        $('.sptejia').hide()//特价描述
        $('.sptoudeng').hide(); //头等公务舱描述
        $('.add').hide();
        $(".cut").hide();
        $('.yc').hide();
        $('.gl').attr('colSpan', '3')
        $('.gl2').attr('colSpan', '3');
        $('.cType').hide();
        voyageType("6");
        DelSubSeatALL();
    }
}
function voyageType(type){
   var hiddUpdate = $("#hiddUpdate").val();
    $("#chklVoyageType input:checkbox").each(function () {
        var self = $(this);
        if (type == "3") {//往返
            if (self.attr("id") == "chklVoyageType_1") {
                self.removeAttr("disabled").attr("checked", "checked");
            } else {
                self.attr("disabled", "disabled");
                if (hiddUpdate != "update") {
                    self.removeAttr("checked");
                }
            }

        } else if (type == "4") {//中转
            if (self.attr("id") == "chklVoyageType_2" || self.attr("id") == "chklVoyageType_3") {
                self.removeAttr("disabled").attr("checked", "checked");
            } else {
                self.attr("disabled", "disabled");
                if (hiddUpdate != "update") {
                    self.removeAttr("checked");
                }
            }
        } else {
            self.removeAttr("disabled");
            if (hiddUpdate != "update") {
                self.removeAttr("checked");
            }
        }
    });
    $("#chklTravelType input:checkbox").each(function () {
        var self = $(this);
        if (type == "6") {//团队
            if (self.attr("id") == "chklTravelType_1") {
                self.removeAttr("disabled").attr("checked", "checked");
            } else {
                self.attr("disabled", "disabled");
                if (hiddUpdate != "update") {
                    self.removeAttr("checked");
                }
            }
        } else {
            self.removeAttr("disabled");
            if (hiddUpdate != "update") {
                self.removeAttr("checked");
            }
        }
    });
}