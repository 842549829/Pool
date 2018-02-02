<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cpyc.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.About.cpyc" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <title>B3B相关软件下载</title>
    <link href="/Styles/company.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
</head>
<body>
	<form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
	<div class="downloadBg">
		<div class="downloadHd downloadPic">
			<div class="downloadTitle downloadPic">
				订单提醒工具
			</div>
			<div class="clearfix">			
				<a href="<%=ToolUrl %>" class="freeDownload downloadPic">
					免费下载
				</a>
				<p class="freeDownloadDesc">
					<span>软件版本：1.1</span>
					<span>更新时间：2012/12/06</span>
					<span>运行环境：windows XP/Vista/Win7</span>
				</p>
			</div>
				<p class="freeDownloadSupply">
					本软件需要.Net2.0环境支持，如无法运行本软件请<a href="http://download.microsoft.com/download/5/6/7/567758a3-759e-473e-bf8f-52154438565a/dotnetfx.exe">点此下载</a>环境安装包
				</p>			
		</div>
		<div class="downloadContent clearfix">
			<div class="downloadLeft">
				<h4 class="downloadPic">功能介绍</h4>
				<p class="downloadPic">
					<span class="downloadUnit downloadPic Unit1">
						<b>航空公司自主备置</b>
						支持全国所有国内航空公司的自由配置
					</span>
					<span class="downloadUnit downloadPic Unit2">
						<b>订单状态自动提醒</b>
						可自由选择支持多种订单状态哦
					</span>
					<span class="downloadUnit downloadPic Unit3">
						<b>多账户自由切换</b>
						可以在各个帐号之间自由切换处理工作哦
					</span>
					<span class="downloadUnit downloadPic Unit4">
						<b>体积小巧，无需安装</b>
						绿色版程序，无需安装即可放心使用
					</span>
				</p>
			</div>
			<div class="downloadRight">
				<h4 class="downloadPic">相关下载</h4>
				<p class="downloadPic">
					<span class="downloadUnit downloadPic Unit5">
						<b>B3B官方推荐使用谷歌浏览器</b>
						<a href="https://dl.google.com/tag/s/appguid={8A69D345-D564-463C-AFF1-A69D9E530F96}&iid={71801D85-FC84-DFCF-C306-0202F861EB18}&lang=zh-CN&browser=4&usagestats=0&appname=Google%20Chrome&needsadmin=true&installdataindex=defaultbrowser/update2/installers/ChromeSetup.exe">点此下载</a>
						以便更好的体验B3B平台
					</span>
					<span class="downloadUnit downloadPic Unit6">
						<b>.Net2.0支持库安装包</b>
						<a href="http://download.microsoft.com/download/5/6/7/567758a3-759e-473e-bf8f-52154438565a/dotnetfx.exe">点此下载</a>
						订单提醒无法运行时使用本安装包
					</span>
					<span class="downloadUnit downloadPic Unit7">
						<b>订单提醒工具iphone客户端</b>
						敬请期待...
					</span>
					<span class="downloadUnit downloadPic Unit8">
						<b>B3B购票平台移动客户端</b>
						敬请期待...
					</span>
					<span class="downloadUnit downloadPic Unit9">
						<b>国付通综合支付客户端</b>
						敬请期待...
					</span>
				</p>
			</div>
		</div>
	</div>
	<uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </form>
</body>
</html>