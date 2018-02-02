<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderPay.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.OrderPay" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagName="Voyage" TagPrefix="uc" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagName="Passenger"
    TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link href="/Styles/bank.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .con p
        {
            line-height: 1.5em;
            margin-bottom: 15px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="payContent">
        <%-- 进度条 --%>
        <div runat="server" id="divProgress" class="corner">
            <div class="step">
                <div class="line-bg">
                </div>
                <div style="width: 50%;" class="active-line-bg">
                </div>
                <i class="active">●<span>1</span></i> <i class="active" style="left: 48%;">●<span>2</span></i>
                <i class="active" style="left: 174px">●<span>3</span></i>
                <label style="left: -20px;">
                    选择航班</label>
                <label style="left: 40%">
                    补全信息</label>
                <label style="left: 75%">
                    付款完成订单</label>
            </div>
        </div>
        <%-- 订单预订提示信息 --%>
        <div runat="server" id="divTipInfo" class="obvious-tips">
            <!-- <i class="ico"></i> -->
            <p runat="server" id="pTipsInfo" class="obvious success">
            </p>
            <p class="txt-l" style="color:#666; font-size:14px;padding:10px 0 0 50px;"><asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>提醒您：请尽快完成支付，避免编码被取消（被X编码）或被NO位；由于长时间不支付造成的NO位无条件取消订单且平台不承担任何责任。</p>
        </div>
        <uc:Voyage runat="server" ID="ucVoyages"></uc:Voyage>
        <uc:Passenger runat="server" ID="ucPassengers"></uc:Passenger>
        <%-- 支付方式 --%>
        <div class="column">
            <div class="hd">
                <h2>
                    支付方式</h2>
            </div>
            <div class="tab tab-a">
                <ul runat="server" id="divPayTypes" class="payNav">
                </ul>
                <div class="tab-con">
                    <strong class="large-font">应付金额：<asp:Label runat="server" ID="lblPayAmount" CssClass="price"></asp:Label></strong>
                    <div runat="server" id="divPayTypeDetails">
                    </div>
                </div>
            </div>
        </div>
        <div class="btns" runat="server" id="divOperations" style="padding-top:15px;">
            <a class="btn class1" id="btnPay" target="_blank" style="padding:8px 20px;">确认支付</a> <a class="btn class2"
                id="btnCancel" style="padding:8px 20px;">稍后支付</a>
            <input type="button" class="btn class2" id="btnWaiting" value="执行中" disabled="disabled"
                style="display: none;" />
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidBusinessId" />
    <asp:HiddenField runat="server" ID="PayPoolBindAccount" />
    <asp:HiddenField runat="server" ID="hidBusinessType" />
    <asp:HiddenField runat="server" ID="hidProductType" />
    <asp:HiddenField runat="server" ID="hidPublishRefundRule" />
    <asp:HiddenField runat="server" ID="hidShowProductAttention" />
    <asp:HiddenField runat="server" id="ShowTicketPrice" Value="1" />
    <asp:HiddenField runat="server" ID="orderIsImport" Value="0"/>
    <%--<asp:HiddenField runat="server" ID="hidPayHost" />--%>
    <asp:HiddenField runat="server" ID="hidUserName" />
    <%-- 特殊票提示 --%>
    <a id="specialProductAttention" style="display: none" data="{type:'pop',id:'divSpecialProductAttention'}">
    </a>
    <div id="divSpecialProductAttention" class="layer">
        <h3>
            特别提示</h3>
        <div class="con">
            <p>
                由于您预订的是特殊产品，您的订单提交后，需要供应商为您进行座位申请并会尽快为您进行座位确认，当供应商为您确认座位后您就可以直接支付并等待供应商为您出票，感谢您对平台的支持。</p>
        </div>
        <div class="btns">
            <a id="btnSpecialProductConfirm" class="btn class1 close">确&nbsp;&nbsp;&nbsp;定</a></div>
    </div>
    <%-- 支付成功 --%>
    <a id="paySuccess" style="display: none" data="{type:'pop',id:'divPaySuccess'}">
    </a>
    <div id="divPaySuccess" class="layer">
        <div class="con">
            <strong id="paySuccessAmount"></strong>
            <p>
                对方将立即收到您的付款信息。</p>
            <p>
                如果您有未付款信息，<a id="btnPayOtherOrder">查看并继续付款</a></p>
        </div>
        <div class="btns">
            <a id="btnPaySuccess" class="btn class1">确&nbsp;&nbsp;&nbsp;定</a></div>
    </div>
    <%-- 支付跳转提示 --%>
    <a id="payAttention" style="display: none" data="{type:'pop',id:'divPayAttention'}">
    </a>
    <div id="divPayAttention" class="layer">
        <h3>
            支付</h3>
        <div class="con">
            <strong>请您在新打开的支付页面上完成付款。</strong>
            <p>
                付款完成前请不要关闭此窗口。
            </p>
            <p>
                完成付款后请根据您的情况点击下面的按钮完成操作。</p>
        </div>
        <div class="btns">
            <a id="btnPayComplete" class="btn class1">已完成付款</a> <a id="btnPayError" class="btn class2">
                付款遇到问题</a>
        </div>
    </div>
    <!-- 余额不足支付提示信息 -->
    <a id="payFaild" style="display: none" data="{type:'pop',id:'payTip'}"></a>
    <div class="layer4" id="payTip" style="display: none;">
        <h4>
            机票支付 <a href="#" class="close">关闭</a>
        </h4>
        <div class="layer4Content">
            <strong class="large-font">本次交易额： <span class="price">
                <asp:Literal text="0.00" ID="liShouldPay1" runat="server" /></span> 元 </strong>
            <strong class="large-font">您购买的是： <span class="curr1" id="TipVoyages">
                <asp:literal text="2012-10-01 昆明-西安 东方航空 MU5927机票" ID="voyagesInfo" runat="server" /></span>
            </strong>
            <div class="layer4Title">
                <span>国付通余额支付</span>
            </div>
            <table>
                <tbody>
                    <tr>
                        <td class="title">
                            应付总价：
                        </td>
                        <td>
                            <span class="price" id="shouldPay">
                                <asp:Literal text="0.00" ID="liShouldPay" runat="server" /></span>元
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            国付通账户名：
                        </td>
                        <td>
                            <asp:Label text="" ID="liPayAccount" runat="server" />
                        </td>
                    </tr>
                    <tr id="notSame3">
                        <td class="title">
                            国付通余额：
                        </td>
                        <td>
                            <span>
                                <asp:Literal text="0.00" ID="liRestMoney" runat="server" /></span>元
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="layer4Tip">
                <p class="layerico fl">
                    您的账户余额不足<span id="notSame1">，还差<span class="price" id="needMore">
                        <asp:Literal text="0.00" ID="liNeedMore" runat="server" />
                    </span>元</span>
                </p>
                <a class="layerbtn btn1 fl" id="notSame2" href="javascript:ReturnUrl('5')">充 值</a>
                <span class="layerPrompt clear">国付通支持网上银行、支付宝、财付通、快钱等多种在线充值 </span>
            </div>
        </div>
    </div>
    <!-- 订单支付超时提示信息 -->
    <a id="payDelayed" style="display: none" data="{type:'pop',id:'payDelay'}"></a>
    <div class="layer4" id="payDelay" style="display: none;">
        <h4>
            机票支付 <a href="/OrderModule/Purchase/OrderList.aspx?Search=Back" id="closeHandler" class="close">关闭</a>
        </h4>
        <div class="layer4Tip">
            <p class="layerico layerfull">
                您所购买的 <span class="curr1">
                    <asp:literal text="2012-10-01 昆明-西安 东方航空 MU5927机票" ID="voyagesInfo1" runat="server" /></span>
            </p>
            <span class="layerOvertime">
                <asp:Label text="" ID="liErrorTip" runat="server" />
            </span>
            <div class="layer4Btns" id="ErrorOption" runat="server" style="width: 80px">
                <a class="layerbtn btn1 fl" runat="server" id="ReSearchFlight" target="_parent">重新查询</a>
                <asp:LinkButton text="删除该订单" ID="CancleOrder" runat="server" Visible="False" OnClick="CancleThisOrder"
                    CssClass="layerlink fl" />
            </div>
        </div>
    </div>
    
    
    <!-- 编码有效性提示信息 -->
    <a id="notValidateTip" style="display: none" data="{type:'pop',id:'notValidateTipContent'}"></a>
    <div class="layer4" id="notValidateTipContent" style="display: none;">
        <h4>
            机票支付 <a href="javascript:void(0)" class="close">关闭</a>
        </h4>
        <div class="layer4Tip">
            <p class="imTips layerfull">
                平台暂时无法验证您的位置是否有效,请您自行验证<br /> <span style="color:#FF6400;font-weight: 900">
                您可以继续支付完成订单最大限度确保您的订单能够出票

                                                </span>
            </p>
            <span class="layerOvertime">
                <asp:Label text="" ID="Label1" runat="server" />
            </span>
            <div class="btns">
                <a class="btn class1 close">编码有效 继续支付</a>
                <a class="btn class2" href="/OrderModule/Purchase/OrderList.aspx?Search=Back">编码失效 取消订单</a>
            </div>
        </div>
    </div>
        <%--<input type="button" value="余额不足提示" onclick="$('#payFaild').click()"/>
    <input type="button" value="订单超过支付时限" onclick="$('#notValidateTip').click()"/>--%>
    
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/widget/d.tabs.js" type="text/javascript"></script>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/pay.js?20130523" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        if ($("#ShowTicketPrice").val() != "1")
        {
            $(".ticketPrice").text("出票后可见");
        }
    });

</script>
