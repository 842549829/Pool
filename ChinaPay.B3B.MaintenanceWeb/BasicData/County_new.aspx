<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="County_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.County_new" EnableSessionState="ReadOnly"  enableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>县城代码维护</title>
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>县城代码维护</span>
    </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search">
                        <tr>
                            <th>
                                县城代码：
                            </th>
                            <td>
                                <asp:TextBox CssClass="input1" ID="txtCountyCode" runat="server"></asp:TextBox>
                            </td>
                            <th>
                                中文名称：
                            </th>
                            <td>
                                <asp:TextBox CssClass="input1" ID="txtChineseName" runat="server"></asp:TextBox>
                            </td>
                            <th>中文全拼：</th>
                            <td>
                                <asp:TextBox CssClass="input1" ID="txtSpelling" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>中文简拼：</th>
                            <td>
                                 <asp:TextBox CssClass="input1" ID="txtShortSpelling" runat="server"></asp:TextBox>
                            </td>
                            <th>
                               所属城市名称：
                            </th>
                            <td height="23">
                                <asp:DropDownList ID="ddlCityName" CssClass="input1" runat="server">
                                </asp:DropDownList>
                            </td>
                            <th>
                                热点级别：</th>
                            <td>
                                <asp:TextBox CssClass="input1" ID="txtHotLevel" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button" OnClientClick="return btnSubmit()" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                <input type="button" value="返回" class="button" onclick="window.location.href='County.aspx?Search=Back'" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
    </div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript">
    function btnSubmit() {
        var regu = /^\d{1,6}$/;
        var chinese = /[\u4E00-\u9FA5]/;
        var Spelling = /^[a-zA-z]*$/;

        var txtCountyCode = $("#txtCountyCode").val();
        if ($.trim(txtCountyCode).length == 0) {
            alert("县城代码格式不正确,不能为空");
            $("#txtCountyCode").focus();
            return false;
        }
        if (!($.trim(txtCountyCode).match(regu))) {
            alert("县城代码格式不正确,只能是1到6位数字!");
            $("#txtCountyCode").select();
            return false;
        }
        var txtChineseName = $("#txtChineseName").val();
        if ($.trim(txtChineseName).length == 0) {
            alert("中文名称格式不正确,不能为空");
            $("#txtChineseName").focus();
            return false;
        }
        if (!($.trim(txtChineseName).match(chinese))) {
            alert("中文名称格式不正确,只能为中文!");
            $("#txtChineseName").select();
            return false;
        }
        if ($.trim(txtChineseName).length > 20) {
            alert("中文名称格式不正确,最多为25位字符!");
            $("#txtChineseName").select();
            return false;
        }
        var txtSpelling = $("#txtSpelling").val();
        if (!($.trim(txtSpelling).match(Spelling))) {
            alert("中文全拼格式不正确,只能为字母!");
            $("#txtSpelling").select();
            return false;
        }
        if ($.trim(txtSpelling).length > 50) {
            alert("中文全拼格式不正确,最多50个字符!");
            $("#txtSpelling").select();
            return false;
        }
        var txtShortSpelling = $("#txtShortSpelling").val();
        if (!($.trim(txtShortSpelling).match(Spelling))) {
            alert("中文简拼格式不正确,只能为字母!");
            $("#txtShortSpelling").select();
            return false;
        }
        if ($.trim(txtShortSpelling).length > 10) {
            alert("中文简拼格式不正确，最多10个字符");
            $("#txtShortSpelling").select();
            return false;
        }
        if ($("#ddlCityName").val() == "0") {
            alert("请选择所属城市");
            $("#ddlCityName").focus();
            return false;
        }
        var txtHotLevel = $("#txtHotLevel").val();
        if (!($.trim(txtHotLevel).match(regu))) {
            alert("热点级别格式不正确,只能为数字!");
            $("#txtHotLevel").select();
            return false;
        }
        if ($.trim(txtHotLevel) >= 100 || $.trim(txtHotLevel) < 0) {
            alert("热点级别格式不正确，只能为100以内的正整数");
            $("#txtHotLevel").select();
            return false;
        }
    }
</script>