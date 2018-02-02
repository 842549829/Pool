<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketDefault.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.TicketDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
    <link href="/Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style>
        .culture_box
        {
            background-color: #f5f5f5;
            border: 10px solid #e2e2e0;
            margin: 10px 0;
            padding: 10px;
        }
        .culture_box h2
        {
            color: #FB6702;
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 10px;
            text-align: center;
        }
        .culture_box h4
        {
            color: #00327B;
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 10px;
        }
        .culture_box p
        {
            color: #333;
            font-size: 14px;
            line-height: 1.8;
        }
        .culture_left_box
        {
            background-color: #fff;
            border: 10px solid #d8d8d8;
            height: 190px;
            margin: 0 10px 10px;
            padding: 10px;
            width: 240px;
        }
        .culture_right_box
        {
            background-color: #fff;
            border: 10px solid #d8d8d8;
            height: 190px;
            margin: 0 20px 10px 320px;
            padding: 10px;
        }
        .culture_right_box p
        {
            padding-left: 20px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="divHeader" class="tips-box_1">
    </div>
    <a href="#" runat="server" id="newRelease" visible="false">
        <div class="releasetipsMini">
            <div class="releaseconMini">
                <div class="releasenewMini">
                </div>
            </div>
        </div>
    </a>
    <div class="clearfix">
        <div id="middle">
            <div runat="server" id="divRemind" class="remind">
            </div>
            <div runat="server" id="divSuspendedPolicy" class="policy">
            </div>
        </div>
        <div runat="server" id="right">
            <div class="img-list">
                <h3 class="titleBg">
                    系统公告</h3>
                <div runat="server" id="divAnnounce" class="lump">
                </div>
            </div>
        </div>
    </div>
    <div class="culture_box" id="divSwf" runat="server">
        <h2>
            态度决定成败，业绩决定收获</h2>
        <div class="clearfix">
            <div class="fl culture_left_box">
                <h4>
                    企业使命：</h4>
                <p class="txt-ind">
                    让客户体验最好的在线机票服务与交易感受，为供应商提供最简单的操作流程、最创新的盈利模式与安全、稳定的交易平台。</p>
            </div>
            <div class="culture_right_box">
                <h4>
                    企业价值观：</h4>
                <p>
                    客户至上：尊重客户需求，专业客户应用体验，帮助客户盈利。</p>
                <p>
                    团队协作：共担互助，共同进步。</p>
                <p>
                    诚实信义：真诚正直，信守承诺。</p>
                <p>
                    创新务实：敢于创新，行于实际。</p>
                <p>
                    双赢：互利互惠，共同发展，共同得到利益最大化。</p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script src="Scripts/widget/common.js" type="text/javascript"></script>
<script src="Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($.browser.msie && $.browser.version == "7.0") {
            $("#divSwf object").width($("body").width());
        }
    });
</script>