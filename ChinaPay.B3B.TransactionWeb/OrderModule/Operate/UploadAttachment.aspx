<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadAttachment.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.UploadAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上传附件</title>
</head>
<body>
    <link href="../../Styles/form.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/core.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <span class="name">附&nbsp;&nbsp;&nbsp;件：</span>
        <asp:FileUpload  CssClass="text"  runat="server" ID="fileAttachment"/>
        <p></p>
        <asp:Button runat="server" ID="btnConfirm" Text="确定" CssClass="btn class1" 
            onclick="btnConfirm_Click" />
        <input type="button" value="取消" id="btnCancel" class="btn class2"/>
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnAttachment").click(function () {
            $("#fileAttachment").click();
        });
        $("#fileAttachment").change(function () {
            $("#txtAttachment").val($(this).val());
        });
        $("#btnCancel").click(function () {
            $("#mask,#divUploadAttachmentLayer").hide();
        });
    });
</script>
