<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Supply.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.Supply" %>
<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<%@ Register Src="~/OrderModule/UserControls/PNRInfo.ascx" TagName="PNR" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
 </head>
   <style type="text/css">
        .layer3Btns
        {
           width:70px;   
        }
        #newPrice
        {
            text-align:right;
            padding-right:3px;
            margin-right:5px;
        }
        #PriceInput
        {
            width:250px;
        }
         .blink
        {
            font-weight:900;
            font-size:16px;
            color:#FE6104;
        }
        .provision
        {
            display:none;    
        }
        .policyInfoBox table td.AUTH
        {
            text-align: left;
            background-color: white;
        }
       .btn
        {
            position:static;
        }
        #PolicyContainers .table th, .table td
        {
            padding:2px;
        }
        .choseDetail
        {
            background-color: #FFF3EB;
            line-height: 2em;
            font-size: 12px;
            text-align: center;
        }
        .code
        {
            color: #FF6600;
        }
        #txtYBPrice
        {
            border:1px solid #FF6600;
            width:40px;
            color:#FF6600
        }
    </style>
<body>
     <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
   <form id="form1" runat="server">
    <%--订单头部信息--%>
    <div runat="server" id="divHeader" class="column" visible="false">
        <h3 class="titleBg">订单详情</h3>
        <div class="column form">
            <table>
                <colgroup>
                    <col class="10" />
                    <col class="20" />
                    <col class="10" />
                    <col class="20" />
                    <col class="10" />
                    <col class="20" />
                </colgroup>
                <tbody>
                    <tr>
                        <td class="title">订单编号：</td>
                        <td><asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label></td>
                        <td class="title">订单状态：</td>
                        <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
                        <td class="title">创建时间：</td>
                        <td><asp:Label runat="server" ID="lblProducedTime" CssClass="obvious"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="title">特殊产品类型：</td>
                        <td colspan="5"><asp:Label runat="server" ID="lblSpecialPolicyType" CssClass="obvious"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <%--编码组信息--%>
    <uc:PNR runat="server" ID="pnrGroups" Visible="false" />
    <%--政策备注--%>
    <div runat="server" id="divPolicyRemark" visible="false" class="column table">
        <h3 class="titleBg">政策备注</h3>
        <div runat="server" ID="divPolicyRemarkContent"></div>
    </div>
    <%--订座参考代码--%>
    <div runat="server" id="divHKCode" class="column table" visible="false">
        <div class="hd"><h2>HK代码</h2><div class="fl minor">提示：以下信息仅供参考，一切以实际情况为准。</div></div>
        <asp:TextBox runat="server" ID="txtHKCode" CssClass="text" TextMode="MultiLine" Rows="6" Columns="50" ReadOnly="true"></asp:TextBox><br />
        <input class="btns btn class1" type="button" value="复&nbsp;&nbsp;&nbsp;制" onclick="copyToClipboard($('#txtHKCode').val())"/>
    </div>
    <%--编码信息--%>
    <div runat="server" id="divPNRCode" class="column table" visible="false">
        <h3 class="titleBg">编码信息</h3>
        <div class="box" style="padding-left:150px;background:#ccc;">
            <div id="xiaobianma">
                请在此处输入<strong class="blink">小编码</strong>：<asp:TextBox runat="server" ID="txtPNRCode" CssClass="text"></asp:TextBox>  <%--<strong><a href="javascript:;" class="obvious" id='shurudabina'>若您没有小编码，请点击<strong style="text-decoration: underline">这里输入大编码</strong></a></strong>--%>
            </div>
            <div style="display:none;" class="bianmaTip" id="dabianma">
                请在此处输入<strong class="blink">大编码</strong>：<asp:TextBox runat="server" ID="txtBPNRCode" CssClass="text"></asp:TextBox>
                <strong> <a href="javascript:;" class="obvious" id='toxiaobianma'>若您没有大编码，请点击<strong style="text-decoration: underline">这里输入小编码</strong></a></strong>
            </div> 
        </div>
    </div>
    <%--底部操作按钮--%>
    <div class="btns">
        <input type="button" runat="server" visible="false" class="btn class1" id="btnSupply" value="提供座位" onclick="supplyResource()"/>
        <input type="button" runat="server" visible="false" class="btn class1" id="btnReviseReleasedFare" value="修改价格" onclick="reviseReleasedFare();" />
        <input type="button" class="btn class1" id="btnCommitReviseReleasedFare" style="display:none" value="提&nbsp;&nbsp;&nbsp;交" onclick="commitReviseReleasedFare();"/>
        <input class="btn class2" id="btnCancelReviseReleasedFare" onclick="cancelReviseReleasedFare();"
            style="display: none" type="button" value="取&nbsp;&nbsp;&nbsp;消" />
        <input type="button" runat="server" visible="false" class="btn class1" id="btnDeny" value="拒绝提供" onclick="denySupply();" />
        <asp:Button runat="server"  class="btn class2" Text="解锁并返回" ID="btnReleaseLockAndBack" onclick="btnReleaseLockAndBack_Click"/>
        <input type="button" runat="server" class="btn class2" id="btnBack" value="返&nbsp;&nbsp;&nbsp;回" />
        
        <%--<input type="button" value='testPop' onclick="$('#chooseProviderPop').click()" />--%>
    </div>
    <%--拒绝提供座位原因--%>
    <a id="denySupplyInfo1" style="display:none" data="{type:'pop',id:'divDenySupply'}"></a>
    <div id="divDenySupply" class="form layer" style="display:none;">
        <h2>拒绝提供座位</h2>
        <table>
            <colgroup><col class="w20" /><col class="w80" /></colgroup>
            <tr>
                <td class="title">类型</td>
                <td>
                    <div class="check">
                        <input type="radio" id="reasonType1" value='<%=((int)SystemDictionaryType.RefuseSupplySelfReason).ToString() %>' name="radioone" /><label for="reasonType1">自身原因</label>
                        <input type="radio" id="reasonType2" value='<%=((int)SystemDictionaryType.RefuseSupplyPlatformReason).ToString() %>' name="radioone" /><label for="reasonType2">平台原因</label>
                        <input type="radio" id="reasonType3" value='<%=((int)SystemDictionaryType.RefuseSupplyPurchaseReason).ToString() %>' name="radioone" /><label for="reasonType3">采购原因</label>
                        <input type="radio" id="reasonType4" value='<%=((int)SystemDictionaryType.RefuseSupplyOtherReason).ToString() %>' name="radioone" /><label for="reasonType4">其他原因</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">原因</td>
                <td>
                    <select id="selDenyReason"></select>                
                </td>
            </tr>
            <tr>
                <td class="title">描述详情</td>
                <td>
                    <textarea id="txtDenyReason" rows="5" cols="50" class="text null"></textarea>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <input class="btn class1" type="button" id="btnCommitDeny" value="提&nbsp;&nbsp;&nbsp;交" onclick="commitDenySupply();"/>
                    <input class="btn class2 close" type="button" id="btnCancelDeny" value="返&nbsp;&nbsp;&nbsp;回" onclick="cancelDenySupply();"/>
                </td>
            </tr>
        </table>
    </div>
    <%--价格错误提示信息--%>
    <a id="priceErrorInfo" style="display:none" data="{type:'pop',id:'divPriceError'}"></a>
    <a id="showPop" style="display:none" data="{type:'pop',id:'Tips'}"></a>
    <div id="divPriceError" class="form layer" style="display:none"></div>
    <%--编码授权提示信息--%>
    <div class="layer3" id="Tips" style="display:none">
		<h4>
			授权提醒：
			<a href="#" class="close">关闭</a>
		</h4>
		<p class="layer3Tips">
			提示：因您未及时对OFFICE号进行授权而造成的NO位或拒单，平台不承担责任。
		</p>
		<div class="layer3Content">
			
			<label for="">你需要对编码进行授权，授权指令为：</label> 
			<p class="code">
				RMK TJ AUTH <span id="pnr"></span>
				<a href="javascript:copyToClipboard('RMK TJ AUTH '+$('#pnr').text())">复制</a>
			</p>
		</div>
		
		
		<div class="layer3Btns">
			<a class="layerbtn btn1 fl close" id="btnSure" href="#">确定</a>
		</div>
	</div>
    
    <%--  输入票价面  --%>
    <a id="showPriceInputPop" style="display:none" data="{type:'pop',id:'PriceInputPop'}"></a>
    <div class="layer4" id="PriceInputPop" style="display:none">
        <h4>
            确认票面价
            <a href="#" class="close">关闭</a>
        </h4>
        <div class="layer4Tip">
            <p class="layerico layerfull">
                <span class="curr1" id="errorInfo">无法获取编码价格信息</span>
            </p>
            <span style="line-height:1.5; padding-left:45px;">
                你需要手动输入航程票面价
            </span>
            <div class="layer4Btns" id="PriceInput">
              <div class="fl"><input type="text" id="newPrice" class="text"/></div> 
              <div class="fl"> <a class="layerbtn btn1 fl" href="javascript:SupplyPNR()">确定</a></div>
            </div>
        </div>
    </div>
    
        <%--选择出票方--%>
    <a id="chooseProviderPop" style="display: none;" data="{type:'pop',id:'ChooseProvider'}">
    </a>
    <div class="chooseProvider" id="ChooseProvider" style="display: none;">
        <h4>
            选择供应商<a class="close" href="javascript:void(0);">关闭</a></h4>
        <div class="info clearfix">
        </div>
        <div class="policyInfoBox table" id="PolicyContainers">
            <table>
                <tbody>
                    <tr>
                        <th class="w8">
                            票面价
                        </th>
                        <th class="w20">
                            政策返点
                        </th>
                        <th class="w15">
                            返点金额(元)
                        </th>
                        <th class="w12">
                            工作时间
                        </th>
                        <th class="w12">
                            出票速度
                        </th>
                        <th class="w15">
                            政策类型
                        </th>
                        <th class="w10">
                            政策选择
                        </th>
                    </tr>
                </tbody>
            </table>
            
        </div>
        <ul class="choseDetail">
                <li>您所提供的编码为:<asp:Label Text="JD6FMH" ID="lbPNR" runat="server" />   舱位为：<asp:Label Text="" ID="lbBunk" runat="server" />舱</li>  
            
                <li><span class="red">*</span>票面价为: 
                    <asp:TextBox runat="server" id="txtYBPrice" /> 元    </li>
                <li>订单的服务费为：<span id='spServiceFee'></span>元 你所选择的供应商返佣：<span id='spCommissionFee'></span>元</li>
                <li>订单概况：<span id='spProfit'>盈利</span>元（未扣除交易手续费）</li>

            </ul>
        <div class="btns">
            <input type="button" class="btn class1" id="btnChageProvider" onclick="SupplyPNR()" value="确认并继续" />
                &nbsp;
            <input type="button" class="btn class2 close" onclick="denySupply()" value="取消并拒绝提供座位出票" />
                
        </div>
    </div>

    
    

    <asp:HiddenField runat="server" ID="hidProcessType" />
    <asp:HiddenField runat="server" ID="returnUrl"/>
    <asp:HiddenField runat="server" ID="hidIsFree" />
    <asp:HiddenField runat="server" ID="hidIsThirdRelation" />
    <asp:HiddenField runat="server" ID="JsParameter" />
       
    </form>
    <script type="text/x-jquery-tmpl" id="nomalPolicyTmpl">

        <table>
                <colgroup>
                    <col class="w8">
                    <col class="w20">
                    <col class="w15">
                    <col class="w12">
                    <col class="w12">
                    <col class="w15">
                    <col class="w10">
                </colgroup>
                <tbody><tr>
                    <td class='Fare'>${Fare}</td>
                    <td>${Rebate} <input type='hidden' value='${dRebate}' />
                    {{html $item.TranslateRelationName(RelationType)}}</td>
                    <td>${Commission}</td>
                    <td>${WorkingTime}</td>
                    <td style='text-align:right'>${ETDZEfficiency}<span style='text-align: left' class="{{if IsBusy}}busy{{else}}free{{/if}}">{{if IsBusy}}忙{{else}}闲{{/if}}</span></td>
                    <td>${TicketType}</td>
                    <td>
                    <input type="radio" onclick="chooseNormalPolicy(this,'${PolicyId}','${PolicyOwner}','${PolicyType}','${OfficeNo}',${SettleAmount},${dRebate},${NeedAUTH},'${Condition}',${HasSubsidized})" name="policies" />
                    </td>
                </tr>
                <tr class="provision">
                    <td class="AUTH" colspan="7">
                          {{if NeedAUTH}}  <span class='obvious2 fl' style='display:block'>需授权</span> 您选择的供应商需要对其OFFICE号进行授权，请在黑屏内执行以下指令 <span class="code">RMK TJ AUTH ${OfficeNo}</span> <a href="javascript:copyToClipboard('RMK TJ AUTH ${OfficeNo}')">复制</a>{{else}}
                             <span class='obvious1 fl' style='display:block'>无需授权</span>
                          {{/if}}
                    </td>
                </tr>
            </tbody></table>
    </script>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../../Scripts/widget/template.js" type="text/javascript"></script>
