<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Purchaser.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.Purchaser" %>
<%@ Register src="/UserControl/Header.ascx" tagPrefix="uc" tagName="Header" %>
<%@ Register src="/UserControl/Footer.ascx" tagPrefix="uc" tagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>采购用户注册</title>
    <link href="../../Styles/core.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/page.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/form.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form action="#" id="form1" runat="server">
    <div class="wrap">
    <uc:Header runat="server" ID="ucHeader"></uc:Header>
    <div id="bd">
        <div id="enrol">
            <ul>
                <li class="e1"><span>采购用户注册</span></li>
                <li onclick="window.location.href='Provider.aspx';"><span>产品合作申请</span></li>
                <li onclick="window.location.href='Agent.aspx';"><span>出票代理申请</span></li>
            </ul>
        </div>
        <div id="enrolPenrol">
            <h3>采购用户注册　<a href="#">什么是采购用户？</a></h3>
            <p class="obvious">请填写用户申请资料,*为必填</p>
        </div> 
        <div class="form">
               <table>
                <colgroup>
                    <col class="w20" />
                    <col class="w80" />
                </colgroup>
                <tr>
                    <td colspan="2">
                        <h3>
                            账户信息</h3>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        用户名:<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="text"></asp:TextBox>
                        <button type="button" class="btn class3" id="btnCheCkAccounNo">
                            验证用户名</button>
                        <span class="tips-txt" runat="server" id="lblAccountNo"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        密码<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassWord" runat="server" CssClass="text"  TextMode="Password"
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
                            验证公司名称</button>
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
                            验证公司简称</button>
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
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td class="title">业务类型</td>
                    <td>
                       <asp:CheckBoxList ID="chklBusinessType" runat="server"  RepeatColumns="8" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="title">您是否有固定客户</td>
                    <td>
                        <asp:RadioButtonList ID="rdolHasClientType" runat="server" RepeatLayout="Flow"  RepeatDirection="Horizontal"  RepeatColumns="8"></asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="title">从哪知道我们</td>
                    <td>
                        <asp:RadioButtonList ID="rdolHowToKnow" runat="server" RepeatLayout="Flow"  RepeatDirection="Horizontal"  RepeatColumns="8"></asp:RadioButtonList>
                        <label id="lblMarket" style="display:none">销售推荐号<asp:TextBox ID="txtMarket" runat="server" CssClass="text"></asp:TextBox><span></span></label>
                    </td>
                </tr>
                <tr>
                    <td class="title">验证码</td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="text"></asp:TextBox>
                         <img id="iVerifyCode" alt="验证码不区分大小写" title="验证码不区分大小写" src="../../VerifyCode.ashx?verifyType=agent" style="display:inline-block; vertical-align:middle;"/>
                         <a href="javascript:VerifyCode('iVerifyCode','agent')">看不清楚,换一张</a>
                        <span id="lblCode"></span>
                    </td>
                </tr>        
                <tr class="btns">
                    <td colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="btn class1" 
                            onclick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
             <asp:HiddenField ID="hidAddress" runat="server" />
        </div>
    </div>
     <div class="layers" id="test_layer">
            <h2 class="hei obvious">
                错误信息<a href="javascript:void(0)"  class="qclose closeBtn" >&times;</a>
            </h2>
            <div>
                <span class="ds" style="width: 32px; background: url('../../Images/prompt087169.gif') no-repeat  -127.7px 21px;">
                </span><span class="ds" style="vertical-align: middle; padding-left: 40px;" id="lblMessage" runat="server"></span>
            </div>
            <div class="btns hei" style="height:35px;">
                <button class="btn class1 closeBtn">
                    确定</button>
            </div>
     </div>
     <div class="fixed"></div>
    <uc:Footer runat="server" id="ucFooter"></uc:Footer>
    </div>
    </form>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/form-ui.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/Address.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/ExtendCompanyManage/RegisterBuy.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/Checking.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/Register/Register.js" type="text/javascript"></script>
    <script src="../../Scripts/setting.js" type="text/javascript"></script>
</body>
</html>

