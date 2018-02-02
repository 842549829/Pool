<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddExtend.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.AddExtend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>推广用户开户</title>
</head>
<body>
    <div class="success-tips box">
        <div class="flow">
            <p class="current">推广公司类型：</p>
            <ul class="box-b">
                <li>
                    <input type="radio" value="6x" name="radioone" id="cheOutTicket" checked="checked" /><label for="cheOutTicket">出票方</label>
                </li>
                <li>
                    <input type="radio" value="6x" name="radioone" id="cheProduct" /><label for="cheProduct">产品方</label>
                </li>
                <li>
                    <input type="radio" value="6x" name="radioone" id="cheBuy" /><label for="cheBuy">采购方</label>
                </li>
            </ul>
        </div>
        <input type="button" class="btn class1" value="确定" onclick="spread();" />
        <input type="button" class="btn class2" value="取消" onclick="javascript:window.location.href='./ExtendCompanyList.aspx'" /></div>
    <script type="text/javascript">
        window.history.go(1);
        function $(id) {
            return document.getElementById(id);
        }
        function spread() {
            if ($("cheOutTicket").checked) {
                window.location.href = "RegisterPage/RegisterOutTicket.aspx?type=SpreadOut";
            } else if ($("cheProduct").checked) {
                window.location.href = "RegisterPage/RegisterProduct.aspx"
            } else if ($("cheBuy").checked) {
                window.location.href = 'RegisterPage/RegisterBuy.aspx?type=SpreadBuy';
            }
        }
    </script>
</body>
</html>
