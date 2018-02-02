<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.Register" %>

<%@ Register src="../../UserControl/Header.ascx" TagName="Header" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>注册</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header ID="heard" runat="server" />
        <div id="bd">
            <div id="enrol" runat="server">
                <ul>
                    <li class="e1" tip="1"><em class="regisert_ico purchaser_ico">采购</em><span>采购用户注册</span></li>
                    <li tip="2" ><em class="regisert_ico provide_ico">产品</em><span>产品供应申请</span></li>
                    <li tip="3"><em class="regisert_ico agent_ico">出票</em><span>出票代理申请</span></li>
                </ul>
            </div>
            <div id="enrolPenrol" runat="server">
                <p id="enrolPenrolContent" class="txt-ind">B3B平台兼容目前国内B2B机票平台的高返点业务，也是国内首家支持所有行程编码交易的唯一平台，更可以实现普通票、特价票、特殊票的交易。单程、往返程、多段联程、缺口程，散客和团队全航支持。增值服务申领实名POS机更是立马用客户的钱来做活生意。价比三家、低者为赢、现在加入采购，享高额采购积分换取免票......</p>
                <p class="obvious">请填写用户申请资料,*为必填</p>
            </div> 
            <div class="form space">
               <h3 class="titleBg">账户信息</h3>
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
                        <asp:RadioButton  ID="person" runat="server" Text="个人" GroupName="registrationType"/>
                        <asp:RadioButton  ID="company" runat="server" Text="企业"  GroupName="registrationType" />
                    </td>    
                </tr>
                <tr>
                    <td class="title">
                        <i class="must">*</i>用户名：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccount" runat="server" CssClass="text normalText"></asp:TextBox>
                        <input id="btnCheckAccount" type="button" class="btn class5" value="验证用户名是否可用" onclick="CheckAccount(this,'txtAccount')" />
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
                        为确保您能够对票务进行正常的支付操作，本系统自动为您开通国付通付款账户，账户密码同<asp:Literal runat="server" ID="lblPlatform"></asp:Literal>账号密码。
                        <a href="#" id="aPoolPayUserName">自定义国付通账号</a>
                    </td>
                </tr>
                <tr id="trPoolPayUserName">
                    <td class="title">国付通用户名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPoolPayUserName" CssClass="text normalText"></asp:TextBox>
                        <input id="btnPoolPayUserName" type="button" class="btn class5" value="验证用户名是否可用" onclick="CheckAccount(this,'txtPoolPayUserName')" />
                        <span></span>
                        <p id="pIsPrivateAccountNo">
                            <asp:CheckBox runat="server" ID="chkIsPersonAccountNo" Text="使用个人账户" />&nbsp;&nbsp;
                            <label class="obvious1">选择该项后国付通付款账号姓名将使用您的联系人姓名(需同身份证姓名)</label>
                        </p>
                    </td>
                </tr>
            </table>
               <h3 class="titleBg">基础信息</h3>
               <table class="noTopBorder">
                        <colgroup>
                            <col class="w20" />
                            <col class="w80" />
                        </colgroup>
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
                                <input type="button" value="验证可用" class="btn class5 companyContent" onclick="CheckCompanyName(this,'txtCompany')" />
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>企业简称：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAbbreviation" CssClass="text normalText"></asp:TextBox>
                                <input type="button" value="验证可用" class="btn class5 companyContent" onclick ="CheckAbbreviation(this,'txtAbbreviation')" />
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
                        <tr style="display:none;" id="trCompanyIDCard">
                            <td class="title">
                                <i class="must">*</i>身份证号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompanyIDCard" runat="server" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                <i class="must">*</i>手机：
                            </td>
                            <td>
                               <asp:TextBox runat="server" ID="txtPhone" CssClass="text normalText"></asp:TextBox>
                                <input type="button" class="btn class3" id="btnPhoneCode" value="获取验证码" />
                                <span></span>
                            </td>
                        </tr>
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
                            <td style="line-height: 2;">
                                我希望申领POS机：<asp:CheckBox ID="chkPos" runat="server" /><a href="/banner/banner_pos.aspx" target="_blank">POS机介绍</a><br />
                                <input type="checkbox" id="chkReadingProtocol" runat="server" checked="checked" /><label for="chkReadingProtocol" id="forReadingProtocol" runat="server">我已阅读并同意<a target="_blank" href="/About/serviceAgreement.aspx">《B3B机票平台用户服务协议》</a></label>
                            </td>
                        </tr>
                        <tr class="btns">
                            <td colspan="2">
                               <asp:Button ID="btnSubmit" runat="server" CssClass="btn class1" Text="提&nbsp;&nbsp;&nbsp;交" onclick="btnSubmit_Click" />
                            </td>
                        </tr>
                </table>
            </div>
        </div>
        <uc:Footer ID="footer" runat="server" />
        <asp:HiddenField runat="server" ID="hidCpmpanyType" Value="Purchaser" />
        <asp:HiddenField runat="server" ID="hidAccountType"  Value="person"/>
        <asp:HiddenField runat="server" ID="hfdIsOem" Value="False" />
        <asp:HiddenField runat="server" ID="hfdPlatformName" Value="B3B" />
    </form>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Register/NewRegister.js?20130123" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Register/Checking.js?20121129" type="text/javascript"></script>
    <script src="/Scripts/setting.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/RoleModule/FixityInformation.js?20130522" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Register/CheckingText.js?20100228" type="text/javascript"></script>
</body>
</html>
