$(function () { 
    $("#btnQuery").click(function () {
        if ($("#txtDeparture_ddlAirports").val() == "") {
            alert("执行被取消,请先选择一个有效的出发城市！");
            return false;
        }
        if ($("#txtArrival_ddlAirports").val() == "") {
            alert("执行被取消,请先选择一个有效的到达城市！");
            return false;
        }
        return true;
    });
});