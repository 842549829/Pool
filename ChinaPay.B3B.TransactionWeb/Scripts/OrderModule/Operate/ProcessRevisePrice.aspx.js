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
        return false;
    }
    return true;
}
var totalReleasePrice = "";
var totalAirPortFee = "";

$(CalcTotal);
$(function ()
{
    $("#PriceInputContainer input").blur(CalcTotal);
    totalReleasePrice = parseFloat($("#totalReleasePrice").text());
    totalAirPortFee = parseFloat($("#totalAirPortFee").text());
});

function CalcTotal()
{
    var fee = 0;
    var airportFee = 0;
    $("#PriceInputContainer .releasePrice").each(function (index, item)
    {
        fee += parseFloat($(item).val());
    });
    $("#PriceInputContainer .airportFee").each(function (index, item)
    {
        airportFee += parseFloat($(item).val());
    });
    $("#totalReleasePrice").text(fee);
    $("#totalAirPortFee").text(airportFee);
}

///检查票面价和民航基金输入内容是否正确
function CheckPrice()
{
    var flag = true;
    var priceReg = /^\d+\.?\d*$/;
    $("#PriceInputContainer .releasePrice").each(function (index, item)
    {
        var that = $(item);
        var releasePrice = that.val();
        if (releasePrice == "")
        {
            alert("请输入票面价！");
            that.select();
            flag = false;
            return false; ;
        }
        if (!priceReg.test(releasePrice))
        {
            alert("票面价格式不正确");
            that.select();
            flag = false;
            return false;
        }
    });
    $("#PriceInputContainer .airportFee").each(function (index, item)
    {
        var that = $(item);
        var releasePrice = that.val();
        if (releasePrice == "")
        {
            alert("请输民航基金！");
            that.select();
            flag = false;
            return false; ;
        }
        if (!priceReg.test(releasePrice))
        {
            alert("民航基金格式不正确");
            that.select();
            flag = false;
            return false; ;
        }
    });
    if (totalReleasePrice != $("#totalReleasePrice").text())
    {
        alert("票面总价不正确");
        flag = false;
    }
    if (totalAirPortFee != $("#totalAirPortFee").text())
    {
        alert("民航基金总价不正确");
        flag = false;
    }
    return flag;
}