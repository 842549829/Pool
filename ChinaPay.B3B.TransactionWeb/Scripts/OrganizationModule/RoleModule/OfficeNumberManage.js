//Office号
var urlId = getRequest();
$(function () {
    //加载数据
    bindOfficeNos();
    //添加
    $("#btnSaveOffice").click(function () {
        if (!officeCkecking($("#txtOffice").val())) {
            return false;
        }
        var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/AddOffice";
        var officeNo = $("#txtOffice").val(), impower = document.getElementById("chkAuthorization").checked;
        var parameters = JSON.stringify({ officeNo: officeNo, impower: impower, companyId: (urlId != null ? urlId["CompanyId"] : null) });
        sendPostRequest(targetUrl, parameters, function (e) {
            if (e == true) {
                alert("保存成功");
                bindOfficeNos();
            } else {
                alert("保存失败，请重试");
            }
        }, function (e) {
            alert(JSON.parse(e.responseText));
        });
    });
    //删除
    $("a.deleteOffice").live("click", function () {
        if (confirm("确定要删除吗？")) {
            var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/DeleteOfficeNo";
            var officeNo = $(this).parent("td").prev("td").prev("td").text();
            var parameters = JSON.stringify({ officeNo: officeNo, companyId: (urlId != null ? urlId["CompanyId"] : null) });
            sendPostRequest(targetUrl, parameters, function (e) {
                alert("删除成功");
                bindOfficeNos();
            }, function (e) { alert(JSON.parse(e.responseText)); });
        }
    });
    //修改
    $("a:contains('修改')").live("click", function () {
        var self = $(this).parent("td").prev("td");
        self.find("select option[value='" + self.find("label").attr("code") + "']").attr("selected", true);
        $(this).hide().next("a").show().next("a").next().show().next("a").hide().parent("td").prev("td").find("span").show().end().find("label").hide();
    });
    //取消
    $("a.cancel").live("click", function () {
        $(this).hide().prev("a").hide().prev("a").hide().prev("a").show().end().end().end().next("a").show().
        parent("td").prev("td").find("span").hide().end().find("label").show();
    });
    var save = function (self, suessed) {
        self.hide().prev("a").show().end().next("a").next("a").hide().next("a").show().parent("td").prev("td").find("span").hide().end().find("label").show();
        if (arguments.length == 2) {
            self.parent("td").prev("td").find("label").text(self.parent("td").prev("td").find("select option:selected").text()).attr("code", self.parent("td").prev("td").find("select option:selected").val());
        }
    };
    //保存
    $("a:contains('保存')").live("click", function () {
        var self = $(this);
        var selfs = self.parent("td").prev("td");
        var impower = selfs.find("select option:selected").val(), officeNumber = selfs.prev("td").text();
        var parmeter = JSON.stringify({ officeNumber: officeNumber, impower: impower, companyId: (urlId != null ? urlId["CompanyId"] : null) });
        sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/UpdateOffice", parmeter, function (e) {
            if (e) {
                alert("保存成功");
                save(self, true);
            } else {
                alert("保存失败");
                save(self);
            }
        }, function () {
            alert("保存失败");
            save(self);
        });
    });
});
//加载
function bindOfficeNos() {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/QueryOfficeNos";
    sendPostRequest(targetUrl, JSON.stringify({ companyId: (urlId != null ? urlId["CompanyId"] : null) }), function (e) {
        var strOfficeNos = "<table class='maintainTable'><colgroup><col class='w20'/><col class='w25'/><col class='w25'/><col class='w30'/></colgroup><tr><th>序号</th><th>OFFICE</th><th>是否需要授权</th><th>操作</th></tr>";
        for (var i = 0; i < e.length; i++) {
            strOfficeNos += "<tr><td>" + (i + 1) + "</td><td>" + e[i].Number + "</td><td><label class='impower' code='" + e[i].Impower + "'>" + (e[i].Impower ? "需要" : "不需要") + "</label><span class='impowers' style='display:none;'><select><option value='true'>需要</option><option value='false'>不需要</option></select><span></td><td><a href='#'>修改</a><a href='#' style='display:none;'>保存<a/>&nbsp;&nbsp;<a href='#' style='display:none;' class='cancel'>取消</a>&nbsp;&nbsp;<a href='#' class='deleteOffice'>删除</a></tr>";
        }
        strOfficeNos += "</table>";
        $("#divOffice").html(strOfficeNos);
        if ($("#ddlOffice").length > 0) { BindselOffice(); }
    }, function (e) { });
}
//验证
function officeCkecking(officeValue) {
    if (officeValue.length < 1) {
        alert("office不允许为空");
        return false;
    }
    if (!/^[a-zA-Z]{3}[0-9]{3}$/.test(officeValue)) {
        alert("office格式错误");
        return false;
    }
    return true;
}