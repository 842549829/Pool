<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WithholdingSet.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.WithholdingSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>自动支付设置</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<link href="/Styles/bank.css" rel="stylesheet" type="text/css" />
<link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="/Styles/public.css?20121118" />
<body>
    <form id="form1" runat="server">
    <div class="form">
        <h3 class="titleBg">
            自动支付设置</h3>
        <div class="gray-box">
            <p>
                自动支付就是当您在<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>发生机票交易时，在您所设定的支付金额范围内无需输入密码即可委托国付通账务系统自动扣除相应的机票款；您可以设置由您的现金、预存、信用、POS四类账户中的任意一个账号（若存在该类型账号）进行自动支付、通过自动支付可用省去繁琐的账号和支付密码输入过程，国付通官方推荐您进行小额的机票款支付时使用该设置。</p>
        </div>
        <div class="SMS-nav-box">
            <p>
                选择您用于自动支付的方式和账号：</p>
            <ul class="SMS-nav clearfix" id="options">
                <li class="curr" id="poolpay">国付通账户</li>
                <li id="alipay">支付宝账户</li>
            </ul>
        </div>
        <div class="column form poolpay" runat="server" id="divPoolpaySignedInfo" visible="false">
            <table>
                <colgroup>
                    <col class="w20" />
                    <col class="w30"/>
                    <col class="w20" />
                    <col class="w30" />
                </colgroup>
                <tr>
                    <td class="title">
                        账号：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPoolpayAccountNo"></asp:Label>
                    </td>
                    <td class="title">
                        最大金额：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAmount"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        签约时间：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTime"></asp:Label>
                    </td>
                    <td class="title">
                        签约状态 ：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblStatus"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="btns"><input type="button" id="btnCancel" value="关闭代扣" class="btn class2" /></td>
                </tr>
            </table>
        </div>
        <table class="noTopBorder poolpay"  runat="server" id="tabledivSignedOperating">
            <tr>
                <td class="title">
                    自动支付最大金额：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAmount" CssClass="text text-s"></asp:TextBox>元
                    <span class="obvious1 pad-l">只有在此金额内的订单才会允许自动支付，超过金额的订单需要您手动进行支付</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    请输入国付通账号：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPoolpayAccountNo" CssClass="text"></asp:TextBox>
                    <span class="obvious1 pad-l">您可用使用现金、信用、预存、POS账户中的任意一种账号</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    请输入支付密码：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPayPassword" CssClass="text" TextMode="Password"></asp:TextBox>
                    <span class="obvious1 pad-l">请填写您所输入的国付通账号的支付密码</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnPoolpayNo" Text="同意以下协议并提交" CssClass="btn class1"
                        OnClick="btnPoolpayNo_Click" />&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <table class="noTopBorder alipay">
            <tr>
                <td class="title">
                    请输入支付宝账号：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAliPayNo" CssClass="text"></asp:TextBox>
                    <asp:Button runat="server" ID="btnGetWithholdingStatus" Text="获取代扣状态" CssClass="btn class3"
                        OnClick="btnGetWithholdingStatus_Click" />
                    <span class="obvious1 pad-l">为保证自动支付顺利完成，请定期检查您的支付宝内代付金额是否充足</span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnAliPaySubmit" Text="提交" CssClass="btn class1" OnClick="btnAliPaySubmit_Click" />
                </td>
            </tr>
        </table>
        <div runat="server" id="divAgreement" class="treaty-box poolpay" style="width: 95%;">
            <h4>
                国付通自动支付机票款服务协议</h4>
            <p>
                国付通自动代扣许可协议（你有权停止代扣，同意代扣协议即表明接受该协议所有条款）</p>
            <br />
            <p>
                不得利用本站危害国家安全、泄露国家秘密、不得侵犯国家社会集体的公民的合法权益、不得利用本站制作、复制和传播下列信息：</p>
            <p class="treaty-list">
                （1）煽动抗拒、破环宪法和法律、行政法规实施的；</p>
            <p class="treaty-list">
                （2）煽动颠覆国家政权，推翻社会主义制度的；</p>
            <p class="treaty-list">
                （3）煽动分裂国家、破环国家统一的；</p>
            <p class="treaty-list">
                （4）煽动民族仇恨、名族歧视、破环民族团结的；</p>
            <p class="treaty-list">
                （5）捏造或者歪曲事实、散步谣言、扰乱社会秩序的；</p>
            <p class="treaty-list">
                （6）宣扬封建迷信、淫秽、色情、赌博、暴力、凶杀、恐怖、教唆犯罪的</p>
            <p class="treaty-list">
                （7）公然侮辱他人或者捏造事实诽谤他人的，或者进行其他恶意攻击的；</p>
        </div>
        <a id="layer" class="hidden" data="{type:'pop',id:'divCancel'}"></a>
        <div style="width: 500px;" class="layer3 hidden" id="divCancel">
            <h4>
                关闭代扣<a href="javascript:;" class="close">关闭</a></h4>
            <div class="con">
                <p class="tips mar">该操作将关闭您的自动支付功能，请输入支付密码以确认操作</p>
                <br />
                <span>代扣账号：</span>
                <asp:Label runat="server" ID="lblCanclePoolpayAccountNo"></asp:Label>
                <br /><br />
                <span>支付密码：</span>
                <asp:TextBox runat="server" ID="txtCancelPayPassword" CssClass="text" TextMode="Password"></asp:TextBox>
            </div>
            <div class="btns">
                <asp:Button runat="server" ID="btnConfirm" Text="确认" CssClass="btn class1" OnClick="btnConfirm_Click" />
                <input type="button" class="btn class2 close" value="取消" />
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfdIsProtocol" Value="true" />
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/TerraceModule/CompanyOption.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnCancel").click(function () {
            $("#layer").click();
        });
        setOptions($("#options li"));
        $("#btnAliPaySubmit").click(function () {
            if ($.trim($("#txtAliPayNo").val()).length <= 0) {
                alert("支付宝账号不能为空");
                return false;
            } else {
                if ($("#hfdIsProtocol").val() == "true") {
                    document.getElementById('form1').target = '_blank';
                } else {
                    document.getElementById('form1').target = '_self';
                }
            }
        });
        $("#btnPoolpayNo").click(function () {
            document.getElementById("form1").target = "_self";
        });
    });
</script>
