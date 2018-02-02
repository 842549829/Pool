//左右筛选数据
function Airports() {
    $("#lbSource").live("dblclick", function () {
        addAirport($(this), $(this).next("select"), $("#txtAirports", $(this).parents("td")));
    });
    $("#lbSelected").dblclick(function () {
        removeAirport($(this).prev("select"), $(this), $("#txtAirports", $(this).parents("td")));
    });
    $("#txtAirports").blur(function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports").keydown(function () {
        if (event.keyCode == 13) {
            matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
            return false;
        } else {
            return filterAirport(event);
        }
    });
    $("#btnAddAll").click(function () {
        addAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnAdd").click(function () {
        addAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemove").click(function () {
        removeAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemoveAll").click(function () {
        removeAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    matchAirports($("#lbSource"), $("#lbSelected"), $("#txtAirports"));
}