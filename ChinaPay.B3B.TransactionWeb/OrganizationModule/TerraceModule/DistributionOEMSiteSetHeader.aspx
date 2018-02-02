<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteSetHeader.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOEMSiteSetHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>页头页脚设置</title>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        页头页脚设置</h3>
    <div class="O_formBox">
        <span>网站关键词：</span><br />
        <input type="text" id="txtKeyWord" runat="server" class="text" />
        <span class="muted">多个关键词以半角短号隔开，利于搜索引擎收录和抓取</span>
    </div>
    <div class="O_formBox">
        <span>网站描述：</span><br />
        <input type="text" id="txtKeyDes" runat="server" class="text" />
        <span class="muted">在搜索引擎中显示您的网站描述，利于搜索引擎等抓取</span>
    </div>
    <%--<div class="O_formBox">
        <span>logo设置：</span><br />
        <input type="file" class="text" />
        <span class="muted">建议使用200x30像素的png图片，最大体积不能超过50kb</span>
    </div>--%>
    <div class="O_formBox headerdiv link_text" id="divHeader" runat="server">
        <span>增加头部链接：</span>
        <div>
            <input type="text" placeholder="链接名称" class='text text-s' value="链接名称" />
            <input type="text" placeholder="链接地址" class='text' value="链接地址" />
            <a class='add'>+</a><span class="muted">请注意控制头部链接的数量，链接太多将会影响用户体验</span>
        </div>
    </div>
    <div class="O_formBox footerdiv link_text" id="divFooter" runat="server">
        <span>底部链接管理：</span>
        <div>
            <input type="text" placeholder="链接名称" class="text text-s" value="链接名称" />
            <input type="text" placeholder="链接地址" class="text" value="链接地址" />
            <a class="add">+</a><span class="muted">请注意控制页脚链接的数量，链接太多将会影响用户体验</span>
        </div>
    </div>
    <div class="O_formBox">
        <span>底部背景色：</span><br />
        <input type="text" id="txtBGColor" class="text" runat="server" />
        <span id="txtShowBGColor" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
        <%--<input type="text" class="text text-s1" id="txtShowBGColor" runat="server" readonly="readonly" />--%>
        <span class="muted">输入16进制颜色#RRGGBB</span> <span id="msg" style="color: Red"></span>
    </div>
    <div class="O_formBox">
        <span>底部版权信息：</span><br />
        <input type="text" id="txtCopyright" runat="server" class="text" />
        <span class="muted">版权信息将显示在底部链接的下一行</span>
    </div>
    <asp:HiddenField runat="server" ID="hidOemdId" />
    <asp:HiddenField runat="server" ID="hidValue" />
    <input type="button" id="btnSave" value="提交" class="btn class1" />
    <input type="button" id="btnCancel" value="取消" class="btn class2" runat="server"/>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/OrganizationModule/TerraceModule/DistributionOEMSiteSetFooter.js"></script>
