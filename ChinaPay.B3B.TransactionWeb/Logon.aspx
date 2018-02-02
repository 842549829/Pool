<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.Logon" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>B3B机票交易平台 中国首家创新型电子机票销售平台 全球首家任意编码行程支持平台</title>
    <meta name="description" content="B3B机票交易平台是国内领先的机票行业同行购票平台，政策好、返点高、政策齐全，支持单程/往返/联程/缺口程的团队及散客票、特价票、特殊票交易，是中国机票销售平台的领导者与创新旗舰平台；是中国首家任意行程编码完美支持平台！欢迎加入B3B与我们一起协同创富！详询400-739-0838。" />
    <meta name="keywords" content="国内机票,B2B机票,机票平台,机票同行,特殊票,免票,散冲团,集团票,商旅卡,b3b,b3b机票,b3b机票交易,电子客票,黄牛机票,打折机票,特价机票,免费机票,卖机票,机票分销,机票高返" />
    
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
</head>    <link href="Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="Styles/skin.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Styles/d.cutover.css" />

<body onresize="ReSet()">
    <form id="form1" runat="server">
    <div class="wrap">
        <!--[if lte IE 6]>
        <div>
            <p style="padding:20px 10px;background-color:#ddd;color:red;line-height:20px;">Hi,您的IE浏览器版本过低，请升级您的浏览器，以享受优质的浏览效果！<br />
            <a style="padding-right:20px;color:#4EB0E9;" href="https://dl.google.com/tag/s/appguid%3D%7B8A69D345-D564-463C-AFF1-A69D9E530F96%7D%26iid%3D%7BFC6D60AE-D878-84BF-19BF-BBB1BC4239D3%7D%26lang%3Dzh-CN%26browser%3D4%26usagestats%3D0%26appname%3DGoogle%2520Chrome%26needsadmin%3Dprefers%26installdataindex%3Ddefaultbrowser/update2/installers/ChromeSetup.exe">google浏览器下载</a>
            <a style="padding-right:20px;color:#4EB0E9;" href="http://download.firefox.com.cn/releases/webins3.0/official/zh-CN/Firefox-latest.exe">firefix浏览器下载</a>
            <a style="padding-right:20px;color:#4EB0E9;" href="http://download.microsoft.com/download/1/6/1/16174D37-73C1-4F76-A305-902E9D32BAC9/IE8-WindowsXP-x86-CHS.exe">IE8浏览器下载</a></p>
        </div>
        <![endif]-->
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div class="loginAreaBox">
            <div class="loginArea">
                <h4>
                    登录<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal></h4>
                <p>
                    用户名</p>
                <div class="loginBorderBox">
                    <asp:TextBox runat="server" ID="txtUserName" TabIndex="1"></asp:TextBox>
                </div>
                <p>
                    密码<a href="/About/LostPassword.aspx">忘记密码？</a></p>
                <div class="loginBorderBox">
                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" TabIndex="2"></asp:TextBox>
                    <span class="unlock"></span>
                </div>
                <p>
                    验证码</p>
                <div class="clearfix">
                <div class="loginBorderBox1">
                    <asp:TextBox ID="txtCode" runat="server" TabIndex="3"></asp:TextBox>
                </div>
                <img src="" alt="验证码" id="imgValidateCode" class="checkCore" onclick="javascript:loadValidateCode()"
                    title="换一张" />
                    </div>
                 <div class="clearfix">
                    <div class="fl" id="divJizhu"style="width:31%;margin-top:10px;">
                        <asp:CheckBox runat="server" TabIndex="4" ID="chkJizhu"  Text="记住密码"/></div>
                    <div class="ErrorTipEmpty fl" style="width:64%;" id="ErrorMsg">
                        &nbsp;</div>
                 </div>
                <div class="loginBtnBox clearfix">
                    <asp:Button runat="server" ID="btnLogon" CssClass="loginBtn" OnClientClick="return logonValidate();"
                        OnClick="btnLogon_Click" Text="登&nbsp;&nbsp;录" TabIndex="5" />
                    <a href="/OrganizationModule/Register/Register.aspx" id="btnRegister" runat="server">免费注册</a>
                </div>
                <p class="phone">
                    客服电话：<asp:Label runat="server" ID="lblServicePhone"></asp:Label><%--<span id="copy">复制</span>--%></p>
            </div>
        </div>
        <div class="loginBgBox">
            <div class="focus">
                <ul>
                    <li><a href="/banner/banner_summer.aspx" id="loginBigPic1"></a></li>
                    <li><a href="/banner/banner_zhaoshang.aspx" id="loginBigPic2"></a></li>
                    <li><a href="/banner/banner_credit.aspx" id="loginBigPic3"></a></li>
                    <li><a href="/banner/banner_pos.aspx" id="loginBigPic4"></a></li>
                    <li><a href="/banner/banner_bianma.aspx" id="loginBigPic5"></a></li>
                </ul>
            </div>
        </div>
        <div class="loginSubBox">
            <div class="recommend">
                <div class="recommendTitle">
                    <h4 class="header">
                        特价推荐</h4>
                    <ul id="divRecommendCities">
                    </ul>
                </div>
                <ul class="clearfix recommendList" id="divRecommentContents">
                </ul>
            </div>
            <div class="bigBox">
                <div class="smallBox">
                    <div class="loginSmallBox">
                        <div class="smallPicBox">
                            <ul>
                                <li><a href="/banner/banner_zhaoshang.aspx" id="loginSmallPic4"></a></li>
                                <li><a href="/banner/banner_jifen.aspx" id="loginSmallPic1"></a></li>
                                <li><a href="/banner/banner_service.aspx" id="loginSmallPic2"></a></li>
                                <li><a href="/banner/banner_introduce.aspx" id="loginSmallPic3"></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="background-color: #ededed; height: 120px; margin: 15px 0 0 0;">
            <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
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
<script src="Scripts/widget/common.js" type="text/javascript"></script>
<script src="Scripts/widget/template.js" type="text/javascript"></script>
<script src="Scripts/swfobject_modified.js" type="text/javascript"></script>
<script src="Scripts/logon.js?20130403" type="text/javascript"></script>
<script type="text/javascript" src="Scripts/jfocus.js"></script>
<script type="text/javascript">
    $(function () {
        if ($("#btnRegister").size() ==0) {
            $("#btnLogon").width("250px");
        } else {
            $("#btnLogon").width("150px");
        }
    })
</script>