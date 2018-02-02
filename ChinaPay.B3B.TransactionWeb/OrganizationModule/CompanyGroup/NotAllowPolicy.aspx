<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotAllowPolicy.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.NotAllowPolicy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>不允许采购其他代理发布的政策</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <link href="../../Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body style="min-height:340px;">
    <div class="column table">
		<h2>不允许采购其他代理发布的政策</h2>
		<table>
            <thead>
                <tr>
				    <th>受限航空公司</th>
				    <th>受限出港城市</th>
				    <th>操作内容</th>
			    </tr>
             </thead>
	         <tbody id="tbCompanyGroup" runat="server">
			<tr>
				<td colspan="3" class="btns">
					<button class="btn class2" id="btnBack" runat="server">返回</button>
				</td>
			</tr>
		    </tbody>
        </table>
	</div>
    <script src="../../Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () { $(".postion_airline,.postion_departure").tipTip({maxWidth:"420px"}); });
    </script>
</body>
</html>
