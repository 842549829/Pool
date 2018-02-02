<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessRefund.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ProcessRefund" %>
<%@ Register Src="~/UserControl/OutPutImage.ascx" TagName="OutPutImage" TagPrefix="uc" %>
<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 </head>
   <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
   <link href="/Styles/core.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .faxInput, .FeeInput, .RefundFee, .RefundTotlal,.ActualRefundFee
        {
            width: 50px;
            text-align: right;
            padding-right: 5px;
        }
        .layers
        {
            width: 500px;
            height: 230px;
            left: 10%;
        }
        .ticketPrices, .flightTotal
        {
            display: none;
        }
        .RefundFee, .RefundTotlal,.ActualRefundFee
        {
            color: red;
        }
        .btn
        {
            position:static;
        }
    </style>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">
        申请单详情</h3>
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
                    编码
                </td>
                <td>
                    <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                </td>
                <td class="title">采购商</td>
                      <td colspan="3">
                    <asp:Label runat="server" ID="lblRelation" CssClass="obvious"></asp:Label>
                    <a href="#" runat="server" id="hrefPurchaseName"></a>
                      </td>
            </tr>
        </table>
    </div>
    <div class="column">

            <h3 class="titleBg">
                申请信息</h3>
        <div class="table">
            <table class="ClearAlternate">
                <tr>
                    <th>
                        提交时间
                    </th>
                    <th>
                        原因
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedReason"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!--     处理信息        -->
    <div class="column">

            <h3 class="titleBg">
                处理信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>
                        操作类型
                    </th>
                    <th>
                        提交时间
                    </th>
                    <th>
                        内容
                    </th>
                    <th>
                        操作人
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="ProcessInfo">
                    <itemtemplate>
                        <tr>
                    <td>
                       <%#Eval("Keyword") %>
                    </td>
                    <td>
                       <%#Eval("Time","{0:yyyy-MM-dd HH:mm}")%>
                    </td>
                    <td> <%#Eval("Content") %></td>
                    <td> <%#Eval("Account")%></td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <!--附件-->
    <div runat="server" id="divApplyAttachment" class="column table">
        <uc:OutPutImage runat="server" ID="ucOutPutImage" ClientIDMode="Static" />
        <script type="text/javascript">
            $(function () {
                $("#divOutPutImage img.buttonImg").click(function () {
                    var aid = $(this).attr("dataType");
                    var filePath = $(this).attr("FilePath");
                    $("#divLayerImage img").attr("src", filePath);
                    $("#a" + aid).click();
                });
            });
        </script>
    </div>
        <%--政策备注--%>
    <div runat="server" id="divPolicyRemark" visible="false" class="column table">
            <h3 class="titleBg">
                政策备注</h3>
        <div runat="server" id="divPolicyRemarkContent">
        </div>
    </div>
    <div runat="server" id="divETDZCondition" visible="False" class="column">
            <h3 class="titleBg">
                出票条件</h3>
            <div runat="server" id="divConditionContent">
            </div>
    </div>

    <div class="column table returnMoney">
            <h3 class="titleBg">
                退票信息</h3>
        <asp:Repeater runat="server" ID="RefundInfos" onitemdatabound="RefundInfos_ItemDataBound">
            <itemtemplate>
        <p>
            <span class="flagType"><%#Eval("TripType")%></span>
            <span class="flagType flagType1">第<%#Eval("Seaial") %>程</span>
            <%#Eval("Departure")%>-<%#Eval("Arrival")%> <%#Eval("Carrier")%><%#Eval("FlightNo")%> <%#Eval("TakeoffTime")%> <%#Eval("Bunk")%>
            <span style="display:inline-block;position:relative"><a class="flightEI" target="_blank" href='/Index.aspx?redirectUrl=/SystemSettingModule/Role/AirlineRetreatChangeNew.aspx?Carrier=<%#Eval("Carrier")%>'>退改签</a>
            </span>
            
                    <div class="tgq_box hidden">
        <h2>退改签规定</h2>
        <div class="tgq_bd" id="Tip">
            <%#Eval("EI") %>
        </div>
    </div>
       </p>
        <div class="clearfix">
            <p class="fl handle">
                <span><input type="radio" class="option1" name="auto<%#Container.ItemIndex %>" id="item<%#Container.ItemIndex %>"><label for="item<%#Container.ItemIndex %>">只退税费</label></span>
                <span><input type="radio" class="option2" name="auto<%#Container.ItemIndex %>" checked="checked" id="nomal<%#Container.ItemIndex %>" ><label for="nomal<%#Container.ItemIndex %>">正常退票</label></span>
                <span class="disabled">退票费率：<asp:textbox runat="server" CssClass="faxInput text" Text='<%#Eval("Rate")%>' onblur="ReCalc(this)"/>%
                        <span class="ticketPrices"><%#Eval("TicketPrice")%></span>
                        <span class="flightTotal"><%#Eval("Total")%></span></span>
                <span class="disabled"><asp:textbox runat="server" CssClass="FeeInput text" Text='<%#Eval("Fee")%>'  onblur="FillFee(this)"/>元</span>
            </p>
            <p class="fr">
                <span class="b">应退金额：<span><asp:textbox runat="server" CssClass="RefundTotlal"/></span>元</span>
                <span class="disabled">（包含税费及正常退票时给出的佣金）</span>
            </p>
        </div>
        <table class="Passengers ClearAlternate">
            <tbody>
                <tr>
                    <th>
                        姓名
                    </th>
                    <th>
                        类型
                    </th>
                    <th>
                        票号
                    </th>
                    <th>
                        票面价
                    </th>
                    <th>
                        民航基金
                    </th>
                    <th>
                        燃油
                    </th><%if (!IsSpeical)
                           { %>
                    <th>
                        返佣
                    </th><% } %>
                    <th>
                        收款金额
                    </th>
                    <th>
                        退票费率(%)
                    </th>
                    <th>
                        手续费 
                    </th>
                    <th style='display:<%#(bool)Eval("RenderServiceCharge")?"":"none;" %>'>
                        服务费
                    </th>
                    <th>
                        应退金额
                    </th>
                    <th>
                      实退金额
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="Passengers">
                    <itemtemplate>
                                        <tr>
                    <td>
                        <%#Eval("Name")%>
                    </td>
                    <td>
                        <%#Eval("PassengerType")%>
                    </td>
                    <td>
                        <%#Eval("No")%>
                    </td>
                    <td class="ticketPrice">
                       <%#Eval("TicketPrice")%>
                    </td>
                    <td>
                        <%#Eval("AirportFee")%>
                    </td>
                    <td>
                        <%#Eval("BAF")%>
                    </td><%if (!IsSpeical)
                           { %>
                    <td>
                        <%#Eval("Commission")%>
                    </td><%} %>
                    <td>
                        <%#Eval("YingShou", "{0:0.00}")%>
                    </td>
                    <td>
                        <asp:textbox runat="server" CssClass="faxInput" Text='<%#Eval("Rate") %>' ID="Reate" />
                        <asp:HiddenField runat="server" Value='<%#Eval("StrFee") %>'/>
                    </td>
                    <td>
                        <asp:textbox runat="server" CssClass="FeeInput" Text='<%#Eval("Fee") %>' ID="Fee" />
                    </td>
                    <td  style='display:<%#(bool)Eval("RenderServiceCharge")?"":"none;" %>'>
                        <%#Eval("RefundServiceCharge")%> 
                    </td>
                    <td>
                       <span> <asp:textbox runat="server" CssClass="RefundFee" /> </span>
                        <asp:HiddenField runat="server" ID="Voyage" Value='<%#Eval("AirportPair") %>'/>
                    </td>
                    <td>
                      <span><asp:TextBox runat="server" CssClass="ActualRefundFee"></asp:TextBox></span>
                    </td>
                </tr>
                    </itemtemplate>
                </asp:Repeater>
            </tbody>
        </table>


            </itemtemplate>
        </asp:Repeater>
    </div>
    
    <div class="importantBox broaden" id="MutilFlightTip" runat="server">
        <p class="imTips">
            请注意，该退票为多段退票，请仔细核对该航程的票面价及民航基金，如果错误请通知平台修改；如因未认真核对而产生的退票纠纷由您自己进行处理，平台不承担任何责任！平台退票组电话：<asp:Label runat="server" ID="lblRefunPhone"></asp:Label>
        </p>
    </div>

    <div class="column table" style="display: <%=RequireSeparatePNR?"":"none;" %>">
      
            <h3 class="titleBg">
                分离出的新编码信息</h3>
      
        <div>
            <table>
                <tr>
                    <td class="txt-l">
                        <div class="input">
                            <span class="name">小编码：</span>
                            <asp:textbox runat="server" ID="txtPNR" />
                        </div>
                    </td>
                    <td class="txt-l">
                        <div class="input">
                            <span class="name">大编码：</span>
                            <asp:textbox runat="server" ID="txtBPNR" />
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="btns">
        <asp:Button class="btn class1" runat="server" id="btnAgree" Text="同意退票" onclick="btnAgree_Click"
            OnClientClick="return CheckRate()" />
        <input type="button" class="btn class1" id="btnDeny" onclick="ShowReashoInput()"
            value="拒绝退票" />
        <asp:Button class="btn class1" runat="server" id="btnReleaseLockAndBack" Text="解锁并返回"
            onclick="btnReleaseLockAndBack_Click" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <div id="divDeny" class="form layers" style="display: none;">
        <h2>
            拒绝退票</h2>
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
                        <input type="radio" id="reasonType1" value='<%=((int)SystemDictionaryType.RefuseRefundSelfReason).ToString() %>'
                            name="radioone" /><label for="reasonType1">自身原因
                            </label>
                        <input type="radio" id="reasonType2" value='<%=((int)SystemDictionaryType.RefuseRefundPlatformReason).ToString() %>'
                            name="radioone" /><label for="reasonType2">平台原因
                            </label>
                        <input type="radio" id="reasonType3" value='<%=((int)SystemDictionaryType.RefuseRefundPurchaseReason).ToString() %>'
                            name="radioone" /><label for="reasonType3">采购原因
                            </label>
                        <input type="radio" id="reasonType4" value='<%=((int)SystemDictionaryType.RefuseRefundOtherReason).ToString() %>'
                            name="radioone" /><label for="reasonType4">其他原因
                            </label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">
                    原因
                </td>
                <td>
                    <select id="selDenyReason">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">
                    描述详情
                </td>
                <td>
                    <asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px"
                        MaxLength="500" Height="60px" />
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnReset" runat="server" CssClass="class1 btn" Text="拒绝退票" OnClientClick="return CheckReason()"
                        onclick="btnDeny_Click" />
                    <input class="btn class2 close" type="button" id="btnCancelDeny" value="取&nbsp;&nbsp;&nbsp;消"
                        onclick="CancleInput();" />
                </td>
            </tr>
        </table>
    </div>
    <div class="form layer2 hidden" id="divLayerImage">
         <h4>原图<a href="javascript:void(0);" class="close">关闭</a></h4>
        <img  style="max-height:500px; max-width:500px"/>
    </div>
    <div class="fixed">
    </div>
    <asp:HiddenField ID="hfdTradeFee" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20130305" type="text/javascript"></script>
