<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentsAccount.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.PaymentsAccount.PaymentsAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">公司基础信息</h3>
    <div class="form">
        <table>
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">用户名</td>
                <td><asp:Label ID="lblAccountNo" runat="server"></asp:Label></td>
                <td class="title">公司类型</td>
                <td><asp:Label ID="lblCompanyType" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title" runat="server" id="tdUserName">公司名称</td>
                <td><asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                <td class="title" runat="server" id="tdNickName">公司简称</td>
                <td><asp:Label ID="lblPetName" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">所在地</td>
                <td><asp:Label ID="lblLocation" runat="server"></asp:Label></td>
                <td class="title">地址</td>
                <td><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">邮政编码</td>
                <td><asp:Label ID="lblPostCode" runat="server"></asp:Label></td>
                <td class="title">传真</td>
                <td><asp:Label ID="lblFaxes" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">联系人</td>
                <td><asp:Label ID="lblLinkman" runat="server"></asp:Label></td>
                <td class="title">联系人电话</td>
                <td><asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label></td>
            </tr>
            <tbody id="tbArgen" runat="server">
                <tr>
                    <td class="title">负责人</td>
                    <td><asp:Label ID="lblPrincipal" runat="server"></asp:Label></td>
                    <td class="title">负责人电话</td>
                    <td><asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="title">紧急联系人</td>
                    <td><asp:Label ID="lblUrgencyLinkman" runat="server"></asp:Label></td>
                    <td class="title">紧急联系人电话</td>
                    <td><asp:Label ID="lblUrgencyLinkmanPhone" runat="server"></asp:Label></td>
                </tr>
            </tbody>
            <tr>
                <td class="title">Email</td>
                <td><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                <td class="title">QQ</td>
                <td><asp:Label ID="lblQQ" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">MSN</td>
                <td><asp:Label ID="lblMSN" runat="server"></asp:Label></td>
                <td class="title" id="fixedPhoneTitle" runat="server">固定电话</td>
                <td id="fixedPhoneValue" runat="server">
                 <asp:Label ID="lblFixedPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4"><h3 class="titleBg" style="height:auto;">收付款账号</h3></td>
            </tr>
            <tr runat="server" id="trPayment">
                <td class="title">付款账号</td>
                <td colspan="3">
                    <div class="col">
                        <asp:Label ID="lblpaymentAccount" runat="server"></asp:Label>
                        <asp:Label ID="lblpayment" runat="server" CssClass="obvious"></asp:Label>
                        <input id="btnpayment" type="button" class="btn class1" runat="server" value="无权操作" disabled="disabled" />
                    </div>
                </td>
            </tr>
            <tr runat="server" id="trReceiving">
                <td class="title">收款账号</td>
                <td colspan="3">
                    <div class="col">
                        <asp:Label ID="lblReceivingAccount" runat="server"></asp:Label>
                        <asp:Label ID="lblReceiving" runat="server" CssClass="obvious"></asp:Label>
                        <input id="btnReceiving" type="button" class="btn class1" runat="server" value="有效" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="btns">
        <button class="btn class2" runat="server" id="btnGoBack">返回</button>
    </div>
    <asp:HiddenField ID="hidId" runat="server" />
    </form>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".col span").each(function () { $(this).eq(0).css({ "display": "inline-block", "width": "150px" }); });
            Account("btnReceiving", 2);
        });
        function Account(id, parament) {
            $("#" + id).bind("click", function () {
                if (!confirm("您确定你的操作吗?")) return false;
                if ($.trim($(this).val()) == "有效") {
                    send("IsEnblue", $(this), parament);
                } else {
                    send("IsDisable", $(this), parament);
                }
            });
        }
        function send(method, text, parament) {
            var paraments = JSON.stringify({ "id": $("#hidId").val(), "bytType": parament });
            text.attr("disabled", "disabled");
            sendPostRequest("/OrganizationHandlers/Account.ashx/" + method, paraments, function (e)
            {
                text.removeAttr("disabled");
                if (e == true)
                {
                    if (text.val() == "有效")
                        text.val("无效").prev().text("有效");
                    else
                        text.val("有效").prev().text("无效");

                }
            }, function (e) { text.removeAttr("disabled"); ; alert(e.responseText); });
        }
    </script>
</body>
</html>

