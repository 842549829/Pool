 
/*
* 表单序列化 2json
*/
function paramString2obj(serializedParams) {
    var obj = {};
    function evalThem(str) {
        var attributeName = str.split("=")[0];
        var attributeValue = str.split("=")[1];
        if (!attributeValue) {
            return;
        }

        var array = attributeName.split(".");
        for (var i = 1; i < array.length; i++) {
            var tmpArray = Array();
            tmpArray.push("obj");
            for (var j = 0; j < i; j++) {
                tmpArray.push(array[j]);
            };
            var evalString = tmpArray.join(".");
            // alert(evalString);
            if (!eval(evalString)) {
                eval(evalString + "={};");
            }
        };

        eval("obj." + attributeName + "='" + attributeValue + "';");

    };

    var properties = serializedParams.split("&");
    for (var i = 0; i < properties.length; i++) {
        evalThem(properties[i]);
    };

    return obj;
}

$.fn.form2json = function () {
    var serializedParams = this.serialize();
    var obj = paramString2obj(serializedParams);
    return JSON.stringify(obj);
}

///获取URL参数
function getRequest() {
    var url = location.search; // 获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") > -1) {
        var pair = url.substr(1).split("&");
        for (var i = 0; i < pair.length; i++) {
            theRequest[pair[i].split("=")[0]] = decodeURI(pair[i].split("=")[1]);
        }
    }
    return theRequest;
}

function sendPostRequest(targetUrl, parameters, successCallback, errorCallback) {
    $.ajax({
        type: "POST",
        url: targetUrl,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: successCallback,
        error: function (e) {
            if (e.status == 300) {
                if (JSON.parse(e.responseText) == "RequireLogon") {
                    window.location.href = "/Logon.aspx";
                    return;
                } else if (JSON.parse(e.responseText) == "Unauthorized") {
                    window.location.href = "/StaticHtml/NoAccess.aspx";
                    return;
                }
            }
            if (errorCallback) {
                errorCallback(e);
            }
        }
    });
}
 