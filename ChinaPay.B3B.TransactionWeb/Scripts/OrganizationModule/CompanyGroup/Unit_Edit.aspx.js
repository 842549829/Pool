function Init(e, data, isfirst, index) {
    var container = $(this);
    var controls = {
        airPort: container.find(".txtAirports"),
        airLine: container.find(".airLine"),
        source: container.find(".source"),
        target: null, 
        btnAdd: container.find(".add"),
        btnAddAll: container.find(".addAll"),
        btnRemove: container.find(".remove"),
        btnRemoveAll: container.find(".removeAll"),
        allCheck: container.find(".checkAll"),
        checkOpposite:container.find(".checkOpposite"),
        deleteBtn: container.find(".deleteButton")
    };

    if (isfirst) {
        CreateTarget(controls.source);
        controls.btnAdd.click(function () { addAirport(controls.source, controls.target, controls.airPort); });
        controls.btnAddAll.click(function () { addAllAirports(controls.source, controls.target, controls.airPort); });
        controls.btnRemove.click(function () { removeAirport(controls.source, controls.target, controls.airPort); });
        controls.btnRemoveAll.click(function () { removeAllAirports(controls.source, controls.target, controls.airPort); });
        controls.airPort.blur(function () { matchAirports(controls.source, controls.target, controls.airPort); });
        controls.source.dblclick(function () { addAirport(controls.source, controls.target, controls.airPort); });
        controls.target.dblclick(function () { removeAirport(controls.source, controls.target, controls.airPort); });
        controls.airPort.keydown(function () {
            if (event.keyCode == 13) { matchAirports(controls.source, controls.target, controls.airPort); return false; }
            return filterAirport(event);
        });
        controls.allCheck.click(function () { container.find("input[type='checkbox']").attr("checked", $(this).is(":checked")); });
        controls.checkOpposite.click(function () {
            $("input", controls.airLine).each(function (itemIndex, domEle) {
                var cur = $(domEle);
                cur.is(":checked") ? cur.removeAttr("checked") : cur.attr("checked", "checked");
            });
        });
        controls.deleteBtn.click(function () {
            if ($(".Item").size() <= 1) {
                alert("至少保留1项设置！");
            } else {
                if (confirm("确定要删除此项设置吗？")) {
                    container.remove();
                    ControlPageHeight();
                }
            }
        });

        setLabelFor(controls.allCheck, "allCheck");
        setLabelFor(controls.checkOpposite, "checkOpposite");
    }

    if (data) {
        loadChecks(controls.airLine, data.AirlineCode);
        loadSelect(controls.source, data.DepartuesCode);
        loadSelect(controls.target, data.ForbiddenAirPorts);
        controls.airPort.val(data.FobiddenAirPort);
        container.data("Id", data.Id);

    } else {
        loadSelect(controls.source, DefaultAirPort);
        loadChecks(controls.airLine, DefautAirLine);
    }

    function CreateTarget(place) {
        var target = $("<select>").attr("multiple", "multiple").addClass("op_con")
            .addClass("op_con_r").addClass("target");
        controls.target = target;
        place.after(target);
    }

    function setLabelFor(target, id) {
        target.attr("id", id + index).next().attr("for", id + index);
    }

    function setRadioName(target, name) {
        target.attr("name", name + index);
    }
    function loadSelect(target, datas) {
        if (datas) {
            $.each(datas, function (index, element) {
                target.append("<option value='" + element.Code + "'>"
                            + element.Code + "-" + element.Name + "</option>");
            });
        }
    }
    function loadChecks(target, datas) {
        if (datas) {
            var columnIndex = 0;
            var columnCount = 6;
            var td = "<td>";
            var tdEnd = "</td>";
            var html = ["<tr>"];
            for (var dindex in datas) {
                if (columnIndex == columnCount) {
                    html.push("</tr>", "<tr>");
                    columnIndex = 0;
                }
                html.push(td, "<input type='checkbox'");
                html.push(" value='", datas[dindex].Code, "'");
                if (datas[dindex].Valid) html.push(" checked='checked' ");
                html.push(" id='chk_", datas[dindex].Code,index);
                html.push("'><label for='chk_", datas[dindex].Code,index, "'>", datas[dindex].Code, "</label>", tdEnd);
                columnIndex++;
            }
            for (var i = columnIndex; i < columnCount; i++) {
                html.push(td, "&nbsp;", tdEnd);
            }
            html.push("</tr>");
            target.html(html.join(""));
        }
    }
    
    //移除黄色背景的警告提示
    function clearAlert(radio) {
        $(radio).css("background-color", "white");
    }
        
    //创建锚记
    function CreateHash(host,idPrefix) {
        //$("<a id='" + idPrefix + index + "'>&nbsp;</a>").insertAfter(host.parent());
       // host.data("hash",idPrefix+index);
    }
}
var itemIndex = 1;
var DefautAirLine;
var DefaultAirPort;

