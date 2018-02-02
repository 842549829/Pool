function editCredentials(sender) {
    getEditButton(sender).hide();
    getUpdateButton(sender).show();
    getCancelButton(sender).show();
    var newCredentials = getEditText(sender);
    newCredentials.show();
    newCredentials.val("");
}
function updateCredentials(sender) {
    var editRow = $(sender).parent().parent();
    var originalCredentials = $("#originalCredentials", editRow).html();
    var newCredentials = $.trim($("#newCredentials", editRow).val());
    if (newCredentials == '') {
        alert("请输入新证件号");
        $("#newCredentials", editRow).select();
        return;
    } else if (newCredentials == originalCredentials) {
        alert("新证件号不能与原证件号相同");
        $("#newCredentials", editRow).select();
        return;
    }
    var orderId = $("#hidOrderId").val();
    var passengerName = $("#passengerName", editRow).html();
    var parameters = JSON.stringify({ "orderId": orderId, "passengerName": passengerName, "originalCredentials": originalCredentials, "newCredentials": newCredentials });
    setAllButtonEnable(false);
    sendPostRequest("../../OrderHandlers/Order.ashx/UpdateCredentials", parameters, function (e)
    {
        alert("操作成功");
        $("#originalCredentials", editRow).html(newCredentials);
        setAllButtonEnable(true);
        cancelEditCredentials($(sender));
    }, function (e)
    {
        if (e.status == 300)
        {
            alert(JSON.parse(e.responseText));
        } else
        {
            alert("系统故障，请联系平台技术人员");
        }
        setAllButtonEnable(true);
    });

    /*$.ajax({
        type: "POST",
        url: "../../OrderHandlers/Order.ashx/UpdateCredentials",
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            alert("操作成功");
            $("#originalCredentials", editRow).html(newCredentials);
            setAllButtonEnable(true);
            cancelEditCredentials($(sender));
        },
        error: function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台技术人员");
            }
            setAllButtonEnable(true);
        }
    });*/
}
function cancelEditCredentials(sender) {
    getEditButton(sender).show();
    getUpdateButton(sender).hide();
    getCancelButton(sender).hide();
    getEditText(sender).hide();
}
function getEditButton(sender) {
    return $(".editButton", $(sender).parent());
}
function getUpdateButton(sender) {
    return $(".updateButton", $(sender).parent());
}
function getCancelButton(sender) {
    return $(".cancelButton", $(sender).parent());
}
function getEditText(sender) {
    return $("#newCredentials", $(sender).parent().parent());
}
function setAllButtonEnable(enable) {
    if (enable) {
        $("button, input:button").removeAttr("disabled");
    } else {
        $("button, input:button").attr("disabled", "disabled");
    }
}