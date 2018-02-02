﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundApplyformDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.RefundApplyformDetail" %>

<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagPrefix="uc" TagName="Voyage" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagPrefix="uc" TagName="Passenger" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagPrefix="uc" TagName="Bill" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">
        申请单详情</h3>
    <div class="column form">
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
                    申请单号
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    申请类型
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">
                    状态
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单编号
                </td>
                <td>
                    <a runat="server" id="linkOrderId" class="obvious-a"></a>
                </td>
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
            </tr>
            <tr>
                <td class="title">
                    采购方
                </td>
                <td>
                    <a runat="server" id="linkPurchaser" class="obvious-a"></a>
                </td>
                <td class="title">
                    出票方
                </td>
                <td colspan="3">
                    <a runat="server" id="linkProvider" class="obvious-a"></a>
                </td>
            </tr>
            <tr>
                <td class="title">
                    编码
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <uc:Voyage runat="server" ID="voyages" />
    <uc:Passenger runat="server" ID="passengers" />
    <!--附件-->
    <div runat="server" id="divApplyAttachment" class="column table">
    </div>
    <div class="column">
        <h3 class="titleBg">
            申请/处理信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>
                        提交时间
                    </th>
                    <th>
                        原因
                    </th>
                    <th>
                        处理时间
                    </th>
                    <th>
                        处理结果
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedReason"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProcessedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProcessedResult"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="table" runat="server" id="divRefundFeeInfo">
        </div>
    </div>
    <uc:Bill runat="server" ID="bill" />
     <div class="form layer2 hidden" id="divLayerImage">
        <h4>
            原图<a href="javascript:void(0);" class="close">关闭</a></h4>
        <img style="max-height:500px; max-width:500px"/>
    </div>
    <a id="aUploadAttachmentLayer" style="display: none;" data="{type:'pop',id:'divUploadAttachmentLayer'}"></a>
    <div class="form layer3 hidden" id="divUploadAttachmentLayer">
        <h4>上传附件<a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">上传退票附件</p>
            <asp:FileUpload  CssClass="text"  runat="server" ID="fileAttachment"/><br />
             <label class="obvious1">附件类型支持jpg\png\bmp三种类型，附件小于600kb</label>
        </div>
        <div class="txt-c mar">
            <asp:Button runat="server" ID="btnConfirm" Text="确定" CssClass="btn class1" onclick="btnConfirm_Click" />
            <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
        </div>
    </div>
    </form>
    <div class="btns">
        <button class="btn class1" runat="server" id="btnProcess" visible="false">
            处&nbsp;&nbsp;&nbsp;理</button>
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
</body>
</html>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Operate/UploadApplyAttachment.js" type="text/javascript"></script>
