var url = decodeURI(window.location.href);

//加载菜单
var setting = {
    simpleData: {
        enable: true
    },
    callback: {

}, check: {
    enable: true,
    chkStyle: "checkbox",
    chkboxType: { "Y": "ps", "N": "ps" }
}
};


$(document).ready(function () {
    var id = url.substring(url.indexOf('?CompanyId=') + 11, url.indexOf('&'));
    //调用菜单的方法
    var action_url = "../../PermissionHandlers/MenusQuery.ashx/QueryCompanyMaxValidMenus";
    var action_name = "xinzeng";
    var parame = { "id": id };
    MenuInfoMethod(action_url, parame, action_name);

    action_url = "../../PermissionHandlers/MenusQuery.ashx/QueryCompanyMaxValidForbiddenMenus";
    action_name = "jinzhi";
    parame = { "id": id };
    MenuInfoMethod(action_url, parame, action_name);

    $("#btnSave").click(function () {
        //得到树
        var treeObj = $.fn.zTree.getZTreeObj("disenable_tree");
        var str_list = GetCheckValue(treeObj);
        action_url = "../../PermissionHandlers/MenusQuery.ashx/SaveCompanyForbiddenPermission";
        action_name = "modify_jinzhi";
        parame = { "company": id, "menuViews": str_list };
        MenuInfoMethod(action_url, parame, action_name);
        //得到树
        treeObj = $.fn.zTree.getZTreeObj("add_tree");
        str_list = GetCheckValue(treeObj);
        action_url = "../../PermissionHandlers/MenusQuery.ashx/SaveCompanyAllowablePermission";
        action_name = "modify_add";
        parame = { "company": id, "menuViews": str_list };
        MenuInfoMethod(action_url, parame, action_name);
    });
    $("#btnReturn").click(function () {
        window.location.href = "../TerraceModule/CompanyInfoManage/CompanyList.aspx?Search=Back";
    });
});
function GetCheckValue(treeObj) {
    var str_list = new Array();
    var z_children = null;
    if (treeObj != null) {
        //取得所有选中的
        var nodes = treeObj.getCheckedNodes(true);
    }
    for (var i = 1; i < nodes.length; i++) {
        z_children = new Array();
        if (nodes[i].children != null) {
            for (var j = 0; j < nodes[i].children.length; j++) {
                if (nodes[i].children[j].checked) {
                    z_children.push({ "Id": nodes[i].children[j].id, "Name": nodes[i].children[j].name });
                }
            }
        }
        str_list.push({ "Id": nodes[i].id, "Name": nodes[i].name, "Children": null });
        if (z_children.length > 0) {
            str_list.push({ "Id": nodes[i].id, "Name": nodes[i].name, "Children": z_children });
        }
    }
    return str_list;
};
//查询菜单数据源
function MenuInfoMethod(action_url, parame, action_name) {
    sendPostRequest(action_url, JSON.stringify(parame), function (e) {
        //加载新增菜单
        if (action_name == "xinzeng") {
            //初始化菜单
            $.fn.zTree.init($("#add_tree"), setting, e);
            if (url.indexOf("?CompanyId=") != -1) {
                //得到树
                var treeObj = $.fn.zTree.getZTreeObj("add_tree");
                action_url = "../../PermissionHandlers/MenusQuery.ashx/QueryCompantValidMenusOfCompanyId";
                loadRoleInfo(treeObj, action_url, action_name);
            }
        }
        //加载禁止菜单
        if (action_name == "jinzhi") {
            //初始化菜单
            $.fn.zTree.init($("#disenable_tree"), setting, e);
            if (url.indexOf("?CompanyId=") != -1) {
                //得到树
                var treeObj = $.fn.zTree.getZTreeObj("disenable_tree");
                action_url = "../../PermissionHandlers/MenusQuery.ashx/QueryCompanyMaxValidForbiddenMenusOfCompanyId";
                loadRoleInfo(treeObj, action_url, action_name);
            }
        }
        //修改设置权限
        if (action_name == "modify_jinzhi") {
            alert("设置成功");
            window.location.href = "../TerraceModule/CompanyInfoManage/CompanyList.aspx?Search=Back";
        }
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
};
function loadRoleInfo(treeObj, action_url, action_name) {
    //修改绑定已有的菜单 
    //得到传过来的id
    var CompanyId = url.substring(url.indexOf("?CompanyId=") + 11, url.indexOf("&"));
    sendPostRequest(action_url, JSON.stringify({ "companyId": CompanyId }), function (result) {
        //绑定新增的菜单选中
        if (action_name == "xinzeng") {
            //选中
            $.each(result.children, function (j, itemchildren) {
                $.each(itemchildren.children, function (j, item) {
                    if (item != null && treeObj.getNodeByParam("id", item.id, null) != null && treeObj.getNodeByParam("id", item.id, null).isParent == false) {
                        treeObj.checkNode(treeObj.getNodeByParam("id", item.id, null), true, true);
                    }
                });
            });
        }
        //绑定禁止的菜单选中
        if (action_name == "jinzhi") {
            //选中
            $.each(result.children, function (j, itemchildren) {
                $.each(itemchildren.children, function (j, item) {
                    if (item != null && treeObj.getNodeByParam("id", item.id, null) != null && treeObj.getNodeByParam("id", item.id, null).isParent == false) {
                        treeObj.checkNode(treeObj.getNodeByParam("id", item.id, null), true, true);
                    }
                });
            });
        }
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
};