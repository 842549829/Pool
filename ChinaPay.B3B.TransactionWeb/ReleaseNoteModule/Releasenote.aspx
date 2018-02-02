<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Releasenote.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.ReleaseNoteModule.Releasenote" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>更新日志</title>
</head>
    <link rel="stylesheet" type="text/css" href="/Styles/releasenote.css"/>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
   <%-- <uc:Header runat="server" ID="ucHeader"></uc:Header>--%>
    <div class="bd">
        <div class="history-date" id="divContext" runat="server">
        </div>
    </div>
   <%-- <uc:Footer runat="server" ID="ucFooter"></uc:Footer>--%>
    </form>
</body>
</html>
