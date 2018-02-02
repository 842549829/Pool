//function SearchItem(obj, div, companyId, companyName, companyType, event) {
//    if ($.trim($("#" + obj).val()) != "") {
//        switch (event.keyCode) {
//            //向上方向键                  
//            case 38:
//                selPrev(div, companyId, companyName);
//                return;
//                //向下方向键
//            case 40:
//                selNext(div, companyId, companyName);
//                return;
//                //向右方向键
//            case 39:
//                //向左方向键
//            case 37:
//                //对应Alt键
//            case 18:
//                //对应CapsLock键
//            case 20:
//                return;
//                //对应回车键
//            case 13:
//                //fillCurrent(valueControl, sender);
//                return;
//                //对应Esc键
//            case 27:
//                hideCompanyTip(div);
//                return;
//            default:
//                bindCompanyinfo(obj, div, companyId, companyName, companyType);
//                index = 0;
//                return;
//        }
//        // bindCompanyinfo(obj);
//    } else {
//        $("#" + div).hide();
//    }

//}

////function SetTextvalue(obj, CompanyId, CompanyName, div) {
////    if ($.trim($("#" + obj).val()) != "") {
////        if ($("#" + div + "ul li").length == 0) {
////            $("#" + CompanyId).val("");
////            $("#" + CompanyName).val("");
////        } else {
////            $("#" + CompanyId).val($("#" + obj).parent().find(".obvious-a").attr("val"));
////            $("#" + CompanyName).val($("#" + obj).parent().find(".obvious-a").html().split('-')[0]);
////        }
////    } $("#" + div).hide();
////}
//var index = 0;
//function selPrev(div, companyId, companyName) {
//    if ($("#" + div).css("display") != "none") {
//        index = parseInt(index) - 1;
//        $("#" + div + " ul li").removeClass("obvious-a");
//        $("#" + div + " ul li").eq(index).addClass("obvious-a");

//        $("#" + CompanyId).val($("#" + div + " ul li").eq(index).attr("val"));
//        $("#" + CompanyName).val($("#" + div + " ul li").eq(index).html().split('-')[0]);
//        if (index <= 0) {
//            index = $("#" + div + " ul li").length;
//        }
//    }
//}
//function selNext(div, companyId, companyName) {
//    if ($("#" + div).css("display") != "none") {
//        index = parseInt(index) + 1;
//        if (index >= $("#" + div + " ul li").length) {
//            index = 0;
//        }
//        $("#" + companyId).val($("#" + div + " ul li").eq(index).attr("val"));
//        $("#" + companyName).val($("#" + div + " ul li").eq(index).html().split('-')[0]);
//        $("#" + div + " ul li").removeClass("obvious-a");
//        $("#" + div + " ul li").eq(index).addClass("obvious-a");
//    }
//}
//function hideCompanyTip(div) {
//    $("#" + div).hide();
//}
//function bindCompanyinfo(obj, div, companyId, companyName, companyType) {
//    sendPostRequest("/UserControlHandler/CompanyQuery.ashx/QueryCompanyInfo", JSON.stringify({ "companyName": $.trim($("#" + obj).val()), "companyType": $("#" + companyType).val() }), function (e) {
//        $("#" + div).html("");
//        var str = "<ul>";
//        $.each(e, function (i, item) {
//            str += "<li val='" + item.value + "' >" + item.text + "</li>";
//        });
//        str += "</ul>";
//        $("#" + div).append(str);
//        if (e.length == 0) {
//            $("#" + div).hide();
//        } else {
//            $("#" + div).show();
//        }
//        $("#" + div + " ul li").eq(0).addClass("obvious-a");
//    }, function (e) {
//        alert(e.responseText);
//    });
//}