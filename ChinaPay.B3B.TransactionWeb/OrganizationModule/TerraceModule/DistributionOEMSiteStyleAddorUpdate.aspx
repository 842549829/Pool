<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteStyleAddorUpdate.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOEMSiteStyleAddorUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>风格管理</title>
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js" ></script>
</head>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        风格管理</h3>
    <div class="O_formBox">
        <span>风格方案名称：</span><br />
        <input type="text" id="txtKeyWord" runat="server" class="text" />
        <span class="muted">输入2-6个汉字做为该风格的名称</span>
        <asp:HiddenField runat="server" id="hdStyleId"  />
    </div>
    <div class="O_formBox">
        <span>模板路径：</span><br />
        <input type="text" id="txtTemplatePath" runat="server" class="text" />
        <span class="muted">请填写根目录内skin模版路径文件包名称，以 / 结尾</span>
    </div>
    <div class="O_formBox">
        <span>样式路径：</span><br />
        <div id="files"><asp:Repeater runat="server" id="rpStyleFilePath">
            <ItemTemplate>
                <input type="text" id="txtStyleFilePath" runat="server" class="text" value='<%#GetDataItem() %>' />
            </ItemTemplate>
        </asp:Repeater>  </div>
        <span class="muted">请填写需要加入页面样式表的路径</span>
    </div>
    <div class="O_formBox">
        <span>模板缩略图：</span><br />
        <input type="text" id="txtThumbnailPicture" runat="server" class="text" />
        <span class="muted">请填写模板首页预览图路径 </span>
    </div>
    <div class="O_formBox">
        <span>添加模版描述：</span><br />
        <input type="text" id="txtTemplateDescription" runat="server" class="text" />
        <span class="muted">鼠标移动到缩略图是会显示该文字，请尽量使用简短的文字描述该模版的亮点及特色</span>
    </div>
    <div class="O_formBox">
        <span>模版排序：</span><br />
        <input type="text" id="txtTemplateIndex" runat="server" class="text text-s" />
        <span class="muted">请输入纯数字来决定模版的排序，输入整数，数字越小越靠前</span>
    </div>
    <div class="O_formBox">
        <span>是否开启模版：</span><br />
        <asp:RadioButton ID="radTemplateEnabled" Checked="true" GroupName="regex" Text="开启" runat="server" />
        <asp:RadioButton ID="radTemplateDisabled" GroupName="regex" Text="禁用" runat="server" />
        <span class="muted">当模版出现脚本错误如脚本错误等情况时可禁用该模版，禁用后使用该模版的用户将会被切换到初始化模版</span>
    </div>
    <%--<asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保存" 
        onclick="btnSave_Click" OnClientClick="return ValidateInput()" />--%>
        <input type="button" value="保存" class="btn class1" onclick="SaveData()" />
        <input type="button" value="返回" class="btn class2" onclick="location.href='DistributionOEMSiteStyleManage.aspx'" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script type="text/javascript" src="/Scripts/Global.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $("#txtTemplateIndex").OnlyNumber();
        $("#txtKeyWord").LimitLength(50);
        $("#txtTemplatePath").LimitLength(50);
        $("#txtThumbnailPicture").LimitLength(50);
        $("#txtTemplateDescription").LimitLength(200);
    });
    function ValidateInput(inputData) {
        if (inputData.StyleName=="") {
            alert("请输入风格方案名称");
            $("#txtKeyWord").select();
            return false;
        }
        if (inputData.TemplatePath=="") {
            alert("请输入模板路径");
            $("#txtTemplatePath").select();
            return false;
        }
        if (inputData.ThumbnailPicture == "") {
            alert("请输入模板缩略图");
            $("#txtThumbnailPicture").select();
            return false;
        }
        return true;
    }

    function GetInput() {
        var style = {Id:$("#hdStyleId").val()};
        style.StyleName = $("#txtKeyWord").val();
        style.TemplatePath = $("#txtTemplatePath").val();
        style.ThumbnailPicture = $("#txtThumbnailPicture").val();
        style.Remark = $("#txtTemplateDescription").val();
        style.Sort = parseInt($("#txtTemplateIndex").val());
        style.Enable = $("#radTemplateEnabled").is(":checked");
        style.StylePath = $.map($("#files input"), function (item)
        {
            return $(item).val();
        });
        return style;
    }

    function SaveData(style)
    {
        style = style || GetInput();
        if (!ValidateInput(style)) return;
        sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/SaveOEMStyle", JSON.stringify({ style: style }), function (msg)
        {
            alert("保存成功！");
        }, function (errMessage)
        {
            alert(errMessage);
        });
    }
</script>
    

