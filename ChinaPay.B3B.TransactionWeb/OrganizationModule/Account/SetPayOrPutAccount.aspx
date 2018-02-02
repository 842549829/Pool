<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPayOrPutAccount.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Account.SetPayOrPutAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收付款账号设置</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="column table">
        <table>
            <colgroup>
                <col class="w30" />
                <col class="w40" />
            </colgroup>
            <tbody id="tbPut" runat="server">
                <tr>
                    <td colspan="2"> <h2> 收款账号设置</h2></td>
                </tr>
                <tr>
                    <th>收款账号</th>
                    <th>操作</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPutAccount"></asp:Label>
                        <asp:Label runat="server" ID="lblPutStatus" CssClass="obvious"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPutAccount" CssClass="text"></asp:TextBox>
                    </td>
                    <td>
                        <a href="#" class="obvious" id="btnPut" runat="server">设置</a>
                        <span style="display: none;"><a href="#" id="btnPutSvae">保存</a>&nbsp;&nbsp;<a href="#">取消</a></span>
                    </td>
                </tr>
            </tbody>
            <tbody>
                <tr>
                    <td colspan="2"><h2>付款账号设置</h2></td>
                </tr>
                <tr>
                    <th>付款账号</th>
                    <th>操作</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPayAccount"></asp:Label>
                        <asp:Label runat="server" ID="lblPaySatus" CssClass="obvious"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPayAccount" CssClass="text"></asp:TextBox>
                    </td>
                    <td>
                        <a href="#" class="obvious" id="btnPay" runat="server">设置</a> 
                        <span style="display: none;"><a href="#" id="btnPaySvae">保存</a>&nbsp;&nbsp;<a href="#">取消</a></span>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <input type="hidden" runat="server" id="hidId" />
    </form>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnPutSvae").click(function () {
                if (!checking($("#txtPutAccount").val())) { return false; } else { send(3, $("#txtPutAccount")); return true; }
            });
            $("#btnPaySvae").click(function () {
                if (!checking($("#txtPayAccount").val())) { return false; } else { send(2, $("#txtPayAccount")); return true; }
            });
            //设置
            $("#btnPay,#btnPut").click(function () {
                var self = $(this).next("span").show().end().hide().parent("td").prev("td");
                self.find(":text").val(self.find("span:first").text()).show().prev("span").hide().prev("span").hide();
            });
            $("a:contains('取消')").click(function () {
                $(this).parent("span").hide().prev("a").show().parent("td").prev("td").find(":text").hide().prev("span").show().prev("span").show();
            });
            var send = function (type, account) {
                var parameters = JSON.stringify({ "id": $("#hidId").val(), "account": $.trim(account.val()), "bytType": type });
                sendPostRequest("../../OrganizationHandlers/Account.ashx/UpdateAccount", parameters, function (e) {
                     if (e == true) {
                       account.hide().prev("span").show().prev("span").text(account.val()).show().end().end().parent("td").next("td").find("a:first").show().next("span").hide();
                                alert("修改成功");
                      } else {
                        accountSuccess(account);
                        alert("修改失败");
                     }
                }, function () {
                    accountSuccess(account);
                    alert("修改异常");
                });
            };
            var accountSuccess = function (account) {
                account.hide().prev("span").show().prev("span").show().end().end().parent("td").next("td").find("a:first").show().next("span").hide();
            };
            //验证
            var checking = function (text) {

                if (text == "") {
                    alert("账号不允许为空");
                    return false;
                }
                var reg = /^[\w@\.]{5,20}$/;
                if (!reg.test(text)) {
                    alert("账号格式错误");
                    return false;
                }
                return true;
            };
        });
    </script>
</body>
</html>
