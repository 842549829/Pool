<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRestrictionSettingGlobal.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.PurchaseRestrictionSettingGlobal" %>

<%@ Register Src="~/UserControl/MultipleAirport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <style type="text/css">
        #txtDepartureAirports_txtAirports
        {
            width: 300px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        采买设置</h3>
    <div class="O_formBox">
        <div class="importantBox broaden">
            <span class="obvious">提示：本页所设置的规则将对您的所有用户生效，请认真核对后再提交！</span><br />
            采买限制将控制您的用户所采购的某些航空公司和出港城市仅显示您发布的政策，若您没有发布某些航线的政策时，将显示您所设置的默认返点，<br />
            我们非常建议当您未发布相关政策允许您的下级采购其他供应商的普通或特价政策，这样有助于您提高用户的粘性。
        </div>
        <div>
            <asp:RadioButton ID="rbnPurchaseNone" runat="server" Text="不对用户做限制" GroupName="purchase"
                Checked="true" /><br />
            <asp:RadioButton ID="rbnPurchaseEach" runat="server" Text="允许各用户组分别设置采买规则" GroupName="purchase" /><br />
            <asp:RadioButton ID="rbnPurchaseGlobal" runat="server" Text="使用全局的统一采买设置" GroupName="purchase" />
        </div>
    </div>
    <div id="divGlobal" style="display: ">
        <div class="simple-box clearfix">
            <label class="title">
                限制航空公司：</label>
            <div class="simple-content check-box-list">
                <div runat="server" id="divAirlinelist">
                </div>
                <br />
                <span class="obvious">
                    <input type="radio" name="radAir" id="radAll" value="0" /><label for="radAll">全选</label>
                    <input type="radio" name="radAir" id="radNot" value="1" /><label for="radNot">反选</label></span>
            </div>
        </div>
        <div class="simple-box clearfix">
            <div class="fl" style="width: 500px;">
                <div class="clearfix">
                    <label class="title">
                        限制出港城市：</label>
                    <div class="simple-content">
                        <uc:City ID="txtDepartureAirports" runat="server" />
                    </div>
                </div>
            </div>
            <div style="margin-left: 510px;">
                <p class="obvious1">
                    温馨提示：支持多选，可用Shift连选，也支持Ctrl间隔选择。可手动输入机场三字码，如果三字码正确，城市名字会自动加到右边的已选择列表中。输入三字码时，如果有多个，请用"/"分隔，如：CTU/PEK留空则表示选择了全国出港城市
                </p>
            </div>
        </div>
        <div id="rebateInput">
            <div class="simple-box">
                <label class="title">
                    普通政策限制：</label><input type="hidden" value="0" class="policyType" />
                <div class="simple-content">
                    <asp:RadioButton ID="rbnPurchaseNormalPolicy" runat="server" Text="我未发布政策时允许下级采购平台其他供应商的普通政策"
                        GroupName="NormalPolicy" Checked="true" /><br />
                    <asp:RadioButton ID="rbnPurchaseOnlySelfNormalPolicy" runat="server" Text="即使我未发布政策也只允许采购我发布的默认返点的普通政策"
                        GroupName="NormalPolicy" />
                    <div id="PurchaseOnlySelfNormalPolicy" runat="server" class="box">
                        <p>
                            当您未发布采购所采买的航线政策时，将使用下面的默认返点，仅适用于普通政策默认返点：</p>
                        <p>
                            下级默认返点：<asp:TextBox ID="txtDefaultRebateAdultNormalPolicy" runat="server" CssClass="text"></asp:TextBox>%
                            <span class="pad-l obvious1">仅对成人类型乘客进行限制，不支持儿童政策出票</span></p>
                    </div>
                </div>
            </div>
            <div class="simple-box">
                <label class="title">
                    特价政策限制：</label><input type="hidden" value="1" class="policyType" />
                <div class="simple-content">
                    <asp:RadioButton ID="rbnPurchaseBargainPolicy" runat="server" Text="我未发布政策时允许下级采购平台其他供应商的特价政策"
                        GroupName="BargainPolicy" Checked="true" /><br />
                    <asp:RadioButton ID="rbnPurchaseOnlySelfBargainPolicy" runat="server" Text="即使我未发布政策也只允许采购我发布的默认返点的特价政策"
                        GroupName="BargainPolicy" />
                    <div id="PurchaseOnlySelfBargainPolicy" runat="server" class="box">
                        <p>
                            当您未发布采购所采买的航线政策时，将使用下面的默认返点，仅适用于特价政策默认返点：</p>
                        <p>
                            下级默认返点：<asp:TextBox ID="txtDefaultRebateAdultBargainPolicy" runat="server" CssClass="text"></asp:TextBox>%
                            <span class="pad-l obvious1">仅对成人类型乘客进行限制，不支持儿童政策出票</span></p>
                    </div>
                </div>
            </div>
           <%-- <div class="simple-box">
                <label class="title">
                    团队政策限制：</label><input type="hidden" value="2" class="policyType" />
                <div class="simple-content">
                    <asp:RadioButton ID="rbnPurchaseTeamPolicy" runat="server" Text="我未发布政策时允许下级采购平台其他供应商的团队政策"
                        GroupName="TeamPolicy" Checked="true" /><br />
                    <asp:RadioButton ID="rbnPurchaseOnlySelfTeamPolicy" runat="server" Text="即使我未发布政策也只允许采购我发布的默认返点的团队政策"
                        GroupName="TeamPolicy" />
                    <div id="PurchaseOnlySelfTeamPolicy" runat="server" class="box">
                        <p>
                            当您未发布采购所采买的航线政策时，将使用下面的默认返点，仅适用于团队政策默认返点：</p>
                        <p>
                            下级默认返点：<asp:TextBox ID="txtDefaultRebateAdultTeamPolicy" runat="server" CssClass="text"></asp:TextBox>%
                            <span class="pad-l obvious1">仅对成人类型乘客进行限制，不支持儿童政策出票</span></p>
                    </div>
                </div>
            </div>--%>
           <%-- <div class="simple-box">
                <label class="title">
                    缺口政策限制：</label><input type="hidden" value="3" class="policyType" />
                <div class="simple-content">
                    <asp:RadioButton ID="rbnPurchaseNotchPolicy" runat="server" Text="我未发布政策时允许下级采购平台其他供应商的缺口程政策"
                        GroupName="NotchPolicy" Checked="true" /><br />
                    <asp:RadioButton ID="rbnPurchaseOnlySelfNotchPolicy" runat="server" Text="即使我未发布政策也只允许采购我发布的默认返点的缺口程政策"
                        GroupName="NotchPolicy" />
                    <div id="PurchaseOnlySelfNotchPolicy" runat="server" class="box">
                        <p>
                            当您未发布采购所采买的航线政策时，将使用下面的默认返点，仅适用于缺口政策默认返点：</p>
                        <p>
                            下级默认返点：<asp:TextBox ID="txtDefaultRebateAdultNotchPolicy" runat="server" CssClass="text"></asp:TextBox>%
                            <span class="pad-l obvious1">仅对成人类型乘客进行限制，不支持儿童政策出票</span></p>
                    </div>
                </div>
            </div>--%>
            <%-- <div class="simple-box">
                <label class="title">
                    特殊政策限制：</label><input type="hidden" value="4" class="policyType" />
                <div class="simple-content">
                    <asp:RadioButton ID="rbnPurchaseSpecialPolicy" Checked="true" GroupName="SpecialPolicy"
                        runat="server" Text="我未发布政策时允许下级采购其他供应商的特殊政策" /><br />
                    <asp:RadioButton ID="rbnPurchaseOnlySelfSpecialPolicy" runat="server" GroupName="SpecialPolicy"
                        Text="即使我未发布政策也只允许采购我发布的默认的特殊政策" />
                    <br />
                    <input type="text" style="display: none" class="text" />
                </div>
            </div>--%>
        </div>
        <div class="simple-box last">
            <div class="simple-content">
                <a href="javascript:void(0);" id="addSubGroup" class="simple-add-btn">添加到下面的分组</a>
            </div>
        </div>
        <div class="accumulate-box" id="divPurchaseLimitation" runat="server">
            <h4>
                双击即可编辑您添加的分组</h4>
            <%-- <p>
                KMG/CTU/SHA/LJG/PEK/MFM/XIY(3U/MU)<br />
                普通政策：可采购同行政策<br />
                特价政策：可采购同行政策<br />
                团队政策：只显示我的默认返点政策<br />
                缺口政策：可采购同行政策<br />
                特殊政策：可采购同行政策 <a href="#" class="remove">删除</a>
            </p>
            <p>
                KMG/CTU/SHA/LJG/PEK/MFM/XIYKMG/CTU/SHA/LJG/PEK/MFM/XIYKMG/CTU/SHA/LJG/PEK/MFM/XIYKMG/CTU/SHA/LJG/PEK/MFM/XIY(3U/MU)<br />
                普通政策：可采购同行政策<br />
                特价政策：可采购同行政策<br />
                团队政策：只显示我的默认返点政策<br />
                缺口政策：可采购同行政策<br />
                特殊政策：可采购同行政策 <a href="#" class="remove">删除</a>
            </p>--%>
        </div>
    </div>
    <div class="btns">
        <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="提交" OnClick="btnSave_Click" />
    </div>
    <asp:HiddenField ID="hfdLimitation" runat="server" />
    </form>
</body>
</html>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/PurchaserRestrictionSettingGlobal.js?2013052105"></script>
<script src="../../Scripts/widget/template.js" type="text/javascript"></script>
<script type="text/x-juqery-tmpl" id="limitationTmpl">
   <p>
     ${Departures}(${Airlines})<br/>
     {{tmpl(Rebate,{ TranslateRelationName: TranslateRelationName ,RenderRate:RenderRate,TranslatePolicyTypeName:TranslatePolicyTypeName}) "#policyTypeItem"}}
      <a href="javascript:void(0);" class="remove">删除</a>
      <input type="hidden" value='${Data}' name='Data_${$item.CountAcc()}' />
   </p>

</script>
<script type="text/x-juqery-tmpl" id="policyTypeItem">
   ${$item.TranslateRelationName(Type)} {{if AllowOnlySelf}} ${$item.TranslatePolicyTypeName(Type)}  ${$item.RenderRate(Rebate)}% {{else}}可采购同行政策 {{/if}}  <br/>
</script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/PurchaseLimitation.js?20130521"></script>
<script type="text/javascript" src="../../Scripts/Global.js?2013052001"></script>
