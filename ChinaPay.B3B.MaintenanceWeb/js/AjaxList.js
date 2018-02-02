if (!window.fireEvent) {
	window.fireEvent = function(obj, fnEvent, args) {
		///<summary>出发指定对象上的事件。</summary>
		///<param name="obj">触发事件的对象。</param>
		///<param name="fnEvent">要触发的事件。</param>
		///<param name="args">不定数量的事件参数。</param>
		if (!(typeof (fnEvent) == 'function')) { fnEvent = obj ? obj[fnEvent] : null; }
		if (!fnEvent || !typeof (fnEvent) == 'function') { return; }
		var params = [];
		for (var i = 2; i < arguments.length; i++) {
			params.push(arguments[i]);
		}
		return fnEvent.apply(fnEvent, params);
	}
}
if (!window.parseJson) {
	///<summary>从 JSON 格式的字符串中解析数据。</summary>
	///<param name="str">被解析的字符串。</param>
    window.parseJson = function(str) {
		if (typeof (str) == 'string' && str != null) {
//			try { return eval('(' + str.replace(/\r/g,'\\r').replace(/\n/g,'\\n') + ')'); }
//			catch (e) { return null; }
            try{
                var fn=new Function('return '+str);
                return fn();
            }
            catch(e){return null;}
		}
		return null;
	}
}
if (!window.Request) {
	///<summary>请求类。只能通过调用 requestPool 对象的 getRequest 方法返回 Request 类的实例。</summary>
	window.Request = function(obj) {
		if (!obj) { throw 'argument \'obj\' is a null reference or undefined.'; }
		if (arguments.callee.caller != window.requestPool.getRequest) { throw 'Can not call Request constructor directly'; }
		var timerId = 0;
		function prepare(requestType, url) {
			var rtype = requestType.toUpperCase();
			obj.open(rtype, url, _self.async);
			setHeader();
			if (rtype == 'SOAP') { throw 'SOAP request deos not implement.'; }
			if (rtype == 'POST') { obj.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); }
		}
		function setHeader() { for (var header in headers) { obj.setRequestHeader(header, headers[header]); } }
		function assignResult(receiveType) {
			switch (receiveType) {
				case 0: //responseText	[default]
					return obj.responseText;
					break;
				case 1: //responseXML
					return obj.responseXML;
					break;
				case 2: //responseBody
					return obj.responseBody;
					break;
				default:
					throw 'unknown receive type.';
			}
		}
		function fireTimeOut() {
			obj.abort();
			fireEvent(null, _self.ontimeout, { code: 600, message: '请求超时。' });
		}
		function callback() {
			if (obj.readyState == 4) {
				// 已获取到服务器端的响应，清除超时计时。
				if (timerId) {
					clearTimeout(timerId);
					timerId = 0;
				}
				if (obj.status == 200) {
					try {
						fireEvent(null, _self.oncomplete, assignResult(_self.receiveType));
					}
					catch (e) {
						fireEvent(null, _self.onerror,e);
					}
				}
				else {
					fireEvent(null, _self.onerror, parseJson(obj.responseText));
				}
			}
		}
		function sendRequest(data) {
			if (_self.async == true) {
				obj.onreadystatechange = callback;
				obj.send(data);
				// 开始超时计时
				if (_self.timeout) {
					timerId = setTimeout(fireTimeOut, _self.timeout);
				}
			}
			else {
				obj.send(data);
				callback();
			}
		}

		var _self = this;
		var headers = {};
		_self.async = true;
		_self.timeout = 0;
		_self.receiveType = 0;
		_self.oncomplete = {};
		_self.onerror = {};
		_self.ontimeout = {};
		_self.sendGetRequest = function(url) {
			prepare('GET', url);
			sendRequest(null);
		};
		_self.sendPostRequest = function(url, data) {
			prepare('POST', url);
			sendRequest(data);
		};
		_self.sendSoapRequest = function(url, data) {
			prepare('SOAP', url);
			sendRequest(data);
		};
		_self.sendHeadRequest = function(url) {
			prepare('HEAD', url);
			sendRequest(null);
		};
		_self.isIdle = function() { return obj.readyState == 4 || obj.readyState == 0; };
		_self.setHeader = function(key, val) { headers[key] = val; }
	}
}
if (!window.requestPool) {
	window.requestPool = new function() {
		///<summary>请求池对象。</summary>
		function createRequest() {
			return window.XMLHttpRequest
				? new XMLHttpRequest()
				: function() {
					if (window.ActiveXObject) {
						var vers = new Array(
						  'MSXML2.XMLHTTP.6.0',
						  'MSXML2.XMLHTTP.5.0',
						  'MSXML2.XMLHTTP.4.0',
						  'MSXML2.XMLHTTP.3.0',
						  'MSXML2.XMLHTTP',
						  'Microsoft.XMLHTTP'
						);
						for (var i = 0; i < vers.length; i++) {
							try { return new ActiveXObject(vers[i]); }
							catch (e) { }
						}
					}
				} ();
		}
		var _self = this;
		var _requestPool = [];
		_self.getRequest = function() {
			for (var i = 0; i < _requestPool.length; i++) {
				if (_requestPool[i].isIdle()) {
					return _requestPool[i];
				}
			}
			_requestPool.push(new Request(createRequest()));
			return _requestPool[_requestPool.length - 1];
		}
	} ();
}
function Method(host, assembly, type, method, timeout) {
	///<summary>表示服务器端方法的类。</summary>
	///<param name="host"></param>
	///<param name="type">调用目标类。</param>
	///<param name="method">方法名称。</param>
	///<param name="timeout">以毫秒为单位的超时时间。</param>
	var result = function(params, fncomp, fnerror, fntimeout) {
		///<param name="params"></param>
		var request = requestPool.getRequest();
		request.oncomplete = fncomp;
		request.onerror = fnerror;
		request.ontimeout = fntimeout;
		request.setHeader('RemoteInvoke', 'MethodInvoke');
		if (result.assembly) { request.setHeader('Assembly', result.assembly); }
		if (result.type) { request.setHeader('TargetType', result.type); }
		if (result.method) { request.setHeader('CallingMethod', result.method); }
		request.sendPostRequest(result.host, params);
	}
	result.host = host;
	result.assembly = assembly;
	result.type = type;
	result.method = method;
	result.timeout = timeout;

	return result;
}
if (!window.scripts) { window.scripts = {}; }
window.scripts.load = function(path, defer) {
	///<summary>载入指定的脚本文件。</summary>
	if (typeof (path) == 'string' && path != '') {
		var request = requestPool.getRequest();
		if (request) {
			request.async = false;
			request.oncomplete = function(result) {
				if (defer == true) {
					var invoke = function() { eval(result); }
					if (window.attachEvent) {
						window.attachEvent('onload', invoke);
					}
					else {
						window.addEventListener('load', invoke, false);
					}
				}
				else {
					eval(result);
				}
			}
			request.sendGetRequest(path);
			return true;
		}
		else {
			document.writln('<script type=\'text/javascript\' src=\'' + path + '\'' + (defer ? ' defer=\'defer\'' : '') + ' />');
			return false;
		}
	}
	throw 'path is empty or null reference';
}