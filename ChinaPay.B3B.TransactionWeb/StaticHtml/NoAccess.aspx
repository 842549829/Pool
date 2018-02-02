<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="NoAccess.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.StaticHtml.NoAccess" %>

<%@ Import Namespace="ChinaPay.B3B.Common.Enums" %>
<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>您没有权限访问该文件或目录</title>
 </head>
   <link rel="stylesheet" type="text/css" href="/Styles/error.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        if (window.top != window.self) {
            window.top.location = window.location;
        }
        function action() {
            window.location.href = "/Default.aspx";
        }
        setTimeout(action, 5000);
    </script>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div id="bd">
            <div class="errorBg" id="limits">
                <div id="limitsTitle">
                    <h4>
                        您没有权限访问该文件或目录</h4>
                    <span>请确认您已经登录或有权限访问该页面 </span>
                    <br />
                    <br />
                    系统将在5秒之后自动返回首页，如没有跳转请点击 <a href="/Index.aspx" class="obvious">&lt;&lt;返回首页</a>
                </div>
                <div id="limitsContent">
                    <ul>
                        <li>在地址中可能存在键入错误。</li>
                        <li>当您点击某个链接时，它可能已过期。</li>
                        <li>请检查您输入的网址是否正确</li>
                    </ul>
                </div>
                <div id="limitsLink">
                    <span>您是否正寻找： <a href="/OrganizationModule/Register/Register.aspx">注册B3B</a> <a href="/PurchaseDefault.aspx">
                        购买机票</a> <a href="/About/OnLineService.aspx">在线客服</a> <a href="/About/jrwm.aspx">申请合作</a> </span>
                </div>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/setting.js" type="text/javascript"></script>
