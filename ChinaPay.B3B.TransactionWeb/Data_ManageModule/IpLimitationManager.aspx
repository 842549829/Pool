<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IpLimitationManager.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.Data_ManageModule.IpLimitationManager" %>

<%@ Register TagPrefix="uc" TagName="Pager_1" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .remark
        {
            text-align: left !important;
            word-break: break-all;
            word-wrap: break-word;
            width: 160px;
        }
        .break
        {
            word-break: break-all;
            word-wrap: break-word;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        IP设置</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">姓名：</span>
                                <asp:TextBox ID="txtName" class="text textarea" type="text" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">用户名：</span>
                                <asp:TextBox ID="txtUserName" class="text textarea" type="text" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">IP：</span>
                                <asp:TextBox ID="txtIp" runat="server" CssClass="text textarea"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <input id="btnSetIpLimitation" type="button" class="btn class1" value="IP设置" />
                            <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查询" OnClick="btnQuery_Click" />
                            <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                姓名
                            </th>
                            <th>
                                性别
                            </th>
                            <th>
                                用户名
                            </th>
                            <th>
                                手机
                            </th>
                            <th>
                                状态
                            </th>
                            <th>
                                Ip限制
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("Name") %>
                    </td>
                    <td>
                        <%#Eval("Gender") %>
                    </td>
                    <td>
                        <%#Eval("UserName") %>
                    </td>
                    <td>
                        <%#Eval("Cellphone") %>
                    </td>
                    <td>
                        <%#(bool)Eval("Enabled")?"启用":"禁用"%>
                    </td>
                    <td>
                        <%#Eval("IpLimitation")%>
                    </td>
                    <td class='op'>
                        <a style="cursor: pointer" class='IpLimitation' ipLimitation='<%#Eval("IpLimitation") %>' employeeid='<%#Eval("Id") %>'>IP设置</a></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager_1 runat="server" ID="pager" Visible="false"></uc:Pager_1>
    </div>
    <a id="divMain" style="display: none;" data="{type:'pop',id:'divRemind'}"></a>
    <div class="layer3 hidden" id="divRemind">
        <h4>
            IP设置操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar" id="allEmployees">
                请注意，该操作将对您公司下的所有员工有效。</p>
            <p>
                IP限制：<input id="txtIpLimitation" type="text" class="text textarea" />
            </p>
        </div>
        <div class="txt-c mar">
            <input type="button" id="btnSubmit" class="btn class1" value="提交" />
            <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
        </div>
    </div>
    <asp:HiddenField ID="hfdOwner" runat="server" />
    <asp:HiddenField ID="hfdIsAllEmployees" runat="server" />
    <asp:HiddenField ID="hfdEmployeeId" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSetIpLimitation").click(function () {
            $("#hfdIsAllEmployees").val("true");
            $("#txtIpLimitation").val("");
            $("#allEmployees").show();
            $("#divMain").click();
        });
        $(".IpLimitation").click(function () {
            $("#hfdIsAllEmployees").val("false");
            $("#hfdEmployeeId").val($(this).attr("employeeId"));
            $("#txtIpLimitation").val($(this).attr("ipLimitation"));
            $("#allEmployees").hide();
            $("#divMain").click();
        });
        $("#btnSubmit").click(function () {
            var ipLimiation = $.trim($("#txtIpLimitation").val());
            var ipLimationPattern = /^((25[0-5]|2[0-4]\d|(1\d|[1-9])?\d)\.){3}(25[0-5]|2[0-4]\d|(1\d|[1-9])?\d)$/;
            if (ipLimiation.length > 0) {
                if (!ipLimationPattern.test(ipLimiation)) {
                    alert("IP限制格式错误");
                    $("#txtIpLimitation").select();
                    return false;
                }
            }
            if ($("#hfdIsAllEmployees").val() == "true") {
                var owner = $("#hfdOwner").val();
                sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/SetAllIpLimitation", JSON.stringify({ "companyId": owner, "ipLimitation": ipLimiation }), function (result) {
                    alert("设置成功");
                    ResetSearchOption();
                    $("#btnQuery").click();
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            } else {
                var employeeId = $("#hfdEmployeeId").val();
                sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/SetSingleIpLimitation", JSON.stringify({ "employeeId": employeeId, "ipLimitation": ipLimiation }), function (result) {
                    alert("设置成功");
                    $("#btnQuery").click();
                }, function (e) {
                    if (e.statusText == "timeout") {
                        alert("服务器忙");
                    } else {
                        alert(e.responseText);
                    }
                });
            }
        });
    })
</script>