<script src="../../Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    var RequireSeparatePNR = <%=RequireSeparatePNR?1:0 %>;
    $(function () {
        $("input").keypress(function (e) {
            if(e.keyCode==13) {
                return false;
            }
        });
    });
</script>
<script src="/Scripts/OrderModule/purchase/ProcessRefund.aspx.min.js?2013031201" type="text/javascript"></script>


<script type="text/javascript">
    var feeInputEnabled = <%=FeeInputEnabled?"true":"false" %>;
    $(function ()
        {
        $(".tgq_box").appendTo($("body"));
        LoadTipEvents();
        if(!feeInputEnabled) {
           LockFeeInput(); 
        }
    });
    function LoadTipEvents()
    {
        $(".flightEI").live("mouseenter", function ()
        {
            $(".tgq_box").removeClass("hidden");
            $(".tgq_box").css("left", $(this).offset().left - 125);
            $(".tgq_box").css("top", $(this).offset().top + 15);
            var h = $(".tgq_box").height();
            var top = $(".tgq_box").offset().top;
            var sor = $(window).scrollTop();
            var wh = $(window).height();
            if (h + top - sor > wh)
            {
                $(".tgq_box").css({ top: (top - h - 35) });
                $(".tgq_box").addClass("tgq_box1").removeClass("tgq_box");
            };
        }).live("mouseleave", function ()
        {
            $(".tgq_box1").addClass("tgq_box").removeClass("tgq_box1");
            $(".tgq_box").addClass("hidden");
        });
    }

</script>
