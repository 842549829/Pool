$(function () {
    $("#btnSave").click(function () {
        if ($("#dropConfigType").val() == "") {
            alert("请选择配置类型！");
            return false;
        }
        var loginObj = $("#txtConfigName");
        var login = $.trim(loginObj.val());
        if (login == "") {
            alert("请填写配置用户名！");
            loginObj.focus();
            return false;
        } else {
            var loginPattern = /^[a-zA-Z0-9]{0,20}$/;
            if (login.length > 20 ||!loginPattern.test(login)) {
                alert("配置用户名格式错误！");
                loginObj.select();
                return false;
            }
        }
        var passwordObj = $("#txtConfigPwd");
        var password = $.trim(passwordObj.val());
        if (password == "") {
            alert("请填写配置密码！");
            passwordObj.focus();
            return false;
        } else {
            if (password.length > 20) {
                alert("配置密码格式错误！");
                passwordObj.select();
                return false;
            }
        }
        var serverAddressObj = $("#txtServerAddress");
        var serverAddress = $.trim(serverAddressObj.val());
        if (serverAddress == "") {
            alert("请输入服务器地址！");
            serverAddressObj.focus();
            return false;
        } else {
            var serverPattern = /^(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))\.)(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))\.){2}([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))$/;
            if (!serverPattern.test(serverAddress)) {
                alert("服务器地址格式错误！");
                serverAddressObj.select();
                return false;
            }
        }
        var portObj = $("#txtServerDk");
        var port = $.trim(portObj.val());
        if (port == "") {
            alert("请输入服务器端口！");
            portObj.focus();
            return false;
        } else {
            var portPattern = /^(\d)+$/;
            if (!portPattern.test(port)) {
                alert("服务器端口格式错误！");
                portObj.select();
                return false;
            }
        }
        var officeNoObj = $("#txtOfficeNo");
        var officeNo = $.trim(officeNoObj.val());
        if (officeNo == "") {
            alert("请输入Office号！");
            officeNoObj.focus();
            return false;
        } else {
            var officeNoPattern = /^[a-zA-Z]{3}[0-9]{3}$/;
            if (officeNo.length > 10 ||!officeNoPattern.test(officeNo)) {
                alert("Office号格式错误！");
                officeNoObj.select();
                return false;
            }
        }
        var siObj = $("#txtSI");
        var si = $.trim(siObj.val());
        if (officeNo == "") {
            alert("请输入SI: 工作号/密码！");
            siObj.focus();
            return false;
        } else {
            if (si.length > 50) {
                alert("工作号/密码格式错误！");
                siObj.select();
                return false;
            }
        }
    });
})