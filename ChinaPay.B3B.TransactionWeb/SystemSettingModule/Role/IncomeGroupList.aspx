<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeGroupList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.IncomeGroupList" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link rel="stylesheet" href="/Styles/skin.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .layer3 #data-list th, .layer3 #data-list td
        {
            min-height:25px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        用户组管理</h3>
    <div class="table mar">
        <asp:Repeater ID="rptIncomeGroup" runat="server">
            <HeaderTemplate>
                <table class="mar-t" id="incomeGroupList">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th class="w20">
                                用户组名称
                            </th>
                            <th class="w12">
                                用户数量
                            </th>
                            <th class="w12">
                                创建时间
                            </th>
                            <th class="w35">
                                用户组描述
                            </th>
                            <th class="w30">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <input type="checkbox" id='<%#Eval("Id") %>' />
                    </td>
                    <td>
                        <span>
                            <%#Eval("Name")%></span>
                    </td>
                    <td>
                        <%#Eval("UserCount")%>
                    </td>
                    <td>
                        <%#Eval("CreateTime")%>
                    </td>
                    <td>
                        <span class="Description">
                            <%#Eval("Description")%></span>
                    </td>
                    <td>
                        <%#Eval("incomeGlobal")%>
                        <%#Eval("purchaseRestriction")%>
                         <a href='DistributionOEMUserList.aspx?IncomeGroupId=<%#Eval("Id") %>'>用户列表</a>
                        |<a href="#" class="addUser" groupname='<%#Eval("Name") %>' grpupid='<%#Eval("Id") %>'>添加用户</a>
                        |<a href="#" changeid='<%#Eval("Id") %>' groupname='<%#Eval("Name")%>' groupdescription='<%#Eval("Description") %>'
                            class="change">编辑分组</a> |<a href="#" class="delete" deleteid='<%#Eval("Id") %>'>删除本组</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="btns">
            <uc:Pager ID="pager" runat="server" Visible="false" />
        </div>
        <a href="javascript:;" id="imgAdd" class="add_ico_btn">
            <img src="../../Images/add.png" />添加新用户组</a>
        <div>
            <div class="total-table" runat="server" id="showOrHide">
                <input type="checkbox" id="check_All" onclick="checkAll(this);" /><label for="check_All"
                    class="pad-l">全选</label>&nbsp;&nbsp;&nbsp;您删除用户组不会对用户产生影响，用户组被删除后该组内用户将自动变更为“未分组”状态<br />
                <input type="button" id="btnSubmit" value="删除所选用户组" class="btn class1" />
                <br />
            </div>
        </div>
    </div>
    <a id="divOption" style="display: none" data="{type:'pop',id:'divCopy'}"></a>
    <div class="layer3 hidden" id="divCopy">
        <h4>
            添加/修改用户组<a href="#" class="close">关闭</a></h4>
        <div class="handle-box">
            <p>
                用户组名称：<asp:TextBox ID="txtIncomeGroupName" runat="server" CssClass="text selectarea"></asp:TextBox>请输入您自定义的用户组名称</p>
            <p>
                用户组描述：<asp:TextBox ID="txtIncomeGroupDescription" runat="server" CssClass="text selectarea1"></asp:TextBox>
                为您创建的用户组填写备注信息
            </p>
        </div>
        <div class="btns">
            <input type="button" id="btnSubmitIncomeGroup" class="btn class1" value="提交" />
            <input type="button" class="btn class2 close" value="取消" />
        </div>
    </div>
    <asp:HiddenField ID="hfdGroupId" runat="server" />
    <a id="divDeleteSingle" style="display: none; background: #1A1A1A; color: #fff; margin: 10px 0;
        padding: 8px 0; text-align: center;" data="{type:'pop',id:'divDeleteGroupSingle'}">
    </a>
    <div class="layer3 hidden" id="divDeleteGroupSingle" style="width: 500px;">
        <h4>
            操作提示<a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="con">
            <p>
                <i class="i-icon-warning"></i>您即将删除一个用户组，请确认您的操作：</p>
            <table class="block-table">
                <tr>
                    <th>
                        用户组名称
                    </th>
                    <td>
                        <asp:Label ID="lblGroupName" runat="server"></asp:Label>
                    </td>
                    <th>
                        用户数量
                    </th>
                    <td>
                        <asp:Label ID="lblUserCount" runat="server"></asp:Label><a href="#" id="linkUserList">用户列表</a>
                    </td>
                </tr>
                <tr>
                    <th>
                        用户组描述
                    </th>
                    <td colspan="3">
                        <asp:Label ID="lblGroupDescription" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="btns">
                <input type="button" id="btnDeleteSingle" class="btn class1" value="确认删除" />
                <input type="button" class="btn class2 close" value="取消" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfdSingleGroupId" runat="server" />
    <a id="divDeleteList" style="display: none; background: #1A1A1A; color: #fff; margin: 10px 0;
        padding: 8px 0; text-align: center;" data="{type:'pop',id:'divDeleteGroupList'}">
    </a>
    <div class="layer3 hidden" id="divDeleteGroupList">
        <h4>
            操作提示<a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="con">
            <p>
                <i class="i-icon-warning"></i>您即将删除多个用户组，请确认您的操作：</p>
            <div id="incomegroupList">
            </div>
            <div class="btns">
                <input type="button" id="btnDeleteList" class="btn class1" value="确认删除" />
                <input type="button" class="btn class2 close" value="取消" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfdGroupList" runat="server" />
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
               <input type="checkbox" id="chkAll" /> <label for="chkAll" class="pad-l">全选</label>
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
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../../Scripts/jquery.tipTip.minified.js?20130322" type="text/javascript"></script>
<script type="text/javascript" src="../../Scripts/SystemSetting/IncomeGroupList.js?20130520"></script>
<script src="../../Scripts/SystemSetting/DistributionOEMUserList.js?20130516" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".addUser").click(function () {
            $("#lblUserGroupName").text($(this).attr("groupname"));
            $("#hfdCurrentIncomeGroupId").val($(this).attr("grpupid"));

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
        $("#btnUpdateRelation").click(function () {
            if ($(":checkbox:checked", "#dataList tr td").length == 0) {

                alert("请选择用户");
                return;
            } else {
                var str = '';
                for (var i = 0; i < $(":checkbox:checked", "#dataList tr td").length; i++) {
                    str += $(":checkbox:checked", "#dataList tr td").eq(i).attr("companyId") + ',';
                }
                str = str.substring(0, str.length - 1);
                sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/UpdateIncomeGroupRelation", JSON.stringify({ "newIncomeGroupId": $("#hfdCurrentIncomeGroupId").val(), "companyIds": str }), function () {
                    alert("添加用户成功");
                    $(".close").click();
                    window.location.href = 'IncomeGroupList.aspx';
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            }
        });
        $("#chkAll").click(function () {
            $("#chkAll").attr("checked", "checked");
            $("#dataList").find("input[type='checkbox']").attr("checked", "checked");
        });
        $("#chkOther").click(function () {
            for (var k = 0; k < $("#dataList").find("input[type='checkbox']").length; k++) {
                if ($("#dataList").find("input[type='checkbox']").eq(k).is(":checked")) {
                    $("#dataList").find("input[type='checkbox']").eq(k).removeAttr("checked");
                } else {
                    $("#dataList").find("input[type='checkbox']").eq(k).attr("checked", "checked");
                }
            }
            if ($("#dataList input[type='checkbox']:checked").length == $("#dataList").find("input[type='checkbox']").length) {
                $("#chkAll").attr("checked", "checked");
            } else {
                $("#chkAll").removeAttr("checked");
            }
        });
        $("#dataList").find("input[type='checkbox']").live("click", function () {
            if ($("#dataList input[type='checkbox']:checked").length == $("#dataList").find("input[type='checkbox']").length) {
                $("#chkAll").attr("checked", "checked");
            } else {
                $("#chkAll").removeAttr("checked");
            }
        });
        $(".delete").click(function () {
            var groupId = $(this).attr("deleteId");
            $("#hfdSingleGroupId").val(groupId);
            sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/QueryIncomeGroup", JSON.stringify({ "groupId": groupId }), function (result) {
                $("#lblGroupName").text(result.Name);
                $("#lblUserCount").text(result.UserCount);
                $("#lblGroupDescription").text(result.Description);
                $("#linkUserList").attr("href", "DistributionOEMUserList.aspx?IncomeGroupId=" + result.IncomeGroupId);
                $("#divDeleteSingle").click();
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
        });

        $("#btnDeleteSingle").click(function () {
            sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/DeleteIncomeGroup", JSON.stringify({ "groupId": $("#hfdSingleGroupId").val() }),
              function (result) {
                  alert("删除成功");
                  $(".close").click();
                  window.location.href = 'IncomeGroupList.aspx';
              },
              function (e) {
                  if (e.statusText == "timeout") {
                      alert("服务器忙");
                  } else {
                      alert(e.responseText);
                  }
              });
        });

        $("#btnSubmit").click(function () {
            var str = '';
            for (var i = 0; i < $(":checkbox:checked", "#incomeGroupList tr td").length; i++) {
                str += $(":checkbox:checked", "#incomeGroupList tr td").eq(i).attr("id") + ',';
            }
            str = str.substring(0, str.length - 1);
            if (str.length == 0) {
                alert("请先选择需要被删除的用户组");
            } else {
                $("#hfdGroupList").val(str);
                sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/QueryIncomeGroupList", JSON.stringify({ "groupIds": str }), function (result) {
                    var html = new Array();
                    for (var item in result) {
                        html.push("<table class='block-table'><tr><th>用户组名称</th><td>");
                        html.push(result[item].Name);
                        html.push("</td><th>用户数量</th><td>")
                        html.push(result[item].UserCount);
                        html.push("<a href='DistributionOEMUserList.aspx?IncomeGroupId=" + result[item].IncomeGroupId + "'>用户列表</a>");
                        html.push("</td></tr><tr><th>用户组描述</th><td colspan='3'>");
                        html.push(result[item].Description);
                        html.push("</td></tr></table>");
                    }
                    $("#incomegroupList").html(html.join(''));
                    $("#divDeleteList").click();
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            }
        });

        $("#btnDeleteList").click(function () {
            sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/DeleteIncomeGroupList", JSON.stringify({ "groupIds": $("#hfdGroupList").val() }),
              function (result) {
                  alert("删除成功");
                  $(".close").click();
                  window.location.href = 'IncomeGroupList.aspx';
              },
              function (e) {
                  if (e.statusText == "timeout") {
                      alert("服务器忙");
                  } else {
                      alert(e.responseText);
                  }
              });
        });
        $(".close").click(function () {
            $("#txtBeginTime").val("");
            $("#txtEndTime").val("");
            $("#txtUserNo").val("");
            $("#ddlIncomeGroup").val("");
            $("#txtAbbreviateName").val("");
        })
    })
</script>
