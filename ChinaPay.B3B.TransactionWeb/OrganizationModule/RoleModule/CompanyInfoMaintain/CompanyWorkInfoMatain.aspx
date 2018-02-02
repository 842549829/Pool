<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyWorkInfoMatain.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain.CompanyWorkInfoMatain" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工作信息维护</title>
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="smallbd1">
        <ul class="navType1 clearfix" id="sel" runat="server">
            <li><a href="javascript:;" class="navType1Selected" id="officeworkinfo" runat="server"
                cus="OFFICE号设置[最多可设置4个]">OFFICE号设置</a></li>
            <li><a href="javascript:" id="customNumber" runat="server" cus="自定义编号设置">自定义编号</a></li>
            <li><a href="javascript:;" id="fuzerenwoekinfo" runat="server" cus="负责人设置[退废改、出票]">负责人</a></li>
            <li><a href="javascript:;" id="timeworkinfo" runat="server" cus="工作/废票时间设置">工作/废票时间</a></li>
            <li><a href="javascript:;" id="companyworkinfo" runat="server" cus="公司工作设置">工作设置</a></li>
            <li><a href="javascript:;" id="companyDrawerCondition" runat="server" cus="出票条件政策备注设置">
                出票/备注</a></li>
        </ul>
        <div id="companyWorkInfoMatain" runat="server">
            <h3 class="titleBg">
                工作信息维护 -<label id="navTip" runat="server">OFFICE号设置[最多可设置4个]</label></h3>
            <div id="divProviderOffice" class="officeworkinfo" runat="server">
                <div class="table" runat="server" id="divOffice">
                </div>
                <div class="officAdd">
                    <span class="name">新增</span>
                    <asp:TextBox CssClass="text text-s" runat="server" ID="txtOffice"></asp:TextBox>
                    <input type="checkbox" value="" id="chkAuthorization" /><label for="chkAuthorization">编码需要授权出票</label>
                    <input type="button" id="btnSaveOffice" class="btn class1" value="保&nbsp;&nbsp;存" />
                </div>
            </div>
            <div id="divCustomNumber" class="customNumber">
                <div class="table" runat="server" id="CustomNumbers">
                </div>
                <div class="officAdd">
                    <span class="name">编号：</span>
                    <asp:TextBox CssClass="text text-s" runat="server" ID="txtCustomNumber"></asp:TextBox><br />
                    <br />
                    <span class="name">描述：</span>
                    <textarea class="text" id="textDescribe" style="vertical-align: top; height: 50px;"></textarea>
                    <br />
                    <br />
                    <input type="button" id="btnSvaeCustom" class="btn class1" value="保&nbsp;&nbsp;存"
                        style="margin-left: 130px" />
                </div>
            </div>
            <div class="table fuzerenwoekinfo" runat="server" id="divProviderPerson">
                <table class="maintainTable">
                    <colgroup>
                        <col class='w25' />
                        <col class='w25' />
                        <col class='w25' />
                        <col class='w25' />
                    </colgroup>
                    <tr>
                        <th>
                            负责方向
                        </th>
                        <th>
                            负责人
                        </th>
                        <th>
                            手机
                        </th>
                        <th>
                            QQ
                        </th>
                    </tr>
                    <tr>
                        <td>
                            出票
                        </td>
                        <td>
                            <asp:TextBox ID="txtDrawerPerson" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDrawerCellPhone" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDrawerQQ" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            退票
                        </td>
                        <td>
                            <asp:TextBox ID="txtRetreatPerson" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRetreatCellPhone" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRetreatQQ" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            废票
                        </td>
                        <td>
                            <asp:TextBox ID="txtWastePerson" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWasteCellPhone" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWasteQQ" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            改期
                        </td>
                        <td>
                            <asp:TextBox ID="txtReschedulingPerson" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReschedulingCellPhoen" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReschedulingQQ" runat="server" CssClass="text"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="btns maintainBtns">
                    <br />
                    <input type="button" id="btnSaveManager" class="btn class1" value="保&nbsp;&nbsp;存" />
                </div>
            </div>
            <div id="workTime" class="clearfix timeworkinfo" runat="server">
                <div id="workTimeSet" class="maintainLeft" runat="server">
                    <br />
                    <h4>
                        工作时间设置</h4>
                    <table class="maintainTable1">
                        <tr>
                            <td class="title">
                                周一至周五：
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkdayWorkStart" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate: '#F{$dp.$D(\'txtWorkdayWorkEnd\')}'})"></asp:TextBox>
                                至
                                <asp:TextBox ID="txtWorkdayWorkEnd" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate: '#F{$dp.$D(\'txtWorkdayWorkStart\')}'})"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                周六、周日：
                            </td>
                            <td>
                                <asp:TextBox ID="txtRestdayWorkStart" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtRestdayWorkEnd\')}'})"></asp:TextBox>
                                至
                                <asp:TextBox ID="txtRestdayWorkEnd" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtRestdayWorkStart\')}'})"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divRefundTime" class="maintainRight" runat="server">
                    <br />
                    <h4>
                        废票时间设置</h4>
                    <table class="maintainTable1">
                        <tr>
                            <td class="title">
                                周一至周五：
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkdayRefundStart" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtWorkdayRefundEnd\')}'})"></asp:TextBox>
                                至
                                <asp:TextBox ID="txtWorkdayRefundEnd" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtWorkdayRefundStart\')}'})"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                周六、周日：
                            </td>
                            <td>
                                <asp:TextBox ID="txtRestdayRefundStart" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtRestdayRefundEnd\')}'})"></asp:TextBox>
                                至
                                <asp:TextBox ID="txtRestdayRefundEnd" runat="server" CssClass="datepicker datefrom btn class3"
                                    onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtRestdayRefundStart\')}'})"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="btns  maintainCenter">
                    <input type="button" id="btnSaveRefundTime" value="保&nbsp;&nbsp;存" class="btn class1" />
                </div>
            </div>
            <div id="divWorkInfo" class="companyworkinfo maintainCenter" runat="server">
                <table class="tableType1 textLeft">
                    <tbody>
                        <tr>
                            <th class="title">
                                默认出发城市
                            </th>
                            <td>
                                <uc:Airport ID="Departure" runat="server" />
                            </td>
                            <th class="title">
                                默认到达城市
                            </th>
                            <td>
                                <uc:Airport ID="Arrival" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                    <tbody id="exceptPurchase" runat="server">
                        <tr>
                            <th class="title">
                                默认出票OFFICE
                            </th>
                            <td>
                                <select id="ddlOffice">
                                </select>
                            </td>
                            <th class="title">
                                退款财务审核
                            </th>
                            <td>
                                <asp:CheckBox ID="chkRefundFinancialAudit" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <th class="title">
                                政策自定义编号需按员工授权
                            </th>
                            <td colspan="3">
                                <asp:CheckBox ID="chkEmpowermentOffice" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                    <tr id="supplierWorkInfo" runat="server">
                        <th class="title">
                            可提供资源航空公司
                        </th>
                        <td colspan="3">
                            <asp:CheckBoxList ID="chklAirline" runat="server" RepeatColumns="12" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" Enabled="false">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tbody id="providerWorkInfo" runat="server">
                        <tr>
                            <th class="title">
                                成人可出票航空公司
                            </th>
                            <td colspan="3">
                                <asp:CheckBoxList ID="chklAirlines" runat="server" RepeatColumns="15" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" Enabled="false">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <%--<tr>
                            <th class="title">
                                默认返佣
                            </th>
                            <td colspan="3">
                                <input type="checkbox" id="chkDefaultCommission" runat="server" /><label for="chkDefaultCommission">默认返点</label><br />
                                <span id="spanDefaultCommission">
                                    <label>
                                        默认返点:
                                    </label>
                                    <asp:TextBox ID="txtDefaultCommission" runat="server" CssClass="text text-s"></asp:TextBox>
                                    <i class="must">%</i><br />
                                    <i class="must">允许内部及下级采购的航空：</i><br />
                                    <div id="divDefaultAirlines" runat="server">
                                    </div>
                                </span>
                            </td>
                        </tr>--%>
                        <tr>
                            <th class="title">
                                儿童返佣
                            </th>
                            <td colspan="3">
                                <input type="checkbox" id="chkChildern" runat="server" /><label for="chkChildern">儿童返点</label><br />
                                <span id="lblChildern">
                                    <label>
                                        儿童返点:
                                    </label>
                                    <asp:TextBox ID="txtCholdrenDeduction" runat="server" CssClass="text text-s"></asp:TextBox>
                                    <i class="must">%</i><br />
                                    <i class="must">可出儿童票的航空公司：</i><br />
                                    <div id="chklCholdrenDeduction" runat="server">
                                    </div>
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="btns">
                    <input type="button" id="btnProvider" value="保&nbsp;&nbsp;存" class="btn class1" />
                </div>
            </div>
            <div id="divDrawerCondition" class="companyDrawerCondition">
                <div class="table" runat="server" id="tbDrawerCondition">
                </div>
                <div class="btns">
                    <input type="button" id="btnDrawerConditionAdd" class="btn class1 txt-c" value="添加" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfdDreawId" runat="server" />
    <asp:HiddenField ID="hfdCompanyId" runat="server" />
    <asp:HiddenField ID="hfdCompanyType" runat="server" />
    <asp:HiddenField ID="hfdEmpowermentOffice" runat="server" />
    </form>
    <a id="aDrawerCondition" style="display: none;" data="{type:'pop',id:'divaDrawerCondition'}">
    </a>
    <div id="divaDrawerCondition" class="from layer4" style="display: none; width: 550px;">
        <h4>
            添加出票条件或政策备注信息<a href="javascript:void(0)" class="close">关闭</a></h4>
        <table>
            <colgroup>
                <col class="w20" />
            </colgroup>
            <tr>
                <td>
                    类型：
                </td>
                <td>
                    <input type="radio" id="radChuPiao" value="0" name="radcondition" checked="checked" /><label
                        for="radChuPiao">出票条件</label><label class="obvious1">出票条件为采购可见，请准确填写您的出票要约。</label><br />
                    <input type="radio" id="radPReamek" value="1" name="radcondition" /><label for="radPReamek">政策备注</label><label
                        class="obvious1">政策备注为内部工作人员可见，作备忘使用。</label>
                </td>
            </tr>
            <tr>
                <td>
                    内容：
                </td>
                <td>
                    <textarea class="text" id="txtDreawContext" style="vertical-align: top; height: 80px;
                        width: 300px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <span class="obvious1">您已输入<span id="spanSmsNum" class="obvious font-b">0</span>字。最多能输入200个字</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="txt-c">
                    <input type="button" id="btnDrawSave" class="btn class1" value="保&nbsp;&nbsp;存" />
                    <input type="button" id="btnDrawCancel" class="btn class2 close" value="取&nbsp;&nbsp;消" />
                </td>
            </tr>
        </table>
    </div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none; width: 550px;">
        <h4>
            将自定义编号分配给员工
        </h4>
        <div class="layer3Content">
            将自定义编号： <span class="obvious" id="lblOfficeNumber" code="">LKE696</span> 分配给以下员工：
            <a href="../../CommonContent/EmployeeInfoPage/AddEmployee.aspx" class="layerlink">员工不全？添加员工</a>
        </div>
        <div class="layer3Form clearfix">
            <span class="allPick">
                <input type="checkbox" id="chkAll" />
                <label for="chkAll">
                    全选</label>
            </span>
            <div id="divEmoloyee">
            </div>
        </div>
        <div class="layer3Btns">
            <a href="#" class="layerbtn btn1 fl" id="btnEmployeeSave">确定</a> <a href="#" class="layerbtn btn2 fr close">
                取消</a>
        </div>
    </div>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?20130503" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20130514" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/CompanyOption.js?20130514" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/CustomNumber.js?20130514" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/CompanyWorkInfoMatain.js?20130522"
    type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/OfficeNumberManage.js?20130514"
    type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnDrawerConditionAdd").click(function () {
            $("#hfdDreawId").val("");
            $("#txtDreaw").val("");
            $("#txtDreawContext").val("");
            $("#aDrawerCondition").click();
        });
        $("#txtDreawContext").keyup(function () {
            $("#spanSmsNum").html($.trim($("#txtDreawContext").val()).length);
        });
        $("#btnDrawSave").click(function () {

            if ($.trim($("#txtDreawContext").val()) == "") {
                alert("内容不能为空！请输入。");
                return;
            }
            if ($.trim($("#txtDreawContext").val()).length >= 200) {
                alert("内容不能大于200个字符！请输入。");
                return;
            }
            var targetUrl = ""
            var parameters = "";
            if ($("#hfdDreawId").val() == "") {
                targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/InsertDrawContext";
                parameters = JSON.stringify({ "title": "", "type": $("input[type='radio'][name='radcondition']:checked").val(), "context": $("#txtDreawContext").val() });
            } else {
                targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/UpdateDrawContext";
                parameters = JSON.stringify({ "id": $("#hfdDreawId").val(), "type": $("input[type='radio'][name='radcondition']:checked").val(), "title": "", "context": $("#txtDreawContext").val() });
            }
            sendPostRequest(targetUrl, parameters, function (e) {
                if (e == true) {
                    alert("保存成功");
                    $("#hfdDreawId").val("");
                    $("#txtDreaw").val("");
                    $("#txtDreawContext").val("");
                    $(".close").click();
                    $("#spanSmsNum").html("0");
                    //$("#btnDrawCancel").css("display", "none");
                } else {
                    alert("保存失败，请重试");
                }
            }, function (e) {
                alert(JSON.parse(e.responseText));
            });
            BindDrawdition();
            $(".navType1Selected").focus();
        });
        $("#btnDrawCancel").click(function () {
            $(".update").css("display", "");
            $("#hfdDreawId").val("");
            $("#txtDreaw").val("");
            $("#txtDreawContext").val("");
            BindDrawdition();
            $(".navType1Selected").focus();
        });
        $(".close").click(function () {
            $(".update").css("display", "");
            $("#hfdDreawId").val("");
            $("#txtDreaw").val("");
            $("#txtDreawContext").val("");
            BindDrawdition();
            $(".navType1Selected").focus();
        });
    });
    function UpdateDition(id, type, title, context) {
        $("#hfdDreawId").val(id);
        //$("#txtDreaw").val(title);
        $("#txtDreawContext").val(context);
        //$("#btnDrawCancel").css("display", "");
        $("input[type='radio'][name='radcondition']").removeAttr("checked");
        if (type == 0) {
            $("#radChuPiao").attr("checked", "checked");
        } else {
            $("#radPReamek").attr("checked", "checked");
        }
        $("#aDrawerCondition").click();
        $("#txtDreawContext").keyup();
    }
    function DelDition(id) {
        if (confirm("是否删除？")) {
            var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/DelDrawbyId";
            var parameters = JSON.stringify({ "id": id });
            sendPostRequest(targetUrl, parameters, function (e) {
                if (e == true) {
                    alert("删除成功");
                } else {
                    alert("删除失败，请重试");
                }
            }, function (e) {
                alert(JSON.parse(e.responseText));
            });
            BindDrawdition();
            $(".navType1Selected").focus();
        }
    }
</script>
