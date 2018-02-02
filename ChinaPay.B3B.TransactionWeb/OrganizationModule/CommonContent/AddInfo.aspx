<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>补填基本信息</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="addBox">
        <h4>
            操作提示</h4>
        <p>
            为确保您能够顺利的在平台上操作所有功能，您需要进行以下资料的补填</p>
        <table class="tableType1 addTable">
            <colgroup>
                <col class="w10" />
                <col class="w60" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <th>
                    邮箱：
                </th>
                <td>
                    <input type="text" id="txtEmail" runat="server" />
                </td>
                <th>
                    邮编：
                </th>
                <td>
                    <input type="text" id="txtPostCode" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    所在地：
                </th>
                <td>
                    <select id="ddlProvince" style="width: 80px;">
                    </select>
                    <select id="ddlCity" style="width: 80px;">
                        <option value="">-请选择-</option>
                    </select>
                    <select id="ddlCounty" style="width: 80px;">
                        <option value="">-请选择-</option>
                    </select>
                </td>
                <th>
                    地址：
                </th>
                <td>
                    <input type="text" id="txtAddress" runat="server" />
                </td>
            </tr>
            <tbody runat="server" id="enterprise">
                <tr>
                    <th>
                        负责人：
                    </th>
                    <td>
                        <input type="text" id="txtManagerName" runat="server" />
                    </td>
                    <th>
                        负责人手机：
                    </th>
                    <td>
                        <input type="text" id="txtManagerCellphone" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        紧急联系人：
                    </th>
                    <td>
                        <input type="text" id="txtEmergencyContact" runat="server" />
                    </td>
                    <th>
                        紧急联系人手机：
                    </th>
                    <td>
                        <input type="text" id="txtEmergencyCall" runat="server" />
                    </td>
                </tr>
            </tbody>
            <tr>
                <th>
                    传真：
                </th>
                <td>
                    <input type="text" id="txtFaexs" runat="server" />
                </td>
                <th>
                    QQ:
                </th>
                <td>
                    <input type="text" id="txtQQ" runat="server" />
                </td>
            </tr>
            <tbody id="fixedPhone" runat="server">
                <tr>
                    <th>
                        固定电话：
                    </th>
                    <td>
                        <input type="text" id="txtFixedPhone" runat="server" />
                    </td>
                    <td colspan="2">
                     固定电话可为空
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <input type="button" value="提 交" class="btn class1" id="btnSubmit" />
            <input type="button" value="取 消" class="btn class2 close" id="colse" />
        </div>
    </div>
    <asp:HiddenField ID="hfldAddressCode" runat="server" />
    </form>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#colse").click(function () { $("#mask").css("display", "none"); });
            $("#btnSubmit").click(function () {
                var txtEmail = $("#txtEmail").val(), txtManagerName = $("#txtManagerName").val(), ddlProvince = $("#ddlProvince option:selected").val(),
            ddlCity = $("#ddlCity option:selected").val(), ddlCounty = $("#ddlCounty option:selected").val(), txtManagerCellphone = $("#txtManagerCellphone").val(),
            txtAddress = $("#txtAddress").val(), txtEmergencyContact = $("#txtEmergencyContact").val(), txtPostCode = $("#txtPostCode").val(),
            txtEmergencyCall = $("#txtEmergencyCall").val(), txtFaexs = $("#txtFaexs").val(), txtQQ = $("#txtQQ").val(), txtFixedPhone = $("#txtFixedPhone").val();
                var email = /^\w+@\w+(\.\w{2,4}){1,2}$/, name = /(^[\u4e00-\u9fa5]{2,}$)|(^[\u4e00-\u9fa5]+[a-z,A-Z]+$)|(^[a-z,A-Z]+\/[a-z,A-Z]+$)/,
            phone = /^1[3458]\d{9}$/, postCode = /^[1-9]\d{5}$/, faexs = /^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$/,
            QQ = /^\d{5,12}$/;
                if (txtEmail.length < 1) { alert("Email不能为空"); return false; }
                if (!email.test(txtEmail)) { alert("Email格式错误"); return false; }
                if (txtPostCode.length < 1) { alert("邮编不能为空"); return false; }
                if (!postCode.test(txtPostCode)) { alert("邮编格式错误"); return false; }
                if (ddlCounty.length < 1) { alert("地址不能为空"); return false; }
                if (txtAddress.length < 1) { alert("详细地址不能为空"); return false; }

                if ($("#enterprise").length > 0) {
                    if (txtManagerName.length < 1) { alert("负责人姓名不能为空"); return false; }
                    if (!name.test(txtManagerName)) { alert("负责人姓名格式错误"); return false; }
                    if (txtManagerCellphone.length < 1) { alert("负责人手机不能为空"); return false; }
                    if (!phone.test(txtManagerCellphone)) { alert("负责人手机不能为空"); return false }
                    if (txtEmergencyContact.length < 1) { alert("紧急联系人不能为空"); return false; }
                    if (!name.test(txtEmergencyContact)) { alert("紧急联系人格式错误"); return false; }
                    if (txtEmergencyCall.length < 1) { alert("紧急联系人手机不能为空"); return false; }
                    if (!phone.test(txtEmergencyCall)) { alert("紧急联系人手机格式错误"); return false; }
                }
                if ($("#fixedPhone").length > 0) {
                    if (txtFixedPhone.length > 0 && !faexs.test(txtFixedPhone)) {
                        alert("固定电话格式错误！");
                        return false;
                    }
                }
                if (txtFaexs.length > 0 && !faexs.test(txtFaexs)) { alert("传真格式错误"); return false; }
                if (txtQQ.length > 0 && !QQ.test(txtQQ)) { alert("QQ格式错误"); return false; }
                var parameter, url;
                if ($("#enterprise").length > 0) {
                    url = "AddPurchaseEnterpriseInfo";
                    parameter = { "Email": txtEmail, "ManagerName": txtManagerName, "Province": ddlProvince, "City": ddlCity, "District": ddlCounty,
                        "ManagerCellphone": txtManagerCellphone, "Address": txtAddress, "EmergencyContact": txtEmergencyContact, "ZipCode": txtPostCode,
                        "EmergencyCall": txtEmergencyCall, "Faxes": txtFaexs, "QQ": txtQQ
                    };
                } else {
                    url = "AddPurchaseIndividualInfo";
                    parameter = { "Email": txtEmail, "Province": ddlProvince, "City": ddlCity, "District": ddlCounty, "Address": txtAddress,
                        "ZipCode": txtPostCode, "Faxes": txtFaexs, "QQ": txtQQ, "OfficePhone": txtFixedPhone
                    };
                }
                sendPostRequest("/OrganizationHandlers/AddInfo.ashx/" + url, JSON.stringify({ "info": parameter }), function (result) {
                    if (result == true) {
                        alert("修改成功");
                        $("#divPolicy,#mask").hide();
                    } else {
                        alert("修改失败")
                    }
                }, function () { alert("异常"); });
                return false;
            });
        });
    </script>
</body>
</html>
