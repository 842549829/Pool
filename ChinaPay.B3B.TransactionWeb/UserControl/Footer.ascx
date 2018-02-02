<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.Footer" %>
<div id="ft" style="width:98%">
    <div class="links" id="links" runat="server">
        <a href="/About/gsjj.aspx" class="firstChild">公司简介</a>
        <a href="/About/qywh.aspx">企业文化</a>
        <a href="/About/aboutB3B.aspx">关于B3B</a>
        <a href="/About/aboutPoolPay.aspx">关于国付通</a>
        <a href="/About/cpyc.aspx">诚征英才</a>
        <a href="/About/jrwm.aspx">招商频道</a>
        <a href="/About/lxwm.aspx">联系我们</a>
        <a href="/About/help.aspx">帮助中心</a>
    </div>
    <div class="copyright" runat="server" id="copyright">
        <span>Copyright &copy; 2012-2020 </span>
        <span>昆明国付通</span>
        <span>www.b3b.so</span>
        <span>All Right Reserved. 滇ICP备12002338号-1号</span>
        <script src="http://s95.cnzz.com/stat.php?id=4753584&web_id=4753584&show=pic"  language="JavaScript"></script>
    </div>
    <div class="linksTypePic" runat="server" id="linksTypePic">
        <ul>
            <li>
                <a href="https://ss.knet.cn/verifyseal.dll?sn=e12071653010031867308004" class="ftLink1"  target="_blank">可信网站</a>
            </li>
            <li>
                <a href="/About/cnnic-date.html" class="ftLink2"  target="_blank">中国互联网络信息中心</a>
            </li>
            <li>
                <a href="http://www.miibeian.gov.cn/" class="ftLink3"  target="_blank">滇ICP备</a>
            </li>
            <li>
                <a href="http://www.yn.cyberpolice.cn:81/RecValidate/RecView.aspx?RecordID=53011103402326" class="ftLink4"  target="_blank">云南网警</a>
            </li>
            <li>
                <a href="http://www.yn.cyberpolice.cn/" class="ftLink5" target="_blank">报警平台</a>
            </li>
        </ul>
    </div>
    <asp:HiddenField runat="server" ID="hidValue" />
</div>


<div style="display: none; position: absolute; z-index: 9999; top: 148px; left: 265.5px;" id="feedback">
        <a onclick="HideMark();closeDiv();" class="close1" href="javascript:void(0)">关闭</a>
        <div class="step_one">
            <h4>请选择问题分类：</h4>
            <ul class="nav clearfix">
                <li><a class="selected" href="javascript:Sugges(1)">使用不方便</a></li>
                <li><a href="javascript:Sugges(2)">政策不满意</a></li>
                <li><a href="javascript:Sugges(4)">页面有错误</a></li>
                <li><a href="javascript:Sugges(8)">数据不准确</a></li>
                <li><a href="javascript:Sugges(16)">界面不美观</a></li>
                <li><a href="javascript:Sugges(32)">其他建议</a></li>
            </ul>
            <textarea id="suggestContent"></textarea>
        </div>
        <div class="step_two">
            <h4>请留下联系方式，您将有机会获得精美礼品：</h4>
            <input type="text" onclick="if(this.value!='')this.value='';" onfocus="this.select();" onblur="if(this.value=='')this.value='QQ/邮箱/电话';" onmouseover="this.focus();" value="QQ/邮箱/电话" class="text" id="contractInfomation">
            <span>（可选）</span>
        </div>
        <div class="btns">
            <input type="button" class="btn" value="提&nbsp;&nbsp;&nbsp;交" id="SaveSuggest">
        </div>
    </div>
    
    <div id="help_trigger">
        <div  class="fl">
            <a href="javascript:void(0)" id="help_trigger_a" title="在线客服">在线客服</a>
            <a href="javascript:void(0)" id="return_top" style="display:none;" title="返回顶部">返回顶部</a>
        </div>
        <div id="help_trigger_box" class="fr hidden">
            <a href="javascript:void(0)" class="close1">关闭</a>
            <p>出票催单：<asp:Literal runat="server" ID="lblOutTicketPhone"></asp:Literal></p>
            <p>变更服务：<asp:Literal runat="server" ID="lblChangeServicePhone"></asp:Literal></p>
            <p>退票服务：<asp:Literal runat="server" ID="lblRefundPhone"></asp:Literal></p>
            <p>紧急业务：<asp:Literal runat="server" ID="lblEmergencyPhone"></asp:Literal></p>
            <p>投诉监督：<asp:Literal runat="server" ID="lblComplainPhone"></asp:Literal></p>
            <p>支付帮助：<asp:Literal runat="server" ID="lblPayServicePhone"></asp:Literal></p>
            <a href="javascript:void(0)" id="send_msg" onclick="ShowMark();showDiv();">建议反馈</a>
            <a href="http://wpa.qq.com/msgrd?V=1&amp;Uin=<%=EnterpriseQQ %>&amp;Site=-&amp;Menu=no" id="phone_service" target="_blank">在线客服</a>
        </div>
    </div>
    
<script src="/Scripts/UserControl/Footer.ascx.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#ft").css("backgroundColor", "#" + $("#<%=hidValue.ClientID %>").val().replace("#", ""));
    });
</script>