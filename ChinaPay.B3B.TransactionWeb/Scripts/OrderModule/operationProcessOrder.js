function cancelOrder() {
    sendProcessRequest("Cancel");
}
function reETDZ()
{
    if (!confirm('您确定要将该订单再次指向给该出票方吗?')) return;
    sendProcessRequest("ReETDZ");
}
function reSupply() {
    sendProcessRequest("ReSupply");
}
function setAllButtonEnable(enable) {
    if (enable) {
        $("button").removeAttr("disabled");
    } else {
        $("button").attr("disabled", "disabled");
    }
}
function sendProcessRequest(method)
{
    setAllButtonEnable(false);
    var target = "/OrderHandlers/Order.ashx/" + method;
    var orderId = $("#lblOrderId").html();
    var parameters = JSON.stringify({ "orderId": orderId });
    sendPostRequest(target, parameters, function (e) {
        alert("操作成功");
        window.location.href = $("#hidReturnUrl").val();
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert("系统故障，请联系平台技术人员");
        }
        setAllButtonEnable(true);
    });
}