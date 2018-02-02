<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_lightblue.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.Authentication.lightblue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OEM-02</title>
</head>
<link href="/Styles/public.css" rel="stylesheet" type="text/css" />
<link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/Styles/oemlogin2.css" />
<link rel="stylesheet" type="text/css" href="/Styles/d.cutover.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    if (window.top != window.self) {
        window.top.location = window.location;
    }
</script>
<body>
    <form runat="server" id="form1">
    <div class="O-wrap">
        <div class="O-header-bg">
            <div class="O-header">
                <a href="#" class="O-logo">
                    <img src="/Images/logo.png" alt="" id="imgLogo" style="width: 220px; height: 30px;"
                        runat="server" /></a>
                <div class="O-bar">
                    <p>
                        <a href="#">首页</a><a href="#">关于我们</a><a href="#">联系我们</a><span class="O-phone">客服热线:<asp:Literal
                            runat="server" ID="lblServicePhone"></asp:Literal></span></p>
                    <div class="O-ui">
                        <a href="/OrganizationModule/Register/Register.aspx" runat="server" id="registerPage"
                            class="O-register">注册</a> <a href="#this" onclick='setHomePage(this);'>设为首页</a>
                        | <a href="javascript:addFavorite('我的机票平台')">加入收藏</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="O-content">
            <div class="O-login-box">
                <div class="O-login-area">
                    <h4>
                        登录</h4>
                    <div class="O-login-info-box">
                        <div class="O-login-info">
                            <label>
                                用户名：<br />
                                <asp:TextBox runat="server" ID="txtUserName" CssClass="text" TabIndex="1"></asp:TextBox></label>
                        </div>
                        <div class="O-login-info">
                            <label>
                                密&nbsp;&nbsp;&nbsp;码：<br />
                                <asp:TextBox runat="server" ID="txtPassword" CssClass="text" TextMode="Password"
                                    TabIndex="2"></asp:TextBox></label>
                            <a href="/About/LostPassword.aspx" class="O-forget-pw">忘记密码？</a>
                        </div>
                        <div class="O-login-info last">
                            <label>
                                验证码：<br />
                                <asp:TextBox ID="txtCode" runat="server" CssClass="text" Width="90px" TabIndex="3"></asp:TextBox>
                            </label>
                            <img src="javascript:void(0);" alt="验证码" id="imgValidateCode" onclick="javascript:loadValidateCode()"
                                title="换一张" />
                        </div>
                    </div>
                    <div style="margin-bottom: 5px; position: relative;">
                        <asp:CheckBox runat="server" ID="chkJizhu" Text="记住密码" CssClass="O-save-pw" tabIndex="4" />
                        <div id="ErrorMsg" class="O-warning">
                            &nbsp;
                        </div>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnLogon" CssClass="O-login-btn" OnClientClick="return logonValidate();"
                    Text="登&nbsp;&nbsp;录" TabIndex="5" OnClick="btnLogon_Click" />
                <a href="/OrganizationModule/Register/Register.aspx" id="btnRegister" class="O-register-btn"
                    runat="server">免费注册</a>
            </div>
            <div class="clearfix O-con">
                <div class="O-tips-box O-tips-a">
                    <h4>
                        机票天天都特惠</h4>
                    <p>
                        普通/特价/特殊票任选哦</p>
                </div>
                <div class="O-tips-box O-tips-b">
                    <h4>
                        出票速度快</h4>
                    <p>
                        B2B/BSP均可出票</p>
                </div>
                <div class="O-tips-box O-tips-c">
                    <h4>
                        平台返点高</h4>
                    <p>
                        供应商众多 返点有保障</p>
                </div>
                <div class="O-tips-box O-tips-d">
                    <h4>
                        客服快速响应</h4>
                    <p>
                        7x24小时在线客服待命</p>
                </div>
                <div class="O-tips-box O-tips-e">
                    <h4>
                        资金安全保障</h4>
                    <p>
                        国付通账务管理系统</p>
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
                        <asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>版权所以&nbsp;&nbsp;&nbsp;团结奉献·合作共赢</p>
                    <p runat="server" id="Copyright">
                        Copyright:&copy;2012-2020 All Right Reserved</p>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/setting.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#btnRegister").size() == 0) {
            $("#btnLogon").width("230px");
        } else {
            $("#btnLogon").width("100px");
        }
        if (typeof msg != "undefined") {
            ShowMsg(msg);
        }
    })
    function loadValidateCode() {
        $("#imgValidateCode").attr("src", '/ValidateCode.aspx?' + Math.random());
    }
    function logonValidate() {
        if ($("#txtUserName").val() == "") {
            ShowMsg("请输入帐号！");
            $("#txtUserName").focus();
            return false;
        }
        if ($("#txtPassword").val() == "") {
            ShowMsg("请输入密码！");
            $("#txtPassword").focus();
            return false;
        }
        if ($("#txtCode").val() == "") {
            ShowMsg("请输入验证码！");
            $("#txtCode").focus();
            return false;
        }

        return true;
    }
    function ShowMsg(msg) {
        $("#ErrorMsg").text(msg);
    }
    function ClearMsg() {
        $("#ErrorMsg").empty();
    }
    $(function () {
        loadValidateCode();
    });
</script>
