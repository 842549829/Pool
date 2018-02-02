window.qs = new function() {
    qs = window.location.search.substring(1);
    if (qs.length > 0) {
        var parts = qs.split('&');
        for (var s in parts) {
            var pair = parts[s].split('=');
            var value = parts[s].substring(parts[s].indexOf('=') + 1);
            this[pair[0]] = value;
        }
    }
} ();
if (!window.Gathor) {
    window.Gathor = function(buffer) {
        var _inner = buffer instanceof Array ? buffer : [];
        this.sput = function(name, val) {
            _inner.push('"' + name + '":"' + val.toString() + '"');
        }
        this.put = function(name, val) {
            _inner.push('"' + name + '":' + val.toString() + '');
        }
        this.toString = function() {
            return '{' + _inner.join(',') + '}';
        }
        this.copy = function() { return new Gathor(_inner); }
        this.clear = function() { _inner = []; }
    }
}
function callByName(bsFace, methodName, params, oncomplete, onerror, ontimeout) {
    if (typeof (methodName) != 'string' || methodName == '') { throw 'invalid methodName.'; }
    var method = new Method('/remotecall.xyz', 'XhTravel.MaintenanceWebUI', bsFace, methodName, ontimeout);
    method(params, oncomplete, onerror);
}

function onFailed(error) {
    if (error) { alert(error.message); } else { alert("调用出错，请销候重试！"); }
}