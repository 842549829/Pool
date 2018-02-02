<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeGroupByRestrictionSetting.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.IncomeGroupByRestrictionSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>对组设置收益信息</title>
    <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        <asp:Label ID="lblName" runat="server"></asp:Label>收益设置&nbsp;&nbsp;&nbsp;本设置将限制本组用户进行普通票返点扣除或特殊票加价销售的操作设置后,本组用户在平台购票时您将得到您所设置的交易利润<a
            href="IncomeGroupRestrictionSettingGlobal.aspx" class="obvious-a">全局设置</a>
    </h3>
    <div class="box-a">
        <div class="clearfix">
            <div class="user-type-left">
                用户组名称：<asp:Label ID="lblGroupName" runat="server"></asp:Label></div>
            <div class="user-type-right">
                用户数量：<asp:Label ID="lblUserCount" runat="server"></asp:Label>
                <a href="#" id="queryUsrList" runat="server" class="obvious-a">查看用户列表</a> | <a href="#"
                    id="addUser" class="obvious-a">添加用户</a></div>
        </div>
        <div class="user-type">
            用户组描述：<asp:Label ID="lblGroupDescription" runat="server"></asp:Label></div>
    </div>
    <div class="importantBox broaden">
        <p class="important">
            收益设置将限制本组用户进行普通票返点扣除或特殊票加价销售的操作设置后,本组用户在平台购票时您将得到您所设置的交易利润</p>
    </div>
    <div id="divGlobal" runat="server">
        <table class="mini-table">
            <tr>
                <td class="title">
                    扣点设置
                </td>
                <td>
                    <asp:RadioButton runat="server" ID="radQujian" Checked="true" GroupName="radKou"
                        Text="区间扣点" />
                    <asp:RadioButton runat="server" ID="radTongyi" GroupName="radKou" Text="统一返点" />
                    <div class="parent_div" id="qujian" runat="server" style="width: 660px">
                        <div class="condition">
                            <table>
                                <tbody class="quyu_table" id="rangeItems">
                                    <tr>
                                        <td class="title rangeSerial">
                                            第1组区域：
                                        </td>
                                        <td>
                                            <input style="width: 30px" class="text rangeStart" disabled="disabled" value="0" />%(<span>含</span>)
                                        </td>
                                        <td>
                                            至
                                        </td>
                                        <td>
                                            <input style="width: 30px" class="text rangeEnd" value="100" />%(含)
                                        </td>
                                        <td class="title">
                                            设置值：
                                        </td>
                                        <td>
                                            <input style="width: 30px" class="text rangeValue" value="0" />%
                                        </td>
                                        <td>
                                            <input type="button" class="btn class1 addRange" value="添加区域" />
                                        </td>
                                        <td style="width: 50px">
                                            <input type="button" class='btn class2 delRange' style='color: White; display: none;'
                                                value="删除&nbsp;&nbsp;X" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div id="tongyi" runat="server" class="condition" style="display: none">
                        <div class="importantBox broaden">
                            <div class="obvious">
                                设置后您的所有下级用户都将看到同一返点，当供应商发布的返点低于您设置的统一返点时将显示供应商的实际返点，<br />
                                当供应商发布了高于您设置的同一返点时，超出部分的返点都将会做为利润分配到您的账户。</div>
                        </div>
                        统一返点值<asp:TextBox runat="server" CssClass="text text-s" ID="txtTongyi" />%</div>
                </td>
                <td><div class="obvious1">请注意，本栏目的设置仅对普通单程政策进行扣点</div></td>
            </tr>
            <tr>
                <td class="title">
                    加价设置
                </td>
                <td>
                    每张票加价<asp:TextBox runat="server" CssClass="text text-s" ID="txtPrice" />元进行出售
                </td>
                <td><div class="obvious1">请注意，本栏目的设置仅对特殊政策生效</div></td>
            </tr>
            <tr>
                <td class="title">
                    备注
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="text" Rows="10" Columns="60" TextMode="MultiLine"
                        ID="txtRemark" />
                </td>
                <td><div class="obvious1">备注本操作的理由，无可以为空</div></td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidRanges" />
        <asp:HiddenField runat="server" ID="hidSettingId" />
    </div>
    <div >
        <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
        <input type="button" id="btnCancel" value="取消" class="btn class2" />
    </div>
    <a id="divAddUser" style="display: none; background: #1A1A1A; color: #fff; margin: 10px 0;
        padding: 8px 0; text-align: center;" data="{type:'pop',id:'divAddUserList'}">
    </a>
    <div class="layer3 hidden" id="divAddUserList">
        <h4>
            添加用户：<asp:Label ID="lblUserGroupName" runat="server"></asp:Label>
            <asp:HiddenField ID="hfdCurrentIncomeGroupId" runat="server" />
            <a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="box-a clearfix input-pad">
            <div class="fl">
                <p class="input">
                    <span class="name">开户时间：</span>
                    <asp:TextBox ID="txtBeginTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;<asp:TextBox
                        ID="txtEndTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                </p>
                <p class="input">
                    <span class="name">用户组：</span>
                    <asp:DropDownList ID="ddlIncomeGroup" runat="server">
                    </asp:DropDownList>
                </p>
            </div>
            <div class="fl">
                <p class="input">
                    <span class="name">用户名：</span>
                    <asp:TextBox ID="txtUserNo" class="text" type="text" runat="server" />
                </p>
                <p class="input">
                    <span class="name">简称：</span>
                    <asp:TextBox ID="txtAbbreviateName" class="text" runat="server" />
                </p>
            </div>
            <div class="fl">
                <input type="button" style="margin: 25px 0 0 50px;" id="btnQuery" class="btn class1"
                    value="查询" />
            </div>
        </div>
        <div class="table" id='data-list'>
            <table id="dataList">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            开户时间
                        </th>
                        <th>
                            用户名
                        </th>
                        <th>
                            公司简称
                        </th>
                        <th>
                            用户组
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <div class="total-table" id="divChoise">
                <input type="checkbox" id="chkAll" /><label for="chkAll" class="pad-l">全选</label>
                |
                <input type="checkbox" id="chkOther" /><label for="chkOther" class="pad-l">反选</label>
                <br />
            </div>
        </div>
        <div id="emptyInfo" class="box hidden">
            没有任何符合条件的查询结果</div>
        <div class="btns" id="divPagination">
        </div>
        <div class="btns" id="divBtns">
            <input type="button" id="btnUpdateRelation" class="btn class1" value="确认并提交" />
            <input type="button" id="Button1" value="取消" class="btn class2 close" />
        </div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/policyset.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/SystemSetting/PurchaseRestrictionSetting.js?2013051001"></script>
