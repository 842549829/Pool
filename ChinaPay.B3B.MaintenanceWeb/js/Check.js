/*----------------------------------------------------------------
* File Name        Check.js
* File Desc 	   判断数据是否正确
*
* CreatedBy        jason
* CreatedDate	   2010-8-16 20:23
*----------------------------------------------------------------*/
String.prototype.trim = function() {
    return this.replace(/^\s+/, '').replace(/\s+$/, '');
}
XHCheck = {
    UnEmail: /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
    IsEmail: function(str) {
        return !this.UnEmail.test(str.trim());
    },
    Email: "this.IsEmail(value)",

    UnPhone: /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/,
    IsPhone: function(str) {
        if (str.indexOf("/") != "") {
            var Phones = str.split('/');
            for (var i = 0; i < Phones.length; i++) {
                if (this.UnPhone.test(Phones[i]) == false) {
                    return false;
                }
            }
        }
        else {
            return !this.UnPhone.test(str.trim());
        }
    },
    Phone: "this.IsPhone(value)",

    UnMobile: /^((\(\d{2,3}\))|(\d{3}\-))?1(3|5|8|4)\d{9}$/,
    IsMobile: function(str) {
        return !this.UnMobile.test(str.trim());
    },
    Mobile: "this.IsMobile(value)",

    UnUrl: /^[http:\/\/]{0,1}[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/,

    IsUrl: function(str) {
        return !this.UnUrl.test(str.trim());
    },
    Url: "this.IsUrl(value)",

    UnPostNo: /^[0-9]\d{5}(?!\d)$/,
    IsPostNo: function(str) {
        return !this.UnPostNo.test(str.trim());
    },
    PostNo: "this.IsPostNo(value)",

    UnNumber: /^\d+$/,
    IsNumber: function(str) {
        return !this.UnNumber.test(str.trim());
    },
    Number: "this.IsNumber(value)",

    UnCompanyName: /^[A-Za-z0-9\u0391-\uFFE5]{1,50}$/,
    IsCompanyName: function(str) {
        return !this.UnCompanyName.test(str.trim());
    },
    CompanyName: "this.IsCompanyName(value)",

    UnInteger: /^[-\+]?\d+$/,
    IsInteger: function(str) {
        return !this.UnInteger.test(str.trim());
    },
    Integer: "this.IsInteger(value)",

    UnDouble: /^[-\+]?\d+(\.\d+)?$/,
    IsDouble: function(str) {
        return !this.UnDouble.test(str.trim());
    },
    Double: "this.IsDouble(value)",

    unFax: /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/,
    IsFax: function(str) {
        return !this.unFax.test(str.trim());
    },
    Fax: "this.IsFax(value)",

    unOfficeNo: /^[a-zA-Z0-9]{6,7}$/,
    IsOfficeNo: function(str) {
        return !this.unOfficeNo.test(str.trim());
    },
    OfficeNo: "this.IsOfficeNo(value)",

    UnUserCode: /^[a-zA-z]{1}[a-zA-z0-9]{5,25}$/,
    IsUserCode: function(str) {
        return !this.UnUserCode.test(str.trim());
    },
    UnRate: /^([0-9]{1,2})+(.[0-9])?$/,
    IsRate: function(str) {
        return !this.UnRate.test(str.trim());
    },
    Rate: "this.IsRate(value)",
    UserCode: "this.IsUserCode(value)",

    UnPassword: /^[a-zA-z0-9]{6,20}$/,
    IsPassword: function(str) {
        return !this.UnPassword.test(str.trim());
    },
    UnIp: /^(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))\.)(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5]))|0)\.){2}([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5]))|0)$/,
    IsIp: function(str) {
        return !this.UnIp.test(str.trim());
    },
    Ip: "this.IsIp(value)",
    unPort: /^\d{1,5}$/,
    IsPort: function(str) {
        return !this.unPort.test(str.trim());
    },
    Port: "this.IsPort(value)",

    Password: "this.IsPassword(value)",
    English: /^[A-Za-z]+$/,
    Chinese: /^[\u0391-\uFFE5]+$/,
    UnUsername: /[a-z]\w{3,20}|^[\u4e00-\u9fa5]{2,4}$/,
    IsUsername: function(str) {
        return !this.UnUsername.test(str.trim());
    },
    Username: "this.IsUsername(value)",
    UnSafe: /^(([-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|([and|select|insert|delete|or|username|password|alert(|script|html]*))$|\s/,
    IsSafe: function(str) {
        return !this.UnSafe.test(str.trim());
    },


    SafeString: "this.IsSafe(value)",
    Date: "this.IsDate(value, getAttribute('min'), getAttribute('format'))",
    Range: "getAttribute('min') <= (value|0) && (value|0) <= getAttribute('max')",
    Compare: "this.compare(value,getAttribute('operator'),getAttribute('to'))",
    Validate: function(theForm) {
        var obj = theForm;
        var count = obj.elements.length;
        for (var i = 0; i < count; i++) {
            with (obj.elements[i]) {
                if (disabled) {
                    continue;
                }
                if (getAttribute("check") && getAttribute("check") == "1"
						&& ((obj.elements[i].value.trim()) == null || obj.elements[i].value.trim() == "" || obj.elements[i].value.trim() == "-1" || obj.elements[i].value.trim() == '' || obj.elements[i].value.trim() == '-1')) {
                    alert(getAttribute("showname"));
                    if (!getAttribute("readonly") && getAttribute("type") != 'hidden') {
                        focus();
                    }
                    return false;
                }
                if (getAttribute("bitian") && getAttribute("bitian") == "1"
						&& ((obj.elements[i].value.trim()) == null || obj.elements[i].value.trim() == "" || obj.elements[i].value.trim() == "-1" || obj.elements[i].value.trim() == '' || obj.elements[i].value.trim() == '-1')) {
                    alert(getAttribute("showname") + "不能为空！");
                    if (!getAttribute("readonly") && getAttribute("type") != 'hidden') {
                        focus();
                    }
                    return false;
                }
                var _dataType = getAttribute("dataType");
                if (typeof (_dataType) == "object"
						|| typeof (this[_dataType]) == "undefined")
                    continue;
                if (getAttribute("require") == "false" && value == "")
                    continue;
                switch (_dataType) {
                    case "Date":
                    case "Range":
                    case "Double":
                    case "Compare":
                    case "Ip":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不合法');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Port":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不合法');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Rate":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不合法');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Password":

                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不合法');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "CompanyName":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Number":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Username":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "UserCode":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "OfficeNo":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Integer":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Fax":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "PostNo":
                        if (value.trim() != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Url":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Mobile":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Email":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "Phone":
                        if (value != "" && eval(this[_dataType])) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    case "SafeString":
                        if (value != "" && !eval(this[_dataType])) {
                            alert(getAttribute("showname") + '非法字符');
                            value = '';
                            if (!getAttribute("readonly")
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                    default:
                        if (value != "" && !this[_dataType].test(value)) {
                            alert(getAttribute("showname") + '不正确');
                            value = '';
                            if (scrollTop > 0
									&& getAttribute("type") != 'hidden') {
                                focus();
                            }
                            return false;
                        }
                        break;
                }
            }
        }
        return true;
    },
    ValidateObj: function(obj) {
        with (obj) {
            if (getAttribute("bitian") && getAttribute("bitian") == "1"
					&& getAttribute("value") == "") {
                alert(getAttribute("showname") + "不能为空！");
                focus();
                return false;
            }
            var _dataType = getAttribute("dataType");
            switch (_dataType) {
                case "Date":
                case "Range":
                case "Double":
                case "Compare":
                case "SafeString":
                    if (value != "" && !eval(this[_dataType])) {
                        alert(getAttribute("showname") + '不正确');
                        value = '';
                        if (scrollTop > 0 && getAttribute("type") != 'hidden') {
                            focus();
                        }
                        return false;
                    }
                    break;
                default:
                    if (value != "" && !this[_dataType].test(value)) {
                        alert(getAttribute("showname") + '不正确');
                        if (scrollTop > 0 && getAttribute("type") != 'hidden') {
                            focus();
                        }
                        return false;
                    }
                    break;
            }
        }

        return true;
    },
    ValiNull: function(obj) {
        with (obj) {
            if (getAttribute("value") == "") {
                alert(getAttribute("showname") + "不能为空！");
                if (scrollTop > 0 && getAttribute("type") != 'hidden') {
                    focus();
                }
                return false;
            }
        }

        return true;
    },
    compare: function(op1, operator, op2) {
        switch (operator) {
            case "NotEqual":
                return (op1 != op2);
            case "GreaterThan":
                return (op1 > op2);
            case "GreaterThanEqual":
                return (op1 >= op2);
            case "LessThan":
                return (op1 < op2);
            case "LessThanEqual":
                return (op1 <= op2);
            default:
                return (op1 == op2);
        }
    },

    IsDate: function(op, formatString) {
        alert(op);
        formatString = formatString || "ymd";
        var m, year, month, day;
        switch (formatString) {
            case "ymd":
                m = op
						.match(new RegExp("^((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})$"));
                if (m == null)
                    return false;
                day = m[6];
                month = m[5] * 1;
                year = (m[2].length == 4) ? m[2] : GetFullYear(parseInt(m[3],
						10));
                break;
            case "dmy":
                m = op
						.match(new RegExp("^(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))$"));
                if (m == null)
                    return false;
                day = m[1];
                month = m[3] * 1;
                year = (m[5].length == 4) ? m[5] : GetFullYear(parseInt(m[6],
						10));
                break;
            default:
                break;
        }
        if (!parseInt(month))
            return false;
        month = month == 0 ? 12 : month;
        var date = new Date(year, month - 1, day);
        return (typeof (date) == "object" && year == date.getFullYear()
				&& month == (date.getMonth() + 1) && day == date.getDate());
        function GetFullYear(y) {
            return ((y < 30 ? "20" : "19") + y) | 0;
        }
    }
}

function XHLXCheck(id, dataType, showName) {
    var Email = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
    var Phone = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/;
    var Mobile = /^((\(\d{2,3}\))|(\d{3}\-))?1(3|5|8|4)\d{9}$/;
    var Number = /^\d+$/;
    var Integer = /^[-\+]?\d+$/;
    var Double = /^[-\+]?\d+(\.\d+)?$/;
    var Fax = /^(\d{3,4})?\d{7,8}$/;
    var Url = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
    var PostNo = /^[1-9]\d{5}(?!\d)$/;
    var _dataType;
    var value = $.trim($("#" + id).val());

    switch (dataType) {
        case "Email":
            _dataType = Email;
            break;
        case "Phone":
            _dataType = Phone;
            break;
        case "Mobile":
            _dataType = Mobile;
            break;
        case "Number":
            _dataType = Number;
            break;
        case "Integer":
            _dataType = Integer;
            break;
        case "Double":
            _dataType = Double;
            break;
        case "Fax":
            _dataType = Fax;
            break;
        case "Url":
            _dataType = Url;
            break;
        case "PostNo":
            _dataType = PostNo;
            break;


    }

    if (!_dataType.exec(value) && value !== "") {
        alert(showName + "格式错误");
        $("#" + id).val("");
        $("#" + id).select();
        return false;
    }
    return true;
}
