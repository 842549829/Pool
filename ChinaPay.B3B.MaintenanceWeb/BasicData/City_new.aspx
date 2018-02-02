<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="City_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.City_new" EnableSessionState="ReadOnly"  enableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>城市代码维护</title>
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>城市代码维护</span>
    </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>城市代码：</th>
                                <td>
                                    <asp:TextBox ID="txtCityCode" runat="server" CssClass="input1" bitian="1" showname="城市代码" ></asp:TextBox>
                                </td>
                              
                                <th>
                                   城市名称：</th>
                                <td>
                                    <asp:TextBox ID="txtCityName" runat="server" CssClass="input1"  bitian="1" showname="城市名称"></asp:TextBox>
                                </td>
                                <th>所属省份名称：</th>
                                <td>
                                    <asp:DropDownList ID="ddlProvinceName" CssClass="input1" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    中文全拼：</th>
                                <td>
                                    <asp:TextBox CssClass="input1" ID="txtSpelling" runat="server"></asp:TextBox>
                                </td>
                                <th>中文简称：</th>
                                <td>
                                    <asp:TextBox CssClass="input1" ID="txtShortSpelling" runat="server"></asp:TextBox>
                                </td>
                                <th>热点级别：</th>
                                <td>
                                    <asp:TextBox CssClass="input1" ID="txtHotLevel" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button" 
                                        OnClientClick="return btnSubmit()" onclick="btnSave_Click" />&nbsp;&nbsp;
                                   <input type="button" class="button" onclick="javascript:window.location.href='City.aspx?Search=Back'" value="返回" />
                                </td>
                            </tr>
                        </tbody>
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
        var txtCityCode = $("#txtCityCode").val();
        var txtCityName = $("#txtCityName").val();
        var regu = /^\d{1,6}$/;
        if ($.trim(txtCityCode) == "") {
            alert("城市代码不能为空!");
            $("#txtCityCode").focus();
            return false;
        }
        if (!($.trim(txtCityCode).match(regu))) {
            alert("城市代码格式不正确,只能是1到6位数字!");
            $("#txtCityCode").select();
            return false;
        }
        if ($.trim(txtCityName) == "") {
            alert("城市名称不能为空!");
            $("#txtCityName").focus();
            return false;
        }
        if ($.trim(txtCityName).length > 20) {
            alert("城市名称格式不正确,最多20个字符!");
            $("#txtCityName").select();
            return false;
        }
        if ($("#ddlProvinceName").val() == "0") {
            alert("请选择所属省份");
            $("#ddlProvinceName").focus();
            return false;
        }

        var regu = /^[0-9]*$/;
        var Spelling = /^[a-zA-z]*$/;
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