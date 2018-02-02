<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessRefund.aspx.cs" 
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ProcessRefund" %>
<%@ Reference Control="~/UserControl/OutPutImage.ascx" %>
<%@ Register Src="../UserControls/Voyage.ascx" TagName="Voyage" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Passenger.ascx" TagName="Passenger" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/core.css" rel="stylesheet" type="text/css" />
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        申请单详情</h3>
    <%--订单头部信息--%>
    <div class="form">
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
                    申请单号：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyformId" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    申请类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyType" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    状态：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单编号：
                </td>
                <td>
                    <a runat="server" id="linkOrderId" class="red"></a>
                </td>
                <td class="title">
                    产品类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProductType" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    客票类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTicketType" CssClass="red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    编码：
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <uc1:Voyage ID="voyages" runat="server" />
    <uc2:Passenger ID="passengers" runat="server" Tip="机票信息" />
    <%--政策备注--%>
    <div runat="server" id="divPolicyRemark" visible="false" class="column table">
        <div class="hd">
            <h2>
                政策备注</h2>
        </div>
        <div runat="server" id="divPolicyRemarkContent">
        </div>
    </div>
    <!--附件-->
    <div runat="server" id="divApplyAttachment" class="column table">
    </div>
    <div class="column">
        <h3 class="titleBg">
            申请信息</h3>
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
    </div>
    <div runat="server" id="ModPriceInfo" visible="false">
        <div class="fl">
            票面价：<input type="text" id="newPrice" /></div>
        <div class="fl">
            民航基金：<input type="text" id="newAirPortFee" /></div>
    </div>
    <div class="column" id="SetServiceCharge" runat="server" visible="False">
        <p class="b" style="line-height: 30px;">
            退服务费金额：<span class="price pad-r" runat="server" id="renderedServiceCharge">0.00</span>元
            <input type="hidden" runat="server" id="ChangedServiceCharge" />
            <a id="setTrigger" class="obvious-a" href="javascript:void(0)">修改</a> <span id="modifyServiceCharge"
                style="display: none;">
                <input type="text" id="newCharge" class="text text-s" />
                <input type="button" class="btn class1" value="保存" id="save"></span> <span>退服务费总额：<span
                    id="totalServiceCharge" class="price pad-r"> </span>元(请在左边输入单人退服务费金额，系统会自动计算总额)</span></p>
        <asp:TextBox TextMode="MultiLine" Style="border-color: #dedede; color: #999; height: 100px;
            padding: 10px 1%; margin-top: 10px; width: 97%;" ID="ChangeServiceChargeReason"
            runat="server">请在此输入修改服务费的理由或与采购和供应（或产品方）协商过程中的一些简要记录备注，以备后期查询</asp:TextBox>
    </div>
    <div class="btns">
        <asp:Button Text="重新退/废票" ID="btnReDeal" runat="server" CssClass="btn class1" OnClick="btnReDeal_Click"
            Visible="False" OnClientClick="return CheckPriceInput()" />
        <asp:Button Text="拒绝退/废票" ID="btnDenyRefund" runat="server" CssClass="btn class1"
            OnClientClick="return ShowReashoInput()" Visible="False" />
        <asp:Button Text="指向出票方" ID="RedircetToProvider" runat="server" CssClass="btn class1"
            OnClick="RedircetToProvider_Click" Visible="False" />
        <asp:Button Text="指向出票方退票" ID="RedircetToProviderRefund" CssClass="btn class1" runat="server"
            Visible="False" OnClick="ToRefund" OnClientClick="return CheckPriceInput()" />
        <%--<asp:Button class="btn class1" runat="server" id="btnAgree" Text="可退/废票" onclick="btnAgree_Click"
            Visible="False" />--%>
        <%--<button class="btn class1" id="btnDeny" onclick="ShowReashoInput()"> 拒绝退/废票</button>--%>
        <%--<asp:Button ID="btnNotAgree" runat="server" CssClass="class1 btn" Text="不可退票/废票"
            onclick="btnNotAgree_Click" Visible="False" />--%>
        <asp:Button class="btn class1" runat="server" ID="btnReleaseLockAndBack" Text="解锁并返回"
            OnClick="btnReleaseLockAndBack_Click" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <div class="layers" id="test_layer" style="display: none">
        <div>
            <h2>
                拒绝原因：</h2>
            <asp:TextBox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px"
                Height="60px" />
        </div>
        <div class="btns">
            <asp:Button Text="拒绝退/废票" ID="btnDenyRefund1" runat="server" CssClass="btn class1"
                OnClientClick="return CheckReason()" OnClick="btnDenyRefund_Click" />
            <input type="button" value="返回" class="btn class2" id="btnGoBack" onclick="CancleInput()" />
        </div>
    </div>
    <a id="aUploadAttachmentLayer" style="display: none;" data="{type:'pop',id:'divUploadAttachmentLayer'}"></a>
    <div class="form layer3 hidden" id="divUploadAttachmentLayer">
        <h4>上传附件<a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">上传退票附件</p>
            <asp:FileUpload  CssClass="text"  runat="server" ID="fileAttachment"/><br />
            <label class="obvious1">附件类型支持jpg\png\bmp三种类型，单个附件小于600kb</label>
        </div>
        <div class="txt-c mar">
            <asp:Button runat="server" ID="btnConfirm" Text="确定" CssClass="btn class1" onclick="btnConfirm_Click" />
            <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
        </div>
    </div>
    <div class="form layer2 hidden" id="divLayerImage">
        <h4>
            原图<a href="javascript:void(0);" class="close">关闭</a></h4>
        <img  style="max-height:500px; max-width:500px"/>
    </div>
    <div class="fixed">
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Operate/UploadApplyAttachment.js" type="text/javascript"></script>
<script type="text/javascript">
    var passengerCount = <%=PassengerCount %>;
</script>
<script src="/Scripts/OrderModule/Operate/ProcessRefund.aspx.js?20121222" type="text/javascript"></script>