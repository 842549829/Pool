<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.OrderList"
    EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单查询</title>
</head>
<body>
    <form runat="server" id="form">
    <h3 class="titleBg clearfix">
        <span class="fl">订单查询</span><%--<a href="/About/download.aspx" target="_parent" class="download fr obvious-a">下载订单提醒工具</a>--%></h3>
    <ul class="orderNav clearfix" id="ulProduct" runat="server">
    </ul>
    <div class="orderBox">
        <div class="orderCondition clearfix">
            <span>创建日期：
                <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>-
                <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"></asp:TextBox>
            </span><span>乘机人：
                <asp:TextBox runat="server" ID="txtPassenger" CssClass="text"></asp:TextBox>
            </span><span>PNR：
                <asp:TextBox runat="server" ID="txtPNR" CssClass="text text-s"></asp:TextBox>
            </span><span>订单编号：
                <asp:TextBox runat="server" ID="txtOrderId" CssClass="text"></asp:TextBox>
            </span>
            <input type="button" class="btn class1 fl" value="查询" id="btnSerach" />
        </div>
        <ul class="orderSubnav clearfix" id="ulOrderStatus" runat="server">
        </ul>
    </div>
    <div class="table" id='data-list'>
        <table id='dataList'>
            <thead>
                <tr>
                    <th>
                        订单编号
                    </th>
                    <th>
                        PNR
                    </th>
                    <th>
                        行程
                    </th>
                    <th>
                        乘机人
                    </th>
                    <th>
                        票面价<br />
                        民航基金/燃油
                    </th>
                    <th>
                        结算价<br />
                        返佣
                    </th>
                    <th>
                        创建时间
                    </th>
                    <th>
                        状态<br />
                        出票等待(分钟)
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div id="emptyInfo" class="box hidden">
        没有任何符合条件的查询结果</div>
    <div class="btns" id="divPagination">
    </div>
    <a id="divOption" style="display: none;" data="{type:'pop',id:'divMessage'}"></a>
    <div class="layer4 hidden" id="divMessage" style="width: 700px;">
        <h4>
            短信通知<a href="#" class="close">关闭</a></h4>
        <table style="width: 600px;">
            <colgroup>
                <col class="w20" />
                <col />
            </colgroup>
            <tr>
                <td class="title">
                    手机号码
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="text" Width="50%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    短信内容
                </td>
                <td>
                <textarea runat="server" class="text fl" readonly="readonly" id="txtContext" style="width:100%;min-height:100px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="fr">
                    <span class="obvious1">已输入<span id="spanSmsNum" class="obvious font-b">0</span>字，将分<span
                        id="spanNum" class="obvious">1</span>条发送。</span>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <input type="button" id="btnSubmit" class="btn class1" value="提交" />
                    <input type="button" id="btnCancel" value="取消" class="btn class2 close" />
                </td>
            </tr>
        </table>
    </div>

    <a id="divMain" style="display:none;" data="{type:'pop',id:'divRemind'}"></a>
     <div class="layer3 hidden" id="divRemind">
        <h4>催单操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">请注意，<span id="movedTime" runat="server"></span>分钟内能进行一次催单，请在下方输入您的催单理由或备注。</p>
            <textarea class="text" cols="105" rows="4" id="txtRemindContent"></textarea>
        </div>
        <div class="txt-c mar">
            <input type="button" id="btnSubmitRemind" class="btn class1" value="提交" />
                    <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
        </div>
    </div>
   <%-- <div class="layer4 hidden" id="divRemind">
        <h4>
            催单操作<a href="#" class="close">关闭</a></h4>
         <div class="importantBox" id="LastDiv">
        <p class="important">
            请在下方输入您的催单理由或备注。</p>
    </div>
        <table>
            <colgroup>
                <col class="w20" />
                <col />
            </colgroup>
            <tr>
                <td colspan="2">
                <textarea runat="server" class="text fl" id="txtRemindContent" rows="5" cols="60"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <input type="button" id="btnSubmitRemind" class="btn class1" value="提交" />
                    <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
                </td>
            </tr>
        </table>
    </div>--%>
    <asp:HiddenField ID="hfdOrderId" runat="server" />
    <asp:HiddenField ID="hfdContent" runat="server" />
    <asp:HiddenField ID="hfdOperatorAccount" runat="server" />
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/DateExtandJSCount-min.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js?2013117" type="text/javascript"></script>
<script type="text/javascript">
    var IsFirstLoad = <%=IsFirstLoad?"true":"false" %>;
    var ServerTime = Date.fromString('<%=DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm") %>');
