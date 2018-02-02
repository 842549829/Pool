
$(document).ready(function () {
    var website = $("input[name='website']:checked").val();
    $("input[name='website']").click(function () {
        website = $(this).val();
        //初始化菜单
        $.fn.zTree.init($("#tree"), setting);
    });
    function getAsyncUrl(treeId, treeNode) {

        if (treeNode == null) {
            //加载主菜单
            return "/SystemMenuHandlers/MenusQuery.ashx/QueryFristMenus?website=" + website;
        }
        else if (treeNode.level == 1) {
            //加载子菜单
            return ("/SystemMenuHandlers/MenusQuery.ashx/QuerySecondaryMenus?id=" + treeNode.id);
        } else if (treeNode.level == 2) {
            //加载资源菜单
            return ("/SystemMenuHandlers/MenusQuery.ashx/QueryResources?id=" + treeNode.id);
        }
    };
    //加载菜单
    var setting = {
        view: {
            selectedMulti: false
        },
        async: {
            enable: true,
            url: getAsyncUrl,
            dataType: "json"
        },
        callback: {
            onAsyncSuccess: zTreeOnAsyncSuccess,
            onAsyncError: onAsyncError,
            onClick: zTreeOnClick,
            onExpand: zTreeOnExpand,
            onCollapse: zTreeOnCollapse,
            beforeRemove: zTreeBeforeRemove,
            beforeEditName: zTreeBeforeEditName,
            beforeDrag: zTreeBeforeDrag
        },
        edit: {
            enable: true,
            showRenameBtn: setRenameBtn,
            showRemoveBtn: setRemoveBtn,
            removeTitle: "删除节点",
            renameTitle: "编辑节点名称"
        },
        data: {
            key: {
                title: "title"
            }
        }
    };
    $("#btn_modify").hide();
    $(".class_url").hide();
    $("#btn_save").show();
    $("input[name='rad']").attr("disabled", "disabled");

    //初始化菜单
    $.fn.zTree.init($("#tree"), setting);

    //修改菜单
    $("#btn_modify").click(function () {
        if ($("#txt_remark").val().length >= 100) {
            $("#txt_remark").val().substring(0, 100);
            alert("备注信息不能超过100个字符!");
            return;
        }
        //得到树
        var treeObj = $.fn.zTree.getZTreeObj("tree");
        var nodes = treeObj.getSelectedNodes();
        var menuid = $("#menuid").html();
        var menuname = $("#txt_menu").val();
        var seq = $("#txt_seq").val();
        var remark = $("#txt_remark").val();
        var menuUrl = $("#txt_url").val();
        var enable = $("#radEnable").is(":checked");
        var display = $("#radShow").is(":checked");

        if (!Validate(nodes, menuname, seq, remark, menuUrl, "modify")) {
            return;
        }
        var condition;
        var parameters;
        var address;
        if (nodes[0].level == 1) {
            //修改主菜单
            condition = { "Name": menuname, "SortLevel": seq, "Remark": remark, "Valid": enable, "Display": display };
            parameters = JSON.stringify({ "menuId": menuid, "menuView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/UpdateMenu";
        } else if (nodes[0].level == 2) {
            //修改子菜单
            condition = { "Name": menuname, "SortLevel": seq, "Remark": remark, "Valid": enable, "Address": menuUrl, "Display": display };
            parameters = JSON.stringify({ "subMenuId": menuid, "subMenuView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/UpdateSubMenu";
        } else if (nodes[0].level == 3) {
            //修改资源页菜单
            condition = { "Name": menuname, "Remark": remark, "Valid": enable, "Address": menuUrl, "Display": display };
            parameters = JSON.stringify({ "resouceId": menuid, "resourceView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/UpdateResource";
        }
        MenuInfoMethod(parameters, address);
    });


    //添加菜单
    $("#btn_save").click(function () {
        if ($("#txt_remark").val().length >= 100) {
            $("#txt_remark").val().substring(0, 100);
            alert("备注信息不能超过100个字符!");
            return;
        }
        //得到树
        var treeObj = $.fn.zTree.getZTreeObj("tree");
        var nodes = treeObj.getSelectedNodes();
        var menuid = $("#menuid").html();
        var menuname = $("#txt_menu").val();
        var seq = $("#txt_seq").val();
        var remark = $("#txt_remark").val();
        var menuUrl = $("#txt_url").val();
        var display = $("#radShow").is(":checked");
        if (!Validate(nodes, menuname, seq, remark, menuUrl, "save")) {
            return;
        }
        var condition;
        var parameters;
        var address;
        if (nodes[0].level == 0) {
            //添加主菜单
            condition = { "Name": menuname, "SortLevel": seq, "Remark": remark, "Valid": "True", "Display": display };
            parameters = JSON.stringify({ "website": website, "menuView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/RegisterMenu";
        } else if (nodes[0].level == 1) {
            //添加子菜单
            condition = { "Name": menuname, "SortLevel": seq, "Remark": remark, "Valid": "True", "Address": menuUrl, "Display": display };
            parameters = JSON.stringify({ "website": website, "pid": menuid, "subMenuView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/RegisterSubMenu";
        } else if (nodes[0].level == 2) {
            //添加资源页菜单
            condition = { "Name": menuname, "Remark": remark, "Valid": "True", "Address": menuUrl, "Display": display };
            parameters = JSON.stringify({ "subMenu": menuid, "resourceView": condition });
            address = "/SystemMenuHandlers/MenusQuery.ashx/RegisterResource";
        }

        MenuInfoMethod(parameters, address);

    });
});
//验证
function Validate(nodes, menuname, seq, remark, menu_url, falg) {
    var regtxtSEQ = /(^-?[1-9]\d*)$/;
    var regtxtUrl = /^[A-Za-z0-9_,.\/=?&]+$/;
    var regtxtMenu = /^([\u4E00-\u9FA5A-Za-z0-9\/]+$)/;
    var regtxtRemark = /^[\u4E00-\u9FA5A-Za-z0-9_,.，、。\/]+$/;
    if (menuname.length < 2) {
        $("#menuName").html("<span style='Color:Red;'>长度至少要大于2</span>");
        $("#txt_menu").val("");
        $("#txt_menu").focus();
        return false;
    }
    if (!regtxtMenu.test(menuname)) {
        $("#menuName").html("<span style='Color:Red;'>格式不正确，只能输入汉字、英文、数字、/\</span>");
        $("#txt_menu").val("");
        $("#txt_menu").focus();
        return false;
    }
    $("#menuName").html("<span style='Color:Green;'>√</span>");

    if (!regtxtSEQ.test(seq) && $("#seqdisplay").css("display") != "none") {
        $("#Seq").html("<span style='Color:Red;'>格式不正确，只能输入大于零的整数</span>");
        $("#txt_seq").val("");
        $("#txt_seq").focus();
        return false;
    }
    $("#Seq").html("<span style='Color:Green;'>√</span>");
    if (nodes[0].level != 0 && falg == "save" && $("#urldisplay").css("display") != "none") {
        if (!regtxtUrl.test(menu_url)) {
            $("#Url").html("<span style='Color:Red;'>格式不正确，只能输入英文 数字 _ /\ , .= ? &   </span>");
            $("#txt_url").val("");
            $("#txt_url").focus();
            return false;
        }
        $("#Url").html("<span style='Color:Green;'>√</span>");
    }
    if (nodes[0].level > 1 && falg == "modify" && $("#urldisplay").css("display") != "none") {
        if (!regtxtUrl.test(menu_url)) {
            $("#Url").html("<span style='Color:Red;'>格式不正确，只能输入英文 数字 _ /\ , . = ? &  </span>");
            $("#txt_url").val("");
            $("#txt_url").focus();
            return false;
        }
        $("#Url").html("<span style='Color:Green;'>√</span>");
    }
    if (remark.length > 0 && !regtxtRemark.test(remark)) {
        $("#Remark").html("<span style='Color:Red;'>格式不正确，只能输入中文 英文 数字 _,.，。/\ </span>");
        $("#txt_remark").val("");
        $("#txt_remark").focus();
        return false;
    }
    $("#Remark").html("<span style='Color:Green;'>√</span>");
    return true;
}
//调用菜单的方法
function MenuInfoMethod(parameters, address) {
    sendPostRequest(address, parameters, function (e) {
        window.location.href = window.location.href;
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert(e.statusText);
        }
    });
}
//不显示删除按钮
function setRemoveBtn(treeId, treeNode) {
    if (treeNode.id == "0") {
        return false;
    } else {
        return true;
    }
};
//不显示删除按钮
function setRenameBtn(treeId, treeNode) {
    if (treeNode.id == "0") {
        return false;
    } else {
        return true;
    }
};
//异步加载成功
function zTreeOnAsyncSuccess(event, treeId, treeNode, msg) {
    if (treeNode == null) {
        //得到树
        var treeObj = $.fn.zTree.getZTreeObj("tree");
        var nodes = treeObj.getNodes();
        if (nodes.length > 0) {
            treeObj.selectNode(nodes[0]);
        }
    }
};
//异步加载失败
function onAsyncError(event, treeId, treeNode, XMLHttpRequest, textStatus, errorThrown) {
    alert("菜单加载失败,请稍后再试...");
}
//菜单节点 点击事件
function zTreeOnClick(event, treeId, treeNode) {
    $("#menuid").html(treeNode.id);
    if (treeNode.level == 3) {
        //赋值
        GetTreeNodeValue(treeNode);
    } else {
        //清空
        ClaerTextValue(treeNode);
    }
};
//节点展开事件
function zTreeOnExpand(event, treeId, treeNode) {
    $.fn.zTree.getZTreeObj("tree").selectNode(treeNode);
    $("#menuid").html(treeNode.id);

    //清空
    ClaerTextValue(treeNode);
};
//节点关闭事件
function zTreeOnCollapse(event, treeId, treeNode) {
    $.fn.zTree.getZTreeObj("tree").selectNode(treeNode);
    $("#menuid").html(treeNode.id);
    //清空
    ClaerTextValue(treeNode);
};


//删除节点事件 
function zTreeBeforeRemove(treeId, treeNode) {
    var parameters;
    var address;
    if (treeNode.level != 3) {
        if (confirm('你删除的是父级菜单\r将会删除 [ ' + treeNode.name + ' ] 下的所有子菜单\r\r是否要继续删除?')) {
            if (treeNode.level == 1) {
                //删除主菜单
                parameters = JSON.stringify({ "menu": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteMenu";
                MenuInfoMethod(parameters, address);
            } else if (treeNode.level == 2) {
                //删除子菜单
                parameters = JSON.stringify({ "subMenu": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteSubMenu";
                MenuInfoMethod(parameters, address);

            } else if (treeNode.level == 3) {
                //删除资源页菜单
                parameters = JSON.stringify({ "resource": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteResource";
                MenuInfoMethod(parameters, address);
            }
            return false;
        }
    } else {
        if (confirm('是否要删除菜单： ' + treeNode.name + " ?")) {
            if (treeNode.level == 1) {
                //删除主菜单
                parameters = JSON.stringify({ "menu": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteMenu";
                MenuInfoMethod(parameters, address);
            } else if (treeNode.level == 2) {
                //删除子菜单
                parameters = JSON.stringify({ "subMenu": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteSubMenu";
                MenuInfoMethod(parameters, address);

            } else if (treeNode.level == 3) {
                //删除资源页菜单
                parameters = JSON.stringify({ "resource": treeNode.id });
                address = "/SystemMenuHandlers/MenusQuery.ashx/DeleteResource";
                MenuInfoMethod(parameters, address);
            }
            return false;
        }
    }

    return false;
}

//禁止拖动
function zTreeBeforeDrag(treeId, treeNodes) {
    return false;
};
//修改事件
function zTreeBeforeEditName(treeId, treeNode) {
    $.fn.zTree.getZTreeObj("tree").selectNode(treeNode);
    //赋值
    GetTreeNodeValue(treeNode);
    return false;
}

function GetTreeNodeValue(treeNode) {
    $("#btn_save").hide();
    $("#btn_modify").show();
    $("input[name='rad']").removeAttr("disabled");
    //赋值
    $("#menuid").html(treeNode.id);
    $("#txt_menu").val(treeNode.name);
    $("#txt_url").val(treeNode.meun_url);
    $("#txt_seq").val(treeNode.seq);
    $("#txt_remark").val(treeNode.remark);
    if (treeNode.statu) {
        $("#radEnable").attr("checked", "checked");
    } else {
        $("#radDisEnable").attr("checked", "checked");
    }
    if (treeNode.display) {
        $("#radShow").attr("checked", "checked");
    } else {
        $("#radDisplay").attr("checked", "checked");
    }
    if (treeNode.level == 3) {
        $("#seqdisplay").css("display", "none");
        $("#showOrDisplay").css("display", "none");
    } else {
        $("#seqdisplay").css("display", "");
        $("#showOrDisplay").css("display", "");
    }
    if (treeNode.level == 1) {
        $("#urldisplay").css("display", "none");
    } else {
        $("#urldisplay").css("display", "");
    }
    $("#menu_tips").html("修改系统菜单:");
    $("#menuName").html("请输入菜单名");
    $("#Seq").html("请输入排序次序");
    $("#Url").html("请输入连接地址");
    $("#Remark").html("描述可以为空");
}
function ClaerTextValue(treeNode) {
    if (treeNode.level == 2) {
        $("#seqdisplay").css("display", "none");
    } else {
        $("#seqdisplay").css("display", "");
    }
    $("#showOrDisplay").css("display", "");
    if (treeNode.level == 0) {
        $("#urldisplay").css("display", "none");
    } else {
        $("#urldisplay").css("display", "");
    }
    $("#btn_modify").hide();
    $("#btn_save").show();
    $("#radEnable").attr("checked", "checked");
    $("#radShow").attr("checked", "checked");
    $("input[name='rad']").attr("disabled", "disabled");
    //清空
    $("input[type='text']").val("");
    $("#txt_remark").val("");
    $("#menu_tips").html("添加系统菜单:");
    $("#menuName").html("请输入菜单名");
    $("#Seq").html("请输入排序次序");
    $("#Url").html("请输入连接地址");
    $("#Remark").html("描述可以为空");
}