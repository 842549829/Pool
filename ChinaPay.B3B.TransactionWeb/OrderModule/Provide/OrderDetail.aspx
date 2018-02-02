<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="OrderDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.OrderDetail" %>

<%@ Reference Control="~/OrderModule/UserControls/PNRInfo.ascx" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagName="Bill" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台</title>
</head>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .con p
        {
            line-height: 1.5em;
            margin-bottom: 15px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <%--订单头部信息--%>
    <div runat="server" id="divHeader" class="column form" visible="false">
        <h3 class="titleBg">
            订单详情</h3>
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">
                    订单编号
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    订单状态
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label>
                    &nbsp;&nbsp;&nbsp; <a runat="server"
                        id="linkPrividerPolicy" class="obvious-a" target="_blank">政策详细</a>
                </td>
                <td class="title">
                    订单金额
                </td>
                <td>
                    <asp:Label runat="server" ID="lblAmount" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    产品类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    客票类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    原订单号
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOriginalOrderId" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    创建时间
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProducedTime" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    支付时间
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPayTime" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    出票时间
                </td>
                <td>
                    <asp:Label runat="server" ID="lblETDZTime" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
            
                      <td class="title">采购商</td>
                      <td colspan="5">
                    <asp:Label runat="server" ID="lblRelation" CssClass="obvious"></asp:Label>
                    <a href="#" runat="server" id="hrefPurchaseName"></a>
                      </td>
                </tr>
        </table>
        <div runat="server" id="divFailed" visible="false">
            拒绝出票：<asp:Label runat="server" ID="lblFailedReason"></asp:Label></div>
    </div>
    <%--编码组信息--%>
    <div runat="server" id="divPNRGroups" visible="false">
    </div>
    <%--政策备注--%>
    <div runat="server" id="divPolicyRemark" visible="false" class="column table">
        <div class="hd">
            <h2>
                政策备注</h2>
        </div>
        <div runat="server" id="divPolicyRemarkContent">
        </div>
    </div>
    <%--账单信息--%>
    <uc:Bill runat="server" ID="bill" Visible="false" />
    <%--底部操作按钮--%>
    <div class="btns">
        <button class="btn class1" runat="server" visible="false" id="btnConfirm">
            确认座位</button>
        <button class="btn class1" runat="server" visible="false" id="btnSupply">
            提供座位</button>
        <button class="btn class1" runat="server" visible="false" id="btnETDZ">
            出&nbsp;&nbsp;&nbsp;票</button>
        <button class="btn class1" runat="server" visible="false" id="btnOrderHistory">
            订单历史记录</button>
        <button class="btn class1" runat="server" visible="false" id="btnProcessingApplyforms">
            进行中的申请</button>
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    
    <asp:HiddenField runat="server" ID="hidRenderPrice" Value="0"/>
    </form>
    <a id="BeginEditTicket" style="display: none" data="{type:'pop',id:'EditTicket'}">
    </a>
    
     <div class="layer4" id="EditTicket" style="display:none;" >
        <h4>
            修改票号
            <a href="javascript:void(0)" class="close">关闭</a>
        </h4>
        <div class="layer4Tip">
            <p class="layerico">旧的票号：<span class="js"></span>-<span id="oldTicket"></span></p>
            <p>新的票号：<input type="text" id="newJs" class="text" style="width:35px" /><input type="text" class="text" id="newTicket" style="width:80px" />
            <span id="mutilNums" style="display:none;">- <input type="text" class="text" id="ticketNoEnd" style="width:35px"/></span></p>
            <div class="layer4Btns">
                <a href="javascript:updateTicketNo()" class="layerbtn btn1 fl" id="SaveTicket">保 存</a>
                <a href="javascript:void(0)" class="layerbtn btn2 fr close">取 消</a>
            </div>
        </div>
    </div>

</body>
</html>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Provide/OrderDetail.aspx.js" type="text/javascript"></script>
