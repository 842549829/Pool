<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenAccount.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount.OpenAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开户</title>
</head>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style>
        #openAccount{max-width:980px;}
        #openAccount #enrol ul li{width:33.3%;}
        #openAccount #enrolPenrol{width:95.5%;}
    </style>
<body>
	<form runat="server" id="form1">
        <asp:HiddenField runat="server" ID="hfdPlatformName" Value="B3B" />
		<div id="openAccount">
			<div id="enrol" style="padding-top:20px;">
				<ul>
					<li class="e1" tip="1"><em class="regisert_ico purchaser_ico">采购</em><span>采购开户</span></li>
                    <li tip="2"><em class="regisert_ico provide_ico">产品</em><span>产品合作申请</span></li>
                    <li tip="3"><em class="regisert_ico agent_ico">出票</em><span>出票代理申请</span></li>
				</ul>
			</div>
			<div id="enrolPenrol">
				<p id="enrolPenrolContent" class="txt-ind"><asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>平台兼容目前国内B2B机票平台的高返点业务，也是国内首家支持所有行程编码交易的唯一平台，更可以实现普通票、特价票、特殊票的交易。单程、往返程、多段联程、缺口程，散客和团队全航支持。增值服务申领实名POS机更是立马用客户的钱来做活生意。价比三家、低者为赢、现在加入采购，享高额采购积分换取免票......</p>
				<p class="obvious">请填写用户申请资料，*为必填</p>
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
							<i class="must">*</i>用户名：
						</td>
						<td>
                            <asp:TextBox runat="server" ID="txtAccountNo" CssClass="text normalText"></asp:TextBox>
							<input id="btnCheckAccount" type="button" class="btn class5" value="验证用户名是否可用" onclick="CheckAccount(this,'txtAccountNo')" />
							<span></span>
						</td>
					</tr>
					<tr>
						<td class="title">
							<i class="must">*</i>登录密码:
						</td>
						<td>
                            <asp:TextBox runat="server" ID="txtPassword" CssClass="text normalText" TextMode="Password" onpaste="return false;"></asp:TextBox>
							<span></span>
						</td>
					</tr>
					<tr>
						<td class="title">
							<i class="must">*</i>确认登录密码:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtConfirmPassword" CssClass="text normalText" TextMode="Password" onpaste="return false;"></asp:TextBox>
							<span></span>
						</td>
					</tr>
					<tr>
						<td class="title">
							国付通账号：
						</td>
						<td>
							 为确保您能够对票务进行正常的支付操作，本系统自动为您开通国付通付款账户，账户密码同<asp:Literal runat="server" ID="lblPlatformName1"></asp:Literal>账号密码。
						</td>
					</tr>
				</table>
				<h3 class="titleBg">基础信息</h3>
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
                          <asp:RadioButton  ID="person" runat="server" Text="个人" Checked="true"  GroupName="registrationType"/>
                          <asp:RadioButton  ID="company" runat="server" Text="企业"  GroupName="registrationType" />
						</td>
					</tr>
					<tr class="personContent">
                            <td class="title">
                                <i class="must">*</i>真实姓名：
                            </td>
                            <td>
                                <asp:TextBox ID="txtPresonName" runat="server" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="personContent">
                            <td class="title">
                                <i class="must">*</i>身份证号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtPresonIDCard" runat="server" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="personContent">
                            <td class="title">
                                <i class="must">*</i>手机：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPresonPhone" CssClass="text normalText"></asp:TextBox>
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
                            <td class="title"><i class="must">*</i>机构代码：</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationCode" CssClass="text normalText"></asp:TextBox><span></span>
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
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>联系人身份证号：
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="text normalText" ID="txtCompanyIDCard"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>联系人手机：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtContactPhone" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>负责人：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMangerName" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>负责人手机：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtManagerCellphone" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>紧急联系人：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmergencyContact" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr class="companyContent">
                            <td class="title">
                                <i class="must">*</i>紧急联系人手机：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmergencyPhone" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                <i class="must">*</i>Email：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                <i class="must">*</i>所在地：
                            </td>
                            <td>
                                <select id="ddlProvince" style="width:100px;" >
						        </select>
						        <select id="ddlCity" style="width:100px;">
							        <option value="">-请选择-</option>
						        </select>
						        <select id="ddlCounty"  style="width:100px;">
							        <option value="">-请选择-</option>
						        </select>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                地址：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAddress" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                QQ：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtQQ" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                传真：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFaxes" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                邮编：
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPostCode" CssClass="text normalText"></asp:TextBox>
                                <span></span>
                            </td>
                        </tr>
                        <tr runat="server" id="tdverifyCode">
                            <td class="title">
                                <i class="must">*</i>图片验证码：
                            </td>
                            <td>
                                <asp:TextBox ID="txtverifyCode" runat="server" CssClass="text"></asp:TextBox>
                                <img  alt="验证码" id="imgValidateCode"  onclick="javascript:loadValidateCode()" title="换一张" style="display:inline; vertical-align:middle; padding: 0 5px;height:26px;" />
                                <span></span>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><input type="checkbox" id="chkReadingProtocol" checked="checked" /><label for="chkReadingProtocol">我已阅读并同意<a target="_blank" href="/About/serviceAgreement.aspx">《<asp:Literal runat="server" ID="lblPlatformName2"></asp:Literal>机票平台用户服务协议》</a></label></td>
                        </tr>
                        <tr class="btns">
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnSubmit" CssClass="btn class1" Text="提交" 
                                    onclick="btnSubmit_Click" />
                            </td>
                        </tr>
				</table>
			</div>
		</div>
        <asp:HiddenField runat="server" ID="hidCpmpanyType" Value="Purchaser" />
        <asp:HiddenField runat="server" ID="hidAccountType"  Value="person"/>
        <asp:HiddenField runat="server" ID="hfldAddressCode" />
	 </form>
        <script src="/Scripts/json2.js" type="text/javascript"></script>
        <script src="/Scripts/widget/common.js" type="text/javascript"></script>
        <script src="/Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
        <script src="/Scripts/OrganizationModule/Register/NewRegister.js" type="text/javascript"></script>
        <script src="/Scripts/OrganizationModule/Register/Checking.js" type="text/javascript"></script>
        <script src="/Scripts/OrganizationModule/RoleModule/FixityInformation.js?20130522" type="text/javascript"></script>
        <script src="/Scripts/OrganizationModule/Register/OpenAccountChecking.js?version=1.1" type="text/javascript"></script>
        <script type="text/javascript">
            function loadValidateCode() {
                $("#imgValidateCode").attr("src", '/VerifyCode.ashx?verifyType=vate&id=' + Math.random());
            }
            loadValidateCode();
        </script>
    </body>
</html>
