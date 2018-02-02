<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateProductWorkInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.UpdateProductWorkInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
     <ul class="navType1" id="sel" runat="server">
        <li><a href="javascript:;" class="navType1Selected" id="divCompanyInfo">公司信息</a></li>
        <li><a href="javascript:" id="divpay">账户信息</a></li>
     </ul>
     <div class="divCompanyWorkInfo">
        <h3 class="titleBg">公司信息</h3>
        <div class="divCompanyInfo">
             <div class="form">
                 <table>
                     <colgroup>
                         <col class="w20" />
                         <col class="w30" />
                         <col class="w20" />
                         <col class="w30" />
                     </colgroup>
                     <tr>
                         <td class="title">
                             使用期限
                         </td>
                         <td colspan="3">
                             <asp:TextBox ID="txtBeginTime" runat="server" CssClass="datepicker datefrom btn class3 "
                                 onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>至
                             <asp:TextBox ID="txtEndTime" runat="server" CssClass="datepicker datefrom btn class3"
                                 onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <%--<td class="title">
                             特殊交易费率
                         </td>
                         <td>
                             <asp:TextBox ID="txtSpecialTicketCostRate" runat="server" CssClass="text" />‰
                         </td>--%>
                         <td class="title">
                             平台自动审核政策
                         </td>
                         <td>
                             <asp:CheckBox ID="chkAutomaticAuditPolicy" runat="server" />
                         </td>
                         <td class="title">单程控位产品</td>
                        <td>
                            <asp:CheckBox ID="chkSingleness" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtSingleness" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                     </tr>
                     <tr>
                        
                        <td class="title">散冲团产品</td>
                        <td>
                            <asp:CheckBox ID="chkDisperse" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtDisperse" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                       <%--  <td class="title">免票产品</td>
                        <td>
                            <asp:CheckBox ID="chkCostFree" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtCostFree" CssClass="text"></asp:TextBox>‰</span>
                        </td>--%>
                         <td class="title">集团票产品</td>
                         <td>
                            <asp:CheckBox ID="chkBloc" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtBloc" CssClass="text"></asp:TextBox>‰</span>
                         </td>
                     </tr>
                     <tr>
                         <td class="title">商旅卡产品</td>
                         <td >
                            <asp:CheckBox ID="chkBusiness" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtBusiness" CssClass="text"></asp:TextBox>‰</span>
                         </td>
                         <td class="title">信誉评级</td>
                        <td >
                            <asp:DropDownList runat="server" ID="dropCreditworthiness">
                                <asp:ListItem Value="0">0&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="0.5">0.5星</asp:ListItem>
                                <asp:ListItem Value="1">1&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="1.5">1.5星</asp:ListItem>
                                <asp:ListItem Value="2">2&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="2.5">2.5星</asp:ListItem>
                                <asp:ListItem Value="3">3&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="3.5">3.5星</asp:ListItem>
                                <asp:ListItem Value="4">4&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="4.5">4.5星</asp:ListItem>
                                <asp:ListItem Value="5">5&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                     </tr>
                 </table>
             </div>
             <div class="btns">
                 <asp:Button CssClass="btn class1" ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                 <input type="button" value="返回" class="btn class2" onclick="window.location.href='./CompanyList.aspx?Search=Back'" />
             </div>
         </div>
        <div class="form divpay">
                 <table>
                     <colgroup>
                         <col class="w20" />
                         <col class="w30" />
                         <col class="w20" />
                         <col class="w30" />
                     </colgroup>
                     <tr>
                         <td class="title">
                             付款账号
                         </td>
                         <td>
                             <asp:TextBox ID="txtPayment" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                             <span id="lblPayment" runat="server" class="obvious" style="display: block;"></span>
                             <input runat="server" type="button" value="修改" class="btn class1" id="btnPayment" />
                         </td>
                         <td class="title">
                             收款账号
                         </td>
                         <td>
                             <asp:TextBox ID="txtReceiving" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                             <span id="lblReceiving" runat="server" class="obvious" style="display: block;"></span>
                             &nbsp;
                             <input runat="server" type="button" value="修改" class="btn class1" id="btnReceiving" />
                         </td>
                     </tr>
                 </table>
             </div>
     </div>
    <asp:HiddenField runat="server" ID="hidId" />
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Account/UpdateAccount.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/TerraceModule/CompanyOption.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnSave").click(function () {
                if ($("#txtBeginTime").val().length < 1 || $("#txtEndTime").val().length < 1) {
                    alert("请选择工作时间");
                    return false;
                }
                var regex = /^\d{1,3}$/;
                if ($("#chkSingleness").is(":checked") && !regex.test($("#txtSingleness").val())) {
                    alert("单程控位产品费率格式错误");
                    $("#txtSingleness").focus().select();
                    return false;
                }
                if ($("#chkDisperse").is(":checked") && !regex.test($("#txtDisperse").val())) {
                    alert("散冲团产品费率格式错误");
                    $("#txtDisperse").focus().select();
                    return false;
                }
                if ($("#chkCostFree").is(":checked") && !regex.test($("#txtCostFree").val())) {
                    alert("免票产品费率格式错误");
                    $("#txtCostFree").focus().select();
                    return false;
                }
                if ($("#chkBloc").is(":checked") && !regex.test($("#txtBloc").val())) {
                    alert("集团票产品费率格式错误");
                    $("#txtBloc").focus().select();
                    return false;
                }
                if ($("#chkBusiness").is(":checked") && !regex.test($("#txtBusiness").val())) {
                    alert("商旅卡产品费率格式错误");
                    $("#txtBusiness").focus().select();
                    return false;
                }
            });
        });
    </script>
</body>
</html>
