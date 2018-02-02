<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agent.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Register.Agent" %>
<%@ Register src="/UserControl/Header.ascx" tagPrefix="uc" tagName="Header" %>
<%@ Register src="/UserControl/Footer.ascx" tagPrefix="uc" tagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出票注册</title>
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
                <li onclick="window.location.href='Provider.aspx';"><span>产品合作申请</span></li>
                <li class="e1"><span>出票代理申请</span></li>
            </ul>
        </div>
        <div id="enrolPenrol">
            <h3>供应代理申请　<a href="#">什么是供应代理？</a></h3>
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
                        <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="text"  TextMode="Password"
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
                    <td colspan="4" style="height:5px;"></td>
                </tr>
                <tr>
                    <td class="title">航协经营批准号<i class="must">*</i></td>
                    <td>
                        <asp:TextBox ID="txtIATABusinessApprovalNumber" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblIATABusinessApprovalNumber"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">IATA号<i class="must">*</i></td>
                    <td>
                        <asp:TextBox ID="txtIATANumber" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblTATANumber"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">OFFICE号<i class="must">*</i></td>
                    <td>
                        <asp:TextBox ID="txtOFFICENumber" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblOFFICENumber"></span>
                    </td>
                </tr>
               <tr>
                    <td class="title">中航协担保金<i class="must">*</i></td>
                    <td>
                        <asp:TextBox ID="txtCaticAssociationSuch" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblCaticAssociationSuch"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">业务类型<i class="must">*</i></td>
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
                    <td colspan="2">
                        <textarea readonly="readonly" class="text clauseText" id="clause"  style="width:100%;">
                                                                               B3B供应商申请条款
                            为保证B3B机票净价结算平台的整体服务质量，保证全体客户的利益,作为B3B机票净价结算平台国内机票供应商做出如下服务承诺：
                            一、人员配备：我公司提供必要的人员配置，在B3B机票净价结算平台进行异地出票服务。 二、运价维护
                            1.我方在收到航空公司政策后，第一时间按平台定义的格式进行发布，并保证所发布政策的真实准确性。如有发布失误(如代理费错误，航线限制错误，舱位等级错误等)或航空公司政策变动未及时更新，所造成的损失由我方承担；
                            2.为保证我方收益以及平台的整体价格优势，我方会积极配合B3B平台根据市场需要调整政策。
                            三、出票处理 
                            1.在我方设定的出票工作时间内，保证采购方支付后3分钟内受理订单，保证采购方支付后10分钟内完成平台所有出票流程。若超过10分钟未出票导致航空公司NO位，我方负责并承担相应赔偿损失； 注：订单受理时间计算以供应商在平台上点击“开始处理”的时间为准，订单出票时间以供应商在平台上成功执行“更新为已出票”操作为准。
                            2.我方在收到订单后第一时间在平台上更新为“开始处理”，完成出票后立即在平台上更新为已出票，以便采购方能及时了解订单处理状态。对于B2B票证，我方负责在更新订单状态之前将票号贴入PNR以保证平台订单能正确读取票号；
                            3.我方将严格按采购方所选政策的票证类型（B2B/BSP）出票，不会私自更改票证类型。若B2B订单因航空公司B2B网站问题造成无法顺利出票，我方将及时换出BSP票证以保证出票速度；若BSP票证因断号等因素无法顺利出票，我方将与平台客服联系，在得到客户允许后立即换开B2B票证；
                            4.我方在处理订单时认真审核票价、舱位等信息，确认PNR与订单信息一致，保证正确出票，若因我方审核失误造成相应损失由我方负责；
                            5.出票过程中如遇到问题（采购方未输入证件号码、NO位、B2B无法入库等相关情况），我方将及时通知平台客服,并积极配合平台客服进行解决。
                            6.我方如有异常情况无法出票，将在第一时间挂起自己的政策并通知平台客服，对挂起前已收到的出票订单，我方仍将想办法进行妥善处理，以保证客户满意，如造成经济损失由我方承担。
                            7.我方保证订单处理的准确性，不会遗漏、误更新订单。我方将检查关联项状态是HK状态，并保证所提供的票证是有效的票证。
                            8.对于重复支付或采购方取消交易的订单，我方在核实情况后将及时退回票款。
                            9.已完成出票的PNR，我方不会随意进行操作（挂起、取消、作废重出等），如确有需要，我方将联系平台客服并得到客户同意。如因我方员工对客票进行误操作（误取消、误挂、误退废等）等情况造成客人无法乘机，我方承担实际损失（含采购方本张订单机票代理费的利润）。 
                            10.工作时间内，如采购方有暂时挂起客票的需求，我方将积极提供协助，并保证能随时解挂。 
                            10.我方不会随意转换PNR编码,按相应政策发布规则操作订单，若擅自更改,由此所造成的损失由我方承担.若该订单在政策发布时注明需更换PNR，我方负责对航空公司航班延误、取消、航段变动等信息通知平台客服，确保旅客正常登机。 
                            11.我方保证不会在未经平台与采购方允许情况下擅自将平台订单换开航空公司免票、里程积分免票等，若发生此问题，我方视为恶意操作将追纠相应责任与赔偿。 
                            12.如客人办理登机手续时遇到异常情况，如航班超售等，我方将尽力协助解决。如果是我方责任，我方将相应承担。 
                            四、客票变更处理 
                            1.我方承诺采购方提交变更请求后5分钟内受理订单，10分钟内完成处理。我方保证按订单要求进行处理，在变更后检查变更信息（PNR中行程、承运人、舱位、改签时间、身份证号码、关联项、用DETR：TN/票号指令检查航段时间按是否已由OPEN状态改为采购方指定的旅行日期和起飞时间），检查无误后在平台上执行“已完成变更”操作。如确实无法变更，我方也将在10分钟内通知平台客服进行相应处理。如因未及时操作航班变更订单，导致无要变更舱位或航班无座位等情况，我方做及时补救，所造成的经济损失由我方承担。 注：受理时间以在平台更新状态的时间为准
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
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="btn class1" onclick="btnSubmit_Click" />
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
            <span class="ds" style="width: 32px;">
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
    <script src="../../Scripts/setting.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/form-ui.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/Address.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/ExtendCompanyManage/RegisterBuy.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/RoleModule/Checking.js" type="text/javascript"></script>
    <script src="../../Scripts/OrganizationModule/Register/Register.js" type="text/javascript"></script>
</body>
</html>
