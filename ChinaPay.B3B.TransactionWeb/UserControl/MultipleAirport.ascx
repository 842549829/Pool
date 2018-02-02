<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipleAirport.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.MultipleAirport" %>
<div class="s-dbl_select">
    <div runat="server" id="divInclude"><asp:RadioButton runat="server" ID="rbInclude" GroupName="include" Checked="True" Text="包含"/> &nbsp; <asp:RadioButton runat="server" ID="rbExclude" GroupName="include" Text="不包含"/></div>
    <br />
    <asp:TextBox runat="server" ID="txtAirports" CssClass="text"></asp:TextBox>
    <br />
    <div class="op_item">
        <asp:ListBox runat="server" ID="lbSource" CssClass="op_con op_con_l" SelectionMode="Multiple"></asp:ListBox>
        <asp:ListBox runat="server" ID="lbSelected" CssClass="op_con op_con_r" SelectionMode="Multiple" EnableViewState="False"></asp:ListBox>
        <div class="btns">
            <button class="btn class2" runat="server" id="btnAddAll">全部添加</button>
            <button class="btn class2" runat="server" id="btnAdd">添&nbsp;&nbsp; &nbsp;&nbsp; 加</button>
            <button class="btn class2" runat="server" id="btnRemove">删&nbsp;&nbsp; &nbsp;&nbsp; 除</button>
            <button class="btn class2" runat="server" id="btnRemoveAll">全部删除</button>
        </div>
    </div>
</div>
<script type="text/javascript" language="javascript">
    $(function () {
        $("#<%= this.lbSource.ClientID %>").live("dblclick",function () {
            addAirport($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
        });
        $("#<%= this.lbSelected.ClientID %>").live("dblclick", function () {
            removeAirport($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
        });
        $("#<%= this.txtAirports.ClientID %>").live("blur",function () {
            matchAirports($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
        });
        $("#<%= this.txtAirports.ClientID %>").live("keydown", function () {
            if (event.keyCode == 13) {
                matchAirports($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
                return false;
            } else {
                return filterAirport(event);
            }
        });
        $("#<%= this.btnAddAll.ClientID %>").live("click", function () {
            addAllAirports($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
            return false;
        });
        $("#<%= this.btnAdd.ClientID %>").live("click", function () {
            addAirport($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
            return false;
        });
        $("#<%= this.btnRemove.ClientID %>").live("click", function () {
            removeAirport($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
            return false;
        });
        $("#<%= this.btnRemoveAll.ClientID %>").live("click", function () {
            removeAllAirports($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
            return false;
        });
        matchAirports($("#<%= this.lbSource.ClientID %>"), $("#<%= this.lbSelected.ClientID %>"), $("#<%= this.txtAirports.ClientID %>"));
    });
</script>