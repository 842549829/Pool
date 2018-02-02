var applyformId = $("#lblApplyformId").text();
var pnrPattern = /^[0-9,a-z,A-Z]{6}$/;
function agreePostponeWithoutFee() {
    var postponeView = getPostponeView();
    if (postponeView) {
        var parameters = JSON.stringify({ "applyformId": applyformId, "postponeView": postponeView });
        sendPostRequest("/OrderHandlers/Apply.ashx/AgreePostponeWithoutFee", parameters, function (e) {
            alert("操作成功");
            window.location.href = $("#hidReturnUrl").val();
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
        });
    }
    function getPostponeView() {
        var flg = true;
        var postponeItems = new Array();
        $("#divAgreeWithoutFee table tbody tr").each(function (index) {
            var currentRow = $(this);
            var flightNo = $.trim($(".flightNo", currentRow).val());
            if (flightNo == '') {
                alert("请输入航班号");
                $(".flightNo", currentRow).select();
                flg = false;
                return true;
            } else if (!(/^\w{3,5}$/.test(flightNo))) {
                alert("航班号格式错误");
                $(".flightNo", currentRow).select();
                flg = false;
                return true;
            }
            var flightDate = $.trim($(".flightDate", currentRow).val());
            if (flightDate == '') {
                alert("请输入航班日期");
                $(".flightDate", currentRow).select();
                flg = false;
                return true;
            }
            var departure = $(".departureCode", currentRow).val();
            var arrival = $(".arrivalCode", currentRow).val();
            var takeoffTime = flightDate + ' ' + $(".takeoffHour", currentRow).val() + ':' + $(".takeoffMinitue", currentRow).val();
            var landingTime = flightDate + ' ' + $(".landingHour", currentRow).val() + ':' + $(".landingMinitue", currentRow).val();
            postponeItems[index] = { "AirportPair": { "Departure": departure, "Arrival": arrival }, "FlightNo": flightNo, "TakeoffTime": takeoffTime, "LandingTime": landingTime };
        });
        if (flg) {
            var newPNR = null;
            if ($("#hidRequireNewPNR").val() == "1") {
                var pnr = $.trim($("#txtPNRCode").val());
                var bpnr = $.trim($("#txtBPNRCode").val());
                if (pnr == '' && bpnr == '') {
                    alert("该申请需要分离编码，请输入分离后的新编码");
                    return null;
                }
                if (pnr != '' && !pnrPattern.test(pnr)) {
                    alert("小编码格式错误");
                    $("#txtPNRCode").select();
                    return null;
                }
                if (bpnr != '' && !pnrPattern.test(bpnr)) {
                    alert("大编码格式错误");
                    $("#txtBPNRCode").select();
                    return null;
                }
                newPNR = { "PNR" : pnr, "BPNR" : bpnr };
            }
            return { "NewPNR": newPNR, "Items": postponeItems };
        } else {
            return null;
        }
    }
}
function agreePostponeWithFee() {
    var postponeFeeViews = getPostponeFeeViews();
    if (postponeFeeViews) {
        var parameters = JSON.stringify({ "applyformId": applyformId, "postponeFeeViews": postponeFeeViews });
        sendPostRequest("/OrderHandlers/Apply.ashx/AgreePostponeWithFee", parameters, function (e) {
            alert("操作成功");
            window.location.href = $("#hidReturnUrl").val();
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
        });
    }
    function getPostponeFeeViews() {
        var flg = true;
        var result = new Array();
        $("#divAgreeWithFee table tbody tr").each(function (index) {
            var currentRow = $(this);
            var fee = $.trim($(".fee", currentRow).val());
            if (fee == '') {
                alert("请输入改期费");
                $(".fee", currentRow).select();
                flg = false;
                return true;
            } else if (!(/^\d+(\.\d{1,2})?$/.test(fee)) || parseFloat(fee) < 0) {
                alert("改期费格式错误");
                $(".fee", currentRow).select();
                flg = false;
                return true;
            }
            var departure = $(".departureCode", currentRow).val();
            var arrival = $(".arrivalCode", currentRow).val();
            result[index] = { "AirportPair": { "Departure": departure, "Arrival": arrival }, "Fee": fee };
        });
        if (flg) {
            return result;
        } else {
            return null;
        }
    }
}
function denyPostpone() {
    var reason = $.trim($("#txtDenyReason").val());
    if (reason == '') {
        alert("请输入拒绝原因");
        $("#txtDenyReason").select();
    } else {
        var parameters = JSON.stringify({ "applyformId": applyformId, "reason": reason });
        sendPostRequest("/OrderHandlers/Apply.ashx/DenyPostpone", parameters, function (e) {
            alert("拒绝成功");
            window.location.href = $("#hidReturnUrl").val();
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
        });
    }
}