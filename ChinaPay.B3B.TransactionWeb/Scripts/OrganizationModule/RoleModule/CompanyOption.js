//选项
$(function () {
    $("#companyWorkInfoMatain .customNumber, #companyWorkInfoMatain .officeworkinfo,#companyWorkInfoMatain .fuzerenwoekinfo,#companyWorkInfoMatain .timeworkinfo,#companyWorkInfoMatain .companyworkinfo,#divDrawerCondition").hide();
    if ($("#hfdCompanyType").val() == "产品方") {
        $("#timeworkinfo").addClass("navType1Selected");
        $("#companyWorkInfoMatain .timeworkinfo").show();
    } else if ($("#hfdCompanyType").val() == "采购商") {
        $("#companyworkinfo").addClass("navType1Selected");
        $("#companyWorkInfoMatain .companyworkinfo").show();
    } else if ($("#hfdCompanyType").val() == "出票方") {
        $("#officeworkinfo").addClass("navType1Selected");
        $("#companyWorkInfoMatain .officeworkinfo").show();
    }
    $(".navType1 li a").click(function () {
        $(".navType1 li a").removeClass("navType1Selected");
        $("#companyWorkInfoMatain .customNumber, #companyWorkInfoMatain .officeworkinfo,#companyWorkInfoMatain .fuzerenwoekinfo,#companyWorkInfoMatain .timeworkinfo,#companyWorkInfoMatain .companyworkinfo,#divDrawerCondition").hide();
        $("." + $(this).attr("id")).show();
        $(this).addClass("navType1Selected");
        $("#navTip").html($(this).attr("cus"));
    });
});