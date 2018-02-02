<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Airline_new.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Airline_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>航空公司维护</title>
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>航空公司维护</span>
    </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    代码：
                                </th>
                                <td>
                                  <asp:TextBox ID="txtErCode" runat="server"  CssClass="input1" ></asp:TextBox>
                                </td>
                                <th>
                                    名称：
                                </th>
                                <td>
                                   <asp:TextBox ID="txtAirlineName" runat="server" CssClass="input1"  ></asp:TextBox>
                                </td>
                                <th>
                                    简称：
                                </th>
                                <td>
                                  <asp:TextBox ID="txtAirlineShortName" runat="server" CssClass="input1" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    结算代码：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtJsCode" runat="server" CssClass="input1" ></asp:TextBox>
                                </td>
                                <th>
                                    状态：
                                </th>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlAirlineStatus" CssClass="input1" runat="server" >
                                        <asp:ListItem Value="-1">请选择</asp:ListItem>
                                        <asp:ListItem Value="T">启用</asp:ListItem>
                                        <asp:ListItem Value="F" >禁用</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button"
                                        OnClientClick="return btnCheckForm();" onclick="btnSave_Click" />&nbsp;&nbsp;
                                  <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='Airline.aspx?Search=Back'"/>
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
<script type="text/javascript" language="javascript" src="../js/Check.js"></script>
<script type="text/javascript">
    function btnCheckForm() {
        var txtErCode = $("#txtErCode").val();
        var txtJsCode = $("#txtJsCode").val();
        var txtAirlineName = $("#txtAirlineName").val();
        var txtAirlineShortName = $("#txtAirlineShortName").val();
        var txtEnglishName = $("#txtEnglishName").val();
        var status = $("#ddlAirlineStatus").val();
        var regu = /^([a-zA-Z]{2}||[0-9]{1}[A-Za-z]{1}||[A-Za-z]{1}[0-9]{1})$/;
        var r = /^[A-Za-z]*$/;
        var r1 = /^[0-9]{3}$/;

        if ($.trim(txtErCode) == "") {
            alert("航空公司代码不能为空!");
            $("#txtErCode").focus();
            return false;
        }
        if ((txtErCode.length != 2) || !($.trim(txtErCode).match(regu))) {
            alert("航空公司代码格式不正确,只能是2位的字母或者字母跟数字!");
            $("#txtErCode").select();
            return false;
        }
        if ($.trim(txtAirlineName) == "") {
            alert("航空公司名称不能为空!");
            $("#txtAirlineName").focus();
            return false;
        }
        if ($.trim(txtAirlineName).length >= 25) {
            alert("航空公司名称格式不正确，最多为25个字符!");
            $("#txtAirlineName").select();
            return false;
        }
        if ($.trim(txtJsCode) == "") {
            alert("结算代码不能为空!");
            $("#txtJsCode").select();
            return false;
        }
        if (!($.trim(txtJsCode).match(r1))) {
            alert("结算代码格式不正确,只能为3位的正整数");
            $("#txtJsCode").select();
            return false;
        }
        if ($.trim(txtAirlineShortName) == "") {
            alert("航空公司简称不能为空!");
            $("#txtAirlineShortName").select();
            return false;
        }
        if ($.trim(txtAirlineShortName).length >= 10) {
            alert("航空公司简称格式不正确，最多为10个字符!");
            $("#txtAirlineShortName").select();
            return false;
        }
        if (status == "-1") {
            alert("请选择航空公司状态！");
            $("#ddlAirlineStatus").focus();
            return false;
        }
    }
</script>