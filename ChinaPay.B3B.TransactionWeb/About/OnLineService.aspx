<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnLineService.aspx.cs" EnableViewState="false"
    Inherits="ChinaPay.B3B.TransactionWeb.About.OnLineService" %>

<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 在线客服</title>
    <link href="../Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../Styles/icon/fontello.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/skin.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <style type="text/css">
        ul
        {
            float: left;
            width: 100%;
        }
        ul li
        {
            width: 150px;
            float: left;
            margin: 0px;
            padding: 0px;
        }
        .box
        {
            margin: 5px;
            width: 650px;
        }
        h2, hr
        {
            clear: both;
        }
        #divDivideGroup li
        {
            line-height: 25px;
            height:25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div id="bd">
            <div class="flow">
                <div class="column table">
                    <h3 class="titleBg">
                        客服服务</h3>
                </div>
                <div runat="server" id="OneNew" style="margin-left: 20px; width: 100%;">
                    <h2 style='text-align: center;'>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    </h2>
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblContent"></asp:Label>
                </div>
                <div id="divDivideGroup" runat="server">
                </div>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </div>
    </form>
</body>
</html>
