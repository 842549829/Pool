<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeGroupAdd.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.IncomeGroupAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        添加用户组</h3>
    <div>
        <div class="handle-box">
            <p>
                用户组名称：<asp:TextBox ID="txtIncomeGroupName" runat="server" CssClass="text" Width="300px"></asp:TextBox>请输入您自定义的用户组名称</p>
            <p>
                用户组描述：<asp:TextBox ID="txtIncomeGroupDescription" runat="server" Width="500px" CssClass="text"></asp:TextBox>
                为您创建的用户组填写备注信息
            </p>
        </div>
        <div class="btns">
            <asp:Button runat="server" ID="btnSubmitIncomeGroup" CssClass="btn class1" 
                Text="提交" onclick="btnSubmitIncomeGroup_Click" />
            <input id="btnBack" type="button" class="btn class2" runat="server" value="返回"/>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../../Scripts/jquery-1.7.2.min.js"></script>
<script type="text/javascript" src="../../Scripts/json2.js"></script>
<script type="text/javascript" src="../../Scripts/widget/common.js"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSubmitIncomeGroup").click(function () {
            if (!valiateIncomeGroup()) {
                return false;
            }
        });
    });
  function valiateIncomeGroup() {
      var name = $("#txtIncomeGroupName");
      if ($.trim(name.val()).length == 0) {
          alert("请输入用户组名称");
          name.select();
          return false;
      }
      if ($.trim(name.val()).length > 25) {
          alert("用户组名称位数不能超过25");
          name.select();
          return false;
      }
      var description = $("#txtIncomeGroupDescription");
      if ($.trim(description.val()).length > 200) {
          alert("用户组描述位数不能超过200");
          description.select();
          return false;
      }
      return true;
  }
</script>
