$(function () {
    $("#btnModify").hide();
    $("#btnSave").show();
    var url = decodeURI(window.location.href);
    if (url.indexOf('id=') != -1) {
        var parameters = JSON.stringify({ "permissionRole": url.substring(url.indexOf('id=') + 3, url.length) });
        ashx_url = "../../PermissionHandlers/PermissionRoleQuery.ashx/QueryPermissionRole";
        RequestAshx(ashx_url, parameters, "ShowRole");
        $("#RoleId").html(url.substring(url.indexOf('id=') + 3, url.length));
        $("#btnModify").show();
        $("#btnSave").hide();
    }
    $("#btnSave").click(function () {
        var rolename = $("#RoleName").val();
        var roleremark = $("#RoleRemark").val();
        if ($("#RoleName").val() == "") {
            alert("角色名不能为空!");
            return;
        }
        if (rolename.length > 100) {
            $("#RoleName").val(rolename.substring(0, 100));
            alert("角色名称只能输入100个字符以内！");
            return;
        }
        if (roleremark.length > 100) {
            $("#RoleRemark").val(roleremark.substring(0, 100));
            alert("备注信息只能输入100个字符以内！");
            return;
        }
        var enable = true;
        if ($("input[name='rad']:checked").val() == 0) {
            enable = true;
        } else {
            enable = false;
        }

        var parameters = JSON.stringify({ "permissionRoleView": { "Name": rolename, "Remark": roleremark, "Valid": enable} });
        RequestAshx("../../PermissionHandlers/PermissionRoleQuery.ashx/RegisterPermissionRole", parameters, "Save");
    });
    $("#btnModify").click(function () {
        var rolename = $("#RoleName").val();
        var roleremark = $("#RoleRemark").val();
        if ($("#RoleName").val() == "") {
            alert("角色名不能为空!");
            return;
        }
        if (rolename.length > 100) {
            $("#RoleName").val(rolename.substring(0, 100));
            alert("角色名称只能输入100个字符以内！");
            return;
        }
        if (roleremark.length > 100) {
            $("#RoleRemark").val(roleremark.substring(0, 100));
            alert("备注信息只能输入100个字符以内！");
            return;
        }
        var enable = true;
        if ($("input[name='rad']:checked").val() == 0) {
            enable = true;
        } else {
            enable = false;
        }
        var parameters = JSON.stringify({ "id": $("#RoleId").html(), "permissionRoleView": { "Name": rolename, "Remark": roleremark, "Valid": enable} });
        RequestAshx("../../PermissionHandlers/PermissionRoleQuery.ashx/UpdatePermissionRole", parameters, "Modify");
    });
    $("#btnCheck").click(function () {
        if ($("#RoleName").val() == "") {
            $("#msg").html("角色名称不能为空！");
            $("#msg").css("color", "Red");
        } else {
            var parameters = JSON.stringify({ "permissionRoleName": $("#RoleName").val() });
            var ashx_url = "../../PermissionHandlers/PermissionRoleQuery.ashx/IsValidPermissionRoleNameOfRegister";
            if (url.indexOf('id=') != -1) {
                parameters = JSON.stringify({ "permissionRole": $("#RoleId").html(), "permissionRoleName": $("#RoleName").val() });
                ashx_url = "../../PermissionHandlers/PermissionRoleQuery.ashx/IsValidPermissionRoleNameOfModify";
            }
            RequestAshx(ashx_url, parameters, "Check");
        }
    });
});
function RequestAshx(ashx_url, parameters, msg) {
    sendPostRequest(ashx_url, parameters, function (e) {
        if (msg == "Modify") {
            alert("修改成功");
            window.location.href = 'System_RoleMenusList.aspx';
        }
        if (msg == "Save") {
            alert("新增成功");
            window.location.href = 'System_RoleMenusList.aspx';
        }
        if (msg == "Check") {
            if (e == true) {
                $("#msg").html("角色名称可以使用");
                $("#msg").css("color", "Green");
            } else {
                $("#msg").html("角色名称已重复");
                $("#msg").css("color", "Red");
            }
        }
        if (msg == "ShowRole") {
            $("#RoleName").val(e.Name);
            $("#RoleRemark").val(e.Remark);
            if (e.Valid) {
                $("#radEnable").attr("checked", "checked");
            } else {
                $("#radDisEnable").attr("checked", "checked");
            }
            $("#title").html("修改角色");
        }
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
}