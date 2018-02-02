<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSender.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSSender" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发送短信</title>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        发送短信</h3>
    <div class="importantBox broaden" id="LastDiv" runat="server">
        <p class="important">
            您的短信剩余条数为：<asp:Label runat="server" ID="lblLastNum"></asp:Label>
            条，建议您点此<a href="SMSBuy.aspx" class="pad-l obvious-a">立即购买</a></p>
    </div>
    <div class="importantBox broaden" id="MoreDiv" runat="server">
        <p class="ok">
            您的短信剩余条数为：<asp:Label runat="server" ID="lblMoreNum"></asp:Label>
            条，请放心使用</p>
    </div>
    <div class="form">
        <table>
            <colgroup>
                <col class="w20" />
                <col />
            </colgroup>
            <tr>
                <td class="title">
                    手机号码
                </td>
                <td>
                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtPhone" CssClass="text" Width="50%"
                        Height="100px"></asp:TextBox>
                    <br />
                    <span class="obvious1">输入手机号码，多个号码请用逗号（半角）隔开。</span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    短信内容
                </td>
                <td>
                    <span class="obvious1">请勿用于发送广告信息，一旦发现将禁用您的短信功能。您已输入<span id="spanSmsNum" class="obvious font-b">0</span>字，将分<span
                        id="spanNum" class="obvious">1</span>条发送。最多可以发送300个字</span>
                    <br />
                    <asp:TextBox runat="server" TextMode="MultiLine" CssClass="text fl" ID="txtContext"
                        Width="70%" Height="100px"></asp:TextBox>
                    <div class="fl mar-l" id="getDefault">
                        <p>
                            插入<a href="javascript:;" id="chupiao">[出票成功]</a></p>
                        <p>
                            插入<a href="javascript:;" id="hangban">[航班变动]</a></p>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="发送" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#txtContext").keyup(function () {
            $("#spanSmsNum").html($.trim($("#txtContext").val()).length);
            if ($("#txtContext").val() != "") {
                $("#spanNum").html(parseInt(Math.ceil(parseFloat($.trim($("#txtContext").val()).length) / 61)));
            }
        });
        $("#btnSave").click(function () {
            var pattern = /^\d[,]{1,2000}$/;
            var phonePattern = /^1[3458]\d{9}$/;
            var phones = $.trim($("#txtPhone").val());
            var phoneContent = new Array();
            phoneContent = phones.split(/[,，]/);
            if (phones.length == 0) {
                alert("请输入手机号码");
                return false;
            }
            if (phones.length > 2000) {
                alert("手机号码长度不能超过2000位");
                return false;
            }
            //            if (!pattern.test(phones)) {
            //                alert("手机号码格式错误");
            //                return false;
            //            }
            if ($.trim($("#txtContext").val())=="") {
                alert("短信内容为空");
                return false;
            }
            if ($.trim($("#txtContext").val()).length > 300) {
                alert("短信内容不能超过300位");
                return false;
            }
            for (var i = 0; i < phoneContent.length; i++) {
                if (!phonePattern.test(phoneContent[i])) {
                    alert("第 [" + (i + 1) + "] 个手机号码格式错误");
                    return false;
                }
            }
        });
        $("#getDefault a").click(function () { 
            var key = "";
            if ($(this).attr("id") == "chupiao") {
                key = "B3BTicketSuccessTemplte";
            } else {
                key = "B3BFlightDelayTemplte";
            }
            sendPostRequest("/SMSHandlers/SMS.ashx/GetDefaultTemlete", JSON.stringify({ "paramerKey": key }), function (e) {
                $("#txtContext").val(e);
                $("#txtContext").keyup();
            }, function (e) {
                if (e.status == 300) {
                    alert(JSON.parse(e.responseText));
                } else {
                    alert(e.statusText);
                }
            });
        });
        $("#txtContext").keyup();
    });
</script>
