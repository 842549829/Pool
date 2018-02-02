<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Province_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Province_new"  EnableEventValidation="false" EnableSessionState="ReadOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>省份代码维护</title>
</head>
<body>
    <div class="contents">
     <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>省份代码维护</span>
    </div>
        <form id="form1" runat="server">
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>省份代码：</th>
                                <td><asp:TextBox ID="txtProvinceCode" runat="server" CssClass="input1"></asp:TextBox></td>
                                <th>省份名称：</th>
                                <td><asp:TextBox ID="txtProvinceName" runat="server" CssClass="input2"></asp:TextBox></td>
                                <th>所属区域：</th>
                                <td><asp:DropDownList ID="ddlArea" runat="server" CssClass="input2"></asp:DropDownList></td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button" OnClientClick="return check()" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                    <input type="button" value="返回" class="button" onclick="window.location.href='Province.aspx?Search=Back'" />
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
<script type="text/javascript" language="javascript" src="../js/Check.js"></script>
<script type="text/javascript">
    function check() {
        var txtProvinceCode = $("#txtProvinceCode").val();
        if ($.trim(txtProvinceCode) == "") {
            $("#txtProvinceCode").focus();
            alert("省份代码格式不正确，不能为空！");
            return false;
        }
        var regu = /^\d{1,6}$/;
        if (!($.trim(txtProvinceCode).match(regu))) {
            alert("省份代码格式不正确,只能是1到6位数字!");
            $("#txtProvinceCode").select();
            return false;
        }
        if ($.trim($("#txtProvinceName").val()) == "") {
            $("#txtProvinceName").focus();
            alert("省份名称格式不正确，不能为空！");
            return false;
        }
        if ($.trim($("#txtProvinceName").val()).length > 20) {
            $("#txtProvinceName").select();
            alert("省份名称格式不正确，最多20个字符！");
            return false;
        }
        if ($("#ddlArea").val() == "") {
            alert("未选择所属区域");
            $("ddlArea").select();
            return false;
        }
        return true;
    }
</script>