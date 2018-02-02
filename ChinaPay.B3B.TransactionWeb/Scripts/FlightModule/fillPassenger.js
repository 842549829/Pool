var mobilePattern = /^1[3|5|8][0-9]{9}$/;
var pnrCodePattern = /^[0-9,a-z,A-Z]{6}$/;
var emailPattern = /^\w+@\w+(\.\w{2,4}){1,}$/;
var birthDayPattern = /^\d{4}-\d{1,2}-\d{1,2}$|^\d{4}\/\d{1,2}\/\d{1,2}$/;
var idCardNOPattern = /^\d{6}(\d{4})(\d{2})(\d{2}).{4}$/;
var currentPassengerForChooseCustomer;
var pageSize = 10;
$(function () {
    loadCredentialsTypes();
    if ($("#ddlPassengerType option").size() > 1) {
        $("#ddlPassengerType").change(function () {
            if ($("#ddlPassengerType").val() == "1") {
                // 儿童时，需要输入成人编码
                $("#adultPNRInfo").show();
            } else {
                $("#adultPNRInfo").hide();
            }
            $("#divPassengers .credentialsType").trigger("change");
        });
        $("#txtAdultPNRCode").blur(function () {
            validateAdultPNRCode($(this));
        });
    }
    $("#divPassengers .passengerName").live("focus", function () { loadPassger(this); });
    $("#divPassengers .passengerName").live("blur", function () { validatePassengerName(this); });
    $("#divPassengers .tipControl").live("mouseleave", function () { $(".tipControl").hide(); });
    $("#divPassengers .tipControl img").live("click", function () { $(".tipControl").hide(); });
    $("#divPassengers .credentials").live("blur", function () { validateCredentials(this); });
    $("#divPassengers .passengerMobile").live("blur", function () { validateMobile($(this)); });
    $("#divPassengers .txtBirthDay").live("blur", function () { validatebirthDay($(this)); });
    $("#divPassengers .passengerName").live("keyup", function () { loadPassger(this); });

    $("#divPassengers .tipControl li").live("click", function () {
        var pass = $(this).attr("passenger").toString().split("|");
        $(this).parent().parent().parent().find(".passengerName").val(pass[0]);
        $(this).parent().parent().parent().parent().parent().find(".credentialsType").val(pass[1]);
        $(this).parent().parent().parent().parent().parent().find(".credentials").val(pass[2]);
        $(this).parent().parent().parent().parent().parent().find(".passengerMobile").val(pass[3]);

        $(this).parent().parent().parent().find(".passengerName").blur();
        $(this).parent().parent().parent().parent().parent().find(".credentialsType").blur();
        $(this).parent().parent().parent().parent().parent().find(".credentials").blur();
        $(this).parent().parent().parent().parent().parent().find(".passengerMobile").blur();
        $(".tipControl").hide();
    });
    $("#divPassengers .credentialsType").live("change", function () {
        var container = $(this).parent().parent();
        if (GetAirLine() == "CZ" && $("#ddlPassengerType").val() == "1" && $(this).val() != "0" && $(this).val() != "4") {
            $(".birthDay", container).show().data("needCheck", "1");
        } else {
            $(".birthDay", container).hide().data("needCheck", "0");
        }
    });
    $("#divPassengers .btnSelectCustomer").live("click", function () {
        getCustomer(this);
    });
    $("#divPassengers .btnRemoverCustomer1").live("click", function () {
        var passengerCount = $("#divPassengers .memberItem").length;
        if (passengerCount != "1") {
            var currentPassenger = $(this).parent().parent();
            currentPassenger.remove();
            $(".newPassengerOption").show();
        }
        $("#btnNewPassenger").removeAttr("disabled");
    });
    if ($("#hidSeatCount").val() == "1") {
        $(".newPassengerOption").hide();
    }
    $("#btnNewPassenger").live("click", function () {
        var passengerCount = $("#divPassengers .memberItem").length;
        var maxPassengerCount = $("#hidSeatCount").val();
        if (maxPassengerCount > 9) {
            maxPassengerCount = 9;
        }
        if (passengerCount < maxPassengerCount) {
            var lastPassenger = $("#divPassengers .memberItem:last");
            var newPassegner = lastPassenger.clone();
            $("input:text", newPassegner).val("");
            $("div.error", newPassegner).remove();
            lastPassenger.after(newPassegner);
            if (passengerCount == maxPassengerCount - 1) {
                $(".newPassengerOption").hide();
            }
        }
        if (parseInt(passengerCount) == parseInt(maxPassengerCount) - 1) {
            $("#btnNewPassenger").attr("disabled", "disabled");
        }
        for (var i = 0; i < parseInt(passengerCount); i++) {
            $("#divPassengers .memberItem").eq(i).find("td>div").css("z-index", (998 - i));
        }
        $(".tipControl").hide();
    });
    $("#txtEmail").blur(function () {
        var email = $.trim($("#txtEmail").val());
        if (email.length > 0 && !emailPattern.test(email) || email.length > 60) {
            showErrorMessage("Email格式错误", $("#txtEmail"));
            return false;
        } else {
            clearErrorMessage($(this));
            return true;
        }
    });
    $("#txtMobile").blur(function () {
        validateMobile($(this));
    });
    $("#txtContact").blur(function () {
        if ($.trim(this.value) == '') {
            showErrorMessage('请输入联系人', $(this));
            return false;
        }
        if (!/(^[\u4e00-\u9fa5]{2,}$)|(^[\u4e00-\u9fa5]+[a-z,A-Z]+$)|(^[a-z,A-Z]+\/[a-z,A-Z]+$)/.test($.trim(this.value))) {
            showErrorMessage('联系人格式错误', $(this));
            return false;
        }
        clearErrorMessage($(this));
    });
    $(".btnSeverCustomer1").live("click", function () {
        var container = $(this).parent().parent();
        var name = $.trim($(".passengerName", container).val());
        var credentialsType = $.trim($(".credentialsType", container).val());
        var credentials = $.trim($(".credentials", container).val());
        if (name == '') {
            showErrorMessage("乘机人姓名不能为空", $(".passengerName", container));
            return false;
        }
        if (credentials == '') {
            showErrorMessage("乘机人证件号不能为空", $(".credentials", container));
            return false;
        }
        if (!validateMobile($(".passengerMobile", container))) {
            return false;
        }
        var mobile = $.trim($(".passengerMobile", container).val());
        var passenger = { "Name": name, "PassengerType": $("#ddlPassengerType").val(), "CredentialsType": credentialsType, "Credentials": credentials, "Phone": mobile };
        sendPostRequest("/FlightHandlers/FillPassenger.ashx/RegisterCustomer", JSON.stringify({ "passenger": passenger }), function (rsp) {
            if (rsp == "OK") {
                alert("保存成功！");
            }
            else {
                alert("常旅客保存失败," + rsp);
            }
        });
    });
    $("#btnNext").click(function () {

        if ($(".error").size() != 0) return;
        $("#btnNext").hide();
        $("#btnWaiting").show();
        if ($("#ddlPassengerType").val() == "1") {
            if (!validateAdultPNRCode($("#txtAdultPNRCode"))) {
                $("#btnNext").show();
                $("#btnWaiting").hide();
                return;
            }
        }
        var adultPNR = $.trim($("#txtAdultPNRCode").val());
        var passengers = getPassengers();
        var contact = getContact();
        if (passengers == null || contact == null || passengers == false) {
            $("#btnNext").show();
            $("#btnWaiting").hide();
        } else {
            sendPostRequest("/FlightHandlers/FillPassenger.ashx/CommitPassengers", JSON.stringify({ "passengers": passengers, "contact": contact, "adultPNR": adultPNR }), function (result) {
                window.location.href = '/Agency.htm?target=' + encodeURIComponent('/FlightReserveModule/ChoosePolicy.aspx?source=1');
            }, function (error) {
                if (error.status == 300) {
                    showErrorMessage(JSON.parse(error.responseText));
                } else {
                    showErrorMessage("操作失败");
                }
                $("#btnNext").show();
                $("#btnWaiting").hide();
                $(".close").click();
            });
        }
    });
    $("#btnQueryCustomers").click(function () {
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryCustomers(1, pageSize);
    });
});

