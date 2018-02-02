/* 销售推荐*/var txtMarket = function (e) { return Validate(["EmptyOrObject=销售推荐号不能为空", "Account=销售推荐号格式错误"], "txtMarket", e); };
$(function () {
    $(":radio[name='rdolHowToKnow']").click(function () {
        if ($(":radio[name='rdolHowToKnow']").last().is(":checked")) {
            $("#lblMarket").show().children(":text").blur(function () { return txtMarket(); });
        } else {
            $("#lblMarket").hide();
        }
    });
    $(".closeBtn").click(function () {
        $(".fixed,.layers").css("display", "none");
        return false;
    });

    if ($("#lblMessage").text().length > 0) {
        $(".fixed,.layers").css("display", "block");
        return false;
    }
});
function VerifyCode(img, verifyType) {
    var dt = new Date();
    $("#" + img).attr("src", "../../VerifyCode.ashx?verifyType=" + verifyType + "&id=" + dt);
}
function AgreedToRead() {
    if (!$("#chkAgreedToRead").is(":checked")) {
        $("#lblMessage").html("您还没有阅读并同B3B协议");
        $(".fixed,.layers").css("display", "block");
        return false;
    }
    return true;
 }  