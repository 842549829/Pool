<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialProductManagement_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.SystemManagement.SpecialProductManagement_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特殊产品管理维护</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>系统管理</span>&raquo;<span>特殊产品管理维护</span>
    </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>产品ID</th>
                                <td><asp:Label runat="server" ID="lblProductId"></asp:Label></td>
                            </tr>
                            <tr>
                                <th>产品名称</th>
                                <td><asp:TextBox runat="server" CssClass="input2" ID="txtProductName"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th>产品说明</th>
                                <td><asp:TextBox ID="txtProductExplain" TextMode="MultiLine" Width="400" Height="100" runat="server" CssClass="input1"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th>产品描述</th>
                                <td><asp:TextBox ID="txtProductDescribe" TextMode="MultiLine" Width="400" Height="100" runat="server" CssClass="input1"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th>产品状态</th>
                                <td>
                                    <asp:RadioButton runat="server" ID="rdoEnabled" GroupName="state"  Text="启用"/>
                                    <asp:RadioButton runat="server" ID="rdoDisabled"  GroupName="state" Text="禁用"/>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                  <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                  <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='./SpecialProductManagement.aspx'"/>
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
<script type="text/javascript">
    function $(o) {
        return document.getElementById(o);
     }
     window.onload = function () {
         $("btnSave").onclick = function () {
             var txtProductName = $("txtProductName").value, txtProductExplain = $("txtProductExplain").value, txtProductDescribe = $("txtProductDescribe").value;
             var name = /^[\u4e00-\u9fa5\w]{1,30}$/, reg = /[^<>]/;
             if (txtProductName.length < 1) {
                 alert("产品名称不能为空");
                 return false;
             }
             if (!name.test(txtProductName)) {
                 alert("产品名称格式不正确");
             }
             if (txtProductExplain.length < 1) {
                 alert("产品说明不能为空 ");
                 return false;
             }
             if (!reg.test(txtProductExplain)) {
                 alert("产品说明格式不正确");
                 return false;
             }
             if (txtProductExplain.length > 200) {
                 alert("产品说明最多200个字符");
                 return false;
             }
             if (txtProductDescribe.length < 1) {
                 alert("产品描述不能为空");
                 return false;
             }
             if (!reg.test(txtProductDescribe)) {
                 alert("产品描述格式错误");
                 return false;
             }
             if (txtProductDescribe.length > 80) {
                 alert("产品描述最多80个字符 ");
                 return false;
             }
         }
     }
</script>
