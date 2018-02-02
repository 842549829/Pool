<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteSet.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOEMSiteSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>站点信息</title>
 </head>   <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        站点信息</h3>
    <div class="O_formBox">
        <span>网站名称：</span><br />
        <asp:TextBox runat="server" ID="txtName" CssClass="text"></asp:TextBox>
        <span class="muted">网站名称，将显示在浏览器窗口标题等位置</span>
    </div>
    <div class="O_formBox">
        <span>网站域名：</span><br />
        <asp:TextBox runat="server" ID="txtDomain" CssClass="text"></asp:TextBox>
        <span class="muted">用户可用通过该域名访问您的站点，也作为站内首页跳转URL</span>
    </div>
    <div class="O_formBox">
        <span>logo设置：</span><br />
        <asp:FileUpload runat="server" ID="fileImg" CssClass="text" /><span class="obvious"
            runat="server" style="position: relative;" id="OldImgUrlDiv" visible="false"><a href="javascript:;"
                onmouseover="ShowoldImg();" onmouseout="HideoldImg();">查看原图片</a>
            <img style="position: absolute; display: none; background-color: white; width: 220px;
                height: 30px;" class="box" id="oldimg" />
            <asp:Label runat="server" ID="OldImgUrl"  CssClass="hidden"></asp:Label></span> <span class="muted">建议使用200x30像素的png图片，最大体积不能超过50kb</span>
    </div>
    <div class="O_formBox">
        <span>管理员邮箱：</span><br />
        <asp:TextBox runat="server" ID="txtEmail" CssClass="text"></asp:TextBox>
        <span class="muted">管理员Email，将作为系统发邮件的时候的发件人地址</span>
    </div>
    <div class="O_formBox">
        <span>网站备案信息代码：</span><br />
        <asp:TextBox runat="server" ID="txtICP" TextMode="MultiLine" Rows="3" Columns="60"
            CssClass="text"></asp:TextBox>
        <span class="muted">页面底部可用显示ICP备案信息，在此输入您的授权码</span>
    </div>
    <div class="O_formBox">
        <span>网站第三方统计代码：</span><br />
        <asp:TextBox runat="server" ID="txtEmbedCode" TextMode="MultiLine" Rows="3" Columns="60"
            CssClass="text"></asp:TextBox>
        <span class="muted">页面底部可用显示第三方统计帮助您查看网站的相关访问统计信息 如：http://xxx.xxx.xxx的连接</span>
    </div>
    <div class="O_formBox">
        <span>是否关闭站点：</span><br />
        <asp:RadioButton ID="radEnabled" Text="启用" GroupName="enable" runat="server" />
        <asp:RadioButton ID="radDisEnabled" Text="关闭" GroupName="enable" runat="server" />
    </div>
    <div class="O_formBox">
        <span>是否允许用户自行注册：</span><br />
        <asp:RadioButton ID="radAllowSelfRegex" GroupName="regex" Text="允许" runat="server" />
        <asp:RadioButton ID="radDisSelfRegex" GroupName="regex"  Text="禁止" runat="server" />
    </div>
    <asp:Button runat="server" ID="btnSave" Text="提交" CssClass="btn class1" OnClick="btnSave_Click" />
    <input type="button" id="btnCancel" value="取消" class="btn class2" />
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function ShowoldImg() {
        var url = $("#OldImgUrl").html();
        $("#oldimg").attr("src", url);
        $("#oldimg").css("display", "");
    };
    function HideoldImg() {
        $("#oldimg").css("display", "none");
    };
    $(function () {
        $("#btnCancel").click(function () {
            window.location.href = 'DistributionOemAuthorizationList.aspx?Search=Back';
        });
    });
</script>
