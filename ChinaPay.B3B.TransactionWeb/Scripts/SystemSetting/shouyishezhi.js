$(function () {
    $(".addRange").live("click", function () {
        if ($(".quyu_table tr").length == 5) {
            alert("扣点区间只能添加五组。");
            return;
        }
        var currentRange = $(this).parent().parent();
        var currentRangeStart = parseFloat($(".rangeStart", currentRange).val());
        var currentRangeEnd = parseFloat($(".rangeEnd", currentRange).val());
        $(".quyu_table tr td").eq(1).find("span").html("不含");
        var newRange = currentRange.clone(true);
        newRange.find(".delRange").show().css("display", "block");
        currentRange.after(newRange);
        adjustRangeSerial();
        $(".rangeEnd", currentRange).val(roundPolicy((currentRangeEnd + currentRangeStart) / 2));
        adjustRange(currentRange);
        $(".quyu_table tr td").eq(1).find("span").html("含");
    });
    $(".delRange").live("click", function () {
        var currentRange = $(this).parent().parent();
        var prevRange = currentRange.prev();
        currentRange.remove();
        $(".rangeEnd", prevRange).val($(".rangeEnd", currentRange).val());
        adjustRangeSerial();
        adjustRange(prevRange);
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
});
function adjustRangeSerial() {
//    $(".rangeSerial").each(function (index) {
//        $(this).html('第' + (index + 1) + '组区域');
//    });
}
function validatePolicyValue(sender) {
    var policyValuePattern = /^[0-9]{1,2}(\.[1-9])?$/;
    var policyValue = $.trim($(sender).val());
    if (policyValue == 100 || policyValuePattern.test(policyValue)) {
        $(sender).val(parseFloat($.trim($(sender).val())));
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
    $("#hidRanges").val(ranges.join(';'));
}
 function parseRanges() {
    var ranges = $.trim($("#hidRanges").val());
    $(".quyu_table tr td").eq(1).find("span").html("不含");
    if (ranges != '') {
        var firstRange = $("#rangeItems").children().first();
        $.each(ranges.split(';'), function (index) {
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
                $(".delRange", newRange).show().css("display", "block");
                $("#rangeItems").append(newRange);
            }
        });
        $(".quyu_table tr td").eq(1).find("span").html("含");
        adjustRangeSerial();
    }
}