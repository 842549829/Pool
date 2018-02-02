<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Voyage.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.UserControls.Voyage" %>
<div class="column" id="voyages" style="position:relative;z-index:1;"><h3 class="titleBg"><asp:Label runat="server" ID="lblTip" Text="航班信息"></asp:Label></h3><div runat="server" id="divFlight" class="table"></div></div>

    <div class="tgq_box hidden">
        <h2>退改签规定</h2>
        <div class="tgq_bd" id="Tip">
            <p><span class="b">退票规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的5%。</p>
            <p><span class="b">更改规定：</span>航班规定离站时间（含）前，免费；航班规定离站时间后，收取票面价的10%。</p>
            <p><span class="b">签转规定：</span>允许。</p>
        </div>
    </div>

<script type="text/javascript">
    var loaded = false;
    $(function ()
        {
        if ($(".tgq_box").size() > 1)
        {
            $(".tgq_box").last().remove();
        }
        $(".tgq_box").appendTo($("body"));
        if (!loaded) LoadTipEvents();
    });
    function LoadTipEvents()
    {
        loaded = true;
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
