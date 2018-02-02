$(function () {
    //类型切换
    if ($("#ddlRoleType option:selected").text() == "出票方") $("#dl_ddlAccountType li:contains('个人')").hide();
    $("#ddlRoleType").removeClass("custed").custSelect({ width: "78px", 
        callback: function () {
            $("#dl_ddlAccountType li:first").trigger("click");
             if ($("#ddlRoleType option:selected").text() == "出票方") {
                 $("#dl_ddlAccountType li:contains('个人')").hide();
             } else{
                 $("#ddlAccountType").removeClass("custed").custSelect({ width: "65px" });
             }
       }
     });
     $("#ddlAccountType").removeClass("custed").custSelect({ width: "65px" });
});