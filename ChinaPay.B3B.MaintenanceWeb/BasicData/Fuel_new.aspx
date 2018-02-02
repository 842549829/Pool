<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fuel_new.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Fuel_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>燃油附加费维护</title>
</head>
<body>
    <div class="contents">
     <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>燃油附加费维护</span>
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
                                    <asp:DropDownList ID="ddlAirline" CssClass="input1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    里程分界：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtStartMileage" CssClass="input1" runat="server" ></asp:TextBox>
                                </td>
                                <th>
                                    生效日期：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="input1"  onfocus="WdatePicker({isShowClear:true,readOnly:true})" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    失效日期：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtStopDate" runat="server" CssClass="input1" onfocus="WdatePicker({isShowClear:true,readOnly:true})" ></asp:TextBox>
                                </td>
                                <th>
                                    成人价格：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtAdult" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                                <th>儿童价格：</th>
                                <td>
                                    <asp:TextBox ID="txtChild" runat="server" CssClass="input1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" 
                                        OnClientClick="return btnSubmit();" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                    <input type="button" class="button" onclick="javascript:window.location.href='Fuel.aspx?Search=Back'" value="返回" />
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
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    function btnSubmit() {
        var txtStartMileage = $("#txtStartMileage").val();
        var txtStartDate = $("#txtStartDate").val();
        var txtAdult = $("#txtAdult").val();
        var txtChild = $("#txtChild").val();
        var txtStopDate = $("#txtStopDate").val();
        var ddlAirline = $("#ddlAirline").val();
        var regu = /^[0-9]*$/;
        var ref = /^\d{1,}(\.\d{1,2})?$/;
        if (ddlAirline == "0") {
            alert("请选择航空公司");
            $("#ddlAirline").focus();
            return false;
        }
        if ($.trim(txtStartMileage) == "") {
            alert("里程分界格式不正确,不能为空!");
            $("#txtStartMileage").focus();
            return false;
        }
        if (!($.trim(txtStartMileage).match(regu))) {
            alert("里程分界格式不正确!");
            $("#txtStartMileage").select();
            return false;
        }
        if (txtStartDate == "") {
            alert("生效日期格式不正确!");
            $("#txtStartDate").select();
            return false;
        }
        if ($.trim(txtStopDate).length != 0) {
            if (txtStopDate < txtStartDate) {
                alert("失效日期格式不正确,失效日期必须在生效日期之前!");
                $("#txtStopDate").select();
                return false;
            }
        }
        if ($.trim(txtAdult) == "") {
            alert("成人价格格式不正确,不能为空!");
            $("#txtAdult").focus();
            return false;
        }
        if (!($.trim(txtAdult).match(ref))) {
            alert("成人价格格式不正确!");
            $("#txtAdult").select();
            return false;
        }
        if ($.trim(txtChild) == "") {
            alert("儿童价格格式不正确,不能为空!");
            $("#txtChild").focus();
            return false;
        }
        if (!($.trim(txtChild).match(ref))) {
            alert("儿童价格格式不正确!");
            $("#txtChild").select();
            return false;
        }
    }
</script>