<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" action="Upload.aspx" enctype="multipart/form-data" method="post">
        <input type="file" id="flAttachment" name="flAttachment" />
        <asp:Button runat="server" ID="btnUploadFile" onclick="btnUploadFile_Click" />
        <asp:HiddenField runat="server" ID="hfdFilePath" ClientIDMode="Static" />
    </form>
</body>
</html>