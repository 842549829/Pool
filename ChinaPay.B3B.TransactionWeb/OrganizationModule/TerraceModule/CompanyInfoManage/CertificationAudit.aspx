<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificationAudit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.CertificationAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>认证中心</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <style type="text/css">
        input.width
        {
            width: 200px;
        }
        input.width-s
        {
            width: 60px;
        }
        .type-file-file
        {
            width: 60px;
            position: relative;
            top: 5px;
            left: -63px;
            color: rgb(255, 0, 0);
            filter: alpha(opacity:0);
            -moz-opacity: 0;
            -khtml-opacity: 0;
            opacity: 0;
        }
    </style>

<body>
    <div>
        <h3 class="titleBg">
            认证处理</h3>
        <form class="infoMaintenanceForm" id="form1" runat="server">
        <table class="tableType1 companyTable form">
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblAccountName"></asp:Label>
                </td>
                <td class="title">
                    类型：
                </td>
                <td>
                    <asp:Label ID="lblCong" runat="server" Text="由&nbsp;&nbsp;" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblCompanyType"></asp:Label>
                    <asp:Label ID="lblChange" runat="server" Text="&nbsp;&nbsp;变更为&nbsp;&nbsp;" Visible="false"></asp:Label>
                    <asp:Label ID="lblUpgradeType" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr id="lblIndividual" runat="server">
                <td class="title">
                    真实姓名：
                </td>
                <td>
                    <asp:Label ID="lblTrueName" runat="server"></asp:Label>
                </td>
                <td class="title">
                    身份证号：
                </td>
                <td>
                    <asp:Label ID="lblCertNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tbody id="lblCompanyInfo" runat="server">
                <tr>
                    <td class="title">
                        企业名称：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCompanyName"></asp:Label>
                    </td>
                    <td class="title">
                        企业简称：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAbbreviation"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        企业电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCompanyPhone"></asp:Label>
                    </td>
                    <td class="title">
                        机构代码：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblIdCode"></asp:Label>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    联系人：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblContact"></asp:Label>
                </td>
                <td class="title">
                    联系人手机：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblContactPhone"></asp:Label>
                </td>
            </tr>
            <tbody id="lblContactInfo" runat="server">
                <tr>
                    <td class="title">
                        负责人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManagerName" runat="server" CssClass="text null" tip="用于进行备用联系"></asp:TextBox>
                    </td>
                    <td class="title">
                        负责人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManagerPhone" runat="server" CssClass="text null" tip="请填写负责人手机号码"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        紧急联系人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="text null" tip="用于航班变动通知"></asp:TextBox>
                    </td>
                    <td class="title">
                        紧急联系人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergencyCall" runat="server" CssClass="text null" tip="请填写紧急联系人手机"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    所在地：
                </td>
                <td>
                    <select id="ddlProvince" style="width: 100px;">
                    </select>
                    <select id="ddlCity" style="width: 100px;">
                        <option value="">-请选择-</option>
                    </select>
                    <select id="ddlCounty" style="width: 100px;">
                        <option value="">-请选择-</option>
                    </select>
                </td>
                <td class="title">
                    地址：
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="text null" tip="如人民中路36号以便向您邮寄相关物品"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ：
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text null" tip="填写QQ号码方便在线沟通"></asp:TextBox>
                </td>
                <td class="title">
                    Email：
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text null" tip="用于接收通知信息,格式如565124541@qq.com"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮编：
                </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text null" tip="填写当地的邮编"></asp:TextBox>
                </td>
                <td class="title">
                    传真：
                </td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text null" tip="用于接收平台所发送的资料等"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    帐号使用期限
                </td>
                <td>
                    <asp:TextBox ID="txtBeginTime" runat="server" Width="100px" CssClass="text" onClick="WdatePicker({readOnly:true,isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}',minDate:'%y-%M-{%d}'})"></asp:TextBox>至
                    <asp:TextBox ID="txtEndTime" runat="server" Width="100px" CssClass="text" onClick="WdatePicker({readOnly:true,isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                </td>
                <td class="title" id="fixedPhoneTitle" runat="server">
                    固定电话
                </td>
                <td id="fixedPhoneValue" runat="server">
                    <asp:TextBox ID="txtFixedPhone" runat="server" CssClass="text null" tip="固定电话可为空"></asp:TextBox>
                </td>
            </tr>
            <tr id="certNoOrBussness" runat="server">
                <td class="title" rowspan="2">
                    附件：
                </td>
                <td colspan="3" id="certNoOrBussnessAccess" runat="server">
                    <input type="text" class="text width" readonly="readonly" id="txtBusinessLicense" />
                    <input type="button" value="浏览" class="btn class2 width-s" id="btnBusinessLicense" />
                    <asp:FileUpload runat="server" ID="fupBusinessLicense" Width="60" CssClass="type-file-file" />
                    <span>请点此上传<asp:Label ID="lblType" runat="server" Text="营业执照"></asp:Label>（副本）扫描件</span>
                    <span class="obvious1">仅支持jpg、png、bmp格式且小于500KB的图片上传</span>
                </td>
            </tr>
            <tr id="iataAccess" runat="server">
                <td class="title" rowspan="2" id="access" runat="server" visible="false">
                    附件：
                </td>
                <td colspan="3">
                    <input type="text" class="text width" readonly="readonly" id="txtIATA" />
                    <input type="button" class="btn class2 width-s" value="浏览" id="btnIATA" />
                    <asp:FileUpload runat="server" ID="fupIATA" Width="60" CssClass="type-file-file " />
                    <span class="obvious1">请点此上传IATA航协认可证书（副本)</span> <span>(产品方可留空)</span>
                </td>
            </tr>
            <tr id="bussnessTime" runat="server">
                <td class="title">
                    该用户从业时间：
                </td>
                <td colspan="3">
                    <select class="selectTime" id="dropYear" runat="server">
                        <option value="1">1年</option>
                        <option value="2">2年</option>
                        <option value="3">3年</option>
                        <option value="4">4年</option>
                        <option value="5">5年</option>
                        <option value="6">6年</option>
                        <option value="7">7年</option>
                        <option value="8">8年</option>
                        <option value="9">9年</option>
                        <option value="10">10年</option>
                        <option value="11">11年</option>
                        <option value="12">12年</option>
                        <option value="13">13年</option>
                        <option value="14">14年</option>
                        <option value="15">15年</option>
                    </select>
                    <span class="obvious1">填写从业时间有助于平台进行相关航线及优惠政策的控制等</span>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnPassed" runat="server" Text="审核通过" CssClass="btn class1" OnClick="btnPassed_Click" />
        <%--<asp:Button ID="btnRefuse" runat="server" Text="拒绝审核" CssClass="btn class2" OnClick="btnRefuse_Click" />--%>
        <input type="button" value="拒绝审核" class="btn class2" id="btnRefuse" />
        <input type="reset" value="清空表单" class="btn class2" />
        <asp:HiddenField runat="server" ID="hfldAddressCode" />
        <asp:HiddenField runat="server" ID="hfdValid" />
        <asp:HiddenField runat="server" ID="hfdBussnessValid" />
        <asp:HiddenField runat="server" ID="hfdAccountType" />
        <asp:HiddenField runat="server" ID="hfdAuditType" />
        <asp:HiddenField runat="server" ID="hfdCompanyId" />
        <a id="divOpcial" style="display: none;" data="{type:'pop',id:'div_Refuse'}"></a>
        <div class="layer3 hidden" id="div_Refuse">
         <%-- <div>
            <h2>
                拒绝审核原因：</h2>
            <asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="txtReason" Width="340px"
                Height="60px" MaxLength="500" />
        </div>--%>
        <h4>拒绝审核操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar fl">请在下方输入您的拒绝审核理由或备注。</p>
            <textarea class="text" cols="105" rows="4" id="txtReason"></textarea>
        </div>
           <div class="btns">
              <input type="button" class="btn class1" id="btnConfirm" value="确定" />
              <input type="button" class="btn class2 close"  value="取消"/>
           </div>
        </div>
        </form>
    </div>