function validateAdultPNRCode(sender) {
    var code = $.trim(sender.val());
    if (code == '') {
        showErrorMessage("请输入成人PNR编码", sender);
        return false;
    } else if (!(pnrCodePattern.test(code))) {
        showErrorMessage("成人PNR编码格式错误", sender);
        return false;
    } else {
        clearErrorMessage(sender);
        return true;
    }
}
function validatePassengerName(sender) {
    var name = $.trim($(sender).val());
    if (name == "") {
        showErrorMessage("请输入乘机人姓名", $(sender));
    } else {
        sendPostRequest("/FlightHandlers/FillPassenger.ashx/ValidatePassengerName", JSON.stringify({ "name": name }), function (result) {
            clearErrorMessage($(sender));
        }, function (error) {
            if (error.status == 300) {
                showErrorMessage(error.responseText, $(sender));
            }
        });
    }
}
function validateCredentials(sender) {
    var identifyCardNo = $.trim($(sender).val());
    if (!/^\w+$/.test(identifyCardNo)) showErrorMessage("证件号格式错误", $(sender));
    var credentialsType = $(sender).prev().val();
    if (credentialsType == "0") {
        // 验证身份证号
        if (identifyCardNo == '') {
            showErrorMessage("请输入证件号", $(sender));
        } else {
            sendPostRequest("/FlightHandlers/FillPassenger.ashx/ValidateIdentifyCard", JSON.stringify({ "identifyCardNo": identifyCardNo }), function (result) {
                if (result == false) {
                    showErrorMessage("证件号格式错误", $(sender));
                } else {
                    clearErrorMessage($(sender));
                }
            });
        }
    } else {
        clearErrorMessage($(sender));
    }
}
function validateMobile(sender) {
    var mobile = $.trim(sender.val());
    if (mobile == '') {
        showErrorMessage('请输入手机号', sender);
        return false;
    } else if (!(mobilePattern.test(mobile))) {
        showErrorMessage("手机号格式错误", sender);
        return false;
    } else {
        clearErrorMessage(sender);
        return true;
    }
}
function validatebirthDay(sender) {
    if (!sender.is(":visible")) return true;
    if (sender.parent().data("needCheck") != "1") return true;
    var birthDayValue = $.trim(sender.val());
    if (birthDayValue == '') {
        showErrorMessage('请输儿童出生日期', sender);
        return false;
    } else if (!(birthDayPattern.test(birthDayValue))) {
        showErrorMessage("出生日期格式错误", sender);
        return false;
    } else {
        clearErrorMessage(sender);
        return true;
    }
}
function loadCredentialsTypes() {
    sendPostRequest("/FlightHandlers/FillPassenger.ashx/GetCredentialsTypes", "", function (data) {
        var objCredentialsType = $(".credentialsType");
        $(data).each(function (index, item) {
            objCredentialsType.append('<option value="' + item.Value + '">' + item.Text + '</option>');
        });
    });
}
function getPassengers() {
    var passengers = new Array();
    var passengerType = $("#ddlPassengerType").val();
    $("#divPassengers .memberItem").each(function (index) {
        var container = $(this);
        var name = $.trim($(".passengerName", container).val());
        var credentialsType = $.trim($(".credentialsType", container).val());
        var credentials = $.trim($(".credentials", container).val());
        var birthDay = $(".txtBirthDay", container).val();
        if (name == '') {
            showErrorMessage("乘机人姓名不能为空", $(".passengerName", $(this)));
            passengers = null;
            return false;
        }
        if (credentials == '') {
            showErrorMessage("乘机人证件号不能为空", $(".credentials", $(this)));
            passengers = null;
            return false;
        }
        if (!validateMobile($(".passengerMobile", $(this)))) {
            passengers = null;
            return false;
        }
        if (validatebirthDay($(".txtBirthDay", container))) {
            var birthDayCtl = $(".txtBirthDay", container);
            var now = new Date();
            if (birthDayCtl.parent().data("needCheck") == "1") {
                birthDay = birthDayCtl.val();
                var birthDate1 = Date.fromString(birthDay);
                var age1 = Date.diff(now, birthDate1, 'y');
                if (age1 > 12 || age1 < 2) {
                    alert("乘机人" + name + "不是2到12岁的儿童");
                    birthDay = null;
                    return false;
                }
            } else {
                if (GetAirLine() == "CZ" && $("#ddlPassengerType").val() == "1") {
                    if ($(".credentialsType", container).val() == "0") {
                        idCardNOPattern.test(credentials);
                        birthDay = RegExp.$1 + "-" + RegExp.$2 + "-" + RegExp.$3;
                    } else if ($(".credentialsType", container).val() == "4") {
                        if (/(\d{4})(\d{2})(\d{2})/.test(credentials)) {
                            birthDay = RegExp.$1 + "-" + RegExp.$2 + "-" + RegExp.$3;
                        } else {
                            alert("出生日期格式不正确！");
                            return false;
                        }
                    }
                    var birthDate = Date.fromString(birthDay);
                    var age = Date.diff(now, birthDate, 'y');
                    if (age > 12 || age < 2) {
                        alert("乘机人" + name + "不是2到12岁的儿童");
                        birthDay = null;
                        return false;
                    }
                } else {
                    birthDay = null;
                }
            }
        }
        var mobile = $.trim($(".passengerMobile", $(this)).val());
        passengers[index] = { "Name": name, "PassengerType": passengerType, "CredentialsType": credentialsType, "Credentials": credentials, "Phone": mobile, "BirthDay": birthDay };
    });
    return passengers;
}
function getContact() {
    var contact = $.trim($("#txtContact").val());
    var mobile = $.trim($("#txtMobile").val());
    var email = $.trim($("#txtEmail").val());
    if (contact == '') {
        showErrorMessage("联系人姓名不能为空", $("#txtContact"));
        return null;
    }
    if (!/(^[\u4e00-\u9fa5]+[a-z,A-Z]*$)|(^[a-z,A-Z]+\/[a-z,A-Z]+$)/.test(contact)) {
        showErrorMessage("联系人姓名格式错误", $("#txtContact"));
        return null;
    }
    if (!mobilePattern.test(mobile)) {
        showErrorMessage("手机格式错误", $("#txtMobile"));
        return null;
    }
    if (mobile == '') {
        showErrorMessage("联系人手机号不能为空", $("#txtMobile"));
        return null;
    }
    if (email.length > 0 && !emailPattern.test(email) || email.length > 60) {
        showErrorMessage("Email格式错误", $("#txtEmail"));
        return null;
    }
    return { "Name": contact, "Mobile": mobile, "Email": email };
}

