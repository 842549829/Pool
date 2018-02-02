<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login-un.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.Authentication.Login_un" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OEM-02</title>
    <link href="/Styles/core.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/form.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/public.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/page.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Styles/oemlogin2.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/d.cutover.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form runat="server" id="form1">
    <div class="O-wrap">
        <div class="O-header-bg">
            <div class="O-header">
                <a href="#" class="O-logo">
                    <img src="/images/oem/logo2.png" alt="oem-01" /></a>
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
                    <div style="margin-bottom:5px;position:relative;">
                        <asp:CheckBox runat="server" ID="chkJizhu" Text="记住密码" CssClass="O-save-pw" />
                        <div id="ErrorMsg" class="O-warning">
                            &nbsp;
                        </div>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnLogon" CssClass="O-login-btn" OnClientClick="return logonValidate();"
                    Text="登&nbsp;&nbsp;录" TabIndex="4" OnClick="btnLogon_Click" />
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
                        <a href="#" class="O-fight-com">合作航空公司</a>
                    </div>
                    <div class="O-collaborate">
                        <h4>
                            合作机构及网站</h4>
                        <a href="#" class="O-other-com">合作机构及网站</a>
                    </div>
                </div>
                <div class="O-copyright">
                    <p>
                        <asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>版权所以&nbsp;&nbsp;&nbsp;团结奉献·合作共赢</p>
                    <p>
                        Copyright:&copy;2012-2020
                        <asp:Literal runat="server" ID="lblDomainName"></asp:Literal>
                        All Right Reserved</p>
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