<script src="../../Scripts/widget/form-ui.js" type="text/javascript"></script>
<script type="text/javascript">
    function Supply()
    {
        var pnr = $.trim($("#txtPNRCode").val());
        if (pnr == "") {
            alert("请先输入PNR！");
            return;
        }
        $('#showPop').click();
    }

    function getDictionaryItems(typeId, callback) {
        sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
    }

    $(function ()
    {
        $("#divDenySupply input[type='radio']").click(function ()
        {
            $("#selDenyReason").empty();
            getDictionaryItems($(this).val(), function (rsp)
            {
                if (rsp)
                {
                    for (var i in rsp)
                    {
                        $("<option>" + rsp[i].Remark + "</option>").appendTo("#selDenyReason");
                    }
                    $("#dl_selDenyReason").remove();
                    $("#selDenyReason").removeClass("custed").custSelect({ width: 326 });
                }
            });
        });
        $("#shurudabina").click(function ()
        {
            $("#xiaobianma,#dabianma").toggle();
        });
        $("#toxiaobianma").click(function ()
        {
            $("#xiaobianma,#dabianma").toggle();
        });
        $("#dl_selDenyReason li").live("click", function ()
        {
            $("#txtDenyReason").val($(this).find("span").text()).removeClass("null");
        });

        $("#txtYBPrice").keyup(function ()
        {
            var inputVal = $(this).val();
            if (inputVal == null)
            {
                return false;
            }
            if (!/\d(\.\d)?/.test(inputVal))
            {
                return false;
            }
            FrashPrice(parseFloat(inputVal), commission);
            CalculateFee(parseFloat(inputVal), commission);
        });
    });
    function chooseNormalPolicy(sender,PolicyId, PolicyOwner, PolicyType, OfficeNo, SettleAmount, Commission, NeedAUTH, Condition)
    {
        $("#PolicyContainers .provision").hide();
        $(sender).parents("table").find(".provision").show();
        selectedPolicyId = PolicyId;
        commission = Commission;
        var fare = $.trim($("txtYBPrice").text());
        if (fare=="") {
            fare = parseFloat($("#passengers .releasedFare").first().text());
        }
        CalculateFee(parseFloat(fare), Commission);
        $("#txtYBPrice").keyup();
    }

    function FrashPrice(price) {
        $(".Fare").each(function ()
        {
            var that = $(this);
            that.text(price).next().next().text(Round(price * parseFloat(that.next().find("input").val()), 2));
        });
    }

    var PolicyId = '<%=PolicyId %>';
</script>
<script src="../../Scripts/OrderModule/supply.js?2013030201" type="text/javascript"></script>

