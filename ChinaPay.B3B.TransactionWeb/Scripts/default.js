$(function () {
    $('.s-idtabs').tabs({
        event: "click",
        selected: "active btn class1",
        callback: function (i) { }
    });
    loadSpecialProduct();
});
function loadSpecialProduct() {
    var specialProduct = $("#divSpecialProduct");
    if (specialProduct) {
        sendPostRequest("", "", function(data) {
            var productHTML = '<ul>';
            $(data).each(function(index, item) {
                if (index % 2 == 0) {
                    productHTML += '<li>';
                } else {
                    productHTML += '<li style="margin:0;">';
                }
                productHTML += '<a href="#this"><span>[' + item.Date + ']</span>' + item.Departure + '&nbsp;-&nbsp;' + item.Arrival + '<span class="price">￥' + item.Fare + '</span></a></li>';
            });
            productHTML += '</ul>';
            specialProduct.html(productHTML);
        }, function(e) {
            specialProduct.html("加载特殊产品失败");
        });
    }
}