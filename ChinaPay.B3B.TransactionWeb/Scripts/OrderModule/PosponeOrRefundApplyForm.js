$(function () {
    var pageSize = 10;
    $("#txtApplyformId").OnlyNumber().LimitLength(13);
    $("#applyType li").eq(0).find("a").addClass("curr");
    $("#refundStatus li a").each(function () {
        if ($(this).html() == $("#hfdRefundStatus").val()) {
            $("#refundStatus li a").removeClass("curr");
            $(this).addClass("curr");
        }
    });
    $("#posponeStauts li a").each(function () {
        if ($(this).html() == $("#hfdPosponeStatus").val()) {
            $("#posponeStauts li a").removeClass("curr");
            $(this).addClass("curr");
        }
    });
    if ($.trim($("#hfdApplyformType").val()) == "退票" || $.trim($("#hfdApplyformType").val()) == "废票") {
        $("#posponeStauts").hide();
        $("#refundStatus").show();
    } else {
        if ($.trim($("#hfdApplyformType").val()) == "改期") {
            $("#posponeStauts").show();
            $("#refundStatus").hide();
        } else {
            $("#posponeStauts").hide();
            $("#refundStatus").hide();
        }
    }
    queryApplyform(1);
    $("#applyType li a").live("click", function () {
        $("#applyType li a").removeClass("curr");
        $("#hfdApplyformType").val($.trim($(this).html()));
        $(this).addClass("curr");
        if ($.trim($(this).html()) == "退票" || $.trim($(this).html()) == "废票") {
            $("#posponeStauts").hide();
            $("#refundStatus").show();
        } else {
            if ($.trim($("#hfdApplyformType").val()) == "改期") {
                $("#posponeStauts").show();
                $("#refundStatus").hide();
            } else {
                $("#posponeStauts").hide();
                $("#refundStatus").hide();
            }
        }
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
    $("#applyType li a").each(function () {
        if ($(this).html() == $("#hfdApplyformType").val()) {
            $("#applyType li a").removeClass("curr");
            $(this).addClass("curr");
        }
    });

    $("#refundStatus li a").live("click", function () {
        $("#refundStatus li a").removeClass("curr");
        $("#hfdRefundStatus").val($.trim($(this).html()));
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
    $("#posponeStauts li a").live("click", function () {
        $("#posponeStauts li a").removeClass("curr");
        $("#hfdPosponeStatus").val($.trim($(this).html()));
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
    $("#btnQuery").click(function () {
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
})