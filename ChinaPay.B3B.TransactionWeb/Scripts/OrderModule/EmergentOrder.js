$(function () {
    $(".tips_btn").live("mouseenter", function () {
        var self = $(this);
        var layer = self.next();
        if (!layer.hasClass("tips_box")) {
            sendPostRequest("/OrderHandlers/Order.ashx/QueryEmergentOrder", JSON.stringify({ "id": self.attr("id"), "status": self.attr("status") }),
            function (result) {
                self.after($(result));
                layer = self.next();
                setLayerlocation();
            }, function () {});
        } else {
            setLayerlocation();
        }
        function setLayerlocation() {
            var selfWidth = self.width();
            var selfHeight = self.height();
            var selfOffsetLeft = self.offset().left;
            var selftOffsetTop = self.offset().top;
            var layerWidth = layer.width();
            var layerHeight = layer.height();
            var winWidth = 0, winHeight = 0;
            if (window.innerWidth) {
                winWidth = window.innerWidth;
            }
            else if ((document.body) && (document.body.clientWidth)) {
                winWidth = document.body.clientWidth;
            }
            if (window.innerHeight) {
                winHeight = window.innerHeight;
            }
            else if ((document.body) && (document.body.clientHeight)) {
                winHeight = document.body.clientHeight;
            }
            layer.removeClass("hidden");
            layer.css("left", selfOffsetLeft - layerWidth / 2 + selfWidth / 2);
            if (winHeight > 0 && selftOffsetTop > winHeight / 2) {
                layer.css({ "top": selftOffsetTop - layerHeight - selfHeight }).addClass("tips_box1").removeClass("tips_box");
            } else {

            } layer.css("top", selftOffsetTop + selfHeight).addClass("tips_box").removeClass("tips_box1");
        }
    }).live("mouseleave", function () {
        var self = $(this).next().addClass("hidden");
    });
});