function getCustomer(sender) {
    currentPassengerForChooseCustomer = $(sender).parent().parent();
    $("#divCustomersContent").html('');
    $("#divPagination").html('');
    $("#lnkCustomers").click();
    if ($("#dropPageSize").size() > 0) {
        pageSize = $("#dropPageSize option:selected").val();
    }
    queryCustomers(1, pageSize);
}
function showErrorMessage(message, sender) {
    if (sender) {
        if (sender.next().hasClass("error")) {
            $(".content", sender.next()).text(message);
        } else {
            var position = sender.position();
            var errorInfo = $('<div class="error" style="position:absolute;background-color:white;color:red;border:1px solid black;margin:5px 0 0 70px;width:150px;text-align:left;padding:3px"><span class="tips" style="background-color:white;"><i class="blue icon icon-info-circle"></i><span class="content">' + message + '</span></span></div>');

            sender.after($(errorInfo).css({ "top": position.top + 23, "left": position.left - 70, "z-index": 999 }));
        }
    } else {
        alert(message);
    }
}
function clearErrorMessage(sender) {
    if (sender) {
        if (sender.next().hasClass("error")) {
            sender.next().remove();
        }
    }
}
function queryCustomers(pageIndex, pageSize) {
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryCustomersCondition();
    var pagination = getQueryCustomersPagination(pageIndex, pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "condition": condition });
    sendPostRequest("/FlightHandlers/FillPassenger.ashx/GetCustomers", parameters, function (data) {
        bindCustomers(data);
    }, function (e) {
        alert(e.responseText);
    });
}
function loadPassger(sender) {
    var condition = { "Name": $(sender).val(), "Credentials": "", "Mobile": "" };
    var pagination = { "PageSize": 10, "PageIndex": 1, "GetRowCount": true };
    var parameters = JSON.stringify({ "pagination": pagination, "condition": condition });
    sendPostRequest("/FlightHandlers/FillPassenger.ashx/GetCustomers", parameters, function (data) {
        var str = "";
        $.each(data.Customers, function (index, item) {
            str += "<li passenger='" + item.Name + "|" + item.CredentialsTypeValue + "|" + item.Credentials + "|" + item.Mobile + "'>" + item.Name + "</li>";
        });
        $(sender).parent().find(".tipControl .passenger").html(str);
        $(".tipControl").hide();
        if (str != "") {
            $(sender).parent().find(".tipControl").show();
        }
    }, function (e) {
        alert(e.responseText);
    });
}
function getQueryCustomersCondition() {
    return { "Name": $.trim($("#txtConditionName").val()), "Credentials": $.trim($("#txtConditionCredentials").val()), "Mobile": $.trim($("#txtConditionMobile").val()) };
}
function getQueryCustomersPagination(pageIndex, pageSzie) {
    return { "PageSize": pageSzie, "PageIndex": pageIndex, "GetRowCount": true };
}
function bindCustomers(data) {
    var customersHtml = new Array();
    customersHtml.push('<table><tr><th>姓名</th><th>性别</th><th>乘机人类型</th><th>证件类型</th><th>证件号</th><th>手机号码</th><th></th></tr>');
    $.each(data.Customers, function (index, item) {
        customersHtml.push('<tr><td>');
        customersHtml.push(item.Name);
        customersHtml.push('</td><td>');
        customersHtml.push(item.Sex);
        customersHtml.push('</td><td>');
        customersHtml.push(item.PassengerType);
        customersHtml.push('</td><td>');
        customersHtml.push('<input type="hidden" value="' + item.CredentialsTypeValue + '"/>');
        customersHtml.push(item.CredentialsType);
        customersHtml.push('</td><td>');
        customersHtml.push(item.Credentials);
        customersHtml.push('</td><td>');
        customersHtml.push(item.Mobile);
        customersHtml.push('</td><td><input type="button" class="btn class2" value="选&nbsp;&nbsp;&nbsp;&nbsp;择" onclick="chooseCustomer(this)"/></td></tr>');
    });
    customersHtml.push('</table>');
    $("#divCustomersContent").html(customersHtml.join(''));

    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryCustomers);
}
function chooseCustomer(sender) {
    var currentCustomer = $(sender).parent().parent();
    $(".passengerName", currentPassengerForChooseCustomer).val(currentCustomer.children().eq(0).html());
    $(".credentialsType", currentPassengerForChooseCustomer).val(currentCustomer.children().eq(3).children().eq(0).val());
    $(".credentials", currentPassengerForChooseCustomer).val(currentCustomer.children().eq(4).html());
    $(".passengerMobile", currentPassengerForChooseCustomer).val(currentCustomer.children().eq(5).html());
    validatePassengerName($(".passengerName", currentPassengerForChooseCustomer));
    validateCredentials($(".credentials", currentPassengerForChooseCustomer));
    validateMobile($(".passengerMobile", currentPassengerForChooseCustomer));
    $("#btnCustomerBack").click();
    $("#divCustomersContent").html('');
}
function GetAirLine() {
    var flightNo = $("#ucFlights_divFlights tr").last().children().eq(2).text();
    var airline = flightNo.substring(0, 2);
    return airline;
}
