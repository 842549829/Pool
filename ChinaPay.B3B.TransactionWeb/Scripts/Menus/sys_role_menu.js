
//加载菜单
var setting = {
    simpleData: {
        enable: true
    }, check: {
        enable: true,
        chkStyle: "checkbox",
        chkboxType: { "Y": "ps", "N": "ps" }
    }
};
$(document).ready(function () {
    //调用菜单的方法
    var website = $("input[name='website']:checked").val();

    MenuInfoMethod(website);
    $("input[name='website']").click(function () {
        website = $(this).val();
        MenuInfoMethod(website);
    });

    var url = decodeURI(window.location.href);
    if (url.indexOf('id=') != -1 && url.indexOf('name=') != -1 && url.indexOf('code=') != -1) {
        $("#type").html(url.substring(url.indexOf('code=') + 5, url.length));
        $("#typename").html(url.substring(url.indexOf('name=') + 5, url.indexOf('code') - 1));
    } else {
        window.location.href = "./System_roleMenuList.htm";
    }
    $("#btn_save").click(function () {
        var str_ids = new Array();
        var z_children = null;
        var k = 0;
        //得到树
        var treeObj = $.fn.zTree.getZTreeObj("tree");
        if (treeObj != null) {
            //取得所有选中的
            var nodes = treeObj.getCheckedNodes(true);
        }
        for (var i = 1; i < nodes.length; i++) {
            z_children = new Array();
            if (nodes[i].children != null) {
                for (var j = 0; j < nodes[i].children.length; j++) {
                    if (nodes[i].children[j].checked) {
                        z_children[k] = { "Id": nodes[i].children[j].id, "Name": nodes[i].children[j].name };
                        k++;
                    }
                }
            }
            str_ids[i - 1] = { "Id": nodes[i].id, "Name": nodes[i].name, "Children": null };
            if (z_children.length > 0) {
                str_ids[i - 1] = { "Id": nodes[i].id, "Name": nodes[i].name, "Children": z_children };
            }
        }
        sendPostRequest("../PermissionHandlers/PermissionQuery.ashx/SaveUserRolePermission",
            JSON.stringify({ "userRole": url.substring(url.indexOf('code=') + 5, url.length), "website": website, "strs": str_ids }),
            function (e) {
                alert("设置成功！");
                window.location.href = "System_RoleMenuList.htm";
            },
            function (e) {
                if (e.status == 300) {
                    alert(JSON.parse(e.responseText));
                } else {
                    alert(e.statusText);
                }
            });
    });
});
function MenuInfoMethod(website) {
    sendPostRequest("../PermissionHandlers/MenusQuery.ashx/QueryValidMenus", JSON.stringify({ "website": website }), function (e) {
        //初始化菜单
        $.fn.zTree.init($("#tree"), setting, e);
        if (window.location.href.toString().indexOf("code=") != -1) {
            var treeObj = $.fn.zTree.getZTreeObj("tree");
            loadRoleInfo(treeObj, website);
        }
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
}

;

function loadRoleInfo(treeObj, website) {
    //修改绑定已有的菜单 
    //得到传过来的userrole
    var userrole = window.location.href.substring(window.location.href.indexOf('code=') + 5, window.location.href.length);
    sendPostRequest("../PermissionHandlers/PermissionQuery.ashx/QueryPermissionOfUserRole", JSON.stringify({ "userRole": userrole, "website": website }), function (result) {
        //选中
        $.each(result, function (j, item) {
            for (var i = 0; i < item.Children.length; i++) {
                if (item.Children[i] != null && treeObj.getNodeByParam("id", item.Children[i].Id, null) != null && treeObj.getNodeByParam("id", item.Children[i].Id, null).isParent == false) {
                    treeObj.checkNode(treeObj.getNodeByParam("id", item.Children[i].Id, null), true, true);
                }
            }
        });
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
} ;