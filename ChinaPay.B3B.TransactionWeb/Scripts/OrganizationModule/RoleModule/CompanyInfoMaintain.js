$(function () {
    //基本信息维护
    /*公司电话*/var txtCompanyPhone = function (e) { if ($("#txtCompanyPhone").length > 0) { return Validate(["EmptyOrObject=电话不能为空", "Phone=电话格式错误"], "txtCompanyPhone", e) && ValidateLength(["Max=电话最多100位"], "txtCompanyPhone",100,e); } else { $("#txtCompanyPhone").message(""); return true; } };
    /*邮政编码*/var txtPostCode = function (e) { return Validate(["EmptyOrObject=邮政编码不能为空", "PostCode=邮政编码格式错误"], "txtPostCode", e); };
    /*传真*/var txtFaxes = function (e) { if ($("#txtFaxes").val().length > 0) { return Validate(["Phone=传真格式错误"], "txtFaxes", e) && ValidateLength(["Max=传真最多20个字符"], "txtFaxes", 20, e); } else { $("#txtFaxes").message(""); return true; } };
    /*Email*/var txtEmail = function (e) { return Validate(["EmptyOrObject=Emali不能为空", "Email=Email格式错误"], "txtEmail", e) && ValidateLength(["Max=Email最多100个字符"],"txtEmail",100,e); };
    /*MSN*/var txtMSN = function (e) { if ($("#txtMSN").val().length > 0) { return Validate(["Email=MSN格式错误"], "txtMSN", e) && ValidateLength(["Max=MSN最多100个字符"], "txtMSN",100,e); } else { $("#txtMSN").message(""); return true; } };
    /*QQ*/var txtQQ = function (e) { if ($("#txtQQ").val().length > 0) { return Validate(["QQ=QQ格式错误"], "txtQQ", e); } else { $("#txtQQ").message(""); return true; } };
    $("#btnSvaeCompanyInfo").click(function () {
        return txtPostCode(true) && txtCompanyPhone(true) && txtFaxes(true) && txtEmail(true) && txtMSN(true) && txtQQ(true);
    });
    //公司信息
    $("#btnSaveChilder").click(function () {
        var arrival = $("#Departure_txtAirport").val().toLocaleUpperCase(), departure = $("#Arrival_txtAirport").val().toLocaleUpperCase();
        if (arrival.length > 0 && departure.length > 0) {
            if (arrival == departure || $("#Departure_ddlAirports").val() == $("#Arrival_ddlAirports").val()) {
                alert("到达城市不能与出发城市一样");
                return false;
            } 
        }
        if ($("#ddlOffice").val() == "0") {
            alert("请选择一个Office号");
            return false;
        }
        var reg = /^\d{1,2}(\.\d{1})?$/;
        if ($("#chkChildern").is(":checked") && !reg.test($("#txtCholdrenDeduction").val())) {
            alert("儿童返点格式错误,只支持保留一位小数");
            return false;
        }
        if ($("#chkChildern").is(":checked") && parseFloat($("#txtCholdrenDeduction").val()) <= 0) {
            alert("儿童返点不能为0%");
            return false;
        }
        if ($("#chkChildern").is(":checked") && $("#chklCholdrenDeduction :checkbox:checked").length < 1) {
            alert("请选择可发布儿童政策的航空公司");
            return false;
        }
    });
    $("#btnSave").click(function () {
        var IsSubmit = false;
        var reg = /^[a-zA-z\u4e00-\uf95a]{1,8}$/;
        var phone = /^1[3458]\d{9}$/;
        var qq = /^\d{5,12}$/;
        if ($("#divPerson").length > 0) {
            $("table:last tr").each(function (index) {
                if ((index != 0) && !(reg.test($("td:nth-child(2) input:text", this).val()))) {
                    $("td:nth-child(2) input:text", this).focus().select();
                    IsSubmit = false;
                    alert("负责人格式错误");
                    return false;
                }
                if ((index != 0) && !(phone.test($("td:nth-child(3) input:text", this).val()))) {
                    $("td:nth-child(3) input:text", this).focus().select();
                    IsSubmit = false;
                    alert("负责人手机格式错误");
                    return false;
                }
                if ((index != 0) && !(qq.test($("td:nth-child(4) input:text", this).val()))) {
                    $("td:nth-child(4) input:text", this).focus().select();
                    IsSubmit = false;
                    alert("负责人QQ格式错误");
                    return false;
                }
                IsSubmit = true;
            });
        } else {
            IsSubmit = true;
        }
        return IsSubmit;
    });
});