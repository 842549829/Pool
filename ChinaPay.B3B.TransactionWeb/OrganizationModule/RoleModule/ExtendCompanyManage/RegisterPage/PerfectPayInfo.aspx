<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfectPayInfo.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage.PerfectPayInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <title>完善开户信息</title>
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <h2>
            基础修改信息：</h2>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tr>
                <td class="title">
                    账户名<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="text"></asp:TextBox>
                    <button type="button" class="btn class3" id="btnCheCkAccounNo">
                        验证用户</button>
                    <span class="tips-txt" id="lblAccountNo"></span>
                </td>
            </tr>
            <tbody runat="server" id="tbEnterprise">
                <tr>
                    <td class="title">
                        机构代码<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrganizationCode" runat="server" CssClass="text"></asp:TextBox>
                            <span class="tips-txt" id="lblOrganizationCode"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        法人姓名<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLegalPersonName" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblLegalPersonName"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        法人电话<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLegalPersonPhone" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips-txt" id="lblLegalPersonPhone"></span>
                    </td>
                </tr>
            </tbody>
            <tr>
                <td class="title">
                    身份证号<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtIDCard" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblIDCard"></span>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" 
                        onclick="btnSave_Click" />
                </td>
            </tr>
        </table>
        </form>
    </div>
    <div class="layers" id="test_layer">
            <h2 class="hei obvious">
                错误信息<a href="javascript:void(0)"  class="qclose closeBtn" >&times;</a>
            </h2>
            <div>
                <span class="ds" style="width: 32px; background: url('../../../../Images/prompt087169.gif') no-repeat  -127.7px 21px;">
                </span><span class="ds" style="vertical-align: middle; padding-left: 40px;" id="lblMessage" runat="server"></span>
            </div>
            <div class="btns hei" style="height:35px;">
                <button class="btn class1 closeBtn">确定</button>
            </div>
     </div>
    <div class="fixed"></div>
    <script src="../../../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/ExtendCompanyManage/PerfectPayInfo.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/Register/Register.js" type="text/javascript"></script>
</body>
</html>
