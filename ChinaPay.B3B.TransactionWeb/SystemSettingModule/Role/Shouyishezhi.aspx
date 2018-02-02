<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shouyishezhi.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.Shouyishezhi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收益设置</title>
</head>
<link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
<body>
    <form id="form1" runat="server">
    <div>
        <h3 class="titleBg">
            <asp:Label ID="lblName" runat="server"></asp:Label>收益设置&nbsp;&nbsp;&nbsp;本设置将限制本组用户进行普通票返点扣除或特殊票加价销售的操作设置后,本组用户在平台购票时您将得到您所设置的交易利润<a
                href="ShouyishezhiGlobal.aspx" class="obvious-a">全局设置</a>
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
        <div class="clearfix">
            <div class="fl" style="width: 510px;">
                <ul class="tag-nav clearfix">
                    <li class="cur" val="1"><a href="#">本公司政策</a></li>
                    <li val="0"><a href="#">同行政策</a></li>
                </ul>
                <div class="tag-content" style="padding: 10px;">
                    <div class="tag-pane">
                        <div class="clearfix">
                            <div class="fl" style="width: 500px;">
                                <div class="simple-box clearfix">
                                    <label class="title">
                                        限制航空公司：</label>
                                    <div class="simple-content check-box-list">
                                        <div id="hangkonggongsi" runat="server">
                                        </div>
                                        <span class="obvious">
                                            <input type="radio" id="chkquanxuan" name="gongsixuanzhe" value="0" /><label for="chkquanxuan">全选</label></span>
                                        <span class="obvious">
                                            <input type="radio" id="chkfanxuan" name="gongsixuanzhe" value="1" /><label for="chkfanxuan">反选</label></span>
                                    </div>
                                </div>
                                <div class="simple-box">
                                    <label class="title">
                                        扣点/返点类型：</label>
                                    <div class="simple-content">
                                        <p>
                                            <span class="pad-r">
                                                <label>
                                                    <input type="radio" checked="checked" id="radqujian" name="fandiankoudian" value="1" />区间扣点</label></span>
                                            <span class="pad-l pad-r">
                                                <label>
                                                    <input type="radio" id="radfandian" name="fandiankoudian" value="2" />统一返点</label></span>
                                        </p>
                                        <p class="obvious1">
                                            请注意，本栏目的设置仅对普通单程政策进行区间扣点/统一返点</p>
                                    </div>
                                </div>
                                <div class="simple-box qujiankoudian">
                                    <label class="title">
                                        区间扣点设置：</label>
                                    <div class="simple-content">
                                        <table>
                                            <tbody class="quyu_table" id="rangeItems">
                                                <tr>
                                                    <td class='rangeSerial'>
                                                        返点在
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeStart' disabled='disabled' value='0' />%(<span>含</span>)
                                                    </td>
                                                    <td>
                                                        至
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeEnd' value='100' />%(含)
                                                    </td>
                                                    <td>
                                                        值
                                                    </td>
                                                    <td>
                                                        <input style='width: 25px' class='text rangeValue' value='0' />%
                                                    </td>
                                                    <td>
                                                        <a class='addRange add'>+</a>
                                                    </td>
                                                    <td>
                                                        <a class='delRange reduce' style='display: none;'>-</a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <p>
                                            <span class="obvious1">你最多可用设置5个返点区间扣点，若多个冲突时，则取扣点最低的区间设置</span></p>
                                    </div>
                                </div>
                                <div class="simple-box tongyifandian" style="display: none;">
                                    <label class="title">
                                        统一返点设置：</label>
                                    <div class="simple-content">
                                        <p>
                                            统一返点值
                                            <input type="text" class="text text-s" id="tongyifandian" />%
                                        </p>
                                    </div>
                                </div>
                                <div class="simple-box">
                                    <label class="title">
                                        加价设置：</label>
                                    <div class="simple-content">
                                        <p>
                                            每张票加价
                                            <input type="text" class="text text-s" id="price" />
                                            元进行出售
                                             <span class="obvious1">请注意，本栏目只对特殊政策进行加价</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="simple-box last">
                                    <a class="simple-add-btn btnAdd">添加到右侧分组</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin-left: 520px; padding-top: 30px;">
                <div class="accumulate-box xianshi">
                    <h4>
                        双击即可编辑您添加的分组</h4>
                        <br />
                    <h5>
                        本公司政策设置</h5>
                    <div id="bengongsi" runat="server">
                    </div>
                        <br />
                    <h5>
                        同行政策设置</h5>
                    <div id="tonghang" runat="server">
                    </div>
                </div>
                <div class="simple-box last">
                    <label class="title">
                        备注：</label>
                    <div class="simple-content">
                        <textarea class="text" style="width: 300px; height: 80px;" id="remark" runat="server"></textarea>
                        <p class="obvious1">
                            备注本次操作理由，若无可留空</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="btns">
            <input type="button" id="btnSave" class="btn class1" value="提交" />
            <input type="button" id="btnReturn" class="btn class2" value="取消" />
        </div>
    </div>
    
    <a id="divAddUser" style="display: none; background: #1A1A1A; color: #fff;
        margin: 10px 0; padding: 8px 0; text-align: center;" data="{type:'pop',id:'divAddUserList'}">
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
                <label id="chkOther" class="pad-l">
                    反选</label>
                <br />
            </div>
        </div>
        <div id="emptyInfo" class="box hidden">
            没有任何符合条件的查询结果</div>
        <div class="btns" id="divPagination">
        </div>
        <div class="btns" id="divBtns">
            <input type="button" id="btnUpdateRelation" class="btn class1" value="确认并提交" />
            <input type="button" id="btnCancel" value="取消" class="btn class2 close" />
        </div>
    </div>
    <input type="hidden" id="hidRanges" />
    <asp:HiddenField runat="server" ID="hidShouyishezhi" />
    <asp:HiddenField runat="server" ID="hidincomgroupId" />
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script src="/Scripts/SystemSetting/shouyishezhi.js?201305" type="text/javascript"></script>
<script src="/Scripts/SystemSetting/IncomeSet.js?201305" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/SystemSetting/DistributionOEMUserList.js?20130521"></script>
<script type="text/javascript" src="/Scripts/SystemSetting/PurchaseRestrictionSetting.js?20130521"></script>
<script type="text/javascript">
    $(function () {
        $("#btnReturn").click(function () {
            window.location.href = 'IncomeGroupList.aspx';
        });
        $("#addUser").click(function () {
            $("#lblUserGroupName").text($("#lblGroupName").text());
            for (var i = 0; i < $("#ddlIncomeGroup option").length; i++) {
                if ($("#ddlIncomeGroup option").eq(i).attr("value") == $("#hfdCurrentIncomeGroupId").val()) {
                    $("#ddlIncomeGroup option").eq(i).remove();
                }
            }
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
        $("#btnSave").click(function () {
            if ($.trim($("#hidShouyishezhi").val()) == "") {
                alert("右侧还没添加过任何收益信息，请先加。");
                return;
            }
            var actionUrl = "/SystemSettingHandlers/IncomeLimitGroup.ashx/InsertIncomeLimitGroup";
            sendPostRequest(actionUrl, JSON.stringify({ "incomgroupId": $("#hidincomgroupId").val(), "str": $("#hidShouyishezhi").val(), "remark": $("#remark").val() }), function (e) {
                if (e == true) {
                    alert("设置成功");
                    window.location.href = 'IncomeGroupList.aspx';
                } else {
                    alert("设置失败");
                }
            }, function (e) {
                alert(e.responseText);
            });
        });

    });
</script>
