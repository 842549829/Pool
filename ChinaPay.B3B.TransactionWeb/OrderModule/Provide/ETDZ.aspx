<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ETDZ.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ETDZ" %>

<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<%@ Register Src="~/OrderModule/UserControls/PNRInfo.ascx" TagName="PNR" TagPrefix="uc" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagName="Bill" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link href="../../Styles/icon/fontello.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .addTicketNo{ margin-left:5px; display:inline-block; width:20px; height:22px; background:red; color:#ffffff; cursor:pointer;}
        .removeTicketNo{margin-left:5px; display:inline-block; width:20px; height:22px; background:red; color:#ffffff; cursor:pointer; }
        #passengers td
        {
            vertical-align: middle;
        }
        .settleCode
        {
            width: 35px;
        }
        .ticketNOend
        {
            width: 40px;
        }
        .ticketNo
        {
            width: 85px;
        }
        .btn
        {
            position:static;
        }
        #passengers tr:hover td {
            background-color: #FFE2DC;
        }
        .zongjia
        {
            font-size:20px;
            font-weight:bolder;
        }
    </style>
<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h2>
        出票</h2>
    <div class="form column">
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
                    采购商
                </td>
                <td>
                    <asp:Label runat="server" ID="lblRelation" CssClass="obvious"></asp:Label>
                    <a href="#" runat="server" id="hrefPurchaseName"></a>
                </td>
            </tr>
        </table>
    </div>
    <%--账单信息--%>
    <uc:Bill runat="server" ID="bill" />
    <%--编码组信息--%>
    <uc:PNR runat="server" ID="pnrGroups" />
    
    <%--出票信息--%>
        <div class="colume" id="ETDZInfo">
    	<div class="hd">
    		<h2>出票信息</h2>
    	</div>
    	<div class="PNRCodeBox">
    		<span class="condition" id="TicketTypeContainer" runat="server">
    		    出票方式：<input type="radio" name="ticketType" value="1" id="TicketType1" runat="server"/><label for="TicketType1">BSP</label>
    		    <input type="radio" value="0" name="ticketType" id="TicketType2" runat="server" /><label for="TicketType2">B2B</label>
                &nbsp;&nbsp;
    		</span>
    		<span class="condition">
    			出票Office号：
                <asp:dropdownlist runat="server" ID="ddlOfficeNo" CssClass="custed">
                </asp:dropdownlist>
    		</span>
            
            
            <div runat="server" id="divNewPNRInfo">
    				<span class="condition">
    					<%--<input type="radio" name="needChangePNR" checked="checked" ><label>不需要</label>
    					<input type="radio" id="needChangePNR" name ="needChangePNR"><label>需要</label>--%>
                        <input type='checkbox' class='needChangePNR' id="needChangePNR"  /><label for="needChangePNR">需换编码出票</label>
    				</span>
    				<div class="changeCodeBox">
    				<span class="pad-r">
    					新小编码：
    					<asp:TextBox runat="server" ID="txtNewPNRCode" CssClass="text"></asp:TextBox>
    				</span>
    				<span class="pad-r">
    					新大编码：
    					<asp:TextBox runat="server" ID="txtNewBPNRCode" CssClass="text"></asp:TextBox>
    				</span>
    				<span class="obvious1 pad-r">
    					原小编码：
    					<b><asp:Label runat="server" ID="lblPNRCode"></asp:Label></b>
    				</span>
    				<span class="obvious1 pad-r">
    					原大编码：
    					<b><asp:Label runat="server" ID="lblBPNRCode"></asp:Label></b>
    				</span>
    				</div>
    			</div>
            <div runat="server" id="NOChangePNR">
                <span class="condition">
    					换编码出票：采购不允许换编码出票，请使用该编码进行出票：<asp:Label Text="" ID="lbOriginalPNR"
                    runat="server" />
    				</span>

            </div>
    	</div>
    </div>

    

    <%--政策备注--%>
    <div class="column table">
        <div class="hd">
            <h2>
                政策备注</h2>
        </div>
        <div runat="server" id="divPolicyRemarkContent">
        </div>
    </div>
    
    <p class="Tips" runat="server" id="Tips" visible="False">
            提示：采购该机票的用户已选择了：愿意对编码进行授权，您可以进行出票。
    </p>

    <%--拒绝出票原因--%>
    <a id="lnkDeny" data="{type:'pop',id:'divDeny'}" style="display: none"></a>
    <div id="divDeny" class="form" style="display: none; background-color: white; padding: 20px; width:500px;">
        <h2>
            拒绝出票</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tr>
                <td class="title">
                    类型
                </td>
                <td>
                    <div class="check">
                        <input type="radio" id="reasonType1" value='<%=((int)SystemDictionaryType.RefuseETDZSelfReason).ToString() %>'
                            name="radioone" /><label for="reasonType1">自身原因</label>
                        <input type="radio" id="reasonType2" value='<%=((int)SystemDictionaryType.RefuseETDZPlatformReason).ToString() %>'
                            name="radioone" /><label for="reasonType2">平台原因</label>
                        <input type="radio" id="reasonType3" value='<%=((int)SystemDictionaryType.RefuseETDZPurchaseReason).ToString() %>'
                            name="radioone" /><label for="reasonType3">采购原因</label>
                        <input type="radio" id="reasonType4" value='<%=((int)SystemDictionaryType.RefuseETDZOtherReason).ToString() %>'
                            name="radioone" /><label for="reasonType4">其他原因</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">
                    原因
                </td>
                <td>
                    <select id="selDenyReason" style="width:312px">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">
                    描述详情
                </td>
                <td>
                    <textarea id="txtDenyReason" rows="5" cols="50" class="text"></textarea>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <input class="btn class1" type="button" value="提&nbsp;&nbsp;&nbsp;交" onclick="commitDenyETDZ();" />
                    <input class="btn class2 close" type="button" value="返&nbsp;&nbsp;&nbsp;回" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hidReturnUrl" />
    <%--底部操作按钮--%>
    <div class="btns">
        <input type="button" class="btn class1" runat="server" visible="false" id="btnETDZ"
            value="出&nbsp;&nbsp;&nbsp;票" onclick="etdz();" />
        <input type="button" class="btn class1" runat="server" visible="false" id="btnDeny"
            value="拒绝出票" onclick="denyETDZ();" />
        <asp:Button CssClass="btn class1" runat="server" Visible="false" ID="btnReleaseLockAndBack"
            OnClick="btnReleaseLockAndBack_Click" Text="解锁并返回" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    </form>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/OrderModule/etdz.js?20130051701" type="text/javascript"></script>
