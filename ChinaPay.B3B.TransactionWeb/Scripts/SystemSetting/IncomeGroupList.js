$(function () {
    $(".Description").tipTip({ limitLength: 20, maxWidth: "300px" });
    $(".close").click(function () {
        $("#hfdGroupId").val("");
    });
    $(":checkbox", "table tr td").click(function () {
        if (parseFloat($(":checkbox", "table tr td").length) == parseFloat($(":checkbox:checked", "table tr td").length)) {
            $("#check_All").attr("checked", "checked");
        } else {
            $("#check_All").removeAttr("checked");
        }
    })
    $(".change").click(function () {
        var groupId = $(this).attr("changeId");
        var groupName = $(this).attr("groupName");
        var groupDescription = $(this).attr("groupDescription");
        $("#txtIncomeGroupName").val(groupName);
        $("#txtIncomeGroupDescription").val(groupDescription);
        $("#hfdGroupId").val(groupId);
        $("#divOption").click();
    });
    //    $(".delete").click(function ()
    //    {
    //        var groupId = $(this).attr("deleteId");
    //        if (!confirm('确定要删除收益组吗？')) return;
    //        sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/DeleteIncomeGroup", JSON.stringify({ "groupId": groupId }),
    //              function (result)
    //              {
    //                  alert("删除成功");
    //                  $(".close").click();
    //                  window.location.href = 'IncomeGroupList.aspx';
    //              },
    //              function (e)
    //              {
    //                  if (e.statusText == "timeout")
    //                  {
    //                      alert("服务器忙");
    //                  } else
    //                  {
    //                      alert(e.responseText);
    //                  }
    //              });
    //    });
    $("#imgAdd").click(function () {
        $("#txtIncomeGroupName").val("");
        $("#txtIncomeGroupDescription").val("");
        $("#divOption").click();
    });
    $("#btnSubmitIncomeGroup").click(function () {
        if (valiateIncomeGroup()) {
            var groupId = $("#hfdGroupId").val();
            if ($.trim(groupId).length == 0) {
                sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/OpenIncomeGroup", JSON.stringify({ "name": $("#txtIncomeGroupName").val(), "description": $("#txtIncomeGroupDescription").val() }),
              function (result) {
                  alert("添加用户组成功");
                  $(".close").click();
                  window.location.href = 'IncomeGroupList.aspx';
              },
              function (e) {
                  if (e.statusText == "timeout") {
                      alert("服务器忙");
                  } else {
                      alert(e.responseText);
                  }
              });
            } else {
                sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/UpdateIncomeGroup", JSON.stringify({ "groupId": groupId, "name": $("#txtIncomeGroupName").val(), "description": $("#txtIncomeGroupDescription").val() }),
              function (result) {
                  alert("修改用户组成功");
                  $(".close").click();
                  window.location.href = 'IncomeGroupList.aspx';
              },
              function (e) {
                  if (e.statusText == "timeout") {
                      alert("服务器忙");
                  } else {
                      alert(e.responseText);
                  }
              });
            }
        }
    });
})
function valiateIncomeGroup() {
    var name = $("#txtIncomeGroupName");
    if ($.trim(name.val()).length == 0) {
        alert("请输入用户组名称");
        name.select();
        return false;
    }
    if ($.trim(name.val()).length > 25) {
        alert("用户组名称位数不能超过25");
        name.select();
        return false;
    }
    var description = $("#txtIncomeGroupDescription");
    if ($.trim(description.val()).length > 200) {
        alert("用户组描述位数不能超过200");
        description.select();
        return false;
    }
    return true;
}
function checkAll(all) {
    var checks = document.getElementsByTagName("input");
    for (var i = 0; i < checks.length; i++) {
        if (checks[i].type == "checkbox" && checks[i].id != "check_All") {
            checks[i].checked = all.checked;
        }
    }
}