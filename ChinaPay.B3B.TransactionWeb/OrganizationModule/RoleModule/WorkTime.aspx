<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkTime.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.WorkTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工作时间</title>
</head>
<body>
    <div class="form">
        <form id="form1" runat="server">
        <h2>工作时间</h2>
        <table>
           <tbody>
                <tr>
                    <td class="title">
                        工作日：
                    </td>
                    <td>
                        <asp:TextBox ID="txtWorkdayWorkStart" runat="server" CssClass="datepicker datefrom btn class3"  onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate: '#F{$dp.$D(\'txtWorkdayWorkEnd\')}'})"></asp:TextBox>
                        至
                        <asp:TextBox ID="txtWorkdayWorkEnd" runat="server" CssClass="datepicker datefrom btn class3"  onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate: '#F{$dp.$D(\'txtWorkdayWorkStart\')}'})"></asp:TextBox>
                    </td>
                    <td class="title">
                        周六、周日：
                    </td>
                    <td>
                        <asp:TextBox ID="txtRestdayWorkStart" runat="server" CssClass="datepicker datefrom btn class3" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtRestdayWorkEnd\')}'})"></asp:TextBox>
                        至
                        <asp:TextBox ID="txtRestdayWorkEnd" runat="server" CssClass="datepicker datefrom btn class3"  onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtRestdayWorkStart\')}'})"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
             <tbody runat="server" id="tbRefund">
                <tr>
                    <td colspan="4">
                        <h2>废票时间：</h2>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        工作日：
                    </td>
                    <td>
                        <asp:TextBox ID="txtWorkdayRefundStart" runat="server" CssClass="datepicker datefrom btn class3" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtWorkdayRefundEnd\')}'})"></asp:TextBox>
                        至
                        <asp:TextBox ID="txtWorkdayRefundEnd" runat="server" CssClass="datepicker datefrom btn class3" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtWorkdayRefundStart\')}'})"></asp:TextBox>
                    </td>
                    <td class="title">
                        周六、周日：
                    </td>
                    <td>
                        <asp:TextBox ID="txtRestdayRefundStart" runat="server" CssClass="datepicker datefrom btn class3" onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',maxDate:'#F{$dp.$D(\'txtRestdayRefundEnd\')}'})"></asp:TextBox>
                        至
                        <asp:TextBox ID="txtRestdayRefundEnd" runat="server" CssClass="datepicker datefrom btn class3"  onfocus="WdatePicker({isShowClear:false,skin:'whyGreen',dateFmt:'HH:mm',minDate:'#F{$dp.$D(\'txtRestdayRefundStart\')}'})"></asp:TextBox>
                    </td>
                </tr>
               </tbody>
                <tr class="btns">
                    <td colspan="4">
                        <asp:Button ID="btnConfirmUpdate" runat="server" CssClass="btn class1" Text="确认修改" onclick="btnConfirmUpdate_Click"/>
                        <input type="button" class="btn class2" value="取消修改" onclick="return window.location.href='/TicketDefault.aspx';" />
                    </td>
                </tr>
        </table>
        </form>
    </div>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
</body>
</html>
