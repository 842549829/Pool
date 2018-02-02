<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlaneType_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.PlaneType_new"  EnableSessionState="ReadOnly"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>机型代码维护</title>
</head>
<body>
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>机型代码维护</span>
    </div>
    <form id="Form1" runat="server">
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    机型代码：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtPlaneTypeCode" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                                <th>
                                    机型名称：
                                </th>
                                <td>
                                     <asp:TextBox ID="txtPlaneTypeName" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                                <th>
                                    制造商：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtMake" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    民航发展基金：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtAirportPrice" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                                <th>
                                    状态：
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlStatus" CssClass="input1" runat="server">
                                    <asp:ListItem Value="-1">请选择</asp:ListItem>
                                    <asp:ListItem Value="T">启用</asp:ListItem>
                                    <asp:ListItem Value="F">禁用</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>描述信息：</th>
                                <td colspan="5">
                                    <textarea runat="server" id="ttDescription" cols="8" rows="5" class="input5"></textarea>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button"  
                                        OnClientClick="return check()" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                    <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='PlaneType.aspx?Search=Back'"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
        </form>
    </div>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript">
    function check() {
        if ($.trim($("#txtPlaneTypeCode").val()) == "") {
            alert("机型代码格式不正确，不能为空！");
            $("#txtPlaneTypeCode").focus();
            return false;
        }
        if ($.trim($("#txtPlaneTypeCode").val()).length > 5) {
            alert("机型代码格式不正确，最多5位字符！");
            $("#txtPlaneTypeCode").select();
            return false;
        }
        if ($.trim($("#txtPlaneTypeName").val()) == "") {
            alert("机型名称格式不正确，不能为空！");
            $("#txtPlaneTypeName").focus();
            return false;
        }
        if ($.trim($("#txtPlaneTypeName").val()).length > 25) {
            alert("机型名称格式不正确，最多25位字符！");
            $("#txtPlaneTypeName").select();
            return false;
        }
        if ($.trim($("#txtMake").val()) == "") {
            alert("制造商格式不正确，不能为空！");
            $("#txtMake").focus();
            return false;
        }
        if ($.trim($("#txtMake").val()).length > 30) {
            alert("制造商格式不正确，最多为30个字符！");
            $("#txtMake").select();
            return false;
        }
        if ($.trim($("#txtAirportPrice").val()) == "") {
            alert("民航发展基金格式不正确，不能为空！");
            $("#txtAirportPrice").focus();
            return false;
        }
        var regu = /^\d{1,}(\.\d{1,2})?$/;
        if (!($.trim($("#txtAirportPrice").val()).match(regu))) {
            alert("民航发展基金格式不正确!");
            $("#txtAirportPrice").select();
            return false;
        }
        if ($("#ddlStatus").val() == "-1") {
            alert("请选择状态！");
            $("#ddlStatus").focus();
            return false;
        }
        if ($.trim($("#ttDescription").val()).length > 1000) {
            alert("描述格式不正确，最多1000个字符!");
            $("#ttDescription").select();
            return false
        }
    }
</script>