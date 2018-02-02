$(function () {
    $("#divOutPutImage img.buttonImg").click(function () {
        var aid = $(this).attr("dataType");
        var filePath = $(this).attr("FilePath");
        $("#divLayerImage img").attr("src", filePath);
        $("#a" + aid).click();
    });
    $("#divOutPutImage input:button").click(function () {
        var self = $(this);
        var id = self.attr("id");
        if (id != "btnAddApplyAttachment") {
            if (confirm("是否要删除")) {
                sendPostRequest("/OrderHandlers/Apply.ashx/DeleteApplyAttachmentView", JSON.stringify({ "applyAttachmentId": id }),
                                function (result) {
                                    $("#span" + id).remove();
                                    alert("删除成功");
                                    window.location.href = window.location.href;
                                }, function (e) {
                                    alert("删除失败");
                                });
            }
        } else {
            $("#aUploadAttachmentLayer").click();
        }
    });
    $("#btnConfirm").click(function () {
        if (!$("#fileAttachment").val().match(/.jpg|.png|.bmp/i)) {
            alert("文件格式不正确只支持jpg|png|bmp的图片");
            return false;
        }
     });
});