window.onload = setIframeWidth;
function setIframeWidth() {
    var rightIframe = parent.document.getElementById("rightFrame");
    var tableWidth = document.getElementsByTagName("table")[1];
    var searchbk = document.getElementsByTagName("table")[0];
    var center = parent.document.getElementById("w2500");
    var divBanner = parent.document.getElementById("divBanner");
    if (rightIframe && tableWidth && searchbk) {
        divBanner.style.width = rightIframe.style.width = "2300px";
        center.style.width = "2500px";
        searchbk.style.width = "50%";
        window.onunload = function () {
            center.style.width =searchbk.style.width = divBanner.style.width = rightIframe.style.width = "100%";
        }
    }
} 