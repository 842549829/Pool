<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtendOpenAccount.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount.ExtendOpenAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>下级开户</title>
   
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head> <link href="../../../Styles/register.css" rel="stylesheet" type="text/css" />
<body>
	<form runat="server" id="form1">
		<div id="bd">
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
							 为确保您能够对票务进行正常的支付操作，本系统自动为您开通国付通付款账户，账户密码同<asp:Literal runat="server" ID="lblPlatformName"></asp:Literal>账号密码。
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
                                <img alt="验证码" id="imgValidateCode"  onclick="javascript:loadValidateCode()" title="换一张" style="display:inline; vertical-align:middle; padding: 0 5px;height:26px;" />
                                <span></span>
                            </td>
                        </tr>
                        <tr id="protorlIsShow" runat="server">
                            <td></td>
                            <td><input type="checkbox" id="chkReadingProtocol" checked="checked" /><label for="chkReadingProtocol">我已阅读并同意<a target="_blank" href="/About/serviceAgreement.aspx">《B3B机票平台用户服务协议》</a></label></td>
                        </tr>
                        <tr class="btns">
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnSubmit" CssClass="btn class1" Text="提交" onclick="btnSubmit_Click" />
                            </td>
                        </tr>
				</table>
			</div>
		</div>
        <asp:HiddenField runat="server" ID="hidCpmpanyType" Value="Purchaser" />
        <asp:HiddenField runat="server" ID="hidAccountType"  Value="person"/>
        <asp:HiddenField runat="server" ID="hfldAddressCode" />
        <asp:HiddenField ID="hfdIsOem" runat="server" />
	 </form>
        <script src="../../../Scripts/json2.js" type="text/javascript"></script>
        <script src="../../../Scripts/widget/common.js" type="text/javascript"></script>
        <script src="../../../Scripts/OrganizationModule/Address_New.js" type="text/javascript"></script>
        <script src="../../../Scripts/OrganizationModule/Register/Checking.js" type="text/javascript"></script>
        <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js?20130522" type="text/javascript"></script>
        <script src="../../../Scripts/OrganizationModule/Register/OpenAccountChecking.js?version=1.1" type="text/javascript"></script>
        <script type="text/javascript">
            function loadValidateCode() {
                $("#imgValidateCode").attr("src", '/VerifyCode.ashx?verifyType=vate&id=' + Math.random());
            }
            $(function () {
                //帐户切换
                var person = function () { $('.personContent').show(); $('.companyContent').hide(); };
                var company = function () { $('.personContent').hide(); $('.companyContent').show(); };
                $("#person").click(function () {
                    person(); $("#hidAccountType").val("person");
                });
                $("#company").click(function () {
                    company(); $("#hidAccountType").val("company");
                });
                if ($("#hidAccountType").val() == "person") {
                    person(); $("#person").attr("checked", true);
                } else {
                    company(); $("#company").attr("checked", true);
                }
                //获取参数验证是否能够开内部机构
                var parameter = getRequest();
                if (parameter == null) {
                    window.location.href = '/Index.aspx';
                } else {
                    if (parameter.Type == "Subordinate") {
                        $("#company").attr("checked", true);
                        company();
                    }
                }
                loadValidateCode();
            });
        </script>
    </body>
</html>