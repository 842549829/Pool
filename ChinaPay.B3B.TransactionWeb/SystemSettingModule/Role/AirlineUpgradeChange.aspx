<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirlineUpgradeChange.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.AirlineUpgradeChange" EnableViewState="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票交易平台 中国首家创新型电子机票销售平台 全球首家任意编码行程支持平台 各航空公司升舱规定</title>
</head>
    <link href="/Styles/icon/main.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <div style="margin-right: 15px;" class="form">
        <h3 class="titleBg">
            各航空公司升舱规定<span class="obvious pad-l">（此规定仅供参考，最终以航空公司规定为准）</span><a href="./AirlineRetreatChangeNew.aspx" class="pad-l obvious-a">查看各航空公司退废票规定</a>
        </h3>
        <table class="shengcang">
            <tbody runat="server" id="tabContext">
            </tbody>
        </table>
        <div class="btns">
        <a href="./AirlineRetreatChangeNew.aspx" class="btn class1 obvious-a">查看各航空公司退废票规定</a></div>
    </div>
    </form>
</body>
</html>
