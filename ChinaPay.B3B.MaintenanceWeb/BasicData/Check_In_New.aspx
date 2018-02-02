<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check_In_New.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Check_In_New" %>
<%@ Register Src="~/UserControl/Ariline.ascx" TagName="Ariline" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>值机维护</title>
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>值机维护</span>
    </div>
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
                                  <uc:Ariline runat="server" ID="ucAriline" ClientIDMode="Static" />
                                </td>
                                <th>
                                    值机链接：
                                </th>
                                <td>
                                   <asp:TextBox ID="txtHref" runat="server" CssClass="input1" Width="200"  ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>备注：</th>
                                <td colspan="3">
                                  <asp:TextBox ID="txtRemark" runat="server" CssClass="input1" TextMode="MultiLine" Width="500" Height="150" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存"  CssClass="button"
                                        OnClientClick="return btnCheckForm();" onclick="btnSave_Click" />&nbsp;&nbsp;
                                  <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='Check_In.aspx?Search=Back'"/>
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
<script src="/js/jquery.js" type="text/javascript"></script>
<script src="../Scripts/selector.js" type="text/javascript"></script>
<script src="../Scripts/airport.js" type="text/javascript"></script>
<script type="text/javascript">
    function btnCheckForm() {
        return true;
    }
</script>