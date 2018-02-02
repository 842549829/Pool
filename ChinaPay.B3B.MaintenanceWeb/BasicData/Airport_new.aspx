<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Airport_new.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Airport_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>机场代码维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
     <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>机场代码维护</span>
    </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    机场代码：</th>
                                <td>
                                    <asp:TextBox ID="txtAirportCode" runat="server" CssClass="input1" ></asp:TextBox>
                                </td>
                                <th>
                                    机场简称：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtAirportShortName" runat="server" CssClass="input1" ></asp:TextBox>
                                </td>
                                <th>
                                    机场名称：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtAirportName" runat="server" CssClass="input1" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                     机场状态：
                                </th>
                                <td>
                                   <asp:DropDownList ID="ddlAirportStatus" CssClass="input1" runat="server">
                                        <asp:ListItem Value="-1">-请选择-</asp:ListItem>
                                        <asp:ListItem Value="T">启用</asp:ListItem>
                                        <asp:ListItem Value="F">禁用</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                     是否为主机场：</th>
                                <td colspan="3">
                                    <asp:RadioButton ID="rdoOK" runat="server" Checked="True" GroupName="IsMain" Text="是" />&nbsp;
                                    <asp:RadioButton ID="rdoNo" runat="server" GroupName="IsMain" Text="否" />
                                </td>
                            </tr>
                            <tr>
                                <th>所属地方：</th>
                                <td colspan="5">
                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="input1" 
                                        AutoPostBack="True" onselectedindexchanged="ddlProvince_SelectedIndexChanged">
                                        <asp:ListItem Text="-请选择-" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:DropDownList Visible="false" ID="ddlCity" runat="server" CssClass="input1"
                                        AutoPostBack="True" onselectedindexchanged="ddlCity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:DropDownList Visible="false" ID="ddlCounty" runat="server" CssClass="input1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button  ID="btnSave" runat="server" Text="保存" CssClass="button" 
                                        OnClientClick="return  btnSubmit();" onclick="btnSave_Click"/>
                                    &nbsp;&nbsp; <input type="button" class="button" onclick="javascript:window.location.href='Airport.aspx?Search=Back'" value="返回" />
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
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../js/Check.js"></script>
<script type="text/javascript">
    function btnSubmit() {
        var regu = /^[0-9]*$/;
        var txtGjCode = $("#txtGjCode").val();
        var txtAirportName = $("#txtAirportName").val();
        var txtGjCode = $("#txtGjCode").val();
        var txtAirportCode = $("#txtAirportCode").val();
        var ddlProvince = $("#ddlProvince").val();
        var ddlAirportStatus = $("#ddlAirportStatus").val();
        var txtAirportShortName = $("#txtAirportShortName").val();
        var regucity = /^[A-Za-z]{3}$/;
        var regcityspell = /^[A-Za-z]*$/;
        if ($.trim(txtAirportCode) == "") {
            alert("机场代码不能空!");
            $("#txtAirportCode").focus();
            return false;
        }
        if (!($.trim(txtAirportCode).match(regucity))) {
            alert("机场代码格式不正确，必须是长度为3位的字母!");
            $("#txtAirportCode").select();
            return false;
        }
        if ($.trim(txtAirportShortName) == "") {
            alert("机场简称不能为空!");
            $("#txtAirportShortName").focus();
            return false;
        }
        if ($.trim(txtAirportShortName).length >= 10) {
            alert("机场简称格式不正确，最多为10个字符!");
            $("#txtAirportShortName").select();
            return false;
        }
        if ($.trim(txtAirportName) == "") {
            alert("机场名称不能为空!");
            $("#txtAirportName").focus();
            return false;
        }
        if ($.trim(txtAirportName).length >= 25) {
            alert("机场名称格式不正确,最多为25位字符 !");
            $("#txtAirportName").select();
            return false;
        }
        if (ddlAirportStatus == -1) {
            alert("请选择机场状态!");
            $("#ddlAirportStatus").focus();
            return false;
        }
        if (ddlProvince <= 0) {
            alert("请选择所在地!");
            $("#ddlProvince").focus();
            return false;
        }
    }   
</script>