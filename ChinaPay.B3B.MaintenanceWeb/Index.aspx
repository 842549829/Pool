<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.Index"EnableSessionState="ReadOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B运维管理系统</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="indexForm" runat="server">
        <div class="banner" id="divBanner">
            <div class="logo"></div>
            <div class="welcome">
                <p>
                    <span>您好，<asp:Label ID="lblAccount" runat="server"></asp:Label>欢迎登录B3B运维平台</span>
                    <span>
                        <asp:Label ID="lblYear" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="lblWeek" runat="server"></asp:Label>
                    </span>
                    <em><a href="#" onclick="logout();">注销</a></em>
                </p>
            </div>
            <div class="clear"></div>
        </div>
        <!--center -->
        <div class="center" id="w2500">
            <!--left -->
            <div class="sidebar">
                <div id="firstpane" runat="server" class="menu_list">
                    <%--<p class="menu_head">
                        <span>
                            <img src="../images/menu_icon.png" alt=""/></span><em><a href="#"><img src="../images/empty.gif"
                                height="11" width="12" alt="" /></a></em>基础数据管理
                    </p>
                    <div class="menu_body">
                        <a href="BasicData/Province.aspx" target="rightFrame">省份代码维护</a> 
                        <a href="BasicData/City.aspx"target="rightFrame">城市代码维护</a>
                        <a href="BasicData/County.aspx" target="rightFrame">县城代码维护</a> 
                        <a href="BasicData/Airport.aspx" target="rightFrame">机场代码维护</a> 
                        <a href="BasicData/Airline.aspx" target="rightFrame">航空公司维护</a> 
                        <a href="BasicData/BasePrice.aspx"target="rightFrame">基础运价维护</a>
                        <a href="BasicData/Bunk.aspx" target="rightFrame">舱位代码维护</a>
                        <a href="BasicData/ChildTicketMaintain.aspx" target="rightFrame">儿童可预订舱位维护</a> 
                        <a href="BasicData/Fuel.aspx"target="rightFrame">燃油附加费维护</a>
                        <a href="BasicData/PlaneType.aspx" target="rightFrame">机型代码维护</a> 
                        <a href="BasicData/RefundChangeTicket.aspx" target="rightFrame">退改签规定维护</a>
                    </div>
                    <p class="menu_head">
                        <span>
                            <img src="../images/menu_icon.png" alt="" /></span><em><a href="#"><img src="../images/empty.gif"
                                height="11" width="12" alt="" /></a></em>系统管理
                    </p>
                    <div class="menu_body">
                        <a href="SystemManagement/Dictionary.aspx" target="rightFrame">字典表设置</a>
                        <a href="SystemManagement/SystemParameter.aspx" target="rightFrame">系统参数设置</a>
                    </div>
                    <p class="menu_head">
                        <span>
                            <img src="../images/menu_icon.png" alt="" /></span><em><a href="#"><img src="../images/empty.gif"
                                height="11" width="12" alt="" /></a></em>日志管理</p>
                    <div class="menu_body">
                        <a href="Log/Tradement.aspx" target="rightFrame">交易交互日志</a>
                        <a href="Log/Logon.aspx" target="rightFrame">登录日志</a>
                        <a href="Log/OperationLog.aspx" target="rightFrame">操作日志</a>
                        <a href="Log/ExceptionLog.aspx" target="rightFrame">错误日志</a>
                    </div>--%>
                </div>
            </div>
            <div style="margin-left:200px;">
            <iframe frameborder="0" scrolling="no" align="center" onload="this.height=100" noresize="" height="587px" name="rightFrame" 
                target="_blank" id="rightFrame" style="width:100%;">
            </iframe>
            </div>
            <div class="clear"></div>
        </div>
    </form>
</body>
</html>
<script src="js/jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#firstpane p.menu_head").click(function () {
            $(this).next("div.menu_body").slideToggle(300).siblings("div.menu_body").slideUp("slow");
            $(this).siblings();
        });
        // 当前位置样式 
        $("#firstpane a").click(function () {
            $("#firstpane a").removeClass("on");
            $(this).attr("class", "on");
        });
    });
    function reinitIframe() {
        var iframe = document.getElementById("rightFrame");
        try {
            var bHeight = iframe.contentWindow.document.body.scrollHeight;
            var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
            var height = Math.max(bHeight, dHeight);
            iframe.height = height;
        } catch (ex) { }
    }
    window.setInterval("reinitIframe()", 200);
    function logout() {
        $.ajax({
            type: "POST",
            url: "../Logout.ashx",
            data: ""
        });
        window.location.href = "../Logon.aspx";
    }
    function pageHeight() {
        if ($.browser.msie) {
            return document.compatMode == "CSS1Compat" ? document.documentElement.clientHeight : document.body.clientHeight;
        } else {
            return self.innerHeight;
        }
    }
</script>