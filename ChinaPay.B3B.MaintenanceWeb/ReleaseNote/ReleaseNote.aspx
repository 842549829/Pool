<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReleaseNote.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.ReleaseNote.ReleaseNote" %>

<%@ Register Src="../UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <title>系统更新日志维护</title>
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>系统更新日志管理</span>&raquo;<span>更新日志维护</span>
        </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    标题：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input2"></asp:TextBox>
                                </td>
                                <th>
                                    更新时间：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtTime" runat="server" CssClass="input1" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    发布类型：
                                </th>
                                <td colspan="3">
                                    <asp:RadioButton runat="server" ID="radRadB3B" Text="B3B更新日志" GroupName="type" Checked="true" />
                                    <asp:RadioButton runat="server" ID="radRadPoolpay" Text="国付通更新日志" GroupName="type" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    发布范围：
                                </th>
                                <td colspan="3">
                                    <%-- <asp:DropDownList runat="server" ID="ddlType">
                                        <asp:ListItem Text="出票方可见" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="采购商可见" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="产品方可见" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="平台可见" Value="8"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:CheckBoxList runat="server" ID="ddlCompanyList" RepeatColumns="4" Width="250px">
                                    </asp:CheckBoxList>
                                    <asp:CheckBoxList runat="server" ID="ddlPoolList" RepeatColumns="4" Width="250px">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    更新内容：
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="input2" Width="80%" TextMode="MultiLine"
                                        Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存" OnClientClick="return  CheckForm();"
                                        OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                    <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='ReleaseNoteManagement.aspx'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
    </div>
    <div class="clear">
    </div>
    </form>
</body>
</html>
<script type="text/javascript" language="javascript" src="/js/jquery.js"></script>
<script src="/js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    function CheckForm() {
        var txtValue = $("#txtTitle").val();
        if ($.trim(txtValue) == "") {
            alert("标题不能为空!");
            $("#txtTitle").select();
            return false;
        }
        if ($.trim(txtValue).length > 50) {
            alert("标题过长,只能输入50个字符!");
            $("#txtTitle").select();
            return false;
        }
        if ($("#radRadB3B").is(":checked")) {
            if ($("#ddlCompanyList input[type='checkbox']:checked").length == 0) {
                alert("发布范围不能为空!");
                return false;
            }
        }
        if ($("#radRadPoolpay").is(":checked")) {
            if ($("#ddlPoolList input[type='checkbox']:checked").length == 0) {
                alert("发布范围不能为空!");
                return false;
            }
        }
        var txtRemark = $("#txtRemark").val();
        if ($.trim(txtRemark) == "") {
            alert("更新内容不能为空!");
            $("#txtRemark").select();
            return false;
        }
    }
    $(function () {
        if ($("#radRadB3B").is(":checked")) {
            $("#ddlCompanyList").show();
            $("#ddlPoolList").hide();
        } else {
            $("#ddlCompanyList").hide();
            $("#ddlPoolList").show();
        }
        $("input[type='radio'][name='type']").click(function () {
            if ($("#radRadB3B").is(":checked")) {
                $("#ddlCompanyList").show();
                $("#ddlPoolList").hide();
            } else {
                $("#ddlCompanyList").hide();
                $("#ddlPoolList").show();
            }
        });
    });
</script>
