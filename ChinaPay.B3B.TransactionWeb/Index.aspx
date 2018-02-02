<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.Index"%>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
     
    <script type="text/javascript" language="javascript">
        if (window.top != window.self) {
            window.top.location = window.location;
        }
    </script>
</head>    <link href="/Styles/flightQuery.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    var skinPath = '<%=SkingPath %>';
</script>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div id="bd" style="width:98%">
            <div id="sider" class="fl">
                <div runat="server" id="divMenu" EnableViewState="False">
                </div>
            </div>
            <div class="flow" id="divBody">
                <iframe frameborder="0" scrolling="no" align="middle" onload="this.height=100" noresize=""
                    height="587px" name="rightFrame" target="_blank" id="rightFrame" style="float: right;border:1px solid #eee;padding:10px 0.9%;">
                </iframe>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </div>
    <asp:HiddenField runat="server" ID="hidFrameTarget" />
    </form>
    <%--紧急公告 --%>
    <div id="Announce">
        <a id="divOpcial" style="display: none;" data="{type:'pop',id:'div_Announce'}"></a>
        <div id="div_Announce" class="form layer layer2" style="display: none;width:800px;">
            <h4>
                紧急公告 <a href="javascript:;" class="userClose">关闭 </a>
            </h4>
            <div class="layer2TitleBox" style="width:800px;">
                <h3>
                    <span id="lblTitle"></span>
                </h3>
                <span>发布时间：<i id="lblPublishTime">2012-09-02</i></span>
            </div>
            <div class="layer2ContentBox" id="content" style="clear: both;width:800px;">
                <p>
                </p>
            </div>
            <div id="pager" class="obvious-a fr" style="padding: 20px;">
                <a href="javascript:;" class="obvious-a " id="prvIndex">上一条</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a
                    href="javascript:;" id="nextIndex" class="obvious-a ">下一条</a></div>
            <div>
                <div style="text-align: center;width:800px;" class="btns clearfix">
                    <input type="button" value="关闭" class="btn class2 userClose" id="userClose" title="关闭" /></div>
            </div>
        </div>
    </div>
</body>
</html>
<script src="Scripts/widget/common.js" type="text/javascript"></script>
<script src="Scripts/Global.js?20130115" type="text/javascript"></script>
<script src="Scripts/EmergencyAnnounce.js?20130115" type="text/javascript"></script>
<script src="Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#rightFrame").attr("src", $("#hidFrameTarget").val());
        drawMenu();
    });
    function reinitIframe() {
        var iframe = document.getElementById("rightFrame");
        if (iframe) {
            try {
                var bHeight = iframe.contentWindow.document.body.scrollHeight;
                var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
                var height = Math.max(bHeight, dHeight);
                iframe.height = height;
            } catch (ex) { }
        }
    }
    window.setInterval("reinitIframe()", 200);
</script>