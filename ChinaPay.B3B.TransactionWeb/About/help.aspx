<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="help.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.About.help" %>

<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台 - 帮助中心</title>
    <link rel="stylesheet" href="/Styles/company.css" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div id="bd">
        <div id="helpNavBox">
            <h4 id="commonProblem">
                常见问题</h4>
            <ul>
                <li><a href="#" class="selected">使用入门</a></li>
                <li><a href="#">机票相关</a></li>
                <li><a href="#">支付安全</a></li>
                <li><a href="#">积分帮助</a></li>
                <li><a href="#">合作事项</a></li>
            </ul>
            <h4 id="productFeedback">
                产品反馈</h4>
            <ul>
                <li><a href="#">联系客服反馈</a></li>
                <li><a href="#">留言反馈</a></li>
            </ul>
        </div>
        <div class="helpList">
            <h4>
                使用入门</h4>
            <ul class="question">
                <li class="b3bwhat"><a href="javascript:;" class="b3bwhat">B3B平台能够做什么？</a></li>
                <li class="b3bjifen"><a href="javascript:;" class="b3bjifen">什么是B3B积分？</a></li>
                <li class="jifenkeyong"><a href="javascript:;" class="jifenkeyong">什么是可用积分？</a></li>
                <li class="jifenhuode"><a href="javascript:;" class="jifenhuode">积分如何获得？</a></li>
                <li class="jifenxiaofei"><a href="javascript:;" class="jifenxiaofei">积分如何消费？</a></li>
                <li class="teshupiao"><a href="javascript:;" class="teshupiao">什么是特殊票？</a></li>
                <asp:Repeater runat="server" ID="SpecialPolicTitleList">
                    <itemtemplate>
                        <li class="<%#Eval("SpecialProductType") %>"><a href="javascript:;" class="<%#Eval("SpecialProductType") %>">什么是<%#Eval("Name") %>？</a></li>
                    </itemtemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="helpResult" id="b3bwhat">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                B3B平台能够做什么？</h4>
            <p>
                B3B平台是国内领先的机票销售平台，主要以航空电子客票平台开发为主体，融合当下国内通行的B2B机票销售模式，创新B3B全新概念，注入更多实际的三元应用因素，服务理念适用于全国每天80%的机票买卖商（自然人）。B3B.SO平台，只为机票二字而生，兼容目前国内所有机票平台的B2B业务，为全国以机票业务有关的80%从业者创造最新盈利模式。</p>
        </div>
        <div class="helpResult" id="b3bjifen">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                什么是B3B积分？</h4>
            <p>
                使用采购账号经常登录平台并采买机票的活跃交易用户，平台给予的回赠奖励方式。</p>
        </div>
        <div class="helpResult" id="jifenkeyong">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                什么是可用积分？</h4>
            <p>
                B3B可用积分是您的采购账号在拥有一定的积分后，按照70%的比例来可以实际使用的积分。</p>
        </div>
        <div class="helpResult" id="jifenhuode">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                积分如何获得？</h4>
            <p>
                1、第一种获取方式是，用您的采购账号每个自然日登录成功后获得的签到奖励积分。</p>
            <p>
                2、第二种是 您的采购账号在B3B购票成功后，B3B网站送给您的购票累计积分。</p>
        </div>
        <div class="helpResult" id="jifenxiaofei">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                积分如何消费？</h4>
            <p>
                积分累积到一定分数后，可以在B3B的积分商城内兑换实物商品，如：免票、机模等而不需再付任何费用。</p>
        </div>
        <div class="helpResult" id="teshupiao">
            <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
            <h4>
                什么是特殊票？</h4>
            <p>
                就像免票等这些产品，通过特殊渠道、特别流程出票，价格低，出票相对较慢，并且具备不确定性。</p>
        </div>
        <asp:Repeater runat="server" ID="SpecialPolicyInfos">
            <itemtemplate>
                 <div class="helpResult" id="<%#Eval("SpecialProductType") %>">
                    <a href="javascript:;" class="goBack">&lt;&lt;返回</a>
                    <h4>
                        什么是<%#Eval("Name") %>？</h4>
                    <p>
                        <%#Eval("Explain")%>    
                    </p>
                </div>

            </itemtemplate>
        </asp:Repeater>
    </div>
    <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        var parameters = getRequest();
        $(".question a").click(function ()
        {
            $(".helpList").hide();
            $("#" + $(this).attr("class")).show();
        });
        $(".goBack").click(function ()
        {
            $(".helpResult").hide();
            $(".helpList").show();
            var url = location.search; // 获取url中"?"符后的字串
            var locationUrl = window.location.href;
            if (url != "")
            {
                window.location.href = locationUrl.substr(0, locationUrl.indexOf(url));
            }
        });
        if (parameters["flag"] == null)
        {
            $(".helpResult").hide();
        } else
        {
            if ($("#" + parameters["flag"]).html() != null)
            {
                $(".helpList").hide();
                $(".helpResult").hide();
                $("#" + parameters["flag"]).show();
            } else
            {
                $(".helpResult").hide();
            }
        }
    });  
</script>
