<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_blue.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.Authentication.Login_blue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta charset="utf-8" />
</head>
<link href="/Styles/public.css" rel="stylesheet" type="text/css" />
<link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/Styles/oemlogin1.css" />
<link rel="stylesheet" type="text/css" href="/Styles/d.cutover.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    if (window.top != window.self) {
        window.top.location = window.location;
    }
    function ReSet() {
        $(".jfocus").css("width", "100%");
    };
    var hideSuggest = true;
</script>
<body>
    <form id="form1" runat="server">
    <div class="O-wrap">
        <!--[if lte IE 6]>
        <div>
            <p style="padding:20px 10px;background-color:#ddd;color:red;line-height:20px;">Hi,您的IE浏览器版本过低，请升级您的浏览器，以享受优质的浏览效果！<br />
            <a style="padding-right:20px;color:#4EB0E9;" href="https://dl.google.com/tag/s/appguid%3D%7B8A69D345-D564-463C-AFF1-A69D9E530F96%7D%26iid%3D%7BFC6D60AE-D878-84BF-19BF-BBB1BC4239D3%7D%26lang%3Dzh-CN%26browser%3D4%26usagestats%3D0%26appname%3DGoogle%2520Chrome%26needsadmin%3Dprefers%26installdataindex%3Ddefaultbrowser/update2/installers/ChromeSetup.exe">google浏览器下载</a>
            <a style="padding-right:20px;color:#4EB0E9;" href="http://download.firefox.com.cn/releases/webins3.0/official/zh-CN/Firefox-latest.exe">firefix浏览器下载</a>
            <a style="padding-right:20px;color:#4EB0E9;" href="http://download.microsoft.com/download/1/6/1/16174D37-73C1-4F76-A305-902E9D32BAC9/IE8-WindowsXP-x86-CHS.exe">IE8浏览器下载</a></p>
        </div>
        <![endif]-->
        <div class="O-header-bg">
            <div class="O-header">
                <a href="#" class="O-logo">
                    <img src="/Images/logo.png" alt="" id="imgLogo" style="width: 220px; height: 30px;"
                        runat="server" /></a>
                <div class="O-bar">
                    <p>
                        <asp:Label ID="lblPlatformNameTitle" runat="server"></asp:Label>欢迎您 |<span class="O-phone">客服热线:<asp:Label
                            ID="lblServicePhone" runat="server"></asp:Label></span></p>
                    <div class="O-ui">
                        <a href="/OrganizationModule/Register/Register.aspx" id="lblRegister" runat="server"
                            class="O-register">注册</a> <a href="#this" onclick='setHomePage(this);'>设为首页</a>
                        | <a href="#this" onclick="addFavorite('我的机票平台');">加入收藏</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="O-content">
            <div class="O-login-box">
                <div class="O-login-area">
                    <h4>
                        登录<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal></h4>
                    <div class="O-line">
                    </div>
                    <div class="O-login-info-box">
                        <div class="O-login-info">
                            <label>
                                用户名：<asp:TextBox runat="server" ID="txtUserName" CssClass="text" TabIndex="1"></asp:TextBox></label>
                        </div>
                        <div class="O-login-info">
                            <label>
                                密&nbsp;&nbsp;&nbsp;码：
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="text" TabIndex="2"></asp:TextBox></label>
                            <a href="/About/LostPassword.aspx" class="O-forget-pw" >忘记密码？</a>
                        </div>
                        <div class="O-login-info last">
                            <label>
                                验证码：
                                <asp:TextBox ID="txtCode" runat="server" CssClass="text" Style="width: 90px;"  TabIndex="3"></asp:TextBox>
                            </label>
                            <img src="" id="imgValidateCode" onclick="javascript:loadValidateCode()" title="换一张">
                        </div>
                    </div>
                    <div class="O-line">
                    </div>
                    <div style="position: relative;">
                        <label class="O-save-pw">
                            <asp:CheckBox runat="server" TabIndex="4" ID="chkJizhu" Text="记住密码" /></label>
                        <div class="O-warning" id="ErrorMsg">
                            &nbsp;</div>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnLogon" CssClass="O-login-btn" OnClientClick="return logonValidate();"
                    OnClick="btnLogon_Click" Text="登&nbsp;&nbsp;录" TabIndex="5" />
                <a href="/OrganizationModule/Register/Register.aspx" id="btnRegister" class="O-register-btn"  tabIndex="6"
                    runat="server">免费注册</a>
            </div>
            <div class="clearfix O-con">
                <div class="O-recommend-box">
                    <h4 class="O-title">
                        特价推荐</h4>
                    <ul class="O-recommend-menu clearfix" id="divRecommendCities">
                    </ul>
                    <ul class="O-recommend-con clearfix" id="divRecommentContents">
                    </ul>
                </div>
                <div class="O-aim-box">
                    <h4 class="O-title">
                        团结奋进·合众连横</h4>
                    <ul class="O-aim">
                        <li>官方渠道·值得信赖
                            <div class="O-line">
                            </div>
                        </li>
                        <li>省内精英供应商云集<div class="O-line">
                        </div>
                        </li>
                        <li>众多航线政策第一<div class="O-line">
                        </div>
                        </li>
                        <li>携手共创美好明天</li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="O-footer-bg">
            <div class="O-footer">
                <div class="clearfix">
                    <div class="O-collaborate">
                        <h4>
                            合作航空公司</h4>
                        <div class="clearfix">
                            <a href="http://www.xiamenair.com.cn/" target="_blank" class="O-fight-link O-fight-ico1"
                                title="厦门航空">厦门航空</a> <a href="http://www.airchina.com.cn/" target="_blank" class="O-fight-link O-fight-ico2"
                                    title="中国国际航空">中国国际航空</a> <a href="http://www.shandongair.com.cn/" target="_blank"
                                        class="O-fight-link O-fight-ico3" title="山东航空">山东航空</a> <a href="http://www.tianjin-air.com/"
                                            target="_blank" class="O-fight-link O-fight-ico4" title="天津航空">天津航空</a>
                            <a href="http://www.scal.com.cn/" target="_blank" class="O-fight-link O-fight-ico5"
                                title="四川航空">四川航空</a> <a href="http://www.airkunming.com/" target="_blank" class="O-fight-link O-fight-ico6"
                                    title="昆明航空">昆明航空</a> <a href="http://bk.travelsky.com/" target="_blank" class="O-fight-link O-fight-ico7"
                                        title="奥凯航空">奥凯航空</a> <a href="http://www.shenzhenair.com/" target="_blank" class="O-fight-link O-fight-ico8"
                                            title="深圳航空">深圳航空</a> <a href="http://www.tibetairlines.com.cn/" target="_blank"
                                                class="O-fight-link O-fight-ico9" title="西藏航空">西藏航空</a>
                        </div>
                        <div class="clearfix">
                            <a href="http://www.ceair.com/" target="_blank" class="O-fight-link O-fight-ico10"
                                title="东方航空">东方航空</a> <a href="http://www.csair.com/" target="_blank" class="O-fight-link O-fight-ico11"
                                    title="南方航空">南方航空</a> <a href="http://www.hnair.com/" target="_blank" class="O-fight-link O-fight-ico12"
                                        title="海南航空">海南航空</a> <a href="http://www.chinawestair.com/" target="_blank" class="O-fight-link O-fight-ico13"
                                            title="西部航空">西部航空</a> <a href="http://www.china-sss.com/" target="_blank" class="O-fight-link O-fight-ico14"
                                                title="春秋航空">春秋航空</a> <a href="http://www.hbhk.com.cn/" target="_blank" class="O-fight-link O-fight-ico15"
                                                    title="河北航空">河北航空</a> <a href="http://www.juneyaoair.com/" target="_blank" class="O-fight-link O-fight-ico16"
                                                        title="吉祥航空">吉祥航空</a> <a href="http://www.luckyair.net/" target="_blank" class="O-fight-link O-fight-ico17"
                                                            title="祥鹏航空">祥鹏航空</a> <a href="http://www.ceair.com/mu/main/sh/index.html" target="_blank"
                                                                class="O-fight-link O-fight-ico18" title="上海航空">上海航空</a></div>
                    </div>
                    <div class="O-collaborate">
                        <h4>
                            合作机构及网站</h4>
                        <div class="clearfix">
                            <a href="http://www.iata.org/" target="_blank" class="O-other-link O-other-ico1"
                                title="国际航空运输协会">国际航空运输协会</a> <a href="http://pay.poolpay.cn/" target="_blank" class="O-other-link O-other-ico2"
                                    title="国付通">国付通</a> <a href="https://www.alipay.com/" target="_blank" class="O-other-link O-other-ico4"
                                        title="支付宝">支付宝</a> <a href="https://www.tenpay.com/" target="_blank" class="O-other-link O-other-ico5"
                                            title="财付通">财付通</a>
                        </div>
                        <div class="clearfix">
                            <a href="http://www.icbc.com.cn/" target="_blank" class="O-other-link O-other-ico6"
                                title="中国工商银行">中国工商银行</a> <a href="http://www.cmbchina.com/" target="_blank" class="O-other-link O-other-ico7"
                                    title="招商银行">招商银行</a> <a href="javascript:;" class="O-other-link O-other-ico8" title="">
                                    </a><a href="http://www.sdb.com.cn/" target="_blank" class="O-other-link O-other-ico9"
                                        title="深圳发展银行">深圳发展银行</a> <a href="http://www.boc.cn/" target="_blank" class="O-other-link O-other-ico10"
                                            title="中国银行">中国银行</a> <a href="http://www.spdb.com.cn/" target="_blank" class="O-other-link O-other-ico11"
                                                title="浦发银行">浦发银行</a> <a href="http://www.cib.com.cn/" target="_blank" class="O-other-link O-other-ico12"
                                                    title="兴业银行">兴业银行</a> <a href="http://www.ccb.com/" target="_blank" class="O-other-link O-other-ico13"
                                                        title="中国建设银行">中国建设银行</a> <a href="http://www.hxb.com.cn/" target="_blank" class="O-other-link O-other-ico14"
                                                            title="华夏银行">华夏银行</a> <a href="http://bank.ecitic.com/ " target="_blank" class="O-other-link O-other-ico15"
                                                                title="中信银行">中信银行</a> <a href="http://www.abchina.com/cn/ " target="_blank" class="O-other-link O-other-ico16"
                                                                    title="中国农业银行">中国农业银行</a> <a href="http://www.bankcomm.com/" target="_blank" class="O-other-link O-other-ico17"
                                                                        title="交通银行">交通银行</a> <a href="http://www.cmbc.com.cn/" target="_blank" class="O-other-link O-other-ico18"
                                                                            title="中国民生银行">中国民生银行</a>
                        </div>
                    </div>
                </div>
                <div class="O-copyright">
                    <p>
                        <asp:Label ID="lblPlatformNameFooter" runat="server"></asp:Label>版权所有&nbsp;&nbsp;&nbsp;团结奉献·合作共赢</p>
                    <p runat="server" id="Copyright">
                        Copyright:&copy;2012-2020 www.oem.com All Right Reserved</p>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/widget/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/setting.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/d.cutover.js"></script>
<script type="text/javascript" src="/Scripts/widget/d.tabs.js"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/widget/template.js" type="text/javascript"></script>
<script src="/Scripts/swfobject_modified.js" type="text/javascript"></script>
<script src="/Scripts/logon.js?20130403" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/jfocus.js"></script>
<script type="text/javascript">
    $(function () {
        if ($("#btnRegister").size() == 0) {
            $("#btnLogon").width("274px");
        } else {
            $("#btnLogon").width("150px");
        }
    })
</script>