</script>
<script src="/Scripts/OrderModule/purchase/OrderList.aspx.js?20130418" type="text/javascript"></script>
<script type="text/javascript">
    function PrintVoyage(orderId, passengerName) {
        "/Index.aspx?/OrderModule/Purchase/itinerary.html";
    }   

</script>
<script type="text/javascript">
    $(function () {
        $("#txtPhone").OnlyNumber().LimitLength(11);
        $("#btnSubmit").click(function () {
            if (valiate()) {
                var orderId = $("#hfdOrderId").val();
                var phone = $("#txtPhone").val();
                var content = $("#hfdContent").val();
                sendPostRequest("/OrderHandlers/Order.ashx/PurchaseSendMessage", JSON.stringify({ "orderId": orderId, "phone": phone, "content": content }), function (result) {
                    alert("短信发送成功");
                    $(".close").click();
                    queryOrders($("#lblCurrentPage").text());
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            }
        });

        $("#btnSubmitRemind").click(function () {
            if (valiateRemindContent()) {
                var orderId = $("#hfdOrderId").val();
                var content = $("#txtRemindContent").val();
                var operatorAccount = $("#hfdOperatorAccount").val();
                sendPostRequest("/OrderHandlers/Order.ashx/Reminded", JSON.stringify({ "orderId": orderId, "remindContent": content, "operatorAccount": operatorAccount }), function () {
                    alert("采购催单成功");
                    $(".close").click();
                    queryOrders($("#lblCurrentPage").text());
                },
                function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            }
        });

    })
    function PrintMessage(orderId) {
        $("#hfdOrderId").val(orderId);
        sendPostRequest("/OrderHandlers/Order.ashx/QueryTemplate", JSON.stringify({ "orderId": orderId }), function (result) {
            if (!result.IsSended) {
                if (result.IsNeedSend) {
                    $("#txtPhone").val(result.FirstPassengerPhone);
                    $("#txtContext").val(result.Template);
                    $("#hfdContent").val(result.Template);
                    $("#spanSmsNum").html($.trim($("#txtContext").val()).length);
                    $("#spanNum").html(parseInt(Math.ceil(parseFloat($.trim($("#txtContext").val()).length) / 61)));
                    $("#divOption").click();
                } else {
                    alert("该订单乘机人已退完");
                }
            } else {
                alert("该订单已发送短信");
            }
        }, function (e) {
            if (e.statusText == "timeout") {
                alert("服务器忙");
            } else {
                alert(e.responseText);
            }
        });
    }
    function valiate() {
        if ($.trim($("#txtPhone").val()).length == 0) {
            alert("请输入手机号码");
            return false;
        }
        var phonePattern = /^1[3458]\d{9}$/;
        if (!phonePattern.test($.trim($("#txtPhone").val()))) {
            alert("手机号码格式错误");
            $("#txtPhone").select();
            return false;
        }
        return true;
    }

    function Reminded(orderId) {
        $("#hfdOrderId").val(orderId);
        $("#txtRemindContent").val("");
        $("#divMain").click();
    }

    function valiateRemindContent() {
        if ($.trim($("#txtRemindContent").val()).length == 0) {
            alert("请输入催单理由或备注");
            return false;
        }
        if ($.trim($("#txtRemindContent").val()).length > 200) {
            alert("催单理由或备注字数不能超过200");
            return false;
        }
        return true;
    }
</script>
