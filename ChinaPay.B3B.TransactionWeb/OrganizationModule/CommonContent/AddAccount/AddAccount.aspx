<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAccount.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount.AddAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>B3B开户</title>
    <link href="/Styles/register.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hfdPlatformName" Value="B3B" />
    <div id="bd">
        <div id="enrol" style="padding-top: 20px;">
            <ul>
                <li class="e1"><em class="regisert_ico purchaser_ico">采购</em><span>采购开户</span></li>
                <li><em class="regisert_ico provide_ico">产品</em><span>产品合作申请</span></li>
                <li><em class="regisert_ico agent_ico">出票</em><span>出票代理申请</span></li>
            </ul>
        </div>
        <div id="enrolPenrol">
            <p id="enrolPenrolContent">
                采购用户：也就是购买机票的人，B3B是高返点购票平台，为您提供安全、便捷的购票服务，出票时间短，处理速度快。</p>
            <p class="obvious">
                请填写用户申请资料,*为必填</p>
        </div>
        <div class="form space">
            <h3 class="titleBg">
                账户信息</h3>
            <table class="noTopBorder">
                <colgroup>
                    <col class="w20" />
                    <col class="w80" />
                </colgroup>
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
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="text normalText" TextMode="Password"
                            onpaste="return false;"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>确认登录密码：
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text normalText" TextMode="Password"
                            onpaste="return false;"></asp:TextBox>
                        <span></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        国付通账户：
                    </td>
                    <td>
                        为确保您能够对票务进行正常的支付操作，本系统自动为您开通国付通付款账户，账户密码同B3B账号密码。
                    </td>
                </tr>
            </table>
            <h3 class="titleBg">
                基础信息</h3>
            <table class="noTopBorder">
                <colgroup>
                    <col class="w20" />
                    <col class="w80" />
                </colgroup>
                <tr>
                    <td class="title">
                        <i class="must">*</i>注册类型：
                    </td>
                    <td>
                        <asp:RadioButton ID="person" runat="server" Text="个人" Checked="true" GroupName="registrationType" />
                        <asp:RadioButton ID="company" runat="server" Text="企业" GroupName="registrationType" />
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
                        <%--<input type="button" class="btn class3" id="btnPhoneCode" value="获取验证码" runat="server" />--%>
                        <span></span>
                    </td>
                </tr>
                <%--<tbody id="pos" runat="server">
                            <tr>
                                <td class="title">
                                    <i class="must">*</i>手机验证码：
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPhoneCode" CssClass="text smallText"></asp:TextBox>
                                    <span></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    我希望申领POS机：<asp:CheckBox ID="chkPos" runat="server" /><a href="#">POS机介绍</a>
                                </td>
                            </tr>
                       </tbody>--%>
                <tr class="btns">
                    <td colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn class1" Text="提交" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidCpmpanyType" Value="Purchaser" />
    <asp:HiddenField runat="server" ID="hidAccountType" Value="person" />
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/Checking.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/NewRegister.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Register/CheckingText.js" type="text/javascript"></script>
