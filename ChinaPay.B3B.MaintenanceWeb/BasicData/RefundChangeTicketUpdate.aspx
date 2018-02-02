﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundChangeTicketUpdate.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.RefundChangeTicketUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>退改签规定维护</title>
</head>
<body>
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>»<span>退改签客规维护</span>
        </div>
        <form id="Form1" runat="server">
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    航空公司：
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlAirline" runat="server" CssClass="input5">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    航空公司电话：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtPhone" CssClass="input5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    退票规定：</th>
                                <td>
                                    <textarea runat="server" id="txtRefund" cols="8" rows="5" class="input5"></textarea>
                                </td>
                                <th>
                                    废票规定：</th>
                                <td>
                                    <textarea runat="server" id="txtScrap" cols="8" rows="5" class="input5"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    更改规定：</th>
                                <td>
                                    <textarea runat="server" id="txtChange" cols="8" rows="5" class="input5"></textarea>
                                </td>
                                <th>
                                    备注：</th>
                                <td>
                                    <textarea runat="server" id="txtRemark" cols="8" rows="5" class="input5"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    客规排序：
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txtOrderby" runat="server" CssClass="input1"></asp:TextBox>
                                    <span class="style1">数字小排前面 </span>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="4">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" 
                                        OnClientClick="return btnSubmit()" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                    <input type="button" class="button" onclick="javascript:window.location.href='RefundChangeTicket.aspx'" value="返回" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
        </form>
    </div>
    <div class="clear">
    </div>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript">
    function btnSubmit() {
        var txtPhone = $.trim($("#txtPhone").val());
        var ddlAirline = $("#ddlAirline").val();

        if (ddlAirline == "0") {
            alert("请选择航空公司!");
            $("#ddlAirline").focus();
            return false;
        }
        if (txtPhone == "") {
            alert("航空公司电话不能为空!");
            $("#txtPhone").select();
            return false;
        }
        var orderby = $("#txtOrderby").val();
        var reg = /^[0-9]{1,3}$/;
        if (orderby == "") {
            alert("客规排序不能为空！");
            $("#txtOrderby").select();
            return false;
        } else if (!$.trim(orderby).match(reg)) {
            alert("客规排序格式不正确，只能是三位数以内的数字");
            $("#txtOrderby").select();
            return false;
        }

        if ($.trim($("#txtRefund").val()).length > 2000) {
            alert("退票规定格式不正确，最多2000个字符");
            $("#txtRefund").select();
            return false;
        }
        if ($.trim($("#txtScrap").val()).length > 2000) {
            alert("废票规定格式不正确，最多2000个字符");
            $("#txtScrap").select();
            return false;
        }
        if ($.trim($("#txtChange").val()).length > 2000) {
            alert("更改规定格式不正确，最多2000个字符");
            $("#txtChange").select();
            return false;
        }
        if ($.trim($("#txtRemark").val()).length > 2000) {
            alert("备注格式不正确，最多2000个字符");
            $("#txtRemark").select();
            return false;
        }
    }
</script>
<style type="text/css">
    .style1
    {
        color: #FF0000;
    }
</style>