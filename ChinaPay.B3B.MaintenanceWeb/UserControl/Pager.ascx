<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.UserControl.Pager" %>
<asp:LinkButton ID="lbnFirst" runat="server" Text="首页" onclick="lbnFirst_Click" OnClientClick="if(typeof(saveSearchConditionWhenPagging)!='undefined')saveSearchConditionWhenPagging(1)" />
<asp:LinkButton ID="lbnPrev" runat="server" Text="上一页" onclick="lbnPrev_Click"></asp:LinkButton>
<span id="pPages" runat="server"></span>
<asp:LinkButton ID="lbnNext" runat="server" Text="下一页" onclick="lbnNext_Click"></asp:LinkButton>
<asp:LinkButton ID="lbnLast" runat="server" Text="末页" onclick="lbnLast_Click" />
共 <asp:Label runat="server" ID="lblTotalCount"></asp:Label> 条 
每页 <asp:Label runat="server" ID="lblPageSize"></asp:Label> 条 
第 <asp:Label runat="server" ID="lblCurrentPage"/> / <asp:Label runat="server" ID="lblTotalPage"/> 页