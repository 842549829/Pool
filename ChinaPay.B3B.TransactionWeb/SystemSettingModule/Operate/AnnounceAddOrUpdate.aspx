<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="AnnounceAddOrUpdate.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.AnnounceAddOrUpdate" %>

<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        #ftbContainer
        {
            border: none;
        }
        #ftbContainer td, #ftbContainer table
        {
            margin: 0;
            padding: 0;
            border: none;
            height: auto;
        }
        #ftbContainer *
        {
            vertical-align: middle;
        }
        select
        {
            /*Fix Global select css*/
            height: auto;
            width: auto;
            color: black;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        <asp:Label runat="server" ID="lblAddOrUpdate" Text="新增"></asp:Label>公告</h3>
    <div class="form">
        <table class="box">
            <tr>
                <td class="title">
                    标题
                </td>
                <td>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="text text_width"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公告类型
                </td>
                <td>
                    <asp:RadioButton ID="rbnCommonType" runat="server" Text="普通公告" Checked="true" GroupName="announceType" />
                    <asp:RadioButton ID="rbnImportType" runat="server" Text="重要公告" GroupName="announceType" />
                    <asp:RadioButton ID="rbnErgentType" runat="server" Text="紧急公告" GroupName="announceType" />
                </td>
            </tr>
            <tr id="announceScope" runat="server" visible="false">
                <td class="title">
                    公告范围
                </td>
                <td>
                    <asp:CheckBox ID="chkB3b" runat="server" Text="B3B可见"/>
                    <asp:CheckBox ID="chkOem" runat="server" Text="OEM可见"/>
                </td>
            </tr>
            <tr>
                <td class="title">
                    内容
                </td>
                <td id="ftbContainer">
                    <FTB:FreeTextBox ID="ftbContent" runat="server" Width="800px">
                    </FTB:FreeTextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="btns">
        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" OnClick="btnSave_Click" />
        <input type="button" id="btnBack" value="返回" class="btn class2" onclick="javascript:window.location.href='AnnounceList.aspx'" />
    </div>
    <asp:HiddenField ID="hfdIsPlatform" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {
            var titleObj = $("#txtTitle");
            var title = $.trim(titleObj.val());
            if (title == "") {
                alert("请输入公告标题！");
                titleObj.focus();
                return false;
            } else {
                if (title.length > 50) {
                    alert("标题格式错误！");
                    titleObj.select();
                    return false;
                }
            }
            var contentObj = $("#ftbContent");
            var content = $.trim(contentObj.val());
            if (content == "") {
                alert("请输入公告内容！");
                contentObj.focus();
                return false;
            }

            if ($("#hfdIsPlatform").val() == "True") {
                if ($("#chkB3b").attr("checked") != "checked" && $("#chkOem").attr("checked") != "checked")
                {
                    alert("请选择公告范围！");
                    return false;
                }
            }
        });
    })
</script>
