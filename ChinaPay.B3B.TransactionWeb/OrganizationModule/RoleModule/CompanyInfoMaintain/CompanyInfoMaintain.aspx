<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyInfoMaintain.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain.CompanyInfoMaintain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公司信息设置</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <style>
        .layer3Type .btn{ position: static;}
    </style>
<body>
    <div id="smallbd1">
        <form class="infoMaintenanceForm" action="" runat="server">
        <div id="companyInfoMatain" runat="server">
            <h3 class="titleBg">
                公司信息维护</h3>
            <table class="tableType1 form companyTable">
                <colgroup>
                    <col class="w10" />
                    <col class="w40" />
                    <col class="w10" />
                    <col class="w40" />
                </colgroup>
                <tr>
                    <td class="title">
                        用户名：
                    </td>
                    <td>
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        公司类型：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                        (<asp:Label ID="lblAccountType" runat="server"></asp:Label>)&nbsp;&nbsp;<a href="#"
                            visible="false" runat="server" id="typeUpgradeApply">类型变更申请</a>
                    </td>
                </tr>
                <tr id="individualName" runat="server">
                    <td class="title">
                        真实姓名：
                    </td>
                    <td>
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        身份证号：
                    </td>
                    <td>
                        <asp:Label ID="lblCerNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr id="enterpriseName" runat="server">
                    <td class="title">
                        企业名称：
                    </td>
                    <td>
                        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        企业简称：
                    </td>
                    <td>
                        <asp:Label ID="lblAbbreviateName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr id="enterprisePhone" runat="server">
                    <td class="title">
                        企业电话：
                    </td>
                    <td>
                         <asp:TextBox ID="txtCompanyPhone" tip="请填写企业电话" CssClass="text null" runat="server"></asp:TextBox>
                       <%-- <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>--%>
                    </td>
                    <td class="title">
                        组织机构：
                    </td>
                    <td>
                        <asp:Label ID="lblOrganationCode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        联系人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactName" tip="请填写联系人姓名" CssClass="text null" runat="server"></asp:TextBox>
                        <asp:Label ID="lblContactName" runat="server"></asp:Label>
                    </td>
                    <td class="title">
                        联系人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactPhone" tip="请填写联系人手机号码" CssClass="text null" runat="server"></asp:TextBox>
                       <%-- <asp:Label ID="lblContactPhone" runat="server"></asp:Label>--%>
                    </td>
                </tr>
                <tr id="enterpriseManager" runat="server">
                    <td class="title">
                        负责人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManager" tip="用于进行备用联系" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                    <td class="title">
                        负责人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManagerPhone" tip="请填写负责人手机号码" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="enterpriseEmergency" runat="server">
                    <td class="title">
                        紧急联系人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergency" tip="用于航班变动通知" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                    <td class="title">
                        紧急联系人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergencyPhone" tip="请填写紧急联系人手机" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        所在地：
                    </td>
                    <td>
                        <select id="ddlProvince" runat="server">
                        </select>
                        <select id="ddlCity" runat="server">
                            <option value="">-请选择-</option>
                        </select>
                        <select id="ddlCounty" runat="server">
                            <option value="">-请选择-</option>
                        </select>
                    </td>
                    <td class="title">
                        地址：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" tip="如人民中路36号以便向您邮寄相关物品" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        邮编：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPostCode" tip="填写当地的邮编" runat="server" CssClass="text null"></asp:TextBox>
                    </td>
                    <td class="title">
                        Email：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" tip="用于接收通知信息,格式如854985545@qq.com" CssClass="text null" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        传真：
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax" tip="用于接收平台所发送的资料等" runat="server" CssClass="text null"></asp:TextBox>
                    </td>
                    <td class="title">
                        QQ：
                    </td>
                    <td>
                        <asp:TextBox ID="txtQQ" tip="填写QQ号码方便在线沟通" runat="server" CssClass="text null"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td class="title">是否启用数据接口:</td>
                <td>
                  <asp:Label ID="lblIsExternalInterface" runat="server"></asp:Label>
                </td>
                <td class="title" runat="server" id="fixedPhoneTitle" visible="false">
                  固定电话：
                </td>
                <td id="fixedPhoneValue" runat="server" visible="false">
                 <asp:TextBox ID="txtFixedPhone" tip="请填写固定电话" runat="server" CssClass="text null"></asp:TextBox>
                </td>
                </tr>
            </table>
            <div class="btns">
                <asp:Button ID="btnSubmit" runat="server" Text="保&nbsp;&nbsp;存" CssClass=" btn class1"
                    OnClick="btnSubmit_Click" />
            </div>
        </div>
        <a id="divOpcial" style="display: none" data="{type:'pop',id:'div_Upgrade'}"></a>
        <div class="layer3 form" id="div_Upgrade" style="display: none;">
            <h4>
                类型变更申请 <a href="#" class="close">关闭</a>
            </h4>
            <p class="layer3Tips">
                <span>您当前的类型为：<b>
                    <asp:Label ID="lblCurrentCompanyType" runat="server"></asp:Label>
                    (<asp:Label ID="lblCurrentAccountType" runat="server"></asp:Label>)</b></span>
                请注意，同一帐号在一年之内仅能申请一次类型变更！请慎重操作。
            </p>
            <div class="layer3Type">
                <span class="curr">选择您的变更类型：</span>
                <asp:RadioButton ID="rbnSupplierIndividual" runat="server" Text="产品合作(个人)" Visible="false"
                    GroupName="type" />
                <asp:RadioButton ID="rbnSupplierEnterprise" runat="server" Text="产品合作(企业)" Visible="false"
                    GroupName="type" />
                <asp:RadioButton ID="rbnProviderEnterprise" runat="server" Text="出票合作(企业)" Visible="false"
                    GroupName="type" />
            </div>
            <div class="layer3Type" id="upgradeInfo" runat="server">
                <p class="changeTips">
                    请补填以下资料以便进行类型变更,填写完成后无需审核即可变更成功</p>
                <table class="txt-l changeType">
                    <tr>
                        <td class="txt-r" style="width:152px;">
                            <span class="curr">补全其他资料：</span>企业名称：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="text"></asp:TextBox>
                            <input type="button" class="btn class3" value="验证可用" onclick="CheckCompanyName(this,'txtCompanyName')" />
                            <span>营业执照上的企业名称</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            企业简称：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyAbbreaviateName" runat="server" CssClass="text"></asp:TextBox>
                            <input type="button" class="btn class3" value="验证可用" onclick="CheckAbbreviation(this,'txtCompanyAbbreaviateName')" />
                            <span>如xx公司</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            机构代码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrgnationCode" runat="server" CssClass="text"></asp:TextBox>
                            <span>填写您组织机构代码证上的代码如53038123-9</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            企业电话：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOfficePhone" runat="server" CssClass="text"></asp:TextBox>
                            <span>企业电话方便客服进行相关事宜的通知</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            负责人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtManagerName" runat="server" CssClass="text"></asp:TextBox>
                            <span>填写企业负责人姓名</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            负责人手机：
                        </td>
                        <td>
                            <asp:TextBox ID="txtManagerMobile" runat="server" CssClass="text"></asp:TextBox>
                            <span>填写负责人手机号</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            紧急联系人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmergencyName" runat="server" CssClass="text"></asp:TextBox>
                            <span>紧急联系人用于航班变更通知</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-r">
                            紧急联系人手机：
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmergecyMobile" runat="server" CssClass="text"></asp:TextBox>
                            <span>填写紧急联系人手机号以接收短信通知</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="layer3Type" id="warnInfo" runat="server">
                <center>
                    恭喜您，您的资料完整，无需补填，<asp:Label ID="lblTypeShow" runat="server">产品合作</asp:Label>需要平台工作人员审核</center>
                <center>
                    您提交后将会有客服专员与您取得联系</center>
            </div>
            <div class="layer3Type">
               
                <asp:CheckBox ID="chkProtocol" runat="server" />
                <a href="#" class="protocol">我已阅读并同意用户类型变更服务协议</a>
                <textarea id="protocolContent" style="color:#666; height:50px; line-height:1.5;display:block;width:98%;padding:10px 1%" readonly="readonly">
