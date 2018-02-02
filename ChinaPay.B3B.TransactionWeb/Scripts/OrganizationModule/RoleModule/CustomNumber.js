//自定义编号
$(function () {
    //自定义编号加载
    QueryCustomNumber();
    //全选
    $("#chkAll").click(function () {
        $(".fl.w20 :checkbox").each(function () {
            if (!$(this).attr("disabled")) {
                if ($("#chkAll").is(":checked")) {
                    $(this).attr("checked", true);
                } else {
                    $(this).attr("checked", false);
                }
            }
        });
    });
    //员工分配
    $(".staffDistribution").live("click", function () {
        var self = $(this);
        var officeName = self.attr("officeName");
        var officeId = self.attr("officeId");
        //打开遮罩层
        $("#divOpcial").click();
        //设置OFFICE号
        $("#lblOfficeNumber").text(officeName).attr("code", officeId);
        sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/BindEmoloyee", JSON.stringify({ "officeId": officeId }), function (result) {
            $("#divEmoloyee").empty();
            for (var item = 0; item < result.length; item++) {
                var employeeHtml = "<span class='fl w20' title='" + (result[item].Enabled ? '该员工已经被禁用' : '') + "' employeeId='" + result[item].Id + "'><input type='checkbox' id='" + item + "'/> <label for='" + item + "'>" + result[item].Name + "</label></span>";
                $("#divEmoloyee").append(employeeHtml);
                $("#" + item).attr("disabled", result[item].Enabled);
                $("#" + item).attr("checked", result[item].Impower);
            }
            if (employeeHtml == "") {
                $("#divEmoloyee").html("<a href='#'>还没有员工,立即添加</a>");
            } else {
                if ($(".fl.w20 :checkbox[disabled!='disabled']:not(:checked)").length == 0) { $("#chkAll").attr("checked", "checked"); } else { $("#chkAll").removeAttr("checked"); }
            }
        }, function (e) {
            if (e.statusText == "timeout") {
                alert("服务器忙");
            } else {
                alert(e.responseText);
            }
        });
    });
    $("#divEmoloyee>span>input:checkbox").live("click",function () {
        if ($("#divEmoloyee :checkbox[disabled!='disabled']:not(:checked)").length == 0) {
            $("#chkAll").attr("checked", "checked");
        } else {
            $("#chkAll").removeAttr("checked");
        }
    });
    //设置员工授权
    $("#btnEmployeeSave").click(function () {
        var idLiset = new Array();
        $("#divEmoloyee :checked").each(function () {
            idLiset.push($(this).parent("span").attr("employeeId"));
        });
        var officeId = $("#lblOfficeNumber").attr("code");
        var parameter = JSON.stringify({ "officeId": officeId, "employeeId": idLiset });
        sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/SetEmoloyee", parameter, function (e) {
            if (e) {
                alert("保存成功");
            } else {
                alert("保存失败");
            }
            $(".close").click();
        }, function () { alert("保存异常"); });
    });
    //删除
    $("a.deleteCustNumber").live("click", function () {
        if (confirm("是否要删除?")) {
            var self = $(this);
            sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/DeleteCustomNumber", JSON.stringify({ "customNumberId": self.next("a").attr("officeId") }),
            function (result) {
                if (result) {
                    alert("删除成功");
                    QueryCustomNumber();
                } else {
                    alert("删除失败");
                }
            });
        }
    });
    $("#btnSvaeCustom").click(function () {
        var customNumber = $("#txtCustomNumber").val(), describe = $("#textDescribe").val();
        if (customNumber.length < 1) {
            alert("自定义编号不能为空");
            return false;
        }
        if (!/^[a-zA-Z]{3}[0-9]{3}$/.test(customNumber)) {
            alert("自定义编号格式错误,如：KMG665");
            return false;
        }
        if (/[<>]/.test(describe)) {
            alert("描述格式错误");
            return false;
        }
        if (describe.length > 50) {
            alert("描述太长了,最多50个字符");
            return false;
        }
        var parameter = JSON.stringify({ "customNumber": customNumber, "describe": describe });
        sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/AddCustomNumber", parameter, function (reslut) {
            if (reslut) {
                alert("添加成功");
                QueryCustomNumber();
            } else {
                alert("添加失败");
            }
        }, function (e) {
            alert(JSON.parse(e.responseText));
        });
    });
});
function QueryCustomNumber() {
    sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/QueryCustomNumber", null, function (e) {
        var strOfficeNos = "<table class='maintainTable'><colgroup><col class='w20'/><col class='w25'/><col class='w25'/><col class='w30'/></colgroup><tr><th>序号</th><th>编号</th><th>描述</th><th>操作</th></tr>";
        for (var i = 0; i < e.length; i++) {
            strOfficeNos += "<tr><td>" + (i + 1) + "</td><td>" + e[i].Number + "</td><td>" + e[i].Describe + "</td><td><a href='#' class='deleteCustNumber'>删除</a>&nbsp;&nbsp;<a href='#' style='display:" + (hfdEmpowermentOffice = $("#hfdEmpowermentOffice").val().toLocaleUpperCase() != "TRUE" ? 'none' : '') + "' class='staffDistribution' officeName='" + e[i].Number + "' officeId='"+e[i].Id+"'>员工分配</a></tr>";
        }
        strOfficeNos += "</table>";
        $("#CustomNumbers").html(strOfficeNos);
    });
 }