<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dictionary_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.SystemManagement.Dictionary_new"  EnableSessionState="ReadOnly"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>字典表设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs"> <span>当前位置:</span><span>系统管理</span>&raquo;<span>字典表设置</span> </div>
        <div class="title">
            <dl>
                <dt><img src="../images/icon.png"/>&nbsp;<asp:Label runat="server" ID="lblDictionaryName"></asp:Label></dt>
                <dd class="searchbk">
                     <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                        <tr>
                            <th>名称：</th>
                            <td>
                                 <asp:TextBox ID="txtName" runat="server"  CssClass="input2"></asp:TextBox>
                            </td>
                             <th>值：</th>
                            <td>
                                 <asp:TextBox ID="txtValue" runat="server"  CssClass="input2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>备注：</th>
                            <td colspan="3">
                                <textarea name="textfield" class="input_long" id="ttRemark" runat="server" cols="20" rows="5"></textarea>
                             </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存" 
                                    OnClientClick="return CheckForm();" onclick="btnSubmit_Click"/>&nbsp;&nbsp;
                                <input type="button" class="button" value="返回" name="button"onclick="javascript:window.location.href='Dictionary.aspx'"/>
                            </td>
                        </tr>
                    </tbody></table>
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
    function CheckForm() {
        var txtName = $("#txtName").val();
        var ttRemark = $("#ttRemark").val();
        var txtValue = $("#txtValue").val();

        if ($.trim(txtName) == "") {
            alert("名称不能为空!");
            $("#txtName").focus();
            return false;
        }
        if ($.trim(txtName).length > 200) {
            alert("名称过长!");
            $("#txtName").select();
            return false;
        }
        if ($.trim(txtValue) == "") {
            alert("值不能为空!");
            $("#txtValue").focus();
            return false;
        }
        if ($.trim(txtValue).length > 200) {
            alert("值过长!");
            $("#txtValue").select();
            return false;
        }
        if ($.trim(ttRemark) == "") {
            alert("备注不能为空!");
            $("#ttRemark").focus();
            return false;
        }
        if ($.trim(ttRemark).length >= 200) {
            alert("备注太长，最多200个字符!");
            $("#ttRemark").select();
            return false;
        }
    }
</script>