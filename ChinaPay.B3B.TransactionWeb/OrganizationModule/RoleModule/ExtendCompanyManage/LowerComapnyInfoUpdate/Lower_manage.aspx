<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lower_manage.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate.LowerManage" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>下级公司管理</title>
 </head>
   <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .opt
        {
            width: 70px;
        }
        .controls a 
        {
            display:block;
            width:60px;
            float:left;
        }
        .controls
        {
           width:140px;
        }
    </style>
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <h3 class="titleBg">
        下级公司管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox id="txtAbbreviateName" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">状态：</span>
                            <asp:DropDownList runat="server" ID="ddlStatus">
                                <asp:ListItem Text="全部" Value="">
                                </asp:ListItem>
                                <asp:ListItem Text="正常" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="禁用" Value="2">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                       <div class="input up-index" style="z-index: 53;">
                            <span class="name">公司类别：</span>
                             <div class="fl">
                                    <asp:DropDownList ID="ddlRelationType" runat="server" Width="88px">
                                    </asp:DropDownList>
                                </div>
                            <div class="fl">
                                <asp:DropDownList runat="server" ID="ddlAccountType" Width="65px">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:listitem text="个人" Value="0" />
                                <asp:listitem text="企业" Value="1" />
                            </asp:DropDownList>
                            </div>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">用户名：</span>
                            <asp:TextBox id="txtUserName" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">联系人：</span>
                            <asp:TextBox id="txtContact" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td style="padding-left:30px">
                        <asp:Button class="btn class1 submit" ID="btnQuery" runat="server" Text="查询" onclick="BtnQuery_Click" OnClientClick="SaveSearchCondition('LowerCompany')">
                        </asp:Button>
                        <asp:Button CssClass="btn class1" Text="开户" runat="server" ID="btnCreateSubCompany" />
                        <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id='data-list' class="table">
        <asp:Repeater runat="server" ID="datalist" OnItemCommand="ItemCommand" OnPreRender="AddEmptyTemplate">
            <headertemplate>
            <table cellspacing="0">
            <thead>
                <tr>
                    <th>用户名</th>
                    <th>
                        公司类别
                    </th>
                    <th>
                        公司简称
                    </th>
                    <%--<th>
                        城市
                    </th>--%>
                    <th>
                        联系人
                    </th>
                    
                    <th>
                        公司组
                    </th>
                    <%--
                    <th>
                        电话
                    </th>
                    
                    <th>
                        联系人手机
                    </th><th>
                        上次登录
                        <br />时间
                    </th>
                    <th>
                        注册时间
                    </th>--%>
                    <th>
                        状态
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
                    </headertemplate>
            <itemtemplate>
                        <tr>
		                <td><a href="/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerCompanyDetailInfo.aspx?Editable=False&CompanyId=<%#Eval("CompanyId") %>&Type=<%#Eval("RelationTypeStr") %>"><%#Eval("UserNo")%></a></td>  
		                <td><%#Eval("RelationType")%>(<%#Eval("AccountType") %>)</td>
		                <td><%#Eval("AbbreviateName")%></td>
		                <%--<td><%#Eval("AirportCode")%></td>--%>
		                <td><%#Eval("Contact")%> </td>
		                <td><%#Eval("Group")%></td>
		                <%--<td><%#Eval("OfficePhones")%></td>
                        <td><%#Eval("ContactPhone")%></td>
                            <td><%#Eval("LastLoginTime")%></td>
		                <td><%#Eval("RegisterTime")%></td>--%>
		                <td><%#(bool)Eval("Enabled")?"正常":"禁用"%></td>
		                <td class="controls">
			                <a href='javascript:void(0);' class='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' companyAccount='<%#Eval("UserNo")%>' companyId='<%#Eval("CompanyId") %>' ><%#(bool)Eval("Enabled")?"禁用帐号":"启用帐号" %></a>
                           <%-- <asp:LinkButton Text='<%#(bool)Eval("Enabled")?"禁用帐号":"启用帐号" %>' CommandName='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' CommandArgument='<%#Eval("CompanyId") %>' runat="server" />--%>
	<%--		                <a href="/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerComapnyInfoUpdate.aspx?Editable=True&CompanyId=<%#Eval("CompanyId") %>&Type=<%#Eval("RelationTypeStr") %>">修改信息</a>--%>
			                <a href="javascript:ResetCompanyPWd('<%#Eval("UserNo") %>')" style="display:<%#(bool)Eval("ResetPWDEnable")?"":"none;" %>" >重置密码</a>
		</td>
	</tr>
                    </itemtemplate>
            <footertemplate>
                           </tbody>
                            </table>

                    </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="layers" id="test_layer" style="display: none; z-index: 1000">
        <div>
            <h2>
                重置密码原因：</h2>
            <asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px"
                Height="60px" />
        </div>
        <div class="btns">
            <asp:HiddenField runat="server" ID="hdCompanyAccount" />
            <asp:Button ID="btnReset" runat="server" CssClass="btn class1" Text="重置" onclick="btnReset_Click" />
            <input type="button" value="返回" class="btn class2" id="btnGoBack" onclick="CancleReset()" />
        </div>
    </div>
    <div class="fixed" style="z-index:999">
    </div>
     <a id="divMain" style="display: none;" data="{type:'pop',id:'div_Refuse'}"></a>
        <div class="layer3 hidden" id="div_Refuse">
        <h4>禁用账号操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">请在下方输入您的禁用账号理由或备注。</p>
            <textarea class="text" cols="105" rows="4" id="txtReason"></textarea>
        </div>
           <div class="btns">
              <input type="button" class="btn class1" id="btnConfirm" value="确定" />
              <input type="button" class="btn class2 close"  value="取消"/>
           </div>
        </div>
    <asp:HiddenField ID="hfdDisableCompanyId" runat="server" />
    <asp:HiddenField ID="hfdCompanyAccount" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="../../../../Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    function ResetCompanyPWd(companyId)
    {
        $("#hdCompanyAccount").val(companyId);
        $(".fixed,.layers").show();
    }
    function CancleReset()
    {
        $(".fixed,.layers").hide();
    }
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
    $(function () {
        pageName = 'LowerCompany';
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
                alert("禁用账号成功");
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
    });
</script>