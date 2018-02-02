<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRestrictionSetting.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.PurchaseRestrictionSetting" %>

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
        .IE7\.0 .btn.class2
        {
            padding: 3px 5px 2px;
        }
        .layer3 #data-list th, .layer3 #data-list td
        {
            min-height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        <asp:Label ID="lblGroupNameTitle" runat="server"></asp:Label>- 采买设置&nbsp;&nbsp;&nbsp;本设置将限制本组用户所能采购的政策范围及返点等，若您需要对所有用户统一设置请点此进行<a
            href="PurchaseRestrictionSettingGlobal.aspx" class="obvious-a">全局设置</a>
    </h3>
    <div class="box-a">
        <div class="clearfix">
            <div class="user-type-left">
                用户组名称：<asp:Label ID="lblGroupName" runat="server"></asp:Label></div>
            <div class="user-type-right">
                用户数量：<asp:Label ID="lblUserCount" runat="server"></asp:Label>&nbsp;&nbsp;<a href="javascript:void(0);"
                    id="queryUsrList" runat="server" class="obvious-a">查看用户列表</a> | <a href="#" id="addUser"
                        class="obvious-a">添加用户</a></div>
        </div>
        <div class="user-type">
            用户组描述：<asp:Label ID="lblGroupDescription" runat="server"></asp:Label></div>
    </div>
    <div class="importantBox broaden">
        <p class="important">
            采买限制将控制您的用户只允许购买哪些航空公司及哪些出港城市的航班，若您没有发布某些航线的政策时，将显示您所设置的默认返点</p>
    </div>
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
      <%--  <div class="simple-box">
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
        </div>
        <div class="simple-box">
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
        <%--<div class="simple-box">
            <label class="title">
                特殊政策限制：</label><input type="hidden" value="4" class="policyType" />
            <div class="simple-content">
                <asp:RadioButton ID="rbnPurchaseOnlySelfSpecialPolicy" runat="server" Text="我未发布政策时允许下级采购其他供应商的特殊政策" /><br />
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
    <div class="btns">
        <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="提交" OnClick="btnSave_Click" />
          <input type="button" value="取消" class="btn class2" onclick="javascript:window.location.href='IncomeGroupList.aspx'"  />
    </div>
    <a id="divAddUser" style="display: none; background: #1A1A1A; color: #fff;
        margin: 10px 0; padding: 8px 0; text-align: center;" data="{type:'pop',id:'divAddUserList'}">
    </a>
    <div class="layer3 hidden" id="divAddUserList">
        <h4>
            添加用户：<asp:Label ID="lblUserGroupName" runat="server"></asp:Label>
            <asp:HiddenField ID="hfdCurrentIncomeGroupId" runat="server" />
            <a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="box-a clearfix input-pad">
            <div class="fl">
                <p class="input">
                    <span class="name">开户时间：</span>
                    <asp:TextBox ID="txtBeginTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;<asp:TextBox
                        ID="txtEndTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                </p>
                <p class="input">
                    <span class="name">用户组：</span>
                    <asp:DropDownList ID="ddlIncomeGroup" runat="server">
                    </asp:DropDownList>
                </p>
            </div>
            <div class="fl">
                <p class="input">
                    <span class="name">用户名：</span>
                    <asp:TextBox ID="txtUserNo" class="text" type="text" runat="server" />
                </p>
                <p class="input">
                    <span class="name">简称：</span>
                    <asp:TextBox ID="txtAbbreviateName" class="text" runat="server" />
                </p>
            </div>
            <div class="fl">
                <input type="button" style="margin: 25px 0 0 50px;" id="btnQuery" class="btn class1"
                    value="查询" />
            </div>
        </div>
        <div class="table" id='data-list'>
            <table id="dataList">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            开户时间
                        </th>
                        <th>
                            用户名
                        </th>
                        <th>
                            公司简称
                        </th>
                        <th>
                            用户组
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <div class="total-table" id="divChoise">
                <input type="checkbox" id="chkAll" /><label for="chkAll" class="pad-l">全选</label>
                |
                <label id="chkOther" class="pad-l">
                    反选</label>
                <br />
            </div>
        </div>
        <div id="emptyInfo" class="box hidden">
            没有任何符合条件的查询结果</div>
        <div class="btns" id="divPagination">
        </div>
        <div class="btns" id="divBtns">
            <input type="button" id="btnUpdateRelation" class="btn class1" value="确认并提交" />
            <input type="button" id="btnCancel" value="取消" class="btn class2 close" />
        </div>
    </div>
    <asp:HiddenField ID="hfdLimitation" runat="server" />
    </form>
</body>
</html>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/PurchaseRestrictionSetting.js?2013052003"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/DistributionOEMUserList.js?20130516"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/PurchaseLimitation.js?2013052106"></script>
<script type="text/javascript" src="../../Scripts/Global.js?20130520"></script>
<script src="../../Scripts/widget/template.js" type="text/javascript"></script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script type="text/x-juqery-tmpl" id="limitationTmpl">
   <p>
     ${Departures}(${Airlines})<br/>
     {{tmpl(Rebate,{ TranslateRelationName: TranslateRelationName ,RenderRate:RenderRate,TranslatePolicyTypeName:TranslatePolicyTypeName}) "#policyTypeItem"}}
       <a href="javascript:void(0);" class="remove">删除</a>
      <input type="hidden" value='${Data}' name='Data_${$item.CountAcc()}' />
   </p>

</script>
<script type="text/x-juqery-tmpl" id="policyTypeItem">
   ${$item.TranslateRelationName(Type)} {{if AllowOnlySelf}} ${$item.TranslatePolicyTypeName(Type)}  ${$item.RenderRate(Rebate)} %{{else}}可采购同行政策 {{/if}}  <br/>
</script>
<script type="text/javascript">
    $(function () {
        $("#addUser").click(function () {
            $("#lblUserGroupName").text($("#lblGroupName").text());
            for (var i = 0; i < $("#ddlIncomeGroup option").length; i++) {
                if ($("#ddlIncomeGroup option").eq(i).attr("value") == $("#hfdCurrentIncomeGroupId").val()) {
                    $("#ddlIncomeGroup option").eq(i).remove();
                }
            }
            var pageSize = 10;
            queryOrders();
            $("#btnQuery").click(function () {
                if ($("#dropPageSize").size() > 0) {
                    pageSize = $("#dropPageSize option:selected").val();
                }
                queryOrders(1, pageSize);
            });
            $("#divAddUser").click();

        });

        $("#btnSave").click(function () {
            if ( $("#divPurchaseLimitation").find("p").length == 0) {
                alert("请添加到下面的组");
                return false;
            }
        });
    })
</script>
