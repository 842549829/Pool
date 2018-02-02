<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildTicketClass_Add.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.ChildTicketClass_Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>儿童可预订舱位维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>儿童票舱位维护</span>
        </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="100%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    航空公司：
                                </th>
                                <td colspan="4">
                                    <asp:DropDownList ID="dropairlinecode" runat="server" CssClass="input2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    舱&nbsp;&nbsp;位：
                                </th>
                                <td>
                                    <input type="text"  class="input2" id="iptClass" runat="server"
                                        bitian="1" showname="舱位" maxlength="25" />
                                </td>
                                <th>
                                    折&nbsp;&nbsp;扣：
                                </th>
                                <td>
                                    <input type="text" value=""  class="input" id="txtDiscount" runat="server"
                                        bitian="1" showname="折扣" maxlength="30" />
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="4">
                                    <asp:Button Text="保存" CssClass="button" ID="btnSave"  OnClientClick="return btnSubmit()"
                                        runat="server" onclick="btnSave_Click"  />&nbsp;&nbsp;
                                    <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='ChildTicketMaintain.aspx'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript">
    function btnSubmit() {
        var reg = /^[a-z,A-Z][1-9]?$/;
        var regu = /^[0-9]+(.[0-9]+)?$/;
        if ($("#dropairlinecode").val() == "0") {
            alert("请选择航空公司");
            $("#dropairlinecode").focus();
            return false;
        }
        if ($.trim($("#iptClass").val()) == "") {
            alert("舱位格式不正确，不能为空");
            $("#iptClass").focus();
            return false;
        }
        if (!($.trim($("#iptClass").val()).match(reg))) {
            alert("舱位格式不正确");
            $("#iptClass").select();
            return false;
        }
        if ($.trim($("#txtDiscount").val()) == "") {
            alert("折扣格式不正确,不能为空");
            $("#txtDiscount").focus();
            return false;
        }
        if (!($.trim($("#txtDiscount").val()).match(regu))) {
            alert("折扣格式不正确");
            $("#txtDiscount").select();
            return false;
        }
        return true;
    }
</script>