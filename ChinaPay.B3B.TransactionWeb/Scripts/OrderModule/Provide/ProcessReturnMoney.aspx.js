function InputReason()
{
    //$(".fixed,.layers").show();
    $("#DenyTrigger").click();
}
function Cancle()
{
    // $(".fixed,.layers").hide();
    $(".close").click();
}

function getDictionaryItems(typeId, callback)
{
    sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
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
        $("#txtDenyReason").val($(this).find("span").text()).removeClass("null");
    });

});
function CheckReasonInput()
{
    var reason = $.trim($("#txtReason").val());
    if (reason == "")
    {
        alert("请输入拒绝原因");
        $("#txtReason").select();
        return false;
    } else if (reason.length > 200)
    {
        alert("输入原因不能超过200字");
        $("#txtReason").select();
        return false;
    }
    return true;

}
