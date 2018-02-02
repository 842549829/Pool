<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="OrderDetail.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.OrderDetail" %>

<%@ Reference Control="~/OrderModule/UserControls/PNRInfo.ascx" %>
<%@ Register Src="~/OrderModule/UserControls/OrderBill.ascx" TagName="Bill" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B机票平台</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="/Styles/page.css" rel="stylesheet" type="text/css" />

<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">
        订单详情</h3>
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
                    采购方
                </td>
                <td>
                    <a runat="server" id="linkPurchaser" class="obvious-a"></a>
                </td>
                <td class="title">
                    出票方
                </td>
                <td>
                    <a runat="server" id="linkProvider" class="obvious-a"></a>&nbsp;&nbsp;&nbsp; <a runat="server"
                        id="linkPrividerPolicy" class="obvious-a" target="_blank">政策详细</a>
                </td>
                <td class="title">
                    资源方
                </td>
                <td>
                    <a runat="server" id="linkSupplier" class="obvious-a"></a>&nbsp;&nbsp;&nbsp; <a runat="server"
                        id="linkSupplierPolicy" class="obvious-a" visible="false" target="_blank">政策详细</a>
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
        </table>
        <div runat="server" id="divFailed" visible="false">
            拒绝出票：<asp:Label runat="server" ID="lblFailedReason"></asp:Label></div>
    </div>
    <div class="form" runat="server" id="divOemInfo">
        <div class="clearfix">
            <h4 class="drop-header" id="onDropHeader">
                展开OEM信息</h4>
        </div>
        <div class="drop-content hidden">
            <p class="type">
                OEM信息</p>
            <table>
                <tr>
                    <td class="title">
                        用户名：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemCompanyUserName"></asp:Literal>
                    </td>
                    <td class="title">
                        公司类型：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyTypeValue" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        授权时间：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemAuthorizationTime"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        公司名称：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemCompayName"></asp:Literal>
                    </td>
                    <td class="title">
                        简称：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemCompanyAbbreviation"></asp:Literal>
                    </td>
                    <td class="title">
                        OEM名称：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemName"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        公司电话：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemCompayPhone"></asp:Literal>
                    </td>
                    <td class="title">
                        组织机构代码证：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemCompanyOrginationCode"></asp:Literal>
                    </td>
                    <td class="title">
                        授权域名：
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="lblOemDomainName"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        负责人：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyManagerName" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        手机：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyManagerCellPhone" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        授权状态：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemValid" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        所在地：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyLocation" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        地址：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyAddress" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        授权到期：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemEffectTime" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        附件：
                    </td>
                    <td>
                        <a href="#" runat="server" id="lbtnBussinessLicense" target="_blank">营业执照</a>&nbsp;&nbsp;
                        <a href="#" runat="server" id="lbtncertNo" target="_blank" visible="false">身份证</a>&nbsp;&nbsp;
                        <a href="#" runat="server" id="lbtnIATA" target="_blank">IATA</a>
                    </td>
                    <td class="title">
                        使用期限：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemEffectTimeStrat" runat="server"></asp:Literal>
                        <asp:Literal ID="lblOemEffectTimeEnd" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        开户时间：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyRegisterTime" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                        审核时间：
                    </td>
                    <td>
                        <asp:Literal ID="lblOemCompanyAuditTime" runat="server"></asp:Literal>
                    </td>
                    <td class="title">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                $(function () {
                    if ($("#divOemInfo").length > 0) {
                        $("#onDropHeader").toggle(function () {
                            $(this).html("收起OEM信息").parent().next().slideToggle("slow");
                        }, function () {
                            $(this).html("展开OEM信息").parent().next().slideToggle("slow");
                        });
                    } else {
                        $("#onDropHeader").parent().addClass("hidden").next().removeClass("hidden").before('<h3 class="titleBg">采购方</h3>').find("p").addClass("hidden");
                    }
                });
            </script>
        </div>
    </div>
    <%--编码组信息--%>
    <div runat="server" id="divPNRGroups">
    </div>
    <%--账单信息--%>
    <uc:Bill runat="server" ID="bill" />
    <asp:HiddenField runat="server" ID="hidReturnUrl" />
    <asp:HiddenField runat="server" ID="hidOriginalPolicyOwner" />
    <asp:HiddenField runat="server" ID="hidPolicyType" />
    <asp:HiddenField runat="server" ID="OriginalPolicyIsSpecial" />
    <%--换出票方--%>
    <a id="changeProviderPop" style="display: none;" data="{type:'pop',id:'ChangeProvider'}">
    </a>
    <div class="chooseProvider" id="ChangeProvider" style="display: none;">
        <h4>
            换供应商出票<a id="btnCacelLock" class="close" href="javascript:void(0);">关闭</a></h4>
        <div class="info clearfix">
            <div class="fl">
                <p>
                    航班信息：<asp:Label Text="FM9456 2012-11-29 15:35 KMG-XIY Y/78.9" ID="lblFlightInfo"
                        runat="server" /></p>
                <p>
                    乘机人信息：<asp:Label Text="李**/成人/530120198511020513/1080.00/50.00/130.00/<span class='price'>1260.00</span>"
                        ID="lblPassenger" runat="server" /></p>
                <p>
                    原出票方：<a target="_blank" href="/Index.aspx?redirectUrl=/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=<%=ProviderID %>"><span
                        class="obvious-a"><asp:Label Text="master（博宇票务）" ID="lblProvider" runat="server" />
                    </span></a><span class="obvious">
                        <asp:Label Text="返点9%（113.40元）" ID="lblCommition" runat="server" /></span></p>
                <p>
                    购买方：<a target="_blank" href="/Index.aspx?redirectUrl=/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=<%=PurshareID %>"><span
                        class="obvious-a"><asp:Label Text="master（博宇票务）" ID="lblPurchase" runat="server" />
                    </span></a><span class="obvious">
                        <asp:Label Text="返点9%（113.40元）" ID="lblPurchaseCommition" runat="server" /></span>
                    <asp:Label Text="备注：采购要求不允许换编码出票" ID="lbRemark" runat="server" Visible="false" />
                </p>
            </div>
            <div class="fr">
                <a class="refresh" href="javascript:RefrashPolicy()">刷新政策</a>
                <div>
                    <asp:CheckBox Text="隐藏需要换编码出票的政策" runat="server" ID="cbForbidChangePNR" /></div>
            </div>
        </div>
        <div class="policyInfoBox table" id="PolicyContainers">
            <table>
                <tbody>
                    <tr>
                        <th class="w5">
                        </th>
                        <th class="w8">
                            票面价
                        </th>
                        <th class="w20">
                            政策返点
                        </th>
                        <th class="w10">
                            单张结算价
                        </th>
                        <th class="w12">
                            工作时间
                        </th>
                        <th class="w12">
                            废票时间
                        </th>
                        <th class="w10">
                            出票效率
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
        <div class="btns">
            <button type="button" class="btn class1" id="btnChageProvider">
                确认换供应商出票</button>
            <button type="button" class="btn class2" id="btnCacelLock1">
                取消出票</button>
        </div>
    </div>
    <div class="sup-p_box hidden">
        <div class="tips_bd">
            <p>
                该政策是您的<span id="relationTip">上级</span>发布的内部政策请根据您的实际情况做选择。</p>
        </div>
    </div>
    <a id="divOption" style="display: none" data="{type:'pop',id:'Tips'}"></a>
    <div class="layer3" id="Tips" style="display: none">
        <h4>
            授权提醒： <a href="#" class="close">关闭</a>
        </h4>
        <p class="layer3Tips">
            提醒采购再次对新的供应商进行OFFICE号授权
        </p>
        <div class="layer3Content">
            <label for="">
                需要对编码进行授权，授权指令为：</label>
            <p class="code">
                RMK TJ AUTH <span id="officeNo"></span><a href="javascript:copyToClipboard('RMK TJ AUTH '+$('#officeNo').text())">
                    复制</a>
            </p>
        </div>
        <div class="layer3Btns">
            <a class="layerbtn btn1 fl close" id="btnSure" href="#">确定</a>
        </div>
    </div>
    <asp:HiddenField ID="hfdOfficeIsAuth" runat="server" />
    <asp:HiddenField ID="hfdNeedAuthOffice" runat="server" />
    <asp:HiddenField ID="hfdAgreeAuth" runat="server" />
    <asp:HiddenField ID="hfdPnrImport" runat="server" />
    </form>
    <%--底部操作按钮--%>
    <div class="btns">
        <button class="btn class1" runat="server" visible="false" id="btnCancelOrder" onclick="cancelOrder();">
            取消订单</button>
        <button class="btn class1" runat="server" visible="false" id="btnReETDZ" onclick="reETDZ();">
            重新出票</button>
        <button class="btn class1" runat="server" visible="false" id="btnReSupply" onclick="reSupply();">
            重新提供资源</button>
        <button class="btn class1" runat="server" visible="false" id="btnChangeProvider"
            onclick="ChangeProvider()">
            重新选择出票方</button>
        <button class="btn class1" runat="server" visible="false" id="btnOrderHistory">
            订单历史记录</button>
        <button class="btn class1" runat="server" visible="false" id="btnProcessingApplyforms">
            进行中的申请</button>
        <button class="btn class1" runat="server" visible="false" id="btnUpdateTicketNo">
            修改票号</button>
        <button class="btn class1" runat="server" visible="false" id="btnUpdateCredentials">
            修改证件号</button>
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <script type="text/x-jquery-tmpl" id="nomalPolicyTmpl">
            <table class="policys">
    	            <tbody><tr>
        	        	<td class="w5"><span title="返点{{if IsHigher}}高于{{else}}低于{{/if}}原返点" class="ico {{if IsHigher}}ico_higher{{else}}ico_lower{{/if}}">低</span></td>
            	        <td class="w8">${Fare}</td>
                	    <td class="w20">${Rebate}(${Commission}) 
                        {{html $item.TranslateRelationName(RelationType)}}</td>
                    	<td class="w10">${SettleAmount}</td>
	                    <td class="w12">${WorkingTime}</td>
    	                <td class="w12">${ScrapTime}</td>
        	            <td class="w12">${ETDZEfficiency}<span class="{{if IsBusy}}busy{{else}}free{{/if}}">{{if IsBusy}}忙{{else}}闲{{/if}}</span></td>
            	        <td class="w15">${TicketType}(${PolicyTypes}) <input type='hidden' value='${setChangePNREnable}'  /></td>
                	    <td class="w10"><input type="radio" onclick="chooseNormalPolicy(this,'${PolicyId}','${PolicyOwner}','${PolicyType}','${OfficeNo}',${SettleAmount},${Commission},${NeedAUTH},'${Condition}',${RelationType})" name="policies" /></td>
	                </tr>
    	            <tr  class="provision">
        	            <td class="tips" colspan="9">
            	        	<p class="provideInfo b">供应商信息：<a target="_blank" href="/Index.aspx?redirectUrl=/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=${PolicyOwner}"><span class="obvious-a">${ProviderAccount}（${ProviderName}）</span></a>
                            {{html TipInfo}}
                            </p>
                            
                	        <p>{{html EI}}</p>
                            <p>出票条件：{{html Condition}}</p>
                    	</td>
	                </tr>
    	        </tbody></table>
    </script>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script src="../../Scripts/OrderModule/operationProcessOrder.js?20121206" type="text/javascript"></script>
<script src="../../Scripts/widget/template.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Operate/OrderDetail.aspx.js?2013051401" type="text/javascript"></script>
