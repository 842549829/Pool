//验证订单号
function valiateOrderId(orderId) {
    var orderValue = $.trim(orderId.val());
    if (orderValue.length > 0) {
        var orderPattern = /^\d{1,13}$/;
        if (orderPattern.test(orderValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
}
//验证票号
function valiateTicketNo(ticketNoId) {
    var ticketNoValue = $.trim(ticketNoId.val());
    if (ticketNoValue.length > 0) {
        var ticketNoPattern = /^\d{10}$/;
        if (ticketNoPattern.test(ticketNoValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
}
//验证PNR
function valiatePnr(pnrId) {
    var pnrValue = $.trim(pnrId.val());
    if (pnrValue.length > 0) {
        var pnrPattern = /^\w{6}$/;
        if (pnrPattern.test(pnrValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
}
//验证乘机人
function valiatePassenger(passengerId) {
    var passengerValue = $.trim(passengerId.val());
    if (passengerValue.length > 0) {
        if (passengerValue.length > 25) {
            return false;
        } else {
            return true;
        }
    } else {
        return true;
    }
}
//验证申请单号
function valiateApplyformId(applicationNo) {
    var applicationNoValue = $.trim(applicationNo.val());
    if (applicationNoValue.length > 0) {
        //20130424536545
        var applicationNoPattern = /^\d{13}$/;
        if (applicationNoPattern.test(applicationNoValue)) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}