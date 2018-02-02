<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateBuyWorkInfo.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.UpdateBuyWorkInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h2>
        账户信息</h2>
    <div class="form">
        <table>
            <tr>
                <td class="title">
                    付款账号
                </td>
                <td>
                    <asp:TextBox ID="txtPayment" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                    <span id="lblPayment" runat="server" class="obvious"></span>&nbsp;
                    <input runat="server" type="button" value="修改" class="btn class1" id="btnPayment" />
                </td>
                <td class="title">收款账号</td>
                <td>
                    <asp:TextBox ID="txtReceiving" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                    <span id="lblReceiving" runat="server" class="obvious" style="display:block;"></span>
                    <input runat="server" type="button" value="修改" class="btn class1" id="btnReceiving" />
                </td>
            </tr>
        </table>
    </div>
    <div class="btns">
        <input class="btn class2" type="button" onclick="return window.location.href='./CompanyList.aspx?Search=Back'" value="返回" />
    </div>
    <asp:HiddenField runat="server" ID="hidId" />
    <asp:HiddenField runat="server" ID="hfdIsHiddenReceiving" Value=""/>
    </form>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Account/UpdateAccount.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var isHiddenReceiving = $.trim($("#hfdIsHiddenReceiving").val());
            if (isHiddenReceiving.length > 0 && isHiddenReceiving == "true") {
                $("table tr td:eq(1)").attr("colspan", "3");
                $("table tr td:eq(2),table tr td:eq(3)").remove();
            }
        });
    </script>
</body>
</html>
