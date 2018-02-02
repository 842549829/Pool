<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnounceInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.AnnounceInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    	<title>公告详情页面</title>
    <link href="../../Styles/public.css?20121118" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="width:800px;">
    <div class="flow">
			<div class="article">
				<div>
					<h1><asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
					<div class="date minor" style="float:right;"><asp:Label ID="lblPublishTime" runat="server"></asp:Label></div>
				</div>
				<div class="con" id="lblContent" runat="server">
                   
				</div>
			</div>
		</div>
        <div style="text-align:center">
         <input type="button" class="btn class2" value="返回" onclick='javascript:window.location.href="AnnounceList.aspx"' />
        </div>
    </form>
</body>
</html>
