function editTicketNo(sender) {
    getEditButton(sender).hide();
    getUpdateButton(sender).show();
    getCancelButton(sender).show();
    var newTicketNo = getEditText(sender);
    newTicketNo.show();
    newTicketNo.val("");
}
function updateTicketNo(sender) {
    var editRow = $(sender).parent().parent();
    var originalTicketNo = $("#originalTicketNo", editRow).html();
    var newTicketNo = $.trim($("#newTicketNo", editRow).val());
    if (newTicketNo == '') {
        alert("请输入新票号");
        $("#newTicketNo", editRow).select();
        return;
    } else if (newTicketNo == originalTicketNo) {
        alert("新票号不能与原票号相同");
        $("#newTicketNo", editRow).select();
        return;
    } else if (!(/^[0-9]{10}$/).test(newTicketNo)) {
        alert("新票号格式错误，只能为10位数字");
        $("#newTicketNo", editRow).select();
        return;
    }
    var orderId = $("#hidOrderId").val();
    var parameters = JSON.stringify({ "orderId": orderId, "originalTicketNo": originalTicketNo, "newTicketNo": [newTicketNo],"isPlatform":true });
    setAllButtonEnable(false);
    sendPostRequest("../../OrderHandlers/Order.ashx/UpdateTicketNo", parameters,
        function (e)
        {
            alert("操作成功");
            $("#originalTicketNo", editRow).html(newTicketNo);
            setAllButtonEnable(true);
            cancelEditTicketNo(sender);
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
        url: "../../OrderHandlers/Order.ashx/UpdateTicketNo",
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            alert("操作成功");
            $("#originalTicketNo", editRow).html(newTicketNo);
            setAllButtonEnable(true);
            cancelEditTicketNo(sender);
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
function cancelEditTicketNo(sender) {
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
    return $("#newTicketNo", $(sender).parent().parent());
}
function setAllButtonEnable(enable) {
    if (enable) {
        $(".btn").removeAttr("disabled");
    } else {
        $(".btn").attr("disabled", "disabled");
    }
}