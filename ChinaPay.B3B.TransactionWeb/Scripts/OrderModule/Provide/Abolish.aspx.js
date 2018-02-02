function ShowReashoInput()
{
    $(".fixed,.layers").show();
    return false;
}
function CancleInput()
{
    $(".fixed,.layers").hide();
}
function CheckReason()
{
    var reason = $.trim($("#Reason").val());
    if (reason == "")
    {
        alert("请输入拒绝原因");
        $("#Reason").select();
        return false;
    } else if (reason.length > 200)
    {
        alert("拒绝原因不能超过200字");
        $("#Reason").select();
        return false;
    } else if ($("#divDeny .check :checked").size() == 0)
    {
        alert("请选择拒绝类型");
        return false;
    }
    return true;
}
$(function ()
{
    $("#divDeny input[type='radio']").click(function ()
    {
        $("#selDenyReason").empty();
        getDictionaryItems($(this).val(), function (rsp)
        {
            if (rsp)
            {
                for (var i in rsp)
                {
                    $("<option>" + rsp[i].Remark + "</option>").appendTo("#selDenyReason");
                }
                $("#dl_selDenyReason").remove();
                $("#selDenyReason").removeClass("custed").custSelect({ width: 326 });
            }
        });
    });

    $("#dl_selDenyReason li").live("click", function ()
    {
        $("#Reason").val($(this).find("span").text()).removeClass("null");
    });


    $(".FeeInput,.faxInput", $(".Passengers")).add(".RefundTotlal,.RefundFee").attr("readonly", "readonly").css({ border: "none" });
});