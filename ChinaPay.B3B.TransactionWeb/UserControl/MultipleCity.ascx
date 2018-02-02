<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipleCity.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.MultipleCity" %>
<style type="text/css">  .s-dbl_select { height: 280px;  } </style>
<div class="s-dbl_select op_item">
    <asp:TextBox runat="server" ID="txtCitys" CssClass="text"></asp:TextBox>
    <asp:HiddenField runat="server" ID="hidCityCode" />
    <br />
    <br />
    待选列表
    <div class="op_item">
        <asp:ListBox runat="server" ID="lbSourceCity" ShowTip="北京市" CssClass="op_con op_con_l"
            SelectionMode="Multiple"></asp:ListBox>
        <asp:ListBox runat="server" ID="lbSelectedCity" CssClass="op_con op_con_r" SelectionMode="Multiple"></asp:ListBox>
        <div class="btns">
            <input type="button" class="btn class2" value="全部添加" id="btnAddCityAll" />
            <br />
            <br />
            <input type="button" class="btn class2" value="添&nbsp;&nbsp; &nbsp;&nbsp; 加" id="btnAddCity" />
            <br />
            <br />
            <input type="button" class="btn class2" value="删&nbsp;&nbsp; &nbsp;&nbsp; 除" id="btnDelCity" />
            <br />
            <br />
            <input type="button" class="btn class2" value="全部删除" id="btnDelCityAll" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        for (var i = 0; i < $("#<%= this.lbSourceCity.ClientID %> option").length; i++) {
            $("#<%= this.lbSourceCity.ClientID %> option").eq(i).attr("text", $("#<%= this.lbSourceCity.ClientID %> option").eq(i).html());
        }
        $("#<%= this.lbSourceCity.ClientID %>").dblclick(function () {
            addCity($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
        });
        $("#<%= this.lbSelectedCity.ClientID %>").dblclick(function () {
            removeCity($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
        });
        $("#<%= this.txtCitys.ClientID %>").blur(function () {
            matchCitys($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
        });
        $("#<%= this.txtCitys.ClientID %>").keydown(function () {
            if (event.keyCode == 13) {
                matchCitys($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
            }
        });
        $("#btnAddCityAll").click(function () {
            addAllCitys($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
            return false;
        });
        $("#btnAddCity").click(function () {
            addCity($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
            return false;
        });
        $("#btnDelCity").click(function () {
            removeCity($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
            return false;
        });
        $("#btnDelCityAll").click(function () {
            removeAllCitys($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
            return false;
        });
        matchCitys($("#<%= this.lbSourceCity.ClientID %>"), $("#<%= this.lbSelectedCity.ClientID %>"), $("#<%= this.txtCitys.ClientID %>"), $("#<%= this.hidCityCode.ClientID %>"));
    });
</script>
