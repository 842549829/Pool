<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ErrorPage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.StaticHtml.ErrorPage" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>非常抱歉，您访问的页面出错了</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        if (window.top != window.self) {
            window.top.location = window.location;
        }
        function action() {
            window.location.href = "/Index.aspx";
        }
        setTimeout(action,5000);
    </script>
<script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link rel="stylesheet" type="text/css" href="/Styles/error.css" />

<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div id="bd">
            <div class="errorBg" id="error">
                <div id="errorTitle">
                    <h4>
                        非常抱歉，您访问的页面出错了</h4> 
                    <span>我们非常抱歉，无法显示您要访问的页面，我们已经记录本次信息<br />
                        技术人员会立即处理本错误<br />
                        系统将在5秒之后自动返回首页，如没有跳转请点击 <a href="/Index.aspx" class="obvious">&lt;&lt;返回首页</a></span> 
                </div>
                <div id="errorLink">
                   <a href="/Index.aspx">&lt;&lt;返回首页</a>
                </div>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/setting.js" type="text/javascript"></script>
