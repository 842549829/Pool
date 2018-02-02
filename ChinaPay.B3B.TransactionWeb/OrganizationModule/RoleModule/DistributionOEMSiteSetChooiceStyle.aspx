<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOEMSiteSetChooiceStyle.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.DistributionOEMSiteSetChooiceStyle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>风格管理</title>
 </head>
   <link rel="stylesheet" type="text/css" href="/Styles/oem.css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        风格管理</h3>
    <ul class="O_StyleList clearfix" id="ulHtml" runat="server">
    </ul>
    <asp:HiddenField runat="server" ID="hidValue" />
    <asp:Button ID="btnSave" Text="保存" runat="server" CssClass="btn class1" OnClick="btnSave_Click" />
    </form>
</body>
</html>
<script type="text/javascript">
    $(function ()
    {
        $("input[type='radio'][name='radChooice']").live("click", function ()
        {
            $("#hidValue").val($(this).val());
        });
    });
</script>
