<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BasePrice_new.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.BasePrice_new" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>基础运价维护</title>
</head>
<body>
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>»<span>基础运价维护</span>
        </div>
        <form id="Form1" runat="server" >
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                    航空公司：
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlAirline" runat="server" CssClass="input2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    出发机场：
                                </th>
                                <td>
                                    <asp:DropDownList ID="drpDepartAirport" runat="server" CssClass="input2">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    到达机场：
                                </th>
                                <td>
                                    <asp:DropDownList ID="drpArriveAirport" runat="server" CssClass="input2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    航班日期：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtHbDate" runat="server" CssClass="input2"  onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                                </td>
                                <th>
                                    出票日期：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtCpDate" runat="server" CssClass="input2" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    公布价格：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="input2" bitian="1" 
                                        showname="公布价格" MaxLength="30" ></asp:TextBox>
                                </td>
                                <th>
                                    里程数：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtMileage" runat="server" CssClass="input2" bitian="1" showname="里程数" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="4">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" 
                                        OnClientClick="return btnSubmit()" onclick="btnSave_Click" />&nbsp;&nbsp;
                                    <input type="button" class="button" onclick="javascript:window.location.href='BasePrice.aspx?Search=Back'" value="返回" />
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
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    function btnSubmit() {
        var ddlAirline = $("#ddlAirline").val();
        var fromcity = $("#drpDepartAirport").val();
        var tocity = $("#drpArriveAirport").val();
        var txtHbDate = $("#txtHbDate").val();
        var txtCpDate = $("#txtCpDate").val();
        var price = $("#txtPrice").val();
        var num = $("#txtMileage").val();

        if (fromcity == "0") {
            alert("请选择出发机场");
            $("#drpDepartAirport").focus();
            return false;
        }
        if (tocity == "0") {
            alert("请选择到达机场");
            $("#drpArriveAirport").focus();
            return false;
        }
        if ($("#drpDepartAirport").val() == $("#drpArriveAirport").val()) {
            alert("出发机场跟到达机场不能相同!");
            return false;
        }
        if (txtHbDate == "" || txtHbDate == null) {
            alert("请选择航班日期!");
            $("#txtHbDate").select();
            return false;
        }
        if (txtCpDate == "" || txtCpDate == null) {
            alert("请选择出票日期!");
            $("#txtCpDate").select();
            return false;
        }
        if (txtCpDate > txtHbDate) {
            alert("出票日期必须早于航班日期!");
            $("#txtCpDate").select();
            return false;
        }
        var regu = /^(\d{1,})(\.\d{1,2})?$/;
        if ($.trim(price) == "") {
            alert("公布价格不正确!");
            $("#txtPrice").select();
            return false;
        }
        if (!($.trim(price).match(regu))) {
            alert("公布价格格式不正确!");
            $("#txtPrice").select();
            return false;
        }
        if ($.trim(num) == "") {
            alert("里程数不能为空");
            $("#txtMileage").focus();
            return false;
        }
        if (!($.trim(num).match(regu))) {
            alert("里程数格式不正确!");
            $("#txtMileage").select();
            return false;
        }
    }
</script>