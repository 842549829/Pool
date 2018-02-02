<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterOutTicket.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage.RegisterOutTicket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出票方开户</title>
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <form action="#" id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tr>
                <td colspan="2">
                    <h3>账户信息</h3>
                </td>
            </tr>
            <tr>
                <td class="title">
                    用户名:<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="text"></asp:TextBox>
                    <button type="button" class="btn class3" id="btnCheCkAccounNo">
                        验证用户</button>
                    <span class="tips-txt" runat="server" id="lblAccountNo"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    密码<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtPassWord" runat="server" CssClass="text" TextMode="Password"
                        onpaste="return false;"></asp:TextBox>
                    <span runat="server" id="lblPassWord"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    确认密码<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="text" TextMode="Password"
                        onpaste="return false;"></asp:TextBox>
                    <span runat="server" id="lblConfirmPassWord"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>
                        公司联系信息</h3>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司名称<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="text"></asp:TextBox>
                    <button type="button" class="btn class3" id="btnCheckCompanyName">
                        验证用户</button>
                    <span id="lblCompanyName" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司简称<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyShortName" runat="server" CssClass="text"></asp:TextBox>
                    <button type="button" class="btn class3" id="btnCheCkCompanyShortName">
                        验证用户</button>
                    <span id="lblCompanyShortName" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地<i class="must">*</i>
                </td>
                <td>
                    <a style="display: inline;" class="areaSelect" href="javascript:void(0);"><span class="provinceName">
                        请选择地区</span><i class="iconfont">&#405;</i></a> <span id="lblLocation"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司地址<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblAddress" runat="server"></span>
                </td>
            </tr>
                
            <tr>
                <td class="title">
                    公司电话<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblCompanyPhone" runat="server"></span>
                </td>
            </tr>
                
            <tr>
                <td class="title">
                    负责人<i class="must">*</i>
                </td>
                <td>
                    <div class="check">
                        <asp:TextBox ID="txtPrincipal" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips-txt" id="lblPrincipal" runat="server"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">
                    负责人手机<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtPrincipalPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblPrincipalPhone" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    联系人<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtLinkman" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblLinkman" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    联系人手机<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtLinkManPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblLinkManPhone" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    紧急联系人<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtUrgencyLinkMan" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblUrgencyLinkMan" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    紧急联人手机<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtUrgencyLinkManPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblUrgencyLinkManPhone" runat="server"></span>
                </td>
            </tr>
             <tr>
                <td class="title">
                    E_mail<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮政编码<i class="must">*</i>
                </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblPostCode" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    传真
                </td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblFaxes" runat="server"></span>
                </td>
            </tr>
           
            <tr>
                <td class="title">
                    MSN
                </td>
                <td>
                    <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt"></span>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnSubmit" runat="server" Text="完成开户" CssClass="btn class1" 
                        onclick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
            <asp:HiddenField ID="hidAddress" runat="server" />
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
    <script src="../../../../Scripts/widget/form-ui.js" type="text/javascript"></script>
    <script src="../../../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/Address.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/ExtendCompanyManage/RegisterBuy.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/Checking.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/Register/Register.js" type="text/javascript"></script>
</body>
</html>
