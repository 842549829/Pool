<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendSet.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSSendSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>接收短信设置</title>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        接收短信设置</h3>
    <div class="form">
        <table>
            <tr>
                <td class="title">
                    接收设置
                </td>
                <td>
                    在以下情况下接收短信：<br />
                    <asp:CheckBox runat="server" Text="出票后给我发短信" ID="chkChupiao" /><span class="obvious1 pad-l">该信息由您接收，乘机人不可见（选择该项后您的订单出票完成时系统会自动短信通知您，短信条数从您可用短信条数中扣除）</span><br />
                    <asp:CheckBox runat="server" Text="航班变动时给我发短信" ID="chkBina" /><span class="obvious1  pad-l">航班延误或取消时，系统会给您发送短信通知您，短信条数从您可用短信条数中扣除</span><%--<br />
                    <asp:CheckBox runat="server" Text="允许国付通账号绑定本账号发送短信" ID="chkBang" /><span class="obvious1  pad-l">选择此项后您的国付通账号的短信服务可以通过绑定本账号，实现自动发送</span>--%>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保存" 
                        onclick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
