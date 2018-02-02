(function ($) {
    $.fn.extend({
        disabled: function () { $(this).attr("disabled", true); },
        enabled: function () { $(this).attr("disabled", false); },
        showMessage: function (msg, color) { $(this).next("span").text(msg).css("color", color); }
    });
})(jQuery);
//验证账号
function CheckAccount(btn,accountNo) {
    var self = $(btn), account =$.trim($("#"+accountNo).val());
    self.disabled();
    if (/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(account) &&  account.length <= 30) {
        sendPostRequest("/OrganizationHandlers/CheckUpComPanyNews.ashx/CheckAccountNo", JSON.stringify({ "accountNo": account }), function (result) {
            if (result) {
                self.showMessage("很遗憾，该用户名太受欢迎，被人注册了，您换一个吧","red");
            }else{
                 self.showMessage("恭喜，该用户名可以注册","green");
            }
            self.enabled();
        }, function () {
            self.showMessage("很遗憾，该用户名太受欢迎，被人注册了，您换一个吧","red"); self.enabled(); 
        });
    } else {
        self.showMessage("用户名格式错误","red"); self.enabled();
    }
}
//验证公司名称
function CheckCompanyName(btn, accountNo) {
    var self = $(btn), account = $.trim($("#" + accountNo).val());
    if (/^[\u4e00-\u9fa5]{4,25}$/.test(account)) {
        self.disabled();
        sendPostRequest("/OrganizationHandlers/CheckUpComPanyNews.ashx/CheckUpCompanyName", JSON.stringify({ "companyName": account }), function (result) {
            if (result) {
                self.showMessage("该公司名称已经存在", "red");
            } else {
                self.showMessage("该公司名称可使用", "green");
            }
            self.enabled();
        },
        function () {
            self.enabled(); self.showMessage("公司名称已经存在","red");
        })
    } else {
        self.enabled(); self.showMessage("公司名称格式错误","red");
    }
}
//验证公司简称
function CheckAbbreviation(btn, abbreviationName) {
    var self = $(btn),abbreviation = $.trim($("#"+abbreviationName).val());
    if (/^[\u4e00-\u9fa5]{2,10}$/.test(abbreviation)) {
         self.disabled();
         sendPostRequest("/OrganizationHandlers/CheckUpComPanyNews.ashx/CkeckUpCompanyForShort", JSON.stringify({ "companyForShort": abbreviation }), function (result) {
            if (result) {
                self.showMessage("简称已经存在", "red");
            } else {
                self.showMessage("简称可使用", "green");
            }
            self.enabled();
        },
        function () {
            self.enabled(); self.showMessage("简称已经存在","red");
        })
    }else{
        self.enabled(); self.showMessage("简称格式错误","red");
    }
 }