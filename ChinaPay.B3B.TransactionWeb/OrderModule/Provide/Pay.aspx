<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pay.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.Pay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <asp:HiddenField ID="hfdRequetUrl" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        var url = $("#hfdRequetUrl").val();
        if (url != "") {
            window.location.href = url;
        }
        //        var platformText = $("#hfdPlatformText").text();
        //        var payInterfaceValue = $("#hfdPayInterfaceValue").val();
        //        var internalOrderId = $("#hfdInternalOrderId").text();
        //        var externalOrderId = $("#hfdExternalOrderId").text();
        //        var orderAmount = $("#hfdOrderAmount").text();
        //        var parmeters = JSON.stringify({ "platformText": platformText, "payInterfaceValue": payInterfaceValue, "internalOrderId": internalOrderId,
        //            "externalOrderId": externalOrderId, "orderAmount": orderAmount
        //        });
        //        sendPostRequest("/OrderHandlers/Order.ashx/QueryManualPayUrl", parmeters, function (result) {
        //            if (result != "") {
        //                window.location.href = result;
        //            } else {
        //                alert("地址为空");
        //            }
        //        }, function (e) {
        //            if (e.statusText == "timeout") {
        //                alert("服务器忙");
        //            } else {
        //                alert(e.responseText);
        //            }
        //        });
    })
</script>