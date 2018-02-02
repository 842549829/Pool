<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Company.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.Company" %>
<asp:TextBox runat="server" ID="txtCompany" onkeyup="SelectItem(this,'-');" Height="22px"  CssClass="text fl" Width="40px"></asp:TextBox>
<asp:DropDownList runat="server" ID="ddlCompanies" CssClass="ctag"></asp:DropDownList>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
