var pnrCodePattern = /^[a-z,A-Z,0-9]{6}$/;
$(function () {
    $("#AdultPNRCodeForContentTitle").hide();
    $("#txtAdultPNRCodeForContent").hide();
    $("#radAdultPNR").click(function () {
        $("#divAdultPNRContent").show();
        $("#divChildrenPNRContent").hide();
    });
    $("#radChildrenPNR").click(function () {
        $("#divAdultPNRContent").hide();
        $("#divChildrenPNRContent").show();
    });
    $("#radChildrenPNRContent").click(function () {
        $("#AdultPNRCodeForContentTitle").show();
        $("#txtAdultPNRCodeForContent").show();
    });
    $("#radAdultPNRContent").click(function () {
        $("#AdultPNRCodeForContentTitle").hide();
        $("#txtAdultPNRCodeForContent").hide();
    });
    $("#btnPNRContentImport").click(function () {
        if ($("#radChildrenPNRContent").attr('checked') && !validatePNRCode("txtAdultPNRCodeForContent", "成人编码")) {
            return false;
        }
        if ($.trim($("#txtPNRContent").val()) == '') {
            alert("请输入编码内容");
            $("#txtPNRContent").select();
            return false;
        }
        return true;
    });
});
function validatePNRCode(inputControl, operateItem) {
    var pnrCode = $.trim($("#" + inputControl).val());
    if (pnrCode == '') {
        $("#lblWarnInfo").html("请输入"+operateItem);
        $("#divWarnInfo").click();
        $("#" + inputControl).select();
        return false;
    } else if (!pnrCodePattern.test(pnrCode)) {
        $("#lblWarnInfo").html("请输入正确的PNR内容");
        $("#divWarnInfo").click();
        $("#" + inputControl).select();
        return false;
    }
    return true;
}