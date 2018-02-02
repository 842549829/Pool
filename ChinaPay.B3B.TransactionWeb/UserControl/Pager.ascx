<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.Pager" %>
<div class="dataPager clearfix">
<asp:LinkButton ID="lbnFirst" runat="server" Text="首页" onclick="lbnFirst_Click" />
<asp:LinkButton ID="lbnPrev" runat="server" Text="&lt; 上一页" onclick="lbnPrev_Click" ></asp:LinkButton>
<span id="pPages" clientidmode="Static" runat="server"></span>
<asp:LinkButton ID="lbnNext" runat="server" Text="下一页 &gt;" onclick="lbnNext_Click" ></asp:LinkButton>
<asp:LinkButton ID="lbnLast" runat="server" Text="末页" onclick="lbnLast_Click" />
每页显示
<asp:DropDownList runat="server" Width="50" ID="dropPageSize"  AutoPostBack="true"
        onselectedindexchanged="dropPageSize_SelectedIndexChanged">
    <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
    <asp:ListItem Value="20">20</asp:ListItem>
    <asp:ListItem Value="30">30</asp:ListItem>
    <asp:ListItem Value="50">50</asp:ListItem>
    <asp:ListItem Value="100">100</asp:ListItem>
</asp:DropDownList>
条&nbsp;&nbsp;
共 <asp:Label runat="server" ID="lblTotalCount"></asp:Label> 条 <%--
每页 <asp:Label runat="server" ID="lblPageSize"></asp:Label> 条 --%>&nbsp;&nbsp;
第 <asp:Label runat="server" ClientIDMode="Static" ID="lblCurrentPage"/> / <asp:Label runat="server" ID="lblTotalPage"/> 页</div>