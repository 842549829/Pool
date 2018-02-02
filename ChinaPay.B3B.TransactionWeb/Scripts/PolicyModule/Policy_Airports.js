
//始发地左右筛选数据
function AirportsArrivals() {
    $("#lbSource").live("dblclick", function () {
        addAirport($(this), $(this).next("select"), $("#txtAirports", $(this).parents("td")));
    });
    $("#lbSelected").live("dblclick", function () {
        removeAirport($(this).prev("select"), $(this), $("#txtAirports", $(this).parents("td")));
    });
    $("#txtAirports").live("blur", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports").live("change", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports").live("keydown", function () {
        if (event.keyCode == 13) {
            matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
            return false;
        } else {
            return filterAirport(event);
        }
    }); 
    $("#btnAddAll").live("click", function () {
        addAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnAdd").live("click", function () {
        addAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemove").live("click", function () {
        removeAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemoveAll").live("click", function () {
        removeAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    matchAirports($("#lbSource"), $("#lbSelected"), $("#txtAirports"));
}
//目的地左右筛选数据
function AirportsDepartures() {
    $("#lbSource1").live("dblclick", function () {
        addAirport($(this), $(this).next("select"), $("#txtAirports1", $(this).parents("td")));
    });
    $("#lbSelected1").live("dblclick", function () {
        removeAirport($(this).prev("select"), $(this), $("#txtAirports1", $(this).parents("td")));
    });
    $("#txtAirports1").live("blur", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports1").live("change", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports1").live("keydown", function () {
        if (event.keyCode == 13) {
            matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
            return false;
        } else {
            return filterAirport(event);
        }
    });
    $("#btnAddAll1").live("click", function () {
        addAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnAdd1").live("click", function () {
        addAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemove1").live("click", function () {
        removeAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemoveAll1").live("click", function () {
        removeAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    matchAirports($("#lbSource1"), $("#lbSelected1"), $("#txtAirports1"));
}
//中转左右筛选数据
function AirportsZhongZhuanDepartures() {
    $("#lbSource2").live("dblclick", function () {
        addAirport($(this), $(this).next("select"), $("#txtAirports2", $(this).parents("td")));
    });
    $("#lbSelected2").live("dblclick", function () {
        removeAirport($(this).prev("select"), $(this), $("#txtAirports2", $(this).parents("td")));
    });
    $("#txtAirports2").live("blur", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports2").live("change", function () {
        matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
    });
    $("#txtAirports2").live("keydown", function () {
        if (event.keyCode == 13) {
            matchAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this));
            return false;
        } else {
            return filterAirport(event);
        }
    });
    $("#btnAddAll2").live("click", function () {
        addAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnAdd2").live("click", function () {
        addAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemove2").live("click", function () {
        removeAirport($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    $("#btnRemoveAll2").live("click", function () {
        removeAllAirports($(this).parents("td").find("select:first"), $(this).parents("td").find("select:last"), $(this).parents("td").find(":text"));
        return false;
    });
    matchAirports($("#lbSource2"), $("#lbSelected2"), $("#txtAirports2"));
}