角色变更申请协议
1、当申请者点击同意表示自愿申请变更角色，并同意本协议内容。
2、角色变更一年只能申请一次，时间按照自然年计算。
3、变更规则：按照权限只能从低到高进行变更，不可从高到低变更。（例：采购商可申请变更为产品商，但产品商不能申请变更为采购商；在申请变更时，仅允许个人升级为个人/企业，企业升级为企业）。
4、变更角色需要通过平台审核，根据不同的角色提供不同的资质手续。
5、一旦通过平台审核，变更后的角色必须按照平台规定签订相应协议（即：《出票商协议》、《产品商协议》），遵循平台的交易秩序。
                </textarea>
            </div>
            <div class="layer3Btns">
                <asp:Button ID="btnConfirm" runat="server" CssClass="layerbtn btn1 fl" Text="确定"
                    OnClick="btnConfirm_Click" style="line-height:1;" />
                <a href="#" class="layerbtn btn2 fr close">取消</a>
            </div>
        </div>
        <asp:HiddenField ID="hfdAddress" runat="server" />
        <asp:HiddenField ID="hfdContactName" runat="server" />
        <asp:HiddenField ID="hfdContactPhone" runat="server" />
        <asp:HiddenField ID="hfdCompanyPhone" runat="server" />
        <asp:HiddenField ID="hfdManager" runat="server" />
        <asp:HiddenField ID="hfdManagerPhone" runat="server" />
        <asp:HiddenField ID="hfdEmergency" runat="server" />
        <asp:HiddenField ID="hfdEmergencyPhone" runat="server" />
        <asp:HiddenField ID="hfdPostCode" runat="server" />
        <asp:HiddenField ID="hfdEmail" runat="server" />
        <asp:HiddenField ID="hfdFax" runat="server" />
        <asp:HiddenField ID="hfdQQ" runat="server" />
        <asp:HiddenField ID="hfldAddressCode" runat="server" />
        <asp:HiddenField ID="hfdAccountType" runat="server" />
        <asp:HiddenField ID="hfdSign" runat="server" />
        <asp:HiddenField ID="hfdValid" runat="server" />
        <asp:HiddenField ID="hfdFixedPhone" runat="server" />
        </form>
    </div>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/companyInfoMatain.js?20130117" type="text/javascript"></script>
<script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
<script src="../../../Scripts/OrganizationModule/Register/Checking.js" type="text/javascript"></script>
<script src="../../../Scripts/OrganizationModule/RoleModule/CompanyUpgrade.js" type="text/javascript"></script>