<script type="text/javascript" src="/Scripts/SystemSetting/DistributionOEMUserList.js"></script>
<script type="text/javascript">
    $(function () {
        $("#addUser").click(function () {
            var pageSize = 10;
            queryOrders();
            $("#btnQuery").click(function () {
                if ($("#dropPageSize").size() > 0) {
                    pageSize = $("#dropPageSize option:selected").val();
                }
                queryOrders(1, pageSize);
            });
            $("#divAddUser").click();
        });
        $("input[type='radio'][name='radKou']").click(function () {
            if ($(this).attr("Id") == "radQujian") {
                $("#qujian").show();
                $("#tongyi").hide();
            } else {
                $("#qujian").hide();
                $("#tongyi").show();
            }
        });
        $("#btnSave").click(function () {
            return vail();
        });
        $("#btnCancel").click(function () {
            window.location.href = 'IncomeGroupList.aspx?Search=Back';
        });
    });
    function vail() {
        var reg = /^[0-9]{1,10}(\.[0-9])?$/;
        if ($("#radTongyi").is(":checked") && $("#txtTongyi").val() == "") {
            alert("统一扣点不能为空！");
            return false;
        }
        if ($("#radTongyi").is(":checked") && !reg.test($("#txtTongyi").val())) {
            alert("统一扣点只能为整数或一位小数！");
            return false;
        }
        if ($("#txtPrice").val() == "") {
            alert("每张票加价不能为空！");
            return false;
        }
        reg = /^[0-9]{1,10}?$/;
        if (!reg.test($("#txtPrice").val())) {
            alert("每张票加价只能为整数！");
            return false;
        }
        return true;
    }
</script>
