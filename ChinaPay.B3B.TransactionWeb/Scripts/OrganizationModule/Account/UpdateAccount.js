$(function () {
    if ($("#btnArea").length > 0) $("#btnArea").click(function () { Account($(this)); Save(1); });
    if ($("#btnPayment").length > 0) $("#btnPayment").click(function () { Account($(this)); Save(2); });
    if ($("#btnReceiving").length > 0) $("#btnReceiving").click(function () { Account($(this)); Save(3); });
    //添加按钮
    var Account = function (account) {
        account.hide().prev().end().after("<input type='button' class='btn class1' value='保存'> <input type='button' class='btn class2 ' value='取消'>").prev().prev(":text").attr("disabled", false);
        $("input:button[value='取消']").click(function () { $(this).prev().prev().show().prev().prev(":text").attr("disabled", true).end().end().end().remove().end().remove(); });
    };
    //验证
    var checking = function (text) {
        if (text.length < 1) {
            alert("账号不允许为空");
            return false;
        }
        if (!/^[\w@\.]{5,20}$/.test(text)) {
            alert("账号格式错误");
            return false;
        }
        return true;
    };
    //获取参数
    var paramenter = function (text) { return $.trim(text.prev().prev().prev(":text").val()); };
    //数据保存
    var Save = function (save) {
        $("input:button[value='保存']").unbind();
        $("input:button[value='保存']").click(function () {
            var perse = $(this);
            var isbun = false;
            switch (save) {
                case 1:
                    if (checking(paramenter(perse))) {
                        isbun = true;
                        var area = paramenter(perse);
                        CheckPayAcccountNo(area, 1, perse);
                    }
                    break;
                case 2:
                    if (checking(paramenter(perse))) {
                        isbun = true;
                        var payment = paramenter(perse);
                        CheckPayAcccountNo(payment, 2, perse);
                    }
                    break;
                case 3:
                    if (checking(paramenter(perse))) {
                        isbun = true;
                        var receiving = paramenter(perse);
                        CheckPayAcccountNo(receiving, 3, perse);
                    }
                    break;
            }
            return isbun;
        });
    };

});
function sender(parameter, type, la) {
    var par = JSON.stringify({ id: $("#hidId").val(), "account": parameter, "bytType": type });
    sendPostRequest("../../../OrganizationHandlers/Account.ashx/UpdateAccount", par, function (e) {
        if (e == true) {
            la.prev().show().prev().prev(":text").attr("disabled", true).end().end().nextAll().remove();
            alert("修改成功");
        } else {
            la.prev().show().nextAll().remove();
            alert("修改失败");
        }
    }, function (e) {
        alert("修改异常");
    });
}
function CheckPayAcccountNo(account, type, perse) {
    var parameter = JSON.stringify({ "accountNo": account });
    sendPostRequest("/OrganizationHandlers/CheckUpComPanyNews.ashx/CheckPayAccountNOusable", parameter, function (result) {
        if (result != "") {
            alert(result);
            return false;
        } else {
            sender(account, type, perse);
        }
    });
}