<script type="text/javascript">
    $(function () {
        var reg = /^#?([\w]{3}|[\w]{6})$/;
        $("#txtBGColor").blur(function () {
            if (!reg.test($("#txtBGColor").val())) {
                $("#txtBGColor").select();
                $("#msg").html("请输入正确的颜色值！");
            } else {
                $("#msg").html("");
            }
            $("#txtShowBGColor").css("backgroundColor", "#" + ($("#txtBGColor").val().replace("#", "")));
        });

        $("#btnCancel").click(function () {
            window.location.href = 'DistributionOemAuthorizationList.aspx?Search=Back';
        });
        $("#btnSave").click(function () {
            if (Vail()) {
                var headerLinks = new Array();
                var footerLinks = new Array();
                for (var i = 0; i < $(".headerdiv div").length; i++) {
                    headerLinks.push({ "SettingId": $("#hidSettingId").val(), "LinkName": $(".headerdiv div").eq(i).find("input[type='text']").eq(0).val() == $(".headerdiv div").eq(i).find("input[type='text']").eq(0).attr("placeholder") ? "" : $(".headerdiv div").eq(i).find("input[type='text']").eq(0).val(), "URL": $(".headerdiv div").eq(i).find("input[type='text']").eq(1).val() == $(".headerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") ? "" : $(".headerdiv div").eq(i).find("input[type='text']").eq(1).val(), "LinkType": "1", "Remark": "" });
                }
                for (var i = 0; i < $(".footerdiv div").length; i++) {
                    footerLinks.push({ "SettingId": $("#hidSettingId").val(), "LinkName": $(".footerdiv div").eq(i).find("input[type='text']").eq(0).val() == $(".footerdiv div").eq(i).find("input[type='text']").eq(0).attr("placeholder") ? "" : $(".footerdiv div").eq(i).find("input[type='text']").eq(0).val(), "URL": $(".footerdiv div").eq(i).find("input[type='text']").eq(1).val() == $(".footerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") ? "" : $(".footerdiv div").eq(i).find("input[type='text']").eq(1).val(), "LinkType": "2", "Remark": "" });
                }
                sendPostRequest("/OrganizationHandlers/DistrbutionOEM.ashx/UpdateHeaderifno", JSON.stringify({ "setting": { "SiteKeyWord": $("#txtKeyWord").val(), "SiteDescription": $("#txtKeyDes").val(), "BGColor": $("#txtBGColor").val(), "CopyrightInfo": $("#txtCopyright").val(), "HeaderLinks": headerLinks, "FooterLinks": footerLinks }, "oemid": $("#hidOemdId").val() }), function (e) {
                    if (e == "1") {
                        if ($("#hidValue").val() == "1") {
                            alert("设置成功，刷新页面将生效。");
                        } else {
                            alert("设置成功");
                            window.location.href = 'DistributionOemAuthorizationList.aspx?Search=Back';
                        }
                    } else {
                        alert("设置失败，失败原因：" + e);
                    }
                }, function (e) {
                    alert(e.responseText);
                });
            }
        });
    });
    function Vail() {
        if ($("#txtKeyWord").val() == "") {
            alert("网站关键字不能为空");
            $("#txtKeyWord").select();
            return false;
        }
        if ($("#txtKeyDes").val() == "") {
            alert("网站描述不能为空");
            $("#txtKeyDes").select();
            return false;
        }
        var falgName = true;
        var falgUrl = true;
        var index = 0;
        var reg = /http:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
        for (var i = 0; i < $(".headerdiv div").length; i++) {
            if ($(".headerdiv div").eq(i).find("input[type='text']").eq(0).val() != "" && $(".headerdiv div").eq(i).find("input[type='text']").eq(0).val() != $(".headerdiv div").eq(i).find("input[type='text']").eq(0).attr("placeholder") && $(".headerdiv div").eq(i).find("input[type='text']").eq(0).val().length >= 15) {
                $(".headerdiv div").eq(i).find("input[type='text']").eq(0).select();
                index = i;
                falgName = false;
                break;
            }
            if ($(".headerdiv div").eq(i).find("input[type='text']").eq(1).val() != "" && $(".headerdiv div").eq(i).find("input[type='text']").eq(1).val() != $(".headerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") && $(".headerdiv div").eq(i).find("input[type='text']").eq(1).val().length >= 100) {
                $(".headerdiv div").eq(i).find("input[type='text']").eq(1).select();
                index = i;
                falgUrl = false;
                break;
            }
            if ($(".headerdiv div").eq(i).find("input[type='text']").eq(1).val() != "" && $(".headerdiv div").eq(i).find("input[type='text']").eq(1).val() != $(".headerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") && !reg.test($(".headerdiv div").eq(i).find("input[type='text']").eq(1).val())) {
                index = i;
                falgUrl = false;
                break;
            }
        }
        if (!falgName) {
            alert("页头连接名称第 " + (parseInt(index) + 1) + " 项格式错误，只能输入15个字以内的连接名称！");
            return falgName;
        }
        if (!falgUrl) {
            alert("页头连接地址第 " + (parseInt(index) + 1) + " 项格式错误，只能输入100个字以内的连接地址，且只能为http://开始！");
            return falgUrl;
        }
        for (var i = 0; i < $(".footerdiv div").length; i++) {
            if ($(".footerdiv div").eq(i).find("input[type='text']").eq(0).val() != "" && $(".footerdiv div").eq(i).find("input[type='text']").eq(0).val() != $(".footerdiv div").eq(i).find("input[type='text']").eq(0).attr("placeholder") && $(".footerdiv div").eq(i).find("input[type='text']").eq(0).val().length >= 15) {
                $(".footerdiv div").eq(i).find("input[type='text']").eq(0).select();
                index = i;
                falgName = false;
                break;
            }
            if ($(".footerdiv div").eq(i).find("input[type='text']").eq(1).val() != "" && $(".footerdiv div").eq(i).find("input[type='text']").eq(1).val() != $(".footerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") && $(".footerdiv div").eq(i).find("input[type='text']").eq(1).val().length >= 100) {
                $(".footerdiv div").eq(i).find("input[type='text']").eq(1).select();
                index = i;
                falgUrl = false;
                break;
            }
            if ($(".footerdiv div").eq(i).find("input[type='text']").eq(1).val() != "" && $(".footerdiv div").eq(i).find("input[type='text']").eq(1).val() != $(".footerdiv div").eq(i).find("input[type='text']").eq(1).attr("placeholder") && !reg.test($(".footerdiv div").eq(i).find("input[type='text']").eq(1).val())) {
                index = i;
                falgUrl = false;
                break;
            }
        }
        if (!falgName) {
            alert("底部连接名称第 " + (parseInt(index) + 1) + " 项格式错误，只能输入15个字以内的连接名称！");
            return falgName;
        }
        if (!falgUrl) {
            alert("底部连接地址第 " + (parseInt(index) + 1) + " 项格式错误，只能输入100个字以内的连接地址，且只能为http://开始！");
            return falgUrl;
        }
        if ($("#txtBGColor").val() == "") {
            alert("底部背景色不能为空！");
            return false;
        }
        if ($("#txtCopyright").val() == "") {
            alert("底部版权信息不能为空！");
            return false;
        }
        return true;
    }

</script>
