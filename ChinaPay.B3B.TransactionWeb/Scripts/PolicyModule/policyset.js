$(function () {
    $("#txtStartTime").focus(function () {
        WdatePicker({ skin: 'default', isShowClear: false, onpicked: function () { $("#bunks").html(""); }, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}'
        });
    });
    $("#txtEndTime").focus(function () {
        WdatePicker({ skin: 'default', isShowClear: false, onpicked: function () { $("#bunks").html(""); }, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' });
    });
    $(".delRange").live("click", function () {
        var currentRange = $(this).parent().parent();
        var prevRange = currentRange.prev();
        currentRange.remove();
        $(".rangeEnd", prevRange).val($(".rangeEnd", currentRange).val());
        adjustRangeSerial();
        adjustRange(prevRange);
    });
    $(".addRange").live("click", function () {
        var currentRange = $(this).parent().parent();
        var currentRangeStart = parseFloat($(".rangeStart", currentRange).val());
        var currentRangeEnd = parseFloat($(".rangeEnd", currentRange).val());
        $(".quyu_table tr td").eq(1).find("span").html("不含");
        var newRange = currentRange.clone(true);
        newRange.find(".delRange").show();
        currentRange.after(newRange);
        adjustRangeSerial();
        $(".rangeEnd", currentRange).val(roundPolicy((currentRangeEnd + currentRangeStart) / 2));
        adjustRange(currentRange);
        $(".quyu_table tr td").eq(1).find("span").html("含");
    });
    $(".rangeEnd").blur(function () {
        if (validatePolicyValue(this)) {
            var currentRange = $(this).parent().parent();
            var rangeStart = $(".rangeStart", currentRange).val();
            var rangeEnd = $(this).val();
            if (parseFloat(rangeStart) >= parseFloat(rangeEnd)) {
                alert("区间结束值需大于开始值");
                $(this).select();
            } else {
                adjustRange(currentRange);
            }
        }
    });
    $(".rangeValue").blur(function () {
        if (validatePolicyValue(this)) {
            var rangeEndValue = $(this).parent().prev().prev().children().first().val();
            if (parseFloat(rangeEndValue) < parseFloat($(this).val())) {
                alert('设置值不能大于区间最大值');
                $(this).select();
            }
        }
    });
    $("#btnBerths").click(function () {
        initBunks();
    });
    $("#btnSave").click(function () {
        saveRanges();
        saveBunks();
    });
    parseRanges();

    parseBunks();
});
function adjustRangeSerial() {
    $(".rangeSerial").each(function (index) {
        $(this).html('第' + (index + 1) + '组区域');
    });
}
function validatePolicyValue(sender) {
    var policyValuePattern = /^[0-9]{1,2}(\.[1-9])?$/;
    var policyValue = $.trim($(sender).val());
    if (policyValue == 100 || policyValuePattern.test(policyValue)) {
        return true;
    } else {
        alert("格式错误");
        $(sender).select();
        return false;
    }
}
function roundPolicy(num) {
    return Math.round(num * 10) / 10;
}
function adjustRange(sender) {
    var currentRangeEnd = parseFloat($(".rangeEnd", sender).val());
    sender.nextAll().each(function (index, item) {
        var nextRange = $(item);
        if (currentRangeEnd >= 100) {
            nextRange.remove();
        } else {
            $(".rangeStart", nextRange).val(currentRangeEnd);
            var nextRangeEnd = parseFloat($(".rangeEnd", nextRange).val());
            if (nextRangeEnd > currentRangeEnd) {
                return false;
            } else {
                currentRangeEnd += 1;
                if (currentRangeEnd > 100) {
                    currentRangeEnd = 100;
                } else {
                    $(".rangeEnd", nextRange).val(currentRangeEnd);
                }
            }
        }
    });
}
function saveRanges() {
    var ranges = new Array();
    $("#rangeItems").children().each(function () {
        var currentRange = $(this);
        ranges.push($(".rangeStart", currentRange).val() + "|" + $(".rangeEnd", currentRange).val() + "|" + $(".rangeValue", currentRange).val());
    });
    $("#hidRanges").val(ranges.join(','));
}
function saveBunks() {
    var bunks = new Array();
    $("#bunks :checked").each(function () {
        bunks.push($(this).val());
    });
    $("#hidBunks").val(bunks.join(','));
}
function parseBunks() {
    var startTime = $.trim($("#txtStartTime").val());
    var endTime = $.trim($("#txtEndTime").val());
    var airline = $.trim($("#ddlAirLine").val());
    if (startTime != "" && endTime != "" && airline != "") {
        initBunks(function () {
            var bunks = $.trim($("#hidBunks").val());
            if (bunks != '') {
                var bunksContainer = $("#bunks");
                $.each(bunks.split(','), function (index) {
                    $("input:checkbox[value=" + this + "]", bunksContainer).attr("checked", true);
                });
            }
        });
    }
}
function parseRanges() {
    var ranges = $.trim($("#hidRanges").val());
    $(".quyu_table tr td").eq(1).find("span").html("不含");
    if (ranges != '') {
        var firstRange = $("#rangeItems").children().first();
        $.each(ranges.split(','), function (index) {
            var items = this.split('|');
            if (index == 0) {
                $(".rangeStart", firstRange).val(items[0]);
                $(".rangeEnd", firstRange).val(items[1]);
                $(".rangeValue", firstRange).val(items[2]);
            } else {
                var newRange = firstRange.clone(true);
                $(".rangeStart", newRange).val(items[0]);
                $(".rangeEnd", newRange).val(items[1]);
                $(".rangeValue", newRange).val(items[2]);
                $(".delRange", newRange).show();
                $("#rangeItems").append(newRange);
            }
        });
        $(".quyu_table tr td").eq(1).find("span").html("含");
        adjustRangeSerial();
    }
}
function initBunks(callback) {
    var startTime = $.trim($("#txtStartTime").val());
    var endTime = $.trim($("#txtEndTime").val());
    var airline = $.trim($("#ddlAirLine").val());
    if (startTime == "" || endTime == "" || airline == "") {
        alert("请选择航空公司/生效日期");
    } else {
        var targetUrl = "/PolicyHandlers/PolicyManager.ashx/QueryBunks";
        var parameters = JSON.stringify({ airline: airline, startTime: startTime, endTime: endTime });
        sendPostRequest(targetUrl, parameters, function (e) {
            var str = "";
            $.each(eval(e), function (i, item) {
                str += "<input type='checkbox' value='" + item + "'/>" + item;
            });
            if (str == "") {
                alert("此航空公司在生效日期范围中没有舱位！");
            }
            $("#bunks").html(str);
            if (callback) {
                callback(e);
            }
        }, function (e) { });
    }
}