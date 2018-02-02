<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThreeCharCode.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.About.ThreeCharCode" %>

<%@ Register Src="~/FlightReserveModule/FlightQueryNew.ascx" TagPrefix="uc" TagName="FlightQuery" %>
<%@ Register Src="~/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 三字码查询</title>
    <meta name="keywords" content="三字码,机场三字码,三字码查询,三字代码,三字码表,机场代码,机场代码查询,机票三字码" />
    <link href="../Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../Styles/icon/fontello.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .form .name
        {
            width: auto;
            margin: 0;
            text-align: inherit;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader"></uc:Header>
        <div id="bd">
            <div class="flow">
                <h3 class="titleBg">
                    三字码查询</h3>
                <div class="queryBox">
                    <p>
                        关键字:
                        <asp:TextBox ID="txtKeyWord" CssClass="queryText" runat="server">
                        </asp:TextBox>
                        <asp:Button ID="btnQuery" runat="server" Text="查 询" CssClass="queryBtn" OnClick="btnQuery_Click" />
                        <b>
                            <asp:Label ID="lblWarnInfo" runat="server"></asp:Label></b>
                    </p>
                    <p class="queryPrompt">
                        小提示:本系统支持三字码、中文城市名及拼音查询。</p>
                </div>
                <div class="table" id="data-list">
                    <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" OnDataBound="dataSource_DataBound"
                        EnableViewState="False">
                        <columns>
                            <asp:BoundField HeaderText="城市名" DataField="Location" 
                                DataFormatString="&lt;a href=&quot;#{0}&quot;&gt;{0}&lt;/a&gt;" 
                                HtmlEncode="False" HtmlEncodeFormatString="False" />
                            <asp:BoundField HeaderText="机场三字码" DataField="Code" />
                            <asp:BoundField HeaderText="机场名称" DataField="ShortName" />
                            <asp:BoundField HeaderText="英文名称" DataField="EnglishName" />
                            <%--<asp:BoundField DataField="Code" 
                            DataFormatString="&lt;a&gt;选为起始点&lt;/a&gt; &amp;nbsp; &lt;a&gt;选为终止点&lt;/a&gt;" 
                                HeaderText="操作" HtmlEncode="False" HtmlEncodeFormatString="False">
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                  <asp:HyperLink ID="hlkDeparture" runat="server" CssClass="departure" departure='<%#Eval("Departure") %>' departureValue='<%#Eval("Code") %>'>选为起点</asp:HyperLink>
                                  <asp:HyperLink ID="hlkDestination" runat="server" CssClass="destination" destination='<%#Eval("Destination") %>' destinationValue='<%#Eval("Code") %>'>选为终点</asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </columns>
                    </asp:GridView>
                    <div class="box" runat="server" visible="false" id="emptyDataInfo">
                        <center>
                            抱歉！没有你所需要的信息！</center>
                    </div>
                </div>
                <div class="tList" id="threeCodeList" runat="server">
                    <h4>
                        常用城市三字码列表</h4>
                    <ul>
                        <li><span class="tTitle">城市名称</span> <span class="tTitle">三字代码</span>
                        </li>
                        <li><span class="tTitle">城市名称</span> <span class="tTitle">三字代码</span>
                        </li>
                        <li><span class="tTitle">城市名称</span> <span class="tTitle">三字代码</span>
                        </li>
                        <li class="lastChild"><span class="tTitle">城市名称</span> <span class="tTitle">
                            三字代码</span> </li>
                        <li><span class="tContent">阿勒泰市</span><span class="tContent tCR">AAT</span></li><li><span class="tContent">兴&nbsp;义&nbsp;市</span><span class="tContent tCR">ACX</span></li><li><span class="tContent">百&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;色</span><span class="tContent tCR">AEB</span></li><li class="lastChild"><span class="tContent">安&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;康</span><span class="tContent tCR">AKA</span></li><li><span class="tContent">阿克苏市</span><span class="tContent tCR">AKU</span></li><li><span class="tContent">鞍&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">AOG</span></li><li><span class="tContent">安&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;庆</span><span class="tContent tCR">AQG</span></li><li class="lastChild"><span class="tContent">安&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;顺</span><span class="tContent tCR">AVA</span></li><li><span class="tContent">安&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">AYN</span></li><li><span class="tContent">包&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;头</span><span class="tContent tCR">BAV</span></li><li><span class="tContent">蚌&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;埠</span><span class="tContent tCR">BFU</span></li><li class="lastChild"><span class="tContent">北&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">BHY</span></li><li><span class="tContent">博&nbsp;乐&nbsp;市</span><span class="tContent tCR">BPL</span></li><li><span class="tContent">昌&nbsp;都&nbsp;县</span><span class="tContent tCR">BPX</span></li><li><span class="tContent">保&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">BSD</span></li><li class="lastChild"><span class="tContent">广&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">CAN</span></li><li><span class="tContent">潮&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">CCC</span></li><li><span class="tContent">常&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;德</span><span class="tContent tCR">CGD</span></li><li><span class="tContent">郑&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">CGO</span></li><li class="lastChild"><span class="tContent">长&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;春</span><span class="tContent tCR">CGQ</span></li><li><span class="tContent">朝&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">CHG</span></li><li><span class="tContent">酒&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;泉</span><span class="tContent tCR">CHW</span></li><li><span class="tContent">赤&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;峰</span><span class="tContent tCR">CIF</span></li><li class="lastChild"><span class="tContent">长&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;治</span><span class="tContent tCR">CIH</span></li><li><span class="tContent">重&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;庆</span><span class="tContent tCR">CKG</span></li><li><span class="tContent">长&nbsp;海&nbsp;县</span><span class="tContent tCR">CNI</span></li><li><span class="tContent">长&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;沙</span><span class="tContent tCR">CSX</span></li><li class="lastChild"><span class="tContent">成&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;都</span><span class="tContent tCR">CTU</span></li><li><span class="tContent">常&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">CZX</span></li><li><span class="tContent">大&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;同</span><span class="tContent tCR">DAT</span></li><li><span class="tContent">达&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">DAX</span></li><li class="lastChild"><span class="tContent">丹&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;东</span><span class="tContent tCR">DDG</span></li><li><span class="tContent">香格里拉县</span><span class="tContent tCR">DIG</span></li><li><span class="tContent">大&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;连</span><span class="tContent tCR">DLC</span></li><li><span class="tContent">大&nbsp;理&nbsp;市</span><span class="tContent tCR">DLU</span></li><li class="lastChild"><span class="tContent">敦&nbsp;煌&nbsp;市</span><span class="tContent tCR">DNH</span></li><li><span class="tContent">东&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;营</span><span class="tContent tCR">DOY</span></li><li><span class="tContent">大&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;庆</span><span class="tContent tCR">DQA</span></li><li><span class="tContent">鄂尔多斯</span><span class="tContent tCR">DSN</span></li><li class="lastChild"><span class="tContent">张&nbsp;家&nbsp;界</span><span class="tContent tCR">DYG</span></li><li><span class="tContent">恩&nbsp;施&nbsp;市</span><span class="tContent tCR">ENH</span></li><li><span class="tContent">延&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;安</span><span class="tContent tCR">ENY</span></li><li><span class="tContent">二连浩特市</span><span class="tContent tCR">ERL</span></li><li class="lastChild"><span class="tContent">福&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">FOC</span></li><li><span class="tContent">阜&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">FUG</span></li><li><span class="tContent">佛&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">FUO</span></li><li><span class="tContent">富&nbsp;蕴&nbsp;县</span><span class="tContent tCR">FYN</span></li><li class="lastChild"><span class="tContent">德&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">GHN</span></li><li><span class="tContent">格尔木市</span><span class="tContent tCR">GOQ</span></li><li><span class="tContent">广&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;元</span><span class="tContent tCR">GYS</span></li><li><span class="tContent">固&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;原</span><span class="tContent tCR">GYU</span></li><li class="lastChild"><span class="tContent">海&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;口</span><span class="tContent tCR">HAK</span></li><li><span class="tContent">邯&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;郸</span><span class="tContent tCR">HDG</span></li><li><span class="tContent">黑&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;河</span><span class="tContent tCR">HEK</span></li><li><span class="tContent">呼和浩特</span><span class="tContent tCR">HET</span></li><li class="lastChild"><span class="tContent">合&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;肥</span><span class="tContent tCR">HFE</span></li><li><span class="tContent">杭&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">HGH</span></li><li><span class="tContent">淮&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;安</span><span class="tContent tCR">HIA</span></li><li><span class="tContent">怀&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;化</span><span class="tContent tCR">HJJ</span></li><li class="lastChild"><span class="tContent">海拉尔区</span><span class="tContent tCR">HLD</span></li><li><span class="tContent">乌兰浩特市</span><span class="tContent tCR">HLH</span></li><li><span class="tContent">哈&nbsp;密&nbsp;市</span><span class="tContent tCR">HMI</span></li><li><span class="tContent">衡&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">HNY</span></li><li class="lastChild"><span class="tContent">哈&nbsp;尔&nbsp;滨</span><span class="tContent tCR">HRB</span></li><li><span class="tContent">舟&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">HSN</span></li><li><span class="tContent">和&nbsp;田&nbsp;市</span><span class="tContent tCR">HTN</span></li><li><span class="tContent">台&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">HYN</span></li><li class="lastChild"><span class="tContent">汉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;中</span><span class="tContent tCR">HZG</span></li><li><span class="tContent">黎&nbsp;平&nbsp;县</span><span class="tContent tCR">HZH</span></li><li><span class="tContent">银&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;川</span><span class="tContent tCR">INC</span></li><li><span class="tContent">且&nbsp;末&nbsp;县</span><span class="tContent tCR">IQM</span></li><li class="lastChild"><span class="tContent">庆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">IQN</span></li><li><span class="tContent">景&nbsp;德&nbsp;镇</span><span class="tContent tCR">JDZ</span></li><li><span class="tContent">嘉&nbsp;峪&nbsp;关</span><span class="tContent tCR">JGN</span></li><li><span class="tContent">吉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;安</span><span class="tContent tCR">JGS</span></li><li class="lastChild"><span class="tContent">景&nbsp;洪&nbsp;市</span><span class="tContent tCR">JHG</span></li><li><span class="tContent">金&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;昌</span><span class="tContent tCR">JIC</span></li><li><span class="tContent">吉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;林</span><span class="tContent tCR">JIL</span></li><li><span class="tContent">黔&nbsp;江&nbsp;区</span><span class="tContent tCR">JIQ</span></li><li class="lastChild"><span class="tContent">九&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;江</span><span class="tContent tCR">JIU</span></li><li><span class="tContent">泉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">JJN</span></li><li><span class="tContent">佳&nbsp;木&nbsp;斯</span><span class="tContent tCR">JMU</span></li><li><span class="tContent">济&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;宁</span><span class="tContent tCR">JNG</span></li><li class="lastChild"><span class="tContent">锦&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">JNZ</span></li><li><span class="tContent">衢&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">JUZ</span></li><li><span class="tContent">鸡&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;西</span><span class="tContent tCR">JXA</span></li><li><span class="tContent">九寨沟县</span><span class="tContent tCR">JZH</span></li><li class="lastChild"><span class="tContent">库&nbsp;车&nbsp;县</span><span class="tContent tCR">KCA</span></li><li><span class="tContent">康&nbsp;定&nbsp;县</span><span class="tContent tCR">KGT</span></li><li><span class="tContent">喀&nbsp;什&nbsp;市</span><span class="tContent tCR">KHG</span></li><li><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;昌</span><span class="tContent tCR">KHN</span></li><li class="lastChild"><span class="tContent">布尔津县</span><span class="tContent tCR">KJI</span></li><li><span class="tContent">昆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;明</span><span class="tContent tCR">KMG</span></li><li><span class="tContent">赣&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">KOW</span></li><li><span class="tContent">库尔勒市</span><span class="tContent tCR">KRL</span></li><li class="lastChild"><span class="tContent">克拉玛依</span><span class="tContent tCR">KRY</span></li><li><span class="tContent">贵&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">KWE</span></li><li><span class="tContent">桂&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;林</span><span class="tContent tCR">KWL</span></li><li><span class="tContent">龙&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;岩</span><span class="tContent tCR">LCX</span></li><li class="lastChild"><span class="tContent">伊&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;春</span><span class="tContent tCR">LDS</span></li><li><span class="tContent">兰&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">LHW</span></li><li><span class="tContent">梁&nbsp;平&nbsp;县</span><span class="tContent tCR">LIA</span></li><li><span class="tContent">丽&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;江</span><span class="tContent tCR">LJG</span></li><li class="lastChild"><span class="tContent">荔&nbsp;波&nbsp;县</span><span class="tContent tCR">LLB</span></li><li><span class="tContent">永&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">LLF</span></li><li><span class="tContent">临&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;沧</span><span class="tContent tCR">LNJ</span></li><li><span class="tContent">德&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;宏</span><span class="tContent tCR">LUM</span></li><li class="lastChild"><span class="tContent">拉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;萨</span><span class="tContent tCR">LXA</span></li><li><span class="tContent">洛&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">LYA</span></li><li><span class="tContent">连&nbsp;云&nbsp;港</span><span class="tContent tCR">LYG</span></li><li><span class="tContent">临&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;沂</span><span class="tContent tCR">LYI</span></li><li class="lastChild"><span class="tContent">柳&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">LZH</span></li><li><span class="tContent">泸&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">LZO</span></li><li><span class="tContent">林&nbsp;芝&nbsp;县</span><span class="tContent tCR">LZY</span></li><li><span class="tContent">牡&nbsp;丹&nbsp;江</span><span class="tContent tCR">MDG</span></li><li class="lastChild"><span class="tContent">绵&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">MIG</span></li><li><span class="tContent">梅&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">MXZ</span></li><li><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;充</span><span class="tContent tCR">NAO</span></li><li><span class="tContent">北&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;京</span><span class="tContent tCR">NAY</span></li><li class="lastChild"><span class="tContent">白&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">NBS</span></li><li><span class="tContent">齐齐哈尔</span><span class="tContent tCR">NDG</span></li><li><span class="tContent">宁&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;波</span><span class="tContent tCR">NGB</span></li><li><span class="tContent">阿&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;里</span><span class="tContent tCR">NGQ</span></li><li class="lastChild"><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;京</span><span class="tContent tCR">NKG</span></li><li><span class="tContent">新&nbsp;源&nbsp;县</span><span class="tContent tCR">NLT</span></li><li><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;宁</span><span class="tContent tCR">NNG</span></li><li><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">NNY</span></li><li class="lastChild"><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;通</span><span class="tContent tCR">NTG</span></li><li><span class="tContent">满洲里市</span><span class="tContent tCR">NZH</span></li><li><span class="tContent">漠&nbsp;河&nbsp;县</span><span class="tContent tCR">OHE</span></li><li><span class="tContent">北&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;京</span><span class="tContent tCR">PEK</span></li><li class="lastChild"><span class="tContent">上&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">PVG</span></li><li><span class="tContent">攀&nbsp;枝&nbsp;花</span><span class="tContent tCR">PZI</span></li><li><span class="tContent">日喀则市</span><span class="tContent tCR">RKZ</span></li><li><span class="tContent">巴彦淖尔</span><span class="tContent tCR">RLK</span></li><li class="lastChild"><span class="tContent">上&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">SHA</span></li><li><span class="tContent">沈&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;阳</span><span class="tContent tCR">SHE</span></li><li><span class="tContent">秦&nbsp;皇&nbsp;岛</span><span class="tContent tCR">SHP</span></li><li><span class="tContent">石&nbsp;家&nbsp;庄</span><span class="tContent tCR">SJW</span></li><li class="lastChild"><span class="tContent">汕&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;头</span><span class="tContent tCR">SWA</span></li><li><span class="tContent">思&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;茅</span><span class="tContent tCR">SYM</span></li><li><span class="tContent">三&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;亚</span><span class="tContent tCR">SYX</span></li><li><span class="tContent">苏&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">SZV</span></li><li class="lastChild"><span class="tContent">深&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;圳</span><span class="tContent tCR">SZX</span></li><li><span class="tContent">青&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;岛</span><span class="tContent tCR">TAO</span></li><li><span class="tContent">塔&nbsp;城&nbsp;市</span><span class="tContent tCR">TCG</span></li><li><span class="tContent">腾&nbsp;冲&nbsp;县</span><span class="tContent tCR">TCZ</span></li><li class="lastChild"><span class="tContent">铜&nbsp;仁&nbsp;市</span><span class="tContent tCR">TEN</span></li><li><span class="tContent">通&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;辽</span><span class="tContent tCR">TGO</span></li><li><span class="tContent">天&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;水</span><span class="tContent tCR">THQ</span></li><li><span class="tContent">济&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;南</span><span class="tContent tCR">TNA</span></li><li class="lastChild"><span class="tContent">通&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;化</span><span class="tContent tCR">TNH</span></li><li><span class="tContent">天&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;津</span><span class="tContent tCR">TSN</span></li><li><span class="tContent">唐&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">TVS</span></li><li><span class="tContent">黄&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;山</span><span class="tContent tCR">TXN</span></li><li class="lastChild"><span class="tContent">太&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;原</span><span class="tContent tCR">TYN</span></li><li><span class="tContent">乌鲁木齐</span><span class="tContent tCR">URC</span></li><li><span class="tContent">榆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;林</span><span class="tContent tCR">UYN</span></li><li><span class="tContent">潍&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;坊</span><span class="tContent tCR">WEF</span></li><li class="lastChild"><span class="tContent">威&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">WEH</span></li><li><span class="tContent">芜&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;湖</span><span class="tContent tCR">WHU</span></li><li><span class="tContent">文&nbsp;山&nbsp;县</span><span class="tContent tCR">WNH</span></li><li><span class="tContent">温&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">WNZ</span></li><li class="lastChild"><span class="tContent">乌&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">WUA</span></li><li><span class="tContent">武&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;汉</span><span class="tContent tCR">WUH</span></li><li><span class="tContent">南&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;平</span><span class="tContent tCR">WUS</span></li><li><span class="tContent">无&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;锡</span><span class="tContent tCR">WUX</span></li><li class="lastChild"><span class="tContent">梧&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">WUZ</span></li><li><span class="tContent">万&nbsp;州&nbsp;区</span><span class="tContent tCR">WXN</span></li><li><span class="tContent">葫&nbsp;芦&nbsp;岛</span><span class="tContent tCR">XEN</span></li><li><span class="tContent">襄&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;樊</span><span class="tContent tCR">XFN</span></li><li class="lastChild"><span class="tContent">西&nbsp;昌&nbsp;市</span><span class="tContent tCR">XIC</span></li><li><span class="tContent">锡林浩特市</span><span class="tContent tCR">XIL</span></li><li><span class="tContent">兴&nbsp;宁&nbsp;市</span><span class="tContent tCR">XIN</span></li><li><span class="tContent">西&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;安</span><span class="tContent tCR">XIY</span></li><li class="lastChild"><span class="tContent">厦&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;门</span><span class="tContent tCR">XMN</span></li><li><span class="tContent">西&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;宁</span><span class="tContent tCR">XNN</span></li><li><span class="tContent">邢&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;台</span><span class="tContent tCR">XNT</span></li><li><span class="tContent">徐&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">XUZ</span></li><li class="lastChild"><span class="tContent">宜&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;宾</span><span class="tContent tCR">YBP</span></li><li><span class="tContent">运&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;城</span><span class="tContent tCR">YCU</span></li><li><span class="tContent">阿尔山市</span><span class="tContent tCR">YIE</span></li><li><span class="tContent">宜&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;昌</span><span class="tContent tCR">YIH</span></li><li class="lastChild"><span class="tContent">伊&nbsp;宁&nbsp;市</span><span class="tContent tCR">YIN</span></li><li><span class="tContent">义&nbsp;乌&nbsp;市</span><span class="tContent tCR">YIW</span></li><li><span class="tContent">延&nbsp;吉&nbsp;市</span><span class="tContent tCR">YNJ</span></li><li><span class="tContent">烟&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;台</span><span class="tContent tCR">YNT</span></li><li class="lastChild"><span class="tContent">盐&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;城</span><span class="tContent tCR">YNZ</span></li><li><span class="tContent">扬&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;州</span><span class="tContent tCR">YTY</span></li><li><span class="tContent">玉&nbsp;树&nbsp;县</span><span class="tContent tCR">YUS</span></li><li><span class="tContent">张&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;掖</span><span class="tContent tCR">YZY</span></li><li class="lastChild"><span class="tContent">昭&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;通</span><span class="tContent tCR">ZAT</span></li><li><span class="tContent">湛&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;江</span><span class="tContent tCR">ZHA</span></li><li><span class="tContent">中&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;卫</span><span class="tContent tCR">ZHY</span></li><li><span class="tContent">珠&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;海</span><span class="tContent tCR">ZUH</span></li><li class="lastChild"><span class="tContent">遵&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;义</span><span class="tContent tCR">ZYI</span></li>
                    </ul>
                </div>
            </div>
            <div>
                <a id="divOpcial" style="display: none;" data="{type:'pop',id:'div_FlightQuery'}">
                </a>
                <div id="div_FlightQuery" class="form layer" style="display: none; width: 750px">
                    <uc:FlightQuery ID="ucFlightQuery" runat="server" />
                    <input type="button" value="取&nbsp;&nbsp;&nbsp;&nbsp;消" class="btn class2 close"
                        style="float: right; margin-right: 85px;" />
                </div>
            </div>
        </div>
        <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </div>
    <asp:HiddenField ID="hfdIsLogined" runat="server" />
    </form>
</body>
</html>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/setting.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $("#inquireHistory,#recentlyHistory").remove();
        $("#hotCity").css("z-index", "9999").appendTo($("body"));
        $("#btnQuery").click(function ()
        {
            var keyWord = $.trim($("#txtKeyWord").val());
            if (keyWord == "")
            {
                alert("请输入关键字查询!");
                return false;
            }
        });
        $(".departure").click(function ()
        {
            var departure = $(this).attr("departure");
            var departureValue = $(this).attr("departureValue");
            if ($("#hfdIsLogined").val() == "true")
            {
                $("#txtDepartureValue").val(departureValue);
                $("#txtDeparture").val(departure);
                if ($("#txtDepartureValue").val() == $("#txtArrivalValue").val())
                {
                    $("#txtArrivalValue").val("");
                    $("#txtArrival").val("");
                }
            }
            $("#divOpcial").click();
        });
        $(".destination").click(function ()
        {
            var destination = $(this).attr("destination");
            var destinationValue = $(this).attr("destinationValue");
            if ($("#hfdIsLogined").val() == "true")
            {
                $("#txtArrivalValue").val(destinationValue);
                $("#txtArrival").val(destination);
                if ($("#txtDepartureValue").val() == $("#txtArrivalValue").val())
                {
                    $("#txtDepartureValue").val("");
                    $("#txtDeparture").val("");
                }
            }
            $("#divOpcial").click();
        });
    })
</script>
