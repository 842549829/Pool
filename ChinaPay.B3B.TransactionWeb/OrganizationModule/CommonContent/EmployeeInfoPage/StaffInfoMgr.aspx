<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffInfoMgr.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage.StaffInfoMgr" %>
<%@ Register TagPrefix="uc" TagName="Pager_1" Src="~/UserControl/Pager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>员工信息管理</title>

</head><link rel="stylesheet" href="/Styles/public.css?20121118" /> 
   <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .remark {
            text-align: left !important;
            word-break: break-all;
            word-wrap: break-word;
            width: 160px;
        }
        .break {
           word-break: break-all;
           word-wrap: break-word;   
        }
    </style>

<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
    <h3 class="titleBg">员工管理</h3>
    <div class="box-a">
		<div class="condition">
			<table>
				<colgroup>
					<col class="w30"/>
					<col class="w35"/>
					<col class="w35"/>
				</colgroup>
				<tbody><tr>
					<td>
						<div class="input">
							<span class="name">姓名：</span>
						    <asp:TextBox id="txtName" class="text textarea" type="text" runat="server"/>
						</div>
					</td>
					<td>
						<div class="input">
							<span class="name">用户名：</span>
						    <asp:TextBox id="txtUserName" class="text textarea" type="text" runat="server"/>
						</div>
					</td>
					<td>
						<div class="input">
							<span class="name">状态：</span>
                            <asp:DropDownList runat="server" ID="ddlEnabled">
                                <asp:ListItem Text="全部" Value="" />
                                <asp:listitem Value="1" text="启用" />
                                <asp:listitem Value="0" text="禁用" />
                            </asp:DropDownList>
						</div>
					</td>
				</tr>
				<tr>
					<td colspan="3" class="btns">
						<asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查询" OnClick="btnQuery_Click" OnClientClick="SaveSearchCondition('StaffInfo')"/>
						<input class="btn class1" type="button" value="新增" onclick="window.location.href='AddEmployee.aspx';"/>
						<input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()"/>
					</td>
				</tr>
			</tbody></table>
		</div>
	</div>
	<div id="data-list" class="table">
        <asp:Repeater runat="server" ID="datalist" OnItemCommand="ItemCommand" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
			        <thead>
				        <tr>
					        <th>姓名</th>
					        <th>性别</th>
					        <th>用户名</th>
					        <th>角色</th>
					        <th>E_Mail</th>
					        <th>手机</th>
					        <th>状态</th>
					       <%-- <th>备注</th>--%>
					        <th>操作</th>
				        </tr>
			        </thead>
			        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
		        <td><%#Eval("Name") %></td>
		        <td><%#Eval("Gender") %></td>
		        <td><%#Eval("UserName") %></td>
		        <td class="role"><%#Eval("UserRoles")%></td>
		        <td><%#Eval("Email") %></td>
		        <td><%#Eval("Cellphone") %></td>
		        <td><%#(bool)Eval("Enabled")?"启用":"禁用"%></td>
		       <%-- <td class="remark"><%#Eval("Remark")%></td>--%>
		        <td class='op'>
		           <div style='display:<%#(bool)Eval("IsAdministrator")?"none":""%>'> <asp:LinkButton Visible='<%#(string)Eval("UserName")!=CurrentUser.UserName %>'  runat="server" CommandArgument='<%#Eval("Id") %>' Text='<%#(bool)Eval("Enabled")?"禁用":"启用" %>' CommandName='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' />
                    <a href="UpdateEmployee.aspx?EmployeeId=<%#Eval("Id") %>">修改</a>
                    <a href="SetRole.aspx?EmployeeId=<%#Eval("Id") %>&UserName=<%#Eval("UserName") %>">设置角色</a>
                    <a style="cursor:pointer" onclick="return ResetStaffPWd('<%#Eval("Id") %>')">重置密码</a></div>
		        </td>
	        </tr>
            </ItemTemplate>
            <FooterTemplate>
			    </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="btns"><uc:Pager_1 runat="server" id="pager" Visible="false"></uc:Pager_1></div>
        <div class="layers" id="test_layer" style="display: none">
        <div>
            <h2>
                重置密码原因：</h2>
            <asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px" Height="60px" MaxLength="500" />
        </div>
        <div class="btns">
            <asp:HiddenField runat="server" ID="hdStaffId" />
            <asp:Button ID="btnReset" runat="server" CssClass="class1 btn" Text="重置" onclick="btnReset_Click" OnClientClick="return CheckReason()" />
            <input type="button" class="btn class2" id="btnGoBack" onclick="CancleReset()" value="返回" />
                
        </div>
    </div>
    <div class="fixed">
    </div>
    <script type="text/javascript" src="/Scripts/core/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ResetStaffPWd(companyId) {
            $("#hdStaffId").val(companyId);
            $(".fixed,.layers").show();
        }
        function CancleReset() {
            $(".fixed,.layers").hide();
        }
        function CheckReason() {
            var val = $("#Reason").val();
            if ($.trim(val) == "") {
                alert("重置密码，必须输入原因！");
                $("#Reason").select();
                return false;
            }
            return true;
        }

        $(function () {
            $(".remark").tipTip({ limitLength: 32 });
            $(".role").tipTip({ limitLength: 20 });
            pageName = 'StaffInfo';
        });
    </script>
    </form>
</body>
</html>
