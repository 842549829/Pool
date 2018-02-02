<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Provider.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.Provider" %>
<%@ Register src="/UserControl/Header.ascx" tagPrefix="uc" tagName="Header" %>
<%@ Register src="/UserControl/Footer.ascx" tagPrefix="uc" tagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品合作申请</title>
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
                <li onclick="window.location.href='Purchaser.aspx';"><span>采购用户注册</span></li>
                <li class="e1"><span>产品合作申请</span></li>
                <li onclick="window.location.href='Agent.aspx';"><span>出票代理申请</span></li>
            </ul>
        </div>
        <div id="enrolPenrol">
            <h3>产品合作申请　<a href="#">什么是产品合作申请？</a></h3>
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
                        用户姓名<i class="must">*</i>
                    </td>
                    <td>
                       <asp:TextBox ID="txtUserName" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblUserName" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        昵称<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPetName" runat="server" CssClass="text"></asp:TextBox>
                        <button type="button" class="btn class3" id="btnCheCkPetName">
                            验证昵称</button>
                        <span id="lblPetName" runat="server"></span>
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
                        地址<i class="must">*</i>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips-txt" id="lblAddress" runat="server"></span>
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
                    <td colspan="2">
                        <textarea class="text clauseText" id="clause"  style="width:100%;" readonly="readonly">
                                                                     B3B产品合作申请条款
                            为保证B3B机票净价结算平台的整体服务质量，保证全体客户的利益,作为B3B机票净价结算平台国内机票产品合作做出如下服务承诺：
                        </textarea><br />
                        <label><input type="checkbox" id="chkAgreedToRead" />我已经阅读并同意《B3B供应商申请条款》</label>
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
                <span class="ds" style="width: 32px; background: url('../../../../Images/prompt087169.gif') no-repeat  -127.7px 21px;">
                </span><span class="ds" style="vertical-align: middle; padding-left: 40px;" id="lblMessage" runat="server"></span>
            </div>
            <div class="btns hei" style="height:35px;">
                <button class="btn class1 closeBtn">确定</button>
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
    <script src="../../Scripts/OrganizationModule/RoleModule/Checking.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/ExtendCompanyManage/RegisterProduct.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/OrganizationModule/Register/Register.js"></script>
    <script src="../../Scripts/setting.js" type="text/javascript"></script>
</body>
</html>
