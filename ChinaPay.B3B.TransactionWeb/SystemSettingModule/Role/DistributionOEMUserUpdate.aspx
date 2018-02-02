<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMUserUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.DistributionOEMUserUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
     <link href="/Styles/icon/main.css" rel="stylesheet" type="text/css" />
</head>   

<body>
    <div class="form">
        <h3 class="titleBg">
            基础信息修改：</h3>
        <form id="form1" runat="server">
        <table>
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
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                    类型：
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="presonName" runat="server">
                <td class="title">
                    真实姓名：
                </td>
                <td>
                    <asp:TextBox ID="txtPresonName" runat="server" CssClass="text"></asp:TextBox>
                </td>
                <td class="title">
                    身份证：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtIdCard" CssClass="text"></asp:TextBox>
                </td>
            </tr>
            <tbody runat="server" id="enterpriseinfo">
                <tr>
                    <td class="title">
                        企业名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="text"></asp:TextBox>
                    </td>
                    <td class="title">
                        企业简称：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompanyShortName" CssClass="text"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        企业电话：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompanyPhone" CssClass="text"></asp:TextBox>
                    </td>
                    <td class="title">
                        机构代码：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOrginationCode" CssClass="text"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    联系人：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLinkman" CssClass="text"></asp:TextBox>
                </td>
                <td class="title">
                    联系人手机：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLinkManPhone" CssClass="text"></asp:TextBox>
                </td>
            </tr>
            <tbody id="enterpriseContact" runat="server">
                <tr>
                    <td class="title">
                        负责人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManagerName" runat="server" CssClass="text"></asp:TextBox>
                        <span></span>
                    </td>
                    <td class="title">
                        负责人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtManagerCellphone" runat="server" CssClass="text"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        紧急联系人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="text"></asp:TextBox>
                        <span></span>
                    </td>
                    <td class="title">
                        紧急联系人手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmergencyCall" runat="server" CssClass="text"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    所在地：
                </td>
                <td id="tdAddress">
                    <select id="ddlProvince" style="width: 100px;">
                    </select>
                    <select id="ddlCity" style="width: 100px;">
                        <option value="">-请选择-</option>
                    </select>
                    <select id="ddlCounty" style="width: 100px;">
                        <option value="">-请选择-</option>
                    </select>
                    <span id="lblLocation"></span>
                </td>
                <td class="title">
                    地址：
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ：
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
                <td class="title">
                    E_Mail：
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮编：
                </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
                <td class="title">
                    传真：
                </td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
            </tr>
                <tr>
                     <td class="title" runat="server" id="fixedPhoneTitle" visible="false">
                        固定电话：
                    </td>
                    <td  runat="server" id="fixedPhoneValue" visible="false">
                        <asp:TextBox ID="txtFixedPhone" runat="server" CssClass="text"></asp:TextBox>
                    </td>
                    <td class="title">
                      用户组：
                    </td>
                    <td>
                      <asp:DropDownList ID="ddlIncomeGroup" runat="server"></asp:DropDownList>
                      <a href="#" id="addGroup" runat="server"> 添加用户组</a>
                    </td>
                </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
                    <input class="btn class2" value="返回" type="button" onclick="window.location.href='DistributionOEMUserList.aspx?Search=Back';" />
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hfldAddressCode" />
        </form>
    </div>
    <script src="../../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnSave").click(function () {
                var personName = $("#txtPresonName");
                var certNo = $("#txtIdCard");
                var companyName = $("#txtCompanyName");
                var companyShortName = $("#txtCompanyShortName");
                var companyPhone = $("#txtCompanyPhone");
                var orginationCode = $("#txtOrginationCode");
                var contactName = $("#txtLinkman");
                var contactPhone = $("#txtLinkManPhone");
                var fixedPhone = $("#txtFixedPhone");
                var ddlCounty = $("#ddlCounty option:selected").val(), txtPostCode = $("#txtPostCode"), txtAddress = $("#txtAddress"), txtFaxes = $("#txtFaxes")
                txtEmail = $("#txtEmail"), txtQQ = $("#txtQQ"), txtManagerName = $("#txtManagerName"), txtManagerCellphone = $("#txtManagerCellphone"),
                txtEmergencyContact = $("#txtEmergencyContact"), txtEmergencyCall = $("#txtEmergencyCall");
                var email = /^\w+@\w+(\.\w{2,4}){1,2}$/, name = /(^[\u4e00-\u9fa5]{2,10}$)|(^[\u4e00-\u9fa5]+[a-z,A-Z]+$)|(^[a-z,A-Z]+\/[a-z,A-Z]+$)/,

                phone = /^1[3458]\d{9}$/, postCode = /^[1-9]\d{5}$/, faexs = /^(?:\d{7,8})$|^(?:\d{3,4}-\d{7,8})$|^(?:\d{3,4}-\d{7,8}-\d{1,4})$/,
                QQ = /^\d{5,12}$/;
                var companyNamePattern = /^[\u4e00-\u9fa5]{4,25}$/; var shortName = /^[\u4e00-\u9fa5]{2,10}$/; var companyPhonePattern = /^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$/;
                var orginationCodePattern = /^\d{8}-[\dxX]{1}$/;
                if ($("#presonName").length > 0) {
                    if (personName.val().length < 1) {
                        personName.focus().select();
                        alert("真实姓名不能为空！");
                        return false;
                    }
                    if (!name.test(personName.val())) {
                        personName.focus().select();
                        alert("真实姓名格式错误！");
                        return false;
                    }
                    if (certNo.val().length < 1) {
                        certNo.focus().select();
                        alert("身份证号不能为空！");
                        return false;
                    }
                    if (!isCardID(certNo.val())) {
                        certNo.focus().select();
                        alert("身份证号格式错误！");
                        return false;
                    }
                    if ($.trim(fixedPhone.val()) != "") {
                        if (!companyPhonePattern.test(fixedPhone.val())) {
                            fixedPhone.select();
                            alert("固定电话格式错误！");
                            return false;
                        }
                    }
                }
                if ($("#enterpriseinfo").length > 0) {
                    if (companyName.val().length < 1) {
                        companyName.focus().select();
                        alert("公司名称不能为空！");
                        return false;
                    }
                    if (!companyNamePattern.test(companyName.val())) {
                        companyName.focus().select();
                        alert("公司名称格式错误！");
                        return false;
                    }
                    if (companyShortName.val().length < 1) {
                        companyShortName.focus().select();
                        alert("公司简称不能为空！");
                        return false;
                    }
                    if (!shortName.test(companyShortName.val())) {
                        companyShortName.focus().select();
                        alert("公司简称格式错误！");
                        return false;
                    }
                    if (companyPhone.val().length < 1) {
                        companyPhone.focus().select();
                        alert("公司电话不能为空！");
                        return false;
                    }
                    if (!companyPhonePattern.test(companyPhone.val())) {
                        companyPhone.focus().select();
                        alert("公司电话格式错误！");
                        return false;
                    }
                    if (orginationCode.val().length < 1) {
                        orginationCode.focus().select();
                        alert("组织机构代码不能为空！");
                        return false;
                    }
                    if (!orginationCodePattern.test(orginationCode.val())) {
                        orginationCode.focus().select();
                        alert("组织机构代码格式错误！");
                        return false;
                    }
                }
                if (contactName.val().length < 1) { contactName.focus().select(); alert("联系人姓名不能为空"); return false; }
                if (!name.test(contactName.val())) { contactName.focus().select(); alert("联系人姓名格式错误"); return false; }
                if (contactPhone.val().length < 1) { contactPhone.focus().select(); alert("联系人手机不能为空"); return false; }
                if (!phone.test(contactPhone.val())) { contactPhone.focus().select(); alert("联系人手机格式错误"); return false; }

                if ($("#enterpriseContact").length > 0) {
                    if (txtManagerName.val().length < 1) { txtManagerName.focus().select(); alert("负责人姓名不能为空"); return false; }
                    if (!name.test(txtManagerName.val())) { txtManagerName.focus().select(); alert("负责人姓名格式错误"); return false; }
                    if (txtManagerCellphone.val().length < 1) { txtManagerCellphone.focus().select(); alert("负责人手机不能为空"); return false; }
                    if (!phone.test(txtManagerCellphone.val())) { txtManagerCellphone.focus().select(); alert("负责人手机不能为空"); return false }
                    if (txtEmergencyContact.val().length < 1) { txtEmergencyContact.focus().select(); alert("紧急联系人不能为空"); return false; }
                    if (!name.test(txtEmergencyContact.val())) { txtEmergencyContact.focus().select(); alert("紧急联系人格式错误"); return false; }
                    if (txtEmergencyCall.val().length < 1) { txtEmergencyCall.focus().select(); alert("紧急联系人手机不能为空"); return false; }
                    if (!phone.test(txtEmergencyCall.val())) { txtEmergencyCall.focus().select(); alert("紧急联系人手机格式错误"); return false; }
                }
                if (ddlCounty.length < 1) {
                    alert("请选择所在地");
                    return false;
                }
                if (txtAddress.val().length < 1) {
                    txtAddress.focus().select();
                    alert("地址不能为空");
                    return false;
                }
                if (txtQQ.val().length > 0 && !QQ.test(txtQQ.val())) { txtQQ.focus().select(); alert("QQ不格式错误"); return false; }
                if (txtEmail.val().length < 1) {
                    txtEmail.focus().select();
                    alert("Email不能为空");
                    return false;
                }
                if (!email.test(txtEmail.val())) {
                    txtEmail.focus().select();
                    alert("Email格式错误");
                    return false;
                }
                if (txtPostCode.val().length < 1) {
                    txtPostCode.focus().select();
                    alert("邮编不能为空");
                    return false;
                }
                if (!postCode.test(txtPostCode.val())) {
                    txtPostCode.focus().select();
                    alert("邮编格式错误");
                    return false;
                }
                if (txtFaxes.val().length > 0 && !faexs.test(txtFaxes.val())) {
                    txtFaxes.focus().select();
                    alert("传真格式错误");
                    return false;
                }
            });
        });
    </script>
</body>
</html>
