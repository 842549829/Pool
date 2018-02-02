<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyGroupList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.CompanyGroupList" %>

<%@ Register TagPrefix="uc" TagName="Pager_1" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <title>公司组管理</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/Styles/skin.css" />
    <style type="text/css">
        .remark
        {
            width: 150px;
            text-align: left !important;
        }
    </style>
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
    <h3 class="titleBg">
        公司组管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w10" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">组别名称：</span>
                                <asp:TextBox ID="txtName" runat="server" CssClass="text" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">创建账号：</span>
                                <asp:TextBox id="txtRegisterAccount" runat="server" CssClass="text" />
                            </div>
                        </td>
                        <td colspan="2">
                            <div class="input">
                                <span class="name">创建时间：</span>
                                <asp:TextBox id="txtLower" runat="server" CssClass="text-s text" onclick="WdatePicker({isShowClear:false,readOnly:true})" />
                                -
                                <asp:TextBox ID="txtUpper" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})" /><asp:HiddenField
                                    runat="server" ID="hdDefaultDate" />
                            </div>
                        </td>
                    </tr>
                    <tr class="btns">
                        <td colspan="3">
                            <input type="button" name="new" value="添加" class="submit btn class1" onclick="location.href='Unit_Edit.aspx'" />
                            <asp:Button ID="btnQuery" class="submit btn class1" runat="server" Text="查询" onclick="btnQuery_Click"  OnClientClick="SaveSearchCondition('CompanyGroupList')"/>
                            <asp:Button ID="btnDelete" OnClientClick="return confirmDelete()" class="submit btn class1"
                                runat="server" Text="删除" onclick="btnDelete_Click" />
                            <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption()" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" onitemcommand="dataList_ItemCommand" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table>
                <colgroup>
                    <col span="2"/>
                    <col/>
                    <col span="5"/>
                </colgroup>
			<thead>
				<tr>
				    <th><input type="checkbox" id="check_All" onclick="checkAll(this)"/></th>
					<th>组别名称</th>
					<th>组别描述</th>
					<th>创建账号</th>
					<th>成员数</th>
					<th>最后修改时间</th>
					<th class="w10">操作</th>
				</tr>
			</thead><tbody>
            </headertemplate>
            <itemtemplate>
                
			<tr>
			    <td><asp:CheckBox ID="checkboxone"  runat="server"/>
                    <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id") %>'/>
                </td>
				<td><%#Eval("Name")%></td>
				<td class="remark"><%#Eval("Description")%></td>
				<td><%#Eval("Creator")%></td>
				<td><%#Eval("MemberCount")%> <a href="CompanyGroupMemberList.aspx?GroupId=<%#Eval("Id") %>">[查看]</a></td>
				<td><%#Eval("LastModifyTime")%></td>
				<td>
					<span style='display:<%#(bool)Eval("Editable")?"":"none;"%>'><a href="javascript:location.replace('Unit_Edit.aspx?Id=<%#Eval("Id") %>')">修改</a><br></span>
					<asp:LinkButton text="删除" OnClientClick="return confirm('确定要删除吗?')" Visible='<%#Eval("Editable") %>' runat="server" CommandArgument='<%#Eval("Id") %>' CommandName="Delete"/><%#(bool)Eval("Editable")?"<br />":"" %>
					<a href="QueryLog.aspx?Id=<%#Eval("Id") %>">查看日志</a>
				</td>
			</tr>
		
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager_1 runat="server" ID="pager" Visible="false"></uc:Pager_1>
    </div>
    <script type="text/javascript" src="/Scripts/core/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/widget/template.js"></script>
    <script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript" defer="defer"></script>
    <script type="text/javascript">
        function checkAll(sender) {
            if (sender.checked) {
                $("#data-list input[type='checkbox']").attr("checked", "checked");
            }
            else {
                $("#data-list input[type='checkbox']").removeAttr("checked");
            }
        }

        function checkIfAllChecked() {
            var allChecked = $("#data-list td input[type='checkbox']").not(":checked").size() == 0;
            allChecked ? $("#check_All").attr("checked", "checked") : $("#check_All").removeAttr("checked");
        }

        function confirmDelete() {
            if ($("#data-list td input:checked").size() == 0) {
                alert("未选择要删除的公司组！");
                return false;
            }
            return confirm('确定要删除吗?');
        }
        $(function () {
            $(".remark").tipTip();
            $("#data-list td input[type='checkbox']").click(checkIfAllChecked);
            pageName = 'CompanyGroupList';
        });


        SaveDefaultData();
    </script>
    </form>
</body>
</html>
