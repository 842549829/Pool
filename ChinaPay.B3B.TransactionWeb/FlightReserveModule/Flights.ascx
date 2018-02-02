<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Flights.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.Flights" %>
<div class="hd flightH"><h2 class="flightInfo1">航班信息</h2></div>
<div runat="server" id="divFlights" class="table flightInfoBox"></div>

    <div class="tgq_box hidden">
        <h2>退改签规定</h2>
        <div class="tgq_bd" id="Tip">
            <p><span class="b">退票规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的5%。</p>
            <p><span class="b">更改规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的10%。</p>
            <p><span class="b">签转规定：</span>允许。</p>
        </div>
    </div>
    
<script type="text/javascript">
    $(function ()
    {
        $(".tgq_box").appendTo($("body"));
        LoadTipEvents();
    });
    function LoadTipEvents()
    {
        $(".flightEI").live("mouseenter", function ()
        {
            var content = $(this).next().html();
            $("#Tip").html(content);
            $(".tgq_box").removeClass("hidden");
            $(".tgq_box").css("left", $(this).offset().left - 125);
            $(".tgq_box").css("top", $(this).offset().top + 15);
            var h = $(".tgq_box").height();
            var top = $(".tgq_box").offset().top;
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if (h + top - sor > wh)
            {
                $(".tgq_box").css({ top: (top - h - 35) });
                $(".tgq_box").addClass("tgq_box1").removeClass("tgq_box");
            };
        }).live("mouseleave", function ()
        {
            $(".tgq_box1").addClass("tgq_box").removeClass("tgq_box1");
            $(".tgq_box").addClass("hidden");
        });
    }


</script>
