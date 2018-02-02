<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLower.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate.AddLower" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下级开户</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bd">
            <div class="form">
               <h3>账户信息</h3>
               <table>
                <colgroup>
                    <col class="w15" />
                    <col class="w85" />
                </colgroup>
                <caption>
                    
                </caption>
                <tr>
                    <td class="title">
                        <i class="must">*</i>账户名:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccount" runat="server" CssClass="text normalText"></asp:TextBox>
                        <input id="btnCheckAccount" type="button" class="btn class5" value="验证用户名" onclick="CheckAccount(this,'txtAccount')" />
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>登录密码：
                    </td>
                    <td>
                       <asp:TextBox ID="txtPassword" runat="server" CssClass="text normalText" TextMode="Password" onpaste="return false;"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>确认登录密码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text normalText" TextMode="Password" onpaste="return false;"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        国付通账户：
                    </td>
                    <td>
                        为确保您能够对票务进行正常的支付操作，本系统自动为您开通国付通付款账户，账户密码同<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>账号密码。
                    </td>
                </tr>
            </table>
               <h3>基础信息&nbsp;Basis information</h3>
               <table>
                        <colgroup>
                            <col class="w20" />
                            <col class="w80" />
                        </colgroup>
                        <caption>
                            
                        </caption>
                        <tr>
                            <td class="title">
                                <i class="must">*</i>注册类型：
                            </td>
                            <td>
                                <asp:RadioButton  ID="person" runat="server" Text="个人" Checked="true"  GroupName="registrationType"/>
                                <asp:RadioButton  ID="company" runat="server" Text="企业"  GroupName="registrationType" />
                            </td>    
                        </tr>
                        <tr class="personContent">
                            <td class="title">
                                <i class="must">*</i>真实姓名：
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="personContent">
                            <td class="title">
                                <i class="must">*</i>身份证号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtIDCard" runat="server" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>企业名称：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCompany" CssClass="text normalText"></asp:TextBox>
                                <input type="button" value="验证可用" class="btn class5" onclick="CheckCompanyName(this,'txtCompany')" />
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>企业简称：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAbbreviation" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>机构代码：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationCode" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>企业电话：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCompanyPhone" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>联系人：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtContact" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                <i class="must">*</i>手机：
                            </td>
                            <td>
                               <asp:TextBox runat="server" ID="txtPhone" CssClass="text normalText"></asp:TextBox>
                                <%--<input type="button" class="btn class3" id="btnPhoneCode" value="获取验证码" />--%>
                                <span></span>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="title">
                                <i class="must">*</i>手机验证码：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPhoneCode" CssClass="text smallText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>--%>
                        <tr class="btns">
                            <td colspan="2">
                               <asp:Button ID="btnSubmit" runat="server" CssClass="btn class1" Text="提交" 
                                    onclick="btnSubmit_Click" />
                            </td>
                        </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hidAccountType"  Value="person"/>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/Checking.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/NewRegister.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/CheckingText.js" type="text/javascript"></script>