</body>
</html>
<script src="../../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../../Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnRefuse").click(function () {
            $("#divOpcial").click();
        });
        $("#btnConfirm").click(function () {
            if (valiateReason()) {
                var reason = $("#txtReason").val();
                var companyId = $("#hfdCompanyId").val();
                var companyAccount = $("#lblAccountName").text();
                var auditTypeValue = $("#hfdAuditType").val();
                var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/Audit";
                sendPostRequest(targetUrl, JSON.stringify({ "companyId": companyId, "companyAccount": companyAccount, "auditTypeValue": auditTypeValue, "reason": reason }),
            function () {
                alert("拒绝审核通过");
                window.location.href = './CompanyList.aspx?Search=Back';
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
            }
        });

        inputTipText();
        $("#fupBusinessLicense").change(function () { $("#txtBusinessLicense").val($(this).val()); });
        $("#fupIATA").change(function () { $("#txtIATA").val($(this).val()); });
        $("#btnPassed").click(function () {
            var txtManagerName = $("#txtManagerName"), txtManagerPhone = $("#txtManagerPhone"), txtEmergencyContact = $("#txtEmergencyContact"),
            txtEmergencyCall = $("#txtEmergencyCall"), ddlCounty = $("#ddlCounty"), txtAddress = $("#txtAddress"), txtPostCode = $("#txtPostCode"),
            txtEmail = $("#txtEmail"), txtFaxes = $("#txtFaxes"), txtQQ = $("#txtQQ"), txtBusinessLicense = $("#txtBusinessLicense"), txtIATA = $("#txtIATA"),
            txtFixedPhone = $("#txtFixedPhone");

            var regName = /^[a-zA-z\u4e00-\uf9a5]{2,20}$/, regPhone = /^1[3458]\d{9}$/, regEmail = /^\w+@\w+(\.\w{2,4}){1,2}$/, regPostCode = /^\d{6}$/, regQQ = /^\d{5,12}$/,
            regFaxes = /^(?:1[3458]\d{9})$|^(?:\d{7,8})$|^(?:\d{3,4}-\d{7,8})$|^(?:\d{3,4}-\d{7,8}-\d{1,4})$/, officePhonePattern = /^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$/; ;
            if ($("#lblContactInfo").length > 0) {
                if (txtManagerName.val() == txtManagerName.attr("tip") || txtManagerName.val().length < 1) {
                    alert("负责人姓名不能为空 ");
                    txtManagerName.focus().select();
                    return false;
                }
                if (!regName.test(txtManagerName.val())) {
                    alert("负责人姓名格式错误");
                    txtManagerName.focus().select();
                    return false;
                }
                if (txtManagerPhone.val() == txtManagerPhone.attr("tip") || txtManagerPhone.val().length < 1) {
                    alert("负责人手机不能为空");
                    txtManagerPhone.focus().select();
                    return false;
                }
                if (!regPhone.test(txtManagerPhone.val())) {
                    alert("负责人手机格式错误");
                    txtManagerPhone.focus().select();
                    return false;
                }
                if (txtEmergencyContact.val() == txtEmergencyContact.attr("tip") || txtEmergencyContact.val().length < 1) {
                    alert("紧急联系人姓名不能为空");
                    txtEmergencyContact.focus().select();
                    return false;
                }
                if (!regName.test(txtEmergencyContact.val())) {
                    alert("紧急联系人姓名格式错误");
                    txtEmergencyContact.focus().select();
                    return false;
                }
                if (txtEmergencyCall.val() == txtEmergencyCall.attr("tip") || txtEmergencyCall.val().length < 1) {
                    alert("紧急联系人手机不能为空");
                    txtEmergencyCall.focus().select();
                    return false;
                }
                if (!regPhone.test(txtEmergencyCall.val())) {
                    alert("紧急联系人手机格式错误");
                    txtEmergencyCall.focus().select();
                    return false;
                }
            }
            if ($("option:selected", ddlCounty).val().length < 1) {
                alert("请选择所在地");
                return false;
            }
            if (txtAddress.val() == txtAddress.attr("tip") || txtAddress.val().length < 1) {
                alert("地址不能为空");
                txtAddress.focus().select();
                return false;
            }
            if (txtQQ.val() != txtQQ.attr("tip") && txtQQ.val().length > 0 && !regQQ.test(txtQQ.val())) {
                alert("QQ格式错误");
                txtQQ.focus().select();
                return false;
            }
            if (txtQQ.val() == txtQQ.attr("tip")) { txtQQ.val(""); }
            if (txtEmail.val() == txtEmail.attr("tip") || txtEmail.val().length < 1) {
                alert("Email不能为空");
                txtEmail.focus().select();
                return false;
            }
            if (!regEmail.test(txtEmail.val())) {
                alert("Emial格式错误");
                txtEmail.focus().select();
                return false;
            }
            if (txtPostCode.val() == txtPostCode.attr("tip") || txtPostCode.val().length < 1) {
                alert("邮编不能为空");
                txtPostCode.focus().select();
                return false;
            }
            if (!regPostCode.test(txtPostCode.val())) {
                alert("邮编格式错误");
                txtPostCode.focus().select();
                return false;
            }
            if (txtFaxes.val() != txtFaxes.attr("tip") && txtFaxes.val().length > 0 && !regFaxes.test(txtFaxes.val())) {
                alert("传真格式错误");
                txtFaxes.focus().select();
                return false;
            }
            if (txtFaxes.val() == txtFaxes.attr("tip")) { txtFaxes.val(""); }
            if ($("#hfdBussnessValid").val() != "false" && txtBusinessLicense.val().length < 1) {
                alert("请上传营业执照或个人身份证（副本）扫描件");
                return false;
            }
            if ($("#hfdValid").val() != "false" && txtIATA.val().length < 1) {
                alert("请上传IATA航协认可证书（副本)");
                return false;
            }
            if ($("#hfdAccountType").val() == "individual") {
                if (txtFixedPhone.val() != txtFixedPhone.attr("tip") && $.trim(txtFixedPhone.val()) != "" && !officePhonePattern.test(txtFixedPhone.val())) {
                    alert("固定电话格式错误");
                    return false;
                }
            }
            if (txtFixedPhone.val() == txtFixedPhone.attr("tip")) { txtFixedPhone.val(""); }
            return true;
        });
    });
    function valiateReason() {
        if ($.trim($("#txtReason").val()).length == 0) {
            alert("请输入禁用账号理由或备注");
            return false;
        }
        if ($.trim($("#txtReason").val()).length > 200) {
            alert("禁用账号理由或备注字数不能超过200");
            return false;
        }
        return true;
    }
</script>
