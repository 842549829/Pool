$(function () {
    $("#btnSave").click(function () {
        var nameObject = $("#txtName")
        var name = nameObject.val();
        if ($.trim(name) == "") {
            alert("请输入姓名！");
            nameObject.focus();
            return false;
        } else {
            var customerPattern = /^([\u4e00-\u9fa5]+[a-z,A-Z]*)|([a-z,A-Z]+\/[a-z,A-Z]+)$/;
            if (($.trim(name).length > 25) || (!customerPattern.test(name))) {
                alert("姓名格式错误！");
                nameObject.select();
                return false;
            }
        }
        if ($("#dropCustomerType").val() == "") {
            alert("请选择旅客类型！");
            return false;
        }
        var contactPhoneObject = $("#txtContactPhone")
        var contactPhone = contactPhoneObject.val();
        if ($.trim(contactPhone) == "") {
            alert("请输入联系电话！");
            contactPhoneObject.focus();
            return false;
        } else {
            var phonePattern = /^1[3458]\d{9}$/;
            if (!phonePattern.test($.trim(contactPhone))) {
                alert("联系电话格式错误！");
                contactPhoneObject.select();
                return false;
            }
        }
        if ($("#dropCertType").val() == "") {
            alert("请选择证件类型！");
            return false;
        }
        var certObject = $("#txtCertId");
        var certNo = certObject.val();
        if ($.trim(certNo) == "") {
            alert("请填写证件号码！");
            certObject.focus();
            return false;
        } else {
            if ($.trim(certNo).length > 50) {
                alert("证件号码格式错误！");
                certObject.select();
                return false;
            }
        }
        if ($("#dropCertType").val() == "0") {
            if (!isCardID(certNo)) {
                alert("证件号码格式错误！");
                certObject.select();
                return false;
            }
        }
    });
})

    //验证身份证
var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
function isCardID(sId) {
        var iSum = 0;
        var info = "";
        if (!/^\d{17}(\d|x)$/i.test(sId)) return false;
        sId = sId.replace(/x$/i, "a");
        if (aCity[parseInt(sId.substr(0, 2))] == null) return false;
        sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
        var d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) return false;
        for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
        if (iSum % 11 != 1) return false;
        return true; //aCity[parseInt(sId.substr(0,2))]+","+sBirthday+","+(sId.substr(16,1)%2?"男":"女") 
    } 