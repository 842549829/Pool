<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOemAuthorizationAddOrUpdate.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOemAuthorizationAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="form">
        <h3 class="titleBg">
            <asp:Label ID="lblOperator" runat="server">新增</asp:Label>
            授权</h3>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w90" />
            </colgroup>
            <tr>
                <td class="title">
                    输入B3B账号：
                </td>
                <td id="insert" runat="server">
                    <asp:TextBox ID="txtB3bAccountNo" runat="server"></asp:TextBox>
                    <input type="button" id="btnGetAccountInfo" class=" btn class2" value="获取账户信息" />
                </td>
                <td id="update" runat="server" visible="false">
                    <asp:Label ID="lblB3bAccountNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="companyDetailInfo" style="display:none">
                <td colspan="2">
                    <table class="intense">
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
                                公司类型：
                            </td>
                            <td>
                                <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="lblIndividual">
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
                        <tbody id="lblCompany">
                            <tr>
                                <td class="title">
                                    公司名称：
                                </td>
                                <td>
                                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                </td>
                                <td class="title">
                                    公司简称：
                                </td>
                                <td>
                                    <asp:Label ID="lblComapnyShortName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="title">
                                    公司电话：
                                </td>
                                <td>
                                    <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>
                                </td>
                                <td class="title">
                                    组织机构：
                                </td>
                                <td>
                                    <asp:Label ID="lblOrginationCode" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td class="title">
                                联系人：
                            </td>
                            <td>
                                <asp:Label ID="lblLinkman" runat="server"></asp:Label>
                            </td>
                            <td class="title">
                                联系人手机：
                            </td>
                            <td>
                                <asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tbody id="tbBuyorOut">
                            <tr>
                                <td class="title">
                                    负责人：
                                </td>
                                <td>
                                    <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                                </td>
                                <td class="title">
                                    负责人手机：
                                </td>
                                <td>
                                    <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td class="title">
                                所在地：
                            </td>
                            <td>
                                <asp:Label ID="lblLocation" runat="server"></asp:Label>
                            </td>
                            <td class="title">
                                地址：
                            </td>
                            <td>
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="exceptPurchase" style="display:none">
                            <td class="title">
                                附件：
                            </td>
                            <td>
                                <a href="#" id="bussinessLicense" runat="server">营业执照</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="#" id="certNo" runat="server">身份证</a>&nbsp;&nbsp;&nbsp;&nbsp; <a href="#"
                                    id="iata" runat="server">IATA</a>
                            </td>
                            <td class="title">
                                使用期限：
                            </td>
                            <td id="tdCompanyPhone0">
                                <asp:Label ID="lblBeginDeadline" runat="server"></asp:Label>至<asp:Label ID="lblEndDeadline"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                开户时间
                            </td>
                            <td id="registerTime">
                                <asp:Label runat="server" ID="lblRegisterTime"></asp:Label>
                            </td>
                            <td class="title auditTime">
                                审核时间
                            </td>
                            <td class="auditTime">
                                <asp:Label runat="server" ID="lblAudit"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="title">
                    OEM名称：
                </td>
                <td>
                    <asp:TextBox ID="txtOemName" runat="server"></asp:TextBox>该OEM版本的网站标题，设置后用户可自行更改
                </td>
            </tr>
            <tr>
                <td class="title">
                    授权域名：
                </td>
                <td>
                    <asp:TextBox ID="txtAuthorizationDomain" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    授权到期：
                </td>
                <td>
                    <asp:TextBox ID="txtAuthorizationDeadline" runat="server" onClick="WdatePicker({isShowClear:false})"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    授权保证金：
                </td>
                <td>
                    <asp:TextBox ID="txtAuthorizationDeposit" runat="server"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td class="title">
                    登陆页面地址：
                </td>
                <td>
                    <asp:TextBox ID="txtLoginUrl" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    配置来源：
                </td>
                <td>
                    <asp:RadioButton ID="rdnPlatform" runat="server" Text="使用平台配置" GroupName="configration" Checked="true" />
                    <asp:RadioButton ID="rdnOwner" runat="server" Text="使用独立配置" GroupName="configration" />
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnSubmit" Text="提交" CssClass="btn class1" runat="server" OnClick="btnSubmit_Click" />
                    <input type="button" class="btn class2" value="取消" id="btnGoBack" runat="server" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfdCompanyId" runat="server" />
          <asp:HiddenField ID="hfdBussniess" runat="server" />
         <asp:HiddenField ID="hfdCertNo" runat="server" />
         <asp:HiddenField ID="hfdIATA" runat="server" />
        </form>
    </div>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function ()
    {
        $("#btnGetAccountInfo").click(function ()
        {
            var accountNo = $("#txtB3bAccountNo").val();
            sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/GetCompanyInfo", JSON.stringify({ "accountNo": accountNo }), function (result)
            {
                if (result != null && result.CompanyId != undefined)
                {
                    if (result.IsOem)
                    {
                        alert("该账号是OEM");
                    } else if (result.IsPlatformEmployee)
                    {
                        alert("平台帐号不能开通OEM");
                    } else
                    {
                        if (result.Enabled)
                        {
                            $("#hfdBussniess").val("/CompanyInfoManage/LicenseQuery.aspx?type=bussiness&companyId=" + result.CompanyId);
                            $("#hfdCertNo").val("/CompanyInfoManage/LicenseQuery.aspx?type=certNo&companyId=" + result.CompanyId);
                            $("#hfdIATA").val("/CompanyInfoManage/LicenseQuery.aspx?type=iata&companyId=" + result.CompanyId);
                            $("#lblAccountNo").text(result.UserNo);
                            $("#lblCompanyType").text(result.CompanyType);
                            $("#lblLinkman").text(result.Contact);
                            $("#lblLinkmanPhone").text(result.ContactPhone);
                            $("#lblLocation").text(result.Loaction);
                            $("#lblAddress").text(result.Address);
                            $("#lblRegisterTime").text(result.RegisterTime);
                            $("#lblAudit").text(result.AuditTime);
                            if (result.AccountType == 0)
                            {
                                $("#lblIndividual").show();
                                $("#lblCompany").hide();
                                $("#lblTrueName").text(result.CompanyName);
                                $("#lblCertNo").text(result.CertNo);
                            }
                            if (result.AccountType == 1)
                            {
                                $("#lblIndividual").hide();
                                $("#lblCompany").show();
                                $("#lblCompanyName").text(result.CompanyName);
                                $("#lblComapnyShortName").text(result.AbbreviateName);
                                $("#lblCompanyPhone").text(result.OfficePhones);
                                $("#lblOrginationCode").text(result.OrginationCode);
                                $("#lblPrincipal").text(result.ManagerName);
                                $("#lblPrincipalPhone").text(result.ManagerCellphone);
                            }
                            if (result.CompanyTypeValue == 1 && result.CompanyTypeValue == 4)
                            {
                                $("#exceptPurchase").show();
                                $("#lblBeginDeadline").text(result.BeginDeadline);
                                $("#lblEndDeadline").text(result.EndDeadline);
                            }
                            $("#companyDetailInfo").show();
                        } else
                        {
                            alert("该账号已被禁用");
                        }
                    }
                } else
                {
                    alert("没有该账号的信息");
                }
            }, function (e)
            {
                if (e.statusText == "timeout")
                {
                    alert("服务器忙");
                } else
                {
                    alert(e.responseText);
                }
            });
        });
        $("#bussinessLicense").click(function ()
        {
            window.open($("#hfdBussniess").val());
        });
        $("#certNo").click(function ()
        {
            window.open($("#hfdCertNo").val());
        });
        $("#iata").click(function ()
        {
            window.open($("#hfdIATA").val());
        });

        $("#btnSubmit").click(function ()
        {
            if (!valiate())
            {
                return false;
            }
        });
    })

    function valiate() {
        if ($("#lblOperator").text() != "修改") {
            var userNo = $("#txtB3bAccountNo");
            if ($.trim(userNo.val()).length == 0) {
                alert("请输入B3B账号");
                userNo.select();
                return false;
            }
        }
        var siteName = $("#txtOemName");
        if ($.trim(siteName.val()).length == 0) {
            alert("请输入OEM名称");
            siteName.select();
            return false;
        }
        if ($.trim(siteName.val()).length > 50) {
            alert("OEM名称不能超过50位");
            siteName.select();
            return false;
        }
        var domainName = $("#txtAuthorizationDomain");
        if ($.trim(domainName.val()).length == 0) {
            alert("请输入授权域名");
            domainName.select();
            return false;
        }
        var domainNamePattern = /^[\w._]{1,40}$/;
        if (!domainNamePattern.test($.trim(domainName.val()))) {
            alert("授权域名格式错误");
            domainName.select();
            return false;
        }
        var deposit = $("#txtAuthorizationDeposit");
        if ($.trim(deposit.val()).length == 0) {
            alert("请输入授权保证金");
            deposit.select();
            return false;
        }
        var pattern = /^[1-9][0-9]{0,7}(.[0-9]{1,2})?$/;
        if (!pattern.test($.trim(deposit.val()))) {
            alert("授权保证金格式错误");
            deposit.select();
            return false;
        }
        return true;
    }
</script>
