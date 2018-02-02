<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .fixed{position: fixed;left: 0;top: 0; width: 100%;height: 100%;z-index: 20;background-color: Black;filter: alpha(opacity=50);-moz-opacity: 0.5;-khtml-opacity: 0.5;opacity: 0.5;display:none;}
        .layers{position: fixed; height: 135px; width: 350px;left: 35%;top: 30%;z-index: 999;background-color: #ffffff;border:10px solid #F1F1F1;padding: 0px;display:none;}
        .layers div, h2{width: 100%;}
        .hei{ background-color: #F1F1F1; }
        .qclose{ float: right; margin-right: 5px;}
        .ds{display: inline-block; height: 53px;}
    </style>
</head>
<body>
     <form id="form1" runat="server">
      <iframe id="iframe1" height="1000px" width="1000px"  src="RegisterBuy.aspx"></iframe>
      <div class="layers" id="test_layer">
            <h2 class="hei obvious">
                错误信息<a href="javascript:void(0)"  class="qclose closeBtn" >&times;</a>
            </h2>
            <div>
                <span class="ds" style="width: 32px; background: url('../../../../Images/prompt087169.gif') no-repeat  -127.7px 21px;">
                </span><span class="ds" style="vertical-align: middle; padding-left: 40px;" id="lblMessage"></span>
            </div>
            <div class="btns hei" style="height:35px;">
                <button class="btn class1 closeBtn">
                    确定</button>
            </div>
        </div>
      <div class='fixed'></div>
<%--     <script type="text/javascript">
            $(function () {
                $(".closeBtn").bind("click", close);
            });
            function close() {
                $(".fixed,.layers").css("display", "none");
                return false;
            }
    </script>--%>
    </form>
</body>
</html>