function addForm(data) {
    var copy = $("#template").clone();
    copy.addClass("Item").appendTo("#divInfo");
    copy.trigger("Init", [data, true, itemIndex++]);
}


function hideSet() {
    $("#divInfo,#btnAddItem").hide();
    $("#divInfo").empty();
}
function showSet() { $("#divInfo,#btnAddItem").show(); if ($("#divInfo div").size() == 0) addForm(null, true, itemIndex++); }


function btnSave_onclick() {
    if (checking()) {
        var comapnyGroup =  $.makeArray(CollectItemDatas());
        var group = { "CompanyGroup": { "Company": $("#hidCompanyGroupId").val(),
            "Name": $("#txtGroupName").val(),
            "Description": $("#describe").val()
        },
            "CompanyGroupList": comapnyGroup

        };
        if (!isAdd) {
            group.Id = $("#hidId").val();
            group.CompanyGroup.Id = group.Id;
        } 
        var parameter = JSON.stringify({ "group": group }); 
        var action = isAdd ? "CreateComapnyGroup" : "UpdateComapnyGroup";
        sendPostRequest("/OrganizationHandlers/CompanyGroupBy.ashx/" + action, parameter, function (rsp) {
            if (rsp == true) {
                alert("保存成功");
                window.location.replace("/OrganizationModule/CompanyGroup/CompanyGroupList.aspx");
            } else {
                alert("保存失败");
            }
        }, function (rsp) {
            alert("保存失败");
        });
    }
}

//检查数据数据是否正确
function checking() {
    if ($.trim($("#txtGroupName").val()).length < 1) {
        $("#lblGroupName").text("名称不能为空");
        $("#txtGroupName").select();
        return false;
    } else {
        $("#lblGroupName").empty();
    }
    var point = true;
    var validatePass = true;
    if ($("#divInfo .Item").size() > 0) {
        $("#divInfo .Item").each(function () {
            var that = $(this);
            var Item = $(".airLine", that);
            if ($("td :checked", Item).size() == 0) {
                alert("至少选择一个航空公司！");
                validatePass = false;
                return false;
            }
            if ($(".txtAirports", that).val() == "") {
                alert("至少选择一个出港城市！");
                validatePass = false;
                return false;
            }
            return true;
        });
    }
    if (!validatePass) return false;
    return point;

}

//收集输入的限制设置
function CollectItemDatas() {
    var datas = $(".Item").map(function (i, dom) {
        var container = $(dom);
        var airlines = container.find(".airLine :checked").map(function (index, air) { return air.value; });
        var departures = container.find(".txtAirports").val();
        var result = { Airlines: $.makeArray(airlines).join("/"),
            Departures: departures
        };
        if (!isAdd) {
            result.Id = container.data("Id");
            result.Group = $("#hidId").val();
        }
        return result;
    });
    return datas;
}
function LoadDatas(rsp) {
    if (rsp) {
        $("#divInfo,#btnAddItem").show(); 
        $("#txtGroupName").val(rsp.Name);
        $("#describe").val(rsp.Description);
            for (var item in rsp.Limit) {
                addForm(rsp.Limit[item]);
            }
    }
}


$(function () {
    sendPostRequest("/OrganizationHandlers/CompanyGroupBy.ashx/QueryAirlines", {}, function (rsp) { DefautAirLine = rsp;
    sendPostRequest("/OrganizationHandlers/CompanyGroupBy.ashx/QueryAirports", {}, function (rsp) {
        DefaultAirPort = rsp;
        $(".form").live("Init", Init);
        if (!isAdd) {
            sendPostRequest("/OrganizationHandlers/CompanyGroupBy.ashx/QueryCompanyGroup", JSON.stringify({ id: $("#hidId").val() }), LoadDatas, $.noop);
        } else {
            showSet();
        }
    });
 });
    $("#describe").LimitLength(200);

});