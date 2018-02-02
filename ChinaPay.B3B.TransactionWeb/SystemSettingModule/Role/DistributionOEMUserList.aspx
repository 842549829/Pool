<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMUserList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.DistributionOEMUserList" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" defaultbutton="btnQuery" runat="server">
    <div class="hd">
        <h3 class="titleBg">
            用户管理</h3>
    </div>
    <div class="box-a">
        <div class="condition">
                 <table>
                <colgroup>
                    <col class="w25" />
                    <col class="w25" />
                    <col class="w25" />
                    <col class="w25" />
                </colgroup>
                <tr>
                <td>
                        <div class="input">
                            <span class="name">开户时间：</span>
                            <asp:TextBox ID="txtBeginTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>-<asp:TextBox
                            ID="txtEndTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                            <asp:HiddenField ID="hdDefaultDate" runat="server" />
                        </div>
                    </td>
                 <td>
                        <div class="input">
                            <span class="name">用户名：</span>
                            <asp:TextBox id="txtUserNo" class="text" type="text" runat="server" />
                        </div>
                    </td>
                  <td>
                      <div class="input">
                       <span class="name">状态：</span>
                       <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="所有" Value=""></asp:ListItem>
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="停用" Value="0"></asp:ListItem>
                       </asp:DropDownList>
                      </div>
                    </td>
                   <td>
                      <div class="input">
                      <asp:Button ID="btnQuery" class="btn class1" Text="查询" runat="server" 
                         OnClientClick="SaveSearchCondition('DistributionOEMUserList')" onclick="btnQuery_Click" />
                         <asp:Button ID="btnCreateSubCompany" CssClass="btn class2" runat="server" Text="开户" />
                         </div>
                   </td>
                </tr>
                <tr>
                   <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox id="txtAbbreviateName" class="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">联系人：</span><asp:TextBox id="txtContactName" class="text" runat="server" />
                        </div>
                    </td>
                     
                    <td>
                       <div class="input">
                          <span class="name"> 用户组：</span>
                        <asp:DropDownList ID="ddlIncomeGroup" runat="server"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                     <div class="input">
                       <input class="btn class2" type="button" value="用户组管理" onclick="javascript:window.location.href='IncomeGroupList.aspx'" />
                     </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table id="dataListTable">
                <colgroup>
                    <col class="w15" />
                    <col span="5" class="w10"/>
                    <col class="w10" />
                    <col class="w10" />
                    <col class="w15" />
                    <col class="" />
                    <col class="w5" />
                    <col class="w10" />
                </colgroup>
                <thead>
                <tr>
                    <th>开户时间</th>
                    <th>用户名</th>
                    <th>公司简称</th>
                    <th>用户组</th>
                    <th>公司类型</th>
                    <th>联系人</th>
                    <th>状态</th>
                    <th>操作</th>
                </tr>
                </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
                <td><%#Eval("RegisterTime") %></td>
                <td><%#Eval("Login")%></td>
                <td><%#Eval("AbbreviateName")%></td>
                <td><%#Eval("IncomeGroupName")%></td>
		        <td><%#Eval("Type")%> (<%#Eval("AccountType")%>)</td>
		        <td><%#Eval("ContactName")%></td>
                <td><%#(bool)Eval("Enabled") ? "正常" : "停用"%></td>
	            <td>
                   <a href="DistributionOEMUserUpdate.aspx?CompanyId=<%#Eval("CompanyId") %>&IncomeGroupId=<%#Eval("IncomeGroupId") %>&AccountType=<%#Eval("AccountTypeValue") %>">编辑</a>
                  <a href='javascript:void(0);' class='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' companyAccount='<%#Eval("Login")%>' companyId='<%#Eval("CompanyId") %>' ><%#(bool)Eval("Enabled")?"停用":"启用" %></a>
		        </td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
      <a id="divMain" style="display: none;" data="{type:'pop',id:'div_Refuse'}"></a>
        <div class="layer3 hidden" id="div_Refuse">
        <h4>停用账号操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">请在下方输入您禁用账号的理由或备注。</p>
            <textarea class="text" cols="105" rows="4" id="txtReason"></textarea>
        </div>
           <div class="btns">
              <input type="button" class="btn class1" id="btnConfirm" value="确定" />
              <input type="button" class="btn class2 close"  value="取消"/>
           </div>
        </div>
        <asp:HiddenField ID="hfdSearchCondition" runat="server" />
        <asp:HiddenField ID="hfdDisableCompanyId" runat="server" />
        <asp:HiddenField ID="hfdCompanyAccount" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/TerraceModule/CompanyList.aspx.min.js?2012122202" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".Disable").click(function () {
            $("#divMain").click();
            $("#hfdDisableCompanyId").val($(this).attr("companyId"));
            $("#hfdCompanyAccount").val($(this).attr("companyAccount"));
        });
        $("#btnConfirm").click(function () {
            if (valiateReason()) {
                var companyId = $("#hfdDisableCompanyId").val();
                var companyAccount = $("#hfdCompanyAccount").val();
                var reason = $("#txtReason").val();
                sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/Disable", JSON.stringify({ "companyId": companyId, "companyAccount": companyAccount, "reason": reason }),
            function () {
                alert("停用账号成功");
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
        $(".Enable").click(function () {
            var companyId = $(this).attr("companyId");
            sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/Enable", JSON.stringify({ "companyId": companyId }), function () {
                alert("启用账号成功");
                $("#btnQuery").click();
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
        });
    })
    function valiateReason() {
        if ($.trim($("#txtReason").val()).length == 0) {
            alert("请输入禁用账号理由或备注");
            return false;
        }
        if ($.trim($("#txtReason").val()).length > 200) {
            alert("禁用账号理由或备注字数不能超过200");
            return false;
        }
        return true;
    }
</script>
