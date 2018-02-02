<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Apply.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Purchase.Apply" %>
<%@ Import Namespace="ChinaPay.B3B.DataTransferObject.Order" %>
<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>B3B机票平台</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <style type="text/css">
      #dl_Reasons button {
            width: 315px;   
        }
      .h {
          display: none;
      }
      .form li {
          padding: 5px 0 10px 50px;   
      }
      .form li.table {
          padding-left: 0;  
      }
      .flightNo {
          width: 50px;   
      }
      .layer3Tips
      {
          color:black;
      }
      #delegateCanclePNR
      {
          
      }
      .btn
      {
          position:static;
      }
    </style>
<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server" >
    <%--订单头部信息--%>
    <div runat="server" id="divHeader" class="column form">
    <h3 class="titleBg">申请退改签</h3>
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
                <td class="title">订单编号</td>
                <td><asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></td>
                <td class="title">订单状态</td>
                <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
                <td class="title">订单金额</td>
                <td><asp:Label runat="server" ID="lblAmount" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">产品类型</td>
                <td><asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label></td>
                <td class="title">客票类型</td>
                <td><asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label></td>
                <td class="title">原订单号</td>
                <td><asp:Label runat="server" ID="lblOriginalOrderId" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">创建时间</td>
                <td><asp:Label runat="server" ID="lblProducedTime" CssClass="obvious"></asp:Label></td>
                <td class="title">支付时间</td>
                <td><asp:Label runat="server" ID="lblPayTime" CssClass="obvious"></asp:Label></td>
                <td class="title">出票时间</td>
                <td><asp:Label runat="server" ID="lblETDZTime" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">工作时间</td>
                <td><asp:Label runat="server" ID="lblWorkingTime" CssClass="obvious"></asp:Label>
                            <asp:HiddenField runat="server" ID="hdProvider"/></td>
                <td class="title">废票时间</td>
                <td colspan="3"><asp:Label runat="server" ID="lblScrapTime" CssClass="obvious"></asp:Label></td>
            </tr>
        </table>
    </div>
    <%--编码组信息--%>
    <div runat="server" id="divPNRGroups"></div>
    <!-- 申请退/废票  -->
    <a id="applyRefundInfo" style="display:none;" data="{type:'pop',id:'divRefund'}"></a>
    <div class="column layer" id="divRefund" style="display:none;">
        <div class="tips-info tips-info-a">
            <div class="hd"><h3>申请退/废票</h3></div>
            <div class="con">
                <p class="layer3Tips h" id="pSpecial">请注意，应供应商要求，该票若发生废票规定需要收取<span runat="server" id="lblProviderRate"></span> 元废票手续费，敬请悉知！（如果您是编码导入订票，请在提交废票前将您的座位取消并将行程单作废）</p>
                <p class="layer3Tips h" id="pNotSpecial">请注意，该票为特殊票，应供应商要求，该票若发生废票规定需要收取<span runat="server" id="lblResourcerRate"></span> 元废票手续费，敬请悉知！（如果您是编码导入订票，请在提交废票前将您的座位取消并将行程单作废）</p>
                <p class="layer3Tips h" id="BothDisable">请注意，为您出票的供应商废票时间为:<span id="ScrapTime1"></span>; 您已无法提交废票；供应商的退票时间为：<span id="RefundTime1"></span> ;您现在提交将在次日进行处理；敬请知悉。</p>
                 <p class="layer3Tips h" id="RefundEnabled">请注意，为您出票的供应商废票时间为:<span id="ScrapTime2"></span>; 您仅能进行退票申请；供应商的退票时间为：<span id="RefundTime2"></span> ;您仅能进行退票申请</p>
                 <p class="layer3Tips h" id="RefundDisabled">请注意，为您出票的供应商退票时间为:<span id="RefundTime3"></span>;您现在提交将在次日进行处理;如有疑问请致电<asp:Label runat="server" ID="lblServicePhone"></asp:Label></p>
                
                

                <ul class="form">
                    <li><span class="name">乘机人：</span><span id="refundPassengerNames" class="obvious-a"></span></li>
                    <li><span class="name">航&nbsp;&nbsp;&nbsp;段：</span><span id="refundVoyages" class="obvious-a"></span></li>
                    <li>
                        <span class="name">类&nbsp;&nbsp;&nbsp;型：</span>
                        <div id="divRefundType">
                            <input type="radio" id="reasonType1" value="-1" key='<%=((int)SystemDictionaryType.IntradayScrap).ToString() %>' name="refundGroup"/><label for="reasonType1">当日作废</label>
                            <input type="radio" id="reasonType2" value="<%=((int)RefundType.Upgrade).ToString() %>" key='<%=((int)SystemDictionaryType.UpgradeAllRefund).ToString() %>' name="refundGroup"/><label for="reasonType2">升舱全退</label>
                            <input type="radio" id="reasonType3" value="<%=((int)RefundType.Voluntary).ToString() %>" key='<%=((int)SystemDictionaryType.SelfImposedRefund).ToString() %>' name="refundGroup"/><label for="reasonType3">自愿按客规退票</label>
                            <input type="radio" id="reasonType4" value="<%=((int)RefundType.Involuntary).ToString() %>" key='<%=((int)SystemDictionaryType.InvoluntaryRefund).ToString() %>' name="refundGroup"/><label for="reasonType4">非自愿退票</label>
                            <input type="radio" id="reasonType5" value="<%=((int)RefundType.SpecialReason).ToString() %>" key='<%=((int)SystemDictionaryType.SpeicalReasonRefund).ToString() %>' name="refundGroup"/><label for="reasonType5">特殊原因退票</label>
                        </div>
                    </li>
                    <li style="z-index:10;">
                        <span class="name">原&nbsp;&nbsp;&nbsp;因：</span>
                        <select id="Reasons" style="width:326px"></select>
                    </li>
                    <li style="z-index:1;">
                        <span class="name">描&nbsp;&nbsp;&nbsp;述：</span>
                        <asp:TextBox runat="server" ID="txtRefundReason" CssClass="text" Columns="50" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </li>
                    <li id="delegateCanclePNR">
                         <input type='checkbox' id="chkdelegateCanclePNR" style="display:none;"/>
                         <label for="chkdelegateCanclePNR">委托平台取消座位(无法取消被分离后的编码)</label>   
                    </li>
                    <li id="liAttachment" class="hidden">
                        <span class="name">附&nbsp;&nbsp;&nbsp;件：</span>
                        <iframe id="iframeChildren" width="0" height="0" src="Upload.aspx"></iframe>
                        <input type="text" class="text width" readonly="readonly" id="txtAttachment" />
                        <input type="button" value="浏览" class="btn class2 width-s" id="btnAttachment" /><br /><br />
                        <label class="obvious1">附件类型支持jpg\png\bmp三种类型，附件小于600kb</label>
                    </li>
                </ul>
                <div class="btns">
                    <input type="button" class="btn class1" onclick="CommitApplyRefund(this);" value="提&nbsp;&nbsp;&nbsp;交" id="submitApply" />
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回"/>
                </div>
            </div>
        </div>
    </div>
    <!-- 申请改期 -->
    <a id="applyPostponeInfo" style="display:none;" data="{type:'pop',id:'divPostpone'}"></a>
    <div class="column layer layer2" id="divPostpone" style="display:none;">
        <div class="tips-info tips-info-a">
            <div class="hd"><h3>申请改期</h3></div>
            <div class="con">
                <ul class="form">
                    <li><span class="name">乘机人：</span><span id="postponePassengerNames" class="obvious-a"></span></li>
                    <li><span class="name">航空公司：</span><span id="postponeCarrier" class="obvious-a"></span></li>
                    <li class="table"><table></table></li>
                    <li>
                        <span class="name">备&nbsp;&nbsp;&nbsp;注：</span>
                        <asp:TextBox runat="server" ID="txtPostponeRemark" CssClass="text" Columns="50" Rows="5" TextMode="MultiLine" style="width:98%"></asp:TextBox>
                    </li>
                </ul>
                <div class="btns">
                    <input type="button" class="btn class1" onclick="CommitApplyPostpone(this);" value="提&nbsp;&nbsp;&nbsp;交"/>
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回"/>
                </div>
            </div>
        </div>
    </div>
    <!-- 申请升舱  -->
    <a id="applyUpgradeInfo" style="display:none" data="{type:'pop',id:'divUpgrade'}"></a>
    <div class="column layer" id="divUpgrade" style="display:none;">
        <div class="tips-info tips-info-a">
            <div class="hd"><h3>申请升舱</h3></div>
            <div class="con">
                <ul class="form">
                    <li><span class="name">乘机人：</span><span id="upgradePassengerNames" class="obvious-a"></span></li>
                    <li><span class="name">航&nbsp;&nbsp;&nbsp;段：</span><span id="upgradeVoyages" class="obvious-a"></span></li>
                    <li>
                        <div class="check">
                            <input type="radio" value="5x" name="radioone" id="inputCode" checked="checked" /><label for="inputCode">高舱位编码</label>
                            <input type="radio" value="6x" name="radioone" id="NoandReserve" /><label for="NoandReserve">无舱位编码，立即预订</label>
                        </div>
                    </li>
                    <li style="text-align:center" id="section1" style="display:none;">编码：<asp:textbox runat="server" ID="txtPNR" /></li>
                    <li id="section2" style="display:none;">
                        <table>
                            <tr>
                                <th>航段</th>
                                <th>原航班日期</th>
                                <th>原舱位</th>
                                <th>新航班日期</th>
                      </tr>
                      <tbody  id="tbUpgradeAirLines">
                          
                      </tbody>
                        </table>

                    </li>
                </ul>
                <div class="btns">
                    <input type="button" class="btn class1" onclick="CommitApplyUpgrade(this);" value="提&nbsp;&nbsp;&nbsp;交" id="btnSavePNR"/>
                    <asp:Button type="button" CssClass="btn class1" Text="查询航班" id="btnSearchAireLine" style="display:none;" runat="server" OnClick="SearchAirLine" OnClientClick="return CheckDate()"/>
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回"/>
                </div>
            </div>
        </div>
    </div>
     <div class="form layer2 hidden" id="divLayerImage">
         <h4>原图<a class="close">关闭</a></h4>
        <img style="width:500px; height:500px;" />
    </div>
    <%--加载中图片--%>
       <a id="LoadingPop" data="{type:'pop',id:'Loading'}" style="display:none;"></a>
       <div id="Loading" style="display:none">
           <img src="/Images/progress.gif" alt="Loading">
           <span id="loadingClose" class="close" style="display: none">关闭</span>
       </div>

    <a id="CheckProcessPop" data="{type:'pop',id:'Result'}" style="display:none;"></a>
    	<div class="testing-box" id="Result" style="display:none;">
		<h4 class="proceeding" id="tipProceding">正在对您提交的退票申请信息进行验证</h4>
		<h4 class="succeed" id="tipSuccess">恭喜您！退票申请提交成功。</h4>
		<h4 class="warning" id="tipFail">抱歉！退票申请提交失败，请核实以下条件。</h4>
		<ul>
			<li id="tipItem1">申请退票的乘机人一致性验证。</li>
			<li id="tipItem2">申请退票的乘机人机票未使用。</li>
			<li id="tipItem3">申请退票的乘机人座位已取消。</li>
			<li id="tipItem4">作废申请退票的乘机人行程单。</li>
		</ul>
		<p>请将未验证通过的条件处理后重新提交退票申请，若有疑问，请致电平台客服中心。</p>
		<div class="btns">
			<button type="button" class="class1 btn close">返回</button>
		</div>
	</div>
    

    <input type="hidden" id="hidApplyPNR" runat="server" />
    <input type="hidden" id="hidApplyPassengers" runat="server" />
    <input type="hidden" id="hidApplyFlights" runat="server"/>
    <input type="hidden" id="hidApplyType" runat="server" />
    <input type="hidden" id="hidWorkTimeSet" runat="server" />
    <asp:HiddenField runat="server" ID="JsParameter" />
    <asp:HiddenField runat="server" ID="hfdServicePhone" />
    <%--底部操作按钮--%>
    <div class="btns">
        <asp:Button runat="server" ID="btnReleaseLockAndBack" CssClass="btn class2" Visible="false" Text="解锁并返回" OnClick="btnReleaseLockAndBack_Click"/>
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div></form>
</body>
</html>
<script type="text/javascript">
    eval($("#hidWorkTimeSet").val());
    IsImport = <%=IsImport?"true":"false" %>;
</script>


<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?201304" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121126" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/apply.js?version=20130419003" type="text/javascript"></script>

<script type="text/javascript">
    function ShowApplyFormBody(show)
    {
        if (show) {
            $("#divRefund").show();
        } else {
            $("#divRefund").hide();
        }
    };
    function ClearProp() {
        $(".close").click();
    }
    
</script>