var js;
var currentNOId;
function EditTick(id)
{
    var NO = $("#" + id).html();
    currentNOId = id;
    js = NO.substring(0, 3);
    $("#oldTicket").html(NO.substring(4));
    $(".js").html(js);
    $("#newJs").val(js);
    if (/\d+-\d+-\d+/.test(NO)) $("#mutilNums").show();
    $("#BeginEditTicket").click();
}

function updateTicketNo()
{
    var editRow = $("#EditTicket");
    var originalTicketNo = $("#oldTicket", editRow).html();
    var newTicketNo = $.trim($("#newTicket", editRow).val());
    var newJs = $("#newJs", editRow).val();
    var ticketNoEndCtl = $("#ticketNoEnd");
    var endFix = $.trim(ticketNoEndCtl.val());
    if (newJs == "")
    {
        alert("请输入结算码");
        $("#newJs", editRow).select();
        return;
    }
    if (!/^\d{3}$/.test(newJs))
    {
        alert("结算码格式不正确");
        $("#newJs", editRow).select();
        return;
    }
    if (newTicketNo == '')
    {
        alert("请输入新票号");
        $("#newTicket", editRow).select();
        return;
    } else if (newTicketNo + "-" + endFix == originalTicketNo)
    {
        alert("新票号不能与原票号相同");
        $("#newTicket", editRow).select();
        return;
    } else if (!(/^[0-9]{10}$/).test(newTicketNo))
    {
        alert("新票号格式错误，只能为10位数字");
        $("#newTicket", editRow).select();
        return;
    }
    var endfixNumber = 0;
    if (ticketNoEndCtl.is(":visible"))
    {
        if (endFix == "")
        {
            alert("请输入后续票号后缀");
            ticketNoEndCtl.select();
            return;
        } else if (!/^\d{2}$/.test(endFix))
        {
            alert("票号后缀格式错误，请输入两位数字");
            ticketNoEndCtl.select();
            return;
        }
        endfixNumber = parseInt(endFix, 10);
    }
    var number = parseInt(newTicketNo, 10);
    var numbers = [];
    if (endfixNumber != 0)
    {
        var lastNumber = number - (number % 100) + endfixNumber;
        if (number % 100 > endfixNumber)
        {
            lastNumber += 100;
        }
        for (var i = number; i <= lastNumber; i++)
        {
            numbers.push(i.toString());
        }
    } else
    {
        numbers.push(newTicketNo);
    }
    var orderId = $.trim($("#lblOrderId").html());
    var parameters = JSON.stringify({ "orderId": orderId, "originalTicketNo": originalTicketNo, "newTicketNo": numbers, "isPlatform": false, settleCode: newJs });
    setAllButtonEnable(false);
    sendPostRequest("../../OrderHandlers/Order.ashx/UpdateTicketNo", parameters,
        function (e)
        {
            $("#" + currentNOId).html(newJs + "-" + newTicketNo + (endFix == "" ? "" : "-") + endFix);
            $(".close").click();
            setAllButtonEnable(true);
            //cancelEditTicketNo(sender);
        }, function (e)
        {
            if (e.status == 300)
            {
                alert(JSON.parse(e.responseText));
            } else
            {
                alert("系统故障，请联系平台技术人员");
            }
            setAllButtonEnable(true);
        });
}
function setAllButtonEnable(enable)
{
    if (enable)
    {
        $(".btn2,btn1").removeAttr("disabled");
    } else
    {
        $(".btn2,btn1").attr("disabled", "disabled");
    }
}

$(function ()
{
    if ($("#hidRenderPrice").val() != "1")
    {
        $(".ticketPrice").text("");
    }
});